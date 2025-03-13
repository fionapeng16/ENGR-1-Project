using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;  // Assign the Player in the Inspector
    public float smoothSpeed = 0.1f; // Adjust for smoother movement
    private float fixedY; // Store the initial Y position

    void Start()
    {
        fixedY = transform.position.y; // Save the starting Y position
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Follow the player's X position and keep Y fixed
            Vector3 newPosition = new Vector3(player.position.x, fixedY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
        }
    }
}
