using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private float score = 0.0f;

    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 999999;
    private int scoreToNextLevel = 0;

    public Text scoreText;
    public Text scoreText2;
    public GameObject newHighscore;
    public GameObject oldHighscore;

    private bool isDead = false;

    // Update is called once per frame
    void Update()
    {

        if (isDead)
            return;

        if (score >= scoreToNextLevel)
            LevelUp();

        score += Time.deltaTime * 3;
        scoreText.text = ((int)score).ToString();
        scoreText2.text = ((int)score).ToString();
    }

    void LevelUp()
    {
        if (difficultyLevel == maxDifficultyLevel)
            return;

        scoreToNextLevel *= 2;
        difficultyLevel++;

        GetComponent<PlayerMovement>().SetForwardForce(difficultyLevel);
    }

    public void OnDeath()
    {
        isDead = true;

        if (PlayerPrefs.GetFloat("Highscore") < score)
            PlayerPrefs.SetFloat("Highscore", score);

        if (PlayerPrefs.GetFloat("Highscore") > score)
            newHighscore.SetActive(false);
        else newHighscore.SetActive(true);

        if (PlayerPrefs.GetFloat("Highscore") > score)
            oldHighscore.SetActive(true);
        else oldHighscore.SetActive(false);
    }
}
