using SaleDot.data.dapper;
using SaleDot.data.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace SaleDot.bll
{
    public class userutils
    {
        public static data.dapper.user loggedinuserd { get; set; }
        public static string membership { get; set; }
        public static softwaresetting saledotuserid;
        public static softwaresetting saledotusername;
        public static softwaresetting saledotpassword;
        public static softwaresetting saledotmembershipplan;
        public static softwaresetting ravicosoftFreePOSmembershipexpirydate;
        public static softwaresetting ravicosoftFreePOScanrun;
        public static softwaresetting saledotsmsplan;
        public static softwaresetting apiendpoint;

        public static void authorizerole(Window window, string[] roles)
        {

            if (roles.Contains(loggedinuserd.role))
            {
                window.Show();
            }
            else
            {
                RadDesktopAlertManager manager = new RadDesktopAlertManager();
                var alert = new RadDesktopAlert();
                alert.Header = "Предупреждение";
                alert.Content = "Эта страница может быть доступна только " + String.Join(",", roles);
                alert.ShowDuration = 5000;
                System.Media.SystemSounds.Hand.Play();
                manager.ShowAlert(alert);
            }
        }
        
        public static bool checkravicosoftuseridexits() 
        {
            var ravicosoftuser = saledotuserid;
            if (ravicosoftuser == null)
            {
                return false;
            }
            else {
                return true;
            }
        }
        public static void loadsoftwaresetting()
        {
            var ssr = new softwaresettingrepo();
            saledotuserid = ssr.getbyname(commonsettingfields.saledotuserid);
            saledotusername = ssr.getbyname(commonsettingfields.saledotusername);
            saledotpassword = ssr.getbyname(commonsettingfields.saledotpassword);
            saledotmembershipplan = ssr.getbyname(commonsettingfields.saledotmembershipplan);
            saledotsmsplan = ssr.getbyname(commonsettingfields.saledotsmsplan);
            apiendpoint = ssr.getbyname(commonsettingfields.apiendpoint);

        }
        public static void updateapiendpoint(string newurl)
        {
            var ssr = new softwaresettingrepo();
            if (apiendpoint == null)
            {
                var ss = new softwaresetting() { name = commonsettingfields.apiendpoint, valuetype = "string", stringvalue = newurl };
                apiendpoint = ssr.save(ss);
            }
            else
            {
                apiendpoint.stringvalue = newurl;
                apiendpoint = ssr.update(apiendpoint);
            }
        }

        public static void updateusersetting(apiresponseuserclass user) 
        {
            softwaresettingrepo ssr = new softwaresettingrepo();
            var saledotuserid = ssr.getbyname(commonsettingfields.saledotuserid);
            if (saledotuserid == null)
            {
                var ss = new softwaresetting();
                ss.name = commonsettingfields.saledotuserid;
                ss.valuetype = "string";
                ss.stringvalue = user._id;
                userutils.saledotuserid = ssr.save(ss);
            }
            else
            {
                saledotuserid.valuetype = "string";
                saledotuserid.stringvalue = user._id;
                userutils.saledotuserid = ssr.update(saledotuserid);
            }

            var username = ssr.getbyname(commonsettingfields.saledotusername);
            if (username == null)
            {
                var ss = new softwaresetting();
                ss.name = commonsettingfields.saledotusername;
                ss.valuetype = "string";
                ss.stringvalue = user.username;
                userutils.saledotusername = ssr.save(ss);
            }
            else
            {
                username.valuetype = "string";
                username.stringvalue = user.username;
                userutils.saledotusername = ssr.update(username);
            }


            var userpassword = ssr.getbyname(commonsettingfields.saledotpassword);
            if (userpassword == null)
            {
                var ss = new softwaresetting();
                ss.name = commonsettingfields.saledotpassword;
                ss.valuetype = "string";
                ss.stringvalue = user.password;
                userutils.saledotpassword = ssr.save(ss);
            }
            else
            {
                userpassword.valuetype = "string";
                userpassword.stringvalue = user.password;
                userutils.saledotpassword = ssr.update(userpassword);
            }

            var membershiptype = ssr.getbyname(commonsettingfields.saledotmembershipplan);
            if (membershiptype == null)
            {
                var ss = new softwaresetting();
                ss.name = commonsettingfields.saledotmembershipplan;
                ss.valuetype = "string";
                ss.stringvalue = user.FreePOSmembershipplan;
                userutils.saledotmembershipplan = ssr.save(ss);
            }
            else
            {
                membershiptype.valuetype = "string";
                membershiptype.stringvalue = user.FreePOSmembershipplan;
                userutils.saledotmembershipplan = ssr.update(membershiptype);
            }
            

            var cansendsms = ssr.getbyname(commonsettingfields.saledotsmsplan);
            if (cansendsms == null)
            {
                var ss = new softwaresetting();
                ss.name = commonsettingfields.saledotsmsplan;
                ss.valuetype = "string";
                ss.stringvalue = user.smsplan;
                userutils.saledotsmsplan = ssr.save(ss);
            }
            else
            {
                cansendsms.valuetype = "string";
                cansendsms.stringvalue = user.smsplan;
                userutils.saledotsmsplan = ssr.update(cansendsms);
            }
        }
    }
    
}
