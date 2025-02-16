using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;

    int health;

    Rigidbody2D enemyRB;

    private void Start()
    {
        health = maxHealth;

        enemyRB = GetComponent<Rigidbody2D>();
    }

    public void Damage(int damage, Vector2 pos)
    {
        health -= damage;

        Vector2 recoil = ((Vector2)transform.position - pos).normalized;

        enemyRB.linearVelocity += recoil * 20 + Vector2.up * 10;

        if (health < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
