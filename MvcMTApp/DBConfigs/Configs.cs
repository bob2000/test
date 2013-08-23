using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using FluentNHibernate.Cfg;
using SimpleInjector;
using NHibernate.Cfg;

namespace MvcMTApp.DBConfigs
{
    public class Configs
    {
        public void Init()
        {
            Container container = new Container();
            container.RegisterSingle<IDatabaseConfigurationFactory<FishMapDatabaseConfiguration>, DatabaseConfigurationFactory<FishMapDatabaseConfiguration>>();
            container.RegisterSingle<INhSessionFactory<FishMapDatabaseConfiguration>, NhSessionFactory<FishMapDatabaseConfiguration>>();
            container.RegisterPerWebRequest<INhSession<FishMapDatabaseConfiguration>, NhSession<FishMapDatabaseConfiguration>>();
        }
    }





    //public interface IDatabase { }

    //public interface FishMapDatabase { }



    public interface IDatabaseConfigurationFactory<T> where T : INhDatabaseConfiguration, new()
    {
        Configuration GetConfiguration();
    }

    public class DatabaseConfigurationFactory<T> : IDatabaseConfigurationFactory<T> where T : INhDatabaseConfiguration, new()
    {
        public Configuration GetConfiguration()
        {
            return new T().GetConfiguration();
        }
    }

    public interface IDatabaseConfigurationFactory 
    {
        Configuration GetConfiguration<T>() where T : INhDatabaseConfiguration, new();
    }

    public class DatabaseConfigurationFactory : IDatabaseConfigurationFactory
    {
        public Configuration GetConfiguration<T>() where T : INhDatabaseConfiguration, new()
        {
            return new T().GetConfiguration();
        }
    }



    public interface INhDatabaseConfiguration
    {
        Configuration GetConfiguration();
    }

    public class FishMapDatabaseConfiguration : INhDatabaseConfiguration
    {
        private Configuration configuration = null;

        public FishMapDatabaseConfiguration()
        {
            configuration = Fluently.Configure().BuildConfiguration(); 
        }

        public Configuration GetConfiguration()
        {
            return configuration;
        }
    }

    public interface INhSessionFactory<T> where T : INhDatabaseConfiguration
    {
        ISessionFactory GetSessionFactory();
    }

    public class NhSessionFactory<T> : INhSessionFactory<T> where T : INhDatabaseConfiguration, new()
    {
        private ISessionFactory sessionFactory = null;

        public NhSessionFactory(IDatabaseConfigurationFactory<T> factory)
        {
            sessionFactory = factory.GetConfiguration().BuildSessionFactory();
        }

        public ISessionFactory GetSessionFactory()
        {
            return sessionFactory;
        }
    }

    public class NhSessionFactory2<T> : INhSessionFactory<T> where T : INhDatabaseConfiguration, new()
    {
        private ISessionFactory sessionFactory = null;

        public NhSessionFactory2(IDatabaseConfigurationFactory factory)
        {
            sessionFactory = factory.GetConfiguration<T>().BuildSessionFactory();
        }

        public ISessionFactory GetSessionFactory()
        {
            return sessionFactory;
        }
    }

    public interface INhSession<T> where T : INhDatabaseConfiguration
    {
        ISession GetSession();
    }

    public class NhSession<T> : INhSession<T> where T : INhDatabaseConfiguration
    {
        private ISession session = null;

        public NhSession(INhSessionFactory<T> sessionFactory)
        {
            session = sessionFactory.GetSessionFactory().OpenSession();
        }

        public ISession GetSession()
        {
            return session;
        }
    }
}