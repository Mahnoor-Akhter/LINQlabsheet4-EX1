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

namespace LINQlabsheet4_EX1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		NORTHWNDEntities db = new NORTHWNDEntities();
		public MainWindow()
		{
			
			InitializeComponent();
		}

		

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//Populate stock level listbox
			lbxStock.ItemsSource = Enum.GetNames(typeof(StockLevel));
			//Populate the suppliers listbox using anonymous type
			var query1 = from s in db.Suppliers
						 orderby s.CompanyName
						 select new
						 {
							 SupplierName = s.CompanyName,
							 SupplierID = s.SupplierID,
							 Country = s.Country
						 };
			//var query1 = from s in db.Suppliers
			//			 orderby s.CompanyName
			//			 select new

			lbxSupplier.ItemsSource = query1.ToList();
			//Populate the countiers list 
			//var query2 = from s in db.Suppliers
			//               orderby s.Country
			//               select s;


			//Faster to base this query1 which has the information 

			var query2 = query1
				.OrderBy(s => s.Country)
				.Select(s => s.Country);
			var countries = query2.ToList();
			lbxCountries.ItemsSource = countries.Distinct();
		}

		private void lbxStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//get stock level selected 
			var query = from p in db.Products
						where p.UnitPrice < 50
						orderby p.ProductName
						select p.ProductName;

			//select p

			string selected = lbxStock.SelectedItem as string;
		   
			switch(selected)
			{
				case "Low":
					//do nothing as query sorted from above
					break;
				case "Normal":
					query = from p in db.Products
							where p.UnitPrice >= 50 && p.UnitPrice <= 100
							orderby p.ProductName
							select p.ProductName;
					//select p;
					break;

				case "Overstocked ":
					query = from p in db.Products
							where p.UnitPrice > 100
							orderby p.ProductName
							select p.ProductName;
					//select p;
					break;
			}
			// update product list
			lbxProducts.ItemsSource = query.ToList();
		
		}

		private void lbxSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//using the selcted value path
			int supplierID = Convert.ToInt32(lbxSupplier.SelectedValue);

			//MessageBox.Show (supplierId.ToString());
			var query = from p in db.Products
						where p.SupplierID == supplierID
						orderby p.ProductName
						select p.ProductName;

			//update product list 
			lbxProducts.ItemsSource = query.ToList();

		}

		private void lbxCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string country = (string)(lbxCountries.SelectedValue);
			var query = from p in db.Products
						where p.Supplier.Country == country
						orderby p.ProductName
						select p.ProductName;
			//update product list


			lbxCountries.ItemsSource = query.ToList();
		}
	}// end of class
	public enum StockLevel { Low,Normal,Overstocked};

}


