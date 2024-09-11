using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingTransformedByTouch : MonoBehaviour
{
	Vector3 cv = new Vector3(0f, 1f, -5f);
	Rigidbody rb = null;
	//int[] data = { };
	Dictionary<string, int> data = new Dictionary<string, int>();



	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		var obs = GameObject.FindGameObjectsWithTag("Other");
		//	data = new int[obs.Length];
		//	for (int i = 0; i < obs.Length; i++)
		//	{
		//		data[i] = 0;
		//	}
		foreach (var ob in obs)
		{
			data[ob.name] = 0;
		}
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
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Other")
		{
			var n = int.Parse(other.gameObject.name.Substring(7));
			//data[n] += 1;
			data[other.gameObject.name] += 1;
			ChangeOther(other.gameObject);
			//SetUpOther();
		}
	}

	//void SetUpOther()
	//{
	//	for (int i = 0; i < data.Length; i++)
	//	{
	//		var ob = GameObject.Find("Sphere " + i);
	//		var rd = ob.GetComponent<Renderer>();
	//		var d = 1.0f - (0.1f * data[i]);
	//		var c = rd.material.color;
	//		c.a = d;
	//		rd.material.color = c;
	//		rd.material.SetFloat("_Metallic", d);

	//	}
	//}

	void ChangeOther(GameObject obj)
	{
		var rd = obj.GetComponent<Renderer>();
		var d = 1.0f - (0.1f * data[obj.name]);
		var c = rd.material.color;
		c.a = d;
		rd.material.color = c;
		rd.material.SetFloat("_Metallic", d);

	}
}
