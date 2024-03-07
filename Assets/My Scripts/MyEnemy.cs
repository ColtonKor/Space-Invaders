using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    public int columns = 11;
    public float spacing = 1.5f;
    public GameObject topPrefab;
    public GameObject middlePrefab;
    public GameObject bottomPrefab;
    private int rows = 3;
    public static int alienCount = 0;
    private Vector2 moveDirection = Vector2.right;
    public float moveSpeed = 1.0f;
    public float descentStep = 0.5f;
    public float stepInterval = 1.0f;
    public float stepSize = 0.5f;
    public float nextMoveTime = 0f;
    private float boundaryLeft = -9f;
    private float boundaryRight = 9f;
    private Vector3 startPoint;
    public delegate void EnemyLeft();
    public static event EnemyLeft empty;
    
    private void Start(){
        SpawnAliens();
        startPoint = this.transform.position;
        MyAliens.OnDeath += EnemyOnOnDeath;
    }


    void EnemyOnOnDeath(int pointWorth, int rare){
        if(pointWorth != rare){
            alienCount--;
            stepInterval -= .025f;
        }
    }

    void Update(){
        if (Time.time >= nextMoveTime){
            MoveAliens();
            nextMoveTime = Time.time + stepInterval;
        }
        if(alienCount < 1){
            empty.Invoke();
            DestroyAliens();
            stepInterval = 1f;
            this.transform.position = startPoint;
            SpawnAliens();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            DestroyAliens();
            this.transform.position = startPoint;
            SpawnAliens();
        }
    }

    void SpawnAliens(){
        Vector3 startPosition = this.transform.position;
        float totalWidth = (columns - 1) * spacing;
        float totalHeight = (rows - 1) * spacing;
        startPosition.x -= totalWidth / 2;
        startPosition.y += totalHeight / 2;
        for (int row = 0; row < rows; row++){
            for (int col = 0; col < columns; col++){
                Vector3 position = startPosition + new Vector3(col * spacing, -row * spacing, 0);
                if(row == 0){
                    Instantiate(topPrefab, position, Quaternion.identity, this.transform);
                } else if (row == 1){
                    Instantiate(middlePrefab, position, Quaternion.identity, this.transform);
                } else {
                    Instantiate(bottomPrefab, position, Quaternion.identity, this.transform);
                }
                alienCount++;
            }
        }
    }

    void MoveAliens(){
        bool shouldMoveDown = false;
        Vector3 currentDirection = moveDirection * stepSize;

        foreach (Transform child in transform){
            if ((child.position.x + currentDirection.x < boundaryLeft && moveDirection.x < 0) ||
                (child.position.x + currentDirection.x > boundaryRight && moveDirection.x > 0)){
                shouldMoveDown = true;
                break;
            }
        }

        if(shouldMoveDown){
            transform.position += Vector3.down * stepSize;
            moveDirection.x = -moveDirection.x;
        } else {
            transform.position += new Vector3(currentDirection.x, 0, 0);
        }
    }
    void DestroyAliens() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        alienCount = 0;
    }
}
