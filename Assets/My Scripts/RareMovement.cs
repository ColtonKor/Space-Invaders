using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareMovement : MonoBehaviour
{
    private float nextMoveTime = 0f;
    private float stepSize = 0.5f;
    private Vector2 moveDirection = Vector2.right;
    private float stepInterval = .5f;
    public GameObject rarePrefab;
    private int onScreen = 0;
    private Vector3 starter;
    // Start is called before the first frame update
    void Start()
    {
        MyAliens.OnRare += EnemyOnRare;
        starter = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextMoveTime){
            nextMoveTime = Time.time + stepInterval;
            Vector3 currentDirection = moveDirection * stepSize;
            this.transform.position += new Vector3(currentDirection.x, 0, 0);
        }
        if(onScreen == 0){
            Instantiate(rarePrefab, this.transform.position, Quaternion.identity, this.transform);
            onScreen++;
        }
        if(this.transform.position.x > 15f){
            onScreen--;
            this.transform.position = starter;
        }
        if(Input.GetKeyDown(KeyCode.R)){
            onScreen--;
            this.transform.position = starter;
        }
    }

    void EnemyOnRare(int pointWorth, int rare){
        onScreen--;
        this.transform.position = starter;
    }
}
