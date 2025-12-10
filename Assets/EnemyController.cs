using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float moveForce = 20f;       // how strong it pushes itself
    public float maxSpeed = 8f;         // clamp speed
    public float detectionRange = 20f;  // when it starts chasing
    public float bounceForce = 10f;     // bounce off walls

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < detectionRange)
        {
            ChasePlayer();
        }

        LimitSpeed();
    }

    void ChasePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        rb.AddForce(dir * moveForce, ForceMode.Acceleration);
    }

    void LimitSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}
