using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Task
{
  Idle,
  Gathering,
  Delivering,
  Moving
}

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
  Task task;


  void Start()
  {
    IsSelected = false;
    agent = GetComponent<NavMeshAgent>();
    StartCoroutine(GatherTick());
    task = Task.Idle;
  }

  void Update()
  {
    if (heldResource >= maxHeldResource)
    {
      resourceDrops = GameObject.FindGameObjectsWithTag("Drop");
      agent.destination = GetClosestDropOff(resourceDrops).transform.position;
      resourceDrops = null;
      task = Task.Delivering;
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
        task = Task.Moving;
      }
      // Harvest
      else if (hit.collider.tag == "Resource")
      {
        agent.destination = hit.collider.gameObject.transform.position;
        Debug.Log("Harvesting");
        task = Task.Gathering;
      }
    }
  }

  public void OnTriggerEnter(Collider other)
  {
    GameObject hitObject = other.gameObject;
    if (hitObject.tag == "Resource" && task == Task.Gathering)
    {
      hitObject.GetComponent<NodeManager>().Gatherers++;
      IsGathering = true;
      heldResourceType = hitObject.GetComponent<NodeManager>().NodeResourceType;
    }
    else if (hitObject.tag == "Drop" && task == Task.Delivering)
    {
      GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>().AddStone(heldResource);
      heldResource = 0;
      task = Task.Gathering;
      // Get back to work !
    }
  }

  public void OnTriggerExit(Collider other)
  {
    GameObject hitObject = other.gameObject;
    if (hitObject.tag == "Resource")
    {
      IsGathering = false;
      hitObject.GetComponent<NodeManager>().Gatherers--;
    }
  }

  IEnumerator GatherTick()
  {
    while (true)
    {
      yield return new WaitForSeconds(1);
      if (IsGathering && heldResource < maxHeldResource)
      {
        heldResource++;
      }
    }
  }
}
