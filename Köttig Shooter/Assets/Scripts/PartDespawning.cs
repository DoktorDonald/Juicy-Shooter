using UnityEngine;

public class PartDespawning : MonoBehaviour
{
    [SerializeField] float despawnTime = 30;

    private void Update()
    {
        despawnTime -= Time.deltaTime;

        if (despawnTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
