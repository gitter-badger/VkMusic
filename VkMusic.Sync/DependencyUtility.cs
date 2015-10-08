using Microsoft.Practices.Unity;
using DAL.Repository.Abstract;
using DAL.Repository.XMLRepository;

namespace VkMusicSync
{
    public static class DependencyUtility
    {

        private static readonly UnityContainer container = new UnityContainer();

        static DependencyUtility()
        {
            container.RegisterType<ITrackList, TrackList>(new ContainerControlledLifetimeManager())
                     .RegisterType<ITrackRepository, TrackRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(AppPaths.SettingsPath));
        }

        static public T Resolve<T>()
            => container.Resolve<T>();
        

        static public void RegisterInstance<T>(T instance)
            => container.RegisterInstance(instance, new ContainerControlledLifetimeManager());
        

        static public void BuildUp<T>(T instance)
            => container.BuildUp(instance);
        

        public static void RegisterType<T>()
            => container.RegisterType<T>(new ContainerControlledLifetimeManager());
        
    }
}
