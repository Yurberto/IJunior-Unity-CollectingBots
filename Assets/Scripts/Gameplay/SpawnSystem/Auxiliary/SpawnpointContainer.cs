using System.Collections.Generic;
using UnityEngine;

public class SpawnpointContainer : MonoBehaviour
{
    private List<Vector3> _spawnpoints = new();

    public List<Vector3> Spawnpoints => _spawnpoints;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _spawnpoints.Add(transform.GetChild(i).position);
        }
    }
}
