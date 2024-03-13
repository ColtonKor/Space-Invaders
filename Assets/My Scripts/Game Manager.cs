using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    void Start(){
        MyPlayer.AllDead += SendToCredits;
    }
    void Awake(){
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "MainMenu"){
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartGame(){
        StartCoroutine(FindPlayer());
    }

    public void StartCredits(){
        SceneManager.LoadScene("Credits");
    }

    public void CreditReturn(){
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void SendToCredits(){
        SceneManager.LoadSceneAsync("Credits");
    }

    IEnumerator FindPlayer(){
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("SampleScene");
        while(!asyncOp.isDone){
            yield return null;
        }
        GameObject playerObj = GameObject.Find("Player");
        // Debug.Log(playerObj);
    }
}
