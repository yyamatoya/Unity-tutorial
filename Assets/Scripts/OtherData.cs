using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherData : MonoBehaviour
{
	Dictionary<string, int> param = new Dictionary<string, int>()
	{
		{"power", 100 },    //	体力
		{"level", 1 } ,  //	レベル
		{"exp", 0 }	//	経験値
	};
	Color[] levelColor =
	{
		new Color(0f,0f,0.75f),
		new Color(0f,0f,1f),
		new Color(.25f, .25f,1f),
		new Color(.5f, .5f, 1f),
		new Color(.75f, .75f, 1f),
		new Color(1f,1f,1f)
	};

	float dv = 1f;  // 移動係数
	GameObject player = null;   //プレイヤー
	mainGameProgram sss = null;   // メインプログラム

	private void Start()
	{
		player = GameObject.Find("Player");
		sss = player.GetComponent<mainGameProgram>();
	}

	//レベルを取得する
	public int Level()
	{
		return param["level"];
	}

	// 経験値をn加算する
	public void AddExp(int n)
	{
		param["exp"] += n;
		//	もし10溜まったらレベルアップ
		if (param["exp"] >= 10)
		{
			levelUp();
		}
	}

	void levelUp()
	{
		if (param["level"] == 5)
		{
			return;
		}
		//	レベルUP
		param["level"]++;
		//	経験値リセット
		param["exp"] = 0;

		var r = gameObject.GetComponent<Renderer>();
		r.material.color = levelColor[param["level"]];
		// レベルが上がるごとに移動速度が増加
		dv = param["level"];
	}

	void Fight()
	{
		var pr = player.GetComponent<Rigidbody>();
		var mr = GetComponent<Rigidbody>();
		// magitude(絶対値、移動速度)を比べて勝敗判定
		Debug.Log("Player magnitude: " + pr.velocity.magnitude);
		Debug.Log("Other magnitude: " + mr.velocity.magnitude);

		// プレイヤーの当たりが強いとき
		if (pr.velocity.magnitude > mr.velocity.magnitude)
		{
			// 負けた場合
			Debug.Log(gameObject.name + ": Loss...");
			//	体力が落ちる
			param["power"] -= sss.Level();
			//経験値がもらえる
			AddExp(sss.Level() / 2);

			// 体力がなくなった場合
			if (param["power"] <= 0)
			{
				sss.Flag(this.gameObject.name);
				// 消滅
				Destroy(this.gameObject);
			}
			else
			{
				//	勝った場合
				Debug.Log(gameObject.name + ": Gotcha!");
				AddExp(sss.Level());
			}
		}
	}

	//表示更新
	private void FixedUpdate()
	{
		if (sss.Finish())
		{
			return;
		}
		var dp = player.transform.position - transform.position;
		var r = GetComponent<Rigidbody>();
		r.AddForce(dp / 10 * dv);
	}

	//	衝突処理
	private void OnCollisionEnter(Collision collision)
	{
		if (sss.Finish())
		{
			return;
		}
		if (collision.gameObject.name == "Player")
		{
			var halo = (Behaviour)GetComponent("Halo");
			halo.enabled = true;
			
			
			Fight();
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (sss.Finish())
		{
			return;
		}
		if (collision.gameObject.name == "Player")
		{
			var halo = (Behaviour)GetComponent("Halo");
			halo.enabled = false;
		}
	}


}
