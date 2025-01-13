using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject explosion;

    float time;

    public void StartTimer(float time)
    {
        this.time = time;
        StartCoroutine(nameof(Timer));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsTouching("Ground"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            DestroySelf();
        }

        if (IsTouching("Casing"))
        {
            Invoke(nameof(Explosion), 0.05f);
            Invoke(nameof(DestroySelf), 0.05f);
        }
    }

    bool IsTouching(string layerMask)
    {
        return gameObject.GetComponent<CircleCollider2D>().IsTouchingLayers(LayerMask.GetMask(layerMask));
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void Explosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
    
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
