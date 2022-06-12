using SaleDot.bll;
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
using Telerik.Windows.Data;
using static SaleDot.CustomLocalizationManager;

namespace SaleDot.Views.finance
{
    public partial class expences : Window
    {
        financeaccountrepo financeaccountrepo = new data.dapper.financeaccountrepo();
        List<data.dapper.financeaccount> financeaccounts = null;
        financetransactionrepo financetransactionrepo = new data.dapper.financetransactionrepo();
        public expences()
        {

            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();
            financeaccounts = financeaccountrepo.get();
            var assetaccounts = financeaccounts.Where(a => a.type == "Активный счет").ToList();
            payingaccount_combobox.ItemsSource = assetaccounts;
            payingaccount_combobox.DisplayMemberPath = "name";
            payingaccount_combobox.SelectedValuePath = "id";
            var expenceaccounts = financeaccounts.Where(a => a.type == "Расходы").ToList();
            expenceaccount_combobox.ItemsSource = expenceaccounts;
            expenceaccount_combobox.DisplayMemberPath = "name";
            expenceaccount_combobox.SelectedValuePath = "id";

        }

        private void Btn_Search_Transactions_Click(object sender, RoutedEventArgs e)
        {
            var fromdate = FromDate.SelectedDate;
            var toDate = ToDate.SelectedDate;
            if (fromdate != null && toDate != null)
            {
                toDate = TimeUtils.getEndDate(toDate);
                dg.ItemsSource = null;
                dg.Items.Clear();
                dg.Items.Refresh();
                var list = financetransactionrepo.getmanybyfinanceaccounttype("Расходы",fromdate,toDate);
                foreach (var item in list)
                {
                    dg.Items.Add(item);
                }
            }

        }
        private void save(object sender, RoutedEventArgs e) 
        {
            try 
            {
                if (payingaccount_combobox.SelectedItem == null || expenceaccount_combobox.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите аккаунт");
                }
                if (tb_amount.Text == "")
                {
                    MessageBox.Show("Пожалуйста, введите сумму");
                }

                var amount = Convert.ToDouble(tb_amount.Text);
                var paingaccount = (int)payingaccount_combobox.SelectedValue;
                var expenceaccount = (int)expenceaccount_combobox.SelectedValue;
                financeutils.insertexpence(paingaccount, expenceaccount, amount);
             
                MessageBox.Show("Операция успешна");
                Close();
                new expences().Show();
            } catch 
            {
                MessageBox.Show("Операция успешна");
            }
            
            
        }
    }

}
