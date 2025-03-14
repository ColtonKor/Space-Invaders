using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensures it persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate instance
            return;
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "MainMenu")
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        MyPlayer.AllDead += SendToCredits;
    }

    void OnDestroy()
    {
        MyPlayer.AllDead -= SendToCredits;
    }

    public void StartGame()
    {
        StartCoroutine(FindPlayer());
    }

    public void StartCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void CreditReturn()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void SendToCredits()
    {
        SceneManager.LoadSceneAsync("Credits");
    }

    IEnumerator FindPlayer()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("SampleScene");
        while (!asyncOp.isDone)
        {
            yield return null;
        }
        GameObject playerObj = GameObject.Find("Player");
        // Debug.Log(playerObj);
    }
}