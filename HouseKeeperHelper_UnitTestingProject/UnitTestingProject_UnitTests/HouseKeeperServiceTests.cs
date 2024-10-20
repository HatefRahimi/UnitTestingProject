using System;
using HouseKeeperHelper_UnitTestingProject;
using Moq;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace UnitTestingProject_UnitTests
{
    [TestFixture]
    public class HouseKeeperServiceTests
    {
        private HousekeeperHelperService _service;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private DateTime _statementDate = new DateTime(1998, 1, 1);
        private Housekeeper _houseKeeper;
        private  string _statementFileName;

        [SetUp]
        public void CommonSetUp()
        {


            var unitOfWork = new Mock<IUnitOfWork>();
            _houseKeeper = new Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c" };
            unitOfWork.Setup(x => x.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {

                _houseKeeper

            }.AsQueryable());

            _statementFileName = "fileName";
            _statementGenerator = new Mock<IStatementGenerator>();

            // If you keep it like this, the last test cases are going to fail. Why?
            // Because we are programming the Returns() to return the _fileName we defined earlier.

            // Again, we can use the second overload to do lazy evaluation. 

            //_statementGenerator.Setup(x => x.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate)).Returns(_statementFileName);

            _statementGenerator.Setup(x => x.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate)).Returns(()=> _statementFileName);

            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();
            _service= new HousekeeperHelperService(unitOfWork.Object, _statementGenerator.Object, _emailSender.Object, _messageBox.Object);

        }

        [Test]
        public void SendStatementEmails_WhenCalled_ShouldGenerateStatements()
        {
            // Act
            _service.SendStatementEmails(_statementDate);

            // Assert
            VerifyStatementGenerated();
        }

        [Test]
        public void SendStatementEmails_EmailSendingFails_ShouldDisplayAMessage()
        {
            // Arrange
            _emailSender.Setup(x => x.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())
                );
            // Act
            _service.SendStatementEmails(_statementDate);

            // Assert
            _messageBox.Verify(x => x.Show(It.IsAny<string>(),It.IsAny<string>(), MessageBoxButtons.OK));
        }



        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void SendStatementEmails_EmailIsNullEmptyOrSpaced_ShouldNotGenerateStatements(string email)
        {
            // Arrange 
            _houseKeeper.Email = email;

            // Act
            _service.SendStatementEmails(_statementDate);

            // Times can be passed to Verify as the second argument. In this case, it indicates that this should never be called.
            // Assert
            VerifyStatementNotGenerated();
        }

        [Test]
        public void SendStatementEmails_WhenCalled_EmailTheStatement()
        {
            // Act
            _service.SendStatementEmails(_statementDate);

            // Assert
            VerifyEmailSent();
        }

      
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void SendStatementEmails_StatementFileNameIsNullEmptyOrSpaced_ShouldNotEmailTheStatement(string statementFileName)
        {
            // If you just add null, it raises an error. Because the method Returns has two overloads. One returns a string and the other returns a func.
            // When we just pass null, the compiler is not sure which overload we are interested in so we pass a func there to clarify. 

            // Arrange 
            _statementFileName = statementFileName;

            //_statementGenerator.Setup(x => x.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate)).Returns(()=> null);

            // Act
            _service.SendStatementEmails(_statementDate);

            // Assert
            VerifyEmailNotSent();
        }

        private void VerifyEmailNotSent()
        {
            _emailSender.Verify(x => x.EmailFile(_houseKeeper.Email, _houseKeeper.StatementEmailBody, _statementFileName, It.IsAny<string>()), Times.Never);
        }

        private void VerifyStatementNotGenerated()
        {
            _statementGenerator.Verify(x => x.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate), Times.Never);
        }
        private void VerifyEmailSent()
        {
            _emailSender.Verify(x => x.EmailFile(_houseKeeper.Email, _houseKeeper.StatementEmailBody, _statementFileName, It.IsAny<string>()));
        }
        private void VerifyStatementGenerated()
        {
            _statementGenerator.Verify(x => x.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate));
        }

    }
}
