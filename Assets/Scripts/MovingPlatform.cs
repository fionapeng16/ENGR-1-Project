using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; // Speed of rotation
    public float radius = 3f; // Radius of movement
    private Vector3 centerPosition;

    void Start()
    {
        centerPosition = transform.position;
    }

    void Update()
    {
        float x = centerPosition.x + Mathf.Cos(Time.time * speed) * radius;
        float y = centerPosition.y + Mathf.Sin(Time.time * speed) * radius;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}