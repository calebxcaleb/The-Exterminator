using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    // Public
    public GameObject airParticleObj;
    public GameObject poisonParticleObj;
    public Camera SceneCamera;
    public Vector3 dir;
    public bool air = false;
    public bool poison = false;

    // Private
    private ParticleSystem airParticle;
    private ParticleSystem poisonParticle;
    private Vector3 mousePos;
    private bool confined = true;
    private float angle;

    void Start()
    {
        airParticle = airParticleObj.GetComponent<ParticleSystem>();
        poisonParticle = poisonParticleObj.GetComponent<ParticleSystem>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        GetMouseClick();
        Emit();
        RotateEmission();
    }

	void MouseLock(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (!confined){
				confined = true;
				Cursor.lockState = CursorLockMode.Confined;
			}
			else{
				confined = false;
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}

    void GetMouseClick(){
        if (Input.GetMouseButton(0)){
            air = true;
            poison = false;
        }
        else if (Input.GetMouseButton(1)){
            air = false;
            poison = true;
        }
        else{
            air = false;
            poison = false;
        }
    }

    void Emit(){
        if (air){
            if (!airParticle.isPlaying) airParticle.Play();
            if (poisonParticle.isPlaying) poisonParticle.Stop();
        }
        else if(poison){
            if (airParticle.isPlaying) airParticle.Stop();
            if (!poisonParticle.isPlaying) poisonParticle.Play();
        }
        else{
            if (airParticle.isPlaying) airParticle.Stop();
            if (poisonParticle.isPlaying) poisonParticle.Stop();
        }
    }

    void RotateEmission(){
        dir = new Vector3(mousePos.x, mousePos.y, 0) - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        airParticle.transform.rotation = Quaternion.Euler(0, 0, angle);
        poisonParticle.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnGUI(){
		mousePos = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
	}
}
