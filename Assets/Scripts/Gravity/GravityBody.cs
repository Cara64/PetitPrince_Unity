using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {
	
	GravityAttractor planet;

	void Awake () {
		planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
    }
	
	void FixedUpdate () {
		planet.Attract(GetComponent<Rigidbody>());
	}

    public void GetAttracted(GravityAttractor newPlanet)
    {
        planet = newPlanet;
    }
}