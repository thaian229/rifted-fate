using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum AIState
    {
        Follow,
        Attack,
    }

    public GameObject LootPrefab;
    public float DropRate = 1f;
    public NavMeshAgent NavMeshAgent { get; private set; }
    public AIState AiState { get; private set; }
    public Transform player;
    public float MeleeDamage = 5f;
    public float AttackRange = 10f;
    public float OrientationSpeed = 10f;

    Collider[] m_SelfColliders;
    Damageable m_Damageable;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Transform>();
        }

        m_Damageable = GetComponent<Damageable>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        m_SelfColliders = GetComponentsInChildren<Collider>();

        AiState = AIState.Follow;
    }

    void Update()
    {
        UpdateAiStateTransitions();
        UpdateCurrentAiState();
    }

    void UpdateAiStateTransitions()
    {
        // Handle transitions 
        switch (AiState)
        {
            case AIState.Follow:
                // Transition to attack when there is a line of sight to the target
                if (Vector3.Distance(player.position, this.transform.position) < this.AttackRange)
                {
                    AiState = AIState.Attack;
                }
                break;
            case AIState.Attack:
                // Transition to follow when no longer a target in attack range
                if (Vector3.Distance(player.position, this.transform.position) >= this.AttackRange)
                {
                    AiState = AIState.Follow;
                }
                break;
        }
    }

    void UpdateCurrentAiState()
    {
        if (!player) return;
        // Handle logic 
        switch (AiState)
        {
            case AIState.Follow:
                NavMeshAgent.SetDestination(player.position);
                break;
            case AIState.Attack:
                if (Vector3.Distance(player.position, this.transform.position) >= this.AttackRange)
                {
                    NavMeshAgent.SetDestination(player.position);
                }
                else
                {
                    NavMeshAgent.SetDestination(this.transform.position);
                    TryAttack();
                }
                break;
        }
        Vector3 lookPosition = player.position;
        Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, Vector3.up).normalized;
        if (lookDirection.sqrMagnitude != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * OrientationSpeed);
        }
    }

    void TryAttack()
    {

    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.TakeDamage(MeleeDamage);
            }
        }
    }
}
