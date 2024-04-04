using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform cam;

    [SerializeField] KeyCode forward = KeyCode.W, right = KeyCode.A, back = KeyCode.S, left = KeyCode.D, dash = KeyCode.Space, up = KeyCode.E, down = KeyCode.Q;
    [SerializeField] float moveForce = 1, brakeForce = 1, sensitivity = 1, dashForce = 10;

    float orientation = 0.0f;
    float xRot = 0.0f;

    bool canDash = true;
    [SerializeField] float dashReset = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        orientation = transform.eulerAngles.y;
        xRot = cam.eulerAngles.x;
    }

    void Update()
    {

        // Pitch
        // cam.transform.rotation *= Quaternion.AngleAxis(
        //     -Input.GetAxis("Mouse Y") * sensitivity,
        //     Vector3.right
        // );

        xRot += -Input.GetAxis("Mouse Y") * sensitivity;

        xRot = Mathf.Clamp(xRot, -90, 90);

        cam.rotation = Quaternion.Euler(
            xRot,
            cam.eulerAngles.y,
            cam.eulerAngles.z
        );



        // Yaw
        orientation = orientation + Input.GetAxis("Mouse X") * sensitivity;
        transform.rotation = Quaternion.Euler(
            transform.eulerAngles.x,
            orientation,
            transform.eulerAngles.z
        );

        bool f = Input.GetKey(forward), r = Input.GetKey(right), b = Input.GetKey(back), l = Input.GetKey(left), u = Input.GetKey(up), d = Input.GetKey(down);
        Vector3 force = Vector3.zero;
        if (f)
        {
            force += cam.forward;
        }
        if (r)
        {
            force += transform.right;
        }
        if (b)
        {
            force -= cam.forward;
        }
        if (l)
        {
            force -= transform.right;
        }
        if (u)
        {
            force += transform.up;
        }
        if (d)
        {
            force -= transform.up;
        }
        rb.AddForce(force.normalized * moveForce, ForceMode.Acceleration);
        if (Input.GetKeyDown(dash) && canDash)
        {
            rb.AddForce(cam.forward * dashForce, ForceMode.Impulse);
            canDash = false;
            Invoke(nameof(ResetDash), dashReset);
        }
        if (!(f || r || b || l || u || d) && rb.velocity.magnitude != 0)
        {
            rb.AddForce(-rb.velocity.normalized * rb.velocity.magnitude);
            if (rb.velocity.magnitude < 0.05f)
            {
                rb.velocity = Vector3.zero;
            }
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -50, 50), Mathf.Clamp(transform.position.y, -10, 30), Mathf.Clamp(transform.position.z, -50, 50));
    }

    void ResetDash()
    {
        canDash = true;
    }
}
