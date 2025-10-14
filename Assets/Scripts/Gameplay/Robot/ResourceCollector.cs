using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    private PickUpPoint _pickUpPoint;

    private void Awake()
    {
        _pickUpPoint = GetComponentInChildren<PickUpPoint>();
    }

    public void PickUp(Resource resource)
    {
        resource.transform.parent = _pickUpPoint.transform;
        resource.transform.position = _pickUpPoint.transform.position;
    }
}
