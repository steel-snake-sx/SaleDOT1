using SaleDot.bll;
using SaleDot.data;
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

namespace SaleDot.Views.finance
{
    public partial class salepurchasedetails : Window
    {
        public int transactionid;
        List<data.dapper.financeaccount> financeaccounts = null;
        public salepurchasedetails(int transid,string type)
        {
            InitializeComponent();
            var productsalepurchaserepo = new data.dapper.productsalepurchaserepo();
       
            transactionid = transid;
            var productsinsale = productsalepurchaserepo.getmultiplebytransactionid(transactionid);
            foreach (var item in productsinsale)
            {
                dg.Items.Add(item);
            }
            if (type == "Продажа")
            {
                print_btn.IsEnabled = true;
            }
        }
        public void printPeceipt(object sender, RoutedEventArgs e)
        {
            saleutils.printDuplicateRecipt(transactionid);
        }
    }
}
