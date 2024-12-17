using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public bool lockX = false;
    public bool lockY = false;

    void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not assigned in CameraFollow script");
            return;
        }


        Vector3 desiredPosition = target.position + offset;
        if (lockX)
            desiredPosition.x = transform.position.x;
        if (lockY)
            desiredPosition.y = transform.position.y;
        desiredPosition.z = transform.position.z;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
