using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour {
	
	
	public TextMesh ScoreTextMesh;
	public TextMesh BestScoreTextMesh;

	private void Start()
	{
		int Score = PlayerPrefs.GetInt("Score", 0);
		PlayerPrefs.SetInt("Score", 0);
		int bestScore = PlayerPrefs.GetInt("BestScore", 0);
		PlayerPrefs.SetInt("BestScore", 0);
		
		ScoreTextMesh.text = "Score : " + Score.ToString();
		BestScoreTextMesh.text = "BestScore : " + bestScore.ToString();
	}

	public void Check()
	{
		SceneManager.LoadScene(0);
	}
}