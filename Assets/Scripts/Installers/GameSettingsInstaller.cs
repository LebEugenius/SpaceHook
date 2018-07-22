using System;
using UnityEngine;
using System.Collections;
using Zenject;

[CreateAssetMenu(menuName = "Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public HookSettings Hook;
    public AsteroidSettings Asteroid;
    public AudioHandler.Settings AudioHandler;
    public GameInstaller.Settings GameInstaller;
    public AlienSettings Alien;

    // We use nested classes here to group related settings together
    [Serializable]
    public class HookSettings
    {
        public HookStateWaiting.Settings StateStarting;
        public HookStateMoving.Settings StateMoving;
        public HookStateReturning.Settings StateReturning;
        public HookStateDead.Settings StateDead;
    }

    [Serializable]
    public class AlienSettings
    {
        public Alien.Settings Alien;
        public AlienStateWaiting.Settings StateWaiting;
        public AlienStateMoving.Settings StateMoving;
        public AlienStateCatched.Settings StateCatched;
    }
    [Serializable]
    public class AsteroidSettings
    {
        public AsteroidManager.Settings Spawner;
        public Asteroid.Settings General;
    }

    public override void InstallBindings()
    {
        Container.BindInstance(Alien.Alien);
        Container.BindInstance(Alien.StateWaiting);
        Container.BindInstance(Alien.StateMoving);
        Container.BindInstance(Alien.StateCatched);

        Container.BindInstance(Hook.StateMoving);
        Container.BindInstance(Hook.StateDead);
        Container.BindInstance(Hook.StateStarting);
        Container.BindInstance(Hook.StateReturning);

        Container.BindInstance(Asteroid.Spawner);
        Container.BindInstance(Asteroid.General);

        Container.BindInstance(AudioHandler);
        Container.BindInstance(GameInstaller);
    }
}

