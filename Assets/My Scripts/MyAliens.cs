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
    private int currentPoints;

    void Start(){
        if (this.CompareTag("Rare") && currentShootingAlien == null){
            currentShootingAlien = this;
            StartCoroutine(Shoot());
        }
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Mine")){
            GetComponent<Animator>().SetBool("Hit", true);
            Destroy(col.gameObject);
            if(this.CompareTag("Rare")){
                currentPoints = rarePoints;
                OnEnemyDied.Invoke(currentPoints, rarePoints);
                OnRare.Invoke(currentPoints, rarePoints);
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
        Destroy(gameObject);
        OnDeath.Invoke(currentPoints, rarePoints);
    }

    IEnumerator Shoot(){
        isShooting = true;
        while (isShooting){
            if (currentShootingAlien == this){
                ShootBullet();
                yield return new WaitForSeconds(shootingInterval);
            } else {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void ShootBullet(){
        Vector3 shootingPosition = transform.position + bulletOffset;
        Instantiate(bullet, shootingPosition, Quaternion.identity);
    }

    void OnDestroy(){
        isShooting = false;
    }
}
