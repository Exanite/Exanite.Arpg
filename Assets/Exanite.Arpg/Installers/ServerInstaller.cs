using Exanite.Arpg.Networking.Server;
using Zenject;

namespace Exanite.Arpg.Installers
{
    public class ServerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerManager>().AsSingle();
            Container.Bind<UnityServer>().FromComponentInHierarchy().AsSingle();
        }
    }
}
