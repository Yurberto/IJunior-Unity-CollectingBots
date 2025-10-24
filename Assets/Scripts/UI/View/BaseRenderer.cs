using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BaseRenderer : MonoBehaviour
{
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _clickedColor = Color.red;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _renderer.material.color = _defaultColor;
    }

    public void SwitchColor()
    {
        _renderer.material.color = (_renderer.material.color == _defaultColor) ? _clickedColor : _defaultColor;
    }
}
