using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float time;

    public void StartTimer(float time)
    {
        this.time = time;
        StartCoroutine(nameof(Timer));
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
