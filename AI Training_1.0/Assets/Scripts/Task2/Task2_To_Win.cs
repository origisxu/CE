using System.Collections;
using TMPro;
using UnityEngine;

public class Task2_To_Win : MonoBehaviour, ITaskHandler
{
    public int maxRound = 7;
    public int WinScore = 50;

    public TMP_Text scoreText;
    public TMP_Text roundText;

    public GameObject resultPanel;
    public TMP_Text resultText;
    public Animator resultAnimator;

    private int currentRound = 0;
    private int totalScore = 0;
    private bool isGameOver = false;
    private ScoreCalculator_2 scoreCalculator;

    void Start()
    {
        scoreCalculator = GetComponent<ScoreCalculator_2>();
        if (resultPanel != null)
            resultPanel.SetActive(false);
        UpdateUI();

        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        if (roundText == null)
            roundText = GameObject.Find("RoundText")?.GetComponent<TMP_Text>();
    }

    public void OnPlayerMessage(string playerMessage)
    {
        if (isGameOver) return;

        int roundScore = scoreCalculator.CalculateAngryScore(playerMessage);
        totalScore += roundScore;
        currentRound++;

        UpdateUI();

        if (currentRound >= maxRound)
        {
            StartCoroutine(EndGameWithAnimation());
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"求助分：{totalScore}";
        }

        if (roundText != null)
        {
            roundText.text = $"第{currentRound}/{maxRound}轮";
        }
    }

    IEnumerator EndGameWithAnimation()
    {
        
        isGameOver = true;

        yield return new WaitForSeconds(1f);

        if (resultAnimator != null)
        {
            resultAnimator.SetTrigger("Show");
        }

        if (resultPanel != null)
            resultPanel.SetActive(true);

        bool isWin = totalScore >= WinScore;
        if (resultText != null)
            resultText.text = isWin ? "胜利！" : "失败";

        yield return new WaitForSeconds(1f);
    }
}