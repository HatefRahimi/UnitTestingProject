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
        [Test]
        public void SendStatementEmails_WhenCalled_ShouldGenerateStatements()
        {
            // Arrange
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(x => x.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {
                new Housekeeper {Email="a", FullName="b", Oid = 1, StatementEmailBody = "c" }

            }.AsQueryable());

            var statementGenerator = new Mock<IStatementGenerator>();
            var emailSender = new Mock<IEmailSender>();
            var messageBox = new Mock<IXtraMessageBox>();

            var service= new HousekeeperHelperService(unitOfWork.Object, statementGenerator.Object, emailSender.Object, messageBox.Object);


            // Act
            service.SendStatementEmails(new DateTime(1998, 1, 1));

            // Assert
            statementGenerator.Verify(x => x.SaveStatement(1, "b", new DateTime(1998, 1, 1)));
        }

    }
}
