using UnityEngine;
using Zenject;

public class Game : MonoBehaviour
{
    [SerializeField] private BaseSpawnSystem _baseSpawnSystem;
    [SerializeField] private RobotSpawnSystem _robotSpawnSystem;
    [SerializeField] private ResourceViewer _resourceViewer;

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

    private void Start()
    {
        Base firstBase = _baseSpawnSystem.Spawn(_robotSpawnSystem.Spawn(), Vector3.zero);
        _resourceViewer.Add(firstBase.ResourceMonitor);
    }

    private void HandleClick()
    {
        if (_baseClicked)
        {
            if (_raycaster.TryCast(out RaycastHit hit, LayerData.Map) == false || _distanceChecker.IsPlaceIzolated(Input.mousePosition, _clickedBase.Size, LayerData.Base) == false)
                return;

            if (_distanceChecker.IsPlaceIzolated(hit.point, _clickedBase.Size, LayerData.Base) == false)
                return;

            _clickedBase.SetFlag(hit);
            _clickedBase.RobotReachedFlag += SpawnNewBase;
            _baseClicked = false;
        }
        else
        {
            if (_raycaster.TryCast(out RaycastHit hit, LayerData.Base) == false)
                return;

            if (hit.transform.TryGetComponent(out Base @base) == false)
                return;

            if (@base.CanSetFlag == false)
                return;

            _clickedBase = @base;
            _clickedBase.OnClick();
            _baseClicked = true;
        }
    }

    private void SpawnNewBase(Robot startRobot, Vector3 spawnPosition)
    {
        _clickedBase.RobotReachedFlag -= SpawnNewBase;

        Base spawnedBase = _baseSpawnSystem.Spawn(startRobot, spawnPosition);
        _resourceViewer.Add(spawnedBase.ResourceMonitor);
    }
}
