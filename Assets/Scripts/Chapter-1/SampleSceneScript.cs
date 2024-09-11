using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSceneScript : MonoBehaviour
{
    int n = 0;
    Rigidbody rb = null;
    // Start is called before the first frame update
    void Start()
    {
        // RigidBodyの取得
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()

    {
        var v = new Vector3(0f, 0f, 0.01f);
        transform.Translate(v);

        if (n++ > 2500)
        {
            n = 0;
            var v0 = new Vector3(0f, 1f, -10f);
            transform.position = v0;
        }

        // rb.velocity = Vector3.zero;
        // rb.angular = Vector3.zero;

        // var v = new Vector3(0f, 0f, 200f);
        // rb.AddForce(v); // 力を与える
        // n++;
        // if (n == 2500)
        // {
        //     rb.velocity = Vector3.zero;
        //     rb.angularVelocity = Vector3.zero;
        //     var v2 = new Vector3(0f, 1f, -200f);
        //     n = -2500;
        //     rb.AddForce(v2);
        // }
    }
}
