using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeeperPRO.Pages.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using RegisterUnitCase.ADO;
using RegisterUnitCase.Service;
using System.Text.RegularExpressions;

namespace KeeperPRO.Pages.Auth.Tests
{
    [TestClass()]
    public class RegisterPageTests
    {
        [TestMethod()]
        public void RegisterUser_SaveDBTest()
        {
            // Arrange

            User newUser = new User
            {
                Email = "test3Email@gmail.com",
                EncPassword = Hashing.SHA512("password"),
                RoleCode = 1,
                FirstName = "Шубина",
                LastName = "Тамара",
                SurName = "Яковна",
                Login = "test3Email"
            };

            // Act

            DBConnections.DB.User.Add(newUser);
            DBConnections.DB.SaveChanges();

            // Assert

            Assert.IsTrue(newUser.ID != 0);
        }
        
        [TestMethod()]
        public void ValidateEmailTest()
        {
            // Arrange

            string email = "bulatT@mail.com";
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            // Act

            bool result = ValidateEmail(email, pattern);

            // Assert

            Assert.IsTrue(result);
        }


        public bool ValidateEmail(string email, string pattern)
        {
            if (!Regex.IsMatch(email, pattern))
            {
                return false;
            }
            return true;
        }

        [TestMethod()]
        public void RegisterValidatePasswordDBTest()
        {
            // Arrange

            string password = "Zz8POQlP}M4~";

            if (!IsValidatePassword(password))
            {
                Assert.IsTrue(IsValidatePassword(password));
                return;
            }


            User newUser = new User
            {
                Email = "test2Email@gmail.com",
                EncPassword = Hashing.SHA512(password),
                RoleCode = 1,
                FirstName = "Зиновьева",
                LastName = "Таисия",
                SurName = "Яковна",
                Login = "test2Email"
            };

            // Act

            DBConnections.DB.User.Add(newUser);
            DBConnections.DB.SaveChanges();

            // Assert

            Assert.IsTrue(newUser.ID != 0);
        }



        private static bool IsValidatePassword(string password)
        {
            string message = "";
            if (password.Length < 8)
            {
                message += "Пароль должен иметь как минимум 8 символов!\n";
            }
            if (!password.Any(x => char.IsUpper(x)))
            {
                message += "Пароль должен иметь как минимум 1 символ верхнего регистра!\n";
            }
            if (!password.Any(x => char.IsLower(x)))
            {
                message += "Пароль должен иметь как минимум 1 символ нижнего регистра!\n";
            }
            if (!password.Any(x => char.IsDigit(x)))
            {
                message += "Пароль должен иметь как минимум 1 цифру!\n";
            }
            List<char> specialSymbols = new List<char> {'!','@','#','№','$','%','^',
                '&','*','-','_','?','<','>'};
            if (password.Any(x => specialSymbols.Any(y => x == y)))
            {
                message += "Пароль должен иметь как минимум 1 спецсимвол!\n";
            }
            if (message != "")
            {
                return false;
            }
            return true;
        }



    }
}