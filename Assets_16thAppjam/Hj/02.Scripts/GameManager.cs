using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public int Score = 0;
	private int bestScore = 0;

	public Text ScoreText;
	public Text BestScoreText;
	public Text TimeLeftText;
	
	private float nowTime = 0;
	private float maxTime = 120;

	public bool Gameovered = false;

	public Image FadePanel;
	
	public static GameManager instance;

	private void Awake()
	{
		nowTime = maxTime;
		instance = this;
		bestScore = PlayerPrefs.GetInt("BestScore", 0);
		BestScoreText.text = "BestScore : " + bestScore.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			GameOver();
		}
	}

	private void Update()
	{
		if(nowTime > 0)
			nowTime -= Time.deltaTime;
		if(nowTime > maxTime)
			GameOver();
		else if (nowTime > 100)
			EnemyManager.instance.count = 7;
		else if (nowTime > 80)
			EnemyManager.instance.count = 6;
		else if (nowTime > 60)
			EnemyManager.instance.count = 5;
		else if (nowTime < 30)
			TimeLeftText.color = Color.red;

		if (nowTime % 60 < 10)
		{
			TimeLeftText.text = "0" + ((int)nowTime / 60).ToString() + " : 0" + ((int)nowTime % 60).ToString();
		}
		else
		{
			TimeLeftText.text = "0" + ((int)nowTime / 60).ToString() + " : " + ((int)nowTime % 60).ToString();
		}
	}

	public void ScoreAdd(int num)
	{
		Score += num;
		ScoreText.text = "Score : " + Score.ToString();
		PlayerPrefs.SetInt("Score",Score);
		
		if (Score != 0 && Score > bestScore)
		{
			bestScore = Score;
			ScoreText.color = Color.yellow;
			BestScoreText.text = "BestScore : " + Score.ToString();
			PlayerPrefs.SetInt("BestScore",Score);
		}
	}

	public void GameOver()
	{
		if (!Gameovered)
		{
			Gameovered = true;
			StartCoroutine("GameOverAni");
		}
	}

	IEnumerator GameOverAni()
	{
		for (int i = 0; i <= 20; i++)
		{
			FadePanel.color = new Color(0,0,0,i/20f);
			yield return  new WaitForSeconds(0.003f);
		}
		yield return  new WaitForSeconds(0.5f);
		
		SceneManager.LoadScene(2);
	}
}
