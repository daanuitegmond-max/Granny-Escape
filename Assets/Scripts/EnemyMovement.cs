using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform[] doors;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        GameObject[] doorObjects = GameObject.FindGameObjectsWithTag("DoorExit");

        doors = new Transform[doorObjects.Length];

        for (int i = 0; i < doorObjects.Length; i++)
        {
            doors[i] = doorObjects[i].transform;
        }

        if (doors.Length > 0)
        {
            int randomDoor = Random.Range(0, doors.Length);
            navMeshAgent.SetDestination(doors[randomDoor].position);
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