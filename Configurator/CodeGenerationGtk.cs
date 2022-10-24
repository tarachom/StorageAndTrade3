

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
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
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

            Довідники.ФізичніОсоби_Select ФізичніОсоби_Select = new Довідники.ФізичніОсоби_Select();
            ФізичніОсоби_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ФізичніОсоби_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.ФізичніОсоби_Const.Код // [ pos = 2, type = string ]
                    
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
                        Назва = cur.Fields?[ФізичніОсоби_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[ФізичніОсоби_Const.Код]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
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

            Довідники.СтруктураПідприємства_Select СтруктураПідприємства_Select = new Довідники.СтруктураПідприємства_Select();
            СтруктураПідприємства_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.СтруктураПідприємства_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.СтруктураПідприємства_Const.Код // [ pos = 2, type = string ]
                    
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
                        Назва = cur.Fields?[СтруктураПідприємства_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[СтруктураПідприємства_Const.Код]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
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

            Довідники.КраїниСвіту_Select КраїниСвіту_Select = new Довідники.КраїниСвіту_Select();
            КраїниСвіту_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.КраїниСвіту_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.КраїниСвіту_Const.Код // [ pos = 2, type = string ]
                    
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
                        Назва = cur.Fields?[КраїниСвіту_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[КраїниСвіту_Const.Код]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
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
        
        string Назва = "";
        string Код = "";
        string Номенклатура = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, Код, Номенклатура };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Код */
            , typeof(string) /* Номенклатура */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ХарактеристикиНоменклатури_Select ХарактеристикиНоменклатури_Select = new Довідники.ХарактеристикиНоменклатури_Select();
            ХарактеристикиНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ХарактеристикиНоменклатури_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.ХарактеристикиНоменклатури_Const.Код // [ pos = 2, type = string ]
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
                        Назва = cur.Fields?[ХарактеристикиНоменклатури_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[ХарактеристикиНоменклатури_Const.Код]?.ToString() ?? "", // [ pos = 2, type = string ]
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
        
        string Назва = "";
        string Код = "";
        string Валюта = "";
        string Організація = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, Код, Валюта, Організація };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Код */
            , typeof(string) /* Валюта */
            , typeof(string) /* Організація */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.БанківськіРахункиОрганізацій_Select БанківськіРахункиОрганізацій_Select = new Довідники.БанківськіРахункиОрганізацій_Select();
            БанківськіРахункиОрганізацій_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.БанківськіРахункиОрганізацій_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.БанківськіРахункиОрганізацій_Const.Код // [ pos = 2, type = string ]
                    /* , Довідники.БанківськіРахункиОрганізацій_Const.Валюта */ // [ pos = 3, type = pointer ]
                    /* , Довідники.БанківськіРахункиОрганізацій_Const.Організація */ // [ pos = 4, type = pointer ]
                    
                });

            
                  /* JOIN 3 */
                  БанківськіРахункиОрганізацій_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Валюти_Const.TABLE + "." + Довідники.Валюти_Const.Назва, "join_3"));
                  БанківськіРахункиОрганізацій_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.БанківськіРахункиОрганізацій_Const.Валюта, БанківськіРахункиОрганізацій_Select.QuerySelect.Table));
                
                  /* JOIN 4 */
                  БанківськіРахункиОрганізацій_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>(
                      Довідники.Організації_Const.TABLE + "." + Довідники.Організації_Const.Назва, "join_4"));
                  БанківськіРахункиОрганізацій_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Довідники.БанківськіРахункиОрганізацій_Const.Організація, БанківськіРахункиОрганізацій_Select.QuerySelect.Table));
                

            /* SELECT */
            БанківськіРахункиОрганізацій_Select.Select();
            while (БанківськіРахункиОрганізацій_Select.MoveNext())
            {
                Довідники.БанківськіРахункиОрганізацій_Pointer? cur = БанківськіРахункиОрганізацій_Select.Current;

                if (cur != null)
                    Store.AppendValues(new БанківськіРахункиОрганізацій_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[БанківськіРахункиОрганізацій_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[БанківськіРахункиОрганізацій_Const.Код]?.ToString() ?? "", // [ pos = 2, type = string ]
                        Валюта = cur.Fields?["join_3"]?.ToString() ?? "", // [ pos = 3, type = pointer ]
                        Організація = cur.Fields?["join_4"]?.ToString() ?? "" // [ pos = 4, type = pointer ]
                        
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
        
        string Назва = "";
        string Код = "";
        string Контрагент = "";
        string ТипДоговору = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Назва, Код, Контрагент, ТипДоговору };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Код */
            , typeof(string) /* Контрагент */
            , typeof(string) /* ТипДоговору */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.ДоговориКонтрагентів_Select ДоговориКонтрагентів_Select = new Довідники.ДоговориКонтрагентів_Select();
            ДоговориКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ДоговориКонтрагентів_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.ДоговориКонтрагентів_Const.Код // [ pos = 2, type = string ]
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
                        Назва = cur.Fields?[ДоговориКонтрагентів_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[ДоговориКонтрагентів_Const.Код]?.ToString() ?? "", // [ pos = 2, type = string ]
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

            Довідники.БанківськіРахункиКонтрагентів_Select БанківськіРахункиКонтрагентів_Select = new Довідники.БанківськіРахункиКонтрагентів_Select();
            БанківськіРахункиКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.БанківськіРахункиКонтрагентів_Const.Назва // [ pos = 1, type = string ]
                    , Довідники.БанківськіРахункиКонтрагентів_Const.Код // [ pos = 2, type = string ]
                    
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
                        Назва = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Назва]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Код = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Код]?.ToString() ?? "" // [ pos = 2, type = string ]
                        
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
        string Коментар = "";
        string ДатаСтворення = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Номер, Коментар, ДатаСтворення };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Номер */
            , typeof(string) /* Коментар */
            , typeof(string) /* ДатаСтворення */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            
        }

        public static void LoadRecords()
        {
            Store.Clear();

            Довідники.СеріїНоменклатури_Select СеріїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            СеріїНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.СеріїНоменклатури_Const.Номер // [ pos = 1, type = string ]
                    , Довідники.СеріїНоменклатури_Const.Коментар // [ pos = 2, type = string ]
                    , Довідники.СеріїНоменклатури_Const.ДатаСтворення // [ pos = 3, type = datetime ]
                    
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
                        Номер = cur.Fields?[СеріїНоменклатури_Const.Номер]?.ToString() ?? "", // [ pos = 1, type = string ]
                        Коментар = cur.Fields?[СеріїНоменклатури_Const.Коментар]?.ToString() ?? "", // [ pos = 2, type = string ]
                        ДатаСтворення = cur.Fields?[СеріїНоменклатури_Const.ДатаСтворення]?.ToString() ?? "" // [ pos = 3, type = datetime ]
                        
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 2) { SortColumnId = 2 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 3) { SortColumnId = 3 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 4) { SortColumnId = 4 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 5) { SortColumnId = 5 } );
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererText(), "text", 6) { SortColumnId = 6 } );
            
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

  