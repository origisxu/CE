using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    public void ReturnToMain()
    {
        // 删除对话场景的 EventSystem
        GameObject dialogueEventSystem = GameObject.Find("EventSystem");
        if (dialogueEventSystem != null)
        {
            Destroy(dialogueEventSystem);
        }

        // 加载主场景
        SceneManager.LoadScene("Main");
    }
}