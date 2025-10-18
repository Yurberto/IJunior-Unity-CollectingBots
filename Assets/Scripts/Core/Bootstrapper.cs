using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private RobotSpawner _robotSpawner;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private ResourceViewer _resourceViewer;

    private void Awake()
    {
        _base.Initialize(_robotSpawner, _resourceSpawner.Hub);
        _resourceScanner.Initialize(_resourceSpawner.Hub);
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
