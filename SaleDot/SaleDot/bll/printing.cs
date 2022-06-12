using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Shapes;
using System.Drawing.Printing;
using System.Printing;
using System.Windows.Media;
using SaleDot.data.viewmodel;
using SaleDot.Properties;

namespace SaleDot.bll
{
    public class printing
    {
        public static void printSaleReceipt(int salesId, List<productsaleorpurchaseviewmodel> list, double totalBill,double totalpayment, double remaining, bool printcustomerinfoonreceipt, string customerAddress)
        {
            PrintDialog pd = new PrintDialog();
            var doc = ((IDocumentPaginatorSource)getFlowDocument(salesId, list, totalBill,totalpayment, remaining, printcustomerinfoonreceipt, customerAddress)).DocumentPaginator;

            pd.PrintQueue = new PrintQueue(new PrintServer(), new PrinterSettings().PrinterName);
            pd.PrintDocument(doc, "Итоговый счет");
        }
        static FlowDocument getFlowDocument(int salesId, List<productsaleorpurchaseviewmodel> list, double totalBill, double totalpayment, double remaining, bool printcustomerinfoonreceipt, string customerAddress)
        {
            FlowDocument fd = new FlowDocument();
            fd.PageWidth = Settings.Default.PrinterPageWidth + 5;
            fd.LineHeight = Settings.Default.ReciptlineHeight;
            fd.FontFamily = new FontFamily("Arial");

            fd.PagePadding = new Thickness(Settings.Default.PrinterMarginLeft, 0, 0, 0);
            fd.TextAlignment = TextAlignment.Center;
            Section header = new Section();

            Paragraph header1 = new Paragraph(new Bold(new Run(Settings.Default.Title)));

            string date = DateTime.Now.ToShortDateString();
            Paragraph header3 = new Paragraph(new Run("Номер чека: " + salesId + "   Дата:  " + date));
            Paragraph header4 = new Paragraph(new Run("___________________________________________"));
            header1.FontSize = 14;
            header3.FontSize = 9;
            header4.FontSize = 8;
            header.Blocks.Add(header1);
            header.Blocks.Add(header3);
            header.Blocks.Add(header4);


            Section middle = new Section();
            middle.FontSize = 9;
            Table table = new Table();
            table.TextAlignment = TextAlignment.Left;
            TableColumn tb1 = new TableColumn();
            tb1.Width = new GridLength(140);
            TableColumn tb2 = new TableColumn();
            TableColumn tb3 = new TableColumn();
            TableColumn tb4 = new TableColumn();
            table.Columns.Add(tb1);
            table.Columns.Add(tb2);
            table.Columns.Add(tb3);
            table.Columns.Add(tb4);
            table.RowGroups.Add(new TableRowGroup());
            TableRow trHeader = new TableRow();
            table.RowGroups[0].Rows.Add(trHeader);
            trHeader.Cells.Add(new TableCell(new Paragraph(new Run("Имя"))));
            trHeader.Cells.Add(new TableCell(new Paragraph(new Run("Цена за ед  '₽'"))));
            trHeader.Cells.Add(new TableCell(new Paragraph(new Run("Ко-во"))));
            trHeader.Cells.Add(new TableCell(new Paragraph(new Run("Итог  '₽'"))));


            TableRow trHeaderLine = new TableRow();
            trHeaderLine.Cells.Add(new TableCell(new Paragraph(new Run("----------"))));
            trHeaderLine.Cells.Add(new TableCell(new Paragraph(new Run("----------"))));
            trHeaderLine.Cells.Add(new TableCell(new Paragraph(new Run("----------"))));
            trHeaderLine.Cells.Add(new TableCell(new Paragraph(new Run("----------"))));
            table.RowGroups[0].Rows.Add(trHeaderLine);

            double totalItems = 0;
            foreach (productsaleorpurchaseviewmodel item in list)
            {
                totalItems += item.quantity;
                TableRow tr = new TableRow();
                table.RowGroups[0].Rows.Add(tr);
                tr.Cells.Add(new TableCell(new Paragraph(new Run(item.name))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToString(item.price)))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToString(item.quantity)))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToString(item.total)))));
            }


            middle.Blocks.Add(table);
            middle.Blocks.Add(new Paragraph(new Run("______________________________________")));
            middle.Blocks.Add(new Paragraph(new Run("Товар: "+ list.Count.ToString()+"     Количество: "+ totalItems.ToString())));
            middle.Blocks.Add(new Paragraph(new Run("______________________________________")));

            var totalBillLine = new Paragraph(new Bold(new Run("Сумма  '₽'                       " + totalBill)));
            totalBillLine.FontSize = 12;
            middle.Blocks.Add(totalBillLine);
            middle.Blocks.Add(new Paragraph(new Run("Оплаченно  '₽':      " + (totalpayment) + "  Остаток  '₽':      " + remaining)));






            Section footer = new Section();
            Paragraph footer1 = new Paragraph(new Run(Settings.Default.Footer));
            Paragraph footer2 = new Paragraph(new Run("Лицензтровано www.saledot.ru"));
            Paragraph footer3 = new Paragraph(new Run(customerAddress));
            footer1.FontSize = 9;
            footer2.FontSize = 9;
            footer3.FontSize = 9;
            footer.Blocks.Add(footer1);
            footer.Blocks.Add(footer2);

            if (printcustomerinfoonreceipt)
            {
                footer.Blocks.Add(footer3);
            }
            fd.Blocks.Add(header);
            fd.Blocks.Add(middle);
            fd.Blocks.Add(footer);
            return fd;
        }
    }
}
