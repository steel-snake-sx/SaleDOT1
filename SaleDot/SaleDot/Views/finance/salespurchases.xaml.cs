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
using MySql.Data.MySqlClient;
using static SaleDot.CustomLocalizationManager;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;


namespace SaleDot.Views.finance
{
    /// <summary>
    /// Interaction logic for sales.xaml
    /// </summary>
    public partial class salespurchases : System.Windows.Window
    {

        string listtype;
        financetransactionrepo financetransactionrepo = new data.dapper.financetransactionrepo();
        List<financetransactionextended> list = new List<financetransactionextended>();
        public salespurchases(string type)
        {
            LocalizationManager.Manager = new CustomLocalizationManager();
            InitializeComponent();
            listtype = type;
            this.Title = "Продажи";
            if (listtype == "Покупка запасов")
            {
                this.Title = "Покупка запасов";
            }
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
                list = new List<financetransactionextended>();
                if (listtype == "Продажа")
                {

                    list = financetransactionrepo.getmanybymanyfinanceaccountnames(new string[] { "Продажа", "Продажа", "Продажа услуг" }, fromdate, toDate);
                }
                else if (listtype == "Покупка запасов")
                {
                    list = financetransactionrepo.getmanybyselfnameandfinanceaccountname("Покупка запасов", "Инвентаризация", fromdate, toDate);
                }
                foreach (var item in list)
                {
                    if (listtype == "Продажа")
                    {
                        item.amount = -item.amount;
                    };
                    dg.Items.Add(item);
                }
            }

        }
        public void details(object sender, RoutedEventArgs e)
        {
            data.dapper.financetransaction obj = ((FrameworkElement)sender).DataContext as data.dapper.financetransaction;
            new salepurchasedetails(obj.id, listtype).Show();
        }
        public void report(object sender, RoutedEventArgs e)
        {

            data.dapper.financetransaction obj = ((FrameworkElement)sender).DataContext as data.dapper.financetransaction;

            reportingutils.prepareinvoicereport(obj.id);
        }

        private void Btn_datatime_report(object sender, RoutedEventArgs e)
        {
           
            if (list.Count > 0)
            {
                {
                    //Microsoft.Office.Interop.Excel.Application xlApp;
                    Excel.Application xlApp = new Excel.Application();
                    Microsoft.Office.Interop.Excel.Worksheet xlSheet;
                    Excel.Workbook workbook = xlApp.Workbooks.Open(@"C:\Users\shara\source\SaleDot\SaleDot\assets\invoice1.xlsm");//добавляем книгу
                    try
                    {
                        //делаем временно неактивным документ
                        xlApp.Interactive = false;
                        xlApp.EnableEvents = false;

                        //выбираем лист на котором будем работать (Лист 1)
                        Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);
                        xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                        //Название листа
                        xlSheet.Name = "Договор о купле-продажи";

                        //Выгрузка данных
                        //DataTable dt = GetData();

                        

                        int collInd = 0;
                        int rowInd = 0;
                        string data = "";

                        //заполняем строки
                        //for (rowInd = 0; rowInd < dt.Columns.Count; rowInd++)
                        //{
                        //    for (collInd = 0; collInd < dt.Rows.Count; collInd++)
                        //    {
                        //        data = dt.Rows[collInd].ItemArray[rowInd].ToString();
                        //        xlSheet.Cells[collInd + 20, rowInd + 3] = data;             //xlSheet.Cells[rowInd + 6, collInd + 1] = data; двигаем список заполнения на 6 строку
                        //    }

                        //}
                        //Список выводит столько же значений сколько написано при подключении к БД
                        string i = Convert.ToString(rowInd + 4);
                        //MessageBox.Show(i.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        //Показываем ексель
                        xlApp.Visible = true;

                        xlApp.Interactive = true;
                        xlApp.ScreenUpdating = true;
                        xlApp.UserControl = true;

                        //Отсоединяемся от Excel
                        releaseOobject(xlApp);
                    }
                    void releaseOobject(object obj)
                    {
                        try
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                            obj = null;
                        }
                        catch (Exception ex)
                        {
                            obj = null;
                            MessageBox.Show(ex.ToString(), "Ошибка!");
                        }
                        finally
                        {
                            GC.Collect();
                        }

                    }
                    //DataTable GetData()
                    //{
                    //    mysql db = new mysql();
                    //    db.openConnection();
                    //    mysql mysql = new mysql();
                    //    DataTable table = new DataTable();
                    //    MySqlCommand command = new MySqlCommand($"SELECT drug.`name` FROM medicine.recipe, medicine.drug WHERE drug.ID=recipe.drug AND recipe.id_conc={DataBank.lastidconclusion}", mysql.getConnection());
                    //    table = mysql.Command(command);
                    //    return table;
                    //}
                }



