

using Gtk;
using AccountingSoftware;

namespace StorageAndTrade_1_0.Довідники.ТабличніСписки
{
    
    #region DIRECTORY "Організації"
    
      
    public class Організації_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Організації_Select Організації_Select = new Довідники.Організації_Select();
            Організації_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Організації_Const.Код // [ pos = 1, type = string ]
                    , Довідники.Організації_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            Організації_Select.Select();
            while (Організації_Select.MoveNext())
            {
                Довідники.Організації_Pointer? cur = Організації_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Організації_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Організації_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[Організації_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Номенклатура"
    
      
    public class Номенклатура_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ОдиницяВиміру = "";
        string Виробник = "";
        string ТипНоменклатури = "";
        string ВидНоменклатури = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, ОдиницяВиміру, Виробник, ТипНоменклатури, ВидНоменклатури };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* ОдиницяВиміру */
            , typeof(string) /* Виробник */
            , typeof(string) /* ТипНоменклатури */
            , typeof(string) /* ВидНоменклатури */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Одиниця пакування", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Виробник", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Вид", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Номенклатура_Select Номенклатура_Select = new Довідники.Номенклатура_Select();
            Номенклатура_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Номенклатура_Const.Код // [ pos = 1, type = string ]
                    , Довідники.Номенклатура_Const.Назва // [ pos = 2, type = string ]
                    /* , Довідники.Номенклатура_Const.ОдиницяВиміру */ // [ pos = 3, type = pointer ]
                    /* , Довідники.Номенклатура_Const.Виробник */ // [ pos = 4, type = pointer ]
                    , Довідники.Номенклатура_Const.ТипНоменклатури // [ pos = 5, type = enum ]
                    /* , Довідники.Номенклатура_Const.ВидНоменклатури */ // [ pos = 6, type = pointer ]
                    
                });

            
                  /* JOIN 3 */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.ПакуванняОдиниціВиміру_Const.TABLE + "." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "join_3"));
                  Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, Довідники.Номенклатура_Const.ОдиницяВиміру, Номенклатура_Select.QuerySelect.Table));
                
                  /* JOIN 4 */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Виробники_Const.TABLE + "." + Довідники.Виробники_Const.Назва, "join_4"));
                  Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Виробники_Const.TABLE, Довідники.Номенклатура_Const.Виробник, Номенклатура_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.ВидиНоменклатури_Const.TABLE + "." + Довідники.ВидиНоменклатури_Const.Назва, "join_6"));
                  Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ВидиНоменклатури_Const.TABLE, Довідники.Номенклатура_Const.ВидНоменклатури, Номенклатура_Select.QuerySelect.Table));
                

            /* SELECT */
            Номенклатура_Select.Select();
            while (Номенклатура_Select.MoveNext())
            {
                Довідники.Номенклатура_Pointer? cur = Номенклатура_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Номенклатура_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Номенклатура_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[Номенклатура_Const.Назва]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ОдиницяВиміру = cur.Fields?["join_3"]?.ToString() ?? "", // [ pos = 3, type = pointer ]
                        Виробник = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        ТипНоменклатури = ((Перелічення.ТипиНоменклатури)int.Parse(cur.Fields?[Номенклатура_Const.ТипНоменклатури]?.ToString() ?? "0")).ToString(), // [ pos = 5, type = enum ]
                        ВидНоменклатури = cur.Fields?["join_6"]?.ToString() ?? "" // [ pos = 6, type = pointer ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Виробники"
    
      
    public class Виробники_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Виробники_Select Виробники_Select = new Довідники.Виробники_Select();
            Виробники_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Виробники_Const.Код // [ pos = 1, type = string ]
                    , Довідники.Виробники_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            Виробники_Select.Select();
            while (Виробники_Select.MoveNext())
            {
                Довідники.Виробники_Pointer? cur = Виробники_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Виробники_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Виробники_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[Виробники_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиНоменклатури"
    
      
    public class ВидиНоменклатури_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ВидиНоменклатури_Select ВидиНоменклатури_Select = new Довідники.ВидиНоменклатури_Select();
            ВидиНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ВидиНоменклатури_Const.Код // [ pos = 1, type = string ]
                    , Довідники.ВидиНоменклатури_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            ВидиНоменклатури_Select.Select();
            while (ВидиНоменклатури_Select.MoveNext())
            {
                Довідники.ВидиНоменклатури_Pointer? cur = ВидиНоменклатури_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ВидиНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ВидиНоменклатури_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[ВидиНоменклатури_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ПакуванняОдиниціВиміру"
    
      
    public class ПакуванняОдиниціВиміру_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ПакуванняОдиниціВиміру_Select ПакуванняОдиниціВиміру_Select = new Довідники.ПакуванняОдиниціВиміру_Select();
            ПакуванняОдиниціВиміру_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ПакуванняОдиниціВиміру_Const.Код // [ pos = 1, type = string ]
                    , Довідники.ПакуванняОдиниціВиміру_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            ПакуванняОдиниціВиміру_Select.Select();
            while (ПакуванняОдиниціВиміру_Select.MoveNext())
            {
                Довідники.ПакуванняОдиниціВиміру_Pointer? cur = ПакуванняОдиниціВиміру_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПакуванняОдиниціВиміру_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ПакуванняОдиниціВиміру_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[ПакуванняОдиниціВиміру_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Валюти"
    
      
    public class Валюти_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string КороткаНазва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, КороткаНазва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* КороткаНазва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Коротка назва", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Валюти_Select Валюти_Select = new Довідники.Валюти_Select();
            Валюти_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Валюти_Const.Код // [ pos = 1, type = string ]
                    , Довідники.Валюти_Const.Назва // [ pos = 2, type = string ]
                    , Довідники.Валюти_Const.КороткаНазва // [ pos = 3, type = string ]
                    
                });

            

            /* SELECT */
            Валюти_Select.Select();
            while (Валюти_Select.MoveNext())
            {
                Довідники.Валюти_Pointer? cur = Валюти_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Валюти_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Валюти_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[Валюти_Const.Назва]?.ToString() ?? "", // [ pos = 2, type = string ]
                        КороткаНазва = cur.Fields?[Валюти_Const.КороткаНазва]?.ToString() ?? "" // [ pos = 3, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Контрагенти"
    
      
    public class Контрагенти_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Контрагенти_Select Контрагенти_Select = new Довідники.Контрагенти_Select();
            Контрагенти_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Контрагенти_Const.Код // [ pos = 1, type = string ]
                    , Довідники.Контрагенти_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            Контрагенти_Select.Select();
            while (Контрагенти_Select.MoveNext())
            {
                Довідники.Контрагенти_Pointer? cur = Контрагенти_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Контрагенти_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Контрагенти_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[Контрагенти_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Склади"
    
      
    public class Склади_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Склади_Select Склади_Select = new Довідники.Склади_Select();
            Склади_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Склади_Const.Код // [ pos = 1, type = string ]
                    , Довідники.Склади_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            Склади_Select.Select();
            while (Склади_Select.MoveNext())
            {
                Довідники.Склади_Pointer? cur = Склади_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Склади_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Склади_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[Склади_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиЦін"
    
      
    public class ВидиЦін_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, Валюта };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* Валюта */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ВидиЦін_Select ВидиЦін_Select = new Довідники.ВидиЦін_Select();
            ВидиЦін_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ВидиЦін_Const.Код // [ pos = 1, type = string ]
                    , Довідники.ВидиЦін_Const.Назва // [ pos = 2, type = string ]
                    /* , Довідники.ВидиЦін_Const.Валюта */ // [ pos = 3, type = pointer ]
                    
                });

            
                  /* JOIN 3 */
                  ВидиЦін_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_3"));
                  ВидиЦін_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.ВидиЦін_Const.Валюта, ВидиЦін_Select.QuerySelect.Table));
                

            /* SELECT */
            ВидиЦін_Select.Select();
            while (ВидиЦін_Select.MoveNext())
            {
                Довідники.ВидиЦін_Pointer? cur = ВидиЦін_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ВидиЦін_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ВидиЦін_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[ВидиЦін_Const.Назва]?.ToString() ?? "", // [ pos = 2, type = string ]
                        Валюта = cur.Fields?["join_3"]?.ToString() ?? "" // [ pos = 3, type = pointer ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиЦінПостачальників"
    
      
    public class ВидиЦінПостачальників_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ВидиЦінПостачальників_Select ВидиЦінПостачальників_Select = new Довідники.ВидиЦінПостачальників_Select();
            ВидиЦінПостачальників_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ВидиЦінПостачальників_Const.Код // [ pos = 1, type = string ]
                    , Довідники.ВидиЦінПостачальників_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            ВидиЦінПостачальників_Select.Select();
            while (ВидиЦінПостачальників_Select.MoveNext())
            {
                Довідники.ВидиЦінПостачальників_Pointer? cur = ВидиЦінПостачальників_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ВидиЦінПостачальників_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ВидиЦінПостачальників_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[ВидиЦінПостачальників_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Користувачі"
    
      
    public class Користувачі_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Користувачі_Select Користувачі_Select = new Довідники.Користувачі_Select();
            Користувачі_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Користувачі_Const.Код // [ pos = 1, type = string ]
                    , Довідники.Користувачі_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            Користувачі_Select.Select();
            while (Користувачі_Select.MoveNext())
            {
                Довідники.Користувачі_Pointer? cur = Користувачі_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Користувачі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Користувачі_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[Користувачі_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ФізичніОсоби"
    
      
    public class ФізичніОсоби_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ФізичніОсоби_Select ФізичніОсоби_Select = new Довідники.ФізичніОсоби_Select();
            ФізичніОсоби_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ФізичніОсоби_Const.Код // [ pos = 1, type = string ]
                    , Довідники.ФізичніОсоби_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            ФізичніОсоби_Select.Select();
            while (ФізичніОсоби_Select.MoveNext())
            {
                Довідники.ФізичніОсоби_Pointer? cur = ФізичніОсоби_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ФізичніОсоби_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ФізичніОсоби_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[ФізичніОсоби_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СтруктураПідприємства"
    
      
    public class СтруктураПідприємства_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.СтруктураПідприємства_Select СтруктураПідприємства_Select = new Довідники.СтруктураПідприємства_Select();
            СтруктураПідприємства_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.СтруктураПідприємства_Const.Код // [ pos = 1, type = string ]
                    , Довідники.СтруктураПідприємства_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            СтруктураПідприємства_Select.Select();
            while (СтруктураПідприємства_Select.MoveNext())
            {
                Довідники.СтруктураПідприємства_Pointer? cur = СтруктураПідприємства_Select.Current;

                if (cur != null)
                    Store.AppendValues(new СтруктураПідприємства_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[СтруктураПідприємства_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[СтруктураПідприємства_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "КраїниСвіту"
    
      
    public class КраїниСвіту_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.КраїниСвіту_Select КраїниСвіту_Select = new Довідники.КраїниСвіту_Select();
            КраїниСвіту_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.КраїниСвіту_Const.Код // [ pos = 1, type = string ]
                    , Довідники.КраїниСвіту_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            КраїниСвіту_Select.Select();
            while (КраїниСвіту_Select.MoveNext())
            {
                Довідники.КраїниСвіту_Pointer? cur = КраїниСвіту_Select.Current;

                if (cur != null)
                    Store.AppendValues(new КраїниСвіту_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[КраїниСвіту_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[КраїниСвіту_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Файли"
    
      
    public class Файли_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Файли_Select Файли_Select = new Довідники.Файли_Select();
            Файли_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Файли_Const.Код // [ pos = 1, type = string ]
                    , Довідники.Файли_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            Файли_Select.Select();
            while (Файли_Select.MoveNext())
            {
                Довідники.Файли_Pointer? cur = Файли_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Файли_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Файли_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[Файли_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ХарактеристикиНоменклатури"
    
      
    public class ХарактеристикиНоменклатури_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Номенклатура = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, Номенклатура };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* Номенклатура */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ХарактеристикиНоменклатури_Select ХарактеристикиНоменклатури_Select = new Довідники.ХарактеристикиНоменклатури_Select();
            ХарактеристикиНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ХарактеристикиНоменклатури_Const.Код // [ pos = 1, type = string ]
                    , Довідники.ХарактеристикиНоменклатури_Const.Назва // [ pos = 2, type = string ]
                    /* , Довідники.ХарактеристикиНоменклатури_Const.Номенклатура */ // [ pos = 3, type = pointer ]
                    
                });

            
                  /* JOIN 3 */
                  ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Номенклатура_Const.TABLE + "." + Довідники.Номенклатура_Const.Назва, "join_3"));
                  ХарактеристикиНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Номенклатура_Const.TABLE, Довідники.ХарактеристикиНоменклатури_Const.Номенклатура, ХарактеристикиНоменклатури_Select.QuerySelect.Table));
                

            /* SELECT */
            ХарактеристикиНоменклатури_Select.Select();
            while (ХарактеристикиНоменклатури_Select.MoveNext())
            {
                Довідники.ХарактеристикиНоменклатури_Pointer? cur = ХарактеристикиНоменклатури_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ХарактеристикиНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ХарактеристикиНоменклатури_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[ХарактеристикиНоменклатури_Const.Назва]?.ToString() ?? "", // [ pos = 2, type = string ]
                        Номенклатура = cur.Fields?["join_3"]?.ToString() ?? "" // [ pos = 3, type = pointer ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Номенклатура_Папки"
    
      
    #endregion
    
    #region DIRECTORY "Контрагенти_Папки"
    
      
    #endregion
    
    #region DIRECTORY "Склади_Папки"
    
      
    #endregion
    
    #region DIRECTORY "Каси"
    
      
    public class Каси_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, Валюта };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* Валюта */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.Каси_Select Каси_Select = new Довідники.Каси_Select();
            Каси_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Каси_Const.Код // [ pos = 1, type = string ]
                    , Довідники.Каси_Const.Назва // [ pos = 2, type = string ]
                    /* , Довідники.Каси_Const.Валюта */ // [ pos = 3, type = pointer ]
                    
                });

            
                  /* JOIN 3 */
                  Каси_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_3"));
                  Каси_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.Каси_Const.Валюта, Каси_Select.QuerySelect.Table));
                

            /* SELECT */
            Каси_Select.Select();
            while (Каси_Select.MoveNext())
            {
                Довідники.Каси_Pointer? cur = Каси_Select.Current;

                if (cur != null)
                    Store.AppendValues(new Каси_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Каси_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[Каси_Const.Назва]?.ToString() ?? "", // [ pos = 2, type = string ]
                        Валюта = cur.Fields?["join_3"]?.ToString() ?? "" // [ pos = 3, type = pointer ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "БанківськіРахункиОрганізацій"
    
      
    public class БанківськіРахункиОрганізацій_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, Валюта };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* Валюта */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.БанківськіРахункиОрганізацій_Select БанківськіРахункиОрганізацій_Select = new Довідники.БанківськіРахункиОрганізацій_Select();
            БанківськіРахункиОрганізацій_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.БанківськіРахункиОрганізацій_Const.Код // [ pos = 1, type = string ]
                    , Довідники.БанківськіРахункиОрганізацій_Const.Назва // [ pos = 2, type = string ]
                    /* , Довідники.БанківськіРахункиОрганізацій_Const.Валюта */ // [ pos = 3, type = pointer ]
                    
                });

            
                  /* JOIN 3 */
                  БанківськіРахункиОрганізацій_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_3"));
                  БанківськіРахункиОрганізацій_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.БанківськіРахункиОрганізацій_Const.Валюта, БанківськіРахункиОрганізацій_Select.QuerySelect.Table));
                

            /* SELECT */
            БанківськіРахункиОрганізацій_Select.Select();
            while (БанківськіРахункиОрганізацій_Select.MoveNext())
            {
                Довідники.БанківськіРахункиОрганізацій_Pointer? cur = БанківськіРахункиОрганізацій_Select.Current;

                if (cur != null)
                    Store.AppendValues(new БанківськіРахункиОрганізацій_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[БанківськіРахункиОрганізацій_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[БанківськіРахункиОрганізацій_Const.Назва]?.ToString() ?? "", // [ pos = 2, type = string ]
                        Валюта = cur.Fields?["join_3"]?.ToString() ?? "" // [ pos = 3, type = pointer ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ДоговориКонтрагентів"
    
      
    public class ДоговориКонтрагентів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Контрагент = "";
        string ТипДоговору = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, Контрагент, ТипДоговору };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* Контрагент */
            , typeof(string) /* ТипДоговору */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("ТипДоговору", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ДоговориКонтрагентів_Select ДоговориКонтрагентів_Select = new Довідники.ДоговориКонтрагентів_Select();
            ДоговориКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ДоговориКонтрагентів_Const.Код // [ pos = 1, type = string ]
                    , Довідники.ДоговориКонтрагентів_Const.Назва // [ pos = 2, type = string ]
                    /* , Довідники.ДоговориКонтрагентів_Const.Контрагент */ // [ pos = 3, type = pointer ]
                    , Довідники.ДоговориКонтрагентів_Const.ТипДоговору // [ pos = 4, type = enum ]
                    
                });

            
                  /* JOIN 3 */
                  ДоговориКонтрагентів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_3"));
                  ДоговориКонтрагентів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Довідники.ДоговориКонтрагентів_Const.Контрагент, ДоговориКонтрагентів_Select.QuerySelect.Table));
                

            /* SELECT */
            ДоговориКонтрагентів_Select.Select();
            while (ДоговориКонтрагентів_Select.MoveNext())
            {
                Довідники.ДоговориКонтрагентів_Pointer? cur = ДоговориКонтрагентів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ДоговориКонтрагентів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ДоговориКонтрагентів_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[ДоговориКонтрагентів_Const.Назва]?.ToString() ?? "", // [ pos = 2, type = string ]
                        Контрагент = cur.Fields?["join_3"]?.ToString() ?? "", // [ pos = 3, type = pointer ]
                        ТипДоговору = ((Перелічення.ТипДоговорів)int.Parse(cur.Fields?[ДоговориКонтрагентів_Const.ТипДоговору]?.ToString() ?? "0")).ToString() // [ pos = 4, type = enum ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "БанківськіРахункиКонтрагентів"
    
      
    public class БанківськіРахункиКонтрагентів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2, FixedWidth = 100 } );
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.БанківськіРахункиКонтрагентів_Select БанківськіРахункиКонтрагентів_Select = new Довідники.БанківськіРахункиКонтрагентів_Select();
            БанківськіРахункиКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.БанківськіРахункиКонтрагентів_Const.Код // [ pos = 1, type = string ]
                    , Довідники.БанківськіРахункиКонтрагентів_Const.Назва // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            БанківськіРахункиКонтрагентів_Select.Select();
            while (БанківськіРахункиКонтрагентів_Select.MoveNext())
            {
                Довідники.БанківськіРахункиКонтрагентів_Pointer? cur = БанківськіРахункиКонтрагентів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new БанківськіРахункиКонтрагентів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Код]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Назва = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Назва]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СтаттяРухуКоштів"
    
      
    public class СтаттяРухуКоштів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string Код = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, Код };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Код */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.СтаттяРухуКоштів_Select СтаттяРухуКоштів_Select = new Довідники.СтаттяРухуКоштів_Select();
            СтаттяРухуКоштів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.СтаттяРухуКоштів_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.СтаттяРухуКоштів_Const.Код // [ pos = 2, type = string ]
                    
                });

            

            /* SELECT */
            СтаттяРухуКоштів_Select.Select();
            while (СтаттяРухуКоштів_Select.MoveNext())
            {
                Довідники.СтаттяРухуКоштів_Pointer? cur = СтаттяРухуКоштів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new СтаттяРухуКоштів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[СтаттяРухуКоштів_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[СтаттяРухуКоштів_Const.Код]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СеріїНоменклатури"
    
      
    public class СеріїНоменклатури_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Номер = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Номер };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Номер */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.СеріїНоменклатури_Select СеріїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            СеріїНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.СеріїНоменклатури_Const.Номер // [ pos = 1, type = string ]
                    
                });

            

            /* SELECT */
            СеріїНоменклатури_Select.Select();
            while (СеріїНоменклатури_Select.MoveNext())
            {
                Довідники.СеріїНоменклатури_Pointer? cur = СеріїНоменклатури_Select.Current;

                if (cur != null)
                    Store.AppendValues(new СеріїНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Номер = cur.Fields?[СеріїНоменклатури_Const.Номер]?.ToString() ?? "" // [ pos = 1, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ПартіяТоварівКомпозит"
    
      
    public class ПартіяТоварівКомпозит_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string Дата = "";
        string ТипДокументу = "";
        string ПоступленняТоварівТаПослуг = "";
        string ВведенняЗалишків = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, Дата, ТипДокументу, ПоступленняТоварівТаПослуг, ВведенняЗалишків };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Дата */
            , typeof(string) /* ТипДокументу */
            , typeof(string) /* ПоступленняТоварівТаПослуг */
            , typeof(string) /* ВведенняЗалишків */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("ТипДокументу", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("ПоступленняТоварівТаПослуг", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("ВведенняЗалишків", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ПартіяТоварівКомпозит_Select ПартіяТоварівКомпозит_Select = new Довідники.ПартіяТоварівКомпозит_Select();
            ПартіяТоварівКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ПартіяТоварівКомпозит_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.ПартіяТоварівКомпозит_Const.Дата // [ pos = 2, type = datetime ]
                    , Довідники.ПартіяТоварівКомпозит_Const.ТипДокументу // [ pos = 3, type = enum ]
                    /* , Довідники.ПартіяТоварівКомпозит_Const.ПоступленняТоварівТаПослуг */ // [ pos = 4, type = pointer ]
                    /* , Довідники.ПартіяТоварівКомпозит_Const.ВведенняЗалишків */ // [ pos = 5, type = pointer ]
                    
                });

            
                  /* JOIN 4 */
                  ПартіяТоварівКомпозит_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.Назва, "join_4"));
                  ПартіяТоварівКомпозит_Select.QuerySelect.Joins.Add(
                    new Join(Документи.ПоступленняТоварівТаПослуг_Const.TABLE, Довідники.ПартіяТоварівКомпозит_Const.ПоступленняТоварівТаПослуг, ПартіяТоварівКомпозит_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ПартіяТоварівКомпозит_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.Назва, "join_5"));
                  ПартіяТоварівКомпозит_Select.QuerySelect.Joins.Add(
                    new Join(Документи.ВведенняЗалишків_Const.TABLE, Довідники.ПартіяТоварівКомпозит_Const.ВведенняЗалишків, ПартіяТоварівКомпозит_Select.QuerySelect.Table));
                

            /* SELECT */
            ПартіяТоварівКомпозит_Select.Select();
            while (ПартіяТоварівКомпозит_Select.MoveNext())
            {
                Довідники.ПартіяТоварівКомпозит_Pointer? cur = ПартіяТоварівКомпозит_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПартіяТоварівКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПартіяТоварівКомпозит_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Дата = cur.Fields?[ПартіяТоварівКомпозит_Const.Дата]?.ToString() ?? "", // [ pos = 2, type = datetime ]
                        ТипДокументу = ((Перелічення.ТипДокументуПартіяТоварівКомпозит)int.Parse(cur.Fields?[ПартіяТоварівКомпозит_Const.ТипДокументу]?.ToString() ?? "0")).ToString(), // [ pos = 3, type = enum ]
                        ПоступленняТоварівТаПослуг = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        ВведенняЗалишків = cur.Fields?["join_5"]?.ToString() ?? "" // [ pos = 5, type = pointer ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиЗапасів"
    
      
    public class ВидиЗапасів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ВидиЗапасів_Select ВидиЗапасів_Select = new Довідники.ВидиЗапасів_Select();
            ВидиЗапасів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ВидиЗапасів_Const.Назва // [ pos = 1, type = string ]
                    
                });

            

            /* SELECT */
            ВидиЗапасів_Select.Select();
            while (ВидиЗапасів_Select.MoveNext())
            {
                Довідники.ВидиЗапасів_Pointer? cur = ВидиЗапасів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ВидиЗапасів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ВидиЗапасів_Const.Назва]?.ToString() ?? "" // [ pos = 1, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ПродажіДокументКомпозит"
    
      
    public class ПродажіДокументКомпозит_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ПродажіДокументКомпозит_Select ПродажіДокументКомпозит_Select = new Довідники.ПродажіДокументКомпозит_Select();
            ПродажіДокументКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ПродажіДокументКомпозит_Const.Назва // [ pos = 1, type = string ]
                    
                });

            

            /* SELECT */
            ПродажіДокументКомпозит_Select.Select();
            while (ПродажіДокументКомпозит_Select.MoveNext())
            {
                Довідники.ПродажіДокументКомпозит_Pointer? cur = ПродажіДокументКомпозит_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПродажіДокументКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПродажіДокументКомпозит_Const.Назва]?.ToString() ?? "" // [ pos = 1, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "АналітикаНоменклатуриКомпозит"
    
      
    public class АналітикаНоменклатуриКомпозит_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.АналітикаНоменклатуриКомпозит_Select АналітикаНоменклатуриКомпозит_Select = new Довідники.АналітикаНоменклатуриКомпозит_Select();
            АналітикаНоменклатуриКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.АналітикаНоменклатуриКомпозит_Const.Назва // [ pos = 1, type = string ]
                    
                });

            

            /* SELECT */
            АналітикаНоменклатуриКомпозит_Select.Select();
            while (АналітикаНоменклатуриКомпозит_Select.MoveNext())
            {
                Довідники.АналітикаНоменклатуриКомпозит_Pointer? cur = АналітикаНоменклатуриКомпозит_Select.Current;

                if (cur != null)
                    Store.AppendValues(new АналітикаНоменклатуриКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[АналітикаНоменклатуриКомпозит_Const.Назва]?.ToString() ?? "" // [ pos = 1, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "АналітикаКонтрагентівКомпозит"
    
      
    public class АналітикаКонтрагентівКомпозит_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.АналітикаКонтрагентівКомпозит_Select АналітикаКонтрагентівКомпозит_Select = new Довідники.АналітикаКонтрагентівКомпозит_Select();
            АналітикаКонтрагентівКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.АналітикаКонтрагентівКомпозит_Const.Назва // [ pos = 1, type = string ]
                    
                });

            

            /* SELECT */
            АналітикаКонтрагентівКомпозит_Select.Select();
            while (АналітикаКонтрагентівКомпозит_Select.MoveNext())
            {
                Довідники.АналітикаКонтрагентівКомпозит_Pointer? cur = АналітикаКонтрагентівКомпозит_Select.Current;

                if (cur != null)
                    Store.AppendValues(new АналітикаКонтрагентівКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[АналітикаКонтрагентівКомпозит_Const.Назва]?.ToString() ?? "" // [ pos = 1, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "АналітикаПартійКомпозит"
    
      
    public class АналітикаПартійКомпозит_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.АналітикаПартійКомпозит_Select АналітикаПартійКомпозит_Select = new Довідники.АналітикаПартійКомпозит_Select();
            АналітикаПартійКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.АналітикаПартійКомпозит_Const.Назва // [ pos = 1, type = string ]
                    
                });

            

            /* SELECT */
            АналітикаПартійКомпозит_Select.Select();
            while (АналітикаПартійКомпозит_Select.MoveNext())
            {
                Довідники.АналітикаПартійКомпозит_Pointer? cur = АналітикаПартійКомпозит_Select.Current;

                if (cur != null)
                    Store.AppendValues(new АналітикаПартійКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[АналітикаПартійКомпозит_Const.Назва]?.ToString() ?? "" // [ pos = 1, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
}

namespace StorageAndTrade_1_0.Документи.ТабличніСписки
{
    
    #region DOCUMENT "ЗамовленняПостачальнику"
    
      
    public class ЗамовленняПостачальнику_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Контрагент = "";
        string Склад = "";
        string Валюта = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Склад, Валюта, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Склад */
            , typeof(string) /* Валюта */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ЗамовленняПостачальнику_Select ЗамовленняПостачальнику_Select = new Документи.ЗамовленняПостачальнику_Select();
            ЗамовленняПостачальнику_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ЗамовленняПостачальнику_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ЗамовленняПостачальнику_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ЗамовленняПостачальнику_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ЗамовленняПостачальнику_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ЗамовленняПостачальнику_Const.Контрагент */ // [ pos = 5, type = pointer ]
                    /* , Документи.ЗамовленняПостачальнику_Const.Склад */ // [ pos = 6, type = pointer ]
                    /* , Документи.ЗамовленняПостачальнику_Const.Валюта */ // [ pos = 7, type = pointer ]
                    , Документи.ЗамовленняПостачальнику_Const.СумаДокументу // [ pos = 8, type = numeric ]
                    , Документи.ЗамовленняПостачальнику_Const.Коментар // [ pos = 9, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ЗамовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ЗамовленняПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Організація, ЗамовленняПостачальнику_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ЗамовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_5"));
                  ЗамовленняПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Контрагент, ЗамовленняПостачальнику_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  ЗамовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_6"));
                  ЗамовленняПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Склад, ЗамовленняПостачальнику_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  ЗамовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_7"));
                  ЗамовленняПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Валюта, ЗамовленняПостачальнику_Select.QuerySelect.Table));
                

            /* SELECT */
            ЗамовленняПостачальнику_Select.Select();
            while (ЗамовленняПостачальнику_Select.MoveNext())
            {
                Документи.ЗамовленняПостачальнику_Pointer? cur = ЗамовленняПостачальнику_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ЗамовленняПостачальнику_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ЗамовленняПостачальнику_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ЗамовленняПостачальнику_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ЗамовленняПостачальнику_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Контрагент = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Склад = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Валюта = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        СумаДокументу = cur.Fields?[ЗамовленняПостачальнику_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 8, type = numeric ]
                        Коментар = cur.Fields?[ЗамовленняПостачальнику_Const.Коментар]?.ToString() ?? "" // [ pos = 9, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПоступленняТоварівТаПослуг"
    
      
    public class ПоступленняТоварівТаПослуг_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Контрагент = "";
        string Валюта = "";
        string Каса = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Контрагент, Валюта, Каса, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 11) { SortColumnId = 11 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПоступленняТоварівТаПослуг_Select ПоступленняТоварівТаПослуг_Select = new Документи.ПоступленняТоварівТаПослуг_Select();
            ПоступленняТоварівТаПослуг_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ПоступленняТоварівТаПослуг_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ПоступленняТоварівТаПослуг_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ПоступленняТоварівТаПослуг_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ПоступленняТоварівТаПослуг_Const.Склад */ // [ pos = 5, type = pointer ]
                    /* , Документи.ПоступленняТоварівТаПослуг_Const.Контрагент */ // [ pos = 6, type = pointer ]
                    /* , Документи.ПоступленняТоварівТаПослуг_Const.Валюта */ // [ pos = 7, type = pointer ]
                    /* , Документи.ПоступленняТоварівТаПослуг_Const.Каса */ // [ pos = 8, type = pointer ]
                    , Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу // [ pos = 9, type = numeric ]
                    , Документи.ПоступленняТоварівТаПослуг_Const.Коментар // [ pos = 10, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Організація, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_5"));
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Склад, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_6"));
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Контрагент, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_7"));
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Валюта, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table));
                
                  /* JOIN 8 */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "join_8"));
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Каса, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table));
                

            /* SELECT */
            ПоступленняТоварівТаПослуг_Select.Select();
            while (ПоступленняТоварівТаПослуг_Select.MoveNext())
            {
                Документи.ПоступленняТоварівТаПослуг_Pointer? cur = ПоступленняТоварівТаПослуг_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПоступленняТоварівТаПослуг_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПоступленняТоварівТаПослуг_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ПоступленняТоварівТаПослуг_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ПоступленняТоварівТаПослуг_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Склад = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Контрагент = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Валюта = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        Каса = cur.Fields?["join_8"]?.ToString() ?? "", // [ pos = 8, type = pointer ]
                        СумаДокументу = cur.Fields?[ПоступленняТоварівТаПослуг_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 9, type = numeric ]
                        Коментар = cur.Fields?[ПоступленняТоварівТаПослуг_Const.Коментар]?.ToString() ?? "" // [ pos = 10, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ЗамовленняКлієнта"
    
      
    public class ЗамовленняКлієнта_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Контрагент = "";
        string Валюта = "";
        string Каса = "";
        string Склад = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* Склад */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 11) { SortColumnId = 11 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ЗамовленняКлієнта_Select ЗамовленняКлієнта_Select = new Документи.ЗамовленняКлієнта_Select();
            ЗамовленняКлієнта_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ЗамовленняКлієнта_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ЗамовленняКлієнта_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ЗамовленняКлієнта_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ЗамовленняКлієнта_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ЗамовленняКлієнта_Const.Контрагент */ // [ pos = 5, type = pointer ]
                    /* , Документи.ЗамовленняКлієнта_Const.Валюта */ // [ pos = 6, type = pointer ]
                    /* , Документи.ЗамовленняКлієнта_Const.Каса */ // [ pos = 7, type = pointer ]
                    /* , Документи.ЗамовленняКлієнта_Const.Склад */ // [ pos = 8, type = pointer ]
                    , Документи.ЗамовленняКлієнта_Const.СумаДокументу // [ pos = 9, type = numeric ]
                    , Документи.ЗамовленняКлієнта_Const.Коментар // [ pos = 10, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Організація, ЗамовленняКлієнта_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_5"));
                  ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Контрагент, ЗамовленняКлієнта_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_6"));
                  ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Валюта, ЗамовленняКлієнта_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "join_7"));
                  ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Каса, ЗамовленняКлієнта_Select.QuerySelect.Table));
                
                  /* JOIN 8 */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_8"));
                  ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Склад, ЗамовленняКлієнта_Select.QuerySelect.Table));
                

            /* SELECT */
            ЗамовленняКлієнта_Select.Select();
            while (ЗамовленняКлієнта_Select.MoveNext())
            {
                Документи.ЗамовленняКлієнта_Pointer? cur = ЗамовленняКлієнта_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ЗамовленняКлієнта_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ЗамовленняКлієнта_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ЗамовленняКлієнта_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ЗамовленняКлієнта_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Контрагент = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Валюта = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Каса = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        Склад = cur.Fields?["join_8"]?.ToString() ?? "", // [ pos = 8, type = pointer ]
                        СумаДокументу = cur.Fields?[ЗамовленняКлієнта_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 9, type = numeric ]
                        Коментар = cur.Fields?[ЗамовленняКлієнта_Const.Коментар]?.ToString() ?? "" // [ pos = 10, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "РеалізаціяТоварівТаПослуг"
    
      
    public class РеалізаціяТоварівТаПослуг_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Контрагент = "";
        string Валюта = "";
        string Каса = "";
        string Склад = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* Склад */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 11) { SortColumnId = 11 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.РеалізаціяТоварівТаПослуг_Select РеалізаціяТоварівТаПослуг_Select = new Документи.РеалізаціяТоварівТаПослуг_Select();
            РеалізаціяТоварівТаПослуг_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.РеалізаціяТоварівТаПослуг_Const.Назва // [ pos = 1, type = string ]
                    , Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.РеалізаціяТоварівТаПослуг_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент */ // [ pos = 5, type = pointer ]
                    /* , Документи.РеалізаціяТоварівТаПослуг_Const.Валюта */ // [ pos = 6, type = pointer ]
                    /* , Документи.РеалізаціяТоварівТаПослуг_Const.Каса */ // [ pos = 7, type = pointer ]
                    /* , Документи.РеалізаціяТоварівТаПослуг_Const.Склад */ // [ pos = 8, type = pointer ]
                    , Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу // [ pos = 9, type = numeric ]
                    , Документи.РеалізаціяТоварівТаПослуг_Const.Коментар // [ pos = 10, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Організація, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_5"));
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_6"));
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Валюта, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "join_7"));
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Каса, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table));
                
                  /* JOIN 8 */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_8"));
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Склад, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table));
                

            /* SELECT */
            РеалізаціяТоварівТаПослуг_Select.Select();
            while (РеалізаціяТоварівТаПослуг_Select.MoveNext())
            {
                Документи.РеалізаціяТоварівТаПослуг_Pointer? cur = РеалізаціяТоварівТаПослуг_Select.Current;

                if (cur != null)
                    Store.AppendValues(new РеалізаціяТоварівТаПослуг_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Контрагент = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Валюта = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Каса = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        Склад = cur.Fields?["join_8"]?.ToString() ?? "", // [ pos = 8, type = pointer ]
                        СумаДокументу = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 9, type = numeric ]
                        Коментар = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.Коментар]?.ToString() ?? "" // [ pos = 10, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ВстановленняЦінНоменклатури"
    
      
    public class ВстановленняЦінНоменклатури_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Валюта = "";
        string ВидЦіни = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, ВидЦіни, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Валюта */
            , typeof(string) /* ВидЦіни */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Вид ціни", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ВстановленняЦінНоменклатури_Select ВстановленняЦінНоменклатури_Select = new Документи.ВстановленняЦінНоменклатури_Select();
            ВстановленняЦінНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ВстановленняЦінНоменклатури_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ВстановленняЦінНоменклатури_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ВстановленняЦінНоменклатури_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ВстановленняЦінНоменклатури_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ВстановленняЦінНоменклатури_Const.Валюта */ // [ pos = 5, type = pointer ]
                    /* , Документи.ВстановленняЦінНоменклатури_Const.ВидЦіни */ // [ pos = 6, type = pointer ]
                    , Документи.ВстановленняЦінНоменклатури_Const.Коментар // [ pos = 7, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ВстановленняЦінНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ВстановленняЦінНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.Організація, ВстановленняЦінНоменклатури_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ВстановленняЦінНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_5"));
                  ВстановленняЦінНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.Валюта, ВстановленняЦінНоменклатури_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  ВстановленняЦінНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.ВидиЦін_Const.TABLE + "." + Довідники.ВидиЦін_Const.Назва, "join_6"));
                  ВстановленняЦінНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ВидиЦін_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.ВидЦіни, ВстановленняЦінНоменклатури_Select.QuerySelect.Table));
                

            /* SELECT */
            ВстановленняЦінНоменклатури_Select.Select();
            while (ВстановленняЦінНоменклатури_Select.MoveNext())
            {
                Документи.ВстановленняЦінНоменклатури_Pointer? cur = ВстановленняЦінНоменклатури_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ВстановленняЦінНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ВстановленняЦінНоменклатури_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ВстановленняЦінНоменклатури_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ВстановленняЦінНоменклатури_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Валюта = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        ВидЦіни = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Коментар = cur.Fields?[ВстановленняЦінНоменклатури_Const.Коментар]?.ToString() ?? "" // [ pos = 7, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПрихіднийКасовийОрдер"
    
      
    public class ПрихіднийКасовийОрдер_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Валюта = "";
        string Каса = "";
        string Контрагент = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, Каса, Контрагент, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* Контрагент */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПрихіднийКасовийОрдер_Select ПрихіднийКасовийОрдер_Select = new Документи.ПрихіднийКасовийОрдер_Select();
            ПрихіднийКасовийОрдер_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ПрихіднийКасовийОрдер_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ПрихіднийКасовийОрдер_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ПрихіднийКасовийОрдер_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ПрихіднийКасовийОрдер_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ПрихіднийКасовийОрдер_Const.Валюта */ // [ pos = 5, type = pointer ]
                    /* , Документи.ПрихіднийКасовийОрдер_Const.Каса */ // [ pos = 6, type = pointer ]
                    /* , Документи.ПрихіднийКасовийОрдер_Const.Контрагент */ // [ pos = 7, type = pointer ]
                    , Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу // [ pos = 8, type = numeric ]
                    , Документи.ПрихіднийКасовийОрдер_Const.Коментар // [ pos = 9, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Організація, ПрихіднийКасовийОрдер_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_5"));
                  ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Валюта, ПрихіднийКасовийОрдер_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "join_6"));
                  ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Каса, ПрихіднийКасовийОрдер_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_7"));
                  ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Контрагент, ПрихіднийКасовийОрдер_Select.QuerySelect.Table));
                

            /* SELECT */
            ПрихіднийКасовийОрдер_Select.Select();
            while (ПрихіднийКасовийОрдер_Select.MoveNext())
            {
                Документи.ПрихіднийКасовийОрдер_Pointer? cur = ПрихіднийКасовийОрдер_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПрихіднийКасовийОрдер_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПрихіднийКасовийОрдер_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ПрихіднийКасовийОрдер_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ПрихіднийКасовийОрдер_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Валюта = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Каса = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Контрагент = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        СумаДокументу = cur.Fields?[ПрихіднийКасовийОрдер_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 8, type = numeric ]
                        Коментар = cur.Fields?[ПрихіднийКасовийОрдер_Const.Коментар]?.ToString() ?? "" // [ pos = 9, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "РозхіднийКасовийОрдер"
    
      
    public class РозхіднийКасовийОрдер_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Контрагент = "";
        string Валюта = "";
        string Каса = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.РозхіднийКасовийОрдер_Select РозхіднийКасовийОрдер_Select = new Документи.РозхіднийКасовийОрдер_Select();
            РозхіднийКасовийОрдер_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.РозхіднийКасовийОрдер_Const.Назва // [ pos = 1, type = string ]
                    , Документи.РозхіднийКасовийОрдер_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.РозхіднийКасовийОрдер_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.РозхіднийКасовийОрдер_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.РозхіднийКасовийОрдер_Const.Контрагент */ // [ pos = 5, type = pointer ]
                    /* , Документи.РозхіднийКасовийОрдер_Const.Валюта */ // [ pos = 6, type = pointer ]
                    /* , Документи.РозхіднийКасовийОрдер_Const.Каса */ // [ pos = 7, type = pointer ]
                    , Документи.РозхіднийКасовийОрдер_Const.СумаДокументу // [ pos = 8, type = numeric ]
                    , Документи.РозхіднийКасовийОрдер_Const.Коментар // [ pos = 9, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  РозхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Організація, РозхіднийКасовийОрдер_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_5"));
                  РозхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Контрагент, РозхіднийКасовийОрдер_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_6"));
                  РозхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Валюта, РозхіднийКасовийОрдер_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "join_7"));
                  РозхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Каса, РозхіднийКасовийОрдер_Select.QuerySelect.Table));
                

            /* SELECT */
            РозхіднийКасовийОрдер_Select.Select();
            while (РозхіднийКасовийОрдер_Select.MoveNext())
            {
                Документи.РозхіднийКасовийОрдер_Pointer? cur = РозхіднийКасовийОрдер_Select.Current;

                if (cur != null)
                    Store.AppendValues(new РозхіднийКасовийОрдер_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[РозхіднийКасовийОрдер_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[РозхіднийКасовийОрдер_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[РозхіднийКасовийОрдер_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Контрагент = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Валюта = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Каса = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        СумаДокументу = cur.Fields?[РозхіднийКасовийОрдер_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 8, type = numeric ]
                        Коментар = cur.Fields?[РозхіднийКасовийОрдер_Const.Коментар]?.ToString() ?? "" // [ pos = 9, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПереміщенняТоварів"
    
      
    public class ПереміщенняТоварів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string СкладВідправник = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, СкладВідправник, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* СкладВідправник */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("СкладВідправник", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПереміщенняТоварів_Select ПереміщенняТоварів_Select = new Документи.ПереміщенняТоварів_Select();
            ПереміщенняТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ПереміщенняТоварів_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ПереміщенняТоварів_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ПереміщенняТоварів_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ПереміщенняТоварів_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ПереміщенняТоварів_Const.СкладВідправник */ // [ pos = 5, type = pointer ]
                    , Документи.ПереміщенняТоварів_Const.Коментар // [ pos = 6, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ПереміщенняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ПереміщенняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПереміщенняТоварів_Const.Організація, ПереміщенняТоварів_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ПереміщенняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_5"));
                  ПереміщенняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварів_Const.СкладВідправник, ПереміщенняТоварів_Select.QuerySelect.Table));
                

            /* SELECT */
            ПереміщенняТоварів_Select.Select();
            while (ПереміщенняТоварів_Select.MoveNext())
            {
                Документи.ПереміщенняТоварів_Pointer? cur = ПереміщенняТоварів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПереміщенняТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПереміщенняТоварів_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ПереміщенняТоварів_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ПереміщенняТоварів_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        СкладВідправник = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Коментар = cur.Fields?[ПереміщенняТоварів_Const.Коментар]?.ToString() ?? "" // [ pos = 6, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПоверненняТоварівПостачальнику"
    
      
    public class ПоверненняТоварівПостачальнику_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Контрагент = "";
        string Валюта = "";
        string Каса = "";
        string Склад = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* Склад */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 11) { SortColumnId = 11 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПоверненняТоварівПостачальнику_Select ПоверненняТоварівПостачальнику_Select = new Документи.ПоверненняТоварівПостачальнику_Select();
            ПоверненняТоварівПостачальнику_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ПоверненняТоварівПостачальнику_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ПоверненняТоварівПостачальнику_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ПоверненняТоварівПостачальнику_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ПоверненняТоварівПостачальнику_Const.Контрагент */ // [ pos = 5, type = pointer ]
                    /* , Документи.ПоверненняТоварівПостачальнику_Const.Валюта */ // [ pos = 6, type = pointer ]
                    /* , Документи.ПоверненняТоварівПостачальнику_Const.Каса */ // [ pos = 7, type = pointer ]
                    /* , Документи.ПоверненняТоварівПостачальнику_Const.Склад */ // [ pos = 8, type = pointer ]
                    , Документи.ПоверненняТоварівПостачальнику_Const.СумаДокументу // [ pos = 9, type = numeric ]
                    , Документи.ПоверненняТоварівПостачальнику_Const.Коментар // [ pos = 10, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Організація, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_5"));
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Контрагент, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_6"));
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Валюта, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "join_7"));
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Каса, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table));
                
                  /* JOIN 8 */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_8"));
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Склад, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table));
                

            /* SELECT */
            ПоверненняТоварівПостачальнику_Select.Select();
            while (ПоверненняТоварівПостачальнику_Select.MoveNext())
            {
                Документи.ПоверненняТоварівПостачальнику_Pointer? cur = ПоверненняТоварівПостачальнику_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПоверненняТоварівПостачальнику_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПоверненняТоварівПостачальнику_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ПоверненняТоварівПостачальнику_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ПоверненняТоварівПостачальнику_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Контрагент = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Валюта = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Каса = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        Склад = cur.Fields?["join_8"]?.ToString() ?? "", // [ pos = 8, type = pointer ]
                        СумаДокументу = cur.Fields?[ПоверненняТоварівПостачальнику_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 9, type = numeric ]
                        Коментар = cur.Fields?[ПоверненняТоварівПостачальнику_Const.Коментар]?.ToString() ?? "" // [ pos = 10, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПоверненняТоварівВідКлієнта"
    
      
    public class ПоверненняТоварівВідКлієнта_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Валюта = "";
        string Каса = "";
        string Контрагент = "";
        string Склад = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, Каса, Контрагент, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Склад */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 11) { SortColumnId = 11 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПоверненняТоварівВідКлієнта_Select ПоверненняТоварівВідКлієнта_Select = new Документи.ПоверненняТоварівВідКлієнта_Select();
            ПоверненняТоварівВідКлієнта_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ПоверненняТоварівВідКлієнта_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ПоверненняТоварівВідКлієнта_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ПоверненняТоварівВідКлієнта_Const.Валюта */ // [ pos = 5, type = pointer ]
                    /* , Документи.ПоверненняТоварівВідКлієнта_Const.Каса */ // [ pos = 6, type = pointer ]
                    /* , Документи.ПоверненняТоварівВідКлієнта_Const.Контрагент */ // [ pos = 7, type = pointer ]
                    /* , Документи.ПоверненняТоварівВідКлієнта_Const.Склад */ // [ pos = 8, type = pointer ]
                    , Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу // [ pos = 9, type = numeric ]
                    , Документи.ПоверненняТоварівВідКлієнта_Const.Коментар // [ pos = 10, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Організація, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_5"));
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Валюта, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "join_6"));
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Каса, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_7"));
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Контрагент, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table));
                
                  /* JOIN 8 */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_8"));
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Склад, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table));
                

            /* SELECT */
            ПоверненняТоварівВідКлієнта_Select.Select();
            while (ПоверненняТоварівВідКлієнта_Select.MoveNext())
            {
                Документи.ПоверненняТоварівВідКлієнта_Pointer? cur = ПоверненняТоварівВідКлієнта_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПоверненняТоварівВідКлієнта_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Валюта = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Каса = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Контрагент = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        Склад = cur.Fields?["join_8"]?.ToString() ?? "", // [ pos = 8, type = pointer ]
                        СумаДокументу = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 9, type = numeric ]
                        Коментар = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.Коментар]?.ToString() ?? "" // [ pos = 10, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "АктВиконанихРобіт"
    
      
    public class АктВиконанихРобіт_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Валюта = "";
        string Каса = "";
        string Контрагент = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, Каса, Контрагент, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* Контрагент */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.АктВиконанихРобіт_Select АктВиконанихРобіт_Select = new Документи.АктВиконанихРобіт_Select();
            АктВиконанихРобіт_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.АктВиконанихРобіт_Const.Назва // [ pos = 1, type = string ]
                    , Документи.АктВиконанихРобіт_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.АктВиконанихРобіт_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.АктВиконанихРобіт_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.АктВиконанихРобіт_Const.Валюта */ // [ pos = 5, type = pointer ]
                    /* , Документи.АктВиконанихРобіт_Const.Каса */ // [ pos = 6, type = pointer ]
                    /* , Документи.АктВиконанихРобіт_Const.Контрагент */ // [ pos = 7, type = pointer ]
                    , Документи.АктВиконанихРобіт_Const.СумаДокументу // [ pos = 8, type = numeric ]
                    , Документи.АктВиконанихРобіт_Const.Коментар // [ pos = 9, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  АктВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  АктВиконанихРобіт_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.АктВиконанихРобіт_Const.Організація, АктВиконанихРобіт_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  АктВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_5"));
                  АктВиконанихРобіт_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.АктВиконанихРобіт_Const.Валюта, АктВиконанихРобіт_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  АктВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "join_6"));
                  АктВиконанихРобіт_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.АктВиконанихРобіт_Const.Каса, АктВиконанихРобіт_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  АктВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_7"));
                  АктВиконанихРобіт_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.АктВиконанихРобіт_Const.Контрагент, АктВиконанихРобіт_Select.QuerySelect.Table));
                

            /* SELECT */
            АктВиконанихРобіт_Select.Select();
            while (АктВиконанихРобіт_Select.MoveNext())
            {
                Документи.АктВиконанихРобіт_Pointer? cur = АктВиконанихРобіт_Select.Current;

                if (cur != null)
                    Store.AppendValues(new АктВиконанихРобіт_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[АктВиконанихРобіт_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[АктВиконанихРобіт_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[АктВиконанихРобіт_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Валюта = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Каса = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Контрагент = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        СумаДокументу = cur.Fields?[АктВиконанихРобіт_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 8, type = numeric ]
                        Коментар = cur.Fields?[АктВиконанихРобіт_Const.Коментар]?.ToString() ?? "" // [ pos = 9, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ВведенняЗалишків"
    
      
    public class ВведенняЗалишків_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Контрагент = "";
        string Валюта = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Контрагент, Валюта, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("НомерДок", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("ДатаДок", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ВведенняЗалишків_Select ВведенняЗалишків_Select = new Документи.ВведенняЗалишків_Select();
            ВведенняЗалишків_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ВведенняЗалишків_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ВведенняЗалишків_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ВведенняЗалишків_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ВведенняЗалишків_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ВведенняЗалишків_Const.Склад */ // [ pos = 5, type = pointer ]
                    /* , Документи.ВведенняЗалишків_Const.Контрагент */ // [ pos = 6, type = pointer ]
                    /* , Документи.ВведенняЗалишків_Const.Валюта */ // [ pos = 7, type = pointer ]
                    , Документи.ВведенняЗалишків_Const.Коментар // [ pos = 8, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ВведенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ВведенняЗалишків_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ВведенняЗалишків_Const.Організація, ВведенняЗалишків_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ВведенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_5"));
                  ВведенняЗалишків_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ВведенняЗалишків_Const.Склад, ВведенняЗалишків_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  ВведенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_6"));
                  ВведенняЗалишків_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ВведенняЗалишків_Const.Контрагент, ВведенняЗалишків_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  ВведенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_7"));
                  ВведенняЗалишків_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ВведенняЗалишків_Const.Валюта, ВведенняЗалишків_Select.QuerySelect.Table));
                

            /* SELECT */
            ВведенняЗалишків_Select.Select();
            while (ВведенняЗалишків_Select.MoveNext())
            {
                Документи.ВведенняЗалишків_Pointer? cur = ВведенняЗалишків_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ВведенняЗалишків_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ВведенняЗалишків_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ВведенняЗалишків_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ВведенняЗалишків_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Склад = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Контрагент = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Валюта = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        Коментар = cur.Fields?[ВведенняЗалишків_Const.Коментар]?.ToString() ?? "" // [ pos = 8, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "НадлишкиТоварів"
    
      
    public class НадлишкиТоварів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.НадлишкиТоварів_Select НадлишкиТоварів_Select = new Документи.НадлишкиТоварів_Select();
            НадлишкиТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.НадлишкиТоварів_Const.Назва // [ pos = 1, type = string ]
                    , Документи.НадлишкиТоварів_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.НадлишкиТоварів_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.НадлишкиТоварів_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.НадлишкиТоварів_Const.Склад */ // [ pos = 5, type = pointer ]
                    , Документи.НадлишкиТоварів_Const.Коментар // [ pos = 6, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  НадлишкиТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  НадлишкиТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.НадлишкиТоварів_Const.Організація, НадлишкиТоварів_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  НадлишкиТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_5"));
                  НадлишкиТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.НадлишкиТоварів_Const.Склад, НадлишкиТоварів_Select.QuerySelect.Table));
                

            /* SELECT */
            НадлишкиТоварів_Select.Select();
            while (НадлишкиТоварів_Select.MoveNext())
            {
                Документи.НадлишкиТоварів_Pointer? cur = НадлишкиТоварів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new НадлишкиТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[НадлишкиТоварів_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[НадлишкиТоварів_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[НадлишкиТоварів_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Склад = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Коментар = cur.Fields?[НадлишкиТоварів_Const.Коментар]?.ToString() ?? "" // [ pos = 6, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПересортицяТоварів"
    
      
    public class ПересортицяТоварів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("НомерДок", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("ДатаДок", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПересортицяТоварів_Select ПересортицяТоварів_Select = new Документи.ПересортицяТоварів_Select();
            ПересортицяТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ПересортицяТоварів_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ПересортицяТоварів_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ПересортицяТоварів_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ПересортицяТоварів_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ПересортицяТоварів_Const.Склад */ // [ pos = 5, type = pointer ]
                    , Документи.ПересортицяТоварів_Const.Коментар // [ pos = 6, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ПересортицяТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ПересортицяТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПересортицяТоварів_Const.Організація, ПересортицяТоварів_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ПересортицяТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_5"));
                  ПересортицяТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПересортицяТоварів_Const.Склад, ПересортицяТоварів_Select.QuerySelect.Table));
                

            /* SELECT */
            ПересортицяТоварів_Select.Select();
            while (ПересортицяТоварів_Select.MoveNext())
            {
                Документи.ПересортицяТоварів_Pointer? cur = ПересортицяТоварів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПересортицяТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПересортицяТоварів_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ПересортицяТоварів_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ПересортицяТоварів_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Склад = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Коментар = cur.Fields?[ПересортицяТоварів_Const.Коментар]?.ToString() ?? "" // [ pos = 6, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПерерахунокТоварів"
    
      
    public class ПерерахунокТоварів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Склад = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Склад, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Склад */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПерерахунокТоварів_Select ПерерахунокТоварів_Select = new Документи.ПерерахунокТоварів_Select();
            ПерерахунокТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ПерерахунокТоварів_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ПерерахунокТоварів_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ПерерахунокТоварів_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ПерерахунокТоварів_Const.Склад */ // [ pos = 4, type = pointer ]
                    , Документи.ПерерахунокТоварів_Const.Коментар // [ pos = 5, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ПерерахунокТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_4"));
                  ПерерахунокТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПерерахунокТоварів_Const.Склад, ПерерахунокТоварів_Select.QuerySelect.Table));
                

            /* SELECT */
            ПерерахунокТоварів_Select.Select();
            while (ПерерахунокТоварів_Select.MoveNext())
            {
                Документи.ПерерахунокТоварів_Pointer? cur = ПерерахунокТоварів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПерерахунокТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПерерахунокТоварів_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ПерерахунокТоварів_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ПерерахунокТоварів_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Склад = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Коментар = cur.Fields?[ПерерахунокТоварів_Const.Коментар]?.ToString() ?? "" // [ pos = 5, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПсуванняТоварів"
    
      
    public class ПсуванняТоварів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПсуванняТоварів_Select ПсуванняТоварів_Select = new Документи.ПсуванняТоварів_Select();
            ПсуванняТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ПсуванняТоварів_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ПсуванняТоварів_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ПсуванняТоварів_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ПсуванняТоварів_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ПсуванняТоварів_Const.Склад */ // [ pos = 5, type = pointer ]
                    , Документи.ПсуванняТоварів_Const.СумаДокументу // [ pos = 6, type = numeric ]
                    , Документи.ПсуванняТоварів_Const.Коментар // [ pos = 7, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ПсуванняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ПсуванняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПсуванняТоварів_Const.Організація, ПсуванняТоварів_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ПсуванняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_5"));
                  ПсуванняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПсуванняТоварів_Const.Склад, ПсуванняТоварів_Select.QuerySelect.Table));
                

            /* SELECT */
            ПсуванняТоварів_Select.Select();
            while (ПсуванняТоварів_Select.MoveNext())
            {
                Документи.ПсуванняТоварів_Pointer? cur = ПсуванняТоварів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ПсуванняТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПсуванняТоварів_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ПсуванняТоварів_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ПсуванняТоварів_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Склад = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        СумаДокументу = cur.Fields?[ПсуванняТоварів_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 6, type = numeric ]
                        Коментар = cur.Fields?[ПсуванняТоварів_Const.Коментар]?.ToString() ?? "" // [ pos = 7, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ВнутрішнєСпоживанняТоварів"
    
      
    public class ВнутрішнєСпоживанняТоварів_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Валюта = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Валюта, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Валюта */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ВнутрішнєСпоживанняТоварів_Select ВнутрішнєСпоживанняТоварів_Select = new Документи.ВнутрішнєСпоживанняТоварів_Select();
            ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.ВнутрішнєСпоживанняТоварів_Const.Назва // [ pos = 1, type = string ]
                    , Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.ВнутрішнєСпоживанняТоварів_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.ВнутрішнєСпоживанняТоварів_Const.Склад */ // [ pos = 5, type = pointer ]
                    /* , Документи.ВнутрішнєСпоживанняТоварів_Const.Валюта */ // [ pos = 6, type = pointer ]
                    , Документи.ВнутрішнєСпоживанняТоварів_Const.СумаДокументу // [ pos = 7, type = numeric ]
                    , Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар // [ pos = 8, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Організація, ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_5"));
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Склад, ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_6"));
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Валюта, ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table));
                

            /* SELECT */
            ВнутрішнєСпоживанняТоварів_Select.Select();
            while (ВнутрішнєСпоживанняТоварів_Select.MoveNext())
            {
                Документи.ВнутрішнєСпоживанняТоварів_Pointer? cur = ВнутрішнєСпоживанняТоварів_Select.Current;

                if (cur != null)
                    Store.AppendValues(new ВнутрішнєСпоживанняТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Склад = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Валюта = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        СумаДокументу = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 7, type = numeric ]
                        Коментар = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.Коментар]?.ToString() ?? "" // [ pos = 8, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "РахунокФактура"
    
      
    public class РахунокФактура_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Контрагент = "";
        string Валюта = "";
        string Каса = "";
        string Склад = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* Склад */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText(), "text", 7) { SortColumnId = 7 } );
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText(), "text", 8) { SortColumnId = 8 } );
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText(), "text", 9) { SortColumnId = 9 } );
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText(), "text", 10) { SortColumnId = 10 } );
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText(), "text", 11) { SortColumnId = 11 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.РахунокФактура_Select РахунокФактура_Select = new Документи.РахунокФактура_Select();
            РахунокФактура_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Документи.РахунокФактура_Const.Назва // [ pos = 1, type = string ]
                    , Документи.РахунокФактура_Const.НомерДок // [ pos = 2, type = string ]
                    , Документи.РахунокФактура_Const.ДатаДок // [ pos = 3, type = datetime ]
                    /* , Документи.РахунокФактура_Const.Організація */ // [ pos = 4, type = pointer ]
                    /* , Документи.РахунокФактура_Const.Контрагент */ // [ pos = 5, type = pointer ]
                    /* , Документи.РахунокФактура_Const.Валюта */ // [ pos = 6, type = pointer ]
                    /* , Документи.РахунокФактура_Const.Каса */ // [ pos = 7, type = pointer ]
                    /* , Документи.РахунокФактура_Const.Склад */ // [ pos = 8, type = pointer ]
                    , Документи.РахунокФактура_Const.СумаДокументу // [ pos = 9, type = numeric ]
                    , Документи.РахунокФактура_Const.Коментар // [ pos = 10, type = string ]
                    
                });

            
                  /* JOIN 4 */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.РахунокФактура_Const.Організація, РахунокФактура_Select.QuerySelect.Table));
                
                  /* JOIN 5 */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Контрагенти_Const.TABLE + "." + Довідники.Контрагенти_Const.Назва, "join_5"));
                  РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.РахунокФактура_Const.Контрагент, РахунокФактура_Select.QuerySelect.Table));
                
                  /* JOIN 6 */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_6"));
                  РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.РахунокФактура_Const.Валюта, РахунокФактура_Select.QuerySelect.Table));
                
                  /* JOIN 7 */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Каси_Const.TABLE + "." + Довідники.Каси_Const.Назва, "join_7"));
                  РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.РахунокФактура_Const.Каса, РахунокФактура_Select.QuerySelect.Table));
                
                  /* JOIN 8 */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Склади_Const.TABLE + "." + Довідники.Склади_Const.Назва, "join_8"));
                  РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.РахунокФактура_Const.Склад, РахунокФактура_Select.QuerySelect.Table));
                

            /* SELECT */
            РахунокФактура_Select.Select();
            while (РахунокФактура_Select.MoveNext())
            {
                Документи.РахунокФактура_Pointer? cur = РахунокФактура_Select.Current;

                if (cur != null)
                    Store.AppendValues(new РахунокФактура_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[РахунокФактура_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        НомерДок = cur.Fields?[РахунокФактура_Const.НомерДок]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаДок = cur.Fields?[РахунокФактура_Const.ДатаДок]?.ToString() ?? "", // [ pos = 3, type = datetime ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "", // [ pos = 4, type = pointer ]
                        Контрагент = cur.Fields?["join_5"]?.ToString() ?? "", // [ pos = 5, type = pointer ]
                        Валюта = cur.Fields?["join_6"]?.ToString() ?? "", // [ pos = 6, type = pointer ]
                        Каса = cur.Fields?["join_7"]?.ToString() ?? "", // [ pos = 7, type = pointer ]
                        Склад = cur.Fields?["join_8"]?.ToString() ?? "", // [ pos = 8, type = pointer ]
                        СумаДокументу = cur.Fields?[РахунокФактура_Const.СумаДокументу]?.ToString() ?? "", // [ pos = 9, type = numeric ]
                        Коментар = cur.Fields?[РахунокФактура_Const.Коментар]?.ToString() ?? "" // [ pos = 10, type = string ]
                        
                    }.ToArray());
            }
        }
    }
	    
    #endregion
    
}

  