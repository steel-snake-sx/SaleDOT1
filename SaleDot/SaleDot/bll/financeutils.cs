using SaleDot.data;
using SaleDot.data.dapper;
using SaleDot.data.viewmodel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleDot.bll
{
    public class financeutils
    {
        public static int insertSaleTransactions(string accountname,List<productsaleorpurchaseviewmodel> saleList,double totalpayment, int targetuserid)
        {
            var loggedinuserid = userutils.loggedinuserd.id;

            productrepo productrepo = new productrepo();
            financeaccountrepo financeaccountrepo = new financeaccountrepo();
            financetransactionrepo financetransactionrepo = new financetransactionrepo();

            List<data.dapper.financeaccount> accounts = financeaccountrepo.get();
            var saleaccountid = accounts.Where(a => a.name == accountname).FirstOrDefault().id;
            var discountaccountid = accounts.Where(a => a.name == "Скидка").FirstOrDefault().id;
            var cashaccountid = accounts.Where(a => a.name == "Наличные").FirstOrDefault().id;
            var accountreciveableaccountid = accounts.Where(a => a.name == "Дебиторская задолженность").FirstOrDefault().id;
            var cgsaccountid = accounts.Where(a => a.name == "Другое").FirstOrDefault().id;
            var inventoryaccountid = accounts.Where(a => a.name == "Инвентаризация").FirstOrDefault().id;
      
            double totalbill = 0;
            double costofgoodssold = 0;
            foreach (var item in saleList)
            {
                totalbill += (item.price * item.quantity);
                data.dapper.product p = productrepo.get(item.id);
                double productcarrycost = 0;
                if (p.carrycost != null) {
                    productcarrycost = (double)p.carrycost;
                }
                double productpurchaseprice = 0;
                if (p.purchaseprice != null)
                {
                    productpurchaseprice = (double)p.purchaseprice;
                }
                costofgoodssold += ((double)((productpurchaseprice + productcarrycost) * item.quantity));
            }

            data.dapper.financetransaction ftsale = new data.dapper.financetransaction();
            ftsale.amount = -totalbill;
            
            ftsale.date = DateTime.Now;
            ftsale.status = "Опубликовано";
            ftsale.fk_user_createdby_in_financetransaction = loggedinuserid;
            if (targetuserid != 0)
            {
                ftsale.fk_user_targetto_in_financetransaction = targetuserid;
            }
            ftsale.fk_financeaccount_in_financetransaction = saleaccountid;
            financetransactionrepo.save(ftsale);

            if (totalpayment > 0)
            {
                data.dapper.financetransaction ftpayment = new data.dapper.financetransaction();
                ftpayment.amount = totalpayment;
                ftpayment.date = DateTime.Now;
                ftpayment.status = "Опубликовано";
                ftpayment.fk_user_createdby_in_financetransaction = loggedinuserid;
                if (targetuserid != 0)
                {
                    ftpayment.fk_user_targetto_in_financetransaction = targetuserid;
                }
                ftpayment.fk_financeaccount_in_financetransaction = cashaccountid;
                financetransactionrepo.save(ftpayment);
            }

            if (totalpayment!=totalbill)
            {
                data.dapper.financetransaction ftar = new data.dapper.financetransaction();
                ftar.amount = totalbill - totalpayment;
                
                ftar.date = DateTime.Now;
                ftar.status = "Опубликовано";
                ftar.fk_user_createdby_in_financetransaction = loggedinuserid;
                if (targetuserid != 0)
                {
                    ftar.fk_user_targetto_in_financetransaction = targetuserid;
                }
                ftar.fk_financeaccount_in_financetransaction = accountreciveableaccountid;
                financetransactionrepo.save(ftar);
            }

            data.dapper.financetransaction ftcgs = new data.dapper.financetransaction();
            ftcgs.amount = costofgoodssold;
            ftcgs.fk_financeaccount_in_financetransaction = cgsaccountid;
            ftcgs.date = DateTime.Now;
            ftcgs.status = "Опубликовано";
            ftcgs.fk_user_createdby_in_financetransaction = loggedinuserid;
            financetransactionrepo.save(ftcgs);

            data.dapper.financetransaction ftid = new data.dapper.financetransaction();
            ftid.name = "Продажа запасов";
            ftid.amount = -costofgoodssold;
            ftid.fk_financeaccount_in_financetransaction = inventoryaccountid;
            ftid.date = DateTime.Now;
            ftid.status = "Опубликовано";
            ftid.fk_user_createdby_in_financetransaction = loggedinuserid;
            financetransactionrepo.save(ftid);


            return ftsale.id;
            
        }

        public static int insertPurchaseTransactions(List<productsaleorpurchaseviewmodel> purchaseList, double totalpayment, int targetuserid)
        {
            var loggedinuserid = userutils.loggedinuserd.id;

            productrepo productrepo = new productrepo();
            financeaccountrepo financeaccountrepo = new financeaccountrepo();
            financetransactionrepo financetransactionrepo = new financetransactionrepo();

            List<data.dapper.financeaccount> accounts = financeaccountrepo.get();

            var discountaccountid = accounts.Where(a => a.name == "Скидка").FirstOrDefault().id;
            var cashaccountid = accounts.Where(a => a.name == "Наличные").FirstOrDefault().id;
            var accountreciveableaccountid = accounts.Where(a => a.name == "Дебиторская задолженность").FirstOrDefault().id;
            var accountpayableableaccountid = accounts.Where(a => a.name == "Кредиторская задолженность").FirstOrDefault().id;
            var cgsaccountid = accounts.Where(a => a.name == "Другое").FirstOrDefault().id;
            var inventoryaccountid = accounts.Where(a => a.name == "Оплата инвентаря").FirstOrDefault().id;

            double totalbill = 0;
            foreach (var item in purchaseList)
            {
                totalbill += (item.price * item.quantity);
            }


            //New purchase Transaction
            data.dapper.financetransaction ftpurchase = new data.dapper.financetransaction();
            ftpurchase.amount = totalbill;
            ftpurchase.name = "Покупка запасов";
            ftpurchase.fk_financeaccount_in_financetransaction = inventoryaccountid;
            ftpurchase.fk_user_targetto_in_financetransaction = targetuserid;
            ftpurchase.date = DateTime.Now;
            ftpurchase.status = "Опубликовано";
            ftpurchase.fk_user_createdby_in_financetransaction = loggedinuserid;
            financetransactionrepo.save(ftpurchase);

            if (totalpayment > 0)
            {
                data.dapper.financetransaction ftpayment = new data.dapper.financetransaction();
                ftpayment.amount = -(totalpayment);
                ftpayment.date = DateTime.Now;
                ftpayment.status = "Опубликовано";
                ftpayment.fk_user_createdby_in_financetransaction = loggedinuserid;
                ftpayment.fk_user_targetto_in_financetransaction = targetuserid;
                ftpayment.fk_financeaccount_in_financetransaction = cashaccountid;
                financetransactionrepo.save(ftpayment);
            }


            // New AP Transaction if TotalRemaining has ammount
            if ( totalbill!= totalpayment)
            {
                data.dapper.financetransaction ftap = new data.dapper.financetransaction();
                ftap.amount = -(totalbill - totalpayment);
                ftap.date = DateTime.Now;
                ftap.status = "Опубликовано";
                ftap.fk_user_createdby_in_financetransaction = loggedinuserid;
                ftap.fk_user_targetto_in_financetransaction = targetuserid;
                ftap.fk_financeaccount_in_financetransaction = accountpayableableaccountid;
                financetransactionrepo.save(ftap);
            }
            return ftpurchase.id;

        }

        public static void insertCustomerPayment(int accountid, double amount, int targetid) 
        {
            var loggedinuserid = userutils.loggedinuserd.id;

            financeaccountrepo financeaccountrepo = new financeaccountrepo();
            financetransactionrepo financetransactionrepo = new financetransactionrepo();
            List<data.dapper.financeaccount> accounts = financeaccountrepo.get();
            var accountreciveableaccountid = accounts.Where(a => a.name == "Дебиторская задолженность").FirstOrDefault().id;


            data.dapper.financetransaction ftcash = new data.dapper.financetransaction();
            ftcash.amount = amount;
            ftcash.date = DateTime.Now;
            ftcash.status = "Опубликовано";
            ftcash.fk_user_createdby_in_financetransaction = loggedinuserid;
            ftcash.fk_user_targetto_in_financetransaction = targetid;
            ftcash.fk_financeaccount_in_financetransaction = accountid;
            financetransactionrepo.save(ftcash);


            data.dapper.financetransaction ftar = new data.dapper.financetransaction();
            ftar.amount = -amount;
            ftar.date = DateTime.Now;
            ftar.status = "Опубликовано";
            ftar.fk_user_createdby_in_financetransaction = loggedinuserid;
            ftar.fk_user_targetto_in_financetransaction = targetid;
            ftar.fk_financeaccount_in_financetransaction = accountreciveableaccountid;
            financetransactionrepo.save(ftar);
        }

        public static void insertVendorPayment(int accountid, double amount, int targetid)
        {
            var loggedinuserid = userutils.loggedinuserd.id;
            financeaccountrepo financeaccountrepo = new financeaccountrepo();
            financetransactionrepo financetransactionrepo = new financetransactionrepo();
            List<data.dapper.financeaccount> accounts = financeaccountrepo.get();
            var accountpayableableaccountid = accounts.Where(a => a.name == "Кредиторская задолженность").FirstOrDefault().id; ;


            data.dapper.financetransaction ftcash = new data.dapper.financetransaction();
            ftcash.amount = -amount;
            ftcash.date = DateTime.Now;
            ftcash.status = "Опубликовано";
            ftcash.fk_user_createdby_in_financetransaction = loggedinuserid;
            ftcash.fk_user_targetto_in_financetransaction = targetid;
            ftcash.fk_financeaccount_in_financetransaction = accountid;
            financetransactionrepo.save(ftcash);


            data.dapper.financetransaction ftar = new data.dapper.financetransaction();
            ftar.amount = amount;
            ftar.date = DateTime.Now;
            ftar.status = "Опубликовано";
            ftar.fk_user_createdby_in_financetransaction = loggedinuserid;
            ftar.fk_user_targetto_in_financetransaction = targetid;
            ftar.fk_financeaccount_in_financetransaction = accountpayableableaccountid;
            financetransactionrepo.save(ftar);
        }

        public static void insertexpence(int payingaccount,  int expenceaccount, double amount)
        {
            var loggedinuserid = userutils.loggedinuserd.id;
            financeaccountrepo financeaccountrepo = new financeaccountrepo();
            financetransactionrepo financetransactionrepo = new financetransactionrepo();
            List<data.dapper.financeaccount> accounts = financeaccountrepo.get();
            data.dapper.financetransaction ftexpence = new data.dapper.financetransaction();
            ftexpence.amount = amount;
            ftexpence.fk_financeaccount_in_financetransaction = expenceaccount;
            ftexpence.date = DateTime.Now;
            ftexpence.status = "Опубликовано";
            ftexpence.fk_user_createdby_in_financetransaction = loggedinuserid;
            financetransactionrepo.save(ftexpence);

            data.dapper.financetransaction ftasset = new data.dapper.financetransaction();
            ftasset.amount = -amount;
            ftasset.fk_financeaccount_in_financetransaction = payingaccount;
            ftasset.date = DateTime.Now;
            ftasset.status = "Опубликовано";
            ftasset.fk_user_createdby_in_financetransaction = loggedinuserid;
            financetransactionrepo.save(ftasset);
        }

        public static void inserttransaction(int fromaccount, int toaccount, double amount)
        {
            var loggedinuserid = userutils.loggedinuserd.id;
            financeaccountrepo financeaccountrepo = new financeaccountrepo();
            financetransactionrepo financetransactionrepo = new financetransactionrepo();
            List<data.dapper.financeaccount> accounts = financeaccountrepo.get();

            data.dapper.financetransaction ftexpence = new data.dapper.financetransaction();
            ftexpence.amount = -amount;
            ftexpence.fk_financeaccount_in_financetransaction = fromaccount;
            ftexpence.date = DateTime.Now;
            ftexpence.status = "Опубликовано";
            ftexpence.fk_user_createdby_in_financetransaction = loggedinuserid;
            financetransactionrepo.save(ftexpence);

            data.dapper.financetransaction ftasset = new data.dapper.financetransaction();
            ftasset.amount = amount;
            ftasset.fk_financeaccount_in_financetransaction = toaccount;
            ftasset.date = DateTime.Now;
            ftasset.status = "Опубликовано";
            ftasset.fk_user_createdby_in_financetransaction = loggedinuserid;
            financetransactionrepo.save(ftasset);
        }

        public static void getaccountsbalances()
        {

        }

        public static void finance_transaction_getAll_groupby_Month(string FinanceAccount)
        {

        }

    }
}
