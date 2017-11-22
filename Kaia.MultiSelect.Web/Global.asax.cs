using Ninject.Web.Common.WebHost;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Common;
using NLog;
using Kaia.MultiSelect.DataAccess;
using System.Configuration;
using Kaia.Common.DataAccess.Contract;
using Kaia.Common.Web.Binders;
using System.Reflection;
using System;
using System.Linq;
using System.Diagnostics;

namespace Kaia.MultiSelect.Web
{
    public class MvcApplication : NinjectHttpApplication
    {
        private readonly ILogger _logger;

        public MvcApplication()
        {
            _logger = LogManager.GetLogger(GetType().FullName);
        }

        private ILogger Logger { get { return _logger; } }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            if (Logger.IsTraceEnabled) Logger.Trace("Starting up");
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ConfigureBinders();
        }

        protected override void OnApplicationStopped()
        {
            base.OnApplicationStopped();
            if (Logger.IsTraceEnabled) Logger.Trace("Shutting down");
        }

        protected override IKernel CreateKernel()
        {
            var connectionName = ConfigurationManager.AppSettings["Kaia.Db"];
            var connectionString =
                ConfigurationManager.ConnectionStrings[connectionName];

            var kernel = new StandardKernel();
            kernel.Bind<DataAccess.Contract.IUnitOfWork>()
                .To<UnitOfWork>()
                .InRequestScope()
                .WithConstructorArgument(connectionString);
            return kernel;
        }

        protected void ConfigureBinders()
        {
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetExportedTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && 
                        t.GetInterfaces().Any(i => i == typeof(IEntityModifier)))))
            {
                ModelBinders.Binders.Add(type, new EntityModifierBinder());
            }
        }
    }
}
