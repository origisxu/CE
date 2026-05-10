using System.Collections;
using TMPro;
using UnityEngine;

public class Task5_To_Win : MonoBehaviour, ITaskHandler
{
    public int maxRound = 7;
    public int WinScore = 60;

    public TMP_Text scoreText;
    public TMP_Text roundText;

    public GameObject resultPanel;
    public TMP_Text resultText;
    public Animator resultAnimator;

    private int currentRound = 0;
    private int totalScore = 0;
    private bool isGameOver = false;
    private ScoreCalculator_5 scoreCalculator;

    void Start()
    {
        scoreCalculator = GetComponent<ScoreCalculator_5>();
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

        int roundScore = scoreCalculator.CalculateAdviceScore(playerMessage);
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
        Debug.Log("Task5 请教场景结束");
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
        {
            if (isWin)
                resultText.text = "指导有方！对方获益良多！";
            else
                resultText.text = "指导欠佳，需要提升沟通能力";
        }

        yield return new WaitForSeconds(1f);
    }
}