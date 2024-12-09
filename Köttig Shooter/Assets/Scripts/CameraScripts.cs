using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField][Tooltip("The object that the camera follows")] Transform target;
    [SerializeField][Tooltip("The cameras offset from the target")] Vector3 offset;
    [SerializeField][Tooltip("How far infort of the target the camera moves when the target is moving")] float lead;
    [SerializeField][Tooltip("1")] float maxLeadVelocity = 1;
    [SerializeField][Tooltip("How fast the camera accelerates")] float smoothing;
    [SerializeField][Tooltip("How much the camera zooms out depending on the spee of the target")] float sizeScaling;
    [SerializeField][Tooltip("How fast the camera changes zoom")] float sizeSmoothing;

    Camera cam;

    Vector3 velocity1;
    float velocity2;

    float size;


    private void Awake()
    {
        cam = GetComponent<Camera>();

        size = cam.orthographicSize;
    }

    private void Update()
    {
        Vector3 targetPos = target.position + offset + Vector3.right * lead * Mathf.Clamp(target.GetComponent<Rigidbody2D>().linearVelocityX / maxLeadVelocity, -1, 1);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity1, smoothing);

        float targetSize = size + sizeScaling * target.GetComponent<Rigidbody2D>().linearVelocity.magnitude;

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetSize, ref velocity2, sizeSmoothing);
    }
}

