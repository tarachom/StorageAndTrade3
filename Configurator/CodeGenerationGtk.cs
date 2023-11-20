
/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
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
 * Дата конфігурації: 20.11.2023 16:10:18
 *
 *
 * Цей код згенерований в Конфігураторі 3. Шаблон Gtk.xslt
 *
 */

using Gtk;
using AccountingSoftware;

namespace StorageAndTrade_1_0.Довідники.ТабличніСписки
{
    
    #region DIRECTORY "Організації"
    
      
    /* ТАБЛИЦЯ */
    public class Організації_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Організації_Select Організації_Select = new Довідники.Організації_Select();
            Організації_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Організації_Const.Код /* 1 */
                    , Довідники.Організації_Const.Назва /* 2 */
                    
                });

            /* Where */
            Організації_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Організації_Select.QuerySelect.Order.Add(Довідники.Організації_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Організації_Select.Select();
            while (Організації_Select.MoveNext())
            {
                Довідники.Організації_Pointer? cur = Організації_Select.Current;

                if (cur != null)
                {
                    Організації_Записи Record = new Організації_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Організації_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Організації_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Організації_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Організації_Select Організації_Select = new Довідники.Організації_Select();
            Організації_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Організації_Const.Код /* 1 */
                    , Довідники.Організації_Const.Назва /* 2 */
                    
                });

            /* Where */
            Організації_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Організації_Select.QuerySelect.Order.Add(Довідники.Організації_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Організації_Select.Select();
            while (Організації_Select.MoveNext())
            {
                Довідники.Організації_Pointer? cur = Організації_Select.Current;

                if (cur != null)
                {
                    Організації_ЗаписиШвидкийВибір Record = new Організації_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Організації_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Організації_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Номенклатура"
    
      
    /* ТАБЛИЦЯ */
    public class Номенклатура_ЗаписиПрототип
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ОдиницяВиміру = "";
        string ТипНоменклатури = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Код, Назва, ОдиницяВиміру, ТипНоменклатури };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* ОдиницяВиміру */
            , typeof(string) /* ТипНоменклатури */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Пакування", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ОдиницяВиміру*/
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ТипНоменклатури*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Номенклатура_Select Номенклатура_Select = new Довідники.Номенклатура_Select();
            Номенклатура_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Номенклатура_Const.Код /* 1 */
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
                  

            /* SELECT */
            await Номенклатура_Select.Select();
            while (Номенклатура_Select.MoveNext())
            {
                Довідники.Номенклатура_Pointer? cur = Номенклатура_Select.Current;

                if (cur != null)
                {
                    Номенклатура_ЗаписиПрототип Record = new Номенклатура_ЗаписиПрототип
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        ОдиницяВиміру = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[Номенклатура_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Номенклатура_Const.Назва]?.ToString() ?? "", /**/
                        ТипНоменклатури = Перелічення.ПсевдонімиПерелічення.ТипиНоменклатури_Alias( ((Перелічення.ТипиНоменклатури)(cur.Fields?[Номенклатура_Const.ТипНоменклатури]! != DBNull.Value ? cur.Fields?[Номенклатура_Const.ТипНоменклатури]! : 0)) ) /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Номенклатура_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Номенклатура_Select Номенклатура_Select = new Довідники.Номенклатура_Select();
            Номенклатура_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Номенклатура_Const.Код /* 1 */
                    , Довідники.Номенклатура_Const.Назва /* 2 */
                    
                });

            /* Where */
            Номенклатура_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Номенклатура_Select.QuerySelect.Order.Add(Довідники.Номенклатура_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Номенклатура_Select.Select();
            while (Номенклатура_Select.MoveNext())
            {
                Довідники.Номенклатура_Pointer? cur = Номенклатура_Select.Current;

                if (cur != null)
                {
                    Номенклатура_ЗаписиШвидкийВибір Record = new Номенклатура_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Номенклатура_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Номенклатура_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Виробники"
    
      
    /* ТАБЛИЦЯ */
    public class Виробники_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Виробники_Select Виробники_Select = new Довідники.Виробники_Select();
            Виробники_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Виробники_Const.Код /* 1 */
                    , Довідники.Виробники_Const.Назва /* 2 */
                    
                });

            /* Where */
            Виробники_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Виробники_Select.QuerySelect.Order.Add(Довідники.Виробники_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Виробники_Select.Select();
            while (Виробники_Select.MoveNext())
            {
                Довідники.Виробники_Pointer? cur = Виробники_Select.Current;

                if (cur != null)
                {
                    Виробники_Записи Record = new Виробники_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Виробники_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Виробники_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Виробники_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Виробники_Select Виробники_Select = new Довідники.Виробники_Select();
            Виробники_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Виробники_Const.Код /* 1 */
                    , Довідники.Виробники_Const.Назва /* 2 */
                    
                });

            /* Where */
            Виробники_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Виробники_Select.QuerySelect.Order.Add(Довідники.Виробники_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Виробники_Select.Select();
            while (Виробники_Select.MoveNext())
            {
                Довідники.Виробники_Pointer? cur = Виробники_Select.Current;

                if (cur != null)
                {
                    Виробники_ЗаписиШвидкийВибір Record = new Виробники_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Виробники_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Виробники_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиНоменклатури"
    
      
    /* ТАБЛИЦЯ */
    public class ВидиНоменклатури_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиНоменклатури_Select ВидиНоменклатури_Select = new Довідники.ВидиНоменклатури_Select();
            ВидиНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ВидиНоменклатури_Const.Код /* 1 */
                    , Довідники.ВидиНоменклатури_Const.Назва /* 2 */
                    
                });

            /* Where */
            ВидиНоменклатури_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ВидиНоменклатури_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиНоменклатури_Select.Select();
            while (ВидиНоменклатури_Select.MoveNext())
            {
                Довідники.ВидиНоменклатури_Pointer? cur = ВидиНоменклатури_Select.Current;

                if (cur != null)
                {
                    ВидиНоменклатури_Записи Record = new ВидиНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[ВидиНоменклатури_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВидиНоменклатури_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ВидиНоменклатури_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиНоменклатури_Select ВидиНоменклатури_Select = new Довідники.ВидиНоменклатури_Select();
            ВидиНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ВидиНоменклатури_Const.Назва /* 1 */
                    
                });

            /* Where */
            ВидиНоменклатури_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ВидиНоменклатури_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиНоменклатури_Select.Select();
            while (ВидиНоменклатури_Select.MoveNext())
            {
                Довідники.ВидиНоменклатури_Pointer? cur = ВидиНоменклатури_Select.Current;

                if (cur != null)
                {
                    ВидиНоменклатури_ЗаписиШвидкийВибір Record = new ВидиНоменклатури_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[ВидиНоменклатури_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ПакуванняОдиниціВиміру"
    
      
    /* ТАБЛИЦЯ */
    public class ПакуванняОдиниціВиміру_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ПакуванняОдиниціВиміру_Select ПакуванняОдиниціВиміру_Select = new Довідники.ПакуванняОдиниціВиміру_Select();
            ПакуванняОдиниціВиміру_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ПакуванняОдиниціВиміру_Const.Код /* 1 */
                    , Довідники.ПакуванняОдиниціВиміру_Const.Назва /* 2 */
                    
                });

            /* Where */
            ПакуванняОдиниціВиміру_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПакуванняОдиниціВиміру_Select.QuerySelect.Order.Add(Довідники.ПакуванняОдиниціВиміру_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ПакуванняОдиниціВиміру_Select.Select();
            while (ПакуванняОдиниціВиміру_Select.MoveNext())
            {
                Довідники.ПакуванняОдиниціВиміру_Pointer? cur = ПакуванняОдиниціВиміру_Select.Current;

                if (cur != null)
                {
                    ПакуванняОдиниціВиміру_Записи Record = new ПакуванняОдиниціВиміру_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[ПакуванняОдиниціВиміру_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПакуванняОдиниціВиміру_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ПакуванняОдиниціВиміру_Select ПакуванняОдиниціВиміру_Select = new Довідники.ПакуванняОдиниціВиміру_Select();
            ПакуванняОдиниціВиміру_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ПакуванняОдиниціВиміру_Const.Код /* 1 */
                    , Довідники.ПакуванняОдиниціВиміру_Const.Назва /* 2 */
                    
                });

            /* Where */
            ПакуванняОдиниціВиміру_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПакуванняОдиниціВиміру_Select.QuerySelect.Order.Add(Довідники.ПакуванняОдиниціВиміру_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ПакуванняОдиниціВиміру_Select.Select();
            while (ПакуванняОдиниціВиміру_Select.MoveNext())
            {
                Довідники.ПакуванняОдиниціВиміру_Pointer? cur = ПакуванняОдиниціВиміру_Select.Current;

                if (cur != null)
                {
                    ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір Record = new ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[ПакуванняОдиниціВиміру_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПакуванняОдиниціВиміру_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Валюти"
    
      
    /* ТАБЛИЦЯ */
    public class Валюти_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string КороткаНазва = "";
        string Код_R030 = "";
        string ВиводитиКурсНаСтартову = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Код, Назва, КороткаНазва, Код_R030, ВиводитиКурсНаСтартову };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* КороткаНазва */
            , typeof(string) /* Код_R030 */
            , typeof(string) /* ВиводитиКурсНаСтартову */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Коротка назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*КороткаНазва*/
            treeView.AppendColumn(new TreeViewColumn("R030", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Код_R030*/
            treeView.AppendColumn(new TreeViewColumn("Показувати на стартовій", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ВиводитиКурсНаСтартову*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Валюти_Select Валюти_Select = new Довідники.Валюти_Select();
            Валюти_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Валюти_Const.Код /* 1 */
                    , Довідники.Валюти_Const.Назва /* 2 */
                    , Довідники.Валюти_Const.КороткаНазва /* 3 */
                    , Довідники.Валюти_Const.Код_R030 /* 4 */
                    , Довідники.Валюти_Const.ВиводитиКурсНаСтартову /* 5 */
                    
                });

            /* Where */
            Валюти_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Валюти_Select.QuerySelect.Order.Add(Довідники.Валюти_Const.Код, SelectOrder.ASC);
            

            /* SELECT */
            await Валюти_Select.Select();
            while (Валюти_Select.MoveNext())
            {
                Довідники.Валюти_Pointer? cur = Валюти_Select.Current;

                if (cur != null)
                {
                    Валюти_Записи Record = new Валюти_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Валюти_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Валюти_Const.Назва]?.ToString() ?? "", /**/
                        КороткаНазва = cur.Fields?[Валюти_Const.КороткаНазва]?.ToString() ?? "", /**/
                        Код_R030 = cur.Fields?[Валюти_Const.Код_R030]?.ToString() ?? "", /**/
                        ВиводитиКурсНаСтартову = (cur.Fields?[Валюти_Const.ВиводитиКурсНаСтартову]! != DBNull.Value ? (bool)cur.Fields?[Валюти_Const.ВиводитиКурсНаСтартову]! : false) ? "Так" : "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Валюти_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("КороткаНазва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*КороткаНазва*/
            treeView.AppendColumn(new TreeViewColumn("Код R030", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Код_R030*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Валюти_Select Валюти_Select = new Довідники.Валюти_Select();
            Валюти_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Валюти_Const.Код /* 1 */
                    , Довідники.Валюти_Const.Назва /* 2 */
                    , Довідники.Валюти_Const.КороткаНазва /* 3 */
                    , Довідники.Валюти_Const.Код_R030 /* 4 */
                    
                });

            /* Where */
            Валюти_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Валюти_Select.QuerySelect.Order.Add(Довідники.Валюти_Const.Код, SelectOrder.ASC);
            

            /* SELECT */
            await Валюти_Select.Select();
            while (Валюти_Select.MoveNext())
            {
                Довідники.Валюти_Pointer? cur = Валюти_Select.Current;

                if (cur != null)
                {
                    Валюти_ЗаписиШвидкийВибір Record = new Валюти_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Валюти_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Валюти_Const.Назва]?.ToString() ?? "", /**/
                        КороткаНазва = cur.Fields?[Валюти_Const.КороткаНазва]?.ToString() ?? "", /**/
                        Код_R030 = cur.Fields?[Валюти_Const.Код_R030]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Контрагенти"
    
      
    /* ТАБЛИЦЯ */
    public class Контрагенти_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Папка", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Папка*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Контрагенти_Select Контрагенти_Select = new Довідники.Контрагенти_Select();
            Контрагенти_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Контрагенти_Const.Код /* 1 */
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
            await Контрагенти_Select.Select();
            while (Контрагенти_Select.MoveNext())
            {
                Довідники.Контрагенти_Pointer? cur = Контрагенти_Select.Current;

                if (cur != null)
                {
                    Контрагенти_Записи Record = new Контрагенти_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Папка = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[Контрагенти_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Контрагенти_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Контрагенти_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Контрагенти_Select Контрагенти_Select = new Довідники.Контрагенти_Select();
            Контрагенти_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Контрагенти_Const.Код /* 1 */
                    , Довідники.Контрагенти_Const.Назва /* 2 */
                    
                });

            /* Where */
            Контрагенти_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Контрагенти_Select.QuerySelect.Order.Add(Довідники.Контрагенти_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Контрагенти_Select.Select();
            while (Контрагенти_Select.MoveNext())
            {
                Довідники.Контрагенти_Pointer? cur = Контрагенти_Select.Current;

                if (cur != null)
                {
                    Контрагенти_ЗаписиШвидкийВибір Record = new Контрагенти_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Контрагенти_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Контрагенти_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Склади"
    
      
    /* ТАБЛИЦЯ */
    public class Склади_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ТипСкладу = "";
        string НалаштуванняАдресногоЗберігання = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Код, Назва, ТипСкладу, НалаштуванняАдресногоЗберігання };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* ТипСкладу */
            , typeof(string) /* НалаштуванняАдресногоЗберігання */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Тип cкладу", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ТипСкладу*/
            treeView.AppendColumn(new TreeViewColumn("Адресне зберігання", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*НалаштуванняАдресногоЗберігання*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Склади_Select Склади_Select = new Довідники.Склади_Select();
            Склади_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Склади_Const.Код /* 1 */
                    , Довідники.Склади_Const.Назва /* 2 */
                    , Довідники.Склади_Const.ТипСкладу /* 3 */
                    , Довідники.Склади_Const.НалаштуванняАдресногоЗберігання /* 4 */
                    
                });

            /* Where */
            Склади_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Склади_Select.QuerySelect.Order.Add(Довідники.Склади_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Склади_Select.Select();
            while (Склади_Select.MoveNext())
            {
                Довідники.Склади_Pointer? cur = Склади_Select.Current;

                if (cur != null)
                {
                    Склади_Записи Record = new Склади_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Склади_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Склади_Const.Назва]?.ToString() ?? "", /**/
                        ТипСкладу = Перелічення.ПсевдонімиПерелічення.ТипиСкладів_Alias( ((Перелічення.ТипиСкладів)(cur.Fields?[Склади_Const.ТипСкладу]! != DBNull.Value ? cur.Fields?[Склади_Const.ТипСкладу]! : 0)) ), /**/
                        НалаштуванняАдресногоЗберігання = Перелічення.ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_Alias( ((Перелічення.НалаштуванняАдресногоЗберігання)(cur.Fields?[Склади_Const.НалаштуванняАдресногоЗберігання]! != DBNull.Value ? cur.Fields?[Склади_Const.НалаштуванняАдресногоЗберігання]! : 0)) ) /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Склади_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Склади_Select Склади_Select = new Довідники.Склади_Select();
            Склади_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Склади_Const.Код /* 1 */
                    , Довідники.Склади_Const.Назва /* 2 */
                    
                });

            /* Where */
            Склади_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Склади_Select.QuerySelect.Order.Add(Довідники.Склади_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Склади_Select.Select();
            while (Склади_Select.MoveNext())
            {
                Довідники.Склади_Pointer? cur = Склади_Select.Current;

                if (cur != null)
                {
                    Склади_ЗаписиШвидкийВибір Record = new Склади_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Склади_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Склади_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиЦін"
    
      
    /* ТАБЛИЦЯ */
    public class ВидиЦін_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЦін_Select ВидиЦін_Select = new Довідники.ВидиЦін_Select();
            ВидиЦін_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ВидиЦін_Const.Код /* 1 */
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
            await ВидиЦін_Select.Select();
            while (ВидиЦін_Select.MoveNext())
            {
                Довідники.ВидиЦін_Pointer? cur = ВидиЦін_Select.Current;

                if (cur != null)
                {
                    ВидиЦін_Записи Record = new ВидиЦін_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[ВидиЦін_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВидиЦін_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ВидиЦін_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЦін_Select ВидиЦін_Select = new Довідники.ВидиЦін_Select();
            ВидиЦін_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ВидиЦін_Const.Назва /* 1 */
                    
                });

            /* Where */
            ВидиЦін_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиЦін_Select.QuerySelect.Order.Add(Довідники.ВидиЦін_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЦін_Select.Select();
            while (ВидиЦін_Select.MoveNext())
            {
                Довідники.ВидиЦін_Pointer? cur = ВидиЦін_Select.Current;

                if (cur != null)
                {
                    ВидиЦін_ЗаписиШвидкийВибір Record = new ВидиЦін_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[ВидиЦін_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиЦінПостачальників"
    
      
    /* ТАБЛИЦЯ */
    public class ВидиЦінПостачальників_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЦінПостачальників_Select ВидиЦінПостачальників_Select = new Довідники.ВидиЦінПостачальників_Select();
            ВидиЦінПостачальників_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ВидиЦінПостачальників_Const.Код /* 1 */
                    , Довідники.ВидиЦінПостачальників_Const.Назва /* 2 */
                    
                });

            /* Where */
            ВидиЦінПостачальників_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиЦінПостачальників_Select.QuerySelect.Order.Add(Довідники.ВидиЦінПостачальників_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЦінПостачальників_Select.Select();
            while (ВидиЦінПостачальників_Select.MoveNext())
            {
                Довідники.ВидиЦінПостачальників_Pointer? cur = ВидиЦінПостачальників_Select.Current;

                if (cur != null)
                {
                    ВидиЦінПостачальників_Записи Record = new ВидиЦінПостачальників_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[ВидиЦінПостачальників_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВидиЦінПостачальників_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ВидиЦінПостачальників_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЦінПостачальників_Select ВидиЦінПостачальників_Select = new Довідники.ВидиЦінПостачальників_Select();
            ВидиЦінПостачальників_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ВидиЦінПостачальників_Const.Код /* 1 */
                    , Довідники.ВидиЦінПостачальників_Const.Назва /* 2 */
                    
                });

            /* Where */
            ВидиЦінПостачальників_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиЦінПостачальників_Select.QuerySelect.Order.Add(Довідники.ВидиЦінПостачальників_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЦінПостачальників_Select.Select();
            while (ВидиЦінПостачальників_Select.MoveNext())
            {
                Довідники.ВидиЦінПостачальників_Pointer? cur = ВидиЦінПостачальників_Select.Current;

                if (cur != null)
                {
                    ВидиЦінПостачальників_ЗаписиШвидкийВибір Record = new ВидиЦінПостачальників_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[ВидиЦінПостачальників_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВидиЦінПостачальників_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Користувачі"
    
      
    /* ТАБЛИЦЯ */
    public class Користувачі_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Користувачі_Select Користувачі_Select = new Довідники.Користувачі_Select();
            Користувачі_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Користувачі_Const.Код /* 1 */
                    , Довідники.Користувачі_Const.Назва /* 2 */
                    
                });

            /* Where */
            Користувачі_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Користувачі_Select.QuerySelect.Order.Add(Довідники.Користувачі_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Користувачі_Select.Select();
            while (Користувачі_Select.MoveNext())
            {
                Довідники.Користувачі_Pointer? cur = Користувачі_Select.Current;

                if (cur != null)
                {
                    Користувачі_Записи Record = new Користувачі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Користувачі_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Користувачі_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Користувачі_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Користувачі_Select Користувачі_Select = new Довідники.Користувачі_Select();
            Користувачі_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Користувачі_Const.Назва /* 1 */
                    
                });

            /* Where */
            Користувачі_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Користувачі_Select.QuerySelect.Order.Add(Довідники.Користувачі_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Користувачі_Select.Select();
            while (Користувачі_Select.MoveNext())
            {
                Довідники.Користувачі_Pointer? cur = Користувачі_Select.Current;

                if (cur != null)
                {
                    Користувачі_ЗаписиШвидкийВибір Record = new Користувачі_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[Користувачі_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ФізичніОсоби"
    
      
    /* ТАБЛИЦЯ */
    public class ФізичніОсоби_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ФізичніОсоби_Select ФізичніОсоби_Select = new Довідники.ФізичніОсоби_Select();
            ФізичніОсоби_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ФізичніОсоби_Const.Код /* 1 */
                    , Довідники.ФізичніОсоби_Const.Назва /* 2 */
                    
                });

            /* Where */
            ФізичніОсоби_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ФізичніОсоби_Select.QuerySelect.Order.Add(Довідники.ФізичніОсоби_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ФізичніОсоби_Select.Select();
            while (ФізичніОсоби_Select.MoveNext())
            {
                Довідники.ФізичніОсоби_Pointer? cur = ФізичніОсоби_Select.Current;

                if (cur != null)
                {
                    ФізичніОсоби_Записи Record = new ФізичніОсоби_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[ФізичніОсоби_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ФізичніОсоби_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ФізичніОсоби_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ФізичніОсоби_Select ФізичніОсоби_Select = new Довідники.ФізичніОсоби_Select();
            ФізичніОсоби_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ФізичніОсоби_Const.Назва /* 1 */
                    
                });

            /* Where */
            ФізичніОсоби_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ФізичніОсоби_Select.QuerySelect.Order.Add(Довідники.ФізичніОсоби_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ФізичніОсоби_Select.Select();
            while (ФізичніОсоби_Select.MoveNext())
            {
                Довідники.ФізичніОсоби_Pointer? cur = ФізичніОсоби_Select.Current;

                if (cur != null)
                {
                    ФізичніОсоби_ЗаписиШвидкийВибір Record = new ФізичніОсоби_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[ФізичніОсоби_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СтруктураПідприємства"
    
      
    /* ТАБЛИЦЯ */
    public class СтруктураПідприємства_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СтруктураПідприємства_Select СтруктураПідприємства_Select = new Довідники.СтруктураПідприємства_Select();
            СтруктураПідприємства_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СтруктураПідприємства_Const.Код /* 1 */
                    , Довідники.СтруктураПідприємства_Const.Назва /* 2 */
                    
                });

            /* Where */
            СтруктураПідприємства_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СтруктураПідприємства_Select.QuerySelect.Order.Add(Довідники.СтруктураПідприємства_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await СтруктураПідприємства_Select.Select();
            while (СтруктураПідприємства_Select.MoveNext())
            {
                Довідники.СтруктураПідприємства_Pointer? cur = СтруктураПідприємства_Select.Current;

                if (cur != null)
                {
                    СтруктураПідприємства_Записи Record = new СтруктураПідприємства_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[СтруктураПідприємства_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[СтруктураПідприємства_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class СтруктураПідприємства_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СтруктураПідприємства_Select СтруктураПідприємства_Select = new Довідники.СтруктураПідприємства_Select();
            СтруктураПідприємства_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СтруктураПідприємства_Const.Назва /* 1 */
                    
                });

            /* Where */
            СтруктураПідприємства_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СтруктураПідприємства_Select.QuerySelect.Order.Add(Довідники.СтруктураПідприємства_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await СтруктураПідприємства_Select.Select();
            while (СтруктураПідприємства_Select.MoveNext())
            {
                Довідники.СтруктураПідприємства_Pointer? cur = СтруктураПідприємства_Select.Current;

                if (cur != null)
                {
                    СтруктураПідприємства_ЗаписиШвидкийВибір Record = new СтруктураПідприємства_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[СтруктураПідприємства_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "КраїниСвіту"
    
      
    /* ТАБЛИЦЯ */
    public class КраїниСвіту_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.КраїниСвіту_Select КраїниСвіту_Select = new Довідники.КраїниСвіту_Select();
            КраїниСвіту_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.КраїниСвіту_Const.Код /* 1 */
                    , Довідники.КраїниСвіту_Const.Назва /* 2 */
                    
                });

            /* Where */
            КраїниСвіту_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              КраїниСвіту_Select.QuerySelect.Order.Add(Довідники.КраїниСвіту_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await КраїниСвіту_Select.Select();
            while (КраїниСвіту_Select.MoveNext())
            {
                Довідники.КраїниСвіту_Pointer? cur = КраїниСвіту_Select.Current;

                if (cur != null)
                {
                    КраїниСвіту_Записи Record = new КраїниСвіту_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[КраїниСвіту_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[КраїниСвіту_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class КраїниСвіту_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.КраїниСвіту_Select КраїниСвіту_Select = new Довідники.КраїниСвіту_Select();
            КраїниСвіту_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.КраїниСвіту_Const.Код /* 1 */
                    , Довідники.КраїниСвіту_Const.Назва /* 2 */
                    
                });

            /* Where */
            КраїниСвіту_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              КраїниСвіту_Select.QuerySelect.Order.Add(Довідники.КраїниСвіту_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await КраїниСвіту_Select.Select();
            while (КраїниСвіту_Select.MoveNext())
            {
                Довідники.КраїниСвіту_Pointer? cur = КраїниСвіту_Select.Current;

                if (cur != null)
                {
                    КраїниСвіту_ЗаписиШвидкийВибір Record = new КраїниСвіту_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[КраїниСвіту_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[КраїниСвіту_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Файли"
    
      
    /* ТАБЛИЦЯ */
    public class Файли_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string НазваФайлу = "";
        string Розмір = "";
        string ДатаСтворення = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Код, Назва, НазваФайлу, Розмір, ДатаСтворення };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* НазваФайлу */
            , typeof(string) /* Розмір */
            , typeof(string) /* ДатаСтворення */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Назва файлу", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*НазваФайлу*/
            treeView.AppendColumn(new TreeViewColumn("Розмір", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Розмір*/
            treeView.AppendColumn(new TreeViewColumn("Дата створення", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ДатаСтворення*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Файли_Select Файли_Select = new Довідники.Файли_Select();
            Файли_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Файли_Const.Код /* 1 */
                    , Довідники.Файли_Const.Назва /* 2 */
                    , Довідники.Файли_Const.НазваФайлу /* 3 */
                    , Довідники.Файли_Const.Розмір /* 4 */
                    , Довідники.Файли_Const.ДатаСтворення /* 5 */
                    
                });

            /* Where */
            Файли_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Файли_Select.QuerySelect.Order.Add(Довідники.Файли_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Файли_Select.Select();
            while (Файли_Select.MoveNext())
            {
                Довідники.Файли_Pointer? cur = Файли_Select.Current;

                if (cur != null)
                {
                    Файли_Записи Record = new Файли_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Файли_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Файли_Const.Назва]?.ToString() ?? "", /**/
                        НазваФайлу = cur.Fields?[Файли_Const.НазваФайлу]?.ToString() ?? "", /**/
                        Розмір = cur.Fields?[Файли_Const.Розмір]?.ToString() ?? "", /**/
                        ДатаСтворення = cur.Fields?[Файли_Const.ДатаСтворення]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Файли_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Файли_Select Файли_Select = new Довідники.Файли_Select();
            Файли_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Файли_Const.Назва /* 1 */
                    
                });

            /* Where */
            Файли_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Файли_Select.QuerySelect.Order.Add(Довідники.Файли_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Файли_Select.Select();
            while (Файли_Select.MoveNext())
            {
                Довідники.Файли_Pointer? cur = Файли_Select.Current;

                if (cur != null)
                {
                    Файли_ЗаписиШвидкийВибір Record = new Файли_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[Файли_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ХарактеристикиНоменклатури"
    
      
    /* ТАБЛИЦЯ */
    public class ХарактеристикиНоменклатури_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Номенклатура = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Код, Номенклатура, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Номенклатура */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ХарактеристикиНоменклатури_Select ХарактеристикиНоменклатури_Select = new Довідники.ХарактеристикиНоменклатури_Select();
            ХарактеристикиНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ХарактеристикиНоменклатури_Const.Код /* 1 */
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
            await ХарактеристикиНоменклатури_Select.Select();
            while (ХарактеристикиНоменклатури_Select.MoveNext())
            {
                Довідники.ХарактеристикиНоменклатури_Pointer? cur = ХарактеристикиНоменклатури_Select.Current;

                if (cur != null)
                {
                    ХарактеристикиНоменклатури_Записи Record = new ХарактеристикиНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Номенклатура = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[ХарактеристикиНоменклатури_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ХарактеристикиНоменклатури_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ХарактеристикиНоменклатури_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Номенклатура = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Код, Номенклатура, Назва };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Номенклатура */
            , typeof(string) /* Назва */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ХарактеристикиНоменклатури_Select ХарактеристикиНоменклатури_Select = new Довідники.ХарактеристикиНоменклатури_Select();
            ХарактеристикиНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ХарактеристикиНоменклатури_Const.Код /* 1 */
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
            await ХарактеристикиНоменклатури_Select.Select();
            while (ХарактеристикиНоменклатури_Select.MoveNext())
            {
                Довідники.ХарактеристикиНоменклатури_Pointer? cur = ХарактеристикиНоменклатури_Select.Current;

                if (cur != null)
                {
                    ХарактеристикиНоменклатури_ЗаписиШвидкийВибір Record = new ХарактеристикиНоменклатури_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Номенклатура = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[ХарактеристикиНоменклатури_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ХарактеристикиНоменклатури_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Номенклатура_Папки"
    
      
    /* ДЕРЕВО */
    public class Номенклатура_Папки_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc.png" : "folder.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Назва };
        }

        public static TreeStore Store = new TreeStore
        (
            typeof(Gdk.Pixbuf) /* Image */, 
            typeof(string)     /* ID */, 
            typeof(string)     /* Назва */
        );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static void LoadTree(UnigueID? openFolder, UnigueID? selectPointer
        )
        {
            Store.Clear();
            RootPath = SelectPath = null;

            Номенклатура_Папки_Записи rootRecord = new Номенклатура_Папки_Записи
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Номенклатура_Папки_Const.Назва}, 
        {Номенклатура_Папки_Const.Родич}, 
        1 AS level,
        deletion_label
    FROM {Номенклатура_Папки_Const.TABLE}
    WHERE {Номенклатура_Папки_Const.Родич} = '{Guid.Empty}'";

        

            if (openFolder != null) query += $@"
        AND uid != '{openFolder}'";

            query += $@"
    UNION ALL
    SELECT 
        {Номенклатура_Папки_Const.TABLE}.uid, 
        {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Назва}, 
        {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич}, 
        r.level + 1 AS level,
        {Номенклатура_Папки_Const.TABLE}.deletion_label 
    FROM {Номенклатура_Папки_Const.TABLE}
        JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid";

        

            if (openFolder != null) query += $@"
    WHERE {Номенклатура_Папки_Const.TABLE}.uid != '{openFolder}'";

            query += $@"
)
SELECT 
    uid, 
    {Номенклатура_Папки_Const.Назва}, 
    {Номенклатура_Папки_Const.Родич}, 
    level,
    deletion_label
FROM r
ORDER BY level, {Номенклатура_Папки_Const.Назва} ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] row in listRow)
                {
                    string uid = row[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = (row[1]?.ToString() ?? "");
                    string fieldParent = row[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)row[3];
                    bool deletionLabel = (bool)row[4];

                    Номенклатура_Папки_Записи record = new Номенклатура_Папки_Записи
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      
    /* ДЕРЕВО */
    public class Номенклатура_Папки_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc.png" : "folder.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Назва };
        }

        public static TreeStore Store = new TreeStore
        (
            typeof(Gdk.Pixbuf) /* Image */, 
            typeof(string)     /* ID */, 
            typeof(string)     /* Назва */
        );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static void LoadTree(UnigueID? openFolder, UnigueID? selectPointer
        )
        {
            Store.Clear();
            RootPath = SelectPath = null;

            Номенклатура_Папки_ЗаписиШвидкийВибір rootRecord = new Номенклатура_Папки_ЗаписиШвидкийВибір
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Номенклатура_Папки_Const.Назва}, 
        {Номенклатура_Папки_Const.Родич}, 
        1 AS level,
        deletion_label
    FROM {Номенклатура_Папки_Const.TABLE}
    WHERE {Номенклатура_Папки_Const.Родич} = '{Guid.Empty}'";

        

            if (openFolder != null) query += $@"
        AND uid != '{openFolder}'";

            query += $@"
    UNION ALL
    SELECT 
        {Номенклатура_Папки_Const.TABLE}.uid, 
        {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Назва}, 
        {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич}, 
        r.level + 1 AS level,
        {Номенклатура_Папки_Const.TABLE}.deletion_label 
    FROM {Номенклатура_Папки_Const.TABLE}
        JOIN r ON {Номенклатура_Папки_Const.TABLE}.{Номенклатура_Папки_Const.Родич} = r.uid";

        

            if (openFolder != null) query += $@"
    WHERE {Номенклатура_Папки_Const.TABLE}.uid != '{openFolder}'";

            query += $@"
)
SELECT 
    uid, 
    {Номенклатура_Папки_Const.Назва}, 
    {Номенклатура_Папки_Const.Родич}, 
    level,
    deletion_label
FROM r
ORDER BY level, {Номенклатура_Папки_Const.Назва} ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] row in listRow)
                {
                    string uid = row[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = (row[1]?.ToString() ?? "");
                    string fieldParent = row[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)row[3];
                    bool deletionLabel = (bool)row[4];

                    Номенклатура_Папки_ЗаписиШвидкийВибір record = new Номенклатура_Папки_ЗаписиШвидкийВибір
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      
    #endregion
    
    #region DIRECTORY "Контрагенти_Папки"
    
      
    /* ДЕРЕВО */
    public class Контрагенти_Папки_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc.png" : "folder.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Назва };
        }

        public static TreeStore Store = new TreeStore
        (
            typeof(Gdk.Pixbuf) /* Image */, 
            typeof(string)     /* ID */, 
            typeof(string)     /* Назва */
        );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static void LoadTree(UnigueID? openFolder, UnigueID? selectPointer
        )
        {
            Store.Clear();
            RootPath = SelectPath = null;

            Контрагенти_Папки_Записи rootRecord = new Контрагенти_Папки_Записи
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Контрагенти_Папки_Const.Назва}, 
        {Контрагенти_Папки_Const.Родич}, 
        1 AS level,
        deletion_label
    FROM {Контрагенти_Папки_Const.TABLE}
    WHERE {Контрагенти_Папки_Const.Родич} = '{Guid.Empty}'";

        

            if (openFolder != null) query += $@"
        AND uid != '{openFolder}'";

            query += $@"
    UNION ALL
    SELECT 
        {Контрагенти_Папки_Const.TABLE}.uid, 
        {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Назва}, 
        {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич}, 
        r.level + 1 AS level,
        {Контрагенти_Папки_Const.TABLE}.deletion_label 
    FROM {Контрагенти_Папки_Const.TABLE}
        JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid";

        

            if (openFolder != null) query += $@"
    WHERE {Контрагенти_Папки_Const.TABLE}.uid != '{openFolder}'";

            query += $@"
)
SELECT 
    uid, 
    {Контрагенти_Папки_Const.Назва}, 
    {Контрагенти_Папки_Const.Родич}, 
    level,
    deletion_label
FROM r
ORDER BY level, {Контрагенти_Папки_Const.Назва} ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] row in listRow)
                {
                    string uid = row[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = (row[1]?.ToString() ?? "");
                    string fieldParent = row[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)row[3];
                    bool deletionLabel = (bool)row[4];

                    Контрагенти_Папки_Записи record = new Контрагенти_Папки_Записи
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      
    /* ДЕРЕВО */
    public class Контрагенти_Папки_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc.png" : "folder.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Назва };
        }

        public static TreeStore Store = new TreeStore
        (
            typeof(Gdk.Pixbuf) /* Image */, 
            typeof(string)     /* ID */, 
            typeof(string)     /* Назва */
        );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static void LoadTree(UnigueID? openFolder, UnigueID? selectPointer
        )
        {
            Store.Clear();
            RootPath = SelectPath = null;

            Контрагенти_Папки_ЗаписиШвидкийВибір rootRecord = new Контрагенти_Папки_ЗаписиШвидкийВибір
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Контрагенти_Папки_Const.Назва}, 
        {Контрагенти_Папки_Const.Родич}, 
        1 AS level,
        deletion_label
    FROM {Контрагенти_Папки_Const.TABLE}
    WHERE {Контрагенти_Папки_Const.Родич} = '{Guid.Empty}'";

        

            if (openFolder != null) query += $@"
        AND uid != '{openFolder}'";

            query += $@"
    UNION ALL
    SELECT 
        {Контрагенти_Папки_Const.TABLE}.uid, 
        {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Назва}, 
        {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич}, 
        r.level + 1 AS level,
        {Контрагенти_Папки_Const.TABLE}.deletion_label 
    FROM {Контрагенти_Папки_Const.TABLE}
        JOIN r ON {Контрагенти_Папки_Const.TABLE}.{Контрагенти_Папки_Const.Родич} = r.uid";

        

            if (openFolder != null) query += $@"
    WHERE {Контрагенти_Папки_Const.TABLE}.uid != '{openFolder}'";

            query += $@"
)
SELECT 
    uid, 
    {Контрагенти_Папки_Const.Назва}, 
    {Контрагенти_Папки_Const.Родич}, 
    level,
    deletion_label
FROM r
ORDER BY level, {Контрагенти_Папки_Const.Назва} ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] row in listRow)
                {
                    string uid = row[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = (row[1]?.ToString() ?? "");
                    string fieldParent = row[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)row[3];
                    bool deletionLabel = (bool)row[4];

                    Контрагенти_Папки_ЗаписиШвидкийВибір record = new Контрагенти_Папки_ЗаписиШвидкийВибір
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      
    #endregion
    
    #region DIRECTORY "Склади_Папки"
    
      
    /* ДЕРЕВО */
    public class Склади_Папки_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc.png" : "folder.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Назва };
        }

        public static TreeStore Store = new TreeStore
        (
            typeof(Gdk.Pixbuf) /* Image */, 
            typeof(string)     /* ID */, 
            typeof(string)     /* Назва */
        );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static void LoadTree(UnigueID? openFolder, UnigueID? selectPointer
        )
        {
            Store.Clear();
            RootPath = SelectPath = null;

            Склади_Папки_Записи rootRecord = new Склади_Папки_Записи
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Склади_Папки_Const.Назва}, 
        {Склади_Папки_Const.Родич}, 
        1 AS level,
        deletion_label
    FROM {Склади_Папки_Const.TABLE}
    WHERE {Склади_Папки_Const.Родич} = '{Guid.Empty}'";

        

            if (openFolder != null) query += $@"
        AND uid != '{openFolder}'";

            query += $@"
    UNION ALL
    SELECT 
        {Склади_Папки_Const.TABLE}.uid, 
        {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Назва}, 
        {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич}, 
        r.level + 1 AS level,
        {Склади_Папки_Const.TABLE}.deletion_label 
    FROM {Склади_Папки_Const.TABLE}
        JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid";

        

            if (openFolder != null) query += $@"
    WHERE {Склади_Папки_Const.TABLE}.uid != '{openFolder}'";

            query += $@"
)
SELECT 
    uid, 
    {Склади_Папки_Const.Назва}, 
    {Склади_Папки_Const.Родич}, 
    level,
    deletion_label
FROM r
ORDER BY level, {Склади_Папки_Const.Назва} ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] row in listRow)
                {
                    string uid = row[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = (row[1]?.ToString() ?? "");
                    string fieldParent = row[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)row[3];
                    bool deletionLabel = (bool)row[4];

                    Склади_Папки_Записи record = new Склади_Папки_Записи
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      
    /* ДЕРЕВО */
    public class Склади_Папки_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc.png" : "folder.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Назва };
        }

        public static TreeStore Store = new TreeStore
        (
            typeof(Gdk.Pixbuf) /* Image */, 
            typeof(string)     /* ID */, 
            typeof(string)     /* Назва */
        );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static void LoadTree(UnigueID? openFolder, UnigueID? selectPointer
        )
        {
            Store.Clear();
            RootPath = SelectPath = null;

            Склади_Папки_ЗаписиШвидкийВибір rootRecord = new Склади_Папки_ЗаписиШвидкийВибір
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {Склади_Папки_Const.Назва}, 
        {Склади_Папки_Const.Родич}, 
        1 AS level,
        deletion_label
    FROM {Склади_Папки_Const.TABLE}
    WHERE {Склади_Папки_Const.Родич} = '{Guid.Empty}'";

        

            if (openFolder != null) query += $@"
        AND uid != '{openFolder}'";

            query += $@"
    UNION ALL
    SELECT 
        {Склади_Папки_Const.TABLE}.uid, 
        {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Назва}, 
        {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич}, 
        r.level + 1 AS level,
        {Склади_Папки_Const.TABLE}.deletion_label 
    FROM {Склади_Папки_Const.TABLE}
        JOIN r ON {Склади_Папки_Const.TABLE}.{Склади_Папки_Const.Родич} = r.uid";

        

            if (openFolder != null) query += $@"
    WHERE {Склади_Папки_Const.TABLE}.uid != '{openFolder}'";

            query += $@"
)
SELECT 
    uid, 
    {Склади_Папки_Const.Назва}, 
    {Склади_Папки_Const.Родич}, 
    level,
    deletion_label
FROM r
ORDER BY level, {Склади_Папки_Const.Назва} ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] row in listRow)
                {
                    string uid = row[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = (row[1]?.ToString() ?? "");
                    string fieldParent = row[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)row[3];
                    bool deletionLabel = (bool)row[4];

                    Склади_Папки_ЗаписиШвидкийВибір record = new Склади_Папки_ЗаписиШвидкийВибір
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      
    #endregion
    
    #region DIRECTORY "Каси"
    
      
    /* ТАБЛИЦЯ */
    public class Каси_ЗаписиПрототип
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Каси_Select Каси_Select = new Довідники.Каси_Select();
            Каси_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Каси_Const.Код /* 1 */
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
            await Каси_Select.Select();
            while (Каси_Select.MoveNext())
            {
                Довідники.Каси_Pointer? cur = Каси_Select.Current;

                if (cur != null)
                {
                    Каси_ЗаписиПрототип Record = new Каси_ЗаписиПрототип
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[Каси_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Каси_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Каси_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Каси_Select Каси_Select = new Довідники.Каси_Select();
            Каси_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Каси_Const.Код /* 1 */
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
            await Каси_Select.Select();
            while (Каси_Select.MoveNext())
            {
                Довідники.Каси_Pointer? cur = Каси_Select.Current;

                if (cur != null)
                {
                    Каси_ЗаписиШвидкийВибір Record = new Каси_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[Каси_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Каси_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "БанківськіРахункиОрганізацій"
    
      
    /* ТАБЛИЦЯ */
    public class БанківськіРахункиОрганізацій_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.БанківськіРахункиОрганізацій_Select БанківськіРахункиОрганізацій_Select = new Довідники.БанківськіРахункиОрганізацій_Select();
            БанківськіРахункиОрганізацій_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.БанківськіРахункиОрганізацій_Const.Код /* 1 */
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
            await БанківськіРахункиОрганізацій_Select.Select();
            while (БанківськіРахункиОрганізацій_Select.MoveNext())
            {
                Довідники.БанківськіРахункиОрганізацій_Pointer? cur = БанківськіРахункиОрганізацій_Select.Current;

                if (cur != null)
                {
                    БанківськіРахункиОрганізацій_Записи Record = new БанківськіРахункиОрганізацій_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[БанківськіРахункиОрганізацій_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[БанківськіРахункиОрганізацій_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.БанківськіРахункиОрганізацій_Select БанківськіРахункиОрганізацій_Select = new Довідники.БанківськіРахункиОрганізацій_Select();
            БанківськіРахункиОрганізацій_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.БанківськіРахункиОрганізацій_Const.Код /* 1 */
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
            await БанківськіРахункиОрганізацій_Select.Select();
            while (БанківськіРахункиОрганізацій_Select.MoveNext())
            {
                Довідники.БанківськіРахункиОрганізацій_Pointer? cur = БанківськіРахункиОрганізацій_Select.Current;

                if (cur != null)
                {
                    БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір Record = new БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[БанківськіРахункиОрганізацій_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[БанківськіРахункиОрганізацій_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ДоговориКонтрагентів"
    
      
    /* ТАБЛИЦЯ */
    public class ДоговориКонтрагентів_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ТипДоговору*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ДоговориКонтрагентів_Select ДоговориКонтрагентів_Select = new Довідники.ДоговориКонтрагентів_Select();
            ДоговориКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ДоговориКонтрагентів_Const.Код /* 1 */
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
            await ДоговориКонтрагентів_Select.Select();
            while (ДоговориКонтрагентів_Select.MoveNext())
            {
                Довідники.ДоговориКонтрагентів_Pointer? cur = ДоговориКонтрагентів_Select.Current;

                if (cur != null)
                {
                    ДоговориКонтрагентів_Записи Record = new ДоговориКонтрагентів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Контрагент = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[ДоговориКонтрагентів_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ДоговориКонтрагентів_Const.Назва]?.ToString() ?? "", /**/
                        ТипДоговору = Перелічення.ПсевдонімиПерелічення.ТипДоговорів_Alias( ((Перелічення.ТипДоговорів)(cur.Fields?[ДоговориКонтрагентів_Const.ТипДоговору]! != DBNull.Value ? cur.Fields?[ДоговориКонтрагентів_Const.ТипДоговору]! : 0)) ) /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ДоговориКонтрагентів_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Контрагент = "";
        string ТипДоговору = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Назва, Контрагент, ТипДоговору };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Контрагент */
            , typeof(string) /* ТипДоговору */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ТипДоговору*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ДоговориКонтрагентів_Select ДоговориКонтрагентів_Select = new Довідники.ДоговориКонтрагентів_Select();
            ДоговориКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ДоговориКонтрагентів_Const.Назва /* 1 */
                    , Довідники.ДоговориКонтрагентів_Const.ТипДоговору /* 2 */
                    
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
            await ДоговориКонтрагентів_Select.Select();
            while (ДоговориКонтрагентів_Select.MoveNext())
            {
                Довідники.ДоговориКонтрагентів_Pointer? cur = ДоговориКонтрагентів_Select.Current;

                if (cur != null)
                {
                    ДоговориКонтрагентів_ЗаписиШвидкийВибір Record = new ДоговориКонтрагентів_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Контрагент = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ДоговориКонтрагентів_Const.Назва]?.ToString() ?? "", /**/
                        ТипДоговору = Перелічення.ПсевдонімиПерелічення.ТипДоговорів_Alias( ((Перелічення.ТипДоговорів)(cur.Fields?[ДоговориКонтрагентів_Const.ТипДоговору]! != DBNull.Value ? cur.Fields?[ДоговориКонтрагентів_Const.ТипДоговору]! : 0)) ) /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "БанківськіРахункиКонтрагентів"
    
      
    /* ТАБЛИЦЯ */
    public class БанківськіРахункиКонтрагентів_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.БанківськіРахункиКонтрагентів_Select БанківськіРахункиКонтрагентів_Select = new Довідники.БанківськіРахункиКонтрагентів_Select();
            БанківськіРахункиКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.БанківськіРахункиКонтрагентів_Const.Код /* 1 */
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
            await БанківськіРахункиКонтрагентів_Select.Select();
            while (БанківськіРахункиКонтрагентів_Select.MoveNext())
            {
                Довідники.БанківськіРахункиКонтрагентів_Pointer? cur = БанківськіРахункиКонтрагентів_Select.Current;

                if (cur != null)
                {
                    БанківськіРахункиКонтрагентів_Записи Record = new БанківськіРахункиКонтрагентів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.БанківськіРахункиКонтрагентів_Select БанківськіРахункиКонтрагентів_Select = new Довідники.БанківськіРахункиКонтрагентів_Select();
            БанківськіРахункиКонтрагентів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.БанківськіРахункиКонтрагентів_Const.Код /* 1 */
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
            await БанківськіРахункиКонтрагентів_Select.Select();
            while (БанківськіРахункиКонтрагентів_Select.MoveNext())
            {
                Довідники.БанківськіРахункиКонтрагентів_Pointer? cur = БанківськіРахункиКонтрагентів_Select.Current;

                if (cur != null)
                {
                    БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір Record = new БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Валюта = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Код = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[БанківськіРахункиКонтрагентів_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СтаттяРухуКоштів"
    
      
    /* ТАБЛИЦЯ */
    public class СтаттяРухуКоштів_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Код = "";
        string КореспондуючийРахунок = "";
        string ВидРухуКоштів = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Назва, Код, КореспондуючийРахунок, ВидРухуКоштів };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Код */
            , typeof(string) /* КореспондуючийРахунок */
            , typeof(string) /* ВидРухуКоштів */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("КореспондуючийРахунок", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*КореспондуючийРахунок*/
            treeView.AppendColumn(new TreeViewColumn("ВидРухуКоштів", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ВидРухуКоштів*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СтаттяРухуКоштів_Select СтаттяРухуКоштів_Select = new Довідники.СтаттяРухуКоштів_Select();
            СтаттяРухуКоштів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СтаттяРухуКоштів_Const.Назва /* 1 */
                    , Довідники.СтаттяРухуКоштів_Const.Код /* 2 */
                    , Довідники.СтаттяРухуКоштів_Const.КореспондуючийРахунок /* 3 */
                    , Довідники.СтаттяРухуКоштів_Const.ВидРухуКоштів /* 4 */
                    
                });

            /* Where */
            СтаттяРухуКоштів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СтаттяРухуКоштів_Select.QuerySelect.Order.Add(Довідники.СтаттяРухуКоштів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await СтаттяРухуКоштів_Select.Select();
            while (СтаттяРухуКоштів_Select.MoveNext())
            {
                Довідники.СтаттяРухуКоштів_Pointer? cur = СтаттяРухуКоштів_Select.Current;

                if (cur != null)
                {
                    СтаттяРухуКоштів_Записи Record = new СтаттяРухуКоштів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[СтаттяРухуКоштів_Const.Назва]?.ToString() ?? "", /**/
                        Код = cur.Fields?[СтаттяРухуКоштів_Const.Код]?.ToString() ?? "", /**/
                        КореспондуючийРахунок = cur.Fields?[СтаттяРухуКоштів_Const.КореспондуючийРахунок]?.ToString() ?? "", /**/
                        ВидРухуКоштів = Перелічення.ПсевдонімиПерелічення.ВидиРухуКоштів_Alias( ((Перелічення.ВидиРухуКоштів)(cur.Fields?[СтаттяРухуКоштів_Const.ВидРухуКоштів]! != DBNull.Value ? cur.Fields?[СтаттяРухуКоштів_Const.ВидРухуКоштів]! : 0)) ) /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class СтаттяРухуКоштів_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СтаттяРухуКоштів_Select СтаттяРухуКоштів_Select = new Довідники.СтаттяРухуКоштів_Select();
            СтаттяРухуКоштів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СтаттяРухуКоштів_Const.Назва /* 1 */
                    
                });

            /* Where */
            СтаттяРухуКоштів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СтаттяРухуКоштів_Select.QuerySelect.Order.Add(Довідники.СтаттяРухуКоштів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await СтаттяРухуКоштів_Select.Select();
            while (СтаттяРухуКоштів_Select.MoveNext())
            {
                Довідники.СтаттяРухуКоштів_Pointer? cur = СтаттяРухуКоштів_Select.Current;

                if (cur != null)
                {
                    СтаттяРухуКоштів_ЗаписиШвидкийВибір Record = new СтаттяРухуКоштів_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[СтаттяРухуКоштів_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СеріїНоменклатури"
    
      
    /* ТАБЛИЦЯ */
    public class СеріїНоменклатури_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Номер*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СеріїНоменклатури_Select СеріїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            СеріїНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СеріїНоменклатури_Const.Номер /* 1 */
                    
                });

            /* Where */
            СеріїНоменклатури_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СеріїНоменклатури_Select.QuerySelect.Order.Add(Довідники.СеріїНоменклатури_Const.Номер, SelectOrder.ASC);
            

            /* SELECT */
            await СеріїНоменклатури_Select.Select();
            while (СеріїНоменклатури_Select.MoveNext())
            {
                Довідники.СеріїНоменклатури_Pointer? cur = СеріїНоменклатури_Select.Current;

                if (cur != null)
                {
                    СеріїНоменклатури_Записи Record = new СеріїНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Номер = cur.Fields?[СеріїНоменклатури_Const.Номер]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class СеріїНоменклатури_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Номер*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СеріїНоменклатури_Select СеріїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            СеріїНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СеріїНоменклатури_Const.Номер /* 1 */
                    
                });

            /* Where */
            СеріїНоменклатури_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СеріїНоменклатури_Select.QuerySelect.Order.Add(Довідники.СеріїНоменклатури_Const.Номер, SelectOrder.ASC);
            

            /* SELECT */
            await СеріїНоменклатури_Select.Select();
            while (СеріїНоменклатури_Select.MoveNext())
            {
                Довідники.СеріїНоменклатури_Pointer? cur = СеріїНоменклатури_Select.Current;

                if (cur != null)
                {
                    СеріїНоменклатури_ЗаписиШвидкийВибір Record = new СеріїНоменклатури_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Номер = cur.Fields?[СеріїНоменклатури_Const.Номер]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ПартіяТоварівКомпозит"
    
      
    /* ТАБЛИЦЯ */
    public class ПартіяТоварівКомпозит_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Дата*/
            treeView.AppendColumn(new TreeViewColumn("ТипДокументу", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ТипДокументу*/
            treeView.AppendColumn(new TreeViewColumn("ПоступленняТоварівТаПослуг", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ПоступленняТоварівТаПослуг*/
            treeView.AppendColumn(new TreeViewColumn("ВведенняЗалишків", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ВведенняЗалишків*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ПартіяТоварівКомпозит_Select ПартіяТоварівКомпозит_Select = new Довідники.ПартіяТоварівКомпозит_Select();
            ПартіяТоварівКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ПартіяТоварівКомпозит_Const.Назва /* 1 */
                    , Довідники.ПартіяТоварівКомпозит_Const.Дата /* 2 */
                    , Довідники.ПартіяТоварівКомпозит_Const.ТипДокументу /* 3 */
                    
                });

            /* Where */
            ПартіяТоварівКомпозит_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПартіяТоварівКомпозит_Select.QuerySelect.Order.Add(Довідники.ПартіяТоварівКомпозит_Const.Дата, SelectOrder.ASC);
            
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
            await ПартіяТоварівКомпозит_Select.Select();
            while (ПартіяТоварівКомпозит_Select.MoveNext())
            {
                Довідники.ПартіяТоварівКомпозит_Pointer? cur = ПартіяТоварівКомпозит_Select.Current;

                if (cur != null)
                {
                    ПартіяТоварівКомпозит_Записи Record = new ПартіяТоварівКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        ПоступленняТоварівТаПослуг = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        ВведенняЗалишків = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПартіяТоварівКомпозит_Const.Назва]?.ToString() ?? "", /**/
                        Дата = cur.Fields?[ПартіяТоварівКомпозит_Const.Дата]?.ToString() ?? "", /**/
                        ТипДокументу = Перелічення.ПсевдонімиПерелічення.ТипДокументуПартіяТоварівКомпозит_Alias( ((Перелічення.ТипДокументуПартіяТоварівКомпозит)(cur.Fields?[ПартіяТоварівКомпозит_Const.ТипДокументу]! != DBNull.Value ? cur.Fields?[ПартіяТоварівКомпозит_Const.ТипДокументу]! : 0)) ) /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ПартіяТоварівКомпозит_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Дата = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Назва, Дата };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Дата */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Дата*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ПартіяТоварівКомпозит_Select ПартіяТоварівКомпозит_Select = new Довідники.ПартіяТоварівКомпозит_Select();
            ПартіяТоварівКомпозит_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ПартіяТоварівКомпозит_Const.Назва /* 1 */
                    , Довідники.ПартіяТоварівКомпозит_Const.Дата /* 2 */
                    
                });

            /* Where */
            ПартіяТоварівКомпозит_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПартіяТоварівКомпозит_Select.QuerySelect.Order.Add(Довідники.ПартіяТоварівКомпозит_Const.Дата, SelectOrder.ASC);
            

            /* SELECT */
            await ПартіяТоварівКомпозит_Select.Select();
            while (ПартіяТоварівКомпозит_Select.MoveNext())
            {
                Довідники.ПартіяТоварівКомпозит_Pointer? cur = ПартіяТоварівКомпозит_Select.Current;

                if (cur != null)
                {
                    ПартіяТоварівКомпозит_ЗаписиШвидкийВибір Record = new ПартіяТоварівКомпозит_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[ПартіяТоварівКомпозит_Const.Назва]?.ToString() ?? "", /**/
                        Дата = cur.Fields?[ПартіяТоварівКомпозит_Const.Дата]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиЗапасів"
    
      
    /* ТАБЛИЦЯ */
    public class ВидиЗапасів_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЗапасів_Select ВидиЗапасів_Select = new Довідники.ВидиЗапасів_Select();
            ВидиЗапасів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ВидиЗапасів_Const.Код /* 1 */
                    , Довідники.ВидиЗапасів_Const.Назва /* 2 */
                    
                });

            /* Where */
            ВидиЗапасів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиЗапасів_Select.QuerySelect.Order.Add(Довідники.ВидиЗапасів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЗапасів_Select.Select();
            while (ВидиЗапасів_Select.MoveNext())
            {
                Довідники.ВидиЗапасів_Pointer? cur = ВидиЗапасів_Select.Current;

                if (cur != null)
                {
                    ВидиЗапасів_Записи Record = new ВидиЗапасів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[ВидиЗапасів_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВидиЗапасів_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ВидиЗапасів_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЗапасів_Select ВидиЗапасів_Select = new Довідники.ВидиЗапасів_Select();
            ВидиЗапасів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ВидиЗапасів_Const.Код /* 1 */
                    , Довідники.ВидиЗапасів_Const.Назва /* 2 */
                    
                });

            /* Where */
            ВидиЗапасів_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ВидиЗапасів_Select.QuerySelect.Order.Add(Довідники.ВидиЗапасів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЗапасів_Select.Select();
            while (ВидиЗапасів_Select.MoveNext())
            {
                Довідники.ВидиЗапасів_Pointer? cur = ВидиЗапасів_Select.Current;

                if (cur != null)
                {
                    ВидиЗапасів_ЗаписиШвидкийВибір Record = new ВидиЗапасів_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[ВидиЗапасів_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВидиЗапасів_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Банки"
    
      
    /* ТАБЛИЦЯ */
    public class Банки_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ПовнаНазва = "";
        string КодМФО = "";
        string КодЄДРПОУ = "";
        string НомерЛіцензії = "";
        string ДатаЛіцензії = "";
        string Статус = "";
        string ТипНаселеногоПункту = "";
        string УнікальнийКодБанку = "";
        string ПоштовийІндекс = "";
        string НазваНаселеногоПункту = "";
        string Адреса = "";
        string НомерТелефону = "";
        string ДатаВідкриттяУстанови = "";
        string ДатаЗакриттяУстанови = "";
        string КодНБУ = "";
        string КодСтатусу = "";
        string ДатаЗапису = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Код, Назва, ПовнаНазва, КодМФО, КодЄДРПОУ, НомерЛіцензії, ДатаЛіцензії, Статус, ТипНаселеногоПункту, УнікальнийКодБанку, ПоштовийІндекс, НазваНаселеногоПункту, Адреса, НомерТелефону, ДатаВідкриттяУстанови, ДатаЗакриттяУстанови, КодНБУ, КодСтатусу, ДатаЗапису };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* ПовнаНазва */
            , typeof(string) /* КодМФО */
            , typeof(string) /* КодЄДРПОУ */
            , typeof(string) /* НомерЛіцензії */
            , typeof(string) /* ДатаЛіцензії */
            , typeof(string) /* Статус */
            , typeof(string) /* ТипНаселеногоПункту */
            , typeof(string) /* УнікальнийКодБанку */
            , typeof(string) /* ПоштовийІндекс */
            , typeof(string) /* НазваНаселеногоПункту */
            , typeof(string) /* Адреса */
            , typeof(string) /* НомерТелефону */
            , typeof(string) /* ДатаВідкриттяУстанови */
            , typeof(string) /* ДатаЗакриттяУстанови */
            , typeof(string) /* КодНБУ */
            , typeof(string) /* КодСтатусу */
            , typeof(string) /* ДатаЗапису */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Повна назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ПовнаНазва*/
            treeView.AppendColumn(new TreeViewColumn("Код МФО", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*КодМФО*/
            treeView.AppendColumn(new TreeViewColumn("ЄДРПОУ", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*КодЄДРПОУ*/
            treeView.AppendColumn(new TreeViewColumn("Номер ліцензії", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*НомерЛіцензії*/
            treeView.AppendColumn(new TreeViewColumn("Дата ліцензії", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*ДатаЛіцензії*/
            treeView.AppendColumn(new TreeViewColumn("Статус", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*Статус*/
            treeView.AppendColumn(new TreeViewColumn("Тип населеного пункту", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true, SortColumnId = 10 } ); /*ТипНаселеногоПункту*/
            treeView.AppendColumn(new TreeViewColumn("Унікальний код", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true, SortColumnId = 11 } ); /*УнікальнийКодБанку*/
            treeView.AppendColumn(new TreeViewColumn("Поштовий індекс", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true, SortColumnId = 12 } ); /*ПоштовийІндекс*/
            treeView.AppendColumn(new TreeViewColumn("Населений пункт", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true, SortColumnId = 13 } ); /*НазваНаселеногоПункту*/
            treeView.AppendColumn(new TreeViewColumn("Адреса", new CellRendererText() { Xpad = 4 }, "text", 14) { MinWidth = 20, Resizable = true, SortColumnId = 14 } ); /*Адреса*/
            treeView.AppendColumn(new TreeViewColumn("Телефон", new CellRendererText() { Xpad = 4 }, "text", 15) { MinWidth = 20, Resizable = true, SortColumnId = 15 } ); /*НомерТелефону*/
            treeView.AppendColumn(new TreeViewColumn("Дата відкриття установи", new CellRendererText() { Xpad = 4 }, "text", 16) { MinWidth = 20, Resizable = true, SortColumnId = 16 } ); /*ДатаВідкриттяУстанови*/
            treeView.AppendColumn(new TreeViewColumn("Дата закриття установи", new CellRendererText() { Xpad = 4 }, "text", 17) { MinWidth = 20, Resizable = true, SortColumnId = 17 } ); /*ДатаЗакриттяУстанови*/
            treeView.AppendColumn(new TreeViewColumn("Код НБУ", new CellRendererText() { Xpad = 4 }, "text", 18) { MinWidth = 20, Resizable = true, SortColumnId = 18 } ); /*КодНБУ*/
            treeView.AppendColumn(new TreeViewColumn("КодСтатусу", new CellRendererText() { Xpad = 4 }, "text", 19) { MinWidth = 20, Resizable = true, SortColumnId = 19 } ); /*КодСтатусу*/
            treeView.AppendColumn(new TreeViewColumn("ДатаЗапису", new CellRendererText() { Xpad = 4 }, "text", 20) { MinWidth = 20, Resizable = true, SortColumnId = 20 } ); /*ДатаЗапису*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Банки_Select Банки_Select = new Довідники.Банки_Select();
            Банки_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Банки_Const.Код /* 1 */
                    , Довідники.Банки_Const.Назва /* 2 */
                    , Довідники.Банки_Const.ПовнаНазва /* 3 */
                    , Довідники.Банки_Const.КодМФО /* 4 */
                    , Довідники.Банки_Const.КодЄДРПОУ /* 5 */
                    , Довідники.Банки_Const.НомерЛіцензії /* 6 */
                    , Довідники.Банки_Const.ДатаЛіцензії /* 7 */
                    , Довідники.Банки_Const.Статус /* 8 */
                    , Довідники.Банки_Const.ТипНаселеногоПункту /* 9 */
                    , Довідники.Банки_Const.УнікальнийКодБанку /* 10 */
                    , Довідники.Банки_Const.ПоштовийІндекс /* 11 */
                    , Довідники.Банки_Const.НазваНаселеногоПункту /* 12 */
                    , Довідники.Банки_Const.Адреса /* 13 */
                    , Довідники.Банки_Const.НомерТелефону /* 14 */
                    , Довідники.Банки_Const.ДатаВідкриттяУстанови /* 15 */
                    , Довідники.Банки_Const.ДатаЗакриттяУстанови /* 16 */
                    , Довідники.Банки_Const.КодНБУ /* 17 */
                    , Довідники.Банки_Const.КодСтатусу /* 18 */
                    , Довідники.Банки_Const.ДатаЗапису /* 19 */
                    
                });

            /* Where */
            Банки_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Банки_Select.QuerySelect.Order.Add(Довідники.Банки_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Банки_Select.Select();
            while (Банки_Select.MoveNext())
            {
                Довідники.Банки_Pointer? cur = Банки_Select.Current;

                if (cur != null)
                {
                    Банки_Записи Record = new Банки_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Банки_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Банки_Const.Назва]?.ToString() ?? "", /**/
                        ПовнаНазва = cur.Fields?[Банки_Const.ПовнаНазва]?.ToString() ?? "", /**/
                        КодМФО = cur.Fields?[Банки_Const.КодМФО]?.ToString() ?? "", /**/
                        КодЄДРПОУ = cur.Fields?[Банки_Const.КодЄДРПОУ]?.ToString() ?? "", /**/
                        НомерЛіцензії = cur.Fields?[Банки_Const.НомерЛіцензії]?.ToString() ?? "", /**/
                        ДатаЛіцензії = cur.Fields?[Банки_Const.ДатаЛіцензії]?.ToString() ?? "", /**/
                        Статус = cur.Fields?[Банки_Const.Статус]?.ToString() ?? "", /**/
                        ТипНаселеногоПункту = cur.Fields?[Банки_Const.ТипНаселеногоПункту]?.ToString() ?? "", /**/
                        УнікальнийКодБанку = cur.Fields?[Банки_Const.УнікальнийКодБанку]?.ToString() ?? "", /**/
                        ПоштовийІндекс = cur.Fields?[Банки_Const.ПоштовийІндекс]?.ToString() ?? "", /**/
                        НазваНаселеногоПункту = cur.Fields?[Банки_Const.НазваНаселеногоПункту]?.ToString() ?? "", /**/
                        Адреса = cur.Fields?[Банки_Const.Адреса]?.ToString() ?? "", /**/
                        НомерТелефону = cur.Fields?[Банки_Const.НомерТелефону]?.ToString() ?? "", /**/
                        ДатаВідкриттяУстанови = cur.Fields?[Банки_Const.ДатаВідкриттяУстанови]?.ToString() ?? "", /**/
                        ДатаЗакриттяУстанови = cur.Fields?[Банки_Const.ДатаЗакриттяУстанови]?.ToString() ?? "", /**/
                        КодНБУ = cur.Fields?[Банки_Const.КодНБУ]?.ToString() ?? "", /**/
                        КодСтатусу = cur.Fields?[Банки_Const.КодСтатусу]?.ToString() ?? "", /**/
                        ДатаЗапису = cur.Fields?[Банки_Const.ДатаЗапису]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Банки_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Банки_Select Банки_Select = new Довідники.Банки_Select();
            Банки_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Банки_Const.Код /* 1 */
                    , Довідники.Банки_Const.Назва /* 2 */
                    
                });

            /* Where */
            Банки_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Банки_Select.QuerySelect.Order.Add(Довідники.Банки_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Банки_Select.Select();
            while (Банки_Select.MoveNext())
            {
                Довідники.Банки_Pointer? cur = Банки_Select.Current;

                if (cur != null)
                {
                    Банки_ЗаписиШвидкийВибір Record = new Банки_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Банки_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Банки_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СкладськіПриміщення"
    
      
    /* ТАБЛИЦЯ */
    public class СкладськіПриміщення_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Склад = "";
        string НалаштуванняАдресногоЗберігання = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Назва, Склад, НалаштуванняАдресногоЗберігання };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Склад */
            , typeof(string) /* НалаштуванняАдресногоЗберігання */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Налаштування", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*НалаштуванняАдресногоЗберігання*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіПриміщення_Select СкладськіПриміщення_Select = new Довідники.СкладськіПриміщення_Select();
            СкладськіПриміщення_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СкладськіПриміщення_Const.Назва /* 1 */
                    , Довідники.СкладськіПриміщення_Const.НалаштуванняАдресногоЗберігання /* 2 */
                    
                });

            /* Where */
            СкладськіПриміщення_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СкладськіПриміщення_Select.QuerySelect.Order.Add(Довідники.СкладськіПриміщення_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                СкладськіПриміщення_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Довідники.СкладськіПриміщення_Const.Склад, СкладськіПриміщення_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  СкладськіПриміщення_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Склади_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            await СкладськіПриміщення_Select.Select();
            while (СкладськіПриміщення_Select.MoveNext())
            {
                Довідники.СкладськіПриміщення_Pointer? cur = СкладськіПриміщення_Select.Current;

                if (cur != null)
                {
                    СкладськіПриміщення_Записи Record = new СкладськіПриміщення_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Склад = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[СкладськіПриміщення_Const.Назва]?.ToString() ?? "", /**/
                        НалаштуванняАдресногоЗберігання = Перелічення.ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_Alias( ((Перелічення.НалаштуванняАдресногоЗберігання)(cur.Fields?[СкладськіПриміщення_Const.НалаштуванняАдресногоЗберігання]! != DBNull.Value ? cur.Fields?[СкладськіПриміщення_Const.НалаштуванняАдресногоЗберігання]! : 0)) ) /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class СкладськіПриміщення_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Склад = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Назва, Склад };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Склад */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Склад*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіПриміщення_Select СкладськіПриміщення_Select = new Довідники.СкладськіПриміщення_Select();
            СкладськіПриміщення_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СкладськіПриміщення_Const.Назва /* 1 */
                    
                });

            /* Where */
            СкладськіПриміщення_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СкладськіПриміщення_Select.QuerySelect.Order.Add(Довідники.СкладськіПриміщення_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                СкладськіПриміщення_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Довідники.СкладськіПриміщення_Const.Склад, СкладськіПриміщення_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  СкладськіПриміщення_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Склади_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            await СкладськіПриміщення_Select.Select();
            while (СкладськіПриміщення_Select.MoveNext())
            {
                Довідники.СкладськіПриміщення_Pointer? cur = СкладськіПриміщення_Select.Current;

                if (cur != null)
                {
                    СкладськіПриміщення_ЗаписиШвидкийВибір Record = new СкладськіПриміщення_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Склад = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[СкладськіПриміщення_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СкладськіКомірки"
    
      
    /* ТАБЛИЦЯ */
    public class СкладськіКомірки_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Приміщення = "";
        string Лінія = "";
        string Позиція = "";
        string Стелаж = "";
        string Ярус = "";
        string ТипСкладськоїКомірки = "";
        string Типорозмір = "";
        string Папка = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Назва, Приміщення, Лінія, Позиція, Стелаж, Ярус, ТипСкладськоїКомірки, Типорозмір, Папка };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Приміщення */
            , typeof(string) /* Лінія */
            , typeof(string) /* Позиція */
            , typeof(string) /* Стелаж */
            , typeof(string) /* Ярус */
            , typeof(string) /* ТипСкладськоїКомірки */
            , typeof(string) /* Типорозмір */
            , typeof(string) /* Папка */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Приміщення", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Приміщення*/
            treeView.AppendColumn(new TreeViewColumn("Лінія", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Лінія*/
            treeView.AppendColumn(new TreeViewColumn("Позиція", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Позиція*/
            treeView.AppendColumn(new TreeViewColumn("Стелаж", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Стелаж*/
            treeView.AppendColumn(new TreeViewColumn("Ярус", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Ярус*/
            treeView.AppendColumn(new TreeViewColumn("Тип комірки", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*ТипСкладськоїКомірки*/
            treeView.AppendColumn(new TreeViewColumn("Типорозмір", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*Типорозмір*/
            treeView.AppendColumn(new TreeViewColumn("Папка", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true, SortColumnId = 10 } ); /*Папка*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіКомірки_Select СкладськіКомірки_Select = new Довідники.СкладськіКомірки_Select();
            СкладськіКомірки_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СкладськіКомірки_Const.Назва /* 1 */
                    , Довідники.СкладськіКомірки_Const.Лінія /* 2 */
                    , Довідники.СкладськіКомірки_Const.Позиція /* 3 */
                    , Довідники.СкладськіКомірки_Const.Стелаж /* 4 */
                    , Довідники.СкладськіКомірки_Const.Ярус /* 5 */
                    , Довідники.СкладськіКомірки_Const.ТипСкладськоїКомірки /* 6 */
                    
                });

            /* Where */
            СкладськіКомірки_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СкладськіКомірки_Select.QuerySelect.Order.Add(Довідники.СкладськіКомірки_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                СкладськіКомірки_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.СкладськіПриміщення_Const.TABLE, Довідники.СкладськіКомірки_Const.Приміщення, СкладськіКомірки_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  СкладськіКомірки_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.СкладськіПриміщення_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                СкладськіКомірки_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ТипорозміриКомірок_Const.TABLE, Довідники.СкладськіКомірки_Const.Типорозмір, СкладськіКомірки_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  СкладськіКомірки_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.ТипорозміриКомірок_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                СкладськіКомірки_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.СкладськіКомірки_Папки_Const.TABLE, Довідники.СкладськіКомірки_Const.Папка, СкладськіКомірки_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  СкладськіКомірки_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.СкладськіКомірки_Папки_Const.Назва, "join_tab_3_field_1"));
                  

            /* SELECT */
            await СкладськіКомірки_Select.Select();
            while (СкладськіКомірки_Select.MoveNext())
            {
                Довідники.СкладськіКомірки_Pointer? cur = СкладськіКомірки_Select.Current;

                if (cur != null)
                {
                    СкладськіКомірки_Записи Record = new СкладськіКомірки_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Приміщення = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Типорозмір = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Папка = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[СкладськіКомірки_Const.Назва]?.ToString() ?? "", /**/
                        Лінія = cur.Fields?[СкладськіКомірки_Const.Лінія]?.ToString() ?? "", /**/
                        Позиція = cur.Fields?[СкладськіКомірки_Const.Позиція]?.ToString() ?? "", /**/
                        Стелаж = cur.Fields?[СкладськіКомірки_Const.Стелаж]?.ToString() ?? "", /**/
                        Ярус = cur.Fields?[СкладськіКомірки_Const.Ярус]?.ToString() ?? "", /**/
                        ТипСкладськоїКомірки = Перелічення.ПсевдонімиПерелічення.ТипиСкладськихКомірок_Alias( ((Перелічення.ТипиСкладськихКомірок)(cur.Fields?[СкладськіКомірки_Const.ТипСкладськоїКомірки]! != DBNull.Value ? cur.Fields?[СкладськіКомірки_Const.ТипСкладськоїКомірки]! : 0)) ) /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class СкладськіКомірки_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Приміщення = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Назва, Приміщення };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Приміщення */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Приміщення", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Приміщення*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіКомірки_Select СкладськіКомірки_Select = new Довідники.СкладськіКомірки_Select();
            СкладськіКомірки_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.СкладськіКомірки_Const.Назва /* 1 */
                    
                });

            /* Where */
            СкладськіКомірки_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              СкладськіКомірки_Select.QuerySelect.Order.Add(Довідники.СкладськіКомірки_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                СкладськіКомірки_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.СкладськіПриміщення_Const.TABLE, Довідники.СкладськіКомірки_Const.Приміщення, СкладськіКомірки_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  СкладськіКомірки_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.СкладськіПриміщення_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            await СкладськіКомірки_Select.Select();
            while (СкладськіКомірки_Select.MoveNext())
            {
                Довідники.СкладськіКомірки_Pointer? cur = СкладськіКомірки_Select.Current;

                if (cur != null)
                {
                    СкладськіКомірки_ЗаписиШвидкийВибір Record = new СкладськіКомірки_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Приміщення = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[СкладськіКомірки_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ОбластьЗберігання"
    
      
    /* ТАБЛИЦЯ */
    public class ОбластьЗберігання_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Приміщення = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Назва, Приміщення };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Приміщення */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Приміщення", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Приміщення*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ОбластьЗберігання_Select ОбластьЗберігання_Select = new Довідники.ОбластьЗберігання_Select();
            ОбластьЗберігання_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ОбластьЗберігання_Const.Назва /* 1 */
                    
                });

            /* Where */
            ОбластьЗберігання_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ОбластьЗберігання_Select.QuerySelect.Order.Add(Довідники.ОбластьЗберігання_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                ОбластьЗберігання_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.СкладськіПриміщення_Const.TABLE, Довідники.ОбластьЗберігання_Const.Приміщення, ОбластьЗберігання_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ОбластьЗберігання_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.СкладськіПриміщення_Const.Назва, "join_tab_1_field_1"));
                  

            /* SELECT */
            await ОбластьЗберігання_Select.Select();
            while (ОбластьЗберігання_Select.MoveNext())
            {
                Довідники.ОбластьЗберігання_Pointer? cur = ОбластьЗберігання_Select.Current;

                if (cur != null)
                {
                    ОбластьЗберігання_Записи Record = new ОбластьЗберігання_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Приміщення = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ОбластьЗберігання_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ТипорозміриКомірок"
    
      
    /* ТАБЛИЦЯ */
    public class ТипорозміриКомірок_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Висота = "";
        string Глибина = "";
        string Вантажопідйомність = "";
        string Обєм = "";
        string Ширина = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Назва, Висота, Глибина, Вантажопідйомність, Обєм, Ширина };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Назва */
            , typeof(string) /* Висота */
            , typeof(string) /* Глибина */
            , typeof(string) /* Вантажопідйомність */
            , typeof(string) /* Обєм */
            , typeof(string) /* Ширина */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Висота", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Висота*/
            treeView.AppendColumn(new TreeViewColumn("Глибина", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Глибина*/
            treeView.AppendColumn(new TreeViewColumn("Вантажопідйомність", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Вантажопідйомність*/
            treeView.AppendColumn(new TreeViewColumn("Обєм", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Обєм*/
            treeView.AppendColumn(new TreeViewColumn("Ширина", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Ширина*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ТипорозміриКомірок_Select ТипорозміриКомірок_Select = new Довідники.ТипорозміриКомірок_Select();
            ТипорозміриКомірок_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ТипорозміриКомірок_Const.Назва /* 1 */
                    , Довідники.ТипорозміриКомірок_Const.Висота /* 2 */
                    , Довідники.ТипорозміриКомірок_Const.Глибина /* 3 */
                    , Довідники.ТипорозміриКомірок_Const.Вантажопідйомність /* 4 */
                    , Довідники.ТипорозміриКомірок_Const.Обєм /* 5 */
                    , Довідники.ТипорозміриКомірок_Const.Ширина /* 6 */
                    
                });

            /* Where */
            ТипорозміриКомірок_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ТипорозміриКомірок_Select.QuerySelect.Order.Add(Довідники.ТипорозміриКомірок_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ТипорозміриКомірок_Select.Select();
            while (ТипорозміриКомірок_Select.MoveNext())
            {
                Довідники.ТипорозміриКомірок_Pointer? cur = ТипорозміриКомірок_Select.Current;

                if (cur != null)
                {
                    ТипорозміриКомірок_Записи Record = new ТипорозміриКомірок_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[ТипорозміриКомірок_Const.Назва]?.ToString() ?? "", /**/
                        Висота = cur.Fields?[ТипорозміриКомірок_Const.Висота]?.ToString() ?? "", /**/
                        Глибина = cur.Fields?[ТипорозміриКомірок_Const.Глибина]?.ToString() ?? "", /**/
                        Вантажопідйомність = cur.Fields?[ТипорозміриКомірок_Const.Вантажопідйомність]?.ToString() ?? "", /**/
                        Обєм = cur.Fields?[ТипорозміриКомірок_Const.Обєм]?.ToString() ?? "", /**/
                        Ширина = cur.Fields?[ТипорозміриКомірок_Const.Ширина]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class ТипорозміриКомірок_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ТипорозміриКомірок_Select ТипорозміриКомірок_Select = new Довідники.ТипорозміриКомірок_Select();
            ТипорозміриКомірок_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.ТипорозміриКомірок_Const.Назва /* 1 */
                    
                });

            /* Where */
            ТипорозміриКомірок_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ТипорозміриКомірок_Select.QuerySelect.Order.Add(Довідники.ТипорозміриКомірок_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ТипорозміриКомірок_Select.Select();
            while (ТипорозміриКомірок_Select.MoveNext())
            {
                Довідники.ТипорозміриКомірок_Pointer? cur = ТипорозміриКомірок_Select.Current;

                if (cur != null)
                {
                    ТипорозміриКомірок_ЗаписиШвидкийВибір Record = new ТипорозміриКомірок_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Назва = cur.Fields?[ТипорозміриКомірок_Const.Назва]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СкладськіКомірки_Папки"
    
      
    /* ДЕРЕВО */
    public class СкладськіКомірки_Папки_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc.png" : "folder.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Назва };
        }

        public static TreeStore Store = new TreeStore
        (
            typeof(Gdk.Pixbuf) /* Image */, 
            typeof(string)     /* ID */, 
            typeof(string)     /* Назва */
        );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static void LoadTree(UnigueID? openFolder, UnigueID? selectPointer
        , UnigueID? owner)
        {
            Store.Clear();
            RootPath = SelectPath = null;

            СкладськіКомірки_Папки_Записи rootRecord = new СкладськіКомірки_Папки_Записи
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {СкладськіКомірки_Папки_Const.Назва}, 
        {СкладськіКомірки_Папки_Const.Родич}, 
        1 AS level,
        deletion_label
    FROM {СкладськіКомірки_Папки_Const.TABLE}
    WHERE {СкладськіКомірки_Папки_Const.Родич} = '{Guid.Empty}'";

        
            if (owner != null && !owner.IsEmpty()) query += $@"
        AND {СкладськіКомірки_Папки_Const.Власник} = '{owner}'";
        

            if (openFolder != null) query += $@"
        AND uid != '{openFolder}'";

            query += $@"
    UNION ALL
    SELECT 
        {СкладськіКомірки_Папки_Const.TABLE}.uid, 
        {СкладськіКомірки_Папки_Const.TABLE}.{СкладськіКомірки_Папки_Const.Назва}, 
        {СкладськіКомірки_Папки_Const.TABLE}.{СкладськіКомірки_Папки_Const.Родич}, 
        r.level + 1 AS level,
        {СкладськіКомірки_Папки_Const.TABLE}.deletion_label 
    FROM {СкладськіКомірки_Папки_Const.TABLE}
        JOIN r ON {СкладськіКомірки_Папки_Const.TABLE}.{СкладськіКомірки_Папки_Const.Родич} = r.uid";

        
            if (owner != null && !owner.IsEmpty()) query += $@"
        AND {СкладськіКомірки_Папки_Const.Власник} = '{owner}'";
        

            if (openFolder != null) query += $@"
    WHERE {СкладськіКомірки_Папки_Const.TABLE}.uid != '{openFolder}'";

            query += $@"
)
SELECT 
    uid, 
    {СкладськіКомірки_Папки_Const.Назва}, 
    {СкладськіКомірки_Папки_Const.Родич}, 
    level,
    deletion_label
FROM r
ORDER BY level, {СкладськіКомірки_Папки_Const.Назва} ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] row in listRow)
                {
                    string uid = row[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = (row[1]?.ToString() ?? "");
                    string fieldParent = row[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)row[3];
                    bool deletionLabel = (bool)row[4];

                    СкладськіКомірки_Папки_Записи record = new СкладськіКомірки_Папки_Записи
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      
    /* ДЕРЕВО */
    public class СкладськіКомірки_Папки_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc.png" : "folder.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Назва };
        }

        public static TreeStore Store = new TreeStore
        (
            typeof(Gdk.Pixbuf) /* Image */, 
            typeof(string)     /* ID */, 
            typeof(string)     /* Назва */
        );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static void LoadTree(UnigueID? openFolder, UnigueID? selectPointer
        , UnigueID? owner)
        {
            Store.Clear();
            RootPath = SelectPath = null;

            СкладськіКомірки_Папки_ЗаписиШвидкийВибір rootRecord = new СкладськіКомірки_Папки_ЗаписиШвидкийВибір
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {СкладськіКомірки_Папки_Const.Назва}, 
        {СкладськіКомірки_Папки_Const.Родич}, 
        1 AS level,
        deletion_label
    FROM {СкладськіКомірки_Папки_Const.TABLE}
    WHERE {СкладськіКомірки_Папки_Const.Родич} = '{Guid.Empty}'";

        
            if (owner != null && !owner.IsEmpty()) query += $@"
        AND {СкладськіКомірки_Папки_Const.Власник} = '{owner}'";
        

            if (openFolder != null) query += $@"
        AND uid != '{openFolder}'";

            query += $@"
    UNION ALL
    SELECT 
        {СкладськіКомірки_Папки_Const.TABLE}.uid, 
        {СкладськіКомірки_Папки_Const.TABLE}.{СкладськіКомірки_Папки_Const.Назва}, 
        {СкладськіКомірки_Папки_Const.TABLE}.{СкладськіКомірки_Папки_Const.Родич}, 
        r.level + 1 AS level,
        {СкладськіКомірки_Папки_Const.TABLE}.deletion_label 
    FROM {СкладськіКомірки_Папки_Const.TABLE}
        JOIN r ON {СкладськіКомірки_Папки_Const.TABLE}.{СкладськіКомірки_Папки_Const.Родич} = r.uid";

        
            if (owner != null && !owner.IsEmpty()) query += $@"
        AND {СкладськіКомірки_Папки_Const.Власник} = '{owner}'";
        

            if (openFolder != null) query += $@"
    WHERE {СкладськіКомірки_Папки_Const.TABLE}.uid != '{openFolder}'";

            query += $@"
)
SELECT 
    uid, 
    {СкладськіКомірки_Папки_Const.Назва}, 
    {СкладськіКомірки_Папки_Const.Родич}, 
    level,
    deletion_label
FROM r
ORDER BY level, {СкладськіКомірки_Папки_Const.Назва} ASC
";

            #endregion

            string[] columnsName;
            List<object[]>? listRow = null;

            Config.Kernel?.DataBase.SelectRequest(query, null, out columnsName, out listRow);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            if (listRow != null)
                foreach (object[] row in listRow)
                {
                    string uid = row[0]?.ToString() ?? Guid.Empty.ToString();
                    string fieldName = (row[1]?.ToString() ?? "");
                    string fieldParent = row[2]?.ToString() ?? Guid.Empty.ToString();
                    int level = (int)row[3];
                    bool deletionLabel = (bool)row[4];

                    СкладськіКомірки_Папки_ЗаписиШвидкийВибір record = new СкладськіКомірки_Папки_ЗаписиШвидкийВибір
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      
    #endregion
    
    #region DIRECTORY "Блокнот"
    
      
    /* ТАБЛИЦЯ */
    public class Блокнот_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ДатаЗапису = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Код, Назва, ДатаЗапису };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* ДатаЗапису */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ДатаЗапису*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Блокнот_Select Блокнот_Select = new Довідники.Блокнот_Select();
            Блокнот_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Блокнот_Const.Код /* 1 */
                    , Довідники.Блокнот_Const.Назва /* 2 */
                    , Довідники.Блокнот_Const.ДатаЗапису /* 3 */
                    
                });

            /* Where */
            Блокнот_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Блокнот_Select.QuerySelect.Order.Add(Довідники.Блокнот_Const.ДатаЗапису, SelectOrder.ASC);
            

            /* SELECT */
            await Блокнот_Select.Select();
            while (Блокнот_Select.MoveNext())
            {
                Довідники.Блокнот_Pointer? cur = Блокнот_Select.Current;

                if (cur != null)
                {
                    Блокнот_Записи Record = new Блокнот_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Блокнот_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Блокнот_Const.Назва]?.ToString() ?? "", /**/
                        ДатаЗапису = cur.Fields?[Блокнот_Const.ДатаЗапису]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    /* ТАБЛИЦЯ */
    public class Блокнот_ЗаписиШвидкийВибір
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ДатаЗапису = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ , Код, Назва, ДатаЗапису };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* ДатаЗапису */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ДатаЗапису*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Блокнот_Select Блокнот_Select = new Довідники.Блокнот_Select();
            Блокнот_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    , Довідники.Блокнот_Const.Код /* 1 */
                    , Довідники.Блокнот_Const.Назва /* 2 */
                    , Довідники.Блокнот_Const.ДатаЗапису /* 3 */
                    
                });

            /* Where */
            Блокнот_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              Блокнот_Select.QuerySelect.Order.Add(Довідники.Блокнот_Const.ДатаЗапису, SelectOrder.ASC);
            

            /* SELECT */
            await Блокнот_Select.Select();
            while (Блокнот_Select.MoveNext())
            {
                Довідники.Блокнот_Pointer? cur = Блокнот_Select.Current;

                if (cur != null)
                {
                    Блокнот_ЗаписиШвидкийВибір Record = new Блокнот_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Код = cur.Fields?[Блокнот_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Блокнот_Const.Назва]?.ToString() ?? "", /**/
                        ДатаЗапису = cur.Fields?[Блокнот_Const.ДатаЗапису]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

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

            /*сomboBox.Active = 0;*/

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
                case Перелічення.ТипПеріодуДляЖурналівДокументів.Місяць:
                {
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, DateTime.Now.AddMonths(-1)));
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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Склад, Валюта, СумаДокументу, Автор, Коментар };
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
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ЗамовленняПостачальнику_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ЗамовленняПостачальнику_Select ЗамовленняПостачальнику_Select = new Документи.ЗамовленняПостачальнику_Select();
            ЗамовленняПостачальнику_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ЗамовленняПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Автор, ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  ЗамовленняПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "join_tab_5_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ЗамовленняПостачальнику_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ЗамовленняПостачальнику_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ЗамовленняПостачальнику_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ЗамовленняПостачальнику_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ЗамовленняПостачальнику_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Контрагент, Валюта, Каса, СумаДокументу, Автор, Коментар };
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
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПоступленняТоварівТаПослуг_Select ПоступленняТоварівТаПослуг_Select = new Документи.ПоступленняТоварівТаПослуг_Select();
            ПоступленняТоварівТаПослуг_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Автор, ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_6"));
                
                  /* Field */
                  ПоступленняТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "join_tab_6_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_6_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПоступленняТоварівТаПослуг_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПоступленняТоварівТаПослуг_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПоступленняТоварівТаПослуг_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПоступленняТоварівТаПослуг_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПоступленняТоварівТаПослуг_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Автор, Коментар };
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
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ЗамовленняКлієнта_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ЗамовленняКлієнта_Select ЗамовленняКлієнта_Select = new Документи.ЗамовленняКлієнта_Select();
            ЗамовленняКлієнта_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ЗамовленняКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Автор, ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_6"));
                
                  /* Field */
                  ЗамовленняКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "join_tab_6_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_6_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ЗамовленняКлієнта_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ЗамовленняКлієнта_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ЗамовленняКлієнта_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ЗамовленняКлієнта_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ЗамовленняКлієнта_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Автор, Коментар };
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
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РеалізаціяТоварівТаПослуг_Select РеалізаціяТоварівТаПослуг_Select = new Документи.РеалізаціяТоварівТаПослуг_Select();
            РеалізаціяТоварівТаПослуг_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Автор, РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_6"));
                
                  /* Field */
                  РеалізаціяТоварівТаПослуг_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "join_tab_6_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_6_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[РеалізаціяТоварівТаПослуг_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Валюта = "";
        string ВидЦіни = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, ВидЦіни, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Валюта */
            , typeof(string) /* ВидЦіни */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Вид ціни", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*ВидЦіни*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ВстановленняЦінНоменклатури_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ВстановленняЦінНоменклатури_Select ВстановленняЦінНоменклатури_Select = new Документи.ВстановленняЦінНоменклатури_Select();
            ВстановленняЦінНоменклатури_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ВстановленняЦінНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.Автор, ВстановленняЦінНоменклатури_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ВстановленняЦінНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Користувачі_Const.Назва, "join_tab_4_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        ВидЦіни = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВстановленняЦінНоменклатури_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ВстановленняЦінНоменклатури_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ВстановленняЦінНоменклатури_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ВстановленняЦінНоменклатури_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
        string ГосподарськаОперація = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, Каса, Контрагент, СумаДокументу, ГосподарськаОперація, Автор, Коментар };
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
            , typeof(string) /* ГосподарськаОперація */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Операція", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*ГосподарськаОперація*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПрихіднийКасовийОрдер_Select ПрихіднийКасовийОрдер_Select = new Документи.ПрихіднийКасовийОрдер_Select();
            ПрихіднийКасовийОрдер_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
                    , Документи.ПрихіднийКасовийОрдер_Const.Назва /* 1 */
                    , Документи.ПрихіднийКасовийОрдер_Const.НомерДок /* 2 */
                    , Документи.ПрихіднийКасовийОрдер_Const.ДатаДок /* 3 */
                    , Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу /* 4 */
                    , Документи.ПрихіднийКасовийОрдер_Const.ГосподарськаОперація /* 5 */
                    , Документи.ПрихіднийКасовийОрдер_Const.Коментар /* 6 */
                    
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
                    new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Контрагент, ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Контрагенти_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                ПрихіднийКасовийОрдер_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Автор, ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  ПрихіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "join_tab_5_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПрихіднийКасовийОрдер_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПрихіднийКасовийОрдер_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПрихіднийКасовийОрдер_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПрихіднийКасовийОрдер_Const.СумаДокументу]?.ToString() ?? "", /**/
                        ГосподарськаОперація = Перелічення.ПсевдонімиПерелічення.ГосподарськіОперації_Alias( ((Перелічення.ГосподарськіОперації)(cur.Fields?[ПрихіднийКасовийОрдер_Const.ГосподарськаОперація]! != DBNull.Value ? cur.Fields?[ПрихіднийКасовийОрдер_Const.ГосподарськаОперація]! : 0)) ), /**/
                        Коментар = cur.Fields?[ПрихіднийКасовийОрдер_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Контрагент = "";
        string Валюта = "";
        string Каса = "";
        string СумаДокументу = "";
        string ГосподарськаОперація = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, СумаДокументу, ГосподарськаОперація, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Каса */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* ГосподарськаОперація */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Операція", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*ГосподарськаОперація*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.РозхіднийКасовийОрдер_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РозхіднийКасовийОрдер_Select РозхіднийКасовийОрдер_Select = new Документи.РозхіднийКасовийОрдер_Select();
            РозхіднийКасовийОрдер_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
                    , Документи.РозхіднийКасовийОрдер_Const.Назва /* 1 */
                    , Документи.РозхіднийКасовийОрдер_Const.НомерДок /* 2 */
                    , Документи.РозхіднийКасовийОрдер_Const.ДатаДок /* 3 */
                    , Документи.РозхіднийКасовийОрдер_Const.СумаДокументу /* 4 */
                    , Документи.РозхіднийКасовийОрдер_Const.ГосподарськаОперація /* 5 */
                    , Документи.РозхіднийКасовийОрдер_Const.Коментар /* 6 */
                    
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
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Автор, РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  РозхіднийКасовийОрдер_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "join_tab_5_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[РозхіднийКасовийОрдер_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[РозхіднийКасовийОрдер_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[РозхіднийКасовийОрдер_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[РозхіднийКасовийОрдер_Const.СумаДокументу]?.ToString() ?? "", /**/
                        ГосподарськаОперація = Перелічення.ПсевдонімиПерелічення.ГосподарськіОперації_Alias( ((Перелічення.ГосподарськіОперації)(cur.Fields?[РозхіднийКасовийОрдер_Const.ГосподарськаОперація]! != DBNull.Value ? cur.Fields?[РозхіднийКасовийОрдер_Const.ГосподарськаОперація]! : 0)) ), /**/
                        Коментар = cur.Fields?[РозхіднийКасовийОрдер_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string СкладВідправник = "";
        string СкладОтримувач = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, СкладВідправник, СкладОтримувач, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* СкладВідправник */
            , typeof(string) /* СкладОтримувач */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад відправник", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*СкладВідправник*/
            treeView.AppendColumn(new TreeViewColumn("Склад отримувач", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*СкладОтримувач*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПереміщенняТоварів_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПереміщенняТоварів_Select ПереміщенняТоварів_Select = new Документи.ПереміщенняТоварів_Select();
            ПереміщенняТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ПереміщенняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПереміщенняТоварів_Const.Автор, ПереміщенняТоварів_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ПереміщенняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Користувачі_Const.Назва, "join_tab_4_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        СкладВідправник = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        СкладОтримувач = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПереміщенняТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПереміщенняТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПереміщенняТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПереміщенняТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Автор, Коментар };
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
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПоверненняТоварівПостачальнику_Select ПоверненняТоварівПостачальнику_Select = new Документи.ПоверненняТоварівПостачальнику_Select();
            ПоверненняТоварівПостачальнику_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Автор, ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_6"));
                
                  /* Field */
                  ПоверненняТоварівПостачальнику_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "join_tab_6_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_6_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПоверненняТоварівПостачальнику_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПоверненняТоварівПостачальнику_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПоверненняТоварівПостачальнику_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПоверненняТоварівПостачальнику_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПоверненняТоварівПостачальнику_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, Каса, Контрагент, Склад, СумаДокументу, Автор, Коментар };
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
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПоверненняТоварівВідКлієнта_Select ПоверненняТоварівВідКлієнта_Select = new Документи.ПоверненняТоварівВідКлієнта_Select();
            ПоверненняТоварівВідКлієнта_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Автор, ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_6"));
                
                  /* Field */
                  ПоверненняТоварівВідКлієнта_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "join_tab_6_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_6_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПоверненняТоварівВідКлієнта_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Валюта, Каса, Контрагент, СумаДокументу, Автор, Коментар };
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
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.АктВиконанихРобіт_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.АктВиконанихРобіт_Select АктВиконанихРобіт_Select = new Документи.АктВиконанихРобіт_Select();
            АктВиконанихРобіт_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                АктВиконанихРобіт_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.АктВиконанихРобіт_Const.Автор, АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  АктВиконанихРобіт_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "join_tab_5_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[АктВиконанихРобіт_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[АктВиконанихРобіт_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[АктВиконанихРобіт_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[АктВиконанихРобіт_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[АктВиконанихРобіт_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Контрагент = "";
        string Валюта = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Контрагент, Валюта, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Валюта */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("НомерДок", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("ДатаДок", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ВведенняЗалишків_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ВведенняЗалишків_Select ВведенняЗалишків_Select = new Документи.ВведенняЗалишків_Select();
            ВведенняЗалишків_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ВведенняЗалишків_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ВведенняЗалишків_Const.Автор, ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  ВведенняЗалишків_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "join_tab_5_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВведенняЗалишків_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ВведенняЗалишків_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ВведенняЗалишків_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ВведенняЗалишків_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.НадлишкиТоварів_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.НадлишкиТоварів_Select НадлишкиТоварів_Select = new Документи.НадлишкиТоварів_Select();
            НадлишкиТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                НадлишкиТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.НадлишкиТоварів_Const.Автор, НадлишкиТоварів_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  НадлишкиТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "join_tab_3_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[НадлишкиТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[НадлишкиТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[НадлишкиТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[НадлишкиТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("НомерДок", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("ДатаДок", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПересортицяТоварів_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПересортицяТоварів_Select ПересортицяТоварів_Select = new Документи.ПересортицяТоварів_Select();
            ПересортицяТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ПересортицяТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПересортицяТоварів_Const.Автор, ПересортицяТоварів_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ПересортицяТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "join_tab_3_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПересортицяТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПересортицяТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПересортицяТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПересортицяТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Склад = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Склад, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Склад */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПерерахунокТоварів_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПерерахунокТоварів_Select ПерерахунокТоварів_Select = new Документи.ПерерахунокТоварів_Select();
            ПерерахунокТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ПерерахунокТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПерерахунокТоварів_Const.Автор, ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПерерахунокТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Користувачі_Const.Назва, "join_tab_2_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Склад = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПерерахунокТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПерерахунокТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПерерахунокТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПерерахунокТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string СумаДокументу = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, СумаДокументу, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПсуванняТоварів_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПсуванняТоварів_Select ПсуванняТоварів_Select = new Документи.ПсуванняТоварів_Select();
            ПсуванняТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ПсуванняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПсуванняТоварів_Const.Автор, ПсуванняТоварів_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ПсуванняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "join_tab_3_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПсуванняТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПсуванняТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПсуванняТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ПсуванняТоварів_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПсуванняТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Валюта = "";
        string СумаДокументу = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Склад, Валюта, СумаДокументу, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* НомерДок */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Валюта */
            , typeof(string) /* СумаДокументу */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ВнутрішнєСпоживанняТоварів_Select ВнутрішнєСпоживанняТоварів_Select = new Документи.ВнутрішнєСпоживанняТоварів_Select();
            ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Автор, ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ВнутрішнєСпоживанняТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Користувачі_Const.Назва, "join_tab_4_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ВнутрішнєСпоживанняТоварів_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
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
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, НомерДок, ДатаДок, Організація, Контрагент, Валюта, Каса, Склад, СумаДокументу, Автор, Коментар };
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
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*СумаДокументу*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.РахунокФактура_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РахунокФактура_Select РахунокФактура_Select = new Документи.РахунокФактура_Select();
            РахунокФактура_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
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
                  
                /* Join Table */
                РахунокФактура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.РахунокФактура_Const.Автор, РахунокФактура_Select.QuerySelect.Table, "join_tab_6"));
                
                  /* Field */
                  РахунокФактура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "join_tab_6_field_1"));
                  

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
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Контрагент = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Валюта = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Каса = cur.Fields?["join_tab_4_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_5_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_6_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[РахунокФактура_Const.Назва]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[РахунокФактура_Const.НомерДок]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[РахунокФактура_Const.ДатаДок]?.ToString() ?? "", /**/
                        СумаДокументу = cur.Fields?[РахунокФактура_Const.СумаДокументу]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[РахунокФактура_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "РозміщенняТоварівНаСкладі"
    
      
    public class РозміщенняТоварівНаСкладі_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string ДатаДок = "";
        string НомерДок = "";
        string Склад = "";
        string ДокументПоступлення = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, ДатаДок, НомерДок, Склад, ДокументПоступлення, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* НомерДок */
            , typeof(string) /* Склад */
            , typeof(string) /* ДокументПоступлення */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Документ поступлення", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*ДокументПоступлення*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РозміщенняТоварівНаСкладі_Select РозміщенняТоварівНаСкладі_Select = new Документи.РозміщенняТоварівНаСкладі_Select();
            РозміщенняТоварівНаСкладі_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
                    , Документи.РозміщенняТоварівНаСкладі_Const.Назва /* 1 */
                    , Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок /* 2 */
                    , Документи.РозміщенняТоварівНаСкладі_Const.НомерДок /* 3 */
                    , Документи.РозміщенняТоварівНаСкладі_Const.Коментар /* 4 */
                    
                });

            /* Where */
            РозміщенняТоварівНаСкладі_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              РозміщенняТоварівНаСкладі_Select.QuerySelect.Order.Add(Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                РозміщенняТоварівНаСкладі_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.РозміщенняТоварівНаСкладі_Const.Склад, РозміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  РозміщенняТоварівНаСкладі_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Склади_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                РозміщенняТоварівНаСкладі_Select.QuerySelect.Joins.Add(
                    new Join(Документи.ПоступленняТоварівТаПослуг_Const.TABLE, Документи.РозміщенняТоварівНаСкладі_Const.ДокументПоступлення, РозміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  РозміщенняТоварівНаСкладі_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Документи.ПоступленняТоварівТаПослуг_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                РозміщенняТоварівНаСкладі_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.РозміщенняТоварівНаСкладі_Const.Автор, РозміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  РозміщенняТоварівНаСкладі_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "join_tab_3_field_1"));
                  

            /* SELECT */
            РозміщенняТоварівНаСкладі_Select.Select();
            while (РозміщенняТоварівНаСкладі_Select.MoveNext())
            {
                Документи.РозміщенняТоварівНаСкладі_Pointer? cur = РозміщенняТоварівНаСкладі_Select.Current;

                if (cur != null)
                {
                    РозміщенняТоварівНаСкладі_Записи Record = new РозміщенняТоварівНаСкладі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Склад = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        ДокументПоступлення = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[РозміщенняТоварівНаСкладі_Const.Назва]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[РозміщенняТоварівНаСкладі_Const.ДатаДок]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[РозміщенняТоварівНаСкладі_Const.НомерДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[РозміщенняТоварівНаСкладі_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ПереміщенняТоварівНаСкладі"
    
      
    public class ПереміщенняТоварівНаСкладі_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string ДатаДок = "";
        string НомерДок = "";
        string Склад = "";
        string Організація = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, ДатаДок, НомерДок, Склад, Організація, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* НомерДок */
            , typeof(string) /* Склад */
            , typeof(string) /* Організація */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПереміщенняТоварівНаСкладі_Select ПереміщенняТоварівНаСкладі_Select = new Документи.ПереміщенняТоварівНаСкладі_Select();
            ПереміщенняТоварівНаСкладі_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
                    , Документи.ПереміщенняТоварівНаСкладі_Const.Назва /* 1 */
                    , Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок /* 2 */
                    , Документи.ПереміщенняТоварівНаСкладі_Const.НомерДок /* 3 */
                    , Документи.ПереміщенняТоварівНаСкладі_Const.Коментар /* 4 */
                    
                });

            /* Where */
            ПереміщенняТоварівНаСкладі_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ПереміщенняТоварівНаСкладі_Select.QuerySelect.Order.Add(Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПереміщенняТоварівНаСкладі_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварівНаСкладі_Const.Склад, ПереміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПереміщенняТоварівНаСкладі_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Склади_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПереміщенняТоварівНаСкладі_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПереміщенняТоварівНаСкладі_Const.Організація, ПереміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПереміщенняТоварівНаСкладі_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Організації_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ПереміщенняТоварівНаСкладі_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПереміщенняТоварівНаСкладі_Const.Автор, ПереміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ПереміщенняТоварівНаСкладі_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "join_tab_3_field_1"));
                  

            /* SELECT */
            ПереміщенняТоварівНаСкладі_Select.Select();
            while (ПереміщенняТоварівНаСкладі_Select.MoveNext())
            {
                Документи.ПереміщенняТоварівНаСкладі_Pointer? cur = ПереміщенняТоварівНаСкладі_Select.Current;

                if (cur != null)
                {
                    ПереміщенняТоварівНаСкладі_Записи Record = new ПереміщенняТоварівНаСкладі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Склад = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Організація = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ПереміщенняТоварівНаСкладі_Const.Назва]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ПереміщенняТоварівНаСкладі_Const.ДатаДок]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ПереміщенняТоварівНаСкладі_Const.НомерДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ПереміщенняТоварівНаСкладі_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ЗбіркаТоварівНаСкладі"
    
      
    public class ЗбіркаТоварівНаСкладі_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string ДатаДок = "";
        string НомерДок = "";
        string Склад = "";
        string ДокументРеалізації = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, ДатаДок, НомерДок, Склад, ДокументРеалізації, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* НомерДок */
            , typeof(string) /* Склад */
            , typeof(string) /* ДокументРеалізації */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Документ реалізації", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*ДокументРеалізації*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ЗбіркаТоварівНаСкладі_Select ЗбіркаТоварівНаСкладі_Select = new Документи.ЗбіркаТоварівНаСкладі_Select();
            ЗбіркаТоварівНаСкладі_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
                    , Документи.ЗбіркаТоварівНаСкладі_Const.Назва /* 1 */
                    , Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок /* 2 */
                    , Документи.ЗбіркаТоварівНаСкладі_Const.НомерДок /* 3 */
                    , Документи.ЗбіркаТоварівНаСкладі_Const.Коментар /* 4 */
                    
                });

            /* Where */
            ЗбіркаТоварівНаСкладі_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              ЗбіркаТоварівНаСкладі_Select.QuerySelect.Order.Add(Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ЗбіркаТоварівНаСкладі_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ЗбіркаТоварівНаСкладі_Const.Склад, ЗбіркаТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ЗбіркаТоварівНаСкладі_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Склади_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ЗбіркаТоварівНаСкладі_Select.QuerySelect.Joins.Add(
                    new Join(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE, Документи.ЗбіркаТоварівНаСкладі_Const.ДокументРеалізації, ЗбіркаТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ЗбіркаТоварівНаСкладі_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Документи.РеалізаціяТоварівТаПослуг_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ЗбіркаТоварівНаСкладі_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ЗбіркаТоварівНаСкладі_Const.Автор, ЗбіркаТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ЗбіркаТоварівНаСкладі_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "join_tab_3_field_1"));
                  

            /* SELECT */
            ЗбіркаТоварівНаСкладі_Select.Select();
            while (ЗбіркаТоварівНаСкладі_Select.MoveNext())
            {
                Документи.ЗбіркаТоварівНаСкладі_Pointer? cur = ЗбіркаТоварівНаСкладі_Select.Current;

                if (cur != null)
                {
                    ЗбіркаТоварівНаСкладі_Записи Record = new ЗбіркаТоварівНаСкладі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Склад = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        ДокументРеалізації = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[ЗбіркаТоварівНаСкладі_Const.Назва]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[ЗбіркаТоварівНаСкладі_Const.ДатаДок]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[ЗбіркаТоварівНаСкладі_Const.НомерДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[ЗбіркаТоварівНаСкладі_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    
    #region DOCUMENT "РозміщенняНоменклатуриПоКоміркам"
    
      
    public class РозміщенняНоменклатуриПоКоміркам_Записи
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string ДатаДок = "";
        string НомерДок = "";
        string Організація = "";
        string Склад = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ , Назва, ДатаДок, НомерДок, Організація, Склад, Автор, Коментар };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* ДатаДок */
            , typeof(string) /* НомерДок */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РозміщенняНоменклатуриПоКоміркам_Select РозміщенняНоменклатуриПоКоміркам_Select = new Документи.РозміщенняНоменклатуриПоКоміркам_Select();
            РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
                    , Документи.РозміщенняНоменклатуриПоКоміркам_Const.Назва /* 1 */
                    , Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок /* 2 */
                    , Документи.РозміщенняНоменклатуриПоКоміркам_Const.НомерДок /* 3 */
                    , Документи.РозміщенняНоменклатуриПоКоміркам_Const.Коментар /* 4 */
                    
                });

            /* Where */
            РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Where = Where;

            
              /* ORDER */
              РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Order.Add(Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Організація, РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Склад, РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Автор, РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "join_tab_3_field_1"));
                  

            /* SELECT */
            РозміщенняНоменклатуриПоКоміркам_Select.Select();
            while (РозміщенняНоменклатуриПоКоміркам_Select.MoveNext())
            {
                Документи.РозміщенняНоменклатуриПоКоміркам_Pointer? cur = РозміщенняНоменклатуриПоКоміркам_Select.Current;

                if (cur != null)
                {
                    РозміщенняНоменклатуриПоКоміркам_Записи Record = new РозміщенняНоменклатуриПоКоміркам_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        Організація = cur.Fields?["join_tab_1_field_1"]?.ToString() ?? "", /**/
                        Склад = cur.Fields?["join_tab_2_field_1"]?.ToString() ?? "", /**/
                        Автор = cur.Fields?["join_tab_3_field_1"]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[РозміщенняНоменклатуриПоКоміркам_Const.Назва]?.ToString() ?? "", /**/
                        ДатаДок = cur.Fields?[РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок]?.ToString() ?? "", /**/
                        НомерДок = cur.Fields?[РозміщенняНоменклатуриПоКоміркам_Const.НомерДок]?.ToString() ?? "", /**/
                        Коментар = cur.Fields?[РозміщенняНоменклатуриПоКоміркам_Const.Коментар]?.ToString() ?? "" /**/
                        
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    
    #endregion
    

    //
    // Журнали
    //

    
    #region JOURNAL "Повний"
    
    public class Журнали_Повний
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        
        string Назва = "";
        string Дата = "";
        string Номер = "";
        string Організація = "";
        string Контрагент = "";
        string Склад = "";
        string Каса = "";
        string Валюта = "";
        string Сума = "";
        string Автор = "";
        string Коментар = "";

        // Масив для запису стрічки в Store
        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Type, Spend /*Проведений документ*/
            /* */ , Назва, Дата, Номер, Організація, Контрагент, Склад, Каса, Валюта, Сума, Автор, Коментар };
        }

        // Джерело даних для списку
        public static ListStore Store = new ListStore(
          typeof(Gdk.Pixbuf) /* Image */, 
          typeof(string) /* ID */, 
          typeof(string) /* Type */, 
          typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* Дата */
            , typeof(string) /* Номер */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Склад */
            , typeof(string) /* Каса */
            , typeof(string) /* Валюта */
            , typeof(string) /* Сума */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("Type", new CellRendererText(), "text", 2) { Visible = false }); /*Type*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 3)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*Дата*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Номер*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Сума*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 14) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        // Словник з відборами, ключ це Тип документу
        public static Dictionary<string, List<Where>> Where { get; set; } = new Dictionary<string, List<Where>>();

        // Добавляє відбір по періоду в словник відборів
        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            
            {
                List<Where> where = new List<Where>();
                Where.Add("ЗамовленняПостачальнику", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ЗамовленняПостачальнику_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПоступленняТоварівТаПослуг", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПоступленняТоварівТаПослуг_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ЗамовленняКлієнта", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ЗамовленняКлієнта_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("РеалізаціяТоварівТаПослуг", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РеалізаціяТоварівТаПослуг_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ВстановленняЦінНоменклатури", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ВстановленняЦінНоменклатури_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПрихіднийКасовийОрдер", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПрихіднийКасовийОрдер_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("РозхіднийКасовийОрдер", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РозхіднийКасовийОрдер_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПереміщенняТоварів", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПереміщенняТоварів_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПоверненняТоварівПостачальнику", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПоверненняТоварівПостачальнику_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПоверненняТоварівВідКлієнта", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПоверненняТоварівВідКлієнта_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("АктВиконанихРобіт", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, АктВиконанихРобіт_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ВведенняЗалишків", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ВведенняЗалишків_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПсуванняТоварів", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПсуванняТоварів_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ВнутрішнєСпоживанняТоварів", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ВнутрішнєСпоживанняТоварів_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("РахунокФактура", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РахунокФактура_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("РозміщенняТоварівНаСкладі", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РозміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПереміщенняТоварівНаСкладі", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПереміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ЗбіркаТоварівНаСкладі", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ЗбіркаТоварівНаСкладі_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("РозміщенняНоменклатуриПоКоміркам", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, типПеріоду);
            }
              
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            Dictionary<string, string> allowDoc = new Dictionary<string, string>();
            allowDoc.Add("ЗамовленняПостачальнику", "Замовлення постачальнику");
            allowDoc.Add("ПоступленняТоварівТаПослуг", "Поступлення товарів та послуг");
            allowDoc.Add("ЗамовленняКлієнта", "Замовлення клієнта");
            allowDoc.Add("РеалізаціяТоварівТаПослуг", "Реалізація товарів та послуг");
            allowDoc.Add("ВстановленняЦінНоменклатури", "Встановлення цін номенклатури");
            allowDoc.Add("ПрихіднийКасовийОрдер", "Прихідний касовий ордер");
            allowDoc.Add("РозхіднийКасовийОрдер", "Розхідний касовий ордер");
            allowDoc.Add("ПереміщенняТоварів", "Переміщення товарів");
            allowDoc.Add("ПоверненняТоварівПостачальнику", "Повернення товарів постачальнику");
            allowDoc.Add("ПоверненняТоварівВідКлієнта", "Повернення товарів від клієнта");
            allowDoc.Add("АктВиконанихРобіт", "Акт виконаних робіт");
            allowDoc.Add("ВведенняЗалишків", "Введення залишків");
            allowDoc.Add("ПсуванняТоварів", "Псування товарів");
            allowDoc.Add("ВнутрішнєСпоживанняТоварів", "Внутрішнє споживання товарів");
            allowDoc.Add("РахунокФактура", "Рахунок фактура");
            allowDoc.Add("РозміщенняТоварівНаСкладі", "Розміщення товарів на складі");
            allowDoc.Add("ПереміщенняТоварівНаСкладі", "Переміщення товарів на складі");
            allowDoc.Add("ЗбіркаТоварівНаСкладі", "Збірка товарів на складі");
            allowDoc.Add("РозміщенняНоменклатуриПоКоміркам", "Розміщення номенклатури по коміркам");
            
            return allowDoc;
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;
            List<string> allQuery = new List<string>();
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

          
              {
                  Query query = new Query(Документи.ЗамовленняПостачальнику_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ЗамовленняПостачальнику") && Where["ЗамовленняПостачальнику"].Count != 0) {
                      query.Where = Where["ЗамовленняПостачальнику"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ЗамовленняПостачальнику'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПоступленняТоварівТаПослуг_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПоступленняТоварівТаПослуг") && Where["ПоступленняТоварівТаПослуг"].Count != 0) {
                      query.Where = Where["ПоступленняТоварівТаПослуг"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПоступленняТоварівТаПослуг'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ЗамовленняКлієнта_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ЗамовленняКлієнта") && Where["ЗамовленняКлієнта"].Count != 0) {
                      query.Where = Where["ЗамовленняКлієнта"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ЗамовленняКлієнта'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РеалізаціяТоварівТаПослуг") && Where["РеалізаціяТоварівТаПослуг"].Count != 0) {
                      query.Where = Where["РеалізаціяТоварівТаПослуг"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РеалізаціяТоварівТаПослуг'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ВстановленняЦінНоменклатури_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ВстановленняЦінНоменклатури") && Where["ВстановленняЦінНоменклатури"].Count != 0) {
                      query.Where = Where["ВстановленняЦінНоменклатури"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ВстановленняЦінНоменклатури'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВстановленняЦінНоменклатури_Const.TABLE + "." + Документи.ВстановленняЦінНоменклатури_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВстановленняЦінНоменклатури_Const.TABLE + "." + Документи.ВстановленняЦінНоменклатури_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВстановленняЦінНоменклатури_Const.TABLE + "." + Документи.ВстановленняЦінНоменклатури_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.Валюта, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ВстановленняЦінНоменклатури_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВстановленняЦінНоменклатури_Const.TABLE + "." + Документи.ВстановленняЦінНоменклатури_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПрихіднийКасовийОрдер_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПрихіднийКасовийОрдер") && Where["ПрихіднийКасовийОрдер"].Count != 0) {
                      query.Where = Where["ПрихіднийКасовийОрдер"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПрихіднийКасовийОрдер'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Каса, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Валюта, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Автор, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.РозхіднийКасовийОрдер_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РозхіднийКасовийОрдер") && Where["РозхіднийКасовийОрдер"].Count != 0) {
                      query.Where = Where["РозхіднийКасовийОрдер"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РозхіднийКасовийОрдер'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Каса, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Валюта, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Автор, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПереміщенняТоварів_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПереміщенняТоварів") && Where["ПереміщенняТоварів"].Count != 0) {
                      query.Where = Where["ПереміщенняТоварів"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПереміщенняТоварів'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПереміщенняТоварів_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварів_Const.СкладВідправник, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПереміщенняТоварів_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПоверненняТоварівПостачальнику_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПоверненняТоварівПостачальнику") && Where["ПоверненняТоварівПостачальнику"].Count != 0) {
                      query.Where = Where["ПоверненняТоварівПостачальнику"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПоверненняТоварівПостачальнику'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПоверненняТоварівВідКлієнта") && Where["ПоверненняТоварівВідКлієнта"].Count != 0) {
                      query.Where = Where["ПоверненняТоварівВідКлієнта"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПоверненняТоварівВідКлієнта'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.АктВиконанихРобіт_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("АктВиконанихРобіт") && Where["АктВиконанихРобіт"].Count != 0) {
                      query.Where = Where["АктВиконанихРобіт"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'АктВиконанихРобіт'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.АктВиконанихРобіт_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.АктВиконанихРобіт_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.АктВиконанихРобіт_Const.Каса, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.АктВиконанихРобіт_Const.Валюта, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.АктВиконанихРобіт_Const.Автор, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ВведенняЗалишків_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ВведенняЗалишків") && Where["ВведенняЗалишків"].Count != 0) {
                      query.Where = Where["ВведенняЗалишків"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ВведенняЗалишків'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ВведенняЗалишків_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ВведенняЗалишків_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ВведенняЗалишків_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ВведенняЗалишків_Const.Валюта, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ВведенняЗалишків_Const.Автор, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПсуванняТоварів_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПсуванняТоварів") && Where["ПсуванняТоварів"].Count != 0) {
                      query.Where = Where["ПсуванняТоварів"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПсуванняТоварів'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПсуванняТоварів_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПсуванняТоварів_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта"));
                        
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПсуванняТоварів_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ВнутрішнєСпоживанняТоварів") && Where["ВнутрішнєСпоживанняТоварів"].Count != 0) {
                      query.Where = Where["ВнутрішнєСпоживанняТоварів"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ВнутрішнєСпоживанняТоварів'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Валюта, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Автор, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.РахунокФактура_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РахунокФактура") && Where["РахунокФактура"].Count != 0) {
                      query.Where = Where["РахунокФактура"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РахунокФактура'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РахунокФактура_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.РахунокФактура_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.РахунокФактура_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.РахунокФактура_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.РахунокФактура_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.СумаДокументу + "::text", "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РахунокФактура_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.РозміщенняТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РозміщенняТоварівНаСкладі") && Where["РозміщенняТоварівНаСкладі"].Count != 0) {
                      query.Where = Where["РозміщенняТоварівНаСкладі"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РозміщенняТоварівНаСкладі'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РозміщенняТоварівНаСкладі_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.РозміщенняТоварівНаСкладі_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РозміщенняТоварівНаСкладі_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПереміщенняТоварівНаСкладі") && Where["ПереміщенняТоварівНаСкладі"].Count != 0) {
                      query.Where = Where["ПереміщенняТоварівНаСкладі"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПереміщенняТоварівНаСкладі'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПереміщенняТоварівНаСкладі_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварівНаСкладі_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПереміщенняТоварівНаСкладі_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ЗбіркаТоварівНаСкладі") && Where["ЗбіркаТоварівНаСкладі"].Count != 0) {
                      query.Where = Where["ЗбіркаТоварівНаСкладі"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ЗбіркаТоварівНаСкладі'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ЗбіркаТоварівНаСкладі_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ЗбіркаТоварівНаСкладі_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ЗбіркаТоварівНаСкладі_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РозміщенняНоменклатуриПоКоміркам") && Where["РозміщенняНоменклатуриПоКоміркам"].Count != 0) {
                      query.Where = Where["РозміщенняНоменклатуриПоКоміркам"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РозміщенняНоменклатуриПоКоміркам'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            unionAllQuery += "\nORDER BY Дата";
          
            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(unionAllQuery, paramQuery, out columnsName, out listRow);

            foreach (Dictionary<string, object> row in listRow)
            {
                Журнали_Повний record = new Журнали_Повний();
                record.ID = row["uid"]?.ToString() ?? "";
                record.Type = row["type"]?.ToString() ?? "";
                record.DeletionLabel = (bool)row["deletion_label"];
                record.Spend = (bool)row["spend"];
                
                    record.Назва = row["Назва"] != DBNull.Value ? (row["Назва"]?.ToString() ?? "") : "";
                
                    record.Дата = row["Дата"] != DBNull.Value ? (row["Дата"]?.ToString() ?? "") : "";
                
                    record.Номер = row["Номер"] != DBNull.Value ? (row["Номер"]?.ToString() ?? "") : "";
                
                    record.Організація = row["Організація"] != DBNull.Value ? (row["Організація"]?.ToString() ?? "") : "";
                
                    record.Контрагент = row["Контрагент"] != DBNull.Value ? (row["Контрагент"]?.ToString() ?? "") : "";
                
                    record.Склад = row["Склад"] != DBNull.Value ? (row["Склад"]?.ToString() ?? "") : "";
                
                    record.Каса = row["Каса"] != DBNull.Value ? (row["Каса"]?.ToString() ?? "") : "";
                
                    record.Валюта = row["Валюта"] != DBNull.Value ? (row["Валюта"]?.ToString() ?? "") : "";
                
                    record.Сума = row["Сума"] != DBNull.Value ? (row["Сума"]?.ToString() ?? "") : "";
                
                    record.Автор = row["Автор"] != DBNull.Value ? (row["Автор"]?.ToString() ?? "") : "";
                
                    record.Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"]?.ToString() ?? "") : "";
                

                TreeIter CurrentIter = Store.AppendValues(record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
          
        }
    }
    #endregion
    
    #region JOURNAL "Закупівлі"
    
    public class Журнали_Закупівлі
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        
        string Назва = "";
        string Дата = "";
        string Номер = "";
        string Організація = "";
        string Контрагент = "";
        string Склад = "";
        string Каса = "";
        string Валюта = "";
        string Сума = "";
        string Автор = "";
        string Коментар = "";

        // Масив для запису стрічки в Store
        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Type, Spend /*Проведений документ*/
            /* */ , Назва, Дата, Номер, Організація, Контрагент, Склад, Каса, Валюта, Сума, Автор, Коментар };
        }

        // Джерело даних для списку
        public static ListStore Store = new ListStore(
          typeof(Gdk.Pixbuf) /* Image */, 
          typeof(string) /* ID */, 
          typeof(string) /* Type */, 
          typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* Дата */
            , typeof(string) /* Номер */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Склад */
            , typeof(string) /* Каса */
            , typeof(string) /* Валюта */
            , typeof(string) /* Сума */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("Type", new CellRendererText(), "text", 2) { Visible = false }); /*Type*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 3)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*Дата*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Номер*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Сума*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 14) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        // Словник з відборами, ключ це Тип документу
        public static Dictionary<string, List<Where>> Where { get; set; } = new Dictionary<string, List<Where>>();

        // Добавляє відбір по періоду в словник відборів
        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            
            {
                List<Where> where = new List<Where>();
                Where.Add("ЗамовленняПостачальнику", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ЗамовленняПостачальнику_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПоступленняТоварівТаПослуг", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПоступленняТоварівТаПослуг_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПоверненняТоварівПостачальнику", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПоверненняТоварівПостачальнику_Const.ДатаДок, типПеріоду);
            }
              
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            Dictionary<string, string> allowDoc = new Dictionary<string, string>();
            allowDoc.Add("ЗамовленняПостачальнику", "Замовлення постачальнику");
            allowDoc.Add("ПоступленняТоварівТаПослуг", "Поступлення товарів та послуг");
            allowDoc.Add("ПоверненняТоварівПостачальнику", "Повернення товарів постачальнику");
            
            return allowDoc;
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;
            List<string> allQuery = new List<string>();
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

          
              {
                  Query query = new Query(Документи.ЗамовленняПостачальнику_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ЗамовленняПостачальнику") && Where["ЗамовленняПостачальнику"].Count != 0) {
                      query.Where = Where["ЗамовленняПостачальнику"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ЗамовленняПостачальнику'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ЗамовленняПостачальнику_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПоступленняТоварівТаПослуг_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПоступленняТоварівТаПослуг") && Where["ПоступленняТоварівТаПослуг"].Count != 0) {
                      query.Where = Where["ПоступленняТоварівТаПослуг"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПоступленняТоварівТаПослуг'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПоступленняТоварівТаПослуг_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПоверненняТоварівПостачальнику_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПоверненняТоварівПостачальнику") && Where["ПоверненняТоварівПостачальнику"].Count != 0) {
                      query.Where = Where["ПоверненняТоварівПостачальнику"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПоверненняТоварівПостачальнику'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПоверненняТоварівПостачальнику_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            unionAllQuery += "\nORDER BY Дата";
          
            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(unionAllQuery, paramQuery, out columnsName, out listRow);

            foreach (Dictionary<string, object> row in listRow)
            {
                Журнали_Закупівлі record = new Журнали_Закупівлі();
                record.ID = row["uid"]?.ToString() ?? "";
                record.Type = row["type"]?.ToString() ?? "";
                record.DeletionLabel = (bool)row["deletion_label"];
                record.Spend = (bool)row["spend"];
                
                    record.Назва = row["Назва"] != DBNull.Value ? (row["Назва"]?.ToString() ?? "") : "";
                
                    record.Дата = row["Дата"] != DBNull.Value ? (row["Дата"]?.ToString() ?? "") : "";
                
                    record.Номер = row["Номер"] != DBNull.Value ? (row["Номер"]?.ToString() ?? "") : "";
                
                    record.Організація = row["Організація"] != DBNull.Value ? (row["Організація"]?.ToString() ?? "") : "";
                
                    record.Контрагент = row["Контрагент"] != DBNull.Value ? (row["Контрагент"]?.ToString() ?? "") : "";
                
                    record.Склад = row["Склад"] != DBNull.Value ? (row["Склад"]?.ToString() ?? "") : "";
                
                    record.Каса = row["Каса"] != DBNull.Value ? (row["Каса"]?.ToString() ?? "") : "";
                
                    record.Валюта = row["Валюта"] != DBNull.Value ? (row["Валюта"]?.ToString() ?? "") : "";
                
                    record.Сума = row["Сума"] != DBNull.Value ? (row["Сума"]?.ToString() ?? "") : "";
                
                    record.Автор = row["Автор"] != DBNull.Value ? (row["Автор"]?.ToString() ?? "") : "";
                
                    record.Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"]?.ToString() ?? "") : "";
                

                TreeIter CurrentIter = Store.AppendValues(record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
          
        }
    }
    #endregion
    
    #region JOURNAL "Продажі"
    
    public class Журнали_Продажі
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        
        string Назва = "";
        string Дата = "";
        string Номер = "";
        string Організація = "";
        string Контрагент = "";
        string Склад = "";
        string Каса = "";
        string Валюта = "";
        string Сума = "";
        string Автор = "";
        string Коментар = "";

        // Масив для запису стрічки в Store
        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Type, Spend /*Проведений документ*/
            /* */ , Назва, Дата, Номер, Організація, Контрагент, Склад, Каса, Валюта, Сума, Автор, Коментар };
        }

        // Джерело даних для списку
        public static ListStore Store = new ListStore(
          typeof(Gdk.Pixbuf) /* Image */, 
          typeof(string) /* ID */, 
          typeof(string) /* Type */, 
          typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* Дата */
            , typeof(string) /* Номер */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Склад */
            , typeof(string) /* Каса */
            , typeof(string) /* Валюта */
            , typeof(string) /* Сума */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("Type", new CellRendererText(), "text", 2) { Visible = false }); /*Type*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 3)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*Дата*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Номер*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Сума*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 14) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        // Словник з відборами, ключ це Тип документу
        public static Dictionary<string, List<Where>> Where { get; set; } = new Dictionary<string, List<Where>>();

        // Добавляє відбір по періоду в словник відборів
        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            
            {
                List<Where> where = new List<Where>();
                Where.Add("ЗамовленняКлієнта", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ЗамовленняКлієнта_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("РеалізаціяТоварівТаПослуг", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РеалізаціяТоварівТаПослуг_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПоверненняТоварівВідКлієнта", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПоверненняТоварівВідКлієнта_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("АктВиконанихРобіт", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, АктВиконанихРобіт_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("РахунокФактура", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РахунокФактура_Const.ДатаДок, типПеріоду);
            }
              
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            Dictionary<string, string> allowDoc = new Dictionary<string, string>();
            allowDoc.Add("ЗамовленняКлієнта", "Замовлення клієнта");
            allowDoc.Add("РеалізаціяТоварівТаПослуг", "Реалізація товарів та послуг");
            allowDoc.Add("ПоверненняТоварівВідКлієнта", "Повернення товарів від клієнта");
            allowDoc.Add("АктВиконанихРобіт", "Акт виконаних робіт");
            allowDoc.Add("РахунокФактура", "Рахунок фактура");
            
            return allowDoc;
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;
            List<string> allQuery = new List<string>();
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

          
              {
                  Query query = new Query(Документи.ЗамовленняКлієнта_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ЗамовленняКлієнта") && Where["ЗамовленняКлієнта"].Count != 0) {
                      query.Where = Where["ЗамовленняКлієнта"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ЗамовленняКлієнта'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ЗамовленняКлієнта_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РеалізаціяТоварівТаПослуг") && Where["РеалізаціяТоварівТаПослуг"].Count != 0) {
                      query.Where = Where["РеалізаціяТоварівТаПослуг"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РеалізаціяТоварівТаПослуг'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РеалізаціяТоварівТаПослуг_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПоверненняТоварівВідКлієнта") && Where["ПоверненняТоварівВідКлієнта"].Count != 0) {
                      query.Where = Where["ПоверненняТоварівВідКлієнта"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПоверненняТоварівВідКлієнта'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПоверненняТоварівВідКлієнта_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.АктВиконанихРобіт_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("АктВиконанихРобіт") && Where["АктВиконанихРобіт"].Count != 0) {
                      query.Where = Where["АктВиконанихРобіт"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'АктВиконанихРобіт'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.АктВиконанихРобіт_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.АктВиконанихРобіт_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.АктВиконанихРобіт_Const.Каса, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.АктВиконанихРобіт_Const.Валюта, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.АктВиконанихРобіт_Const.Автор, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.РахунокФактура_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РахунокФактура") && Where["РахунокФактура"].Count != 0) {
                      query.Where = Where["РахунокФактура"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РахунокФактура'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РахунокФактура_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.РахунокФактура_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.РахунокФактура_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.РахунокФактура_Const.Каса, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.РахунокФактура_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РахунокФактура_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            unionAllQuery += "\nORDER BY Дата";
          
            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(unionAllQuery, paramQuery, out columnsName, out listRow);

            foreach (Dictionary<string, object> row in listRow)
            {
                Журнали_Продажі record = new Журнали_Продажі();
                record.ID = row["uid"]?.ToString() ?? "";
                record.Type = row["type"]?.ToString() ?? "";
                record.DeletionLabel = (bool)row["deletion_label"];
                record.Spend = (bool)row["spend"];
                
                    record.Назва = row["Назва"] != DBNull.Value ? (row["Назва"]?.ToString() ?? "") : "";
                
                    record.Дата = row["Дата"] != DBNull.Value ? (row["Дата"]?.ToString() ?? "") : "";
                
                    record.Номер = row["Номер"] != DBNull.Value ? (row["Номер"]?.ToString() ?? "") : "";
                
                    record.Організація = row["Організація"] != DBNull.Value ? (row["Організація"]?.ToString() ?? "") : "";
                
                    record.Контрагент = row["Контрагент"] != DBNull.Value ? (row["Контрагент"]?.ToString() ?? "") : "";
                
                    record.Склад = row["Склад"] != DBNull.Value ? (row["Склад"]?.ToString() ?? "") : "";
                
                    record.Каса = row["Каса"] != DBNull.Value ? (row["Каса"]?.ToString() ?? "") : "";
                
                    record.Валюта = row["Валюта"] != DBNull.Value ? (row["Валюта"]?.ToString() ?? "") : "";
                
                    record.Сума = row["Сума"] != DBNull.Value ? (row["Сума"]?.ToString() ?? "") : "";
                
                    record.Автор = row["Автор"] != DBNull.Value ? (row["Автор"]?.ToString() ?? "") : "";
                
                    record.Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"]?.ToString() ?? "") : "";
                

                TreeIter CurrentIter = Store.AppendValues(record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
          
        }
    }
    #endregion
    
    #region JOURNAL "Каса"
    
    public class Журнали_Каса
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        
        string Назва = "";
        string Дата = "";
        string Номер = "";
        string Організація = "";
        string Контрагент = "";
        string Каса = "";
        string Каса2 = "";
        string Валюта = "";
        string Сума = "";
        string Автор = "";
        string Коментар = "";

        // Масив для запису стрічки в Store
        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Type, Spend /*Проведений документ*/
            /* */ , Назва, Дата, Номер, Організація, Контрагент, Каса, Каса2, Валюта, Сума, Автор, Коментар };
        }

        // Джерело даних для списку
        public static ListStore Store = new ListStore(
          typeof(Gdk.Pixbuf) /* Image */, 
          typeof(string) /* ID */, 
          typeof(string) /* Type */, 
          typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* Дата */
            , typeof(string) /* Номер */
            , typeof(string) /* Організація */
            , typeof(string) /* Контрагент */
            , typeof(string) /* Каса */
            , typeof(string) /* Каса2 */
            , typeof(string) /* Валюта */
            , typeof(string) /* Сума */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("Type", new CellRendererText(), "text", 2) { Visible = false }); /*Type*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 3)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*Дата*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Номер*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Каса2", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Каса2*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*Сума*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 14) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        // Словник з відборами, ключ це Тип документу
        public static Dictionary<string, List<Where>> Where { get; set; } = new Dictionary<string, List<Where>>();

        // Добавляє відбір по періоду в словник відборів
        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            
            {
                List<Where> where = new List<Where>();
                Where.Add("ПрихіднийКасовийОрдер", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПрихіднийКасовийОрдер_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("РозхіднийКасовийОрдер", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РозхіднийКасовийОрдер_Const.ДатаДок, типПеріоду);
            }
              
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            Dictionary<string, string> allowDoc = new Dictionary<string, string>();
            allowDoc.Add("ПрихіднийКасовийОрдер", "Прихідний касовий ордер");
            allowDoc.Add("РозхіднийКасовийОрдер", "Розхідний касовий ордер");
            
            return allowDoc;
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;
            List<string> allQuery = new List<string>();
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

          
              {
                  Query query = new Query(Документи.ПрихіднийКасовийОрдер_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПрихіднийКасовийОрдер") && Where["ПрихіднийКасовийОрдер"].Count != 0) {
                      query.Where = Where["ПрихіднийКасовийОрдер"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПрихіднийКасовийОрдер'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Каса, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.КасаВідправник, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса2"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПрихіднийКасовийОрдер_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.РозхіднийКасовийОрдер_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РозхіднийКасовийОрдер") && Where["РозхіднийКасовийОрдер"].Count != 0) {
                      query.Where = Where["РозхіднийКасовийОрдер"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РозхіднийКасовийОрдер'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Контрагенти_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Контрагент, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Контрагенти_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Каса, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Каси_Const.Назва, "Каса"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Каси_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.КасаОтримувач, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Каси_Const.Назва, "Каса2"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Валюти_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Валюта, query.Table, "join_tab_5"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "Валюта"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.СумаДокументу, "Сума"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РозхіднийКасовийОрдер_Const.Автор, query.Table, "join_tab_6"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_6." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            unionAllQuery += "\nORDER BY Дата";
          
            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(unionAllQuery, paramQuery, out columnsName, out listRow);

            foreach (Dictionary<string, object> row in listRow)
            {
                Журнали_Каса record = new Журнали_Каса();
                record.ID = row["uid"]?.ToString() ?? "";
                record.Type = row["type"]?.ToString() ?? "";
                record.DeletionLabel = (bool)row["deletion_label"];
                record.Spend = (bool)row["spend"];
                
                    record.Назва = row["Назва"] != DBNull.Value ? (row["Назва"]?.ToString() ?? "") : "";
                
                    record.Дата = row["Дата"] != DBNull.Value ? (row["Дата"]?.ToString() ?? "") : "";
                
                    record.Номер = row["Номер"] != DBNull.Value ? (row["Номер"]?.ToString() ?? "") : "";
                
                    record.Організація = row["Організація"] != DBNull.Value ? (row["Організація"]?.ToString() ?? "") : "";
                
                    record.Контрагент = row["Контрагент"] != DBNull.Value ? (row["Контрагент"]?.ToString() ?? "") : "";
                
                    record.Каса = row["Каса"] != DBNull.Value ? (row["Каса"]?.ToString() ?? "") : "";
                
                    record.Каса2 = row["Каса2"] != DBNull.Value ? (row["Каса2"]?.ToString() ?? "") : "";
                
                    record.Валюта = row["Валюта"] != DBNull.Value ? (row["Валюта"]?.ToString() ?? "") : "";
                
                    record.Сума = row["Сума"] != DBNull.Value ? (row["Сума"]?.ToString() ?? "") : "";
                
                    record.Автор = row["Автор"] != DBNull.Value ? (row["Автор"]?.ToString() ?? "") : "";
                
                    record.Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"]?.ToString() ?? "") : "";
                

                TreeIter CurrentIter = Store.AppendValues(record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
          
        }
    }
    #endregion
    
    #region JOURNAL "Склад"
    
    public class Журнали_Склад
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        
        string Назва = "";
        string Дата = "";
        string Номер = "";
        string Організація = "";
        string СкладВідправник = "";
        string СкладОтримувач = "";
        string Автор = "";
        string Коментар = "";

        // Масив для запису стрічки в Store
        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Type, Spend /*Проведений документ*/
            /* */ , Назва, Дата, Номер, Організація, СкладВідправник, СкладОтримувач, Автор, Коментар };
        }

        // Джерело даних для списку
        public static ListStore Store = new ListStore(
          typeof(Gdk.Pixbuf) /* Image */, 
          typeof(string) /* ID */, 
          typeof(string) /* Type */, 
          typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* Дата */
            , typeof(string) /* Номер */
            , typeof(string) /* Організація */
            , typeof(string) /* СкладВідправник */
            , typeof(string) /* СкладОтримувач */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("Type", new CellRendererText(), "text", 2) { Visible = false }); /*Type*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 3)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*Дата*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Номер*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("СкладВідправник", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*СкладВідправник*/
            treeView.AppendColumn(new TreeViewColumn("СкладОтримувач", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*СкладОтримувач*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        // Словник з відборами, ключ це Тип документу
        public static Dictionary<string, List<Where>> Where { get; set; } = new Dictionary<string, List<Where>>();

        // Добавляє відбір по періоду в словник відборів
        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            
            {
                List<Where> where = new List<Where>();
                Where.Add("ПереміщенняТоварів", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПереміщенняТоварів_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ВведенняЗалишків", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ВведенняЗалишків_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПсуванняТоварів", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПсуванняТоварів_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ВнутрішнєСпоживанняТоварів", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ВнутрішнєСпоживанняТоварів_Const.ДатаДок, типПеріоду);
            }
              
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            Dictionary<string, string> allowDoc = new Dictionary<string, string>();
            allowDoc.Add("ПереміщенняТоварів", "Переміщення товарів");
            allowDoc.Add("ВведенняЗалишків", "Введення залишків");
            allowDoc.Add("ПсуванняТоварів", "Псування товарів");
            allowDoc.Add("ВнутрішнєСпоживанняТоварів", "Внутрішнє споживання товарів");
            
            return allowDoc;
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;
            List<string> allQuery = new List<string>();
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

          
              {
                  Query query = new Query(Документи.ПереміщенняТоварів_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПереміщенняТоварів") && Where["ПереміщенняТоварів"].Count != 0) {
                      query.Where = Where["ПереміщенняТоварів"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПереміщенняТоварів'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПереміщенняТоварів_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварів_Const.СкладВідправник, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "СкладВідправник"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварів_Const.СкладОтримувач, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "СкладОтримувач"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПереміщенняТоварів_Const.Автор, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ВведенняЗалишків_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ВведенняЗалишків") && Where["ВведенняЗалишків"].Count != 0) {
                      query.Where = Where["ВведенняЗалишків"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ВведенняЗалишків'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ВведенняЗалишків_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "СкладВідправник"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ВведенняЗалишків_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "СкладОтримувач"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ВведенняЗалишків_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ВнутрішнєСпоживанняТоварів") && Where["ВнутрішнєСпоживанняТоварів"].Count != 0) {
                      query.Where = Where["ВнутрішнєСпоживанняТоварів"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ВнутрішнєСпоживанняТоварів'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "СкладВідправник"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "СкладОтримувач"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ВнутрішнєСпоживанняТоварів_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПсуванняТоварів_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПсуванняТоварів") && Where["ПсуванняТоварів"].Count != 0) {
                      query.Where = Where["ПсуванняТоварів"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПсуванняТоварів'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПсуванняТоварів_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПсуванняТоварів_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "СкладВідправник"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "СкладОтримувач"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПсуванняТоварів_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            unionAllQuery += "\nORDER BY Дата";
          
            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(unionAllQuery, paramQuery, out columnsName, out listRow);

            foreach (Dictionary<string, object> row in listRow)
            {
                Журнали_Склад record = new Журнали_Склад();
                record.ID = row["uid"]?.ToString() ?? "";
                record.Type = row["type"]?.ToString() ?? "";
                record.DeletionLabel = (bool)row["deletion_label"];
                record.Spend = (bool)row["spend"];
                
                    record.Назва = row["Назва"] != DBNull.Value ? (row["Назва"]?.ToString() ?? "") : "";
                
                    record.Дата = row["Дата"] != DBNull.Value ? (row["Дата"]?.ToString() ?? "") : "";
                
                    record.Номер = row["Номер"] != DBNull.Value ? (row["Номер"]?.ToString() ?? "") : "";
                
                    record.Організація = row["Організація"] != DBNull.Value ? (row["Організація"]?.ToString() ?? "") : "";
                
                    record.СкладВідправник = row["СкладВідправник"] != DBNull.Value ? (row["СкладВідправник"]?.ToString() ?? "") : "";
                
                    record.СкладОтримувач = row["СкладОтримувач"] != DBNull.Value ? (row["СкладОтримувач"]?.ToString() ?? "") : "";
                
                    record.Автор = row["Автор"] != DBNull.Value ? (row["Автор"]?.ToString() ?? "") : "";
                
                    record.Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"]?.ToString() ?? "") : "";
                

                TreeIter CurrentIter = Store.AppendValues(record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
          
        }
    }
    #endregion
    
    #region JOURNAL "АдреснеЗберігання"
    
    public class Журнали_АдреснеЗберігання
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        
        string Назва = "";
        string Дата = "";
        string Номер = "";
        string Організація = "";
        string Склад = "";
        string Автор = "";
        string Коментар = "";

        // Масив для запису стрічки в Store
        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Type, Spend /*Проведений документ*/
            /* */ , Назва, Дата, Номер, Організація, Склад, Автор, Коментар };
        }

        // Джерело даних для списку
        public static ListStore Store = new ListStore(
          typeof(Gdk.Pixbuf) /* Image */, 
          typeof(string) /* ID */, 
          typeof(string) /* Type */, 
          typeof(bool) /* Spend Проведений документ*/
            , typeof(string) /* Назва */
            , typeof(string) /* Дата */
            , typeof(string) /* Номер */
            , typeof(string) /* Організація */
            , typeof(string) /* Склад */
            , typeof(string) /* Автор */
            , typeof(string) /* Коментар */
            );

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("Type", new CellRendererText(), "text", 2) { Visible = false }); /*Type*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 3)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*Дата*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Номер*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        // Словник з відборами, ключ це Тип документу
        public static Dictionary<string, List<Where>> Where { get; set; } = new Dictionary<string, List<Where>>();

        // Добавляє відбір по періоду в словник відборів
        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            
            {
                List<Where> where = new List<Where>();
                Where.Add("РозміщенняТоварівНаСкладі", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РозміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ПереміщенняТоварівНаСкладі", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ПереміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("ЗбіркаТоварівНаСкладі", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, ЗбіркаТоварівНаСкладі_Const.ДатаДок, типПеріоду);
            }
              
            {
                List<Where> where = new List<Where>();
                Where.Add("РозміщенняНоменклатуриПоКоміркам", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, типПеріоду);
            }
              
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            Dictionary<string, string> allowDoc = new Dictionary<string, string>();
            allowDoc.Add("РозміщенняТоварівНаСкладі", "Розміщення товарів на складі");
            allowDoc.Add("ПереміщенняТоварівНаСкладі", "Переміщення товарів на складі");
            allowDoc.Add("ЗбіркаТоварівНаСкладі", "Збірка товарів на складі");
            allowDoc.Add("РозміщенняНоменклатуриПоКоміркам", "Розміщення номенклатури по коміркам");
            
            return allowDoc;
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;
            List<string> allQuery = new List<string>();
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

          
              {
                  Query query = new Query(Документи.РозміщенняТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РозміщенняТоварівНаСкладі") && Where["РозміщенняТоварівНаСкладі"].Count != 0) {
                      query.Where = Where["РозміщенняТоварівНаСкладі"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РозміщенняТоварівНаСкладі'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РозміщенняТоварівНаСкладі_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.РозміщенняТоварівНаСкладі_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РозміщенняТоварівНаСкладі_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ПереміщенняТоварівНаСкладі") && Where["ПереміщенняТоварівНаСкладі"].Count != 0) {
                      query.Where = Where["ПереміщенняТоварівНаСкладі"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ПереміщенняТоварівНаСкладі'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПереміщенняТоварівНаСкладі_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПереміщенняТоварівНаСкладі_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПереміщенняТоварівНаСкладі_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("ЗбіркаТоварівНаСкладі") && Where["ЗбіркаТоварівНаСкладі"].Count != 0) {
                      query.Where = Where["ЗбіркаТоварівНаСкладі"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'ЗбіркаТоварівНаСкладі'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ЗбіркаТоварівНаСкладі_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ЗбіркаТоварівНаСкладі_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ЗбіркаТоварівНаСкладі_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              {
                  Query query = new Query(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("РозміщенняНоменклатуриПоКоміркам") && Where["РозміщенняНоменклатуриПоКоміркам"].Count != 0) {
                      query.Where = Where["РозміщенняНоменклатуриПоКоміркам"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
                  }

                  query.FieldAndAlias.Add(new NameValue<string>("'РозміщенняНоменклатуриПоКоміркам'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "Склад"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            unionAllQuery += "\nORDER BY Дата";
          
            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Config.Kernel!.DataBase.SelectRequest(unionAllQuery, paramQuery, out columnsName, out listRow);

            foreach (Dictionary<string, object> row in listRow)
            {
                Журнали_АдреснеЗберігання record = new Журнали_АдреснеЗберігання();
                record.ID = row["uid"]?.ToString() ?? "";
                record.Type = row["type"]?.ToString() ?? "";
                record.DeletionLabel = (bool)row["deletion_label"];
                record.Spend = (bool)row["spend"];
                
                    record.Назва = row["Назва"] != DBNull.Value ? (row["Назва"]?.ToString() ?? "") : "";
                
                    record.Дата = row["Дата"] != DBNull.Value ? (row["Дата"]?.ToString() ?? "") : "";
                
                    record.Номер = row["Номер"] != DBNull.Value ? (row["Номер"]?.ToString() ?? "") : "";
                
                    record.Організація = row["Організація"] != DBNull.Value ? (row["Організація"]?.ToString() ?? "") : "";
                
                    record.Склад = row["Склад"] != DBNull.Value ? (row["Склад"]?.ToString() ?? "") : "";
                
                    record.Автор = row["Автор"] != DBNull.Value ? (row["Автор"]?.ToString() ?? "") : "";
                
                    record.Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"]?.ToString() ?? "") : "";
                

                TreeIter CurrentIter = Store.AppendValues(record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
          
        }
    }
    #endregion
    
}

namespace StorageAndTrade_1_0.РегістриВідомостей.ТабличніСписки
{
    
    #region REGISTER "ЦіниНоменклатури"
    
      
    public class ЦіниНоменклатури_Записи
    {
        string Image = AppContext.BaseDirectory + "images/doc.png";
        string ID = "";
        string Період = "";
        
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string ВидЦіни = "";
        string Ціна = "";
        string Пакування = "";
        string Валюта = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Період
            /* */ , Номенклатура, ХарактеристикаНоменклатури, ВидЦіни, Ціна, Пакування, Валюта };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(string) /* Період */
            , typeof(string) /* Номенклатура */
            , typeof(string) /* ХарактеристикаНоменклатури */
            , typeof(string) /* ВидЦіни */
            , typeof(string) /* Ціна */
            , typeof(string) /* Пакування */
            , typeof(string) /* Валюта */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 2));
            /* */
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("ВидЦіни", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ВидЦіни*/
            treeView.AppendColumn(new TreeViewColumn("Ціна", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Ціна*/
            treeView.AppendColumn(new TreeViewColumn("Пакування", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Пакування*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*Валюта*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;

            РегістриВідомостей.ЦіниНоменклатури_RecordsSet ЦіниНоменклатури_RecordsSet = new РегістриВідомостей.ЦіниНоменклатури_RecordsSet();

            /* Where */
            ЦіниНоменклатури_RecordsSet.QuerySelect.Where = Where;

            /* DEFAULT ORDER */
            ЦіниНоменклатури_RecordsSet.QuerySelect.Order.Add("period", SelectOrder.ASC);

            
                /* Join Table */
                ЦіниНоменклатури_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.Номенклатура_Const.TABLE, РегістриВідомостей.ЦіниНоменклатури_Const.Номенклатура, ЦіниНоменклатури_RecordsSet.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ЦіниНоменклатури_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Номенклатура_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ЦіниНоменклатури_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.ХарактеристикиНоменклатури_Const.TABLE, РегістриВідомостей.ЦіниНоменклатури_Const.ХарактеристикаНоменклатури, ЦіниНоменклатури_RecordsSet.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ЦіниНоменклатури_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.ХарактеристикиНоменклатури_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ЦіниНоменклатури_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.ВидиЦін_Const.TABLE, РегістриВідомостей.ЦіниНоменклатури_Const.ВидЦіни, ЦіниНоменклатури_RecordsSet.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ЦіниНоменклатури_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.ВидиЦін_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                ЦіниНоменклатури_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, РегістриВідомостей.ЦіниНоменклатури_Const.Пакування, ЦіниНоменклатури_RecordsSet.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ЦіниНоменклатури_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "join_tab_4_field_1"));
                  
                /* Join Table */
                ЦіниНоменклатури_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, РегістриВідомостей.ЦіниНоменклатури_Const.Валюта, ЦіниНоменклатури_RecordsSet.QuerySelect.Table, "join_tab_5"));
                
                  /* Field */
                  ЦіниНоменклатури_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_5." + Довідники.Валюти_Const.Назва, "join_tab_5_field_1"));
                  

            /* Read */
            ЦіниНоменклатури_RecordsSet.Read();
            foreach (ЦіниНоменклатури_RecordsSet.Record record in ЦіниНоменклатури_RecordsSet.Records)
            {
                ЦіниНоменклатури_Записи Record = new ЦіниНоменклатури_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Номенклатура = ЦіниНоменклатури_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_1_field_1"].ToString() ?? "", /**/
                    ХарактеристикаНоменклатури = ЦіниНоменклатури_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_2_field_1"].ToString() ?? "", /**/
                    ВидЦіни = ЦіниНоменклатури_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_3_field_1"].ToString() ?? "", /**/
                    Пакування = ЦіниНоменклатури_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_4_field_1"].ToString() ?? "", /**/
                    Валюта = ЦіниНоменклатури_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_5_field_1"].ToString() ?? "", /**/
                    Ціна = record.Ціна.ToString() ?? "" /**/
                    
                };

                TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (Record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "КурсиВалют"
    
      
    public class КурсиВалют_Записи
    {
        string Image = AppContext.BaseDirectory + "images/doc.png";
        string ID = "";
        string Період = "";
        
        string Валюта = "";
        string Курс = "";
        string Кратність = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Період
            /* */ , Валюта, Курс, Кратність };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(string) /* Період */
            , typeof(string) /* Валюта */
            , typeof(string) /* Курс */
            , typeof(string) /* Кратність */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 2));
            /* */
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Курс", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Курс*/
            treeView.AppendColumn(new TreeViewColumn("Кратність", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Кратність*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;

            РегістриВідомостей.КурсиВалют_RecordsSet КурсиВалют_RecordsSet = new РегістриВідомостей.КурсиВалют_RecordsSet();

            /* Where */
            КурсиВалют_RecordsSet.QuerySelect.Where = Where;

            /* DEFAULT ORDER */
            КурсиВалют_RecordsSet.QuerySelect.Order.Add("period", SelectOrder.ASC);

            
                /* Join Table */
                КурсиВалют_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, РегістриВідомостей.КурсиВалют_Const.Валюта, КурсиВалют_RecordsSet.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  КурсиВалют_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Валюти_Const.Назва, "join_tab_1_field_1"));
                  

            /* Read */
            КурсиВалют_RecordsSet.Read();
            foreach (КурсиВалют_RecordsSet.Record record in КурсиВалют_RecordsSet.Records)
            {
                КурсиВалют_Записи Record = new КурсиВалют_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Валюта = КурсиВалют_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_1_field_1"].ToString() ?? "", /**/
                    Курс = record.Курс.ToString() ?? "", /**/
                    Кратність = record.Кратність.ToString() ?? "" /**/
                    
                };

                TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (Record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "ШтрихкодиНоменклатури"
    
      
    public class ШтрихкодиНоменклатури_Записи
    {
        string Image = AppContext.BaseDirectory + "images/doc.png";
        string ID = "";
        string Період = "";
        
        string Штрихкод = "";
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Пакування = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Період
            /* */ , Штрихкод, Номенклатура, ХарактеристикаНоменклатури, Пакування };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(string) /* Період */
            , typeof(string) /* Штрихкод */
            , typeof(string) /* Номенклатура */
            , typeof(string) /* ХарактеристикаНоменклатури */
            , typeof(string) /* Пакування */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 2));
            /* */
            treeView.AppendColumn(new TreeViewColumn("Штрихкод", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Штрихкод*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Пакування", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Пакування*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;

            РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet ШтрихкодиНоменклатури_RecordsSet = new РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet();

            /* Where */
            ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Where = Where;

            /* DEFAULT ORDER */
            ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Order.Add("period", SelectOrder.ASC);

            
                /* Join Table */
                ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.Номенклатура_Const.TABLE, РегістриВідомостей.ШтрихкодиНоменклатури_Const.Номенклатура, ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ШтрихкодиНоменклатури_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Номенклатура_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.ХарактеристикиНоменклатури_Const.TABLE, РегістриВідомостей.ШтрихкодиНоменклатури_Const.ХарактеристикаНоменклатури, ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ШтрихкодиНоменклатури_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.ХарактеристикиНоменклатури_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, РегістриВідомостей.ШтрихкодиНоменклатури_Const.Пакування, ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ШтрихкодиНоменклатури_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "join_tab_3_field_1"));
                  

            /* Read */
            ШтрихкодиНоменклатури_RecordsSet.Read();
            foreach (ШтрихкодиНоменклатури_RecordsSet.Record record in ШтрихкодиНоменклатури_RecordsSet.Records)
            {
                ШтрихкодиНоменклатури_Записи Record = new ШтрихкодиНоменклатури_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Номенклатура = ШтрихкодиНоменклатури_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_1_field_1"].ToString() ?? "", /**/
                    ХарактеристикаНоменклатури = ШтрихкодиНоменклатури_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_2_field_1"].ToString() ?? "", /**/
                    Пакування = ШтрихкодиНоменклатури_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_3_field_1"].ToString() ?? "", /**/
                    Штрихкод = record.Штрихкод.ToString() ?? "" /**/
                    
                };

                TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (Record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "ФайлиДокументів"
    
      
    public class ФайлиДокументів_Записи
    {
        string Image = AppContext.BaseDirectory + "images/doc.png";
        string ID = "";
        string Період = "";
        
        string Файл = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Період
            /* */ , Файл };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(string) /* Період */
            , typeof(string) /* Файл */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 2));
            /* */
            treeView.AppendColumn(new TreeViewColumn("Файл", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Файл*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List<Where> Where { get; set; } = new List<Where>();

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = CurrentPath = null;

            РегістриВідомостей.ФайлиДокументів_RecordsSet ФайлиДокументів_RecordsSet = new РегістриВідомостей.ФайлиДокументів_RecordsSet();

            /* Where */
            ФайлиДокументів_RecordsSet.QuerySelect.Where = Where;

            /* DEFAULT ORDER */
            ФайлиДокументів_RecordsSet.QuerySelect.Order.Add("period", SelectOrder.ASC);

            
                /* Join Table */
                ФайлиДокументів_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.Файли_Const.TABLE, РегістриВідомостей.ФайлиДокументів_Const.Файл, ФайлиДокументів_RecordsSet.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ФайлиДокументів_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Файли_Const.Назва, "join_tab_1_field_1"));
                  

            /* Read */
            ФайлиДокументів_RecordsSet.Read();
            foreach (ФайлиДокументів_RecordsSet.Record record in ФайлиДокументів_RecordsSet.Records)
            {
                ФайлиДокументів_Записи Record = new ФайлиДокументів_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Файл = ФайлиДокументів_RecordsSet.JoinValue[record.UID.ToString()]["join_tab_1_field_1"].ToString() ?? "" /**/
                    
                };

                TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (Record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "РозміщенняНоменклатуриПоКоміркамНаСкладі"
    
      
    #endregion
    
}

  