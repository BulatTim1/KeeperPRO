using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Linq;
using RegisterUnitCase.ADO;
using KeeperPRO.Pages.Issues;
using KeeperPRO.ADO;
using System.Windows;
using System;
using Request = RegisterUnitCase.ADO.Request;
using System.Windows.Documents;
using System.Collections.Generic;
using Request_Client = RegisterUnitCase.ADO.Request_Client;
using System.Text.RegularExpressions;
using Client = RegisterUnitCase.ADO.Client;
using System.IO;

namespace KeeperPRO.Pages.Issues.Tests
{
    [TestClass()]
    public class AddGroupRequestTests
    {

        [TestMethod()]
        public void RequestAddToDB()
        {
            // Arrange

            Request request = new Request()
            {
                CreatedDate = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CreatorCode = 1,
                EmployerCode = 1,
                StatusCode = 2,
                IsGroup = false,
                Goal = "testGoal",
                Request_Client = new List<Request_Client>() 
                {
                    new Request_Client()
                    {
                        ClientCode = 1
                    }
                },
            };

            // Act

            DBConnections.DB.Request.Add(request);
            DBConnections.DB.SaveChanges();

            // Assert

            Assert.IsTrue(request.ID != 0);
        }

        [TestMethod()]
        public void RequestListTest()
        {
            // Arrange

            // Act
            var request = DBConnections.DB.Request.ToList();

            // Assert
            Assert.IsNotNull(request);
        }


        [TestMethod()]
        public void RequestCheckTitleTest()
        {
            // Arrange
            Request request = new Request();
            request.StartDate = DateTime.Now.AddDays(1);
            request.EndDate = DateTime.Now.AddDays(2);
            request.Goal = "Цель";
            request.Employer = DBConnections.DB.Employer.FirstOrDefault(x => x.ID == 1);

            // Act

            bool validateRequest = Validate(request);

            // Assert

            Assert.IsFalse(validateRequest);
        }
        
        [TestMethod()]
        public void RequestCheckCorrectClientTest()
        {
            // Arrange

            Client client = new Client();

            // Act

            bool validateClient = ValidateClientTests(client);

            // Assert

            Assert.IsFalse(validateClient);
        }

        [TestMethod()]
        public void RequestUploadPassportTest()
        {
            // Arrange

            Client client = new Client();

            // Act

            client.PassportScan = UploadPassportClick();

            // Assert

            Assert.IsNotNull(client.PassportScan);
        } 
        
        [TestMethod()]
        public void RequestUploadPhotoTest()
        {
            // Arrange

            Client client = new Client();

            // Act

            client.Photo = UploadPhoto();

            // Assert

            Assert.IsNotNull(client.Photo);
        }




        // Code

        private byte[] UploadPhoto()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Images (*.JPG;)|*.JPG";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                // Read file to byte array
                byte[] bytes = File.ReadAllBytes(filename);

                return bytes;
            }
            return null;
        }
        
        private byte[] UploadPassportClick()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Pdf files (*.pdf)|*.pdf";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                // Read file to byte array
                byte[] bytes = File.ReadAllBytes(filename);

                return bytes;
            }
            return null;
        }


        public bool ValidateClientTests(Client client) 
        {
            string message = "";

            if (string.IsNullOrWhiteSpace(client.FirstName))
            {
                message += "Введите имя.\n";
            }
            if (string.IsNullOrWhiteSpace(client.LastName))
            {
                message += "Введите фамилию.\n";
            }
            if (string.IsNullOrWhiteSpace(client.Note))
            {
                message += "Введите примечание.\n";
            }
            if (client.BirthDay == null ||
                client.BirthDay > DateTime.Now.AddYears(-16))
            {
                message += "Посетитель не должен быть моложе 16 лет.\n";
            }
            if (string.IsNullOrWhiteSpace(client.PassportSerial))
            {
                message += "Введите серию паспорта.\n";
            }
            if (string.IsNullOrWhiteSpace(client.PassportNumber))
            {
                message += "Введите номер паспорта.\n";
            }
            if (client.PassportScan == null)
            {
                message += "Необходим скан паспорта!.\n";
            }
            if (message.Length != 0)
            {
                MessageBox.Show($"Клиент:\n{message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return  true;
        }

        private bool Validate(Request request) 
        {
            string message = "";
            bool selectedStartDate = true;
            if (request.StartDate == null ||
                DateTime.Now.AddDays(1).Date > request.StartDate ||
                request.StartDate > DateTime.Now.AddDays(15).Date)
            {
                message += $"Введите корректный срок начала действия заявки " +
                    $"(от {DateTime.Now.AddDays(1).ToString("dd.MM.yyyy")} " +
                    $"до {DateTime.Now.AddDays(15).ToString("dd.MM.yyyy")}).\n";
                selectedStartDate = false;
            }
            if (selectedStartDate &&
                (request.EndDate == null ||
                   request.EndDate < request.StartDate ||
                    request.EndDate.AddDays(15) < request.EndDate))
            {
                message += $"Введите корректный срок окончания действия заявки " +
                    $"(от {request.EndDate.ToString("dd.MM.yyyy")} " +
                    $"до {request.EndDate.AddDays(15).ToString("dd.MM.yyyy")}).\n";
            }
            if (string.IsNullOrWhiteSpace(request.Goal))
            {
                message += "Введите цель посещения.\n";
            }
            if (request.Employer == null)
            {
                message += "Выберете сотрудника.\n";
            }
            if (message.Length != 0)
            {
                return true;
            }
            return false;
        }


    }
}
