using System;
using System.Collections.Generic;

namespace Solum.Helpers
{
    public sealed class ServiceLocator
    {
        public static readonly Lazy<ServiceLocator> instance = new Lazy<ServiceLocator>(() => new ServiceLocator());
        readonly Dictionary<Type, Lazy<object>> _registeredServices = new Dictionary<Type, Lazy<object>>();

        /// <summary>
        /// Singleton instance for the default service locator
        /// </summary>
        public static ServiceLocator Instance => instance.Value;

        /// <summary>
        /// Add a new service implementation
        /// </summary>
        /// <typeparam name="TContract">The type of service</typeparam>
        /// <typeparam name="TService">The service implementation</typeparam>
        public void Add<TContract, TService>() where TService : new()
        {
            _registeredServices[typeof(TContract)] = new Lazy<object>(() => Activator.CreateInstance(typeof(TService)));
        }

        /// <summary>
        /// Resolve the service type into the implementation.  This assumes the key used to register the
        /// object is of the appropriate type - throws InvalidCastException if you get this wrong (at runtime)
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <returns>The service implementation</returns>
        public T Resolve<T>() where T : class
        {
            Lazy<object> service;
            if (_registeredServices.TryGetValue(typeof(T), out service))
            {
                return (T)service.Value;
            }
            return null;
        }
    }
}
