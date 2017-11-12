using Ninject.Web.Common.WebHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using NLog;
using Kaia.MultiSelect.DataAccess.Contract;
using Kaia.MultiSelect.DataAccess;
using System.Configuration;

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

            Logger.Trace("Starting up");
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected override IKernel CreateKernel()
        {
            var connectionName = ConfigurationManager.AppSettings["Kaia.Db"];
            var connectionString = 
                ConfigurationManager.ConnectionStrings[connectionName];

            var kernel = new StandardKernel();
            kernel.Bind<IUnitOfWork>()
                .To<UnitOfWork>()
                .WithConstructorArgument(connectionString);
            return kernel;
        }
    }
}
