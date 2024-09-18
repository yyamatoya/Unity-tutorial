using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "game_data ", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
	public int score;
	public bool[] flag = new bool[5];
	public float[] dv = new float[5];
	public ParamObject[] param = new ParamObject[5];
	public Vector3[] position = new Vector3[5];
	public Vector3[] velocity = new Vector3[5];

	public void InitData()
	{
		score = 1000;
		flag = new bool[] { false, false, false, false, false };
		dv = new float[] { 1f, 1f, 1f, 1f, 1f };

		param = new ParamObject[]
		{
			new ParamObject(100,1,0),
			new ParamObject(100,1,0),
			new ParamObject(100,1,0),
			new ParamObject(100,1,0),
			new ParamObject(100,1,0)
		};

		position = new Vector3[]
		{
			new Vector3(5f,1f,5f),
			new Vector3(5f,1f,-5f),
			new Vector3(-5f,1f,-5f),
			new Vector3(-5f,1f,5f),
			new Vector3(0f,1f,-10f)
		};

		velocity = new Vector3[]
		{
			new Vector3(0f,0f,0f),
			new Vector3(0f,0f,0f),
			new Vector3(0f,0f,0f),
			new Vector3(0f,0f,0f),
			new Vector3(0f,0f,0f)
		};

	}

}

[System.Serializable]
public class ParamObject
{
	public int power;
	public int level;
	public int exp;

	public ParamObject(int p, int l, int e)
	{
		power = p;
		level = l;
		exp = p;
	}
}

