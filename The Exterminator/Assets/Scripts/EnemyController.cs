﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // public

    // private
    private GameObject player;
    private Rigidbody2D rb;
    private float force = 15.0f;
    private bool air = false;
    private bool particle = false;

    void Start()
    {
        this.GetComponent<DisableObject>().enabled = false;
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy(){
        if (particle){
            Vector3 dir3 = player.GetComponent<ShootingController>().dir;
            Vector2 dir2 = new Vector2(dir3.x * force, dir3.y * force);
            rb.AddForce(dir2);
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.tag == "shoot"){            
            if (player.GetComponent<ShootingController>().air){
                air = true;
            }

            if (player.GetComponent<ShootingController>().poison){
                Destroy(this.GetComponent<Animator>());
                this.transform.DetachChildren();
                this.transform.gameObject.layer = 10;
                this.GetComponent<DisableObject>().enabled = true;
                Destroy(this.GetComponent<EnemyController>());
            }

        }
    }

    void OnTriggerExit2D(Collider2D col){
        if (col.tag == "shoot"){
            air = false;
            particle = false;
        }
    }

    void OnParticleCollision(GameObject other){
        if (air){
            particle = true;
        }
    }
}