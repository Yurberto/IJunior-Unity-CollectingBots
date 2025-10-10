using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    private CarryingPoint _carryingPoint;

    private Resource _collectedResource;

    public Resource CollectedResource => _collectedResource;

    private void Awake()
    {
        _carryingPoint = GetComponentInChildren<CarryingPoint>();
    }

    public void Collect(Resource resource)
    {
        resource.OnCollect();

        resource.transform.parent = _carryingPoint.transform;
        resource.transform.position = _carryingPoint.transform.position;

        _collectedResource = resource;
    }
}
