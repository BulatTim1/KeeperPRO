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

namespace KeeperPRO.Pages.Issues
{
	/// <summary>
	/// Interaction logic for ViewIssuesPage.xaml
	/// </summary>
	public partial class ViewIssuesPage : Page
	{
		public ViewIssuesPage()
		{
			InitializeComponent();
			var requests = App.DBConnection.Requests.ToList();
			lvRequests.ItemsSource = requests;
		}
	}
}
