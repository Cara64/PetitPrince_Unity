using UnityEngine;
using System.Collections;

public class ClickButterfly : MonoBehaviour
{
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    GameObject butterfly = GameObject.FindGameObjectWithTag("Butterfly");
    void Start()
    {
        butterfly.AddComponent<FixedJoint>();
        butterfly.GetComponent<FixedJoint>().connectedBody = player.GetComponent<Rigidbody>();
    }
}
