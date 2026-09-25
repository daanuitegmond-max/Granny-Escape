using UnityEngine;
using System.Collections;

public class PickupClass : MonoBehaviour
{
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float ThrowingForce = 15f;
    [SerializeField] private float PickupRange = 5f;
    [SerializeField] private Transform Hand;

    private Rigidbody currentRigidbody;
    private Collider[] currentColliders;
    private EnemyMovement currentEnemy;

    private void Update()
    {
        // Pick up / drop
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupOrDrop();
        }

        // Throw
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowObject();
        }

        // Hold object in hand
        if (currentRigidbody != null)
        {
            currentRigidbody.position = Hand.position;
            currentRigidbody.rotation = Hand.rotation;
        }
    }

    private void PickupOrDrop()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            PickupRange,
            PickupLayer,
            QueryTriggerInteraction.Ignore))
        {
            Rigidbody rb = hit.collider.GetComponentInParent<Rigidbody>();

            if (rb == null)
            {
                Debug.Log("Can't pickup: No Rigidbody.");
                return;
            }

            // Drop currently held object
            if (currentRigidbody != null)
            {
                DropObject();
            }

            currentRigidbody = rb;

            // Get all colliders
            currentColliders =
                currentRigidbody.GetComponentsInChildren<Collider>();

            // Find enemy
            currentEnemy =
                currentRigidbody.GetComponentInParent<EnemyMovement>();

            // Disable enemy AI
            if (currentEnemy != null)
            {
                currentEnemy.SetMovementEnabled(false);
            }

            // Disable physics
            currentRigidbody.isKinematic = true;
            currentRigidbody.useGravity = false;

            // Disable colliders
            foreach (Collider col in currentColliders)
            {
                col.enabled = false;
            }

            Debug.Log("Picked up " + currentRigidbody.gameObject.name);
        }
        else
        {
            if (currentRigidbody != null)
            {
                DropObject();
            }
        }
    }

    private void DropObject()
    {
        if (currentRigidbody == null)
            return;

        // Enable colliders
        foreach (Collider col in currentColliders)
        {
            col.enabled = true;
        }

        // Enable physics
        currentRigidbody.isKinematic = false;
        currentRigidbody.useGravity = true;

        // Enable enemy AI
        if (currentEnemy != null)
        {
            currentEnemy.SetMovementEnabled(true);
        }

        currentRigidbody = null;
        currentColliders = null;
        currentEnemy = null;
    }

    private void ThrowObject()
    {
        if (currentRigidbody == null)
            return;

        Debug.Log("THROWING " + currentRigidbody.gameObject.name);

        // Save references before clearing them
        Rigidbody rb = currentRigidbody;
        Collider[] colliders = currentColliders;
        EnemyMovement enemy = currentEnemy;

        // Stop holding the object
        currentRigidbody = null;
        currentColliders = null;
        currentEnemy = null;

        // Disable enemy AI while throwing
        if (enemy != null)
        {
            enemy.SetMovementEnabled(false);
        }

        // Enable colliders
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        // Enable physics
        rb.isKinematic = false;
        rb.useGravity = true;

        // Make sure there isn't old velocity
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Throw forward
        rb.AddForce(
            playerCamera.transform.forward * ThrowingForce,
            ForceMode.Impulse
        );

        // Let the enemy get thrown before AI takes over again
        if (enemy != null)
        {
            StartCoroutine(EnableEnemyAfterThrow(enemy, 1f));
        }
    }

    private IEnumerator EnableEnemyAfterThrow(
        EnemyMovement enemy,
        float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy != null)
        {
            enemy.SetMovementEnabled(true);
        }
    }
}