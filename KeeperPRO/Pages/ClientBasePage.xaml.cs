using KeeperPRO.Pages.Issues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace KeeperPRO.Pages
{
	/// <summary>
	/// Interaction logic for ClientBasePage.xaml
	/// </summary>
	public partial class ClientBasePage : Page
	{
		ClientMenuPage menu;
		public ClientBasePage()
		{
			InitializeComponent();
			tblUser.Text = App.CurrentUser.Login;
			App.MainWindow.SetBase(this);
			menu = new ClientMenuPage();
			BaseFrame.Navigate(menu);
		}

		private void ProfileClick(object sender, RoutedEventArgs e)
		{
			if (BaseFrame.Content != menu)
			{
				App.MainWindow.SetTitle("Меню");
				BaseFrame.Navigate(menu);
			} 
			else
			{
				App.MainWindow.SetTitle("Заявки");
				BaseFrame.Navigate(new ViewIssuesPage());
			}
		}

		public void SetBtnBack()
		{
			if (BaseFrame.Content != menu)
			{
				btnProfile.Content = "В профиль";
			}
			else
			{
				btnProfile.Content = "Назад";
			}
		}
	}
}
