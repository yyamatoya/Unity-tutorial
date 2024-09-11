using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMoveByKey : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 cv = new Vector3(0f, 1f, -5f);
    Rigidbody rb = null;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var sv = transform.position;
        sv.y = 1f;
        Camera.main.transform.position = sv + cv;

        var v = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            v = new Vector3(0f, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            v = new Vector3(0f, 0f, -1f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            v = new Vector3(-1f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            v = new Vector3(1f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            v = new Vector3(0f, 10f, 0f);
        }
        rb.AddForce(v);
    }
}
