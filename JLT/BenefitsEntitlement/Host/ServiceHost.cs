using System;
using System.Data;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using JLT.BenefitCosts.Domain.Employees;
using JLT.Framework.Service.Data;
using MassTransit;
using MassTransit.Advanced;
using MassTransit.Saga;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace JLT.BenefitCosts.Host
{
    /// <summary>
    /// The Service Host creates and configures and instance of the bus and registers any consumers found
    /// as subscribers to the bus.
    /// </summary>
    internal class ServiceHost
    {
        private IServiceBus _bus;
        private IWindsorContainer _container;

        public void Start()
        {
            _container = new WindsorContainer();

            _container.Register(Classes.FromThisAssembly().BasedOn<IConsumer>());
            _container.Register(Classes.FromThisAssembly().BasedOn<ISaga>());
            _container.Register(Component.For(typeof(ISagaRepository<>)).ImplementedBy(typeof(InMemorySagaRepository<>)));

            _container.Register(Component.For<IServiceBus>()
                .UsingFactoryMethod(() => ServiceBusFactory.New(sbc =>
                {
                    sbc.UseRabbitMq();
                    sbc.UseJsonSerializer();
                    sbc.SetConcurrentReceiverLimit(Environment.ProcessorCount * 2);
                    sbc.ReceiveFrom("rabbitmq://localhost/JLT.BenefitCosts");
                    sbc.Subscribe(x => x.LoadFrom(_container));
                })).LifeStyle.Singleton);

            

            _container.Register(Component.For<ISessionFactory>()
                    .UsingFactoryMethod(() => Fluently.Configure()
                        .Database(
                            MsSqlConfiguration.MsSql2005
                                .AdoNetBatchSize(100)
                                .ConnectionString("Data Source=.;Initial Catalog=JLTBenefitCosts;Integrated Security=true;Application Name=JLTAppServer")
                                .DefaultSchema("dbo")
                                .Raw(NHibernate.Cfg.Environment.Isolation, IsolationLevel.Serializable.ToString()))
                        .ProxyFactoryFactory("NHibernate.Bytecode.DefaultProxyFactoryFactory, NHibernate")
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PolicyCost>())
                        .ExposeConfiguration(cfg =>
                        {
                            cfg.SetInterceptor(new RepositoryAuditInterceptor());
                            new SchemaExport(cfg).Create(false, true);
                        })
                        .BuildSessionFactory())
                    .LifeStyle.Singleton);

            _bus = _container.Resolve<IServiceBus>();

            //EventBroker.WireUp(typeof(Employee).Assembly);
            //EventBroker.WireUp(typeof(EmployeeEventSubscriber).Assembly);
        }

        public void Stop()
        {
            _bus.Dispose();
        }
    }
}
