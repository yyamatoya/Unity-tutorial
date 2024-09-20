﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class AnimSpherePlayableBehavior : PlayableBehaviour
{
	public GameObject Sphere;
	int count = 0;
	bool flag = false;
	// Called when the owning graph starts playing
	public override void OnGraphStart(Playable playable)
	{
		Debug.Log("OnGraphStart");
		if (Sphere == null)
		{
			return;
		}
		Sphere.GetComponent<Renderer>().material.color = Color.magenta;
	}

	// Called when the owning graph stops playing
	public override void OnGraphStop(Playable playable)
	{
		Debug.Log("OnGraphStop");
		if (Sphere == null)
		{
			return;
		}
		Sphere.GetComponent<Renderer>().material.color = Color.black;
	}

	// Called when the state of the playable is set to Play
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		Debug.Log("OnBehaviourPlay");
	}

	// Called when the state of the playable is set to Paused
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		Debug.Log("OnBehaviourPause");
	}

	// Called each frame while the state is set to Play
	public override void PrepareFrame(Playable playable, FrameData info)
	{
		Debug.Log("PrepareFrame");
		if (Sphere == null)
		{
			return;
		}
		var renderer = Sphere.GetComponent<Renderer>();
		if (flag == true)
		{
			var color = renderer.material.color;
			color.r -= 0.02f;
			color.g += 0.02f;
			//color.b -= 0.02f;
			renderer.material.color = color;
		}
		else
		{
			var color = renderer.material.color;
			color.r += 0.02f;
			color.g -= 0.02f;
			//color.b -= 0.02f;
			renderer.material.color = color;
		}
		count++;
		if (count == 50)
		{
			count = 0;
			flag = !flag;
		}
	}
}
