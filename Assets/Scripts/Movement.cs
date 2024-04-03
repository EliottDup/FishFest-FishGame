using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform cam;

    [SerializeField] KeyCode forward = KeyCode.W, right = KeyCode.A, back = KeyCode.S, left = KeyCode.D, dash = KeyCode.Space;
    [SerializeField] float moveForce = 1, brakeForce = 1;


    bool canDash;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(forward))
        {
            print("moving");
            rb.AddForce(cam.forward * moveForce, ForceMode.Acceleration);
            return;
        }
        if (Input.GetKey(right))
        {
            rb.AddForce(cam.right * moveForce, ForceMode.Acceleration);
            return;
        }
        if (Input.GetKey(back))
        {
            print("moving");
            rb.AddForce(-cam.forward * moveForce, ForceMode.Acceleration);
            return;
        }
        if (Input.GetKey(left))
        {
            rb.AddForce(-cam.right * moveForce, ForceMode.Acceleration);
            return;
        }
        rb.AddForce(-rb.velocity.normalized * brakeForce);
    }
}
