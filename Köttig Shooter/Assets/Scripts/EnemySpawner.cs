using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int startingWaveCount;
    [SerializeField] float waveIncreaseFactor;

    [SerializeField] int nextWaveCount;

    private void Start()
    {
        nextWaveCount = startingWaveCount;

        Spawnwave();
    }

    public void Spawnwave()
    {
       for (int i = 0; i < nextWaveCount; i++)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(-2, 2), 0, 0);

            Instantiate(enemy, transform.position + SpawnPos, Quaternion.identity);
        }

       FindFirstObjectByType<EnemyCounter>().AddEnemies(nextWaveCount);

        nextWaveCount = Mathf.RoundToInt(nextWaveCount * waveIncreaseFactor);
    }
}
