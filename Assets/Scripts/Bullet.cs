using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    private bool divineBullet;
    private float explosionRadius;
    private float explosionDamage;
    private LayerMask enemyLayer;

    private float damage;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(float bulleDamage, bool isDivineBullet, float radius, float explosionDmg, LayerMask enemiesLayer)
    {
        damage = bulleDamage;
        divineBullet = isDivineBullet;
        explosionDamage = explosionDmg;
        enemyLayer = enemiesLayer;

        rb.linearVelocity = transform.forward * speed;

        Destroy (gameObject, 5f);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        Debug.Log("La Bala golpeo: " + other.name);

        Health enemyHealth = other.GetComponentInParent<Health>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);

            if (divineBullet)
            {
                CreateDivineExplosion();
            }
        }

        Destroy(gameObject);
    }

    void CreateDivineExplosion()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider enemy in enemies)
        {
            Health health = enemy.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(explosionDamage);
            }
        }

        Debug.Log("Explosion divina de bala");
    }
}
