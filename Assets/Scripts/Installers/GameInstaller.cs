using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Zenject;
using System.Linq;

public class GameInstaller : MonoInstaller
{
    [Inject] Settings _settings = null;

    public override void InstallBindings()
    {
        // Install the main game
        InstallAsteroids();
        InstallAlien();
        InstallHook();
        InstallMisc();
        InitExecutionOrder();
    }

    private void InstallAlien()
    {
        Container.DeclareSignal<AlienCatchedSignal>();

        Container.BindFactory<Alien, Alien.Factory>()
            .FromComponentInNewPrefab(_settings.AlienPrefab)
            .WithGameObjectName("Alien");

        Container.Bind<AlienStateFactory>().AsSingle();

        Container.BindFactory<AlienStateWaiting, AlienStateWaiting.Factory>().WhenInjectedInto<AlienStateFactory>();
        Container.BindFactory<AlienStateMoving, AlienStateMoving.Factory>().WhenInjectedInto<AlienStateFactory>();
        Container.BindFactory<AlienStateCatched, AlienStateCatched.Factory>().WhenInjectedInto<AlienStateFactory>();
    }

    void InstallAsteroids()
    {
        Container.Bind<ITickable>().To<AsteroidManager>().AsSingle();
        Container.Bind<IFixedTickable>().To<AsteroidManager>().AsSingle();
        Container.Bind<AsteroidManager>().AsSingle();

        Container.BindFactory<Asteroid, Asteroid.Factory>()
            .FromComponentInNewPrefab(_settings.AsteroidPrefab)
            .WithGameObjectName("Asteroid")
            .UnderTransformGroup("Asteroids");
    }

    void InstallMisc()
    {
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        Container.Bind<LevelHelper>().AsSingle();

        Container.BindInterfacesAndSelfTo<AudioHandler>().AsSingle();

        Container.Bind<ExplosionFactory>().AsSingle().WithArguments(_settings.ExplosionPrefab);
        Container.Bind<BrokenHookFactory>().AsSingle().WithArguments(_settings.BrokenHookPrefab);
    }

    void InstallHook()
    {
        Container.DeclareSignal<HookCrashedSignal>();

        Container.Bind<HookStateFactory>().AsSingle();

        Container.BindFactory<HookStateWaiting, HookStateWaiting.Factory>().WhenInjectedInto<HookStateFactory>();
        Container.BindFactory<HookStateMoving, HookStateMoving.Factory>().WhenInjectedInto<HookStateFactory>();
        Container.BindFactory<HookStateReturning, HookStateReturning.Factory>().WhenInjectedInto<HookStateFactory>();
        Container.BindFactory<HookStateDead, HookStateDead.Factory>().WhenInjectedInto<HookStateFactory>();
    }

    void InitExecutionOrder()
    {
        // In many cases you don't need to worry about execution order,
        // however sometimes it can be important
        // If for example we wanted to ensure that AsteroidManager.Initialize
        // always gets called before GameController.Initialize (and similarly for Tick)
        // Then we could do the following:
        Container.BindExecutionOrder<AsteroidManager>(-10);
        Container.BindExecutionOrder<GameController>(-20);

        // Note that they will be disposed of in the reverse order given here
    }

    [Serializable]
    public class Settings
    {
        public GameObject AlienPrefab;
        public GameObject ExplosionPrefab;
        public GameObject BrokenHookPrefab;
        public GameObject AsteroidPrefab;
        public GameObject HookPrefab;
    }
}
