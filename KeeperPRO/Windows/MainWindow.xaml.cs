using KeeperPRO.Pages;
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
using KeeperPRO.Pages.Auth;

namespace KeeperPRO.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ClientBasePage _basePage;
		public MainWindow()
		{
			InitializeComponent();
			App.MainWindow = this;
			var page = new LoginPage();
			MainFrame.Navigate(page);
		}

		public void SetBase(ClientBasePage page)
		{
			_basePage = page;
		}

		public void SetTitle(string title)
		{
			this.Title = title;
			_basePage.SetBtnBack();
		}
	}
}
