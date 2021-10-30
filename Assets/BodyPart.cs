using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public CircleCollider2D collider2D;
    public Rigidbody2D rigidbody2D;

    int lifetime = 50;
    int framesElapsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnableCollision", 2.0f);
    }

    public void EnableCollision() {
        collider2D.enabled = true;
        rigidbody2D.simulated = true;
    }

    // Update is called once per frame
    void Update()
    {
        framesElapsed++;
        if (framesElapsed > lifetime) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        transform.localScale = new Vector3(
            transform.localScale.x * 0.99f, 
            transform.localScale.y * 0.99f, 
            transform.localScale.z * 0.99f
            );
    }
}
