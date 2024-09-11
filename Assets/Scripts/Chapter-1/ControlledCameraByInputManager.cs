using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledCameraByInputManager : MonoBehaviour
{
    Rigidbody rb = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var sv = transform.position;
        sv += Vector3.forward * -5 + Vector3.up;

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        var xv = Vector3.zero;
        var yv = Vector3.zero;

        if (x > 0)
        {
            xv = Camera.main.transform.up;
        }
        else if (x < 0)
        {
            xv = Camera.main.transform.up * -1;
        }
        if (y > 0)
        {
            yv = Camera.main.transform.forward;
        }
        else if (y < 0)
        {
            yv = Camera.main.transform.forward * -1;
        }
        Camera.main.transform.position += yv / 100;
        Camera.main.transform.Rotate(xv / 100);
    }
}
