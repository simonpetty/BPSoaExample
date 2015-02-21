using System.Configuration;
using System.Data;
using System.Reflection;
using Castle.MicroKernel.Registration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using JLT.Framework.ServiceBus.Conventions;
using JLT.Framework.ServiceBus.Interceptors;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace JLT.Framework.ServiceBus.Installers
{
    public class SessionFactoryInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            var domainAssembly = ConfigurationManager.AppSettings["DomainAssembly"];
            var connectionString = ConfigurationManager.ConnectionStrings["NHibernate"];
            if (domainAssembly == null || connectionString == null)
                return;

            container.Register(Component.For<ISessionFactory>()
                .UsingFactoryMethod(() => Fluently.Configure()
                    .Database(
                        MsSqlConfiguration.MsSql2005
                            .AdoNetBatchSize(100)
                            .ConnectionString(s => s.FromConnectionStringWithKey("NHibernate"))
                            .DefaultSchema("dbo")
                            .Raw(NHibernate.Cfg.Environment.Isolation, IsolationLevel.Serializable.ToString()))
                    .ProxyFactoryFactory("NHibernate.Bytecode.DefaultProxyFactoryFactory, NHibernate")
                    .Mappings(m =>
                    {
                        m.FluentMappings.AddFromAssembly(Assembly.Load(domainAssembly));
                        m.FluentMappings.Conventions.AddFromAssemblyOf<HasManyConvention>();
                    })
                    .ExposeConfiguration(cfg =>
                    {
                        cfg.SetInterceptor(new ContextIdentityAuditInterceptor());
                        new SchemaExport(cfg).Create(false, true);
                    })
                    .BuildSessionFactory())
                .LifeStyle.Singleton);
        }
    }
}