                //Word.Application wordApp = new Word.Application();
                //wordApp.Visible = true;
                //Word.Application WordApp = new Word.Application();
                //wordApp.Visible = true;
                //Object template = Type.Missing;
                //Object newTemplate = false;
                //Object documentType = Word.WdNewDocumentType.wdNewBlankDocument;
                //Object visible = true;
                //object missing = Type.Missing;
                //Word._Document wordDoc = wordApp.Documents.Add(
                //    ref missing, ref missing, ref missing, ref missing);
                //object start = 0, end = 0;
                //Word.Range range = wordDoc.Range(ref start, ref end);
                //range.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
                //range.PageSetup.LeftMargin = 10f;
                //range.PageSetup.RightMargin = 10f;
                //range.PageSetup.TopMargin = 10f;
                //range.PageSetup.BottomMargin = 10f;
                //range.Text = "я простиn".ToUpper();
                //range.Text += "аОТчет по \n";
                //range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                //range.ParagraphFormat.SpaceAfter = 0;

                //start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                //range = wordDoc.Range(ref start, ref end);
                //range.Text = $"Дата транзакции с: {FromDate.SelectedDate.Value.ToString("dd.MM.yyyy")}\n" +
                //    $"Дата транзакции по: {ToDate.SelectedDate.Value.ToString("dd.MM.yyyy")}\n";
                //range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                //range.ParagraphFormat.SpaceAfter = 0;




                //start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                //range = wordDoc.Range(ref start, ref end);
                //Word.Table table = wordDoc.Tables.Add(range, list.Count + 1, 8, missing, missing);

                //table.Cell(1, 1).Range.Text = "Ид";
                //table.Cell(1, 2).Range.Text = "Дата";
                //table.Cell(1, 3).Range.Text = "Счет";
                //for (int i = 0; i < list.Count; i++)
                //{
                //    if (listtype == "Продажа")
                //    {
                //        list[i].amount = -list[i].amount;
                //    }
                //    table.Cell(i + 2, 1).Range.Text = list[i].id.ToString();
                //    table.Cell(i + 2, 2).Range.Text = list[i].date.Value.ToString("dd.MM.yyyy");
                //    table.Cell(i + 2, 3).Range.Text = list[i].amount.ToString();
                //}
                //table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                //table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            }
            else
            {
                MessageBox.Show("Нет данных для формирования");
            }

            //    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            //    excel.Visible = true;
            //    Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            //    Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            //    sheet1.Name = "Отчет по каталогу";


            //    for (int i = 0; i < dg.Columns.Count; i++)
            //    //{
            //    //    for (int j = 1; j < dg.Items.Count; j++)
            //    //    {
            //    //        TextBlock b =dg.Columns[i].GetCellContent(dg.Items[j]) as TextBlock;
            //    //        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
            //    //        if (b != null)
            //    //        {
            //    //            myRange.Value2 = b.Text;
            //    //        }
            //    //    }

            //}

            //var fromdate = FromDate.SelectedDate;
            //var toDate = ToDate.SelectedDate;
            //if (fromdate != null && toDate != null)
            //{
            //    string conn = databaseutils.connectionstring;
            //    string joinselect = "t1.id,t1.name,t1.amount,t1.status,t1.details,t1.date,t1.fk_user_createdby_in_financetransaction,t1.fk_user_targetto_in_financetransaction,t1.fk_financeaccount_in_financetransaction,t2.name as accountname,t3.name as createdby,t4.name as target  from financetransaction t1 join financeaccount t2 on t1.fk_financeaccount_in_financetransaction = t2.id join `user` t3 on t1.fk_user_createdby_in_financetransaction=t3.id left join `user` t4 on  t1.fk_user_targetto_in_financetransaction=t4.id";
            //    data.dapper.financetransaction obj = ((FrameworkElement)sender).DataContext as data.dapper.financetransaction;

            //    reportingutils.prepareinvoicereport(joinselect.Length);
            //}

        }
    }
}