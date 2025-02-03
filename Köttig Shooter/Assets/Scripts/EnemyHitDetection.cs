using UnityEngine;

public class EnemyHitDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Bullet")
        {
            other.GetComponent<Bullet>().EnemyHit();
        }
    }
}
