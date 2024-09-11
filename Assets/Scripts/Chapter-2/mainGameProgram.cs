﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainGameProgram : MonoBehaviour
{
	Dictionary<string, bool> flag = new Dictionary<string, bool>()
	{
		{"Sphere 0", false },
		{"Sphere 1", false },
		{"Sphere 2", false },
		{"Sphere 3", false },
	};

	Dictionary<string, int> param = new Dictionary<string, int>()
	{
		{"power", 100 },    //	体力
		{"level", 2 } ,  //	レベル
		{"exp", 0 }	//	経験値
	};

	// ゲーム完了フラグ
	private bool finish = false;

	Vector3 cv = new Vector3(0f, 1f, -5f);  //	カメラ位置
	Rigidbody rb = null;
	Text message = null;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		message = GameObject.Find("Message").GetComponent<Text>();

	}

	private void FixedUpdate()
	{
		if (Finish())
		{
			return;
		}
		var sv = transform.position;
		sv.y = 1f;
		Camera.main.transform.position = sv + cv;
		// 入力処理
		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");
		//	var d = (float)(2.0+param["level"]/2);
		var v = new Vector3(x * param["level"], 0, y * param["level"]);
		rb.AddForce(v);
	}
	public int Level()
	{
		return param["level"];
	}

	public bool Finish()
	{
		return finish;
	}

	public void AddExp(int n)
	{
		param["exp"] += n;
		Debug.Log("[AddExp] Player Exp: " + param["exp"]);
		//	もし10溜まったらレベルアップ
		if (param["exp"] >= 5)
		{
			levelUp();
		}
	}

	void Fight(GameObject go)
	{
		var er = go.GetComponent<Rigidbody>();
		var pr = GetComponent<Rigidbody>();
		var od = er.GetComponent<OtherData>();
		// magitude(絶対値、移動速度)を比べて勝敗判定
		if (er.velocity.magnitude > pr.velocity.magnitude)
		{
			// 負けた場合
			Debug.Log("Player: Loss...");
			//	体力が落ちる
			param["power"] -= od.Level();
			//経験値がもらえる
			AddExp(od.Level() / 2);

			// 体力がなくなった場合
			if (param["power"] <= 0)
			{
				// ゲームオーバー
				Loss();
			}
			else
			{
				//	勝った場合
				Debug.Log("Player: Gotcha!");
				AddExp(od.Level());
			}
		}
	}

	// 衝突時の処理
	private void OnCollisionEnter(Collision collision)
	{
		if (Finish())
		{
			return;
		}
		if (collision.gameObject.tag == "Other")
		{
			// Otherタグ(敵)と衝突したとき
			_ = collision.gameObject.GetComponent<OtherData>();
			// 敵と戦う
			Fight(collision.gameObject);
		}
	}

	public void Flag(string flg)
	{
		flag[flg] = true;
		if (CheckFlg())
		{
			// フラグを確認してTrueなら終了
			Win();
		}
	}


	bool CheckFlg()
	{
		//var f = true;
		//foreach (var item in flag)
		//{
		//	if (item.Value == false)
		//	{
		//		f = false;
		//	}
		//}
		//return f; // すべてtrueならtrue

		return !flag.ContainsValue(false);
	}

	void levelUp()
	{
		if (param["level"] == 5)
		{
			return;
		}
		param["level"]++;
		param["exp"] = 0;
		message.text = "Level UP!! \n Level " + param["level"];
		message.color = Color.cyan;
		TimerStart();
	}

	void TimerStart()
	{
		// コルーチン(並列実行機能）を使ってサブプロセスを動かす
		StartCoroutine(TimerEnd());
	}

	// タイマー実行処理
	IEnumerator TimerEnd()
	{
		// 3秒待つ
		yield return new WaitForSeconds(3f);
		// 表示テキストの初期化
		message.text = "";
	}

	void Loss()
	{
		message.text = "GAME OVER";
		message.color = Color.red;
		finish = true;
	}

	void Win()
	{
		message.text = "WIN!!";
		message.color = Color.yellow;
		finish = true;
	}
}
