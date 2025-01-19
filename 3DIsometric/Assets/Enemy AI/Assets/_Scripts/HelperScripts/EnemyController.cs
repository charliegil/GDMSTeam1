using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject PlayerTarget { get; set; }
    [SerializeField] private GameObject m_playerReference;
    [SerializeField] private LayerMask m_playerLayerMask;
    [SerializeField] private float m_detectionRange = 10.0f;
    [SerializeField] private float m_attackRange = 2.0f; // Attack range
    public Vector3 LastKnownPlayerPosition { get; set; }

    private void Start()
    {
        LastKnownPlayerPosition = transform.position;
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 direction = m_playerReference.transform.position - transform.position;

        // Detect player within detection range
        if (Physics.Raycast(transform.position, direction, out hit, m_detectionRange, m_playerLayerMask))
        {
            if (hit.collider != null && hit.collider.gameObject == m_playerReference)
            {
                PlayerTarget = hit.collider.gameObject;
                LastKnownPlayerPosition = PlayerTarget.transform.position;

                // Check if the player is within attack range
                if (Vector3.Distance(transform.position, PlayerTarget.transform.position) <= m_attackRange)
                {
                    PerformAttack(); // Trigger attack logic here
                }
            }
        }
        else
        {
            PlayerTarget = null; // Reset target if out of detection range
        }
    }

    private void PerformAttack()
    {
        Debug.Log("Performing attack on player!"); // Replace with attack animation trigger
        // Example: Animator.SetTrigger("Attack");
    }

    private void OnDrawGizmos()
    {
        // Draw detection and attack ranges for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);

        if (Application.isPlaying && PlayerTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, PlayerTarget.transform.position);
        }
    }
}