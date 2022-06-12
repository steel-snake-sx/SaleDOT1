using SaleDot.bll;
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

namespace SaleDot.Views.others
{
    public partial class DatabaseSettingWindow : Window
    {
        public DatabaseSettingWindow()
        {
            InitializeComponent();
            tb_DatabaseServer.Text = AppSetting.DatabaseServer;
            tb_DatabaseName.Text = AppSetting.DatabaseName;
            tb_DatabaseUsername.Text = AppSetting.DatabaseUsername;
            tb_DatabasePassword.Password = AppSetting.DatabasePassword;
        }
        private void Button_CheckDatabaseConnection_Click(object sender, RoutedEventArgs e)
        {
            if (tb_DatabaseServer.Text != "" && tb_DatabaseName.Text != "" || tb_DatabaseUsername.Text != "" || tb_DatabasePassword.Password != "")
            {
                dynamic result = databaseutils.checkServerConnectionWithCredentials(tb_DatabaseServer.Text, tb_DatabaseUsername.Text, tb_DatabasePassword.Password);
                var isbool = (result is bool);
                if (isbool) {
                    if (result == true)
                    {
                        MessageBox.Show("Подключение к БД установлено", "Информация");
                        btn_Save.IsEnabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Сервер не подключен. У вас могут быть эти проблемы: " +
                        "\n1. Сервер БД MySql не установлен или недоступен." +
                        "\n2. Если сервер установлен, проверьте, запущена ли его служба.  " +
                        "\n3. Введены неправильные данные учетной записи" +
                        "\n Сведения об исключении: \n" +
                        result
                        ,
                        "Информация");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите правильные учетные данные", "Информация");
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            btn_Save.IsEnabled = false;
            if (tb_DatabaseServer.Text!=""&& tb_DatabaseName.Text != "" || tb_DatabaseUsername.Text != "" || tb_DatabasePassword.Password != "")
            {
                AppSetting.saveDatabaseSettings(tb_DatabaseServer.Text, tb_DatabaseName.Text, tb_DatabaseUsername.Text, tb_DatabasePassword.Password);
                MessageBox.Show("Настройки БД обновлены, перезапустите приложение.", "Информация");
            }
            else
            {
                otherutils.notify("Информация", "Неверная конфигурация", 5000);
            }
        }
        
    }
}
