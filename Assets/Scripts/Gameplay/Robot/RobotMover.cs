using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class RobotMover : MonoBehaviour
{
    [SerializeField, Range(0.0f, 5.0f)] private float _reachedDistance = 0.3f;

    private Mover _mover;
    private Coroutine _moveToCoroutine;

    private void Awake()
    {
        _mover = GetComponent<Mover>(); 
    }

    public void MoveTo(Transform target)
    {
        if (_moveToCoroutine != null)
        {
            StopCoroutine(_moveToCoroutine);
            _moveToCoroutine = null;
        }

        _moveToCoroutine = StartCoroutine(MoveToTarget(target));
    }

    private void Stop()
    {
        _mover.Stop();

        StopCoroutine(_moveToCoroutine);
        _moveToCoroutine = null;
    }

    private IEnumerator MoveToTarget(Transform target)
    { 
        Vector3 lastTargetPosiotion = target.position;
        _mover.MoveTo(lastTargetPosiotion);

        while (Vector3.Distance(target.position, transform.position) > _reachedDistance)
        {
            if (lastTargetPosiotion != target.position)
            {
                _mover.MoveTo(target.position);
                lastTargetPosiotion = target.position;
            }

            yield return null;
        }

        Stop();
    }
}
