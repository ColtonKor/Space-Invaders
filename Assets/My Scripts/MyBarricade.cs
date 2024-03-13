using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBarricade : MonoBehaviour
{
    public int hitsPerBarricade = 4;
    private float sub;
    private Vector3 originalScale;
    
    void Start(){
        originalScale = this.transform.localScale;
        sub = this.transform.localScale.x/hitsPerBarricade;
    }
    
    void OnCollisionEnter2D(Collision2D collision){
        Destroy(collision.gameObject);
        this.transform.localScale = new Vector3(this.transform.localScale.x - sub, this.transform.localScale.y - sub, this.transform.localScale.z - sub);
    }

    void OnTriggerEnter2D(Collider2D col){
        Destroy(col.gameObject);
        this.transform.localScale = new Vector3(this.transform.localScale.x - sub, this.transform.localScale.y - sub, this.transform.localScale.z - sub);
    }
}
