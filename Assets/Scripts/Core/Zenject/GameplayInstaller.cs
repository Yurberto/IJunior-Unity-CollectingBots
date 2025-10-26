using UnityEngine;
using Zenject;

public sealed class GameplayInstaller : MonoInstaller
{
    [SerializeField] private ResourceSpawnSystem _resourceSpawnSystem;
    [SerializeField] private RobotSpawnSystem _robotSpawnSystem;
    [SerializeField] private ResourceViewer _resourceViewer;
    [SerializeField] private CameraRaycaster _raycaster;

    public override void InstallBindings()
    {
        Container.Bind<Hub<Resource>>().FromInstance(_resourceSpawnSystem.AvailableResources).AsSingle();
        Container.Bind<ResourceViewer>().FromInstance(_resourceViewer).AsSingle();
        Container.Bind<RobotSpawnSystem>().FromInstance(_robotSpawnSystem).AsSingle();
        Container.Bind<CameraRaycaster>().FromInstance(_raycaster).AsSingle();


        Container.BindInterfacesAndSelfTo<InputReader>().FromNew().AsSingle();
    }
}