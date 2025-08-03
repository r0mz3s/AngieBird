using UnityEngine;

public class Baddie : MonoBehaviour
{
    [SerializeField] private float maxHealth = 8f;
    [SerializeField] private float damageThreshold = 1f;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void DamageBaddie(float damageAmount)
    { 
        currentHealth -= damageAmount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.RemoveBaddie(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;
        if (impactVelocity > damageThreshold)
        {
            DamageBaddie(impactVelocity);
        }
    }
}
