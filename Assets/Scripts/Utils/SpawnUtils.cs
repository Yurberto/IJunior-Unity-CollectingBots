using System.Collections.Generic;
using UnityEngine;

public static class SpawnUtils
{
    public static Vector3 GetSpawnPosition(List<Vector3> availableSpawpoints)
    {
        int randomIndex = Random.Range(0, availableSpawpoints.Count);

        Vector3 spawnpoint = availableSpawpoints[randomIndex];
        availableSpawpoints.RemoveAt(randomIndex);

        return spawnpoint;
    }
}

