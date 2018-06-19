using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector3(Input.GetAxis("Horizontal"), -1, 0) * speed * Time.deltaTime);
        //transform.Translate(new Vector3(Input.GetAxis("Horizontal"), -1, 0) * speed * Time.deltaTime);
        //transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * speed * Time.deltaTime;
    }
}
