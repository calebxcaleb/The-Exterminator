using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinder : MonoBehaviour
{
    // Public
    public float speed = 20.0f;
    public bool stop = false;

    // Private
    private GameObject points;
    private GameObject player;
    private GameObject tempPoint;
    private GameObject nextPoint;
    private GameObject playerPoint;
    private List<GameObject> pointObjs = new List<GameObject>();
    private Color color;
    private bool find = false;
    private int dir = 0;

    void Start()
    {
        player = GameObject.Find("Player");
        points = GameObject.Find("Points");
        SetPoints();
    }

    void Update()
    {
        playerPoint = player.GetComponent<PlayerController>().prevPoint;
        if(!stop){
            FindPlayer();
            FollowPlayer();
        }
    }

    void SetPoints(){
        for (int i = 0; i < points.transform.childCount; i++){
            GameObject point = points.transform.GetChild(i).gameObject;
            GameObject point1 = null;
            GameObject point2 = null;

            if (i < points.transform.childCount - 1){
                point1 = points.transform.GetChild(i + 1).gameObject;
            }
            if (i > 0){
                point2 = points.transform.GetChild(i - 1).gameObject;
            }

            point.GetComponent<PathPointers>().Point12.Add(point1);
            point.GetComponent<PathPointers>().Point12.Add(point2);

            RaycastHit2D rayToPoint = Physics2D.Raycast(this.transform.position, point.transform.position - this.transform.position);
            if (Mathf.Abs((point.transform.position - this.transform.position).magnitude) < 
                Mathf.Abs((rayToPoint.transform.position - this.transform.position).magnitude)){
                tempPoint = point;
                nextPoint = point;
            }
            pointObjs.Add(point.gameObject);
        }
    }

    void FindPlayer(){
        RaycastHit2D rayToPlayer = Physics2D.Raycast(this.transform.position, player.transform.position - this.transform.position);
        if (rayToPlayer.transform.gameObject.tag == "Player"){
            color = Color.green;
            this.transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            find = false;
        }else{
            color = Color.red;
            FindInitialPoint();
            find = true;
        }
        // Debug.DrawLine(this.transform.position, player.transform.position, color, 0.1f);  
    }

    void FindInitialPoint(){
        int adder = 0;
        if(pointObjs.IndexOf(playerPoint) > pointObjs.IndexOf(tempPoint)){
            dir = 0;
            adder = 1;
        }
        else{
            dir = 1;
            adder = -1;
        }

        if (0 <= pointObjs.IndexOf(tempPoint) + adder && pointObjs.IndexOf(tempPoint) + adder <= points.transform.childCount - 1){
            GameObject checkPoint = points.transform.GetChild(pointObjs.IndexOf(tempPoint) + adder).gameObject;
            RaycastHit2D rayToPoint = Physics2D.Raycast(this.transform.position, checkPoint.transform.position - this.transform.position);
            if (Mathf.Abs((checkPoint.transform.position - this.transform.position).magnitude) < 
                Mathf.Abs((rayToPoint.transform.position - this.transform.position).magnitude)){
                    nextPoint = checkPoint;
            }
        }

    }

    void FollowPlayer(){
        if (find){
            this.transform.position = Vector2.MoveTowards(this.transform.position, nextPoint.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.tag == "point"){
            if(find){
                nextPoint = col.gameObject.GetComponent<PathPointers>().Point12[dir];
            }else{
                nextPoint = tempPoint;
            }
            tempPoint = col.gameObject;
        }
    }
}