using NUnit.Framework;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        currentPoint=PointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint==PointB.transform) {
            rb.linearVelocity = new Vector2(speed,0);
        }
        else {
            rb.linearVelocity = new Vector2(-speed,0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position)<1f && currentPoint==PointB.transform) {
            flip(); 
            currentPoint=PointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position)<1f && currentPoint==PointA.transform) {
          flip();
          currentPoint=PointB.transform;
        }
    }

    private void flip() {
        Vector3 localScale = transform.localScale;
        localScale.x  *= -1;
        transform.localScale = localScale;
    }
}
