using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingTransformedByTouchOtherData : MonoBehaviour
{
	Vector3 cv = new Vector3(0f, 1f, -5f);
	Rigidbody rb = null;
	Dictionary<string, int> data = new Dictionary<string, int>();
	Color[] cdata = {
		Color.white, Color.black, Color.gray,
		Color.red, Color.green, Color.blue,
		Color.cyan, Color.magenta, Color.yellow,
		new Color(1f,1f,0f,1f)
	};
	System.Random r = new System.Random();


	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		var obs = GameObject.FindGameObjectsWithTag("Other");
		foreach (var ob in obs)
		{
			var data = ob.GetComponent<OtherData>();
			data.Value = 5;
			data.Color = Color.cyan;
			ChangeOther(ob);
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
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Other")
		{
			var data = other.gameObject.GetComponent<OtherData>();
			data.AddValue();
			data.Color = cdata[r.Next(10)];
			ChangeOther(other.gameObject);
		}
	}

	void ChangeOther(GameObject obj)
	{
		var data = obj.GetComponent<OtherData>();
		var rd = obj.GetComponent<Renderer>();
		var d = 1.0f - (0.1f * data.Value);
		var c = data.Color;
		c.a = d;
		rd.material.color = c;
		rd.material.SetFloat("_Metallic", d);

	}
}
