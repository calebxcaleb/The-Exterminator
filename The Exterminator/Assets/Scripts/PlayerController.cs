using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public
    public GameObject prevPoint;

    // Private
    private Rigidbody2D rb;
    private GameObject points;
    public float speed = 10.0f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        points = GameObject.Find("Points");
        getStartPoint();
    }

    void Update()
    {
        Control();
    }

    void getStartPoint(){
        for (int i = 0; i < points.transform.childCount; i++){
            GameObject point = points.transform.GetChild(i).gameObject;

            LayerMask mask = LayerMask.GetMask("Enemy");
            RaycastHit2D rayToPoint = Physics2D.Raycast(this.transform.position, point.transform.position - this.transform.position, mask);
            if (Mathf.Abs((point.transform.position - this.transform.position).magnitude) < 
                Mathf.Abs((rayToPoint.transform.position - this.transform.position).magnitude)){
                prevPoint = point;
                break;
            }
        }
    }

	void Control(){
		Vector3 playerPos = transform.position;
		float x_move = Input.GetAxisRaw("Horizontal");
		float y_move = Input.GetAxisRaw("Vertical");

		Vector2 direction = new Vector2(x_move, y_move).normalized;

		rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
	}

    void OnTriggerStay2D(Collider2D col){
        if (col.tag == "point"){
            prevPoint = col.gameObject;
        }
    }

}
