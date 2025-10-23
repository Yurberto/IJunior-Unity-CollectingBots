using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private Robot _robot;
    [SerializeField] private BaseSpawner _baseSpawner;
    [SerializeField] private RobotSpawner _robotSpawner;
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private ResourceViewer _resourceViewer;

    public override void InstallBindings()
    {
        Container.Bind<Hub<Resource>>().FromInstance(_resourceSpawner.AvailableResources).AsSingle();
        Container.Bind<RobotSpawner>().FromInstance(_robotSpawner).AsSingle();
        Container.Bind<BaseSpawner>().FromInstance(_baseSpawner).AsSingle();
        Container.Bind<Robot>().FromInstance(_robot).AsSingle();
        Container.Bind<ResourceViewer>().FromInstance(_resourceViewer).AsSingle();

        Container.BindInterfacesAndSelfTo<InputReader>().AsSingle();
    }
}