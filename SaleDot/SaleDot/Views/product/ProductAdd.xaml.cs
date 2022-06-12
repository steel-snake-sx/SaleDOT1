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
using SaleDot.bll;
using SaleDot.data;

namespace SaleDot.Views.product
{
    public partial class ProductAdd : Window
    {
        bool createmode = true;
        data.dapper.product selectedproduct = null;
        data.dapper.productrepo productrepo;
        data.dapper.productsubrepo productsubrepo;
        public ProductAdd(int? productId = null)
        {
            this.productrepo = new data.dapper.productrepo();
            this.productsubrepo = new data.dapper.productsubrepo();
            InitializeComponent();
            if (productId == null)
            {
                this.createmode = true;
            }
            else
            {
                this.createmode = false;
                this.getone((int)productId);
            }
            initFormOperations();
        }
        private void btn_Save(object sender, RoutedEventArgs e)
        {
            if (tb_name.Text == "" || tb_saleprice.Text == "" || tb_purchaseprice.Text == "")
            {
                MessageBox.Show("Пожалуйста, заполните форму", "Информация");
                return;
            }
            if (this.createmode)
            {
                data.dapper.product r = new data.dapper.product();
                r.name = tb_name.Text;
                r.barcode = tb_barcode.Text;
                r.category = tb_category.Text;
                r.saleprice = Convert.ToInt32(tb_saleprice.Text);
                r.purchaseprice = Convert.ToInt32(tb_purchaseprice.Text);
                if (tb_discount.Text != "")
                {
                    try
                    {
                        r.discount = Convert.ToInt32(tb_discount.Text);
                    }
                    catch (Exception ex) { }
                }
                if (tb_carrycost.Text != "")
                {
                    try
                    {
                        r.carrycost = Convert.ToInt32(tb_carrycost.Text);
                    }
                    catch (Exception ex) { }
                }
                r.quantity = 0;
                if (tb_quantity.Text != "")
                {
                    try 
                    {
                        r.quantity = Convert.ToDouble(tb_quantity.Text);
                    }catch(Exception ex) { }
                }
                
                r.saleactive = cbx_SaleActive.IsChecked.Value;
                r.purchaseactive = cbx_PurchaseActive.IsChecked.Value;
                var savedproductresult = productrepo.save(r);
                if (savedproductresult)
                {
                    inventoryutils.updateinventorylogonproductcreate(r);
                    MessageBox.Show("Товар добавлен", "Информация");
                    Close();
                    new ProductAdd().Show();
                }
            }
            else
            {
                selectedproduct.name = tb_name.Text;
                selectedproduct.barcode = tb_barcode.Text;
                selectedproduct.category = tb_category.Text;
                selectedproduct.saleprice = Convert.ToInt32(tb_saleprice.Text);
                selectedproduct.purchaseprice = Convert.ToInt32(tb_purchaseprice.Text);
                if (tb_discount.Text != "")
                {
                    selectedproduct.discount = Convert.ToInt32(tb_discount.Text);
                }
                if (tb_carrycost.Text != "")
                {
                    selectedproduct.carrycost = Convert.ToInt32(tb_carrycost.Text);
                }
                double selectedproductoldinventory = (selectedproduct.quantity!=null)? (double)selectedproduct.quantity:0;
                if (tb_quantity.Text != "")
                {
                    try
                    {
                        selectedproduct.quantity = Convert.ToDouble(tb_quantity.Text);
                    }
                    catch (Exception ex) { }
                }
                selectedproduct.saleactive = cbx_SaleActive.IsChecked.Value;
                selectedproduct.purchaseactive = cbx_PurchaseActive.IsChecked.Value;



                var updateproductresult = this.productrepo.update(selectedproduct);
                if (updateproductresult)
                {
                    inventoryutils.updateinventorylogonproductupdate(selectedproduct.id,(double)selectedproduct.quantity, selectedproductoldinventory);
                    MessageBox.Show("Товар обновлен", "Информация");
                    Close();
                    new ProductAdd(selectedproduct.id).Show();
                }
                
            }
        }
        private void btn_Addproductsub(object sender, RoutedEventArgs e)
        {
            if (this.createmode)
            {
                MessageBox.Show("Сначала сохраните товар", "Информация");
            }
            else
            {
                if (productAutoComplete.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите товар", "Информация");
                    return;
                }
                if (tb_productsubquantity.Text == "" || tb_productsubquantity.Text == "0")
                {
                    MessageBox.Show("Пожалуйста, добавьте количество", "Информация");
                    return;
                }
                var products_cb_selectedobject = productAutoComplete.SelectedItem as data.dapper.product;
                data.dapper.productsub productsub = new data.dapper.productsub();
                productsub.fk_product_main_in_productsub = selectedproduct.id;
                productsub.fk_product_sub_in_productsub = products_cb_selectedobject.id;
                productsub.quantity = Convert.ToInt32(tb_productsubquantity.Text);
                productsubrepo.save(productsub);
                dg.Items.Clear();
                var productsubs = this.productsubrepo.getproduct_productsubs(this.selectedproduct.id);
                foreach (var item in productsubs)
                {
                    dg.Items.Add(item);
                }
                productAutoComplete.SelectedItem = null;
                productAutoComplete.autoTextBox.Text = "";
                tb_productsubquantity.Text = "";
            }
        }
        public void btn_removeproductsub(object sender, RoutedEventArgs e)
        {
            data.dapper.productsub obj = ((FrameworkElement)sender).DataContext as data.dapper.productsub;
            this.productsubrepo.delete(obj);
            dg.Items.Clear();
            var productsubs = this.productsubrepo.getproduct_productsubs(selectedproduct.id);
            foreach (var item in productsubs)
            {
                dg.Items.Add(item);
            }
        }



        void initFormOperations()
        {

        }
        void getone(int productid)
        {
            selectedproduct = productrepo.get(productid);
            tb_name.Text = selectedproduct.name;
            tb_category.Text = selectedproduct.category;
            if (selectedproduct.saleprice != null)
            {
                tb_saleprice.Text = selectedproduct.saleprice.ToString();
            }
            if (selectedproduct.purchaseprice != null)
            {
                tb_purchaseprice.Text = selectedproduct.purchaseprice.ToString();
            }
            if (selectedproduct.discount != null)
            {
                tb_discount.Text = selectedproduct.discount.ToString();
            }
            if (selectedproduct.carrycost != null)
            {
                tb_carrycost.Text = selectedproduct.carrycost.ToString();
            }
            if (selectedproduct.barcode != null)
            {
                tb_barcode.Text = selectedproduct.barcode.ToString();
            }
            if (selectedproduct.quantity != null)
            {
                tb_quantity.Text = selectedproduct.quantity.ToString();
            }

            cbx_SaleActive.IsChecked = selectedproduct.saleactive;
            cbx_PurchaseActive.IsChecked = selectedproduct.purchaseactive;

            var productsubs = this.productsubrepo.getproduct_productsubs(productid);

            foreach (var item in productsubs)
            {
                dg.Items.Add(item);
            }
        }
    }
}
