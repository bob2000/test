using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MvcMTApp.Models.DB;
using NHibernate.Cfg;

namespace MvcMTApp.App_Start
{
    public interface IDbConf 
    {
        Configuration GetConfig();
    }

    public class NhTestConfig : IDbConf
    {
        public Configuration  GetConfig()
        {
            return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c.FromConnectionStringWithKey("nhtest_db")).ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Book>()).BuildConfiguration();
        }
    }

    public class NhTestAnotherConfig : IDbConf
    {
        public Configuration GetConfig()
        {
            return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c.FromConnectionStringWithKey("nhtest_another_db")).ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Something>()).BuildConfiguration();
        }
    }

    public interface ICustomSessionFactory<T> where T : class, IDbConf, new()
    {
        ISessionFactory GetSessionFactory();
    }

    public class CustomSessionFactory<T> : ICustomSessionFactory<T> where T : class, IDbConf, new()
    {
        public ISessionFactory GetSessionFactory()
        {
            return new T().GetConfig().BuildSessionFactory();
        }
    }

    public interface ICustomSession<T> where T : class, IDbConf, new()
    {
        ISession GetSession();
    }

    public class CustomSession<T> : ICustomSession<T> where T : class, IDbConf, new()
    {
        public ISession GetSession()
        {
            return new T().GetConfig().BuildSessionFactory().OpenSession();
        }
    }

    public interface ICustomStatelessSession<T> where T : class, IDbConf, new()
    {
        IStatelessSession GetStatelessSession();
    }

    public class CustomStatelessSession<T> : ICustomStatelessSession<T> where T : class, IDbConf, new()
    {
        public IStatelessSession GetStatelessSession()
        {
            return new T().GetConfig().BuildSessionFactory().OpenStatelessSession();
        }
    }






























    public class NH_Entities1 : INHibernateEntityManager
    {
        private readonly static ISessionFactory nhibernateSessionFactory = null;

        static NH_Entities1()
        {
            ISessionFactory nhibernateSessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c.FromConnectionStringWithKey("nhtest_db")).ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Book>())
                    .BuildSessionFactory();
        }

        public ISession GetSession()
        {
            return nhibernateSessionFactory.OpenSession();
        }

        public IStatelessSession GetStatelessSession()
        {
            return nhibernateSessionFactory.OpenStatelessSession();
        }

        public ISessionFactory GetSessionFactory()
        {
            return nhibernateSessionFactory;
        }
    }

    public class NH_Entities2 : INHibernateEntityManager
    {
        private readonly static ISessionFactory nhibernateSessionFactory = null;

        static NH_Entities2()
        {
            IList<int> l = new int[]{};

            ISessionFactory nhibernateSessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c.FromConnectionStringWithKey("nhtest_another_db")).ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Book>())
                    .BuildSessionFactory();
        }

        public ISession GetSession()
        {
            return nhibernateSessionFactory.OpenSession();
        }

        public IStatelessSession GetStatelessSession()
        {
            return nhibernateSessionFactory.OpenStatelessSession();
        }

        public ISessionFactory GetSessionFactory()
        {
            return nhibernateSessionFactory;
        }
    }

    public interface INHibernateEntityManager
    {
        ISessionFactory GetSessionFactory();
        ISession GetSession();
        IStatelessSession GetStatelessSession();
    }

    public interface INHibernateSession<T> where T : INHibernateEntityManager
    {
        ISession GetSession();
    }

    public interface INHibernateStatelessSession<T> where T : INHibernateEntityManager
    {
        IStatelessSession GetStatelessSession();
    }

    public interface INHibernateSessionFactory<T> where T : INHibernateEntityManager
    {
        ISessionFactory GetSessionFactory();
    }
}