using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using System.Data;

namespace MvcMTApp.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        void Rollback();
        void Commit();
    }

    public interface INhUnitOfWork : IUnitOfWork
    {
        void Begin();
        void Begin(IsolationLevel isolationLevel);
        void Begin(FlushMode flushMode);
        void Begin(IsolationLevel isolationLevel, FlushMode flushMode);
    }

    public class NhUnitOfWork : INhUnitOfWork
    {
        private ISession session = null;
        private ITransaction tx = null;

        public NhUnitOfWork(ISession session)
        {
            this.session = session;
        }

        public void Rollback()
        {
            if (tx.IsActive)
            {
                tx.Rollback();
            }
        }

        public void Commit()
        {
            if (tx.IsActive)
            {
                tx.Commit();
            }
        }

        public void Begin()
        {
            tx = this.session.BeginTransaction();
        }

        public void Begin(IsolationLevel isolationLevel)
        {
            tx = this.session.BeginTransaction(isolationLevel);
        }

        public void Begin(FlushMode flushMode)
        {
            session.FlushMode = flushMode;
        }

        public void Begin(IsolationLevel isolationLevel, FlushMode flushMode)
        {
            Begin(flushMode);
            Begin(isolationLevel);
        }

        public void Dispose()
        {
            session.Close();
        }
    }

    public interface IRepository {}
    public interface IFishRepository2 : IRepository {}
    public interface IUserRepository2 : IRepository {}

    public interface IRepositoryFactory
    {
        T GetRepository<T>() where T : IRepository, new();
    }

    public class RepositoryFactory : IRepositoryFactory 
    {
        public T GetRepository<T>() where T : IRepository, new()
        {
 	        return new T();
        }
    }

    //public class SomeService
    //{
    //    private ISessionFactory factory = null;
    //    private INhUnitOfWork uow = null;
    //    private IRepositoryFactory repositoryFactory = null;

    //    public SomeService(ISessionFactory factory, INhUnitOfWork uow, IRepositoryFactory repositoryFactory)
    //    {
    //        this.factory = factory;
    //        this.worker = uow;
    //        this.repositoryFactory = repositoryFactory;
    //    }

    //    public void DoWork()
    //    {
    //        using(worker)
    //        {
    //            try
    //            {
    //                worker.Begin();

    //                worker.Commit();
    //            }
    //            catch (Exception e)
    //            {
    //                worker.Rollback();
    //            }
    //        }
    //    }
    //}
}