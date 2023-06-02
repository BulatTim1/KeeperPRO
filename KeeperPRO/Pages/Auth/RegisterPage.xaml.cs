using KeeperPRO.ADO;
using System;
using System.Collections.Generic;
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

namespace KeeperPRO.Pages.Auth
{
	/// <summary>
	/// Interaction logic for RegisterPage.xaml
	/// </summary>
	public partial class RegisterPage : Page
	{
		public RegisterPage()
		{
			InitializeComponent();
		}

		private void RegisterClick(object sender, RoutedEventArgs e)
		{
			string message = "";
			Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			if (!emailRegex.IsMatch(tbEmail.Text))
			{
				message += "Введите корректную почту.\n";
			}
			if (tbPass.Password.Length < 8)
			{
				message += "Пароль должен иметь как минимум 8 символов!\n";
			}
			if (!tbPass.Password.Any(x => char.IsUpper(x)))
			{
				message += "Пароль должен иметь как минимум 1 символ верхнего регистра!\n";
			}
			if (!tbPass.Password.Any(x => char.IsLower(x)))
			{
				message += "Пароль должен иметь как минимум 1 символ нижнего регистра!\n";
			}
			if (!tbPass.Password.Any(x => char.IsDigit(x)))
			{
				message += "Пароль должен иметь как минимум 1 цифру!\n";
			}
			List<char> specialSymbols = new List<char> {'!','@','#','№','$','%','^',
				'&','*','-','_','?','<','>'};
			if (tbPass.Password.Any(x => specialSymbols.Any(y => x == y)))
			{
				message += "Пароль должен иметь как минимум 1 спецсимвол!\n";
			}
			if (string.IsNullOrWhiteSpace(tbEmail.Text))
			{
				message += "Введите почту!\n";
			}
			if (message != "")
			{
				MessageBox.Show(message, "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}

			var encryptedPassword = App.SHA512(tbPass.Password);
			var newUser = new User()
			{
				Email = tbEmail.Text,
				EncPassword = encryptedPassword,
				Login = tbEmail.Text.Split('@')[0],
				RoleCode = 1	//костыль) мне лень в ssms ставить стандартное значение
			};

			try
			{
				App.DBConnection.Users.Add(newUser);
				Task.Run(async () => await App.DBConnection.SaveChangesAsync());
			}
			catch
			{
				MessageBox.Show("Пользователь не может быть создан!", "Ошибка",
					MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}

			App.CurrentUser = newUser;
			NavigationService.Content = new ClientBasePage();
		}

		private void ToLoginClick(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}
    }
}
