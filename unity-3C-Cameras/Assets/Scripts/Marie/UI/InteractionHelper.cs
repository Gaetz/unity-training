using TMPro;
using UnityEngine;

public class InteractionHelper : MonoBehaviour
{
    public static InteractionHelper Instance;
    
    public GameObject interactionCue;
    public string interactionText, pickupText, failedText, talkText;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(InteractionType interaction = InteractionType.None)
    {
        if (interaction != InteractionType.None)
        {
            interactionCue.SetActive(true);
            if (interaction == InteractionType.Pickup)
            {
                interactionCue.GetComponentInChildren<TextMeshProUGUI>().text = string.Format(pickupText);
            }else if (interaction == InteractionType.Talk)
            {
                interactionCue.GetComponentInChildren<TextMeshProUGUI>().text = talkText;
            }
            else if (interaction == InteractionType.FailedAction)
            {
                interactionCue.GetComponentInChildren<TextMeshProUGUI>().text = failedText;
            }else{
                interactionCue.GetComponentInChildren<TextMeshProUGUI>().text = interactionText;
            }
        }
        else
        {
            interactionCue.SetActive(false);
        }
    }
}
