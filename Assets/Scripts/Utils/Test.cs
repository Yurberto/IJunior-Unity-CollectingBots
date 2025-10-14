using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private RobotSpawner _robotSpawner;
    [SerializeField] private Scanner _scanner;

    private void Awake()
    {
        _base.Initialize(_robotSpawner, _scanner);
    }
}
