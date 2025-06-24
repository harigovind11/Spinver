using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
                _instance.Init();
            }
            return _instance;
        }
    }

    private string highScoreKey = "HighScore";

    public int HighScore
    {
        get
        {
            return PlayerPrefs.GetInt(highScoreKey, 0);
        }
        set
        {
            PlayerPrefs.SetInt(highScoreKey, value);
        }
    }

    public int CurrentScore { get; set; }
    public bool IsInitialized { get; set; }

    private void Init()
    {
        IsInitialized = false;
        CurrentScore = 0;
    }

    private string MainMenu = "MainMenu";
    private string Gameplay = "Gameplay";

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenu);
    }

    public void GoToGameplay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Gameplay);
    }
}
