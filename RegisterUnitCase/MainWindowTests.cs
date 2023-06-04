using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Linq;
using RegisterUnitCase.ADO;
using KeeperPRO.Pages.Issues;
using KeeperPRO.ADO;
using System.Windows;
using System;
using Request = RegisterUnitCase.ADO.Request;

namespace KeeperPRO.Windows.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        [TestMethod()]
        public void ChangeTitleMainWindow()
        {
            // Arrange
            string notExpected = "FirstTestName";

            MainWindow mainWindow = new MainWindow();
            mainWindow.Title = notExpected;
            
            // Act

            mainWindow.SetTitle("SecondTestName");

            // Assert

            Assert.AreNotEqual(notExpected, mainWindow.Title);

        }

    }
}
