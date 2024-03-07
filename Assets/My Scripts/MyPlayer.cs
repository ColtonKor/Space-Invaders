using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyPlayer : MonoBehaviour
{
  public GameObject bullet;
  public TextMeshProUGUI Score;
  public TextMeshProUGUI High;
  public Transform shottingOffset;
  public float speed = 5f;
  private float minTravelHeight = -9f;
  private float maxTravelHeight = 9f;
  private int myScore = 0;
  private int highScore = 0;
  public int lives = 3;
  private int begin;

  void Start(){
    begin = lives;
    MyAliens.OnEnemyDied += EnemyOnOnEnemyDied;
    MyEnemy.empty += EnemyOnEnemyLeft;
    Score.text = "Score:\n" + myScore.ToString("D4");
    highScore = LoadHighScore();
    High.text = "Highscore:\n" + highScore.ToString("D4");
  }

  void OnDestroy(){
    MyAliens.OnEnemyDied -= EnemyOnOnEnemyDied;
  }


  void EnemyOnOnEnemyDied(int pointWorth, int rare){
    myScore += pointWorth;
    Score.text = "Score:\n" + myScore.ToString("D4");
    SaveHighScore(myScore);
    High.text = "HighScore:\n" + LoadHighScore().ToString("D4");
  }

  void EnemyOnEnemyLeft(){
    lives++;
  }

    // Update is called once per frame
  void Update(){
    float direction = Input.GetAxis("Horizontal");
    Vector3 newPosition = transform.position + new Vector3(direction, 0, 0) * speed * Time.deltaTime;
    newPosition.x = Mathf.Clamp(newPosition.x, minTravelHeight, maxTravelHeight);
    transform.position = newPosition;

    if (Input.GetKeyDown(KeyCode.Space)){
      GameObject shot = Instantiate(bullet, shottingOffset.position, Quaternion.identity);
      // bullet++;
      Destroy(shot, 3f);
    }

    if(Input.GetKeyDown(KeyCode.R)){
      gameObject.transform.position = new Vector3(0f, -3.028f, 0f);
      myScore = 0;
      Score.text = "Score:\n" + myScore.ToString("D4");
      lives = begin;
    }
  }

  void OnCollisionEnter2D(Collision2D collision){
    lives--;
    Destroy(collision.gameObject);
    if(lives == 0){
      gameObject.transform.position = new Vector3(100f, 100f, 0f);
    }
  }

  void OnTriggerEnter2D(Collider2D col){
    lives--;
    Destroy(col.gameObject);
    if(lives == 0){
      gameObject.transform.position = new Vector3(100f, 100f, 0f);
    }    
  }

  public static void SaveHighScore(int score){
    int highScore = PlayerPrefs.GetInt("HighScore", 0);
    if(score > highScore){
      PlayerPrefs.SetInt("HighScore", score);
      PlayerPrefs.Save();
    }
  }
  
  public static int LoadHighScore(){
    return PlayerPrefs.GetInt("HighScore", 0);
  }
}