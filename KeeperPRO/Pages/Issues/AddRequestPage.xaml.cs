using KeeperPRO.ADO;
using System;
using System.Collections.Generic;
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
	/// Interaction logic for AddRequestPage.xaml
	/// </summary>
	public partial class AddRequestPage : Page
	{
		Request request;
		Client client;
		public AddRequestPage()
		{
			InitializeComponent();
			request = new Request();
			request.User = App.CurrentUser;
			client = new Client();
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

				client.Photo = bytes;
				imgAvatar.Source = new BitmapImage(new Uri(filename));
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

				client.PassportScan = bytes;
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
			if (string.IsNullOrWhiteSpace(tbFirstname.Text))
			{
				message += "Введите имя.\n";
			}
			if (string.IsNullOrWhiteSpace(tbLastname.Text))
			{
				message += "Введите фамилию.\n";
			}
			//if (string.IsNullOrWhiteSpace(tbSurname.Text))
			//{
			//	message += "Введите отчество.\n";
			//}
			Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			if (!emailRegex.IsMatch(tbEmail.Text))
			{
				message += "Введите корректную почту.\n";
			}
			if (string.IsNullOrWhiteSpace(tbNote.Text))
			{
				message += "Введите примечание.\n";
			}
			if (dpBirthday.SelectedDate == null ||
				dpBirthday.SelectedDate.Value > DateTime.Now.AddYears(-16))
			{
				message += "Посетитель не должен быть моложе 16 лет.\n";
			}
			if (string.IsNullOrWhiteSpace(tbPassportSerial.Text) || tbPassportSerial.Text.Length != 4)
			{
				message += "Введите корректную серию паспорта.\n";
			}
			if (string.IsNullOrWhiteSpace(tbPassportNumber.Text) || tbPassportNumber.Text.Length != 6)
			{
				message += "Введите корректный номер паспорта.\n";
			}
			if (client.PassportScan == null)
			{
				message += "Необходим скан паспорта.\n";
			}
			if (message.Length != 0)
			{
				MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			request.CreatedDate = DateTime.Now;
			request.StatusCode = 1;
			//request.StatusNote = tbNote.Text;
			request.User = App.CurrentUser;
			request.IsGroup = false;

			Request_Client rq = new Request_Client();
			rq.Request = request;
			rq.Client = client;

			App.DBConnection.Requests.Add(request);
			App.DBConnection.Request_Client.Add(rq);
			App.DBConnection.Clients.Add(client);
			App.DBConnection.SaveChanges();
			NavigationService.GoBack();
		}

		private void ClearClick(object sender, RoutedEventArgs e)
		{
			client = new Client();
			request = new Request();
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
			client.LastName = tbLastname.Text;
		}

		private void tbFirstname_TextChanged(object sender, TextChangedEventArgs e)
		{

			client.FirstName = tbFirstname.Text;
		}

		private void tbSurname_TextChanged(object sender, TextChangedEventArgs e)
		{
			client.SurName = tbSurname.Text;
		}

		private void tbEmail_TextChanged(object sender, TextChangedEventArgs e)
		{
			client.Email = tbEmail.Text;
		}

		private void tbCompany_TextChanged(object sender, TextChangedEventArgs e)
		{
			client.Company = tbCompany.Text;
		}

		private void tbNote_TextChanged(object sender, TextChangedEventArgs e)
		{
			client.Note = tbNote.Text;
		}

		private void dpBirthday_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (dpBirthday.SelectedDate != null)
			{
				client.BirthDay = dpBirthday.SelectedDate.Value;
			}
		}

		private void tbPassportSerial_TextChanged(object sender, TextChangedEventArgs e)
		{
			client.PassportSerial = tbPassportSerial.Text;
		}

		private void tbPassportNumber_TextChanged(object sender, TextChangedEventArgs e)
		{
			client.PassportNumber = tbPassportNumber.Text;
		}

		private void tbPhone_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (tbPhone.IsMaskCompleted)
			{
				client.Phone = tbPhone.Text;
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
