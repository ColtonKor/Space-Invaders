using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class MyBullet : MonoBehaviour
{
  private Rigidbody2D myRigidbody2D;

  public float speed = 5;
  public delegate void BulletDied();
  public static event BulletDied OnBullet;
  // Start is called before the first frame update
  void Start(){
    myRigidbody2D = GetComponent<Rigidbody2D>();
    Fire();
  }

  // Update is called once per frame
  private void Fire(){
    myRigidbody2D.velocity = Vector2.up * speed; 
  }

  void OnCollisionEnter2D(Collision2D collision){
    Destroy(this.gameObject);
  }
  void OnDestroy(){
    OnBullet.Invoke();
  }

}
