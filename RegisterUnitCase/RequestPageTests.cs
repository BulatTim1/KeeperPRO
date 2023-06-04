using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Linq;
using RegisterUnitCase.ADO;
using KeeperPRO.Windows;

namespace KeeperPRO.Pages.Auth.Tests
{
    [TestClass()]
    public class RequestPageTests
    {
        [TestMethod()]
        public void RequestCheckTitleTest()
        {
            // Arrange

            string expected = "Выполнено";

            // Act

            var statuses = DBConnections.DB.Status.FirstOrDefault(x => x.ID == 4);

            // Assert

            Assert.AreEqual(expected, statuses.Title);
        }

        [TestMethod()]
        public void RequestEditStatusTest()
        {
            // Arrange

            Status status = DBConnections.DB.Status.FirstOrDefault(x => x.ID == 4);

            // Act

            var request = DBConnections.DB.Request.FirstOrDefault();
            request.Status = status;
            DBConnections.DB.SaveChanges();

            // Assert

            Assert.IsTrue(request.Status == status);
        }

        [TestMethod()]
        public void RequestEditTest()
        {
            // Arrange

            string goalExpected = "TestGoal";

            // Act

            var request = DBConnections.DB.Request.FirstOrDefault();
            request.Goal = goalExpected;
            DBConnections.DB.SaveChanges();

            // Assert

            Assert.AreEqual(goalExpected, request.Goal);
        }

    }
}
