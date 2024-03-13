using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuDemo : MonoBehaviour
{
    // public TMPro.TextMeshProUGUI titleText;
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
        SceneManager.LoadScene("Credits");
        // StartCoroutine(FindCredits());
    }

    IEnumerator FindPlayer(){
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("SampleScene");
        while(!asyncOp.isDone){
            yield return null;
        }
        GameObject playerObj = GameObject.Find("Player");
        Debug.Log(playerObj);
    }


    // IEnumerator FindCredits(){
    //     AsyncOperation asyncOp = SceneManager.LoadSceneAsync("Credits");
    //     while(!asyncOp.isDone){
    //         yield return null;
    //     }
    //     GameObject playerObj = GameObject.Find("Back");
    //     Debug.Log(playerObj);
    // }

}
