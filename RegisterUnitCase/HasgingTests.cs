using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Linq;
using RegisterUnitCase.ADO;
using KeeperPRO.Windows;
using RegisterUnitCase.Service;

namespace KeeperPRO.Pages.Auth.Tests
{
    [TestClass()]
    public class HasgingTest
    {
        [TestMethod()]
        public void HasingTests()
        {
            // Arrange
            string password = "hasingPassword12!";

            // Act

            string hasPassword = Hashing.SHA512(password);

            // Assert

            Assert.AreNotEqual(password, hasPassword);
        }


    }
}
