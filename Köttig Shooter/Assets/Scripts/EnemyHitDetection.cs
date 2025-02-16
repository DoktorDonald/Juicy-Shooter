using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Bullet")
        {
            transform.root.GetComponent<EnemyHealth>().Damage(1, other.GetComponent<Rigidbody2D>().linearVelocity.normalized * 10);

            other.GetComponent<Bullet>().EnemyHit();
        }
    }
}
