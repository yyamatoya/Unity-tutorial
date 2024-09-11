using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : MonoBehaviour
{
	Vector3 cv = new Vector3(0f, 1f, -5f);
	Rigidbody rb = null;
	GameObject ex = null;
	float dt = 1.0f;    //	描画速度
	ParticleSystem ps = null;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		ex = GameObject.Find("BigExplosion");
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
			var go = other.gameObject;
			//	爆発エフェクトを移動
			ex.transform.position = go.transform.position;
			var main = ex.GetComponent<ParticleSystem>().main;
			main.loop = false;
			main.startSpeed = dt;
			main.simulationSpeed = dt;
			dt /= 2;

			ps = ex.GetComponent<ParticleSystem>();
			ps.Play();
			GameObject.Destroy(go);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Other")
		{
			ps.Stop();
		}
	}

}
