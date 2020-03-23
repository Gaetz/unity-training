using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour
{
    [SerializeField] private float patrolTime = 10f;
    [SerializeField] private float aggroRange = 10f;
    [SerializeField] private Transform[] waypoints;

    private int index;
    private float speed;
    private float agentSpeed;
    private Transform player;

    private Animator anim;
    private NavMeshAgent agent;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agentSpeed = agent.speed;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        index = Random.Range(0, waypoints.Length);
        
        InvokeRepeating("Tick", 0f, 0.5f);
        if (waypoints.Length > 0)
        {
            InvokeRepeating("Patrol", 0f, patrolTime);
        }
    }
    
    private void Patrol()
    {
        index = index == waypoints.Length - 1 ? 0 : index + 1;
    }

    private void Tick()
    {
        agent.destination = waypoints[index].position;
        agent.speed = agentSpeed / 2;
        if (player == null || !(Vector3.Distance(transform.position, player.position) < aggroRange)) return;
        agent.destination = player.position;
        agent.speed = agentSpeed;
    }
}
