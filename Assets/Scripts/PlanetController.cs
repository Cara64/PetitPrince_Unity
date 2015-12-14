using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlanetController : MonoBehaviour {

    public Transform PrefabBaobab;
    public int maxSurface = 60;

    private Dictionary<BaobabInfo, Transform> baobabs = new Dictionary<BaobabInfo, Transform>();
    private List<Transform> habitants = new List<Transform>();
    private float timestamp;
    private int totalBaobabCount = 0;

	void Start () {
        timestamp = Time.time + 1;
    }
	
	void Update () {
        // Pour detruire un baobab : Destroy(baobab.gameObject);
        if(Time.time > timestamp)
        {
            SpawnBaobab();
            timestamp = Time.time + 5.0f;
        }

        // Agrandit les baobabs
        if(GetFreeSpace() > 0)
        {
            foreach (var item in baobabs)
            {
                if (Time.time > item.Key.NextUpdate && item.Key.Level < 3)
                {
                    item.Key.Level++;
                    item.Key.RefreshNextUpdate();

                    Transform baobab = item.Value;
                    baobab.localScale = baobab.localScale * 1.25f;
                }
            }
        }
    }

    void SpawnBaobab()
    {
        int numBaobabs = baobabs.Count;
        if (numBaobabs > 20)
            return;

        float scale = transform.localScale.y;
        float radius = GetComponent<SphereCollider>().radius;
        Vector3 position = transform.position + new Vector3(0, radius * scale, 0);
        bool toDestroy = false;

        // Instancie le baobab sur la sphere
        Transform baobab = Instantiate(PrefabBaobab, position, Quaternion.identity) as Transform;
        baobab.parent = transform;
        baobab.name = "Baobab " + (totalBaobabCount + 1);

        if (numBaobabs == 0)
        {
            baobab.RotateAround(transform.position, Vector3.forward, Random.Range(-180, 180));
        }
        else
        {
            List<Transform> baobabsArray = baobabs.Values.ToList();
            Vector3 previous = baobabsArray[numBaobabs - 1].transform.position;
            baobab.position = previous;

            Vector3 axis = new Vector3(
                Random.Range(-1, 1),
                Random.Range(-1, 1),
                Random.Range(-1, 1)
            ).normalized;

            baobab.RotateAround(transform.position, axis, Random.Range(25, 90));

            if (IntersectWithOtherBaobab(baobab.position))
            {
                toDestroy = true;
            }
        }

        if (toDestroy)
        {
            Destroy(baobab.gameObject);
            Debug.Log("B00M Destroyed!");
        }
        else
        {
            Vector3 gravityUp = (baobab.position - transform.position).normalized;
            Vector3 localUp = baobab.transform.up;
            baobab.rotation = Quaternion.FromToRotation(localUp, gravityUp) * baobab.rotation;

            baobabs.Add(new BaobabInfo(), baobab);
            totalBaobabCount++;

            Debug.Log("Spawned!");
        }
    }

    public float GetFreeSpace()
    {
        int sum = 0;
        foreach (var item in baobabs)
        {
            sum += item.Key.Level;
        }

        return maxSurface - sum;
    }

    public int GetNbAgents()
    {
        return habitants.Count;
    }

    bool IntersectWithOtherBaobab(Vector3 pos)
    {
        List<Transform> baobabsArray = baobabs.Values.ToList();
        for (int i = 0; i < baobabsArray.Count; i++)
        {
            if (Vector3.Distance(pos, baobabsArray[i].transform.position) < 1.5f)
                return true;
        }
        return false;
    }

    public bool TryToRemove(Transform baobab)
    {
        foreach (var item in baobabs)
        {
            if (item.Value.Equals(baobab))
            {
                Destroy(baobab.gameObject);
                baobabs.Remove(item.Key);
                return true;
            }
        }

        return false;
    }

    public void AddNewAgent(Transform agent)
    {
        habitants.Add(agent);
    }

    public void ReleaseAgent(Transform agent)
    {
        habitants.Remove(agent);
    }
}
