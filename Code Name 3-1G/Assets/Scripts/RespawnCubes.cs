using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCubes : MonoBehaviour
{
	Vector3 pos = new Vector3(3, 3, 3);
	
	public Transform effect;
	
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		
		
        if(transform.position.y < -20f)
        {
            transform.position = pos;
			Debug.Log("I'm back");
			
			//effect.GetComponent<ParticleSystem>().enableEmission = true;
			//StartCoroutine(stopEffect ());
        }
		
    }
	
	IEnumerator stopEffect ()
	{
		yield return new WaitForSeconds (1f);
		effect.GetComponent<ParticleSystem>().enableEmission = false;
	}
}
