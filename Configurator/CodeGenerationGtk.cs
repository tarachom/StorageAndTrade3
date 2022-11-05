
/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/
  
/*
 *
 * Конфігурації "Зберігання та Торгівля 3.0"
 * Автор Тарахомин Юрій Іванович, accounting.org.ua
 * Дата конфігурації: 05.11.2022 12:03:06
 *
 */
 
using Gtk;
using AccountingSoftware;

/*
namespace StorageAndTrade_1_0.ТабличніСписки
{
    //
}
*/

namespace StorageAndTrade_1_0.Довідники.ТабличніСписки
{
    
    #region DIRECTORY "Організації"
    
      
    public class Організації_Записи
    {
        string Image = "doc.png";
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Холдинг = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, Холдинг };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* Холдинг */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Холдинг", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*Холдинг*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.Організації_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.Організації_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.Організації_Select Організації_Select = new Довідники.Організації_Select();
            Організації_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Організації_Const.Код /* 1 */
                    , Довідники.Організації_Const.Назва /* 2 */
                    
                });

            /* Where */
            Організації_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Організації_Select.QuerySelect.Order.Add(Довідники.Організації_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                Організації_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Довідники.Організації_Const.Холдинг, Організації_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  Організації_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            Організації_Select.Select();
            while (Організації_Select.MoveNext())
            {
                Довідники.Організації_Pointer? cur = Організації_Select.Current;

                if (cur != null)
                {
                    Організації_Записи Record = new Організації_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Холдинг = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[Організації_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Організації_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Одиниця пакування", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*ОдиницяВиміру*/
            treeView.AppendColumn(new TreeViewColumn("Виробник", new CellRendererText() { Xpad = 4 }, "text", 5) { SortColumnId = 5 } ); /*Виробник*/
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText() { Xpad = 4 }, "text", 6) { SortColumnId = 6 } ); /*ТипНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Вид", new CellRendererText() { Xpad = 4 }, "text", 7) { SortColumnId = 7 } ); /*ВидНоменклатури*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.Номенклатура_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.Номенклатура_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.Номенклатура_Select Номенклатура_Select = new Довідники.Номенклатура_Select();
            Номенклатура_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Номенклатура_Const.Код /* 1 */
                    , Довідники.Номенклатура_Const.Назва /* 2 */
                    , Довідники.Номенклатура_Const.ТипНоменклатури /* 3 */
                    
                });

            /* Where */
            Номенклатура_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Номенклатура_Select.QuerySelect.Order.Add(Довідники.Номенклатура_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, Довідники.Номенклатура_Const.ОдиницяВиміру, Номенклатура_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Виробники_Const.TABLE, Довідники.Номенклатура_Const.Виробник, Номенклатура_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Виробники_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ВидиНоменклатури_Const.TABLE, Довідники.Номенклатура_Const.ВидНоменклатури, Номенклатура_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.ВидиНоменклатури_Const.Назва, "join_tab_3_field_1"));
                  

            /* SELECT */
            Номенклатура_Select.Select();
            while (Номенклатура_Select.MoveNext())
            {
                Довідники.Номенклатура_Pointer? cur = Номенклатура_Select.Current;

                if (cur != null)
                {
                    Номенклатура_Записи Record = new Номенклатура_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        ОдиницяВиміру = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Виробник = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        ВидНоменклатури = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[Номенклатура_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Номенклатура_Const.Назва]?.ToString() ?? "", /**/
                        ТипНоменклатури = ((Перелічення.ТипиНоменклатури)(cur.Fields?[Номенклатура_Const.ТипНоменклатури]!)).ToString() /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.Виробники_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.Виробники_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.Виробники_Select Виробники_Select = new Довідники.Виробники_Select();
            Виробники_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Виробники_Const.Код /* 1 */
                    , Довідники.Виробники_Const.Назва /* 2 */
                    
                });

            /* Where */
            Виробники_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Виробники_Select.QuerySelect.Order.Add(Довідники.Виробники_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            Виробники_Select.Select();
            while (Виробники_Select.MoveNext())
            {
                Довідники.Виробники_Pointer? cur = Виробники_Select.Current;

                if (cur != null)
                {
                    Виробники_Записи Record = new Виробники_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Виробники_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Виробники_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ВидиНоменклатури_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ВидиНоменклатури_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ВидиНоменклатури_Select ВидиНоменклатури_Select = new Довідники.ВидиНоменклатури_Select();
            ВидиНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ВидиНоменклатури_Const.Код /* 1 */
                    , Довідники.ВидиНоменклатури_Const.Назва /* 2 */
                    
                });

            /* Where */
            ВидиНоменклатури_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ВидиНоменклатури_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            ВидиНоменклатури_Select.Select();
            while (ВидиНоменклатури_Select.MoveNext())
            {
                Довідники.ВидиНоменклатури_Pointer? cur = ВидиНоменклатури_Select.Current;

                if (cur != null)
                {
                    ВидиНоменклатури_Записи Record = new ВидиНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ВидиНоменклатури_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВидиНоменклатури_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ПакуванняОдиниціВиміру_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ПакуванняОдиниціВиміру_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ПакуванняОдиниціВиміру_Select ПакуванняОдиниціВиміру_Select = new Довідники.ПакуванняОдиниціВиміру_Select();
            ПакуванняОдиниціВиміру_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ПакуванняОдиниціВиміру_Const.Код /* 1 */
                    , Довідники.ПакуванняОдиниціВиміру_Const.Назва /* 2 */
                    
                });

            /* Where */
            ПакуванняОдиниціВиміру_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПакуванняОдиниціВиміру_Select.QuerySelect.Order.Add(Довідники.ПакуванняОдиниціВиміру_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            ПакуванняОдиниціВиміру_Select.Select();
            while (ПакуванняОдиниціВиміру_Select.MoveNext())
            {
                Довідники.ПакуванняОдиниціВиміру_Pointer? cur = ПакуванняОдиниціВиміру_Select.Current;

                if (cur != null)
                {
                    ПакуванняОдиниціВиміру_Записи Record = new ПакуванняОдиниціВиміру_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ПакуванняОдиниціВиміру_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПакуванняОдиниціВиміру_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
        string Код_R030 = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, КороткаНазва, Код_R030 };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* КороткаНазва */
            , typeof(string) /* Код_R030 */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Коротка назва", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*КороткаНазва*/
            treeView.AppendColumn(new TreeViewColumn("R030", new CellRendererText() { Xpad = 4 }, "text", 5) { SortColumnId = 5 } ); /*Код_R030*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.Валюти_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.Валюти_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.Валюти_Select Валюти_Select = new Довідники.Валюти_Select();
            Валюти_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Валюти_Const.Код /* 1 */
                    , Довідники.Валюти_Const.Назва /* 2 */
                    , Довідники.Валюти_Const.КороткаНазва /* 3 */
                    , Довідники.Валюти_Const.Код_R030 /* 4 */
                    
                });

            /* Where */
            Валюти_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Валюти_Select.QuerySelect.Order.Add(Довідники.Валюти_Const.Код, SelectOrder.ASC);
            

            /* SELECT */
            Валюти_Select.Select();
            while (Валюти_Select.MoveNext())
            {
                Довідники.Валюти_Pointer? cur = Валюти_Select.Current;

                if (cur != null)
                {
                    Валюти_Записи Record = new Валюти_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Валюти_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Валюти_Const.Назва]?.ToString() ?? "", /**/
                        КороткаНазва = cur.Fields?[Валюти_Const.КороткаНазва]?.ToString() ?? "", /**/
                        Код_R030 = cur.Fields?[Валюти_Const.Код_R030]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
        string Папка = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, Папка };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* Папка */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Папка", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*Папка*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.Контрагенти_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.Контрагенти_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.Контрагенти_Select Контрагенти_Select = new Довідники.Контрагенти_Select();
            Контрагенти_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Контрагенти_Const.Код /* 1 */
                    , Довідники.Контрагенти_Const.Назва /* 2 */
                    
                });

            /* Where */
            Контрагенти_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Контрагенти_Select.QuerySelect.Order.Add(Довідники.Контрагенти_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                Контрагенти_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Папки_Const.TABLE, Довідники.Контрагенти_Const.Папка, Контрагенти_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  Контрагенти_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Контрагенти_Папки_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            Контрагенти_Select.Select();
            while (Контрагенти_Select.MoveNext())
            {
                Довідники.Контрагенти_Pointer? cur = Контрагенти_Select.Current;

                if (cur != null)
                {
                    Контрагенти_Записи Record = new Контрагенти_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Папка = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[Контрагенти_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Контрагенти_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.Склади_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.Склади_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.Склади_Select Склади_Select = new Довідники.Склади_Select();
            Склади_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Склади_Const.Код /* 1 */
                    , Довідники.Склади_Const.Назва /* 2 */
                    
                });

            /* Where */
            Склади_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Склади_Select.QuerySelect.Order.Add(Довідники.Склади_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            Склади_Select.Select();
            while (Склади_Select.MoveNext())
            {
                Довідники.Склади_Pointer? cur = Склади_Select.Current;

                if (cur != null)
                {
                    Склади_Записи Record = new Склади_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Склади_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Склади_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*Валюта*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ВидиЦін_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ВидиЦін_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ВидиЦін_Select ВидиЦін_Select = new Довідники.ВидиЦін_Select();
            ВидиЦін_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ВидиЦін_Const.Код /* 1 */
                    , Довідники.ВидиЦін_Const.Назва /* 2 */
                    
                });

            /* Where */
            ВидиЦін_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиЦін_Select.QuerySelect.Order.Add(Довідники.ВидиЦін_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                ВидиЦін_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.ВидиЦін_Const.Валюта, ВидиЦін_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ВидиЦін_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Валюти_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            ВидиЦін_Select.Select();
            while (ВидиЦін_Select.MoveNext())
            {
                Довідники.ВидиЦін_Pointer? cur = ВидиЦін_Select.Current;

                if (cur != null)
                {
                    ВидиЦін_Записи Record = new ВидиЦін_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[ВидиЦін_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВидиЦін_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ВидиЦінПостачальників_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ВидиЦінПостачальників_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ВидиЦінПостачальників_Select ВидиЦінПостачальників_Select = new Довідники.ВидиЦінПостачальників_Select();
            ВидиЦінПостачальників_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ВидиЦінПостачальників_Const.Код /* 1 */
                    , Довідники.ВидиЦінПостачальників_Const.Назва /* 2 */
                    
                });

            /* Where */
            ВидиЦінПостачальників_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиЦінПостачальників_Select.QuerySelect.Order.Add(Довідники.ВидиЦінПостачальників_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            ВидиЦінПостачальників_Select.Select();
            while (ВидиЦінПостачальників_Select.MoveNext())
            {
                Довідники.ВидиЦінПостачальників_Pointer? cur = ВидиЦінПостачальників_Select.Current;

                if (cur != null)
                {
                    ВидиЦінПостачальників_Записи Record = new ВидиЦінПостачальників_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ВидиЦінПостачальників_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВидиЦінПостачальників_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.Користувачі_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.Користувачі_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.Користувачі_Select Користувачі_Select = new Довідники.Користувачі_Select();
            Користувачі_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Користувачі_Const.Код /* 1 */
                    , Довідники.Користувачі_Const.Назва /* 2 */
                    
                });

            /* Where */
            Користувачі_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Користувачі_Select.QuerySelect.Order.Add(Довідники.Користувачі_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            Користувачі_Select.Select();
            while (Користувачі_Select.MoveNext())
            {
                Довідники.Користувачі_Pointer? cur = Користувачі_Select.Current;

                if (cur != null)
                {
                    Користувачі_Записи Record = new Користувачі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Користувачі_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Користувачі_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ФізичніОсоби_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ФізичніОсоби_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ФізичніОсоби_Select ФізичніОсоби_Select = new Довідники.ФізичніОсоби_Select();
            ФізичніОсоби_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ФізичніОсоби_Const.Код /* 1 */
                    , Довідники.ФізичніОсоби_Const.Назва /* 2 */
                    
                });

            /* Where */
            ФізичніОсоби_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ФізичніОсоби_Select.QuerySelect.Order.Add(Довідники.ФізичніОсоби_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            ФізичніОсоби_Select.Select();
            while (ФізичніОсоби_Select.MoveNext())
            {
                Довідники.ФізичніОсоби_Pointer? cur = ФізичніОсоби_Select.Current;

                if (cur != null)
                {
                    ФізичніОсоби_Записи Record = new ФізичніОсоби_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[ФізичніОсоби_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ФізичніОсоби_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.СтруктураПідприємства_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.СтруктураПідприємства_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.СтруктураПідприємства_Select СтруктураПідприємства_Select = new Довідники.СтруктураПідприємства_Select();
            СтруктураПідприємства_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.СтруктураПідприємства_Const.Код /* 1 */
                    , Довідники.СтруктураПідприємства_Const.Назва /* 2 */
                    
                });

            /* Where */
            СтруктураПідприємства_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СтруктураПідприємства_Select.QuerySelect.Order.Add(Довідники.СтруктураПідприємства_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            СтруктураПідприємства_Select.Select();
            while (СтруктураПідприємства_Select.MoveNext())
            {
                Довідники.СтруктураПідприємства_Pointer? cur = СтруктураПідприємства_Select.Current;

                if (cur != null)
                {
                    СтруктураПідприємства_Записи Record = new СтруктураПідприємства_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[СтруктураПідприємства_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[СтруктураПідприємства_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.КраїниСвіту_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.КраїниСвіту_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.КраїниСвіту_Select КраїниСвіту_Select = new Довідники.КраїниСвіту_Select();
            КраїниСвіту_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.КраїниСвіту_Const.Код /* 1 */
                    , Довідники.КраїниСвіту_Const.Назва /* 2 */
                    
                });

            /* Where */
            КраїниСвіту_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              КраїниСвіту_Select.QuerySelect.Order.Add(Довідники.КраїниСвіту_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            КраїниСвіту_Select.Select();
            while (КраїниСвіту_Select.MoveNext())
            {
                Довідники.КраїниСвіту_Pointer? cur = КраїниСвіту_Select.Current;

                if (cur != null)
                {
                    КраїниСвіту_Записи Record = new КраїниСвіту_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[КраїниСвіту_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[КраїниСвіту_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.Файли_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.Файли_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.Файли_Select Файли_Select = new Довідники.Файли_Select();
            Файли_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Файли_Const.Код /* 1 */
                    , Довідники.Файли_Const.Назва /* 2 */
                    
                });

            /* Where */
            Файли_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Файли_Select.QuerySelect.Order.Add(Довідники.Файли_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            Файли_Select.Select();
            while (Файли_Select.MoveNext())
            {
                Довідники.Файли_Pointer? cur = Файли_Select.Current;

                if (cur != null)
                {
                    Файли_Записи Record = new Файли_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Код = cur.Fields?[Файли_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Файли_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*Номенклатура*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ХарактеристикиНоменклатури_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ХарактеристикиНоменклатури_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ХарактеристикиНоменклатури_Select ХарактеристикиНоменклатури_Select = new Довідники.ХарактеристикиНоменклатури_Select();
            ХарактеристикиНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ХарактеристикиНоменклатури_Const.Код /* 1 */
                    , Довідники.ХарактеристикиНоменклатури_Const.Назва /* 2 */
                    
                });

            /* Where */
            ХарактеристикиНоменклатури_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ХарактеристикиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ХарактеристикиНоменклатури_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                ХарактеристикиНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Номенклатура_Const.TABLE, Довідники.ХарактеристикиНоменклатури_Const.Номенклатура, ХарактеристикиНоменклатури_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Номенклатура_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            ХарактеристикиНоменклатури_Select.Select();
            while (ХарактеристикиНоменклатури_Select.MoveNext())
            {
                Довідники.ХарактеристикиНоменклатури_Pointer? cur = ХарактеристикиНоменклатури_Select.Current;

                if (cur != null)
                {
                    ХарактеристикиНоменклатури_Записи Record = new ХарактеристикиНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Номенклатура = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[ХарактеристикиНоменклатури_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ХарактеристикиНоменклатури_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*Валюта*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.Каси_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.Каси_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.Каси_Select Каси_Select = new Довідники.Каси_Select();
            Каси_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.Каси_Const.Код /* 1 */
                    , Довідники.Каси_Const.Назва /* 2 */
                    
                });

            /* Where */
            Каси_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Каси_Select.QuerySelect.Order.Add(Довідники.Каси_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                Каси_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.Каси_Const.Валюта, Каси_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  Каси_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Валюти_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            Каси_Select.Select();
            while (Каси_Select.MoveNext())
            {
                Довідники.Каси_Pointer? cur = Каси_Select.Current;

                if (cur != null)
                {
                    Каси_Записи Record = new Каси_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[Каси_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Каси_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*Валюта*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.БанківськіРахункиОрганізацій_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.БанківськіРахункиОрганізацій_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.БанківськіРахункиОрганізацій_Select БанківськіРахункиОрганізацій_Select = new Довідники.БанківськіРахункиОрганізацій_Select();
            БанківськіРахункиОрганізацій_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.БанківськіРахункиОрганізацій_Const.Код /* 1 */
                    , Довідники.БанківськіРахункиОрганізацій_Const.Назва /* 2 */
                    
                });

            /* Where */
            БанківськіРахункиОрганізацій_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              БанківськіРахункиОрганізацій_Select.QuerySelect.Order.Add(Довідники.БанківськіРахункиОрганізацій_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                БанківськіРахункиОрганізацій_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.БанківськіРахункиОрганізацій_Const.Валюта, БанківськіРахункиОрганізацій_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  БанківськіРахункиОрганізацій_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Валюти_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            БанківськіРахункиОрганізацій_Select.Select();
            while (БанківськіРахункиОрганізацій_Select.MoveNext())
            {
                Довідники.БанківськіРахункиОрганізацій_Pointer? cur = БанківськіРахункиОрганізацій_Select.Current;

                if (cur != null)
                {
                    БанківськіРахункиОрганізацій_Записи Record = new БанківськіРахункиОрганізацій_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[БанківськіРахункиОрганізацій_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[БанківськіРахункиОрганізацій_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("ТипДоговору", new CellRendererText() { Xpad = 4 }, "text", 5) { SortColumnId = 5 } ); /*ТипДоговору*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ДоговориКонтрагентів_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ДоговориКонтрагентів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ДоговориКонтрагентів_Select ДоговориКонтрагентів_Select = new Довідники.ДоговориКонтрагентів_Select();
            ДоговориКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ДоговориКонтрагентів_Const.Код /* 1 */
                    , Довідники.ДоговориКонтрагентів_Const.Назва /* 2 */
                    , Довідники.ДоговориКонтрагентів_Const.ТипДоговору /* 3 */
                    
                });

            /* Where */
            ДоговориКонтрагентів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ДоговориКонтрагентів_Select.QuerySelect.Order.Add(Довідники.ДоговориКонтрагентів_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                ДоговориКонтрагентів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Довідники.ДоговориКонтрагентів_Const.Контрагент, ДоговориКонтрагентів_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ДоговориКонтрагентів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Контрагенти_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            ДоговориКонтрагентів_Select.Select();
            while (ДоговориКонтрагентів_Select.MoveNext())
            {
                Довідники.ДоговориКонтрагентів_Pointer? cur = ДоговориКонтрагентів_Select.Current;

                if (cur != null)
                {
                    ДоговориКонтрагентів_Записи Record = new ДоговориКонтрагентів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Контрагент = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[ДоговориКонтрагентів_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ДоговориКонтрагентів_Const.Назва]?.ToString() ?? "", /**/
                        ТипДоговору = ((Перелічення.ТипДоговорів)(cur.Fields?[ДоговориКонтрагентів_Const.ТипДоговору]!)).ToString() /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*Валюта*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.БанківськіРахункиКонтрагентів_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.БанківськіРахункиКонтрагентів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.БанківськіРахункиКонтрагентів_Select БанківськіРахункиКонтрагентів_Select = new Довідники.БанківськіРахункиКонтрагентів_Select();
            БанківськіРахункиКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.БанківськіРахункиКонтрагентів_Const.Код /* 1 */
                    , Довідники.БанківськіРахункиКонтрагентів_Const.Назва /* 2 */
                    
                });

            /* Where */
            БанківськіРахункиКонтрагентів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              БанківськіРахункиКонтрагентів_Select.QuerySelect.Order.Add(Довідники.БанківськіРахункиКонтрагентів_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                БанківськіРахункиКонтрагентів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.БанківськіРахункиКонтрагентів_Const.Валюта, БанківськіРахункиКонтрагентів_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  БанківськіРахункиКонтрагентів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Валюти_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            БанківськіРахункиКонтрагентів_Select.Select();
            while (БанківськіРахункиКонтрагентів_Select.MoveNext())
            {
                Довідники.БанківськіРахункиКонтрагентів_Pointer? cur = БанківськіРахункиКонтрагентів_Select.Current;

                if (cur != null)
                {
                    БанківськіРахункиКонтрагентів_Записи Record = new БанківськіРахункиКонтрагентів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Код*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.СтаттяРухуКоштів_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.СтаттяРухуКоштів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.СтаттяРухуКоштів_Select СтаттяРухуКоштів_Select = new Довідники.СтаттяРухуКоштів_Select();
            СтаттяРухуКоштів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.СтаттяРухуКоштів_Const.Назва /* 1 */
                    , Довідники.СтаттяРухуКоштів_Const.Код /* 2 */
                    
                });

            /* Where */
            СтаттяРухуКоштів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СтаттяРухуКоштів_Select.QuerySelect.Order.Add(Довідники.СтаттяРухуКоштів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            СтаттяРухуКоштів_Select.Select();
            while (СтаттяРухуКоштів_Select.MoveNext())
            {
                Довідники.СтаттяРухуКоштів_Pointer? cur = СтаттяРухуКоштів_Select.Current;

                if (cur != null)
                {
                    СтаттяРухуКоштів_Записи Record = new СтаттяРухуКоштів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[СтаттяРухуКоштів_Const.Назва]?.ToString() ?? "", /**/
                        Код = cur.Fields?[СтаттяРухуКоштів_Const.Код]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Номер*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.СеріїНоменклатури_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.СеріїНоменклатури_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.СеріїНоменклатури_Select СеріїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            СеріїНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.СеріїНоменклатури_Const.Номер /* 1 */
                    
                });

            /* Where */
            СеріїНоменклатури_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СеріїНоменклатури_Select.QuerySelect.Order.Add(Довідники.СеріїНоменклатури_Const.Номер, SelectOrder.ASC);
            

            /* SELECT */
            СеріїНоменклатури_Select.Select();
            while (СеріїНоменклатури_Select.MoveNext())
            {
                Довідники.СеріїНоменклатури_Pointer? cur = СеріїНоменклатури_Select.Current;

                if (cur != null)
                {
                    СеріїНоменклатури_Записи Record = new СеріїНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Номер = cur.Fields?[СеріїНоменклатури_Const.Номер]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 } ); /*Дата*/
            treeView.AppendColumn(new TreeViewColumn("ТипДокументу", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 } ); /*ТипДокументу*/
            treeView.AppendColumn(new TreeViewColumn("ПоступленняТоварівТаПослуг", new CellRendererText() { Xpad = 4 }, "text", 5) { SortColumnId = 5 } ); /*ПоступленняТоварівТаПослуг*/
            treeView.AppendColumn(new TreeViewColumn("ВведенняЗалишків", new CellRendererText() { Xpad = 4 }, "text", 6) { SortColumnId = 6 } ); /*ВведенняЗалишків*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ПартіяТоварівКомпозит_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ПартіяТоварівКомпозит_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ПартіяТоварівКомпозит_Select ПартіяТоварівКомпозит_Select = new Довідники.ПартіяТоварівКомпозит_Select();
            ПартіяТоварівКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ПартіяТоварівКомпозит_Const.Назва /* 1 */
                    , Довідники.ПартіяТоварівКомпозит_Const.Дата /* 2 */
                    , Довідники.ПартіяТоварівКомпозит_Const.ТипДокументу /* 3 */
                    
                });

            /* Where */
            ПартіяТоварівКомпозит_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПартіяТоварівКомпозит_Select.QuerySelect.Order.Add(Довідники.ПартіяТоварівКомпозит_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                ПартіяТоварівКомпозит_Select.QuerySelect.Joins.Add(
                    new Join(Документи.ПоступленняТоварівТаПослуг_Const.TABLE, Довідники.ПартіяТоварівКомпозит_Const.ПоступленняТоварівТаПослуг, ПартіяТоварівКомпозит_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПартіяТоварівКомпозит_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Документи.ПоступленняТоварівТаПослуг_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПартіяТоварівКомпозит_Select.QuerySelect.Joins.Add(
                    new Join(Документи.ВведенняЗалишків_Const.TABLE, Довідники.ПартіяТоварівКомпозит_Const.ВведенняЗалишків, ПартіяТоварівКомпозит_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПартіяТоварівКомпозит_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Документи.ВведенняЗалишків_Const.Назва, "join_tab_2_field_1"));
                  

            /* SELECT */
            ПартіяТоварівКомпозит_Select.Select();
            while (ПартіяТоварівКомпозит_Select.MoveNext())
            {
                Довідники.ПартіяТоварівКомпозит_Pointer? cur = ПартіяТоварівКомпозит_Select.Current;

                if (cur != null)
                {
                    ПартіяТоварівКомпозит_Записи Record = new ПартіяТоварівКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        ПоступленняТоварівТаПослуг = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        ВведенняЗалишків = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПартіяТоварівКомпозит_Const.Назва]?.ToString() ?? "", /**/
                        Дата = cur.Fields?[ПартіяТоварівКомпозит_Const.Дата]?.ToString() ?? "", /**/
                        ТипДокументу = ((Перелічення.ТипДокументуПартіяТоварівКомпозит)(cur.Fields?[ПартіяТоварівКомпозит_Const.ТипДокументу]!)).ToString() /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ВидиЗапасів_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ВидиЗапасів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ВидиЗапасів_Select ВидиЗапасів_Select = new Довідники.ВидиЗапасів_Select();
            ВидиЗапасів_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ВидиЗапасів_Const.Назва /* 1 */
                    
                });

            /* Where */
            ВидиЗапасів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиЗапасів_Select.QuerySelect.Order.Add(Довідники.ВидиЗапасів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            ВидиЗапасів_Select.Select();
            while (ВидиЗапасів_Select.MoveNext())
            {
                Довідники.ВидиЗапасів_Pointer? cur = ВидиЗапасів_Select.Current;

                if (cur != null)
                {
                    ВидиЗапасів_Записи Record = new ВидиЗапасів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ВидиЗапасів_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.ПродажіДокументКомпозит_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.ПродажіДокументКомпозит_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.ПродажіДокументКомпозит_Select ПродажіДокументКомпозит_Select = new Довідники.ПродажіДокументКомпозит_Select();
            ПродажіДокументКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.ПродажіДокументКомпозит_Const.Назва /* 1 */
                    
                });

            /* Where */
            ПродажіДокументКомпозит_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПродажіДокументКомпозит_Select.QuerySelect.Order.Add(Довідники.ПродажіДокументКомпозит_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            ПродажіДокументКомпозит_Select.Select();
            while (ПродажіДокументКомпозит_Select.MoveNext())
            {
                Довідники.ПродажіДокументКомпозит_Pointer? cur = ПродажіДокументКомпозит_Select.Current;

                if (cur != null)
                {
                    ПродажіДокументКомпозит_Записи Record = new ПродажіДокументКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[ПродажіДокументКомпозит_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.АналітикаНоменклатуриКомпозит_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.АналітикаНоменклатуриКомпозит_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.АналітикаНоменклатуриКомпозит_Select АналітикаНоменклатуриКомпозит_Select = new Довідники.АналітикаНоменклатуриКомпозит_Select();
            АналітикаНоменклатуриКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.АналітикаНоменклатуриКомпозит_Const.Назва /* 1 */
                    
                });

            /* Where */
            АналітикаНоменклатуриКомпозит_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              АналітикаНоменклатуриКомпозит_Select.QuerySelect.Order.Add(Довідники.АналітикаНоменклатуриКомпозит_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            АналітикаНоменклатуриКомпозит_Select.Select();
            while (АналітикаНоменклатуриКомпозит_Select.MoveNext())
            {
                Довідники.АналітикаНоменклатуриКомпозит_Pointer? cur = АналітикаНоменклатуриКомпозит_Select.Current;

                if (cur != null)
                {
                    АналітикаНоменклатуриКомпозит_Записи Record = new АналітикаНоменклатуриКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[АналітикаНоменклатуриКомпозит_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.АналітикаКонтрагентівКомпозит_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.АналітикаКонтрагентівКомпозит_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.АналітикаКонтрагентівКомпозит_Select АналітикаКонтрагентівКомпозит_Select = new Довідники.АналітикаКонтрагентівКомпозит_Select();
            АналітикаКонтрагентівКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.АналітикаКонтрагентівКомпозит_Const.Назва /* 1 */
                    
                });

            /* Where */
            АналітикаКонтрагентівКомпозит_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              АналітикаКонтрагентівКомпозит_Select.QuerySelect.Order.Add(Довідники.АналітикаКонтрагентівКомпозит_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            АналітикаКонтрагентівКомпозит_Select.Select();
            while (АналітикаКонтрагентівКомпозит_Select.MoveNext())
            {
                Довідники.АналітикаКонтрагентівКомпозит_Pointer? cur = АналітикаКонтрагентівКомпозит_Select.Current;

                if (cur != null)
                {
                    АналітикаКонтрагентівКомпозит_Записи Record = new АналітикаКонтрагентівКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[АналітикаКонтрагентівКомпозит_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 } ); /*Назва*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static Довідники.АналітикаПартійКомпозит_Pointer? DirectoryPointerItem { get; set; }
        public static Довідники.АналітикаПартійКомпозит_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = null;

            Довідники.АналітикаПартійКомпозит_Select АналітикаПартійКомпозит_Select = new Довідники.АналітикаПартійКомпозит_Select();
            АналітикаПартійКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                    Довідники.АналітикаПартійКомпозит_Const.Назва /* 1 */
                    
                });

            /* Where */
            АналітикаПартійКомпозит_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              АналітикаПартійКомпозит_Select.QuerySelect.Order.Add(Довідники.АналітикаПартійКомпозит_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            АналітикаПартійКомпозит_Select.Select();
            while (АналітикаПартійКомпозит_Select.MoveNext())
            {
                Довідники.АналітикаПартійКомпозит_Pointer? cur = АналітикаПартійКомпозит_Select.Current;

                if (cur != null)
                {
                    АналітикаПартійКомпозит_Записи Record = new АналітикаПартійКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Назва = cur.Fields?[АналітикаПартійКомпозит_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
}

namespace StorageAndTrade_1_0.Документи.ТабличніСписки
{
    public static class Інтерфейс
    {
        public static ComboBoxText СписокВідбірПоПеріоду()
        {
            ComboBoxText сomboBox = new ComboBoxText();

            if (Config.Kernel != null)
            {
                ConfigurationEnums ТипПеріодуДляЖурналівДокументів = Config.Kernel.Conf.Enums["ТипПеріодуДляЖурналівДокументів"];

                foreach (ConfigurationEnumField field in ТипПеріодуДляЖурналівДокументів.Fields.Values)
                    сomboBox.Append(field.Name, field.Desc);
            }

            return сomboBox;
        }

        public static void ДодатиВідбірПоПеріоду(List<Where> Where, string fieldWhere, Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            switch (типПеріоду)
            {
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
                {
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, 1, 1)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
                {
                    DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
                {
                    DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
                {
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
                {
                    DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
                {
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
                    break;
                }
            }
        }
    }

    
    #region DOCUMENT "ЗамовленняПостачальнику"
    
      
    public class ЗамовленняПостачальнику_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Склад, Валюта, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5)); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 10)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 11)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ЗамовленняПостачальнику_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ЗамовленняПостачальнику_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ЗамовленняПостачальнику_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ЗамовленняПостачальнику_Select ЗамовленняПостачальнику_Select = new Документи.ЗамовленняПостачальнику_Select();
            ЗамовленняПостачальнику_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ЗамовленняПостачальнику_Const.Назва /* 1 */
                    , Документи.ЗамовленняПостачальнику_Const.НомерДок /* 2 */
                    , Документи.ЗамовленняПостачальнику_Const.ДатаДок /* 3 */
                    , Документи.ЗамовленняПостачальнику_Const.СумаДокументу /* 4 */
                    , Документи.ЗамовленняПостачальнику_Const.Коментар /* 5 */
                    
                });

            /* Where */
            ЗамовленняПостачальнику_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ЗамовленняПостачальнику_Select.QuerySelect.Order.Add(Документи.ЗамовленняПостачальнику_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ЗамовленняПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Організація, ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ЗамовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ЗамовленняПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Контрагент, ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ЗамовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ЗамовленняПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Склад, ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ЗамовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                ЗамовленняПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Валюта, ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ЗамовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Валюти_Const.Назва, "join_tab_4_field_1"));
                  

            /* SELECT */
            ЗамовленняПостачальнику_Select.Select();
            while (ЗамовленняПостачальнику_Select.MoveNext())
            {
                Документи.ЗамовленняПостачальнику_Pointer? cur = ЗамовленняПостачальнику_Select.Current;

                if (cur != null)
                {
                    ЗамовленняПостачальнику_Записи Record = new ЗамовленняПостачальнику_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ЗамовленняПостачальнику_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ЗамовленняПостачальнику_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ЗамовленняПостачальнику_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ЗамовленняПостачальнику_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ЗамовленняПостачальнику_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПоступленняТоварівТаПослуг"
    
      
    public class ПоступленняТоварівТаПослуг_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Контрагент, Валюта, Каса, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5)); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 10)); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ПоступленняТоварівТаПослуг_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ПоступленняТоварівТаПослуг_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПоступленняТоварівТаПослуг_Select ПоступленняТоварівТаПослуг_Select = new Документи.ПоступленняТоварівТаПослуг_Select();
            ПоступленняТоварівТаПослуг_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ПоступленняТоварівТаПослуг_Const.Назва /* 1 */
                    , Документи.ПоступленняТоварівТаПослуг_Const.НомерДок /* 2 */
                    , Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок /* 3 */
                    , Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу /* 4 */
                    , Документи.ПоступленняТоварівТаПослуг_Const.Коментар /* 5 */
                    
                });

            /* Where */
            ПоступленняТоварівТаПослуг_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПоступленняТоварівТаПослуг_Select.QuerySelect.Order.Add(Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Організація, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Склад, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Контрагент, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Контрагенти_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Валюта, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Валюти_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Каса, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Каси_Const.Назва, "join_tab_5_field_1"));
                  

            /* SELECT */
            ПоступленняТоварівТаПослуг_Select.Select();
            while (ПоступленняТоварівТаПослуг_Select.MoveNext())
            {
                Документи.ПоступленняТоварівТаПослуг_Pointer? cur = ПоступленняТоварівТаПослуг_Select.Current;

                if (cur != null)
                {
                    ПоступленняТоварівТаПослуг_Записи Record = new ПоступленняТоварівТаПослуг_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПоступленняТоварівТаПослуг_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПоступленняТоварівТаПослуг_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПоступленняТоварівТаПослуг_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПоступленняТоварівТаПослуг_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПоступленняТоварівТаПослуг_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ЗамовленняКлієнта"
    
      
    public class ЗамовленняКлієнта_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ЗамовленняКлієнта_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ЗамовленняКлієнта_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ЗамовленняКлієнта_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ЗамовленняКлієнта_Select ЗамовленняКлієнта_Select = new Документи.ЗамовленняКлієнта_Select();
            ЗамовленняКлієнта_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ЗамовленняКлієнта_Const.Назва /* 1 */
                    , Документи.ЗамовленняКлієнта_Const.НомерДок /* 2 */
                    , Документи.ЗамовленняКлієнта_Const.ДатаДок /* 3 */
                    , Документи.ЗамовленняКлієнта_Const.СумаДокументу /* 4 */
                    , Документи.ЗамовленняКлієнта_Const.Коментар /* 5 */
                    
                });

            /* Where */
            ЗамовленняКлієнта_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ЗамовленняКлієнта_Select.QuerySelect.Order.Add(Документи.ЗамовленняКлієнта_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Організація, ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Контрагент, ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Валюта, ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Валюти_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Каса, ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Склад, ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Склади_Const.Назва, "join_tab_5_field_1"));
                  

            /* SELECT */
            ЗамовленняКлієнта_Select.Select();
            while (ЗамовленняКлієнта_Select.MoveNext())
            {
                Документи.ЗамовленняКлієнта_Pointer? cur = ЗамовленняКлієнта_Select.Current;

                if (cur != null)
                {
                    ЗамовленняКлієнта_Записи Record = new ЗамовленняКлієнта_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ЗамовленняКлієнта_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ЗамовленняКлієнта_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ЗамовленняКлієнта_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ЗамовленняКлієнта_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ЗамовленняКлієнта_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "РеалізаціяТоварівТаПослуг"
    
      
    public class РеалізаціяТоварівТаПослуг_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, типПеріоду);
        }

        public static Документи.РеалізаціяТоварівТаПослуг_Pointer? DocumentPointerItem { get; set; }
        public static Документи.РеалізаціяТоварівТаПослуг_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.РеалізаціяТоварівТаПослуг_Select РеалізаціяТоварівТаПослуг_Select = new Документи.РеалізаціяТоварівТаПослуг_Select();
            РеалізаціяТоварівТаПослуг_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.РеалізаціяТоварівТаПослуг_Const.Назва /* 1 */
                    , Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок /* 2 */
                    , Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок /* 3 */
                    , Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу /* 4 */
                    , Документи.РеалізаціяТоварівТаПослуг_Const.Коментар /* 5 */
                    
                });

            /* Where */
            РеалізаціяТоварівТаПослуг_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              РеалізаціяТоварівТаПослуг_Select.QuerySelect.Order.Add(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Організація, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Валюта, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Валюти_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Каса, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Склад, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Склади_Const.Назва, "join_tab_5_field_1"));
                  

            /* SELECT */
            РеалізаціяТоварівТаПослуг_Select.Select();
            while (РеалізаціяТоварівТаПослуг_Select.MoveNext())
            {
                Документи.РеалізаціяТоварівТаПослуг_Pointer? cur = РеалізаціяТоварівТаПослуг_Select.Current;

                if (cur != null)
                {
                    РеалізаціяТоварівТаПослуг_Записи Record = new РеалізаціяТоварівТаПослуг_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ВстановленняЦінНоменклатури"
    
      
    public class ВстановленняЦінНоменклатури_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, ВидЦіни, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Вид ціни", new CellRendererText() { Xpad = 4 }, "text", 8)); /*ВидЦіни*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ВстановленняЦінНоменклатури_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ВстановленняЦінНоменклатури_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ВстановленняЦінНоменклатури_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ВстановленняЦінНоменклатури_Select ВстановленняЦінНоменклатури_Select = new Документи.ВстановленняЦінНоменклатури_Select();
            ВстановленняЦінНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ВстановленняЦінНоменклатури_Const.Назва /* 1 */
                    , Документи.ВстановленняЦінНоменклатури_Const.НомерДок /* 2 */
                    , Документи.ВстановленняЦінНоменклатури_Const.ДатаДок /* 3 */
                    , Документи.ВстановленняЦінНоменклатури_Const.Коментар /* 4 */
                    
                });

            /* Where */
            ВстановленняЦінНоменклатури_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВстановленняЦінНоменклатури_Select.QuerySelect.Order.Add(Документи.ВстановленняЦінНоменклатури_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ВстановленняЦінНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.Організація, ВстановленняЦінНоменклатури_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ВстановленняЦінНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ВстановленняЦінНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.Валюта, ВстановленняЦінНоменклатури_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ВстановленняЦінНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Валюти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ВстановленняЦінНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ВидиЦін_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.ВидЦіни, ВстановленняЦінНоменклатури_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ВстановленняЦінНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.ВидиЦін_Const.Назва, "join_tab_3_field_1"));
                  

            /* SELECT */
            ВстановленняЦінНоменклатури_Select.Select();
            while (ВстановленняЦінНоменклатури_Select.MoveNext())
            {
                Документи.ВстановленняЦінНоменклатури_Pointer? cur = ВстановленняЦінНоменклатури_Select.Current;

                if (cur != null)
                {
                    ВстановленняЦінНоменклатури_Записи Record = new ВстановленняЦінНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        ВидЦіни = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВстановленняЦінНоменклатури_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ВстановленняЦінНоменклатури_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ВстановленняЦінНоменклатури_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ВстановленняЦінНоменклатури_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПрихіднийКасовийОрдер"
    
      
    public class ПрихіднийКасовийОрдер_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Валюта = "";
        string Каса = "";
        string КасаВідправник = "";
        string Контрагент = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, Каса, КасаВідправник, Контрагент, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* КасаВідправник */
            , typeof(string) /* Контрагент */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Каса відправник", new CellRendererText() { Xpad = 4 }, "text", 9)); /*КасаВідправник*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 10)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ПрихіднийКасовийОрдер_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ПрихіднийКасовийОрдер_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПрихіднийКасовийОрдер_Select ПрихіднийКасовийОрдер_Select = new Документи.ПрихіднийКасовийОрдер_Select();
            ПрихіднийКасовийОрдер_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ПрихіднийКасовийОрдер_Const.Назва /* 1 */
                    , Документи.ПрихіднийКасовийОрдер_Const.НомерДок /* 2 */
                    , Документи.ПрихіднийКасовийОрдер_Const.ДатаДок /* 3 */
                    , Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу /* 4 */
                    , Документи.ПрихіднийКасовийОрдер_Const.Коментар /* 5 */
                    
                });

            /* Where */
            ПрихіднийКасовийОрдер_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПрихіднийКасовийОрдер_Select.QuerySelect.Order.Add(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Організація, ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Валюта, ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Валюти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Каса, ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Каси_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.КасаВідправник, ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Контрагент, ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Контрагенти_Const.Назва, "join_tab_5_field_1"));
                  

            /* SELECT */
            ПрихіднийКасовийОрдер_Select.Select();
            while (ПрихіднийКасовийОрдер_Select.MoveNext())
            {
                Документи.ПрихіднийКасовийОрдер_Pointer? cur = ПрихіднийКасовийОрдер_Select.Current;

                if (cur != null)
                {
                    ПрихіднийКасовийОрдер_Записи Record = new ПрихіднийКасовийОрдер_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        КасаВідправник = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПрихіднийКасовийОрдер_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПрихіднийКасовийОрдер_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПрихіднийКасовийОрдер_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПрихіднийКасовийОрдер_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПрихіднийКасовийОрдер_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "РозхіднийКасовийОрдер"
    
      
    public class РозхіднийКасовийОрдер_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Контрагент = "";
        string Валюта = "";
        string Каса = "";
        string КасаОтримувач = "";
        string СумаДокументу = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, КасаОтримувач, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* КасаОтримувач */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Каса отримувач", new CellRendererText() { Xpad = 4 }, "text", 10)); /*КасаОтримувач*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.РозхіднийКасовийОрдер_Const.ДатаДок, типПеріоду);
        }

        public static Документи.РозхіднийКасовийОрдер_Pointer? DocumentPointerItem { get; set; }
        public static Документи.РозхіднийКасовийОрдер_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.РозхіднийКасовийОрдер_Select РозхіднийКасовийОрдер_Select = new Документи.РозхіднийКасовийОрдер_Select();
            РозхіднийКасовийОрдер_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.РозхіднийКасовийОрдер_Const.Назва /* 1 */
                    , Документи.РозхіднийКасовийОрдер_Const.НомерДок /* 2 */
                    , Документи.РозхіднийКасовийОрдер_Const.ДатаДок /* 3 */
                    , Документи.РозхіднийКасовийОрдер_Const.СумаДокументу /* 4 */
                    , Документи.РозхіднийКасовийОрдер_Const.Коментар /* 5 */
                    
                });

            /* Where */
            РозхіднийКасовийОрдер_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              РозхіднийКасовийОрдер_Select.QuerySelect.Order.Add(Документи.РозхіднийКасовийОрдер_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                РозхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Організація, РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                РозхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Контрагент, РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                РозхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Валюта, РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Валюти_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                РозхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Каса, РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                РозхіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.КасаОтримувач, РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Каси_Const.Назва, "join_tab_5_field_1"));
                  

            /* SELECT */
            РозхіднийКасовийОрдер_Select.Select();
            while (РозхіднийКасовийОрдер_Select.MoveNext())
            {
                Документи.РозхіднийКасовийОрдер_Pointer? cur = РозхіднийКасовийОрдер_Select.Current;

                if (cur != null)
                {
                    РозхіднийКасовийОрдер_Записи Record = new РозхіднийКасовийОрдер_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        КасаОтримувач = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[РозхіднийКасовийОрдер_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[РозхіднийКасовийОрдер_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[РозхіднийКасовийОрдер_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[РозхіднийКасовийОрдер_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[РозхіднийКасовийОрдер_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПереміщенняТоварів"
    
      
    public class ПереміщенняТоварів_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string СкладВідправник = "";
        string СкладОтримувач = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, СкладВідправник, СкладОтримувач, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* СкладВідправник */
            , typeof(string) /* СкладОтримувач */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад відправник", new CellRendererText() { Xpad = 4 }, "text", 7)); /*СкладВідправник*/
            treeView.AppendColumn(new TreeViewColumn("Склад отримувач", new CellRendererText() { Xpad = 4 }, "text", 8)); /*СкладОтримувач*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПереміщенняТоварів_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ПереміщенняТоварів_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ПереміщенняТоварів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПереміщенняТоварів_Select ПереміщенняТоварів_Select = new Документи.ПереміщенняТоварів_Select();
            ПереміщенняТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ПереміщенняТоварів_Const.Назва /* 1 */
                    , Документи.ПереміщенняТоварів_Const.НомерДок /* 2 */
                    , Документи.ПереміщенняТоварів_Const.ДатаДок /* 3 */
                    , Документи.ПереміщенняТоварів_Const.Коментар /* 4 */
                    
                });

            /* Where */
            ПереміщенняТоварів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПереміщенняТоварів_Select.QuerySelect.Order.Add(Документи.ПереміщенняТоварів_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПереміщенняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПереміщенняТоварів_Const.Організація, ПереміщенняТоварів_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПереміщенняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПереміщенняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварів_Const.СкладВідправник, ПереміщенняТоварів_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПереміщенняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ПереміщенняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварів_Const.СкладОтримувач, ПереміщенняТоварів_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ПереміщенняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "join_tab_3_field_1"));
                  

            /* SELECT */
            ПереміщенняТоварів_Select.Select();
            while (ПереміщенняТоварів_Select.MoveNext())
            {
                Документи.ПереміщенняТоварів_Pointer? cur = ПереміщенняТоварів_Select.Current;

                if (cur != null)
                {
                    ПереміщенняТоварів_Записи Record = new ПереміщенняТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        СкладВідправник = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        СкладОтримувач = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПереміщенняТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПереміщенняТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПереміщенняТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПереміщенняТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПоверненняТоварівПостачальнику"
    
      
    public class ПоверненняТоварівПостачальнику_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ПоверненняТоварівПостачальнику_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ПоверненняТоварівПостачальнику_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПоверненняТоварівПостачальнику_Select ПоверненняТоварівПостачальнику_Select = new Документи.ПоверненняТоварівПостачальнику_Select();
            ПоверненняТоварівПостачальнику_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ПоверненняТоварівПостачальнику_Const.Назва /* 1 */
                    , Документи.ПоверненняТоварівПостачальнику_Const.НомерДок /* 2 */
                    , Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок /* 3 */
                    , Документи.ПоверненняТоварівПостачальнику_Const.СумаДокументу /* 4 */
                    , Документи.ПоверненняТоварівПостачальнику_Const.Коментар /* 5 */
                    
                });

            /* Where */
            ПоверненняТоварівПостачальнику_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПоверненняТоварівПостачальнику_Select.QuerySelect.Order.Add(Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Організація, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Контрагент, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Валюта, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Валюти_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Каса, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Склад, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Склади_Const.Назва, "join_tab_5_field_1"));
                  

            /* SELECT */
            ПоверненняТоварівПостачальнику_Select.Select();
            while (ПоверненняТоварівПостачальнику_Select.MoveNext())
            {
                Документи.ПоверненняТоварівПостачальнику_Pointer? cur = ПоверненняТоварівПостачальнику_Select.Current;

                if (cur != null)
                {
                    ПоверненняТоварівПостачальнику_Записи Record = new ПоверненняТоварівПостачальнику_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПоверненняТоварівПостачальнику_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПоверненняТоварівПостачальнику_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПоверненняТоварівПостачальнику_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПоверненняТоварівПостачальнику_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПоверненняТоварівПостачальнику_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПоверненняТоварівВідКлієнта"
    
      
    public class ПоверненняТоварівВідКлієнта_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, Каса, Контрагент, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ПоверненняТоварівВідКлієнта_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ПоверненняТоварівВідКлієнта_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПоверненняТоварівВідКлієнта_Select ПоверненняТоварівВідКлієнта_Select = new Документи.ПоверненняТоварівВідКлієнта_Select();
            ПоверненняТоварівВідКлієнта_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ПоверненняТоварівВідКлієнта_Const.Назва /* 1 */
                    , Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок /* 2 */
                    , Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок /* 3 */
                    , Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу /* 4 */
                    , Документи.ПоверненняТоварівВідКлієнта_Const.Коментар /* 5 */
                    
                });

            /* Where */
            ПоверненняТоварівВідКлієнта_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПоверненняТоварівВідКлієнта_Select.QuerySelect.Order.Add(Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Організація, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Валюта, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Валюти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Каса, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Каси_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Контрагент, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Контрагенти_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Склад, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Склади_Const.Назва, "join_tab_5_field_1"));
                  

            /* SELECT */
            ПоверненняТоварівВідКлієнта_Select.Select();
            while (ПоверненняТоварівВідКлієнта_Select.MoveNext())
            {
                Документи.ПоверненняТоварівВідКлієнта_Pointer? cur = ПоверненняТоварівВідКлієнта_Select.Current;

                if (cur != null)
                {
                    ПоверненняТоварівВідКлієнта_Записи Record = new ПоверненняТоварівВідКлієнта_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "АктВиконанихРобіт"
    
      
    public class АктВиконанихРобіт_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, Каса, Контрагент, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 10)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 11)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.АктВиконанихРобіт_Const.ДатаДок, типПеріоду);
        }

        public static Документи.АктВиконанихРобіт_Pointer? DocumentPointerItem { get; set; }
        public static Документи.АктВиконанихРобіт_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.АктВиконанихРобіт_Select АктВиконанихРобіт_Select = new Документи.АктВиконанихРобіт_Select();
            АктВиконанихРобіт_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.АктВиконанихРобіт_Const.Назва /* 1 */
                    , Документи.АктВиконанихРобіт_Const.НомерДок /* 2 */
                    , Документи.АктВиконанихРобіт_Const.ДатаДок /* 3 */
                    , Документи.АктВиконанихРобіт_Const.СумаДокументу /* 4 */
                    , Документи.АктВиконанихРобіт_Const.Коментар /* 5 */
                    
                });

            /* Where */
            АктВиконанихРобіт_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              АктВиконанихРобіт_Select.QuerySelect.Order.Add(Документи.АктВиконанихРобіт_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                АктВиконанихРобіт_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.АктВиконанихРобіт_Const.Організація, АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  АктВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                АктВиконанихРобіт_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.АктВиконанихРобіт_Const.Валюта, АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  АктВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Валюти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                АктВиконанихРобіт_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.АктВиконанихРобіт_Const.Каса, АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  АктВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Каси_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                АктВиконанихРобіт_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.АктВиконанихРобіт_Const.Контрагент, АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  АктВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Контрагенти_Const.Назва, "join_tab_4_field_1"));
                  

            /* SELECT */
            АктВиконанихРобіт_Select.Select();
            while (АктВиконанихРобіт_Select.MoveNext())
            {
                Документи.АктВиконанихРобіт_Pointer? cur = АктВиконанихРобіт_Select.Current;

                if (cur != null)
                {
                    АктВиконанихРобіт_Записи Record = new АктВиконанихРобіт_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[АктВиконанихРобіт_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[АктВиконанихРобіт_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[АктВиконанихРобіт_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[АктВиконанихРобіт_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[АктВиконанихРобіт_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ВведенняЗалишків"
    
      
    public class ВведенняЗалишків_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Контрагент, Валюта, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("НомерДок", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("ДатаДок", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 10)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ВведенняЗалишків_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ВведенняЗалишків_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ВведенняЗалишків_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ВведенняЗалишків_Select ВведенняЗалишків_Select = new Документи.ВведенняЗалишків_Select();
            ВведенняЗалишків_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ВведенняЗалишків_Const.Назва /* 1 */
                    , Документи.ВведенняЗалишків_Const.НомерДок /* 2 */
                    , Документи.ВведенняЗалишків_Const.ДатаДок /* 3 */
                    , Документи.ВведенняЗалишків_Const.Коментар /* 4 */
                    
                });

            /* Where */
            ВведенняЗалишків_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВведенняЗалишків_Select.QuerySelect.Order.Add(Документи.ВведенняЗалишків_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ВведенняЗалишків_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ВведенняЗалишків_Const.Організація, ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ВведенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ВведенняЗалишків_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ВведенняЗалишків_Const.Склад, ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ВведенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ВведенняЗалишків_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ВведенняЗалишків_Const.Контрагент, ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ВведенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Контрагенти_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                ВведенняЗалишків_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ВведенняЗалишків_Const.Валюта, ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ВведенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Валюти_Const.Назва, "join_tab_4_field_1"));
                  

            /* SELECT */
            ВведенняЗалишків_Select.Select();
            while (ВведенняЗалишків_Select.MoveNext())
            {
                Документи.ВведенняЗалишків_Pointer? cur = ВведенняЗалишків_Select.Current;

                if (cur != null)
                {
                    ВведенняЗалишків_Записи Record = new ВведенняЗалишків_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВведенняЗалишків_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ВведенняЗалишків_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ВведенняЗалишків_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ВведенняЗалишків_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "НадлишкиТоварів"
    
      
    public class НадлишкиТоварів_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.НадлишкиТоварів_Const.ДатаДок, типПеріоду);
        }

        public static Документи.НадлишкиТоварів_Pointer? DocumentPointerItem { get; set; }
        public static Документи.НадлишкиТоварів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.НадлишкиТоварів_Select НадлишкиТоварів_Select = new Документи.НадлишкиТоварів_Select();
            НадлишкиТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.НадлишкиТоварів_Const.Назва /* 1 */
                    , Документи.НадлишкиТоварів_Const.НомерДок /* 2 */
                    , Документи.НадлишкиТоварів_Const.ДатаДок /* 3 */
                    , Документи.НадлишкиТоварів_Const.Коментар /* 4 */
                    
                });

            /* Where */
            НадлишкиТоварів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              НадлишкиТоварів_Select.QuerySelect.Order.Add(Документи.НадлишкиТоварів_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                НадлишкиТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.НадлишкиТоварів_Const.Організація, НадлишкиТоварів_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  НадлишкиТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                НадлишкиТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.НадлишкиТоварів_Const.Склад, НадлишкиТоварів_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  НадлишкиТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "join_tab_2_field_1"));
                  

            /* SELECT */
            НадлишкиТоварів_Select.Select();
            while (НадлишкиТоварів_Select.MoveNext())
            {
                Документи.НадлишкиТоварів_Pointer? cur = НадлишкиТоварів_Select.Current;

                if (cur != null)
                {
                    НадлишкиТоварів_Записи Record = new НадлишкиТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[НадлишкиТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[НадлишкиТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[НадлишкиТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[НадлишкиТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПересортицяТоварів"
    
      
    public class ПересортицяТоварів_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("НомерДок", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("ДатаДок", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПересортицяТоварів_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ПересортицяТоварів_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ПересортицяТоварів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПересортицяТоварів_Select ПересортицяТоварів_Select = new Документи.ПересортицяТоварів_Select();
            ПересортицяТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ПересортицяТоварів_Const.Назва /* 1 */
                    , Документи.ПересортицяТоварів_Const.НомерДок /* 2 */
                    , Документи.ПересортицяТоварів_Const.ДатаДок /* 3 */
                    , Документи.ПересортицяТоварів_Const.Коментар /* 4 */
                    
                });

            /* Where */
            ПересортицяТоварів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПересортицяТоварів_Select.QuerySelect.Order.Add(Документи.ПересортицяТоварів_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПересортицяТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПересортицяТоварів_Const.Організація, ПересортицяТоварів_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПересортицяТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПересортицяТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПересортицяТоварів_Const.Склад, ПересортицяТоварів_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПересортицяТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "join_tab_2_field_1"));
                  

            /* SELECT */
            ПересортицяТоварів_Select.Select();
            while (ПересортицяТоварів_Select.MoveNext())
            {
                Документи.ПересортицяТоварів_Pointer? cur = ПересортицяТоварів_Select.Current;

                if (cur != null)
                {
                    ПересортицяТоварів_Записи Record = new ПересортицяТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПересортицяТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПересортицяТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПересортицяТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПересортицяТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПерерахунокТоварів"
    
      
    public class ПерерахунокТоварів_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Склад = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Склад, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Склад */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПерерахунокТоварів_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ПерерахунокТоварів_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ПерерахунокТоварів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПерерахунокТоварів_Select ПерерахунокТоварів_Select = new Документи.ПерерахунокТоварів_Select();
            ПерерахунокТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ПерерахунокТоварів_Const.Назва /* 1 */
                    , Документи.ПерерахунокТоварів_Const.НомерДок /* 2 */
                    , Документи.ПерерахунокТоварів_Const.ДатаДок /* 3 */
                    , Документи.ПерерахунокТоварів_Const.Коментар /* 4 */
                    
                });

            /* Where */
            ПерерахунокТоварів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПерерахунокТоварів_Select.QuerySelect.Order.Add(Документи.ПерерахунокТоварів_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПерерахунокТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПерерахунокТоварів_Const.Склад, ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПерерахунокТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Склади_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            ПерерахунокТоварів_Select.Select();
            while (ПерерахунокТоварів_Select.MoveNext())
            {
                Документи.ПерерахунокТоварів_Pointer? cur = ПерерахунокТоварів_Select.Current;

                if (cur != null)
                {
                    ПерерахунокТоварів_Записи Record = new ПерерахунокТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Склад = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПерерахунокТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПерерахунокТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПерерахунокТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПерерахунокТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПсуванняТоварів"
    
      
    public class ПсуванняТоварів_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 8)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПсуванняТоварів_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ПсуванняТоварів_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ПсуванняТоварів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ПсуванняТоварів_Select ПсуванняТоварів_Select = new Документи.ПсуванняТоварів_Select();
            ПсуванняТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ПсуванняТоварів_Const.Назва /* 1 */
                    , Документи.ПсуванняТоварів_Const.НомерДок /* 2 */
                    , Документи.ПсуванняТоварів_Const.ДатаДок /* 3 */
                    , Документи.ПсуванняТоварів_Const.СумаДокументу /* 4 */
                    , Документи.ПсуванняТоварів_Const.Коментар /* 5 */
                    
                });

            /* Where */
            ПсуванняТоварів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПсуванняТоварів_Select.QuerySelect.Order.Add(Документи.ПсуванняТоварів_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПсуванняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПсуванняТоварів_Const.Організація, ПсуванняТоварів_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПсуванняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПсуванняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПсуванняТоварів_Const.Склад, ПсуванняТоварів_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПсуванняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "join_tab_2_field_1"));
                  

            /* SELECT */
            ПсуванняТоварів_Select.Select();
            while (ПсуванняТоварів_Select.MoveNext())
            {
                Документи.ПсуванняТоварів_Pointer? cur = ПсуванняТоварів_Select.Current;

                if (cur != null)
                {
                    ПсуванняТоварів_Записи Record = new ПсуванняТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПсуванняТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПсуванняТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПсуванняТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПсуванняТоварів_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПсуванняТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ВнутрішнєСпоживанняТоварів"
    
      
    public class ВнутрішнєСпоживанняТоварів_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Валюта, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { FixedWidth = 160 } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 9)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 10)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, типПеріоду);
        }

        public static Документи.ВнутрішнєСпоживанняТоварів_Pointer? DocumentPointerItem { get; set; }
        public static Документи.ВнутрішнєСпоживанняТоварів_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.ВнутрішнєСпоживанняТоварів_Select ВнутрішнєСпоживанняТоварів_Select = new Документи.ВнутрішнєСпоживанняТоварів_Select();
            ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.ВнутрішнєСпоживанняТоварів_Const.Назва /* 1 */
                    , Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок /* 2 */
                    , Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок /* 3 */
                    , Документи.ВнутрішнєСпоживанняТоварів_Const.СумаДокументу /* 4 */
                    , Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар /* 5 */
                    
                });

            /* Where */
            ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Order.Add(Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Організація, ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Склад, ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Валюта, ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Валюти_Const.Назва, "join_tab_3_field_1"));
                  

            /* SELECT */
            ВнутрішнєСпоживанняТоварів_Select.Select();
            while (ВнутрішнєСпоживанняТоварів_Select.MoveNext())
            {
                Документи.ВнутрішнєСпоживанняТоварів_Pointer? cur = ВнутрішнєСпоживанняТоварів_Select.Current;

                if (cur != null)
                {
                    ВнутрішнєСпоживанняТоварів_Записи Record = new ВнутрішнєСпоживанняТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "РахунокФактура"
    
      
    public class РахунокФактура_Записи
    {
        string Image = "doc.png";
        bool Spend = false;
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
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0)); /*Image*/
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3)); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4)); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5)); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6)); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7)); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8)); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9)); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10)); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11)); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12)); /*Коментар*/
            
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.РахунокФактура_Const.ДатаДок, типПеріоду);
        }

        public static Документи.РахунокФактура_Pointer? DocumentPointerItem { get; set; }
        public static Документи.РахунокФактура_Pointer? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();

            Документи.РахунокФактура_Select РахунокФактура_Select = new Документи.РахунокФактура_Select();
            РахунокФактура_Select.QuerySelect.Field.AddRange(
                new string[]
                { "spend" /*Проведений документ*/
                    , Документи.РахунокФактура_Const.Назва /* 1 */
                    , Документи.РахунокФактура_Const.НомерДок /* 2 */
                    , Документи.РахунокФактура_Const.ДатаДок /* 3 */
                    , Документи.РахунокФактура_Const.СумаДокументу /* 4 */
                    , Документи.РахунокФактура_Const.Коментар /* 5 */
                    
                });

            /* Where */
            РахунокФактура_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              РахунокФактура_Select.QuerySelect.Order.Add(Документи.РахунокФактура_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.РахунокФактура_Const.Організація, РахунокФактура_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.РахунокФактура_Const.Контрагент, РахунокФактура_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Документи.РахунокФактура_Const.Валюта, РахунокФактура_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Валюти_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Каси_Const.TABLE, Документи.РахунокФактура_Const.Каса, РахунокФактура_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.РахунокФактура_Const.Склад, РахунокФактура_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Склади_Const.Назва, "join_tab_5_field_1"));
                  

            /* SELECT */
            РахунокФактура_Select.Select();
            while (РахунокФактура_Select.MoveNext())
            {
                Документи.РахунокФактура_Pointer? cur = РахунокФактура_Select.Current;

                if (cur != null)
                {
                    РахунокФактура_Записи Record = new РахунокФактура_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[РахунокФактура_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[РахунокФактура_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[РахунокФактура_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[РахунокФактура_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[РахунокФактура_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DocumentPointerItem!.UnigueID.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
}

  