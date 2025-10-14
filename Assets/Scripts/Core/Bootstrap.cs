using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private RobotSpawner _robotSpawner;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private ResourceViewer _resourceViewer;

    private void Awake()
    {
        _base.Initialize(_robotSpawner, _scanner);
    }

    private void OnEnable()
    {
        _base.ResourceValueChanged += _resourceViewer.OnScoreChanged;
    }

    private void OnDisable()
    {
        _base.ResourceValueChanged -= _resourceViewer.OnScoreChanged;
    }
}
