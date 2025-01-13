using UnityEngine;

public class Casing : MonoBehaviour
{
    BoxCollider2D casingCollider;

    private void Awake()
    {
        casingCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            EnableCollider();
        }
    }

    void EnableCollider()
    {
        casingCollider.enabled = true;
    }
}
