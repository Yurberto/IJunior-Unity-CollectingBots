using System;
using System.Collections;
using UnityEngine;

public class BaseResourceScaner : MonoBehaviour
{
    [SerializeField, Range(0.0f, 10.0f)] private float _detectDelay = 1.0f;

    private Scanner _scanner;

    private Coroutine _detectCoroutine;

    public event Action<Resource> NearestDetected;

    public void Initialize(Scanner scanner)
    {
        _scanner = scanner;
    }

    public void StartDetect()
    {
        _detectCoroutine = StartCoroutine(DetectCoroutine());

    }

    private bool TryFindNearest(out Resource nearestResource)
    {
        if (_scanner.TryScanResources(out Resource[] resources))
        {
            float nearestDisance = Vector3.Distance(transform.position, resources[0].transform.position);
            nearestResource = resources[0];

            for (int i = 1; i < resources.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, resources[i].transform.position);

                if (distance < nearestDisance)
                {
                    nearestResource = resources[i];
                    nearestDisance = distance;
                }
            }

            return true;
        }

        nearestResource = null;
        return false;
    }

    private IEnumerator DetectCoroutine()
    {
        var wait = new WaitForSeconds(_detectDelay);

        while (enabled)
        {
            if (TryFindNearest(out Resource nearestResource))
                NearestDetected?.Invoke(nearestResource);

            yield return wait;
        }
    }
}
