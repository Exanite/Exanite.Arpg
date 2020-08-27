using Exanite.Arpg;
using Zenject;

namespace Assets.Exanite.Arpg.Installers
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
        }
    }
}
