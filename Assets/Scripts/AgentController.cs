using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentController : MonoBehaviour {

    public GameObject planets;
    public float movementSpeed;

    private List<Transform> _planetsPositions;
    private Transform currentPlanet;

    private enum State { STRAY, MOVING, FIXED };
    private State _state;

    private Vector3 _movementDirection;
    private Vector3 _currentTarget;

	// Use this for initialization
	void Start () {
        // Ajoute les planetes a la liste interne de l'agent
        _planetsPositions = new List<Transform>();

        foreach (Transform pos in planets.transform)
            _planetsPositions.Add(pos);

        _state = State.STRAY;
	}
	
	void Update () {
        switch (_state)
        {
            case State.STRAY:
                UpdateStray();
                break;
            case State.MOVING:
                UpdateMoving();
                break;
            case State.FIXED:
                UpdateFixed();
                break;
            default:
                break;
        }
    }

    void UpdateStray()
    {
        // Choix d'une planete au hasard
        int i = Random.Range(0, _planetsPositions.Count);

        // Calcul les vecteurs necessaire au deplacement
        _currentTarget = _planetsPositions[i].position;
        _movementDirection = (_currentTarget - transform.position).normalized;

        _state = State.MOVING;
    }

    void UpdateMoving()
    {
        transform.Translate(_movementDirection * movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _currentTarget) < 5)
        {
            // Destination atteinte
            // Prise de decision
            // Changer vers le nouvel etat

            _state = State.STRAY;
        }
    }

    void UpdateFixed()
    {
        if (currentPlanet == null) return;

        PlanetController planet = currentPlanet.GetComponent<PlanetController>();
        float freeSpace = planet.GetFreeSpace();

        if(freeSpace <= 0)
        {
            planet.ReleaseAgent(transform);
            _state = State.STRAY;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Planet")
        {
            // prise de decision
            PlanetController planet = other.GetComponent<PlanetController>();
            bool should_i_stay_or_should_i_go = (Random.value < 0.5) ? true : false;

            if (should_i_stay_or_should_i_go && planet.GetFreeSpace() >= 0)
            {
                planet.AddNewAgent(transform);
                currentPlanet = other.gameObject.transform;

                transform.position = other.transform.position;
                _state = State.FIXED;
            }
        }
    }
}
