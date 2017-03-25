using System;
using Autofac;

namespace LY.Common
{
    public static class IocManager
    {
        private static readonly ContainerBuilder _builder = new ContainerBuilder();
        private static IContainer _container = null;
        private const string _tag = "AutofacWebRequest";

        public static ContainerBuilder ContainerBuilder
        {
            get { return _builder; }
        }

        public static IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = _builder.Build();
                }
                return _container;
            }
        }

        public static object Tag
        {
            get { return _tag; }
        }

        public static void Resolve<IService>(Action<IService> action)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope(_tag))
            {
                IService service = scope.Resolve<IService>();
                action(service);
            }
        }

        public static TResult Scope<TResult> (Func<ILifetimeScope, TResult> action)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope(_tag))
            {
                return action(scope);
            }
        }
        public static void Scope (Action<ILifetimeScope> action)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope(_tag))
            {
                action(scope);
            }
        }
        public static TResult Resolve<IService, TResult>(Func<IService, TResult> func)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope(_tag))
            {
                IService service = scope.Resolve<IService>();
                return func(service);
            }
        }

        public static void Resolve(Type serviceType, Action<object> action)
        {
            using (ILifetimeScope scope = Container.BeginLifetimeScope(_tag))
            {
                object service = scope.Resolve(serviceType);
                action(service);
            }
        }
    }
}
