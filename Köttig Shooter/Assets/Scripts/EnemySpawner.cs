using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] float spawnTime;

    float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;


        if (spawnTimer >= spawnTime)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);

            spawnTimer = 0;
        }
    }
}
