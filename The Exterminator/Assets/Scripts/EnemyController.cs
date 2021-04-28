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

	void explode(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 5f);
        foreach (Collider2D hit in colliders) {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null){
		        var dir = rb.transform.position - this.transform.position;
				rb.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		        rb.AddForce(dir.normalized * 200.0f * rb.mass);
			}
		}
	}

    void MoveEnemy(){
        if (airParticle){
            Vector3 dir3 = player.GetComponent<ShootingController>().dir;
            Vector2 dir2 = new Vector2(dir3.x * force, dir3.y * force);
            rb.AddForce(dir2);
        }
        if (poisonParticle){
            Destroy(this.GetComponent<Animator>());
            changeSpriteLayer();
            this.transform.GetChild(0).transform.DetachChildren();
            explode();
            this.transform.gameObject.layer = 10;
            this.GetComponent<LayEgg>().enabled = true;
            this.GetComponent<DisableObject>().enabled = true;
            Destroy(this.GetComponent<EnemyController>());
        }
    }

    void changeSpriteLayer(){
        GameObject Body = this.transform.GetChild(0).gameObject;
        Body.GetComponent<SpriteRenderer>().sortingOrder = 0;
        for(int i = 0; i < Body.transform.childCount; i++) {
            GameObject bodyPart = Body.transform.GetChild(i).gameObject;
            if (bodyPart.tag == "leg"){
                bodyPart.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
            }
        }
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
