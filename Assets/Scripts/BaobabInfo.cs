using UnityEngine;
using System.Collections;

public class BaobabInfo {
    public int Level { get; set; }
    public float NextUpdate { get; set; }

    public BaobabInfo()
    {
        Level = 1;
        RefreshNextUpdate();
    }

    public void RefreshNextUpdate()
    {
        NextUpdate = Time.time + Random.Range(5, 10);
    }
}
