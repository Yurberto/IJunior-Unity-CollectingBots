using UnityEngine;
using Zenject;

public class Game : MonoBehaviour
{
    [SerializeField] private BaseSpawnSystem _baseSpawnSystem;

    private DistanceChecker _distanceChecker = new();

    private InputReader _inputReader;
    private CameraRaycaster _raycaster;

    private bool _baseClicked = false;
    private Base _clickedBase = null;

    [Inject]
    private void Construct(InputReader inputReader, CameraRaycaster raycaster)
    {
        _inputReader = inputReader;
        _raycaster = raycaster;
    }

    private void OnEnable()
    {
        _inputReader.MouseClicked += HandleClick;
    }

    private void OnDisable()
    {
        _inputReader.MouseClicked -= HandleClick;
    }

    private void HandleClick()
    {
        if (_baseClicked)
        {
            if (_raycaster.TryCast(out RaycastHit hit, LayerData.Map) == false || _distanceChecker.IsPlaceIzolated(Input.mousePosition, _clickedBase.Size, LayerData.Base) == false)
                return;

            if (_distanceChecker.IsPlaceIzolated(hit.point, _clickedBase.Size, LayerData.Base) == false)
                return;

            Debug.Log(_clickedBase.Size);

            _clickedBase.SetFlag(hit);
            _baseClicked = false;
        }
        else
        {
            if (_raycaster.TryCast(out RaycastHit hit, LayerData.Base) == false)
                return;

            if (hit.transform.TryGetComponent(out Base @base) == false)
                return;

            _clickedBase = @base;
            _baseClicked = true;
        }

        _clickedBase.SwitchState();
    }
}
