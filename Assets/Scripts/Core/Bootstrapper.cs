using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private ResourceViewer _resourceViewer;

    private void OnEnable()
    {
        _base.ResourceValueChanged += _resourceViewer.OnScoreChanged;
    }

    private void OnDisable()
    {
        _base.ResourceValueChanged -= _resourceViewer.OnScoreChanged;
    }
}
