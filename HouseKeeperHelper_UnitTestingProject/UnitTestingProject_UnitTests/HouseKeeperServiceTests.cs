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

        [SetUp]
        public void CommonSetUp()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            _houseKeeper = new Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c" };
            unitOfWork.Setup(x => x.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {

                _houseKeeper

            }.AsQueryable());

            _statementGenerator = new Mock<IStatementGenerator>();
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
            _statementGenerator.Verify(x => x.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate));
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

            // Assert
            _statementGenerator.Verify(x => x.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate), Times.Never);
        }

    }
}
