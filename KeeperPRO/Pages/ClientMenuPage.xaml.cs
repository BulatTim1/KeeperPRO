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
	/// Interaction logic for ClientMenuPage.xaml
	/// </summary>
	public partial class ClientMenuPage : Page
	{
		public ClientMenuPage()
		{
			InitializeComponent();
		}

		private void SingleGroupClick(object sender, RoutedEventArgs e)
		{
			App.MainWindow.SetTitle("Создание заявки");
			NavigationService.Navigate(new AddRequestPage());
		}

		private void MultiGroupClick(object sender, RoutedEventArgs e)
		{
			App.MainWindow.SetTitle("Создание групповой заявки");
			NavigationService.Navigate(new AddGroupRequestPage());
		}
	}
}
