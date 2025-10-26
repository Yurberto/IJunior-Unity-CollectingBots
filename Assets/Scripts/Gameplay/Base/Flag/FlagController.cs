using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField, Range(0.0f, 5.0f)] private float _surfaceOffset = 1.5f;

    private Flag _flag;

    public Flag Flag => _flag;

    private void Start()
    {
        _flag = Instantiate(_flagPrefab, transform);
        Remove();
    }

    public void PutOn(RaycastHit hit)
    {
        _flag.gameObject.SetActive(true);
        _flag.transform.position = hit.point + hit.normal * _surfaceOffset;
    }

    public void Remove()
    {
        _flag.gameObject.SetActive(false);
    }
}
