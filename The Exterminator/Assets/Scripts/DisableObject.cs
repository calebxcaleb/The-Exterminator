using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Delay());
    }

    void Update()
    {
        
    }

    IEnumerator Delay(){
        yield return new WaitForSeconds(10);
        Destroy(this.GetComponent<BoxCollider2D>());
        Destroy(this.GetComponent<Rigidbody2D>());
        Destroy(this.GetComponent<DisableObject>());
    }
}
