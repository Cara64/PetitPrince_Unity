using UnityEngine;
using System.Collections;

public class BaobabController : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlanetController planet = transform.GetComponentInParent<PlanetController>();
            planet.TryToRemove(transform);
        }
    }
}
