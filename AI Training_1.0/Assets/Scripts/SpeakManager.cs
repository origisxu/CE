using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeakUIManager : MonoBehaviour
{
    public TMP_Text scoreText;   // 场景中的得分文本
    public TMP_Text roundText;   // 场景中的轮次文本

    void Start()
    {
        // 场景加载完成后，找到当前激活的 Task 脚本并绑定
        SceneManager.sceneLoaded += OnSceneLoaded;
        BindToCurrentTask();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindToCurrentTask();
    }

    void BindToCurrentTask()
    {
        // 找到实现了 ITaskHandler 的 MonoBehaviour
        var taskObject = FindObjectOfType(typeof(ITaskHandler)) as MonoBehaviour;

        if (taskObject != null)
        {
            // 使用反射绑定文本字段
            var scoreField = taskObject.GetType().GetField("scoreText");
            var roundField = taskObject.GetType().GetField("roundText");

            if (scoreField != null)
                scoreField.SetValue(taskObject, scoreText);
            if (roundField != null)
                roundField.SetValue(taskObject, roundText);

            Debug.Log($"已绑定得分文本到 {taskObject.GetType().Name}");
        }
    }
}