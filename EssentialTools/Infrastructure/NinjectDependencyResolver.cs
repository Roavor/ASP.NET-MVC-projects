using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using EssentialTools.Models;
using Ninject.Web.Common;

namespace EssentialTools.Infrastructure
{
    public class NinjectDependencyResolver:IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IValueCalculator>().To<LinqValueCalculator>();
            kernel.Bind<IDiscountHelper>().To<DefaultDiscountHelper>()
                .WithPropertyValue("DiscountSize", 50M);
            kernel.Bind<IDiscountHelper>().To<FexibleDiscountHelper>()
                .WhenInjectedInto<LinqValueCalculator>();
        }
    }
}