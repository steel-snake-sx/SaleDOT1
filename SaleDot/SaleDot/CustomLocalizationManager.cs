using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace SaleDot
{
    public class CustomLocalizationManager : LocalizationManager
    {
        public override string GetStringOverride(string key)
        {
            switch (key)
            {
                case "GridViewSearchPanelTopText":
                    return "Введите текст поиска";
                case "GridViewGroupPanelText":
                    return "Нажмите на заголовок столбца, чтобы сгруппировать по этому столбцу";
                //---------------------- RadGridView Filter Dropdown items texts: 
                case "edit":
                    return "Редактировать";
            }
            return base.GetStringOverride(key);
        }
    }
}
