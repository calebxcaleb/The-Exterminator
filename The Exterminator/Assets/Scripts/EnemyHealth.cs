using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Public
    public int health = 5;

    // Private
    private int recoverTime = 2;
    private bool waiting = false;
    private List<GameObject> legs = new List<GameObject>();

    void Start()
    {
        
    }

    void Update()
    {
        if (health <= 0){
            Die();
        }
        else if(!waiting && this.GetComponent<EnemyController>().damage){
            StartCoroutine(damage());
        }
    }

    void Die(){
        Destroy(this.GetComponent<Animator>());
        Destroy(this.GetComponent<EnemyPathFinder>());
        changeSpriteLayer();
        detachLegs();
        this.transform.GetChild(0).transform.DetachChildren();
        explode();
        deleteLegs();
        this.gameObject.layer = 2;
        this.GetComponent<LayEgg>().enabled = true;
        this.GetComponent<DisableObject>().enabled = true;
        Destroy(this.GetComponent<EnemyController>());
        Destroy(this.GetComponent<EnemyHealth>());
    }

    void detachLegs(){
        GameObject body = this.transform.GetChild(0).gameObject;
        for(int i = 0; i < body.transform.childCount; i++){
            GameObject child = body.transform.GetChild(i).gameObject;
            if(child.tag == "leg"){
                child.GetComponent<DetachLeg>().detatch();
                legs.Add(child);
            }
        }
    }

    void deleteLegs(){
        for(int i = 0; i < legs.Count; i++){
            GameObject child = legs[i];
            if(child.tag == "leg"){
                child.GetComponent<DetachLeg>().delete();
            }
        }
    }

	void explode(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 5f);
        foreach (Collider2D hit in colliders) {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null){
		        var dir = rb.transform.position - this.transform.position;
				rb.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		        rb.AddForce(dir.normalized * 300.0f * rb.mass);
			}
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

    IEnumerator damage(){
        waiting = true;
        yield return new WaitForSeconds(recoverTime);
        health -= 1;
        waiting = false;
    }

}
