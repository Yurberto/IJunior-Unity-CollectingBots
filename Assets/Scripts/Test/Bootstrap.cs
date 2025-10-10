using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Robot _robot;
    [SerializeField] private Scanner _scanner;

    private void Awake()
    {
        Debug.Log("Bootstrap: Awake started");
        _base.Initialize(_robot, _scanner);
        Debug.Log("Bootstrap: Awake finished");
    }
}
