using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 15;
    private Vector3 moveDir;

    private Transform clickedObject;

	void Update ()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                clickedObject = hit.transform;
            }
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveDir * moveSpeed * Time.deltaTime));
    }

    void OnGUI()
    {
        if (clickedObject == null) return;

        if(clickedObject.tag == "Planet")
        {
            PlanetController planet = clickedObject.GetComponent<PlanetController>();
            GravityBody physicBody = GetComponent<GravityBody>();
            GUI.Label(new Rect(10, 10, 200, 110), clickedObject.name, "box");
            GUI.Label(new Rect(20, 30, 200, 100), "Place restante : " + planet.GetFreeSpace());
            GUI.Label(new Rect(20, 50, 200, 100), "Nb agents : " + planet.GetNbAgents());
            if (GUI.Button(new Rect(20, 80, 180, 30), "S'y envoler"))
                physicBody.GetAttracted(clickedObject.GetComponent<GravityAttractor>());
        }
    }
}
