using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float idleDistance = 10f;
    public float attackDistance = 1.2f;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private Animator anim;

    private float lastAttackTime;
    private float lastDamageTime;

    public int damage = 15;
    public float damageCooldown = 1f;

    // 🔊 AUDIO
    public AudioClip attackSound;
    public AudioClip nearSound;

    private AudioSource audioSource;
    private float soundCooldown = 3f;
    private float lastSoundTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        agent.speed = 4f;
        agent.acceleration = 10f;
        agent.angularSpeed = 300f;
        agent.stoppingDistance = 0.8f;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = GetFlatDistance();

        //  NEAR SOUND 
        if (distance < 5f && Time.time > lastSoundTime + soundCooldown)
        {
            if (nearSound != null)
                audioSource.PlayOneShot(nearSound);

            lastSoundTime = Time.time;
        }

        // IDLE
        if (distance > idleDistance)
        {
            agent.isStopped = true;
            agent.ResetPath();
            anim.SetFloat("Speed", 0);
            return;
        }

        // CHASE
        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            anim.SetFloat("Speed", agent.velocity.magnitude);
            RotateTowardsPlayer();
            return;
        }

        // ATTACK
        agent.isStopped = true;
        agent.ResetPath();

        anim.SetFloat("Speed", 0);
        RotateTowardsPlayer();
        // check if enough time has passed since last attack
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("Attack");
            lastAttackTime = Time.time;  // remember when we attacked (for cooldown)

            if (attackSound != null)
                audioSource.PlayOneShot(attackSound);
        }
    }

    float GetFlatDistance()
    {
        Vector3 a = transform.position;
        Vector3 b = player.position;

        a.y = 0;
        b.y = 0;

        return Vector3.Distance(a, b);
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * 10f
            );
        }
    }

    // Called by animation event
    public void DealDamage()
    {
        if (player == null) return;

        
        if (Time.time < lastDamageTime + damageCooldown) return; //prevents attack spam

        float distance = GetFlatDistance();
        float height = Mathf.Abs(player.position.y - transform.position.y);

       
        if (distance > attackDistance * 0.8f) return;
        if (height > 1.5f) return;

        // Line of sight
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 dir = (player.position - origin).normalized;

        if (Physics.Raycast(origin, dir, out hit, attackDistance))
        {
            if (!hit.collider.CompareTag("Player"))
                return;
        }

        PlayerHealth ph = player.GetComponent<PlayerHealth>();

        if (ph != null)
        {
            ph.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}