using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledByInputManager : MonoBehaviour
{
    bool f = true;
    Vector3 cv = new Vector3(0f, 1f, -5f);
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
        sv.y = 1f;
        Camera.main.transform.position = sv + cv;
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var v = new Vector3(x, 0, y);
        var vz = Vector3.zero;

        var jump = Input.GetAxis("Fire1");

        if (jump > 0)
        {
            if (f)
            {
                vz = new Vector3(0f, 1000f, 0f);
            }
            f = false;
        }
        else
        {
            f = true;
        }
        rb.AddForce(v + vz);

    }
}
