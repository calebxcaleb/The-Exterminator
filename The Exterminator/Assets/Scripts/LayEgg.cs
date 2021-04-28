using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayEgg : MonoBehaviour
{
    // public
    public GameObject eggs;

    // private
    private int size;
    private Hashtable sizes = new Hashtable();


    void Start()
    {
        sizes.Add(1, 0.25f);
        sizes.Add(2, 0.5f);
        sizes.Add(3, 1f);
        sizes.Add(4, 1.5f);
        sizes.Add(5, 2f);

        size = this.GetComponent<EnemyController>().size;
        if (size > 1){
            size -= 1;
            GameObject eggObj = Instantiate(eggs, this.transform.position, Quaternion.identity);
            eggObj.transform.localScale = new Vector3((float)sizes[size], (float)sizes[size], 1);

            for(int i = 0; i < eggObj.transform.childCount; i++) {
                GameObject egg = eggObj.transform.GetChild(i).gameObject;
                egg.GetComponent<HatchEgg>().size = size;
            }
        }

        Destroy(this);
    }

}
