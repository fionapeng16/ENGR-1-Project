using UnityEngine;
using System.Collections;


public class movement2 : MonoBehaviour
{
    
    public float speed = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
	    {
		transform.position += Vector3.left * speed * Time.deltaTime;
	    }
	if (Input.GetKey(KeyCode.RightArrow))
	    {
		transform.position += Vector3.right * speed * Time.deltaTime;
	    }
	if (Input.GetKey(KeyCode.UpArrow))
	    {
		transform.position += Vector3.up * speed * Time.deltaTime;
	    }
	if (Input.GetKey(KeyCode.DownArrow))
	    {
		transform.position += Vector3.down * speed * Time.deltaTime;
	    }
    }
}
