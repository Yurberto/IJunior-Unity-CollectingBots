using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private RobotSpawner _robotSpawner;

    public override void InstallBindings()
    {
        Container.Bind<Hub<Resource>>().FromInstance(_resourceSpawner.AvailableResources).AsSingle();
        Container.Bind<RobotSpawner>().FromInstance(_robotSpawner).AsSingle();
    }
}