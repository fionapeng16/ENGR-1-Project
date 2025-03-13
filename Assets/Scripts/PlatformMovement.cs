using UnityEngine;

public class MovingPlatformMovement : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    private Rigidbody2D rb;
    private Vector3 nextPosition;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        nextPosition=PointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed*Time.deltaTime);
        if (nextPosition==transform.position) {
            if (nextPosition==PointA.position) {
                nextPosition = PointB.position;
            }
            else {
                nextPosition = PointA.position;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.transform.parent = null;
        }
    }
}
