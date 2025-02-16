using UnityEngine;

public class PitchRandomizer : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.0f);
    }
}
