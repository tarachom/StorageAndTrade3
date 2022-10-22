

using Gtk;
using AccountingSoftware;

namespace StorageAndTrade_1_0.Довідники.ТабличніСписки
{
    
    #region DIRECTORY "Організації"
    
      
    public class Організації_Записи
    {
        public string Image = "doc.png";
        public string ID = "";
        
        public string Назва = "";
        public string Код = "";

        public Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID /* */, Назва, Код };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf), typeof(string) /* */, typeof(string), typeof(string));

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 3));
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Організації_Select Організації_Select = new Довідники.Організації_Select();
            Організації_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Організації_Const.Назва, Довідники.Організації_Const.Код
                });

            Організації_Select.Select();
            while (Організації_Select.MoveNext())
            {
                Довідники.Організації_Pointer? cur = Організації_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Організації_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[Довідники.Організації_Const.Назва]?.ToString() ?? "", //
                        Код = cur.Fields?[Довідники.Організації_Const.Код]?.ToString() ?? "" //
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Номенклатура"
    
      
    #endregion
    
    #region DIRECTORY "Виробники"
    
      
    #endregion
    
    #region DIRECTORY "ВидиНоменклатури"
    
      
    #endregion
    
    #region DIRECTORY "ПакуванняОдиниціВиміру"
    
      
    #endregion
    
    #region DIRECTORY "Валюти"
    
      
    #endregion
    
    #region DIRECTORY "Контрагенти"
    
      
    #endregion
    
    #region DIRECTORY "Склади"
    
      
    #endregion
    
    #region DIRECTORY "ВидиЦін"
    
      
    #endregion
    
    #region DIRECTORY "ВидиЦінПостачальників"
    
      
    #endregion
    
    #region DIRECTORY "Користувачі"
    
      
    #endregion
    
    #region DIRECTORY "ФізичніОсоби"
    
      
    #endregion
    
    #region DIRECTORY "СтруктураПідприємства"
    
      
    #endregion
    
    #region DIRECTORY "КраїниСвіту"
    
      
    #endregion
    
    #region DIRECTORY "Файли"
    
      
    #endregion
    
    #region DIRECTORY "ХарактеристикиНоменклатури"
    
      
    #endregion
    
    #region DIRECTORY "Номенклатура_Папки"
    
      
    #endregion
    
    #region DIRECTORY "Контрагенти_Папки"
    
      
    #endregion
    
    #region DIRECTORY "Склади_Папки"
    
      
    #endregion
    
    #region DIRECTORY "Каси"
    
      
    #endregion
    
    #region DIRECTORY "БанківськіРахункиОрганізацій"
    
      
    #endregion
    
    #region DIRECTORY "ДоговориКонтрагентів"
    
      
    #endregion
    
    #region DIRECTORY "БанківськіРахункиКонтрагентів"
    
      
    #endregion
    
    #region DIRECTORY "СтаттяРухуКоштів"
    
      
    #endregion
    
    #region DIRECTORY "СеріїНоменклатури"
    
      
    #endregion
    
    #region DIRECTORY "ПартіяТоварівКомпозит"
    
      
    #endregion
    
    #region DIRECTORY "ВидиЗапасів"
    
      
    #endregion
    
    #region DIRECTORY "ПродажіДокументКомпозит"
    
      
    #endregion
    
    #region DIRECTORY "АналітикаНоменклатуриКомпозит"
    
      
    #endregion
    
    #region DIRECTORY "АналітикаКонтрагентівКомпозит"
    
      
    #endregion
    
    #region DIRECTORY "АналітикаПартійКомпозит"
    
      
    #endregion
    
}

  