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
	/// Interaction logic for AdminPage.xaml
	/// </summary>
	public partial class AdminPage : Page
	{
		public AdminPage()
		{
			InitializeComponent();
			var requests = App.DBConnection.Requests.ToList();
			var statuses = App.DBConnection.Status.ToList();
			dgRequests.ItemsSource = requests;
			dgCbStatuses.ItemsSource = statuses;
		}

		private void dgRequests_CurrentCellChanged(object sender, EventArgs e)
		{
			App.DBConnection.SaveChanges();
		}
	}
}
