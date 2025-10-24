using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField, Range(0.1f, 1000.0f)] private float _castDistance = 300;

    public bool TryCast(out RaycastHit hit, LayerMask castMask)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, _castDistance, castMask);
    }
}
