﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private float speed = 7.0f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Control();
    }

	void Control(){
		Vector3 playerPos = transform.position;
		float x_move = Input.GetAxisRaw("Horizontal");
		float y_move = Input.GetAxisRaw("Vertical");

		Vector2 direction = new Vector2(x_move, y_move).normalized;

		rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
	}

}
