using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using System.Data;
using System.Runtime.InteropServices;

namespace MvcMTApp.UOW
{
    public interface IMongoCollection { }



    public interface IDaoFactory<TDao> where TDao : class
    {
        TDao GetSession<TDaoWrapper>() where TDaoWrapper : class, new();
    }




    public interface INhDaoFactory : IDaoFactory<ISession>
    {
        ISession GetSession<TDaoWrapper>() where TDaoWrapper : class,  new();
    }

    public class NhDaoFactory : INhDaoFactory
    {
        public IDictionary<Type, INhDao> sessions = new Dictionary<Type, INhDao>();

        public ISession GetSession<TDaoWrapper>() where TDaoWrapper : class, new()
        {
            Type sessionType = typeof(TDaoWrapper);
            INhDao sessionObject = null;

            if (!sessions.Keys.Contains(sessionType))
            {
                sessionObject = (INhDao)new TDaoWrapper();
                sessions.Add(sessionType, sessionObject);
            }
            else
            {
                sessionObject
                    = sessions.Where(i => i.Key == sessionType).Select(i => i.Value).SingleOrDefault();
            }

            return sessionObject.GetDao();
        }

        //public ISession GetSession<TDaoWrapper>() where TDaoWrapper : class, new()
        //{
        //    throw new NotImplementedException();
        //}

        //public ISession GetSession<TDaoWrapper>() where TDaoWrapper : class, INhSession, new()
        //{
        //    throw new NotImplementedException();
        //}
    }

    public interface IMongoDaoFactory : IDaoFactory<IMongoCollection>
    {
        IMongoCollection GetSession<TDaoWrapper>() where TDaoWrapper : class,  new();
    }

    public class MongoDaoFactory : IMongoDaoFactory
    {
        public IMongoCollection GetSession<TDaoWrapper>() where TDaoWrapper : class,  new()
        {
            throw new NotImplementedException();
        }
    }
    










    //public interface IDatabaseSessionFactory<TDao> where TDao : class
    //{
    //    TDao GetSession<T>() where T : class, INhDao, new();
    //}

    //public class NhSessionFactory : IDatabaseSessionFactory<ISession>
    //{
    //    public IDictionary<Type, INhDao> sessions = new Dictionary<Type, INhDao>();

    //    public ISession GetSession<T>() where T : class, INhDao, new()
    //    {
    //        Type sessionType = typeof(T);
    //        INhDao sessionObject = null;

    //        if (!sessions.Keys.Contains(sessionType))
    //        {
    //            sessionObject = new T();
    //            sessions.Add(sessionType, sessionObject);
    //        }
    //        else
    //        {
    //            sessionObject 
    //                = sessions.Where(i => i.Key == sessionType).Select(i => i.Value).SingleOrDefault();
    //        }

    //        return sessionObject.GetSession();
    //    }
    //}

    public interface IDao<T>
    {
        T GetDao();
    }

    public interface INhDao : IDao<ISession>
    {
        ISession GetDao();
    }

    public interface IMongoDao : IDao<IMongoCollection>
    {
        IMongoCollection GetDao();
    }

    public class FishDB : INhDao
    {
        private ISession session = null;
    
        public ISession GetDao()
        {
            if (session == null)
            {
                //session 
            }

            return null;
        }
    }
    public class ChickenDB : INhDao
    {
        private ISession session = null;

        public ISession GetDao()
        {
            if (session == null)
            {
                //session 
            }

            return null;
        }
    }
    public class LogDB : IMongoDao
    {
        private IMongoCollection session = null;

        public IMongoCollection GetDao()
        {
            if (session == null)
            {
                //session 
            }

            return null;
        }
    }


    public interface ITransactionWorker : IDisposable
    {
        void Rollback();
        void Commit();
    }

    public interface INhTransactionWorker : ITransactionWorker
    {
        void SetIsolationLevel(IsolationLevel isolationLevel);
        void SetFlushMode(FlushMode flushMode);
    }

    public class NhTransactionWorker : INhTransactionWorker
    {
        private ISession session = null;
        private NhDaoFactory ndDaoFactory = null;
        private IsolationLevel isolationLevel = IsolationLevel.ReadCommitted;
        private FlushMode flushMode = FlushMode.Auto;

        public NhTransactionWorker(NhDaoFactory factory)
        {
            ndDaoFactory = factory;
        }

        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            this.isolationLevel = isolationLevel;
        }

        public void SetFlushMode(FlushMode flushMode)
        {
            this.flushMode = flushMode;
        }

        //public void SetDefaultIsolationLevel()
        //{
        //    this.isolationLevel = IsolationLevel.ReadCommitted;
        //}

        //public void SetDefualtFlushMode()
        //{
        //    this.flushMode = FlushMode.Auto;
        //}

        public void TryExecuteGracefully<T>(Action a) where T : class, INhDao, new()
        {
            try
            {
                DoWork<T>(a);
            }
            catch (Exception e)
            {
                Rollback();
            }
        }

        public void TryExecute<T>(Action a) where T : class, INhDao, new()
        {
            try
            {
                DoWork<T>(a);
            }
            catch (Exception e)
            {
                Rollback();
                throw;
            }
        }

        public void ExecuteGracefully<T>(Action a) where T : class, INhDao, new()
        {
            try
            {
                DoWork<T>(a);
            }
            catch (Exception e)
            {
                Rollback();
                Dispose();
            }
            finally
            {
                Dispose();
            }
        }

        public void Execute<T>(Action a) where T : class, INhDao, new()
        {
            try
            {
                DoWork<T>(a);
            }
            catch (Exception e)
            {
                Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            if (session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
            }
        }

        public void Commit()
        {
            if (session.Transaction.IsActive)
            {
                session.Transaction.Commit();
            }
        }

        public void Dispose()
        {
            session.Close();
        }

        private void DoWork<T>(Action a) where T : class, INhDao, new()
        {
            session = ndDaoFactory.GetSession<T>();

            if (session.IsOpen)
            {
                session.FlushMode = flushMode;
                session.Transaction.Begin(isolationLevel);

                a.Invoke();

                Commit();
            }
        }
    }




    public class SomeService1
    {
        private ISessionFactory factory = null;
        private NhTransactionWorker worker = null;

        public SomeService1(ISessionFactory factory, NhTransactionWorker worker)
        {
            this.worker = worker;
        }

        public void DoWork()
        {
            //daoFactory.get<ISession>(typeof());

            using(worker)
            {
                try
                {
                    //worker.Execute<>();

                    worker.Commit();
                }
                catch (Exception e)
                {
                    worker.Rollback();
                }
            }
        }
    }
}