using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchEgg : MonoBehaviour
{
    // public
    public GameObject enemy;
    public int size;

    // private
    private Animator anim;


    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("hatched")){
            Color newCol = this.GetComponent<SpriteRenderer>().color;
            newCol.a = 0.5f;
            this.GetComponent<SpriteRenderer>().color = newCol;
            GameObject enemyObj = Instantiate(enemy, this.transform.position, Quaternion.identity);
            enemyObj.GetComponent<EnemyController>().size = size;
            Destroy(this);
        }
    }
}
