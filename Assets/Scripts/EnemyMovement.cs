using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player != null && navMeshAgent != null && navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = enabled;
        }
    }
}