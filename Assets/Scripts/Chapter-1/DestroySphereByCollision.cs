using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySphereByCollision : MonoBehaviour
{
	// bool f = true;
	Vector3 cv = new Vector3(0f, 1f, -5f);
	Rigidbody rb = null;
	Logger lg;

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

		rb.AddForce(v);
	}

	// void OnCollisionEnter(Collision other)
	// {
	//     if (other.gameObject.tag == "Other")
	//     {
	//         GameObject.Destroy(other.gameObject);
	//     }
	// }

	/*void OnCollisionEnter(Collision other)
	{
        if (other.gameObject.tag == "Other")
        {
            var r = other.gameObject.GetComponent<Renderer>();
            r.material.color = new Color(0f, 0f, 0f, 0.25f);
            r.material.SetFloat("_Metallic", 0f);
        }
    }*/

	//private void OnTriggerEnter(Collider other)
	//{
	//	if (other.gameObject.tag == "Other")
	//	{
	//		var h = (Behaviour)other.gameObject.GetComponent("Halo");
	//		h.enabled = true;
	//	}
	//}

	//private void OnTriggerEnter(Collider other)
	//{
	//	if (other.gameObject.tag == "Other")
	//	{
	//		var ps = other.gameObject.GetComponent<ParticleSystem>();
	//		//ps.Play();
	//		var ep = new ParticleSystem.EmitParams();
	//		ep.startColor = Color.yellow;
	//		ep.startSize = 0.1f;
	//		ps.Emit(ep, 1000);
	//	}
	//}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Other")
		{
			var ex = GameObject.Find("BigExplosion");
			var ps = ex.GetComponent<ParticleSystem>();
			ps.Play();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Other")
		{
			var h = (Behaviour)other.gameObject.GetComponent("Halo");
			h.enabled = false;
		}
	}
}
