using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] GameObject[] bodyParts;

    Rigidbody2D enemyRB;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    public void Damage(int damage, Vector2 recoilDirection)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
            return;
        }

        enemyRB.linearVelocity += recoilDirection;
    }
    void Die()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            GameObject part = Instantiate(bodyParts[i], transform.position, Quaternion.identity);
            part.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 10;
        }

        Destroy(gameObject);
    }
}
