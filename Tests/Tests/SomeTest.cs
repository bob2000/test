using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSubstitute;

namespace Tests.Tests
{
    [TestFixture]
    public class SomeTest
    {
        // test czy metoda walidacyjna polecenia dziala dobrze

        //[Test]
        //public void t1()
        //{
        //    AddBookCommand command = new AddBookCommand(1);
        //    if(command.IsValid())
        //    {
        //        ICommandHandler<AddBookCommand> handler = new AddBookCommandHandler();
        //        handler.Handle(command);
        //    }

        //    Assert.
        //}

        [Test]
        public void t2()
        {
            var c = Substitute.For<ICommand>();
            //var c = Substitute.For<AddBookCommand>(2);
            Assert.AreEqual(true, c.IsValid());
        }

        [Test]
        public void t3()
        {
            var c = Substitute.For<AddBookCommand>(1);
            Assert.AreEqual(false, c.IsValid());
        }

        ///////

        public interface ICommand 
        { 
            bool IsValid();
        }

        public interface ICommandHandler<TCommand> where TCommand : ICommand 
        {
            void Handle(TCommand command);
        }

        public class AddBookCommand : ICommand
        {
            private int i { get; set; }

            public AddBookCommand(int i)
            {
                this.i = i;
            }

            public bool IsValid()
            {
                if (i > 1)
                {
                    return true;
                }
                return false;
            }
        }

        public class AddBookCommandHandler : ICommandHandler<AddBookCommand>
        {
            public void Handle(AddBookCommand command)
            {
                throw new NotImplementedException();
            }
        }
    }
}
