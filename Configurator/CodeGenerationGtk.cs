

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
        public string Склад = "";

        public Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, Код, Склад };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Код */
            , typeof(string) /* Склад */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 3));
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 4));
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Організації_Select Організації_Select = new Довідники.Організації_Select();
            Організації_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Організації_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.Організації_Const.Код // [ pos = 2, type = string ]
                    /* , Довідники.Організації_Const.Склад */ // [ pos = 3, type = pointer ]
                    
                });

            
                  /* JOIN 3 */
                  Організації_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Код, "join_3"));
                  Організації_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Довідники.Організації_Const.Склад, Організації_Select.QuerySelect.Table));
                

            /* SELECT */
            Організації_Select.Select();
            while (Організації_Select.MoveNext())
            {
                Довідники.Організації_Pointer? cur = Організації_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Організації_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[Організації_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[Організації_Const.Код]?.ToString() ?? "", // [ pos = 2, type = string ]
                        Склад = cur.Fields?["join_3"]?.ToString() ?? "" // [ pos = 3, type = pointer ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Номенклатура"
    
      
    public class Номенклатура_Записи
    {
        public string Image = "doc.png";
        public string ID = "";
        
        public string Назва = "";
        public string Код = "";
        public string Артикул = "";
        public string ТипНоменклатури = "";
        public string Виробник = "";
        public string ВидНоменклатури = "";
        public string ОдиницяВиміру = "";
        public string Папка = "";
        public string ОсновнаКартинкаФайл = "";
        public string Suma = "";

        public Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, Код, Артикул, ТипНоменклатури, Виробник, ВидНоменклатури, ОдиницяВиміру, Папка, ОсновнаКартинкаФайл, Suma };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Код */
            , typeof(string) /* Артикул */
            , typeof(string) /* ТипНоменклатури */
            , typeof(string) /* Виробник */
            , typeof(string) /* ВидНоменклатури */
            , typeof(string) /* ОдиницяВиміру */
            , typeof(string) /* Папка */
            , typeof(string) /* ОсновнаКартинкаФайл */
            , typeof(string) /* Suma */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 3));
            treeView.AppendColumn(new TreeViewColumn("Артикул", new CellRendererText(), "text", 4));
            treeView.AppendColumn(new TreeViewColumn("ТипНоменклатури", new CellRendererText(), "text", 5));
            treeView.AppendColumn(new TreeViewColumn("Виробник", new CellRendererText(), "text", 6));
            treeView.AppendColumn(new TreeViewColumn("ВидНоменклатури", new CellRendererText(), "text", 7));
            treeView.AppendColumn(new TreeViewColumn("ОдиницяВиміру", new CellRendererText(), "text", 8));
            treeView.AppendColumn(new TreeViewColumn("Папка", new CellRendererText(), "text", 9));
            treeView.AppendColumn(new TreeViewColumn("ОсновнаКартинкаФайл", new CellRendererText(), "text", 10));
            treeView.AppendColumn(new TreeViewColumn("Suma", new CellRendererText(), "text", 11));
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Номенклатура_Select Номенклатура_Select = new Довідники.Номенклатура_Select();
            Номенклатура_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Номенклатура_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.Номенклатура_Const.Код // [ pos = 2, type = string ]
                    , Довідники.Номенклатура_Const.Артикул // [ pos = 3, type = string ]
                    , Довідники.Номенклатура_Const.ТипНоменклатури // [ pos = 4, type = enum ]
                    /* , Довідники.Номенклатура_Const.Виробник */ // [ pos = 5, type = pointer ]
                    /* , Довідники.Номенклатура_Const.ВидНоменклатури */ // [ pos = 6, type = pointer ]
                    /* , Довідники.Номенклатура_Const.ОдиницяВиміру */ // [ pos = 7, type = pointer ]
                    /* , Довідники.Номенклатура_Const.Папка */ // [ pos = 8, type = pointer ]
                    /* , Довідники.Номенклатура_Const.ОсновнаКартинкаФайл */ // [ pos = 9, type = pointer ]
                    , Довідники.Номенклатура_Const.Suma // [ pos = 10, type = integer ]
                    
                });

            
                  /* JOIN 5 */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Виробники_Const.TABLE + "." + Довідники.Виробники_Const.Назва, "join_5"));
                  Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Виробники_Const.TABLE, Довідники.Номенклатура_Const.Виробник, Номенклатура_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.ВидиНоменклатури_Const.TABLE + "." + Довідники.ВидиНоменклатури_Const.Назва, "join_6"));
                  Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ВидиНоменклатури_Const.TABLE, Довідники.Номенклатура_Const.ВидНоменклатури, Номенклатура_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.ПакуванняОдиниціВиміру_Const.TABLE + "." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "join_7"));
                  Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, Довідники.Номенклатура_Const.ОдиницяВиміру, Номенклатура_Select.QuerySelect.Table));
                
                  /* JOIN 8 */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Номенклатура_Папки_Const.TABLE + "." + Довідники.Номенклатура_Папки_Const.Назва, "join_8"));
                  Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Номенклатура_Папки_Const.TABLE, Довідники.Номенклатура_Const.Папка, Номенклатура_Select.QuerySelect.Table));
                
                  /* JOIN 9 */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Файли_Const.TABLE + "." + Довідники.Файли_Const.Назва, "join_9"));
                  Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Файли_Const.TABLE, Довідники.Номенклатура_Const.ОсновнаКартинкаФайл, Номенклатура_Select.QuerySelect.Table));
                

            /* SELECT */
            Номенклатура_Select.Select();
            while (Номенклатура_Select.MoveNext())
            {
                Довідники.Номенклатура_Pointer? cur = Номенклатура_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Номенклатура_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[Номенклатура_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[Номенклатура_Const.Код]?.ToString() ?? "", // [ pos = 2, type = string ]
                        Артикул = cur.Fields?[Номенклатура_Const.Артикул]?.ToString() ?? "", // [ pos = 3, type = string ]
                        ТипНоменклатури = ((Перелічення.ТипиНоменклатури)int.Parse(cur.Fields?[Номенклатура_Const.ТипНоменклатури]?.ToString() ?? "0")).ToString(), // [ pos = 4, type = enum ]
                        Виробник = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        ВидНоменклатури = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        ОдиницяВиміру = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        Папка = cur.Fields?["join_8"]?.ToString() ?? "", // [ pos = 8, type = pointer ]
                        ОсновнаКартинкаФайл = cur.Fields?["join_9"]?.ToString() ?? "", // [ pos = 9, type = pointer ]
                        Suma = cur.Fields?[Номенклатура_Const.Suma]?.ToString() ?? "" // [ pos = 10, type = integer ]
                        
                    }.ToArray());
            }
        }
    }
	    
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

  