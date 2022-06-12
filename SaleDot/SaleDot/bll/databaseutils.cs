using SaleDot.bll;
using MySql.Data.MySqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using System.Windows;

namespace SaleDot.data.dapper
{
    public static class databaseutils
    {
        public static string connectionstring = "Server=localhost;Database=saledot;Uid=root;Pwd=1234;";
        public static string getkeyValuestoSqlAnd(dynamic keyvaluepairs) 
        {
            string s = "";
            foreach (KeyValuePair<string, object> kvp in keyvaluepairs)
            {
                s += getkeyValueToEqualTo(kvp.Key,kvp.Value);
                s += " and ";  
             }
            var ss = s.Remove(s.Length - 5);
            return ss;
        }
        public static string getkeyValuesToSqlOr(dynamic keyvaluepairs)
        {
            string s = "";
            foreach (KeyValuePair<string, object> kvp in keyvaluepairs)
            {
                s += getkeyValueToEqualTo(kvp.Key, kvp.Value);
                s += " or ";
            }
            var ss = s.Remove(s.Length - 4);
            return ss;
        }
        public static string getkeyValueToEqualTo(string key,object value) {
            var s = "";
            s += key + "=";
            if (value is string)
            {
                s += "'" + value + "'";
            }
            else
            {
                s += value;
            }
            return s;
        }
        public static string getWhereInSql(object[] values)
        {
            var s = "";
            foreach (object value in values) {
                if (value is string)
                {
                    s += "'" + value + "'";
                }
                else
                {
                    s += value;
                }
                s += ",";
            }
            var ss = s.Remove(s.Length - 1);
            return ss;
        }

        public static Boolean initdatabase() 
        {
            RadDesktopAlertManager alertmanager = new RadDesktopAlertManager();
            var connectionstringN = getConnectionString();
            connectionstring = connectionstringN;

            var r1 = checkServerConnection();
            if (r1 != true)
            {
                var alert = new RadDesktopAlert();
                alert.Header = "Сервер БД не подключен";
                alert.Content = "Сервер БД не запущен или не подключен";
                alert.ShowDuration = 10000;
                System.Media.SystemSounds.Hand.Play();
                alertmanager.ShowAlert(alert);
                return false ;
            }
            return true;

        }
        public static string getConnectionString() 
        {
            var connectionstringN  = "Server=" + AppSetting.DatabaseServer + ";Database=" + AppSetting.DatabaseName + ";Uid=" + AppSetting.DatabaseUsername + ";Pwd=" + AppSetting.DatabasePassword + ";";
            return connectionstringN;
        }
        public static Boolean checkServerConnection()
        {
            var serverconnection= "Server=" + AppSetting.DatabaseServer + ";Uid=" + AppSetting.DatabaseUsername + ";Pwd=" + AppSetting.DatabasePassword + ";";
            try
            {
                using (var connection = new MySqlConnection(serverconnection))
                {
                    connection.Open();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static Boolean checkDatabase()
        {
            var serverconnection = "Server=" + AppSetting.DatabaseServer + ";Database=" + AppSetting.DatabaseName + ";Uid=" + AppSetting.DatabaseUsername + ";Pwd=" + AppSetting.DatabasePassword + ";";
            try
            {
                using (var connection = new MySqlConnection(serverconnection))
                {
                    connection.Open();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static dynamic checkServerConnectionWithCredentials(string server, string user, string password)
        {
            var serverconnection = "Server=" + server + ";Uid=" + user + ";Pwd=" + password + ";";
            try
            {
                using (var connection = new MySqlConnection(serverconnection))
                {
                    connection.Open();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void createdatabase()
        {
            var serverconnection = "Server=" + AppSetting.DatabaseServer + ";Uid=" + AppSetting.DatabaseUsername + ";Pwd=" + AppSetting.DatabasePassword + ";";
            using (var connection = new MySqlConnection(serverconnection))
            {
                connection.Open();
                FileInfo file = new FileInfo(@"data/saledot.sql");
                string script = file.OpenText().ReadToEnd();
                string script2 = script.Replace("saledot", AppSetting.DatabaseName);
                MySqlCommand cmd = new MySqlCommand(script2, connection);
                var r = cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static dynamic runCustomQuery(string sql) {
            try {
                using (var connection = new MySqlConnection(connectionstring))
                {
                    dynamic result = connection.Query<dynamic>(sql);
                    return result;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message,"Ошибка");
                return null;
            }
        }

    }
}
