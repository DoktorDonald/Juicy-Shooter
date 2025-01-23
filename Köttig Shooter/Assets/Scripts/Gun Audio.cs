using UnityEngine;

public class GunAudio : MonoBehaviour
{
    [Header("Fire Sounds")]
    [SerializeField] AudioClip[] fire;
    [SerializeField] float fireVolume;

    [Header("Mech Sounds")]
    [SerializeField] AudioClip[] mech;
    [SerializeField] float mechVolume;

    [Header("Casing Sounds")]
    [SerializeField] AudioClip[] casing;
    [SerializeField] float casingVolume;

    [Header("Bass")]
    [SerializeField] AudioClip bass;
    [SerializeField] float bassVolume;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playAudio()
    {
        int rand1 = Random.Range(0, fire.Length);
        int rand2 = Random.Range(0, mech.Length);
        int rand3 = Random.Range(0, casing.Length);

        audioSource.PlayOneShot(fire[rand1], fireVolume);
        audioSource.PlayOneShot(mech[rand2], mechVolume);
        audioSource.PlayOneShot(casing[rand3], casingVolume);
        audioSource.PlayOneShot(bass, bassVolume);
    }
}
