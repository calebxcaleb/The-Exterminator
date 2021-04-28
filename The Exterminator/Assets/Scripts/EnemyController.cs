using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // public
    [Range(1, 5)]
    public int size = 3;

    // private
    private GameObject player;
    private Rigidbody2D rb;
    private float force = 15.0f;
    private bool air = false;
    private bool particle = false;
    private Hashtable sizes = new Hashtable();


    void Start()
    {
        sizes.Add(1, new float[]{0.25f, 4f});
        sizes.Add(2, new float[]{0.5f, 6f});
        sizes.Add(3, new float[]{1f, 8f});
        sizes.Add(4, new float[]{1.5f, 10f});
        sizes.Add(5, new float[]{2f, 12f});
        
        this.transform.localScale = new Vector3(((float[])sizes[size])[0], ((float[])sizes[size])[0], 1);
        this.GetComponent<Rigidbody2D>().mass = ((float[])sizes[size])[1];
        for(int i = 0; i < this.transform.childCount; i++) {
            GameObject leg = this.transform.GetChild(i).gameObject;
            if (leg.tag == "leg"){
                leg.GetComponent<Rigidbody2D>().mass = ((float[])sizes[size])[1];
            }
        }
        this.GetComponent<LayEgg>().enabled = false;
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

    void changeSpriteLayer(){
        this.GetComponent<SpriteRenderer>().sortingOrder = 0;
        for(int i = 0; i < this.transform.childCount; i++) {
            GameObject leg = this.transform.GetChild(i).gameObject;
            if (leg.tag == "leg"){
                leg.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
            }
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.tag == "shoot"){            
            if (player.GetComponent<ShootingController>().air){
                air = true;
            }

            if (player.GetComponent<ShootingController>().poison){
                Destroy(this.GetComponent<Animator>());
                changeSpriteLayer();
                this.transform.DetachChildren();
                this.transform.gameObject.layer = 10;
                this.GetComponent<LayEgg>().enabled = true;
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
