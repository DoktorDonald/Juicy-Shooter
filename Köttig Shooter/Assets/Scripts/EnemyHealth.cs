using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] GameObject[] bodyParts;
    [SerializeField] GameObject deathSFX;
    [SerializeField] GameObject hitSFX;

    Rigidbody2D enemyRB;
    Animator animator;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Damage(int damage, Vector2 recoilDirection)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
            return;
        }

        Instantiate(hitSFX);

        enemyRB.linearVelocity += recoilDirection;

        animator.SetTrigger("Damage");
    }
    void Die()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            GameObject part = Instantiate(bodyParts[i], transform.position, Quaternion.identity);
            part.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 10;
        }

        FindFirstObjectByType<EnemyCounter>().RemoveEnemy();

        Instantiate(deathSFX);

        StartCoroutine(nameof(LagFrame));
    }

    IEnumerator LagFrame()
    {
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(0.05f);

        Time.timeScale = 1;

        Destroy(gameObject);
    }
}
