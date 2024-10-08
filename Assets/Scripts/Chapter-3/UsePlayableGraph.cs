﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class UsePlayableGraph : MonoBehaviour
{
	public Vector3 startPosition = new Vector3(0f, 1f, 100f);
	public float length = 100f;
	public float width = 10f;

	public float second = 10f;
	public int count = 10;

	GameObject sphere;
	PlayableGraph pg;
	AnimationClipPlayable acp;

	float weight = 0f; // MixPlayableのウェイト
	bool flag = true;
	ScriptPlayable<AnimSpherePlayableBehavior> pb;
	AnimationMixerPlayable mxp;


	// Start is called before the first frame update
	void Start()
	{
		pg = CreatePlayableGraph(startPosition, length, width, second, count);
		pb = ScriptPlayable<AnimSpherePlayableBehavior>.Create(pg);
		//pb.GetBehaviour().Sphere = sphere;
		var op = AnimationPlayableOutput.Create(pg, "Animation", sphere.GetComponent<Animator>());
		op.SetSourcePlayable(pb);
		pg.Play();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (flag)
			{
				pg.Stop();
			}
			else
			{
				pg.Play();
			}
			//	acp.SetSpeed(acp.GetSpeed() * -1);
			//	acp.Play();
			flag = !flag;
		}
		//if (Input.GetKeyDown(KeyCode.Return))
		//{
		//	acp.SetTime(0);
		//	acp.SetSpeed(1);
		//	acp.Play();
		//}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			mxp.SetSpeed(mxp.GetSpeed() + 0.1f);
			//acp.SetSpeed(acp.GetSpeed() + 0.1f);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			mxp.SetSpeed(mxp.GetSpeed() - 0.1f);
			//acp.SetSpeed(acp.GetSpeed() - 0.1f);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			weight += 0.1f;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			weight -= 0.1f;
		}
		mxp.SetInputWeight(0, 1.0f - weight);
		mxp.SetInputWeight(1, weight);
	}

	PlayableGraph CreatePlayableGraph(Vector3 pos, float ln, float wh, float sec, int count)
	{
		sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = pos;
		var rdr = sphere.GetComponent<Renderer>();
		rdr.material = new Material(Shader.Find("Standard"));
		rdr.material.color = Color.magenta;

		// AnimationClipの作成
		//var clip = CreateAnimationClip(pos, ln, wh, sec, count);
		var clip0 = CreateAnimationClip(pos, ln, wh, sec, count);
		var clip1 = CreateAnimationClip2(pos, ln, wh, sec, count);

		var animator = sphere.AddComponent<Animator>();
		animator.applyRootMotion = true;    // 相対座標で動くように設定

		var graph = PlayableGraph.Create();
		var op = AnimationPlayableOutput.Create(graph, "Animation", animator);
		mxp = AnimationMixerPlayable.Create(graph, 2);
		op.SetSourcePlayable(mxp);
		var cp0 = AnimationClipPlayable.Create(graph, clip0);
		var cp1 = AnimationClipPlayable.Create(graph, clip1);
		graph.Connect(cp0, 0, mxp, 0);
		graph.Connect(cp1, 0, mxp, 1);

		//acp = AnimationPlayableUtilities.PlayClip(animator, clip, out pg);
		return graph;
	}

	AnimationClip CreateAnimationClip(Vector3 v, float ln, float wh, float sc, int tm)
	{
		var clip = new AnimationClip();
		clip.wrapMode = WrapMode.Loop;
		var delta = ln / tm;
		// X軸カーブの作成
		var curveX = CreateCurveX(v, ln, wh, sc, tm);
		// Y軸カーブの作成
		var curveY = CreateCurveY(sc);
		// Z軸カーブの作成
		var curveZ = CreateCurveZ(v, ln, wh, sc, tm);

		// カーブを設定
		clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);
		return clip;
	}

	AnimationClip CreateAnimationClip2(Vector3 v, float ln, float wh, float sc, int tm)
	{
		var clip = new AnimationClip();
		clip.wrapMode = WrapMode.Loop;
		var delta = ln / tm;
		// X軸カーブの作成
		var curveX = CreateCurveX(v, ln, wh, sc, tm);
		// Y軸カーブの作成
		var curveY = CreateCurveY(sc);
		// Z軸カーブの作成
		var curveZ = CreateCurveZ(v, ln, wh, sc, tm);

		// カーブを設定
		clip.SetCurve("", typeof(Transform), "localPosition.x", curveY);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curveX);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);
		return clip;
	}

	// X軸カーブの作成（ジグザグに動く）
	AnimationCurve CreateCurveX(Vector3 v, float ln, float wh, float sc, int tm)
	{
		var curveX = AnimationCurve.Linear(0f, 0f, sc, 0f);
		var delta = ln / tm;
		var x = v.z;
		var ds = sc / tm;

		for (int i = 0; i < tm; i++)
		{
			curveX.AddKey(ds * i + ds * 0f, v.x);
			curveX.AddKey(ds * i + ds * 0.25f, v.x - wh / 2);
			curveX.AddKey(ds * i + ds * 0.5f, v.x);
			curveX.AddKey(ds * i + ds * 0.75f, v.x + wh / 2);
		}
		return curveX;
	}
	// Y軸カーブの作成（まっすぐ動く）
	AnimationCurve CreateCurveY(float sc)
	{
		return AnimationCurve.Linear(0f, 1f, sc, 1f);
	}
	// Z軸カーブの作成（まっすぐ動く）
	AnimationCurve CreateCurveZ(Vector3 v, float ln, float wh, float sc, int tm)
	{
		var curveZ = AnimationCurve.Linear(0f, v.z, sc, v.z - ln);
		var delta = ln / tm;
		var x = v.z;
		var ds = sc / tm;
		for (int i = 0; i < tm; i++)
		{
			curveZ.AddKey(ds * i + ds * 0f, x);
			curveZ.AddKey(ds * i + ds * 0.25f, x - delta / 4);
			curveZ.AddKey(ds * i + ds * 0.5f, x - delta / 4 * 2);
			curveZ.AddKey(ds * i + ds * 0.75f, x - delta / 4 * 3);
			x -= delta;
		}
		return curveZ;
	}

	void OnDisable()
	{
		pg.Destroy();
	}
}
