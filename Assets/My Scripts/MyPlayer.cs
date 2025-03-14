using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
  private int bullets = 0;
  public AudioSource Shoot;
  public AudioSource Hit;
  private List<ParticleSystem> Particles = new List<ParticleSystem>();
  private bool damaged;
  public delegate void GameOver();
  public static event GameOver AllDead;

  void Start(){
    begin = lives;
    MyAliens.OnEnemyDied += EnemyOnOnEnemyDied;
    MyEnemy.empty += EnemyOnEnemyLeft;
    MyBullet.OnBullet += OnBulletDied;
    Score.text = "Score:\n" + myScore.ToString("D4");
    highScore = LoadHighScore();
    High.text = "Highscore:\n" + highScore.ToString("D4");
    Particles.AddRange(GetComponentsInChildren<ParticleSystem>());
  }

  void OnDestroy(){
    MyAliens.OnEnemyDied -= EnemyOnOnEnemyDied;
  }

  void OnBulletDied(){
    bullets--;
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
    if (damaged)
    {
      return;
    }
    float direction = Input.GetAxis("Horizontal");
    Vector3 newPosition = transform.position + new Vector3(direction, 0, 0) * speed * Time.deltaTime;
    newPosition.x = Mathf.Clamp(newPosition.x, minTravelHeight, maxTravelHeight);
    transform.position = newPosition;

    if (Input.GetKeyDown(KeyCode.Space)){
      if(bullets == 0){
        Particles[1].Play();
        bullets++;
        GameObject shot = Instantiate(bullet, shottingOffset.position, Quaternion.identity);
        Destroy(shot, 3f);
        GetComponent<Animator>().SetTrigger("Shoot");
      }
    }
  }

  void OnCollisionEnter2D(Collision2D collision){
    damaged = true;
    Hit.Play();
    Particles[1].Stop();
    Particles[0].Play();
    GetComponent<Animator>().SetTrigger("WasHit");
    Destroy(collision.gameObject);
  }

  void OnTriggerEnter2D(Collider2D col){
    damaged = true;
    Hit.Play();
    Particles[1].Stop();
    Particles[0].Play();
    GetComponent<Animator>().SetTrigger("WasHit");
    Destroy(col.gameObject);   
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

  void Death(){
    lives--;
    if(lives == 0){
      AllDead?.Invoke();
    }  
    damaged = false;
    GetComponent<Animator>().SetTrigger("GoBack");
  }
}