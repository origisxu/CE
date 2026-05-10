using System.Collections;
using TMPro;
using UnityEngine;

public class Task1_To_Win : MonoBehaviour, ITaskHandler
{
    public int maxRound = 7;
    public int WinScore = 65;

    public TMP_Text scoreText;
    public TMP_Text roundText;

    public GameObject resultPanel;
    public TMP_Text resultText;
    public Animator resultAnimator;

    private int currentRound = 0;
    private int totalScore;
    private bool isGameOver = false;
    private ScoreCalculator scoreCalcultor;

    void Start()
    {
        // 自动查找UI文本（如果没有手动赋值）
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        }

        if (roundText == null)
        {
            roundText = GameObject.Find("RoundText")?.GetComponent<TMP_Text>();
        }

        scoreCalcultor = GetComponent<ScoreCalculator>();
        if (scoreCalcultor == null)
        {
            scoreCalcultor = gameObject.AddComponent<ScoreCalculator>();
        }

        if (resultPanel != null)
            resultPanel.SetActive(false);

        UpdateUI();
    }

    public void OnPlayerMessage(string playerMessage)
    {
        if (isGameOver) return;

        int roundScore = scoreCalcultor.CalculatrScore(playerMessage);
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