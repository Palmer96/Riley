using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    float currentSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float jumpForce;
    public float airModifier;
    public bool onGround;

    Vector3 movement;
    Rigidbody rb;
    Animator anim;

    LineRenderer line;
    public Transform grapple;
    Vector3 target;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateGrapple();
    }

    void UpdateMovement()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("Crouch", true);
            currentSpeed = crouchSpeed;
        }
        else
            anim.SetBool("Crouch", false);

        transform.position += movement * currentSpeed * Time.deltaTime;


        if (movement.x > 0.1f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0), 0.2f);
        else if (movement.x < -0.1f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 270, 0), 0.2f);

        anim.SetFloat("Forward", Mathf.Abs(movement.x * currentSpeed));


        onGround = Physics.CheckSphere(transform.position + new Vector3(0, -0.6f, 0), 0.49f);
        if (onGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce);
            }
        }
    }

    void UpdateGrapple()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3 pos = hit.point;
                pos.z = 0;

                if (Physics.Raycast(transform.position + Vector3.up, pos - transform.position, out hit))
                {
                    target = hit.point;
                }
            }


        }
        if (Input.GetMouseButton(0))
        {

            grapple.transform.position = Vector3.MoveTowards(grapple.transform.position, target, 1);

            line.SetPosition(0, transform.position + Vector3.up);
            line.SetPosition(1, grapple.transform.position);



           // Pull
            if (Vector3.Distance(grapple.transform.position, target) < 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, grapple.transform.position, 0.5f);
            }
        }
        else
        {
            target = transform.position + Vector3.up;
            grapple.transform.position = Vector3.MoveTowards(grapple.transform.position, target, 1);

            line.SetPosition(0, transform.position + Vector3.up);
            line.SetPosition(1, grapple.transform.position);
        }



    }

}
