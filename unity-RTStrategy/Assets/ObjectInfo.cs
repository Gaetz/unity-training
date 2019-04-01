using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectInfo : MonoBehaviour
{
    public bool IsSelected;
    public NodeManager.ResourceType heldResourceType;

    public string ObjectName;
    public bool IsGathering;
    [SerializeField] int maxHeldResource;

    NavMeshAgent agent;
    public int heldResource;
    GameObject[] resourceDrops;


    void Start()
    {
        IsSelected = false;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(GatherTick());
    }

    void Update()
    {
        if (heldResource >= maxHeldResource)
        {
            /* resourceDrops = GameObject.FindGameObjectsWithTag("Drops");
            agent.destination = GetClosestDropOff(resourceDrops).transform.position;
            resourceDrops = null;
            */
        }

        if (Input.GetMouseButtonDown(1) && IsSelected)
        {
            RightClickCommand();
        }
    }

    GameObject GetClosestDropOff(GameObject[] drops)
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject drop in drops)
        {
            Vector3 direction = drop.transform.position - transform.position;
            float distance = direction.sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = drop;
            }
        }
        return closest;
    }

    void RightClickCommand()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            // Move
            if (hit.collider.tag == "Ground")
            {
                agent.destination = hit.point;
                Debug.Log("Moving");
            }
            // Harvest
            else if (hit.collider.tag == "Resource")
            {
                agent.destination = hit.collider.gameObject.transform.position;
                Debug.Log("Harvesting");
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;
        if (hitObject.tag == "Resource")
        {
            hitObject.GetComponent<NodeManager>().Gatherers++;
            IsGathering = true;
            heldResourceType = hitObject.GetComponent<NodeManager>().NodeResourceType;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject hitObject = other.gameObject;
        if (hitObject.tag == "Resource")
        {
            IsGathering = true;
            hitObject.GetComponent<NodeManager>().Gatherers--;
        }
    }

    IEnumerator GatherTick() {
        while(true) {
            yield return new WaitForSeconds(1);
            if(IsGathering && heldResource < maxHeldResource) {
                heldResource++;
            }
        }
    }
}
