using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player; // automatisch ingesteld in Awake
    private NavMeshAgent agent;

    [Header("Settings")]
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float patrolRadius = 10f; // radius voor rondbewegen
    public float patrolWaitTime = 3f; // tijd bij patrol point
    public float chaseSpeed = 4f;
    public float patrolSpeed = 2f;
    public float returnDelay = 2f; // tijd wachten voordat terug naar patrol

    private Vector3 spawnPoint;
    private Vector3 patrolTarget;
    private bool playerDetected = false;
    private float patrolTimer = 0f;
    private float returnTimer = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = true;
        agent.angularSpeed = 720f;
        agent.acceleration = 8f;

        // Vind automatisch de speler
        GameObject p = GameObject.FindGameObjectWithTag("MainCamera");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("Player object met tag 'Player' niet gevonden!");
    }

    private void Start()
    {
        spawnPoint = transform.position;
        patrolTarget = spawnPoint;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Detecteer speler
        if (distance <= detectionRange)
        {
            playerDetected = true;
            returnTimer = 0f; // reset return timer
        }
        else if (playerDetected)
        {
            returnTimer += Time.deltaTime;
            if (returnTimer >= returnDelay)
                playerDetected = false; // pas na delay terug naar patrol
        }

        // Gedrag AI
        if (playerDetected)
            HandleChase(distance);
        else
            HandlePatrol();
    }

    private void HandleChase(float distance)
    {
        if (!agent.isOnNavMesh) return;

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            // Animaties of damage hier toevoegen
            Debug.Log("Enemy valt speler aan!");
        }
    }

    private void HandlePatrol()
    {
        if (!agent.isOnNavMesh) return;

        float distToTarget = Vector3.Distance(transform.position, patrolTarget);

        if (distToTarget < 0.5f)
        {
            // Wacht bij patrol point
            patrolTimer += Time.deltaTime;
            agent.isStopped = true;
            agent.SetDestination(transform.position);

            if (patrolTimer >= patrolWaitTime)
            {
                // Kies nieuw random patrol point
                patrolTarget = GetRandomPatrolPoint();
                agent.isStopped = false;
                agent.speed = patrolSpeed;
                agent.SetDestination(patrolTarget);
                patrolTimer = 0f;
            }
        }
        else
        {
            // Beweeg naar patrolTarget
            agent.isStopped = false;
            agent.speed = patrolSpeed;
            agent.SetDestination(patrolTarget);
        }
    }

    private Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomPos = spawnPoint + Random.insideUnitSphere * patrolRadius;
        randomPos.y = spawnPoint.y;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return spawnPoint;
        }
    }
}