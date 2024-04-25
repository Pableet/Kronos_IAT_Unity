using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetting : MonoBehaviour
{
	public Camera mainCam;
	public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp("Fire1"))
		{
			Debug.Log("카메라 이동");
			mainCam.transform.rotation = new Quaternion(10f, 10f, 10f, 1f) ;
		}

		

    }
}
