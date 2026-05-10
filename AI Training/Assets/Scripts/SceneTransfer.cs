using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    // 静态变量，整个游戏唯一，切换场景也不丢失
    public static string selectedCharacter = "";


    public void LoadDialogueWithCharacter(string character)
    {
        selectedCharacter = character;
        SceneManager.LoadScene("Speak");
    }
}