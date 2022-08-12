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
    public ProjectileBase projectilePrefab;
    public float attackInterval = 3f;
    public Transform weaponRoot;
    public AudioClip deathSfx;
    public GameObject deathVfx;

    Collider[] m_SelfColliders;
    Damageable m_Damageable;
    float dtAttack = 0f;
    AudioSource m_AudioSource;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Transform>();
        }

        m_Damageable = GetComponent<Damageable>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        m_SelfColliders = GetComponentsInChildren<Collider>();
        m_AudioSource = GetComponent<AudioSource>();

        AiState = AIState.Follow;
    }

    void Update()
    {
        UpdateAiStateTransitions();
        UpdateCurrentAiState();
        dtAttack += Time.deltaTime;
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
        if (projectilePrefab == null || weaponRoot == null) return;

        if (dtAttack > attackInterval) {
            RangeAttack();
            dtAttack = 0f;
        }
    }

    void RangeAttack()
    {
        Vector3 lookPosition = player.position + player.up * 1f;
        Vector3 lookDirection = (lookPosition - weaponRoot.position).normalized;
        ProjectileBase newProjectile = Instantiate(projectilePrefab, weaponRoot.position, Quaternion.LookRotation(lookDirection));
        newProjectile.gameObject.tag = this.gameObject.tag;
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

    public void PlayDeathFeedBack()
    {
        if (deathVfx != null)
        {
            GameObject vfx = Instantiate(deathVfx, transform.position, transform.rotation);
            Destroy(vfx, 2f);
        }
        if (deathSfx != null && m_AudioSource != null)
        {
            m_AudioSource.PlayOneShot(deathSfx);
        }
    }
}
