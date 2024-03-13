using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAliens : MonoBehaviour
{
    public float duration = 5f;
    public int rarePoints = 200;
    public int topPoints = 50;
    public int middlePoints = 30;
    public int bottomPoints = 15;
    public GameObject bullet;
    public float shootingInterval = 2f;
    public Vector3 bulletOffset = new Vector3(0f, -1f, 0f);
    public delegate void EnemyDied(int pointWorth, int rare);
    public static event EnemyDied OnEnemyDied;
    public static event EnemyDied OnDeath;
    public delegate void RareDied();
    public static event EnemyDied OnRare;
    private bool isShooting = false;
    private static MyAliens currentShootingAlien;
    private static List<MyAliens> potentialShooters = new List<MyAliens>();
    private int currentPoints;
    public AudioSource ShootSound;
    public AudioSource Hit;
    private List<ParticleSystem> Particles = new List<ParticleSystem>();
    

    void Start(){
        Particles.AddRange(GetComponentsInChildren<ParticleSystem>());
        potentialShooters.Add(this);
        if (currentShootingAlien == null){
            currentShootingAlien = this;
            StartCoroutine(ManageShooting());
        }
    }

    void Update(){
        if (currentShootingAlien == null){
            currentShootingAlien = this;
            StartCoroutine(ManageShooting());
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Mine")){
            GetComponent<Animator>().SetBool("Hit", true);
            Particles[1].Stop();
            Particles[0].Play();
            Destroy(col.gameObject);
            Hit.Play();
            if(this.CompareTag("Rare")){
                currentPoints = rarePoints;
                OnEnemyDied.Invoke(currentPoints, rarePoints);
            } else if (this.CompareTag("Top")){
                currentPoints = topPoints;
                OnEnemyDied.Invoke(currentPoints, rarePoints);
            } else if (this.CompareTag("Middle")){
                currentPoints = middlePoints;
                OnEnemyDied.Invoke(currentPoints, rarePoints);
            } else if (this.CompareTag("Bottom")){
                currentPoints = bottomPoints;
                OnEnemyDied.Invoke(currentPoints, rarePoints);
            }
        }
    }

    void DeathAnimationComplete(){
        if(this.CompareTag("Rare")){
            OnRare.Invoke(currentPoints, rarePoints);
        }
        Destroy(gameObject);
        OnDeath.Invoke(currentPoints, rarePoints);
    }

    IEnumerator ManageShooting(){
        while (potentialShooters.Count > 0){
            if (currentShootingAlien == null || !currentShootingAlien.isShooting){
                int shooterIndex = Random.Range(0, potentialShooters.Count);
                currentShootingAlien = potentialShooters[shooterIndex];
                StartCoroutine(currentShootingAlien.Shoot());
            }
            yield return new WaitForSeconds(shootingInterval);
        }
    }

    IEnumerator Shoot(){
        isShooting = true;
        ShootBullet();
        yield return new WaitForSeconds(shootingInterval);
        isShooting = false;
    }

    void ShootBullet(){
        Vector3 shootingPosition = transform.position + bulletOffset;
        GameObject shot = Instantiate(bullet, shootingPosition, Quaternion.identity);
        Destroy(shot, 3f);
        ShootSound.Play();
        Particles[1].Play();
    }

    void OnDestroy(){
        isShooting = false;
        potentialShooters.Remove(this);
    }
}
