using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuDemo : MonoBehaviour
{
    public TMPro.TextMeshProUGUI titleText;
    void Awake(){
        DontDestroyOnLoad(gameObject);
    }

    public void ConsoleTest(){
        // Debug.Log("ConsoleTest Invoked");
    }

    public void StartGame(){
        StartCoroutine(FindPlayer());
    }

    public void StartCredits(){
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("Credits");
    }

    IEnumerator FindPlayer(){
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("SampleScene");
        while(!asyncOp.isDone){
            yield return null;
        }
        GameObject playerObj = GameObject.Find("Player");
        Debug.Log(playerObj);
    }
}
