using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DuckAI : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Patrol Settings")]
    public float patrolRadius = 5f;
    public float minPatrolWaitTime = 1f;
    public float maxPatrolWaitTime = 5f;
    public float minPatrolSpeed = 1.5f;
    public float maxPatrolSpeed = 3f;

    [Header("Player Detection")]
    public float detectRadius = 3f;
    private Transform player;

    [Header("Soft Boundary")]
    public float boundaryPushStrength = 2f; // snelheid terug naar binnen

    private Vector3 patrolCenter;
    private Vector3 patrolTarget;
    private float patrolTimer = 0f;
    private float currentPatrolWaitTime;
    private float currentPatrolSpeed;

    private enum State { Patrol, Chasing }
    private State currentState = State.Patrol;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;  // we draaien handmatig
        agent.angularSpeed = 720f;
        agent.acceleration = 8f;
    }

    public void SetPatrolCenterAndRadius(Vector3 center, float radius)
    {
        patrolCenter = center;
        patrolRadius = radius;
        patrolTarget = GetRandomPatrolPoint();
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void Start()
    {
        currentPatrolSpeed = Random.Range(minPatrolSpeed, maxPatrolSpeed);
        currentPatrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        agent.speed = currentPatrolSpeed;
        agent.SetDestination(patrolTarget);
    }

    private void Update()
    {
        if (!agent.isOnNavMesh || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        currentState = distanceToPlayer <= detectRadius ? State.Chasing : State.Patrol;

        if (currentState == State.Chasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        RotateTowardsMovement();
        ApplySoftBoundary();
    }

    private void Patrol()
    {
        float distToTarget = Vector3.Distance(transform.position, patrolTarget);

        if (distToTarget < 0.5f)
        {
            patrolTimer += Time.deltaTime;
            agent.isStopped = true;

            if (patrolTimer >= currentPatrolWaitTime)
            {
                patrolTarget = GetRandomPatrolPoint();

                currentPatrolSpeed = Random.Range(minPatrolSpeed, maxPatrolSpeed);
                currentPatrolWaitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);

                agent.speed = currentPatrolSpeed;
                agent.isStopped = false;
                agent.SetDestination(patrolTarget);

                patrolTimer = 0f;
            }
        }
    }

    private void ChasePlayer()
    {
        Vector3 dir = player.position - patrolCenter;
        dir = Vector3.ClampMagnitude(dir, patrolRadius);
        Vector3 targetPos = patrolCenter + dir;

        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.speed = maxPatrolSpeed;
            agent.SetDestination(hit.position);
        }
    }

    private void RotateTowardsMovement()
    {
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            Vector3 lookDir = agent.velocity.normalized;
            lookDir.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
        }
    }

    private void ApplySoftBoundary()
    {
        // Bereken afstand tot center
        Vector3 fromCenter = transform.position - patrolCenter;
        float distance = fromCenter.magnitude;

        if (distance > patrolRadius)
        {
            // Bereken richting terug naar center
            Vector3 pushDir = -fromCenter.normalized;

            // Pas agent velocity tijdelijk aan om terug te duwen
            agent.velocity += pushDir * boundaryPushStrength * Time.deltaTime;
        }
    }

    private Vector3 GetRandomPatrolPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 randCircle = Random.insideUnitCircle * patrolRadius;
            Vector3 randPos = patrolCenter + new Vector3(randCircle.x, 0f, randCircle.y);

            if (NavMesh.SamplePosition(randPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return patrolCenter;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
#endif
}
