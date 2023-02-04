using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;
    private PlayerInteractionAnim _anim;
    private Inventory _inventory;
    private InteractionType _possibleInteraction = InteractionType.None;
    private Pickable _possiblePickable;
    private Interactive _possibleInteractive;

    private void Start()
    {
        if (Instance) Destroy(this);
        else Instance = this;

        _anim = GetComponent<PlayerInteractionAnim>();
        _inventory = Inventory.Instance;
    }

    public void SetInteraction(InteractionType interaction)
    {
        _possibleInteraction = interaction;
        InteractionHelper.Instance.Show(interaction);
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (_possibleInteraction != InteractionType.None && ctx.started)
        {
            _anim.PlayAnimation(_possibleInteraction);
            if (_possibleInteraction == InteractionType.Pickup && _possiblePickable && IsPickableNeeded())
            {
                Invoke("Pickup", 2f);
                QuestManager.Instance.Notify();
            }
            else if (_possibleInteraction != InteractionType.Pickup)
            {
                Invoke("Interact", 1f);
            }
        }
    }

    private bool IsPickableNeeded()
    {
        foreach (QuestData quest in QuestManager.Instance.questsProgress)
        {
            foreach (QuestItem item in quest.requirements)
            {
                if (_possiblePickable.item.Equals(item.item)
                    && !Inventory.Instance.HasEvery(item))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void Pickup()
    {
        _inventory.PickupQuestItem(_possiblePickable.item);
        _possiblePickable.gameObject.SetActive(false);
        SetInteraction(InteractionType.None);
    }

    private void Interact()
    {
        if (_inventory.HasEveryItem(_possibleInteractive.requiredItems))
        {
            _possibleInteractive.OnInteraction();
            if (_possibleInteractive && _possibleInteractive.onlyOnce)
            {
                DisableInteractive();
            }
        }
        else
        {
            _anim.PlayAnimation(_possibleInteractive.interactionType);
            Invoke("OnFail", 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PlayerInteractionAnim.AnimationInProgress)
        {
            if (other.transform.CompareTag("Pickable"))
            {
                SetInteraction(InteractionType.Pickup);
                _possiblePickable = other.GetComponentInChildren<Pickable>();
            }
            else if (other.transform.CompareTag("Interactive"))
            {
                Interactive interactive = other.GetComponent<Interactive>();
                if(interactive == null) return;
                //if interaction doesn't need key object or interaction key object is in inventory
                bool hasRequiredItems = _inventory.HasEveryItem(interactive.requiredItems);
                
                if (!interactive.waitForObject || hasRequiredItems)
                {
                    _possibleInteractive = interactive;
                    SetInteraction(_possibleInteractive.interactionType);
                }

            }
        }
    }

    private void OnFail()
    {
        _anim.PlayAnimation(InteractionType.FailedAction);
        SetInteraction(InteractionType.FailedAction);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!PlayerInteractionAnim.AnimationInProgress)
        {
            if (other.transform.CompareTag("Pickable") || other.transform.CompareTag("Interactive") || other.transform.CompareTag("NPC"))
            {
                StopInteractive();
            }
        }
    }

    public void StopInteractive()
    {
        SetInteraction(InteractionType.None);
        _possibleInteractive = null;
    }
    
    private void DisableInteractive()
    {
        _possibleInteractive.GetComponent<SphereCollider>().enabled = false;
        Destroy(_possibleInteractive);
        SetInteraction(InteractionType.None);
    }
}
