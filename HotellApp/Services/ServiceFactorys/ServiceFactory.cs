using Autofac;
using HotellApp.Services.ServiceFactory;
using HotellApp.Services.ServiceFactorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.ServiceFactorys
{
    public class ServiceFactory : IServiceFactory
    {


        private readonly IComponentContext _context;

        public ServiceFactory(IComponentContext context)
        {
            _context = context;
        }

        public T Get<T>()
        {
            return _context.Resolve<T>();
        }


    }
}
