using SaleDot.data;
using SaleDot.data.dapper;
using SaleDot.data.viewmodel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SaleDot.bll
{
    public class inventoryutils
    {
        public static void updateInventoryonsale(List<productsaleorpurchaseviewmodel> salelist,int saleid)
        {
            foreach (var item in salelist)
            {
                recursiveupdateinventoryonsale(item.id, item.quantity,saleid,"");
            }
        }

        private static void recursiveupdateinventoryonsale(int productid,double productquantity, int saleid,string inventorylogcomment)
        {
            var productrepo = new productrepo();
            data.dapper.product p = productrepo.get(productid);
            var productsubrepo = new productsubrepo();
            var productsubs = productsubrepo.getproduct_productsubs(productid);
            if(productsubs.Count == 0)
            {
                p.quantity = p.quantity - productquantity;
                productrepo.update(p);
                updateinventorylogonsale(productid, productquantity, saleid, inventorylogcomment);
            }
            foreach (var productsub in productsubs)
            {
                recursiveupdateinventoryonsale(productsub.fk_product_sub_in_productsub, productquantity * productsub.quantity,saleid, ", Продается как комплектующие " + p.name);
            }
        }
        private static void updateinventorylogonsale(int productid, double productquantity, int saleid,string comment)
        {
            var inventorylogrepo = new inventorylogrepo();
            data.dapper.inventorylog ir = new inventorylog();
            ir.quantity = -productquantity;
            ir.date = DateTime.Now;
            ir.fk_product_in_inventorylog = productid;
            ir.note = "Вычтено с инвенторя по ID продажи " + saleid+comment;
            inventorylogrepo.save(ir);
        }

        public static void updateInventoryonpurchase(List<productsaleorpurchaseviewmodel> purchaseList,int purchaseid)
        {
            foreach (var item in purchaseList)
            {
                recursiveupdateinventoryonpurchase(item.id, item.quantity, purchaseid,"");
            }
        }
        private static void recursiveupdateinventoryonpurchase(int productid, double productquantity, int purchaseid, string inventorylogcomment)
        {
            var productrepo = new productrepo();
            data.dapper.product p = productrepo.get(productid);
            var productsubrepo = new productsubrepo();
            var productsubs = productsubrepo.getproduct_productsubs(productid);
            if (productsubs.Count == 0)
            {
                p.quantity = p.quantity + productquantity;
                productrepo.update(p);
                updateinventorylogonpurchase(productid, productquantity, purchaseid, inventorylogcomment);
            }
            foreach (var productsub in productsubs)
            {
                recursiveupdateinventoryonpurchase(productsub.fk_product_sub_in_productsub, productquantity * productsub.quantity, purchaseid, ", purchased as sub of " + p.name);
            }
        }
        private static void updateinventorylogonpurchase(int productid, double productquantity, int purchaseid, string inventorylogcomment)
        {
            var inventorylogrepo = new inventorylogrepo();
            data.dapper.inventorylog ir = new inventorylog();
            ir.quantity = productquantity;
            ir.date = DateTime.Now;
            ir.fk_product_in_inventorylog = productid;
            ir.note = "Добавлено в инвентарь по ID покупки " + purchaseid+inventorylogcomment;
            inventorylogrepo.save(ir);
        }
        public static void updateinventorylogonproductcreate(data.dapper.product p)
        {
            if (p.quantity != 0)
            {
                var inventorylogrepo = new inventorylogrepo();
                data.dapper.inventorylog ir = new inventorylog();
                ir.quantity = p.quantity;
                ir.date = DateTime.Now;
                ir.fk_product_in_inventorylog = p.id;
                ir.note = "В инвентарь добавлен новый товар";
                inventorylogrepo.save(ir);
            }
        }

        public static void updateinventorylogonproductupdate(int productid, double newinventory, double oldinventory)
        {
            if (newinventory==oldinventory)
            {
                return;
            }
            var inventorylogrepo = new inventorylogrepo();
            data.dapper.inventorylog ir = new inventorylog();
            ir.date = DateTime.Now;
            ir.fk_product_in_inventorylog = productid;
            ir.quantity = newinventory- oldinventory;
            if (newinventory > oldinventory)
            {
                ir.note = "Товар обновлен";
            }
            else if(newinventory < oldinventory)
            {
                ir.note = "Вычет запасов при обновлении товара";
            }
            inventorylogrepo.save(ir);
        }
    }
}
