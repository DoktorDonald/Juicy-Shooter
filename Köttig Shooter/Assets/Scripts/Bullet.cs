using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hitParticles;
    [SerializeField] GameObject enemyHitParticles;
    [SerializeField] GameObject explosion;

    [SerializeField] ExplosionType exposionType;

    Rigidbody2D bulletRB;

    enum ExplosionType {explosive, random, inert}

    float time;

    private void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(bulletRB.linearVelocityY, bulletRB.linearVelocityX) * Mathf.Rad2Deg);
    }

    public void StartTimer(float time)
    {
        this.time = time;
        StartCoroutine(nameof(Timer));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsTouching("Ground"))
        {
            Instantiate(hitParticles, transform.position, Quaternion.identity);
            if (exposionType == ExplosionType.explosive) { Explode(); }
            if (exposionType == ExplosionType.random)
            {
                int rand = Random.Range(0, 2);

                if (rand == 0)
                {
                    Explode();
                }
            }
            DestroySelf();
        }

        if (IsTouching("Casing"))
        {
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

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    public void EnemyHit()
    {
        if (exposionType == ExplosionType.explosive) { Explode(); }
        if (exposionType == ExplosionType.random)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                Explode();
            }
        }

        Instantiate(enemyHitParticles, transform.position, Quaternion.identity);

        DestroySelf();
    }
    
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
