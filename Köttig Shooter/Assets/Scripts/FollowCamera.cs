using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class FollowCamera : MonoBehaviour
{
    [Header("Camera Movement")]

    [SerializeField][Tooltip("The object that the camera follows")] Transform target;
    [SerializeField][Tooltip("The cameras offset from the target")] Vector3 offset;
    [SerializeField][Tooltip("How fast the camera accelerates")] float smoothing;
    [SerializeField][Tooltip("How much the camera zooms out depending on the speed of the target")] float sizeScaling;
    [SerializeField][Tooltip("How fast the camera changes zoom")] float sizeSmoothing;
    [SerializeField] float lookahead;

    [Header("Camerashake")]

    [SerializeField] int shakes;
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeTime;

    Vector3 shakeOffset;

    Camera cam;

    float size;
    bool cameraShake;


    private void Awake()
    {
        cam = GetComponent<Camera>();

        size = cam.orthographicSize;
    }

    private void Update()
    {
        Vector3 targetPos = target.position + offset + shakeOffset;

        targetPos += lookahead * (Camera.main.ScreenToWorldPoint(Input.mousePosition) - target.position).normalized;

        targetPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);

        float targetSize = size + sizeScaling * target.GetComponent<Rigidbody2D>().linearVelocity.magnitude;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, sizeSmoothing * Time.deltaTime);
    }

    public void StartCameraShake()
    {
        if (cameraShake)
        {
            StopCoroutine(nameof(CameraShake));
            cameraShake = false;
        }

        StartCoroutine(nameof(CameraShake));
    }

    IEnumerator CameraShake()
    {
        cameraShake = true;

        for (int i = 0; i < shakes; i++)
        {
            float randX = Random.Range(-shakeIntensity, shakeIntensity);
            float randY = Random.Range(-shakeIntensity, shakeIntensity);

            shakeOffset = new Vector3(randX, randY, 0);

            yield return new WaitForSeconds(shakeTime / shakes);

            if (i == shakes - 1)
            {
                shakeOffset = Vector3.zero;
            }
        }

        cameraShake = false;
    }
}

