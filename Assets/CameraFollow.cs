using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;
    public float xIncrement;
    public float yIncrement;
    public float lerpSpeed;
    Vector3 target;
   public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


    }

    // Update is called once per frame
    void Update()
    {
        target = player.transform.position;
        target.x = Mathf.Round(target.x / xIncrement) * xIncrement;
        target.y = Mathf.Round(target.y / yIncrement) * yIncrement;

        transform.position = Vector3.Lerp(transform.position, target + offset, lerpSpeed);

    }
}
