using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegisterUnitCase.ADO;
using RegisterUnitCase.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace KeeperPRO.Pages.Auth.Tests
{
    [TestClass()]
    public class LoginPageTests
    {
        [TestMethod()]
        public void AuthNotWithHashedPasswordTest()
        {
            // Arrange
            string login = "Radinka100";
            string password = "b3uWS6#Thuvq";

            // Act
            var authUser = DBConnections.DB.User.Any(x => x.Login == login && x.EncPassword == password);

            // Assert
            Assert.IsFalse(authUser);
        }

        [TestMethod()]
        public void AuthWithHashedPasswordTest()
        {
            // Arrange
            string login = "Radinka100";
            string password = "b3uWS6#Thuvq";
            string hasPassword = Hashing.SHA512(password);

            // Act
            var authUser = DBConnections.DB.User.Any(x => x.Login == login && x.EncPassword == hasPassword);

            // Assert
            Assert.IsTrue(authUser);
        }
    }
}
