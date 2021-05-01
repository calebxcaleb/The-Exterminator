using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // public
    [Range(1, 5)]
    public int size = 3;
    public bool damage = false;

    // private
    private GameObject player;
    private Rigidbody2D rb;
    private float force = 15.0f;
    private bool air = false;
    private bool poison = false;
    private bool airParticle = false;
    private bool poisonParticle = false;
    private Hashtable sizes = new Hashtable();


    void Start()
    {
        sizes.Add(1, new float[]{0.25f, 2f});
        sizes.Add(2, new float[]{0.5f, 4f});
        sizes.Add(3, new float[]{1f, 6f});
        sizes.Add(4, new float[]{1.5f, 8f});
        sizes.Add(5, new float[]{2f, 10f});
        
        this.transform.localScale = new Vector3(((float[])sizes[size])[0], ((float[])sizes[size])[0], 1);
        this.GetComponent<Rigidbody2D>().mass = ((float[])sizes[size])[1];
        GameObject Body = this.transform.GetChild(0).gameObject;
        for(int i = 0; i < Body.transform.childCount; i++) {
            GameObject leg = Body.transform.GetChild(i).gameObject;
            if (leg.tag == "leg"){
                leg.GetComponent<DetachLeg>().mass = ((float[])sizes[size])[1];
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
        if (airParticle){
            this.GetComponent<EnemyPathFinder>().stop = true;
            Vector3 dir3 = player.GetComponent<ShootingController>().dir;
            Vector2 dir2 = new Vector2(dir3.x * force, dir3.y * force);
            rb.AddForce(dir2);
        }
        else{
            this.GetComponent<EnemyPathFinder>().stop = false;
        }
        
        damage = poisonParticle;
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.tag == "shoot"){            
            if (player.GetComponent<ShootingController>().air){
                air = true;
            }

            if (player.GetComponent<ShootingController>().poison){
                poison = true;
            }

        }
    }

    void OnTriggerExit2D(Collider2D col){
        if (col.tag == "shoot"){
            air = false;
            poison = false;
            airParticle = false;
            poisonParticle = false;
        }
    }

    void OnParticleCollision(GameObject other){
        if (air){
            airParticle = true;
        }
        if(poison){
            poisonParticle = true;
        }
    }
}
