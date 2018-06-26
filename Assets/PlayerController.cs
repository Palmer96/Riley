using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    Vector3 startPos;
    Vector3 startRot;

    float poisonTimer;
    public float poisonRate;
    public bool poisoned;
    public Text poisonText;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();

        startPos = transform.position;
        startRot = transform.eulerAngles;




        Reset();
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
                if (hit.transform.CompareTag("Grapple"))
                {

                    Vector3 pos = hit.point;
                    pos.z = 0;

                    if (Physics.Raycast(transform.position + Vector3.up, pos - transform.position, out hit))
                    {
                        target = hit.point;
                    }
                    grapple.transform.position = transform.position;
                    line.enabled = true;
                }
            }


        }
        if (Input.GetMouseButton(0))
        {
            if (target != Vector3.zero)
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
        }
        else
        {
            target = Vector3.zero;
            grapple.transform.position = Vector3.MoveTowards(grapple.transform.position, target, 1);

            line.enabled = false;
            line.SetPosition(0, transform.position + Vector3.up);
            line.SetPosition(1, grapple.transform.position);
        }



        if (poisoned)
        {
            poisonTimer -= Time.deltaTime;
            poisonText.text = (Mathf.Round(poisonTimer / 0.1f) * 0.1f).ToString();

            if (poisonTimer <= 0)
                Reset();
        }
        else
            poisonText.text = "";



    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Poison"))
        {
            if (!poisoned)
            {
                poisoned = true;
                poisonTimer = poisonRate;
            }

        }

        if (other.CompareTag("Cure"))
        {
            if (poisoned)
            {
                poisoned = false;
                Destroy(other.gameObject);
            }

        }
    }

    void Reset()
    {
        transform.position = startPos;
        transform.eulerAngles = startRot;

        target = transform.position + Vector3.up;
        grapple.transform.position = target;

        line.SetPosition(0, transform.position + Vector3.up);
        line.SetPosition(1, grapple.transform.position);
        poisoned = false;
        poisonText.text = "";

    }
}
