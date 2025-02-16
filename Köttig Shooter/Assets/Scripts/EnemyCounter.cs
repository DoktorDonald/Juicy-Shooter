using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] int enemies;

    public void AddEnemies(int number)
    {
        enemies += number;
    }

    public void RemoveEnemy()
    {
        enemies--;

        if (enemies == 0)
        {
            EnemySpawner[] spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.InstanceID);

            foreach (EnemySpawner spawner in spawners)
            {
                spawner.Spawnwave();
            }
        }
    }
}
