using KeeperPRO.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeeperPRO.Pages.Auth
{
	/// <summary>
	/// Interaction logic for LoginPage.xaml
	/// </summary>
	public partial class LoginPage : Page
	{
		public LoginPage()
		{
			InitializeComponent();
		}

		private void LogInClick(object sender, RoutedEventArgs e)
		{
			var encryptedPassword = App.SHA512(tbPass.Password);
			try
			{
				var user = App.DBConnection.Users.First(
					u => u.Login == tbLogin.Text
					&& u.EncPassword == encryptedPassword);
				App.CurrentUser = user;
			} 
			catch
			{
				MessageBox.Show("Пользователь не найден или неправильный пароль!", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}

			switch (App.CurrentUser.Role.ID)
			{
				case 1:	//пользователь
					NavigationService.Content = new ClientBasePage();
					break;
				case 2:	//сотрудник
					NavigationService.Content = new AdminPage();
					break;
				case 3:	//админ
					NavigationService.Content = new AdminPage();
					break;
			}
		}

		private void ToRegisterClick(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new RegisterPage());
		}
	}
}
