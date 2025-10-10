using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)] private float _speed = 1.0f;

    private float _speedMultiplier = 1000.0f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        Vector3 direction = new Vector3(targetPosition.x - transform.position.x, 0f, targetPosition.z - transform.position.z).normalized;

        _rigidbody.velocity = direction * (_speed * _speedMultiplier);
    }

    public void Stop()
    {
        _rigidbody.velocity = Vector3.zero;
    }
}
