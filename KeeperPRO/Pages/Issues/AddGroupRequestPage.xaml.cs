using KeeperPRO.ADO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeeperPRO.Pages.Issues
{
	/// <summary>
	/// Interaction logic for AddGroupRequestPage.xaml
	/// </summary>
	public partial class AddGroupRequestPage : Page
	{
		Request request;
		BindingList<Client> clients;
		Client currentClient;
		int currentIndex;
		public AddGroupRequestPage(bool group = false)
		{
			InitializeComponent();
			request = new Request();
			request.User = App.CurrentUser;
			clients = new BindingList<Client>();
			currentClient = new Client();
			currentIndex = -1;
			dgClients.ItemsSource = clients;
			cbSubdivision.ItemsSource = App.DBConnection.Subdivisions.ToList();

		}

		private void UploadPhotoClick(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "Images (*.JPG;)|*.JPG";
			bool? result = dlg.ShowDialog();

			if (result == true)
			{
				string filename = dlg.FileName;

				// Read file to byte array
				byte[] bytes = File.ReadAllBytes(filename);

				currentClient.Photo = bytes;
			}
		}

		private void UploadPassportClick(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.Filter = "Pdf files (*.pdf)|*.pdf";
			bool? result = dlg.ShowDialog();

			if (result == true)
			{
				string filename = dlg.FileName;

				// Read file to byte array
				byte[] bytes = File.ReadAllBytes(filename);

				currentClient.PassportScan = bytes;
			}
		}

		private void SendRequestClick(object sender, RoutedEventArgs e)
		{
			string message = "";
			bool selectedStartDate = true;
			if (dpStartDate.SelectedDate == null ||
				DateTime.Now.AddDays(1).Date > dpStartDate.SelectedDate.Value ||
				dpStartDate.SelectedDate.Value > DateTime.Now.AddDays(15).Date)
			{
				message += $"Введите корректный срок начала действия заявки " +
					$"(от {DateTime.Now.AddDays(1).ToString("dd.MM.yyyy")} " +
					$"до {DateTime.Now.AddDays(15).ToString("dd.MM.yyyy")}).\n";
				selectedStartDate = false;
			}
			if (selectedStartDate &&
				(dpEndDate.SelectedDate == null ||
					dpStartDate.SelectedDate.Value > dpEndDate.SelectedDate.Value ||
					dpStartDate.SelectedDate.Value.AddDays(15) < dpStartDate.SelectedDate.Value))
			{
				message += $"Введите корректный срок окончания действия заявки " +
					$"(от {dpStartDate.SelectedDate.Value.ToString("dd.MM.yyyy")} " +
					$"до {dpStartDate.SelectedDate.Value.AddDays(15).ToString("dd.MM.yyyy")}).\n";
			}
			if (string.IsNullOrWhiteSpace(tbRequestGoal.Text))
			{
				message += "Введите цель посещения.\n";
			}
			if (cbSubdivision.SelectedIndex == -1)
			{
				message += "Выберете подразделение.\n";
			}
			if (cbEmployer.SelectedIndex == -1)
			{
				message += "Выберете сотрудника.\n";
			}
			if (clients.Count > 0)
			{
				message += "Добавьте посетителей.\n";
			}
			if (message.Length != 0)
			{
				MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			request.CreatedDate = DateTime.Now;
			request.StatusCode = 1;
			request.User = App.CurrentUser;
			request.IsGroup = true;
			App.DBConnection.Requests.Add(request);
			int i = 0;

			foreach (var client in clients)
			{

				if (string.IsNullOrWhiteSpace(client.FirstName))
				{
					message += "Введите имя.\n";
				}
				if (string.IsNullOrWhiteSpace(client.LastName))
				{
					message += "Введите фамилию.\n";
				}
				Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
				if (!emailRegex.IsMatch(client.Email))
				{
					message += "Введите корректную почту.\n";
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
					MessageBox.Show($"Клиент №{i}:\n{message}", "Ошибка", 
						MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				Request_Client rq = new Request_Client();
				rq.Request = request;
				rq.Client = client;
				App.DBConnection.Clients.Add(client);
				App.DBConnection.Request_Client.Add(rq);
				++i;
			}

			App.DBConnection.SaveChanges(); 
			NavigationService.GoBack();
		}
		private void ClearClick(object sender, RoutedEventArgs e)
		{
			ClearForm();
		}

		private void ClearForm()
		{
			tbNote.Clear();
			tbPassportNumber.Clear();
			tbPassportSerial.Clear();
			tbFirstname.Clear();
			tbLastname.Clear();
			tbSurname.Clear();
			tbCompany.Clear();
			tbEmail.Clear();
			tbPhone.Clear();
			tbRequestGoal.Clear();
			dpBirthday.SelectedDate = null;
			dpEndDate.SelectedDate = null;
			dpStartDate.SelectedDate = null;
			cbSubdivision.ItemsSource = App.DBConnection.Subdivisions.ToList();
			cbEmployer.ItemsSource = new List<object>();
		}

		private void AddClientClick(object sender, RoutedEventArgs e)
		{
			clients.Add(currentClient);
			currentClient = new Client();
			UpdateForm();
		}

		private void DownloadTemplateClick(object sender, RoutedEventArgs e)
		{
			// Создаем диалог сохранения файла
			Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
			saveFileDialog.Filter = "CSV файлы (*.csv)|*.csv";
			saveFileDialog.Title = "Сохранить шаблон списка клиентов";
			saveFileDialog.FileName = "template.csv";

			// Если пользователь выбрал место сохранения и нажал OK
			if (saveFileDialog.ShowDialog() == true)
			{
				// Создаем и открываем файл для записи
				StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8);

				// Записываем заголовок таблицы
				sw.WriteLine("Фамилия,Имя,Отчество,Телефон,Email,Примечание,Дата рождения,Серия паспорта,Номер паспорта");


				// Проходим по всем контактам и записываем их в файл
				foreach (Client client in clients)
				{
					sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
						client.LastName, client.FirstName, client.SurName, client.Phone,
						client.Email, client.Note, client.BirthDay.ToString("dd.MM.yyyy"), client.PassportSerial, client.PassportNumber);
				}

				// Закрываем файл и выводим сообщение об успешном сохранении
				sw.Close();
				MessageBox.Show("Шаблон списка клиентов успешно сохранен", "Сохранение шаблона");
			}
		}

		private void UploadClientsClick(object sender, RoutedEventArgs e)
		{
			// Создаем диалог открытия файла
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			openFileDialog.Filter = "CSV файлы (*.csv)|*.csv";
			openFileDialog.Title = "Открыть список клиентов";

			// Если пользователь выбрал файл и нажал OK
			if (openFileDialog.ShowDialog() == true)
			{
				ClearForm();
				// Создаем и открываем файл для чтения
				StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.UTF8);

				// Считываем заголовок таблицы и игнорируем его (предполагается, что он всегда есть)
				string header = sr.ReadLine();

				// Создаем список клиентов
				clients = new BindingList<Client>();

				// Считываем каждую строку из файла
				while (!sr.EndOfStream)
				{
					// Разделяем строку по запятой на отдельные значения
					string[] values = sr.ReadLine().Split(',');

					// Создаем нового клиента и заполняем его поля из считанных значений
					Client client = new Client();
					client.LastName = values[0];
					client.FirstName = values[1];
					client.SurName = values[2];
					client.Phone = values[3];
					client.Email = values[4];
					client.Note = values[5];
					client.BirthDay = DateTime.ParseExact(values[6], "dd.MM.yyyy", null);
					client.PassportSerial = values[7];
					client.PassportNumber = values[8];

					// Добавляем клиента в список
					clients.Add(client);
				}

				dgClients.ItemsSource = clients;

				// Закрываем файл и сохраняем список клиентов в нашей модели данных
				sr.Close();
				MessageBox.Show("Список клиентов успешно загружен из файла", "Загрузка списка клиентов");
			}
		}

		private void UpdateForm()
		{
			if (currentClient != null)
			{
				tbNote.Text = currentClient.Note;
				tbPassportNumber.Text = currentClient.PassportNumber;
				tbPassportSerial.Text = currentClient.PassportSerial;
				tbFirstname.Text = currentClient.FirstName;
				tbLastname.Text = currentClient.LastName;
				tbSurname.Text = currentClient.SurName;
				tbCompany.Text = currentClient.Company;
				tbEmail.Text = currentClient.Email;
				tbPhone.Text = currentClient.Phone;
				dpBirthday.SelectedDate = currentClient.BirthDay;
			}
		}

		private void dgClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (dgClients.SelectedItem != null)
			{
				if (currentIndex != -1)
				{
					clients[currentIndex] = currentClient;
					dgClients.InvalidateVisual();
				}
				currentClient = dgClients.SelectedItem as Client;
				currentIndex = dgClients.SelectedIndex;
				UpdateForm();
			}
		}

		private void SubdivisionSelected(object sender, SelectionChangedEventArgs e)
		{
			Subdivision subdivision = cbSubdivision.SelectedItem as Subdivision;
			cbEmployer.ItemsSource = App.DBConnection.Employers.Where(x => x.Subdivision.ID == subdivision.ID).ToList();
		}

		private void EmployerSelected(object sender, SelectionChangedEventArgs e)
		{
			Employer employer = cbEmployer.SelectedItem as Employer;
			request.Employer = employer;
		}

		private void tbLastname_TextChanged(object sender, TextChangedEventArgs e)
		{
			currentClient.LastName = tbLastname.Text;
		}

		private void tbFirstname_TextChanged(object sender, TextChangedEventArgs e)
		{

			currentClient.FirstName = tbFirstname.Text;
		}

		private void tbSurname_TextChanged(object sender, TextChangedEventArgs e)
		{
			currentClient.SurName = tbSurname.Text;
		}

		private void tbEmail_TextChanged(object sender, TextChangedEventArgs e)
		{
			currentClient.Email = tbEmail.Text;
		}

		private void tbCompany_TextChanged(object sender, TextChangedEventArgs e)
		{
			currentClient.Company = tbCompany.Text;
		}

		private void tbNote_TextChanged(object sender, TextChangedEventArgs e)
		{
			currentClient.Note = tbNote.Text;
		}

		private void dpBirthday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (dpBirthday.SelectedDate != null)
			{
				currentClient.BirthDay = dpBirthday.SelectedDate.Value;
			}
		}

		private void tbPassportSerial_TextChanged(object sender, TextChangedEventArgs e)
		{
			currentClient.PassportSerial = tbPassportSerial.Text;
		}

		private void tbPassportNumber_TextChanged(object sender, TextChangedEventArgs e)
		{
			currentClient.PassportNumber = tbPassportNumber.Text;
		}

		private void tbPhone_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (tbPhone.IsMaskCompleted)
			{
				currentClient.Phone = tbPhone.Text;
			}
		}

		private void tbRequestGoal_TextChanged(object sender, TextChangedEventArgs e)
		{
			request.Goal = tbRequestGoal.Text;
		}

		private void dpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (dpEndDate.SelectedDate != null)
			{
				request.EndDate = dpEndDate.SelectedDate.Value;
			}
		}

		private void dpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (dpStartDate.SelectedDate != null)
			{
				request.StartDate = dpStartDate.SelectedDate.Value;
			}
		}
	}
}
