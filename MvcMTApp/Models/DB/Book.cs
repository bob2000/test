using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using NHibernate;
using NHibernate.Criterion;
using SqlFu;

namespace MvcMTApp.Models.DB
{
    public class Book : IValidatableObject
    {
        public Book() { }

        public virtual int Book_Id { get; set; }
        public virtual string Book_Name { get; set; }
        protected virtual IList<Opinion> Opinions { get; set; }
        
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IList<ValidationResult> errors = new List<ValidationResult>();

            if (Book_Name == null)
            {
                errors.Add(new ValidationResult(""));
            }

            return errors;
        }

        public virtual IList<Opinion> GetOpinions()
        {
            return Opinions;
        }
    }

    [SqlFu.Table("books", PrimaryKey = "book_id")]
    public class Books
    {
        public Books() { }

        public long book_id { get; set; }
        public string book_name { get; set; }
        public string bookname { get; set; }
        public DateTime year { get; set; }
        //public IList<Opinion> Opinions { get; set; }
    }

    public class BookAR : Book
    {
        private ISession session = null;

        public BookAR(ISession session)
        {
            this.session = session;
        }

        public IList<Opinion> GetOpinions()
        {
            return null;
        }

        public IList<Opinion> GetOpinion(int id)
        {
            return null;
        }

        public IList<Author> GetAuthors()
        {
            return null;
        }

        public void CreateNew(string name)
        {
            Book b = new Book();
            b.Book_Name = name;
            session.Save(b);
        }

        public void Load(int id)
        {
            Book b = session.Get<Book>(id);
            this.Book_Id = b.Book_Id;
            this.Book_Name = b.Book_Name;
        }
    }

    public class Books2
    {
        private ISession session = null;

        public Books2(ISession session)
        {
            this.session = session;
        }

        public void GetBooks()
        { 
        }

        public void GetBooks(Example example)
        {
        }

        public void GetBook(int id)
        {
        }
    }

    public class B : Book
    {
        public B()
        {
            IList<Opinion> o = base.Opinions;
        }
    }
}