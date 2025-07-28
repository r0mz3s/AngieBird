using UnityEngine;

public class AngieBird : MonoBehaviour
{

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private bool hasBeenLaunched;
    private bool shouldFaceVelDirection;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        circleCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (hasBeenLaunched && shouldFaceVelDirection)
        {
            transform.right = rb.linearVelocity;
        }
    }

    public void LaunchBird(Vector2 direction, float force)
    { 
        rb.bodyType = RigidbodyType2D.Dynamic;
        circleCollider.enabled = true;

        rb.AddForce(direction * force, ForceMode2D.Impulse);
        hasBeenLaunched = true;
        shouldFaceVelDirection = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        shouldFaceVelDirection = false;
    }
}
