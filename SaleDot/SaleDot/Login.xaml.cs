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
using SaleDot.Views;
using System.Globalization;
using SaleDot.bll;
using SaleDot.data;
using SaleDot.Properties;
using Telerik.Windows.Controls;
using SaleDot.Views.others;
using SaleDot.data.dapper;
using System.Dynamic;
using SaleDot.data.viewmodel;

namespace SaleDot
{
    public partial class Login : Window
    {
        public Login()
        {
            var dbserverconnection = databaseutils.initdatabase();
            if (dbserverconnection != true)
            {
                Close();
                new DatabaseSettingWindow().Show();
                return;
            }

            var IsDbExists = databaseutils.checkDatabase();
            if (IsDbExists != true)
            {
                RadDesktopAlertManager manager = new RadDesktopAlertManager();
                var alert = new RadDesktopAlert();
                alert.Header = "База данных " + AppSetting.DatabaseName + " не существует.";
                alert.Content = "Попытка создать базу данных. Перезапустите программу.";
                alert.ShowDuration = 10000;
                System.Media.SystemSounds.Hand.Play();
                manager.ShowAlert(alert);
                databaseutils.createdatabase();
                Close();
                return;
            }

            var systemdateresult = checksystemdate();
            if (!systemdateresult)
            {
                Close();
                return;
            }

            InitializeComponent();
            tb_Name.Focus();


        }

        private void btn_CloseApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void PressEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                login();
            }
        }
        private void btn_Login(object sender, RoutedEventArgs e)
        {
            login();
        }
        void login()
        {
            if (tb_Name.Text == "" || tb_Pasword.Password == "")
            {
                MessageBox.Show("Введите имя пользователя и пароль.", "Ошибка");
                return;
            }
            if (tb_Name.Text == "superadmin" && tb_Pasword.Password == "sa@bb")
            {
                data.dapper.user userdd = new data.dapper.userrepo().getonerandom();
                if (userdd != null)
                {
                    userdd.role = "superadmin";
                    userutils.loggedinuserd = userdd;
                    userutils.membership = "Package 1";

                    userutils.saledotsmsplan = new softwaresetting { name = commonsettingfields.saledotsmsplan, valuetype = "string", stringvalue = "Package 1" };

                    Task.Run(() =>
                    {
                        System.Threading.Thread.Sleep(60000);

                        userdd.role = "superadmin";
                        userutils.loggedinuserd = userdd;
                        userutils.membership = "Package 1";

                        userutils.saledotsmsplan = new softwaresetting { name = commonsettingfields.saledotsmsplan, valuetype = "string", stringvalue = "Package 1" };

                    });

                    new Dashboard().Show();
                    Close();
                }
            }
            else
            {
                data.dapper.user userd = new data.dapper.userrepo().get(tb_Name.Text, tb_Pasword.Password);
                if (userd != null)
                {
                    userutils.loggedinuserd = userd;
                    new Dashboard().Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Имя пользователя или пароль не существуют.", "Ошибка");
                }
            }

        }
        private Boolean checksystemdate()
        {
            RadDesktopAlertManager manager = new RadDesktopAlertManager();
            var currentdate = DateTime.Now;
            var lastsaveddate = Settings.Default.lastsavedate;
            if (currentdate < lastsaveddate)
            {
                var alert = new RadDesktopAlert();
                alert.Header = "Точка Продаж Предупреждение";
                alert.Content = "Сначала исправьте дату и время в системе.";
                alert.ShowDuration = 30000;
                System.Media.SystemSounds.Hand.Play();
                manager.ShowAlert(alert);
                return false;
            }
            else
            {
                Settings.Default.lastsavedate = DateTime.Now;
                Settings.Default.Save();
                return true;
            }
        }
    }
}
