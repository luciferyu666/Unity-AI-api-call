using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 在場景中找不到 GameManager，則建立一個新的 GameObject 來存放
                var managerObj = new GameObject("GameManager");
                _instance = managerObj.AddComponent<GameManager>();
                DontDestroyOnLoad(managerObj);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // 確保只有一個 GameManager 存在
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 切換到遊戲場景
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// 返回主選單（若需要）
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
