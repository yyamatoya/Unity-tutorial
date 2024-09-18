using System.Collections;
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

	// ハイスコア機能
	//	スコア表示用UI
	Text score = null;
	//	ハイスコア用UI
	Text high = null;
	// 情報メッセージ用UI
	Text infoMessage = null;

	//	開始時間
	int EndTime = 1000;
	//	スコア
	int Score = 0;
	// ハイスコア
	int High = 0;


	// GameData本体
	public GameData gameData;

	void LoadGameData()
	{
		EndTime = (int)Time.time + gameData.score;
		transform.position = gameData.position[4];
		GetComponent<Rigidbody>().velocity = gameData.velocity[4];
		// 初期設定（プレイヤー）の読み込み
		param = new Dictionary<string, int>
		{
			{"power", gameData.param[4].power },
			{"level", gameData.param[4].level },
			{"exp", gameData.param[4].exp }
		};
		// 勝利フラグの読み込み
		flag = new Dictionary<string, bool>
		{
			{"Sphere 0", gameData.flag[0] },
			{"Sphere 1", gameData.flag[1] },
			{"Sphere 2", gameData.flag[2] },
			{"Sphere 3", gameData.flag[3] },
		};

		for (int i = 0; i < 4; i++)
		{
			var prm = new Dictionary<string, int>
			{
				{"power", gameData.param[i].power },
				{"level", gameData.param[i].level },
				{"exp", gameData.param[i].exp }
			};
			var obj = GameObject.Find("Sphere " + i);
			obj.GetComponent<OtherData>().SetParam(prm);
			obj.GetComponent<OtherData>().SetDv(gameData.dv[i]);
			obj.transform.position = gameData.position[i];
			obj.GetComponent<Rigidbody>().velocity = gameData.velocity[i];
			obj.GetComponent<OtherData>().UpdateFromGameData();
			Debug.Log("*** Load Data ***");
			UpdateFromGameData();
			message.text = "loaded.";
			TimerStart();
			Debug.Log("*** Load Data ***");

		}
	}

	void UpdateFromGameData()
	{
		var objs = GameObject.FindGameObjectsWithTag("Other");
		foreach (var obj in objs)
		{
			// 勝利フラグが立てば消滅
			if (flag[obj.name] == true)
			{
				GameObject.Destroy(obj);
			}
		}
	}

	void SaveGameData()
	{
		// スコアの保存
		gameData.score = Score;
		gameData.param[4] = new ParamObject(
			param["power"],
			param["level"],
			param["exp"]
		);
		gameData.position[4] = transform.position;
		gameData.velocity[4] = GetComponent<Rigidbody>().velocity;

		for (int i = 0; i < 4; i++)
		{
			var obj = GameObject.Find("Sphere " + i);
			if (obj == null)
			{
				// 敵を探して存在しない場合はスキップする
				continue;
			}
			var od = obj.GetComponent<OtherData>();
			var prm = od.GetParam();
			// gameDataに敵の体力などを保存（書き換え）
			gameData.param[i] = new ParamObject(
				prm["power"],
				prm["level"],
				prm["exp"]
			);
			// 速度も保存
			gameData.dv[i] = obj.GetComponent<OtherData>().GetDv();
			//	位置を記録
			gameData.position[i] = obj.transform.position;
			// 向いている方向
			gameData.velocity[i] = obj.GetComponent<Rigidbody>().velocity;
			// 勝利フラグの保存
			gameData.flag = new bool[]
			{
				flag["Sphere 0"],
				flag["Sphere 1"],
				flag["Sphere 2"],
				flag["Sphere 3"]
			};
			message.text = "saved.";
			TimerStart();
			Debug.Log("*** save data ***");
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		message = GameObject.Find("Message").GetComponent<Text>();
		/* スコア関連設定 */
		score = GameObject.Find("Score").GetComponent<Text>();
		high = GameObject.Find("High").GetComponent<Text>();
		infoMessage = GameObject.Find("InfoMessage").GetComponent<Text>();

		EndTime = (int)Time.time + 1000;
		High = PlayerPrefs.GetInt("high");
		high.text = "High Score: " + High;
		LoadGameData();
	}

	private void FixedUpdate()
	{
		if (Finish())
		{
			return;
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			SaveGameData();
		}
		Score = EndTime - (int)Time.time;
		score.text = "Time: " + Score;
		if (Score <= 0)
		{
			Loss();
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

		}
		else
		{
			//	勝った場合
			Debug.Log("Player: Gotcha!");
			infoMessage.text = "経験値 " + od.Level() + "を取得しました!";
			AddExp(od.Level());
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
			// フラグを確認して全てTrueなら終了
			Win();
		}
	}


	bool CheckFlg()
	{
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
		gameData.InitData();
	}

	void Win()
	{
		message.text = "WIN!!";
		message.color = Color.yellow;
		finish = true;
		if (High < Score)
		{
			High = Score;
			PlayerPrefs.SetInt("high", High);
			high.text = "High Score: " + High;
		}
		gameData.InitData();
	}
}
