using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField, Range(0.0f, 500.0f)] private float _speed = 5.0f;
    
    private float _speedMultiplier = 1000.0f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;

        _rigidbody.velocity = direction.normalized * (_speed * _speedMultiplier * Time.deltaTime);
    }

    public void Stop()
    {
        _rigidbody.velocity = Vector3.zero;
    }
}
