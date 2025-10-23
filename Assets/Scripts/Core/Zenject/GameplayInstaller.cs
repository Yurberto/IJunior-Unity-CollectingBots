using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private RobotSpawner _robotSpawner;

    public override void InstallBindings()
    {
        Container.Bind<ResourceSpawner>().AsSingle();
        Container.Bind<RobotSpawner>().AsSingle();
    }
}