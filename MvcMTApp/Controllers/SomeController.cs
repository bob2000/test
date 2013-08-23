using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MassTransit;
using NHibernate;
using MvcMTApp.Models.DB;
using MvcMTApp.Services;
using MvcMTApp.Repositories;
using MvcMTApp.App_Start;
using System.Transactions;
using System.Threading;
using SqlFu;
using System.Data.Common;

namespace MvcMTApp.Controllers
{
    public class SomeController : Controller
    {
        private IServiceBus bus = null;
        private ISession session = null;
        private ISessionFactory sessionFactory = null;
        private OrderService orderService = null;
        private SimpleInjectorInitializer.MessageConsumer consumer = null;
        private IAuthorRepository authorReposiotry = null;
        private ISomethingRepository somethingRepository = null;

        private ICustomSession<NhTestConfig> nhTest = null;
        private ICustomSession<NhTestAnotherConfig> nhTestAnother = null;
        private ISession sessionNhTest = null;
        private ISession sessionNhTestAnother = null;

        public SomeController(
            IServiceBus bus, 
            //ISession session, 
            OrderService orderService, 
            //ISessionFactory sessionFactory, 
            ICustomSessionFactory<NhTestConfig> sessionFactory,
            SimpleInjectorInitializer.MessageConsumer consumer,
            IAuthorRepository userReposiotry,
            ISomethingRepository somethingRepository)
            //,
            //ICustomSession<NhTestConfig> nhTest,
            //ICustomSession<NhTestAnotherConfig> nhTestAnother)
        {
            this.bus = bus;
            //this.session = session;
            this.orderService = orderService;
            this.sessionFactory = sessionFactory.GetSessionFactory();
            this.consumer = consumer;
            this.authorReposiotry = userReposiotry;
            this.somethingRepository = somethingRepository;

            //this.nhTest = nhTest;
            //this.nhTestAnother = nhTestAnother;
            //sessionNhTest = nhTest.GetSession();
            //sessionNhTestAnother = nhTestAnother.GetSession();
        }

        //private void operations()
        //{
        //    //sessionNhTest.Transaction.Begin();
        //    //sessionNhTestAnother.Transaction.Begin();

        //    Author author = new Author();
        //    author.Name = "Fran1";
        //    authorReposiotry.AddUser(author);
        //    //sessionNhTest.Save(author);

        //    //sessionNhTestAnother.Save(new Something());

        //    throw new Exception("dupa");

        //    sessionNhTestAnother.Transaction.Commit();
        //    sessionNhTest.Transaction.Commit();

        //    //tx.Complete();
        //}

        public ActionResult Index()
        {
            TransactionOptions opt = new TransactionOptions();
            opt.IsolationLevel = IsolationLevel.ReadCommitted;
            TransactionScope tx = new TransactionScope(TransactionScopeOption.Required, opt);

            //sessionNhTestAnother = nhTestAnother.GetSession();
            //sessionNhTest = nhTest.GetSession();

            try
            {
                Author author = new Author();
                author.Name = "Fran1";
                authorReposiotry.AddUser(author);
                //sessionNhTest.Save(author);

                somethingRepository.AddSomething(new Something());
                //sessionNhTestAnother.Save(new Something());

                //throw new Exception("dupa");

                tx.Complete();
            }
            catch (Exception e)
            {
                // loguj
            }
            finally
            {
                Thread.Sleep(6000);

                tx.Dispose();
            }

            //int? result = System.Data.Objects.SqlClient.SqlFunctions.DateDiff("s", DateTime.Now, DateTime.Now.AddDays(-7));

            
            Books book = null;
            PocoFactory.ComplexTypeMapper.Separator = '.';
            SqlFuDao.ConnectionNameIs("nhtest_db");
            //PocoFactory.RegisterMapperFor<Books>(reader => 
            //{ 
            //    Books b = new Books();
            //    b.book_id = (int)reader.GetInt64(0);
            //    b.book_name = reader.GetString(1);
            //    b.year = reader.GetDateTime(2);
            //    return b;
            //});

            using (DbConnection db = SqlFuDao.GetConnection())
            {
                //db stuff
                bool b = db.TableExists<Books>();
                //book = db.QuerySingle<Books>("select * from books where book_id=@0", 1);
                book = db.Get<Books>(1); /
            }



            orderService.Start();

            if (bus != null)
            {
                ViewBag.BusState = "" + bus.GetHashCode();
            }
            else
            {
                ViewBag.BusState = "null";
            }

            //session = nhTest.GetSession();
            if (session != null)
            {
                ViewBag.SessionState = "" + session.GetHashCode();
                Book b = session.QueryOver<Book>().Where(i => i.Book_Id == 1).SingleOrDefault();
                ViewBag.DataFromDB = b.Book_Name;
                //ViewBag.DataFromDB = "to nie jest wartosc z DB! zobacz kod jak nie wierzysz";
            }
            else if(book != null)
            {
                ViewBag.SessionState = "null, micro orm mode";
                ViewBag.DataFromDB = book.year + " | "+ book.book_name + " | " + book.bookname;
            }
            else
            {
                ViewBag.SessionState = "null";
            }

            if (sessionFactory != null)
            {
                ViewBag.SessionFactoryState = "" + sessionFactory.GetHashCode();
            }
            else
            {
                ViewBag.SessionFactoryState = "null";
            }

            if (authorReposiotry != null)
            {
                ViewBag.UserReposiotrySessionHasCode = "" + authorReposiotry.GetSessionHasCode();
            }
            else
            {
                ViewBag.UserReposiotrySessionHasCode = "null";
            }

            orderService.Stop();

            return View();
        }

        //public interface ClassInterace {}

        //public class C1 : ClassInterace{ }

        //public class C2 : ClassInterace{ }

        //public interface IInterface<T> where T : class
        //{
        //}

        //public class InterfaceImpl<T> : IInterface<T> where T : class
        //{ }
    }
}
