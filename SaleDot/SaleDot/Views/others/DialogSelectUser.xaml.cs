using SaleDot.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using SaleDot.data.dapper;


namespace SaleDot.Views
{
    public partial class DialogSelectUser : Window
    {
        string roletype = null;
        public data.dapper.user seleteduser { get; set; }
        List<data.dapper.user> allusers = null;
        
        public DialogSelectUser(string roletype)
        {
            InitializeComponent();
            tb_Phone.Focus();
            this.roletype = roletype;
            var userrepo = new userrepo();

            if (roletype == "staff")
            {

                var roles = new object[] { "admin","user" };
                allusers = userrepo.getbywherein("role",roles);
            }
            else if (roletype == "customer")
            {

                var roles = new object[] { "customer" };
                allusers = userrepo.getbywherein("role", roles);
            }
            else
            {

                var roles = new object[] { "vendor" };
                allusers = userrepo.getbywherein("role", roles);
            }
            dg.ItemsSource = allusers;
        }

        private void tb_Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void select(object sender, RoutedEventArgs e)
        {
            data.dapper.user obj = ((FrameworkElement)sender).DataContext as data.dapper.user;
            this.seleteduser = obj;
            DialogResult = true;
        }
        public void cancel(object sender, RoutedEventArgs e)
        {
            this.seleteduser = null;
            DialogResult = true;
        }


        private void SaveAndSelect(object sender, System.Windows.RoutedEventArgs e)
        {

            data.dapper.user c = new data.dapper.user();
            c.phone = tb_Phone.Text;
            c.role = roletype;
            c.name = tb_Name.Text;
            c.address = tb_Address.Text;

            var userrepo = new userrepo();
            var customer = userrepo.save(c);
            this.seleteduser = customer;
            DialogResult = true;
        }

        

    }
}
