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
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using static SaleDot.CustomLocalizationManager;

namespace SaleDot.Views.finance
{
    public partial class transactions : Window
    {
        List<data.dapper.financeaccount> financeaccounts = null;
        data.dapper.financeaccountrepo financeaccountrepo = new data.dapper.financeaccountrepo();
        data.dapper.financetransactionrepo financetransactionrepo = new data.dapper.financetransactionrepo();
        public transactions()
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();


            financeaccounts = financeaccountrepo.get();
            fromaccount_combobox.ItemsSource = financeaccounts;
            fromaccount_combobox.DisplayMemberPath = "name";
            fromaccount_combobox.SelectedValuePath = "id";

            dg.Columns["fk_user_createdby_in_financetransaction"].IsVisible = false;
            toaccount_combobox.ItemsSource = financeaccounts;
            toaccount_combobox.DisplayMemberPath = "name";
            toaccount_combobox.SelectedValuePath = "id";
        }
        private void save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (fromaccount_combobox.SelectedItem == null || toaccount_combobox.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите аккаунт");
                }
                if (tb_amount.Text == "")
                {
                    MessageBox.Show("Пожалуйста, введите сумму");
                }

                var amount = Convert.ToDouble(tb_amount.Text);
                var fromaccount = (int)fromaccount_combobox.SelectedValue;
                var toaccount = (int)toaccount_combobox.SelectedValue;
                financeutils.insertexpence(fromaccount, toaccount, amount);

                MessageBox.Show("Операция успешнаl");
                Close();
                new expences().Show();
            }
            catch
            {
                MessageBox.Show("Операция успешнаl");
            }


        }

        private void Btn_Search_Transactions_Click(object sender, RoutedEventArgs e)
        {
            var fromdate = FromDate.SelectedDate;
            var toDate = ToDate.SelectedDate;
            if (fromdate != null && toDate != null) {
                toDate = TimeUtils.getEndDate(toDate);
                dg.ItemsSource = null;
                dg.Items.Clear();
                dg.Items.Refresh();
                var financetransactions = financetransactionrepo.getWithReferencedNames(fromdate, toDate);
                
                foreach (var item in financetransactions)
                {
                    dg.Items.Add(item);
                }
            }
            
        }
    }
}
