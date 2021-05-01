using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachLeg : MonoBehaviour
{
    // Public
    public float mass;

    void Start()
    {

    }

    void Update()
    {

    }

    public void detatch(){
        Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.drag = 10;
        rb.angularDrag = 0.1f;
        rb.gravityScale = 0;
        this.GetComponent<BoxCollider2D>().isTrigger = false;
        this.GetComponent<DisableObject>().enabled = true;
        this.gameObject.layer = 8;
    }

    public void delete(){
        this.gameObject.layer = 2;
        Destroy(this.GetComponent<DetachLeg>());
    }
}
