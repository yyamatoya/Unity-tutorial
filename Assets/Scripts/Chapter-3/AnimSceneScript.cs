using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSceneScript : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		var animator = GameObject.Find("Sphere").GetComponent<Animator>();
		float speed = animator.GetFloat("speed");
		//if (Input.GetKeyDown(KeyCode.Space))
		//{
		//	animator.speed = 0f;
		//}
		//else if (Input.GetKeyUp(KeyCode.Space))
		//{
		//	animator.speed = 1f;
		//}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			speed += 0.1f;
			animator.SetFloat("speed", speed);
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			speed -= 0.1f;
			animator.SetFloat("speed", speed);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			var flag = animator.GetBool("flag");
			animator.SetBool("flag", !flag);
		}
	}

}
