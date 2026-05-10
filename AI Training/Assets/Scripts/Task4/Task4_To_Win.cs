using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Task4_To_Win : MonoBehaviour, ITaskHandler
{
    public int maxRound = 7;
    public int WinScore = 80;

    public TMP_Text scoreText;
    public TMP_Text roundText;

    public GameObject resultPanel;
    public TMP_Text resultText;
    public Animator resultAnimator;

    private int currentRound = 0;
    private int totalScore = 0;
    private bool isGameOver = false;
    private ScoreCalculator_4 scoreCalculator;

    void Start()
    {
        scoreCalculator = GetComponent<ScoreCalculator_4>();
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

        int roundScore = scoreCalculator.CalculateBackendScore(playerMessage);
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
        Debug.Log("Task4 Java后端面试结束");
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
            resultText.text = isWin ? "面试通过！获得高级Offer！" : "面试失败，需要加强技术";

        yield return new WaitForSeconds(1f);
    }
}
