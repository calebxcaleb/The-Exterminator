using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachLeg : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<DisableObject>().enabled = false;
    }

    void Update()
    {
        if (this.transform.parent == null){
            Destroy(this.GetComponent<HingeJoint2D>());
            this.GetComponent<BoxCollider2D>().isTrigger = false;
            this.GetComponent<DisableObject>().enabled = true;
            Destroy(this.GetComponent<DetachLeg>());
        }
    }
}
