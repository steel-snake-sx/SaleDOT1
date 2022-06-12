
using SaleDot.data;
using SaleDot.data.dapper;
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
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using static SaleDot.CustomLocalizationManager;

namespace SaleDot.Views.product
{
    public partial class inventorylog : Window
    {
        int productid;
        public inventorylog(int productid)
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();
            this.productid = productid;
            initFormOperations();
        }
        private void initFormOperations()
        {
            var report = new inventorylogrepo().get(this.productid);
            dg.ItemsSource = null;
            dg.ItemsSource = report;
            UpdateLayout();
        }
        private void search(object sender, RoutedEventArgs e)
        {
            var fromdate = fromdate_datepicker.SelectedDate;
            var todate = tb_todate_datepicker.SelectedDate;
            var report = new inventorylogrepo().get(this.productid,fromdate,todate);
            dg.ItemsSource = null;
            dg.ItemsSource = report;
        }

    }
}
