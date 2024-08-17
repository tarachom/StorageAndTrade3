
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
 * Дата конфігурації: 17.08.2024 20:35:19
 *
 *
 * Цей код згенерований в Конфігураторі 3. Шаблон Gtk.xslt
 *
 */

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

namespace StorageAndTrade_1_0.Довідники.ТабличніСписки
{
    
    #region DIRECTORY "Організації"
    
      
    /* ТАБЛИЦЯ */
    public class Організації_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Організації_Select Організації_Select = new Довідники.Організації_Select();
            Організації_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Організації_Const.Код,
                /*Назва*/ Довідники.Організації_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Організації_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Організації_Select.QuerySelect.Order.Add(Довідники.Організації_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Організації_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Організації_Select.MoveNext())
            {
                Довідники.Організації_Pointer? cur = Організації_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Організації_Записи Record = new Організації_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Організації_Const.Код].ToString() ?? "",
                        Назва = Fields[Організації_Const.Назва].ToString() ?? "",
                        
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
    public class Організації_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Організації_Select Організації_Select = new Довідники.Організації_Select();
            Організації_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Організації_Const.Код,
                /*Назва*/ Довідники.Організації_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Організації_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Організації_Select.QuerySelect.Order.Add(Довідники.Організації_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Організації_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Організації_Select.MoveNext())
            {
                Довідники.Організації_Pointer? cur = Організації_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Організації_ЗаписиШвидкийВибір Record = new Організації_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Організації_Const.Код].ToString() ?? "",
                        Назва = Fields[Організації_Const.Назва].ToString() ?? "",
                        
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
    public class Номенклатура_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ОдиницяВиміру = "";
        string ТипНоменклатури = "";
        string Залишок = "";
        string ВРезерві = "";
        string ВРезервіПідЗамовлення = "";
        string ЗалишокВКомірках = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*ОдиницяВиміру*/ ОдиницяВиміру,
                /*ТипНоменклатури*/ ТипНоменклатури,
                /*Залишок*/ Залишок,
                /*ВРезерві*/ ВРезерві,
                /*ВРезервіПідЗамовлення*/ ВРезервіПідЗамовлення,
                /*ЗалишокВКомірках*/ ЗалишокВКомірках,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*ОдиницяВиміру*/ typeof(string),  
                /*ТипНоменклатури*/ typeof(string),  
                /*Залишок*/ typeof(string), 
                /*ВРезерві*/ typeof(string), 
                /*ВРезервіПідЗамовлення*/ typeof(string), 
                /*ЗалишокВКомірках*/ typeof(string), 
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Пакування", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ОдиницяВиміру*/
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ТипНоменклатури*/
            

            /* Додаткові поля */
            treeView.AppendColumn(new TreeViewColumn("Залишок", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Залишок*/
            treeView.AppendColumn(new TreeViewColumn("В резерві", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*ВРезерві*/
            treeView.AppendColumn(new TreeViewColumn("Під замовлення", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*ВРезервіПідЗамовлення*/
            treeView.AppendColumn(new TreeViewColumn("В комірках", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*ЗалишокВКомірках*/
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Номенклатура_Select Номенклатура_Select = new Довідники.Номенклатура_Select();
            Номенклатура_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Номенклатура_Const.Код,
                /*Назва*/ Довідники.Номенклатура_Const.Назва,
                /*ТипНоменклатури*/ Довідники.Номенклатура_Const.ТипНоменклатури,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Номенклатура_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Номенклатура_Select.QuerySelect.Order.Add(Довідники.Номенклатура_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, Довідники.Номенклатура_Const.ОдиницяВиміру, Номенклатура_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "join_tab_1_field_1"));
                  
                /* Additional Field */
                Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN ( WITH Залишки AS ( SELECT ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} AS Номенклатура, SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності} ) AS ВНаявності FROM {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах WHERE ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid GROUP BY Номенклатура ) SELECT ROUND(ВНаявності, 1) FROM Залишки ) END)", "Залишок"));
                /*
                CASE WHEN 
{Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} 
THEN
(
        WITH Залишки AS 
        (
            SELECT
                ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} AS Номенклатура,
                SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності} ) AS ВНаявності
            FROM
                {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах
            WHERE
                ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = 
		{Довідники.Номенклатура_Const.TABLE}.uid
            GROUP BY Номенклатура
        )
        SELECT 
            ROUND(ВНаявності, 1)
        FROM 
            Залишки
)
END
                */
            
                /* Additional Field */
                Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN ( WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} AS Номенклатура, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу} ) AS ВРезервіЗіСкладу FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid GROUP BY Номенклатура ) SELECT ROUND(ВРезервіЗіСкладу, 1) FROM Залишки ) END)", "ВРезерві"));
                /*
                CASE WHEN 
{Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} 
THEN
(
        WITH Залишки AS 
        (
            SELECT
                ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} AS Номенклатура,
                SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу} ) AS ВРезервіЗіСкладу
            FROM
                {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
            WHERE
                ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid
            GROUP BY Номенклатура
        )
        SELECT 
            ROUND(ВРезервіЗіСкладу, 1)
        FROM 
            Залишки
)
END
                */
            
                /* Additional Field */
                Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN ( WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} AS Номенклатура, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення} ) AS ВРезервіПідЗамовлення FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid GROUP BY Номенклатура ) SELECT ROUND(ВРезервіПідЗамовлення, 1) FROM Залишки ) END)", "ВРезервіПідЗамовлення"));
                /*
                CASE WHEN 
{Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} 
THEN
(
        WITH Залишки AS 
        (
            SELECT
                ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} AS Номенклатура,
                SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення} ) AS ВРезервіПідЗамовлення
            FROM
                {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
            WHERE
                ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid
            GROUP BY Номенклатура
        )
        SELECT 
            ROUND(ВРезервіПідЗамовлення, 1)
        FROM 
            Залишки
)
END
                */
            
                /* Additional Field */
                Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN ( WITH Залишки AS ( SELECT ТовариВКомірках.{РегістриНакопичення.ТовариВКомірках_Підсумки_TablePart.Номенклатура} AS Номенклатура, SUM(ТовариВКомірках.{РегістриНакопичення.ТовариВКомірках_Підсумки_TablePart.ВНаявності} ) AS ВНаявності FROM {РегістриНакопичення.ТовариВКомірках_Підсумки_TablePart.TABLE} AS ТовариВКомірках WHERE ТовариВКомірках.{РегістриНакопичення.ТовариВКомірках_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid GROUP BY Номенклатура ) SELECT ROUND(ВНаявності, 1) FROM Залишки ) END)", "ЗалишокВКомірках"));
                /*
                CASE WHEN 
{Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар}
THEN
(
        WITH Залишки AS 
        (
            SELECT
                ТовариВКомірках.{РегістриНакопичення.ТовариВКомірках_Підсумки_TablePart.Номенклатура} AS Номенклатура,
                SUM(ТовариВКомірках.{РегістриНакопичення.ТовариВКомірках_Підсумки_TablePart.ВНаявності} ) AS ВНаявності
            FROM
                {РегістриНакопичення.ТовариВКомірках_Підсумки_TablePart.TABLE} AS ТовариВКомірках
            WHERE
                ТовариВКомірках.{РегістриНакопичення.ТовариВКомірках_Підсумки_TablePart.Номенклатура} = 
		{Довідники.Номенклатура_Const.TABLE}.uid
            GROUP BY Номенклатура
        )
        SELECT 
            ROUND(ВНаявності, 1) 
        FROM 
            Залишки
)
END
                */
            

            /* SELECT */
            await Номенклатура_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Номенклатура_Select.MoveNext())
            {
                Довідники.Номенклатура_Pointer? cur = Номенклатура_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Номенклатура_Записи Record = new Номенклатура_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        ОдиницяВиміру = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[Номенклатура_Const.Код].ToString() ?? "",
                        Назва = Fields[Номенклатура_Const.Назва].ToString() ?? "",
                        ТипНоменклатури = Перелічення.ПсевдонімиПерелічення.ТипиНоменклатури_Alias( ((Перелічення.ТипиНоменклатури)(Fields[Номенклатура_Const.ТипНоменклатури] != DBNull.Value ? Fields[Номенклатура_Const.ТипНоменклатури] : 0)) ),
                        Залишок = Fields["Залишок"].ToString() ?? "",
                        ВРезерві = Fields["ВРезерві"].ToString() ?? "",
                        ВРезервіПідЗамовлення = Fields["ВРезервіПідЗамовлення"].ToString() ?? "",
                        ЗалишокВКомірках = Fields["ЗалишокВКомірках"].ToString() ?? "",
                        
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
    public class Номенклатура_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ОдиницяВиміру = "";
        string Залишок = "";
        string ВРезерві = "";
        string ВРезервіПідЗамовлення = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*ОдиницяВиміру*/ ОдиницяВиміру,
                /*Залишок*/ Залишок,
                /*ВРезерві*/ ВРезерві,
                /*ВРезервіПідЗамовлення*/ ВРезервіПідЗамовлення,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*ОдиницяВиміру*/ typeof(string),  
                /*Залишок*/ typeof(string), 
                /*ВРезерві*/ typeof(string), 
                /*ВРезервіПідЗамовлення*/ typeof(string), 
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Пакування", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ОдиницяВиміру*/
            

            /* Додаткові поля */
            treeView.AppendColumn(new TreeViewColumn("Залишок", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Залишок*/
            treeView.AppendColumn(new TreeViewColumn("В резерві", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ВРезерві*/
            treeView.AppendColumn(new TreeViewColumn("Під замовлення", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*ВРезервіПідЗамовлення*/
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Номенклатура_Select Номенклатура_Select = new Довідники.Номенклатура_Select();
            Номенклатура_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Номенклатура_Const.Код,
                /*Назва*/ Довідники.Номенклатура_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Номенклатура_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Номенклатура_Select.QuerySelect.Order.Add(Довідники.Номенклатура_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                Номенклатура_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ПакуванняОдиниціВиміру_Const.TABLE, Довідники.Номенклатура_Const.ОдиницяВиміру, Номенклатура_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.ПакуванняОдиниціВиміру_Const.Назва, "join_tab_1_field_1"));
                  
                /* Additional Field */
                Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN ( WITH Залишки AS ( SELECT ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} AS Номенклатура, SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності} ) AS ВНаявності FROM {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах WHERE ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid GROUP BY Номенклатура ) SELECT ROUND(ВНаявності, 1) FROM Залишки ) END)", "Залишок"));
                /*
                CASE WHEN 
{Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} 
THEN
(
        WITH Залишки AS 
        (
            SELECT
                ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} AS Номенклатура,
                SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності} ) AS ВНаявності
            FROM
                {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах
            WHERE
                ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = 
		{Довідники.Номенклатура_Const.TABLE}.uid
            GROUP BY Номенклатура
        )
        SELECT 
            ROUND(ВНаявності, 1)
        FROM 
            Залишки
)
END
                */
            
                /* Additional Field */
                Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN ( WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} AS Номенклатура, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу} ) AS ВРезервіЗіСкладу FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid GROUP BY Номенклатура ) SELECT ROUND(ВРезервіЗіСкладу, 1) FROM Залишки ) END)", "ВРезерві"));
                /*
                CASE WHEN 
{Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} 
THEN
(
        WITH Залишки AS 
        (
            SELECT
                ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} AS Номенклатура,
                SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу} ) AS ВРезервіЗіСкладу
            FROM
                {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
            WHERE
                ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid
            GROUP BY Номенклатура
        )
        SELECT 
            ROUND(ВРезервіЗіСкладу, 1)
        FROM 
            Залишки
)
END
                */
            
                /* Additional Field */
                Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN ( WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} AS Номенклатура, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення} ) AS ВРезервіПідЗамовлення FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid GROUP BY Номенклатура ) SELECT ROUND(ВРезервіПідЗамовлення, 1) FROM Залишки ) END)", "ВРезервіПідЗамовлення"));
                /*
                CASE WHEN 
{Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} 
THEN
(
        WITH Залишки AS 
        (
            SELECT
                ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} AS Номенклатура,
                SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення} ) AS ВРезервіПідЗамовлення
            FROM
                {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
            WHERE
                ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid
            GROUP BY Номенклатура
        )
        SELECT 
            ROUND(ВРезервіПідЗамовлення, 1)
        FROM 
            Залишки
)
END
                */
            

            /* SELECT */
            await Номенклатура_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Номенклатура_Select.MoveNext())
            {
                Довідники.Номенклатура_Pointer? cur = Номенклатура_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Номенклатура_ЗаписиШвидкийВибір Record = new Номенклатура_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        ОдиницяВиміру = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[Номенклатура_Const.Код].ToString() ?? "",
                        Назва = Fields[Номенклатура_Const.Назва].ToString() ?? "",
                        Залишок = Fields["Залишок"].ToString() ?? "",
                        ВРезерві = Fields["ВРезерві"].ToString() ?? "",
                        ВРезервіПідЗамовлення = Fields["ВРезервіПідЗамовлення"].ToString() ?? "",
                        
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
    public class Виробники_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Виробники_Select Виробники_Select = new Довідники.Виробники_Select();
            Виробники_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Виробники_Const.Код,
                /*Назва*/ Довідники.Виробники_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Виробники_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Виробники_Select.QuerySelect.Order.Add(Довідники.Виробники_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Виробники_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Виробники_Select.MoveNext())
            {
                Довідники.Виробники_Pointer? cur = Виробники_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Виробники_Записи Record = new Виробники_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Виробники_Const.Код].ToString() ?? "",
                        Назва = Fields[Виробники_Const.Назва].ToString() ?? "",
                        
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
    public class Виробники_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Виробники_Select Виробники_Select = new Довідники.Виробники_Select();
            Виробники_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Виробники_Const.Код,
                /*Назва*/ Довідники.Виробники_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Виробники_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Виробники_Select.QuerySelect.Order.Add(Довідники.Виробники_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Виробники_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Виробники_Select.MoveNext())
            {
                Довідники.Виробники_Pointer? cur = Виробники_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Виробники_ЗаписиШвидкийВибір Record = new Виробники_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Виробники_Const.Код].ToString() ?? "",
                        Назва = Fields[Виробники_Const.Назва].ToString() ?? "",
                        
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
    public class ВидиНоменклатури_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиНоменклатури_Select ВидиНоменклатури_Select = new Довідники.ВидиНоменклатури_Select();
            ВидиНоменклатури_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ВидиНоменклатури_Const.Код,
                /*Назва*/ Довідники.ВидиНоменклатури_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ВидиНоменклатури_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ВидиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ВидиНоменклатури_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиНоменклатури_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВидиНоменклатури_Select.MoveNext())
            {
                Довідники.ВидиНоменклатури_Pointer? cur = ВидиНоменклатури_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВидиНоменклатури_Записи Record = new ВидиНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[ВидиНоменклатури_Const.Код].ToString() ?? "",
                        Назва = Fields[ВидиНоменклатури_Const.Назва].ToString() ?? "",
                        
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
    public class ВидиНоменклатури_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиНоменклатури_Select ВидиНоменклатури_Select = new Довідники.ВидиНоменклатури_Select();
            ВидиНоменклатури_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ВидиНоменклатури_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ВидиНоменклатури_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ВидиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ВидиНоменклатури_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиНоменклатури_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВидиНоменклатури_Select.MoveNext())
            {
                Довідники.ВидиНоменклатури_Pointer? cur = ВидиНоменклатури_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВидиНоменклатури_ЗаписиШвидкийВибір Record = new ВидиНоменклатури_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[ВидиНоменклатури_Const.Назва].ToString() ?? "",
                        
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
    public class ПакуванняОдиниціВиміру_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string КількістьУпаковок = "";
        string НазваПовна = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*КількістьУпаковок*/ КількістьУпаковок,
                /*НазваПовна*/ НазваПовна,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*КількістьУпаковок*/ typeof(string),  
                /*НазваПовна*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Коєфіціент", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*КількістьУпаковок*/
            treeView.AppendColumn(new TreeViewColumn("Опис", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*НазваПовна*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ПакуванняОдиниціВиміру_Select ПакуванняОдиниціВиміру_Select = new Довідники.ПакуванняОдиниціВиміру_Select();
            ПакуванняОдиниціВиміру_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ПакуванняОдиниціВиміру_Const.Код,
                /*Назва*/ Довідники.ПакуванняОдиниціВиміру_Const.Назва,
                /*КількістьУпаковок*/ Довідники.ПакуванняОдиниціВиміру_Const.КількістьУпаковок,
                /*НазваПовна*/ Довідники.ПакуванняОдиниціВиміру_Const.НазваПовна,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ПакуванняОдиниціВиміру_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ПакуванняОдиниціВиміру_Select.QuerySelect.Order.Add(Довідники.ПакуванняОдиниціВиміру_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ПакуванняОдиниціВиміру_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПакуванняОдиниціВиміру_Select.MoveNext())
            {
                Довідники.ПакуванняОдиниціВиміру_Pointer? cur = ПакуванняОдиниціВиміру_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПакуванняОдиниціВиміру_Записи Record = new ПакуванняОдиниціВиміру_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[ПакуванняОдиниціВиміру_Const.Код].ToString() ?? "",
                        Назва = Fields[ПакуванняОдиниціВиміру_Const.Назва].ToString() ?? "",
                        КількістьУпаковок = Fields[ПакуванняОдиниціВиміру_Const.КількістьУпаковок].ToString() ?? "",
                        НазваПовна = Fields[ПакуванняОдиниціВиміру_Const.НазваПовна].ToString() ?? "",
                        
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
    public class ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string КількістьУпаковок = "";
        string НазваПовна = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*КількістьУпаковок*/ КількістьУпаковок,
                /*НазваПовна*/ НазваПовна,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*КількістьУпаковок*/ typeof(string),  
                /*НазваПовна*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Коєфіціент", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*КількістьУпаковок*/
            treeView.AppendColumn(new TreeViewColumn("Опис", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*НазваПовна*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ПакуванняОдиниціВиміру_Select ПакуванняОдиниціВиміру_Select = new Довідники.ПакуванняОдиниціВиміру_Select();
            ПакуванняОдиниціВиміру_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ПакуванняОдиниціВиміру_Const.Код,
                /*Назва*/ Довідники.ПакуванняОдиниціВиміру_Const.Назва,
                /*КількістьУпаковок*/ Довідники.ПакуванняОдиниціВиміру_Const.КількістьУпаковок,
                /*НазваПовна*/ Довідники.ПакуванняОдиниціВиміру_Const.НазваПовна,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ПакуванняОдиниціВиміру_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ПакуванняОдиниціВиміру_Select.QuerySelect.Order.Add(Довідники.ПакуванняОдиниціВиміру_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ПакуванняОдиниціВиміру_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПакуванняОдиниціВиміру_Select.MoveNext())
            {
                Довідники.ПакуванняОдиниціВиміру_Pointer? cur = ПакуванняОдиниціВиміру_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір Record = new ПакуванняОдиниціВиміру_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[ПакуванняОдиниціВиміру_Const.Код].ToString() ?? "",
                        Назва = Fields[ПакуванняОдиниціВиміру_Const.Назва].ToString() ?? "",
                        КількістьУпаковок = Fields[ПакуванняОдиниціВиміру_Const.КількістьУпаковок].ToString() ?? "",
                        НазваПовна = Fields[ПакуванняОдиниціВиміру_Const.НазваПовна].ToString() ?? "",
                        
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
    public class Валюти_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string КороткаНазва = "";
        string Код_R030 = "";
        string ВиводитиКурсНаСтартову = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*КороткаНазва*/ КороткаНазва,
                /*Код_R030*/ Код_R030,
                /*ВиводитиКурсНаСтартову*/ ВиводитиКурсНаСтартову,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*КороткаНазва*/ typeof(string),  
                /*Код_R030*/ typeof(string),  
                /*ВиводитиКурсНаСтартову*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Коротка назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*КороткаНазва*/
            treeView.AppendColumn(new TreeViewColumn("R030", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Код_R030*/
            treeView.AppendColumn(new TreeViewColumn("Показувати на стартовій", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ВиводитиКурсНаСтартову*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Валюти_Select Валюти_Select = new Довідники.Валюти_Select();
            Валюти_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Валюти_Const.Код,
                /*Назва*/ Довідники.Валюти_Const.Назва,
                /*КороткаНазва*/ Довідники.Валюти_Const.КороткаНазва,
                /*Код_R030*/ Довідники.Валюти_Const.Код_R030,
                /*ВиводитиКурсНаСтартову*/ Довідники.Валюти_Const.ВиводитиКурсНаСтартову,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Валюти_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Валюти_Select.QuerySelect.Order.Add(Довідники.Валюти_Const.Код, SelectOrder.ASC);
            

            /* SELECT */
            await Валюти_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Валюти_Select.MoveNext())
            {
                Довідники.Валюти_Pointer? cur = Валюти_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Валюти_Записи Record = new Валюти_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Валюти_Const.Код].ToString() ?? "",
                        Назва = Fields[Валюти_Const.Назва].ToString() ?? "",
                        КороткаНазва = Fields[Валюти_Const.КороткаНазва].ToString() ?? "",
                        Код_R030 = Fields[Валюти_Const.Код_R030].ToString() ?? "",
                        ВиводитиКурсНаСтартову = (Fields[Валюти_Const.ВиводитиКурсНаСтартову] != DBNull.Value ? (bool)Fields[Валюти_Const.ВиводитиКурсНаСтартову] : false) ? "Так" : "",
                        
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
    public class Валюти_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string КороткаНазва = "";
        string Код_R030 = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*КороткаНазва*/ КороткаНазва,
                /*Код_R030*/ Код_R030,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*КороткаНазва*/ typeof(string),  
                /*Код_R030*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("КороткаНазва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*КороткаНазва*/
            treeView.AppendColumn(new TreeViewColumn("Код R030", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Код_R030*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Валюти_Select Валюти_Select = new Довідники.Валюти_Select();
            Валюти_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Валюти_Const.Код,
                /*Назва*/ Довідники.Валюти_Const.Назва,
                /*КороткаНазва*/ Довідники.Валюти_Const.КороткаНазва,
                /*Код_R030*/ Довідники.Валюти_Const.Код_R030,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Валюти_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Валюти_Select.QuerySelect.Order.Add(Довідники.Валюти_Const.Код, SelectOrder.ASC);
            

            /* SELECT */
            await Валюти_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Валюти_Select.MoveNext())
            {
                Довідники.Валюти_Pointer? cur = Валюти_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Валюти_ЗаписиШвидкийВибір Record = new Валюти_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Валюти_Const.Код].ToString() ?? "",
                        Назва = Fields[Валюти_Const.Назва].ToString() ?? "",
                        КороткаНазва = Fields[Валюти_Const.КороткаНазва].ToString() ?? "",
                        Код_R030 = Fields[Валюти_Const.Код_R030].ToString() ?? "",
                        
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
    public class Контрагенти_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Папка = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Папка*/ Папка,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Папка*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Папка", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Папка*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Контрагенти_Select Контрагенти_Select = new Довідники.Контрагенти_Select();
            Контрагенти_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Контрагенти_Const.Код,
                /*Назва*/ Довідники.Контрагенти_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Контрагенти_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Контрагенти_Select.MoveNext())
            {
                Довідники.Контрагенти_Pointer? cur = Контрагенти_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Контрагенти_Записи Record = new Контрагенти_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Папка = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[Контрагенти_Const.Код].ToString() ?? "",
                        Назва = Fields[Контрагенти_Const.Назва].ToString() ?? "",
                        
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
    public class Контрагенти_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Контрагенти_Select Контрагенти_Select = new Довідники.Контрагенти_Select();
            Контрагенти_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Контрагенти_Const.Код,
                /*Назва*/ Довідники.Контрагенти_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Контрагенти_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Контрагенти_Select.QuerySelect.Order.Add(Довідники.Контрагенти_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Контрагенти_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Контрагенти_Select.MoveNext())
            {
                Довідники.Контрагенти_Pointer? cur = Контрагенти_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Контрагенти_ЗаписиШвидкийВибір Record = new Контрагенти_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Контрагенти_Const.Код].ToString() ?? "",
                        Назва = Fields[Контрагенти_Const.Назва].ToString() ?? "",
                        
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
    public class Склади_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ТипСкладу = "";
        string НалаштуванняАдресногоЗберігання = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*ТипСкладу*/ ТипСкладу,
                /*НалаштуванняАдресногоЗберігання*/ НалаштуванняАдресногоЗберігання,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*ТипСкладу*/ typeof(string),  
                /*НалаштуванняАдресногоЗберігання*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Тип cкладу", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ТипСкладу*/
            treeView.AppendColumn(new TreeViewColumn("Адресне зберігання", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*НалаштуванняАдресногоЗберігання*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Склади_Select Склади_Select = new Довідники.Склади_Select();
            Склади_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Склади_Const.Код,
                /*Назва*/ Довідники.Склади_Const.Назва,
                /*ТипСкладу*/ Довідники.Склади_Const.ТипСкладу,
                /*НалаштуванняАдресногоЗберігання*/ Довідники.Склади_Const.НалаштуванняАдресногоЗберігання,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Склади_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Склади_Select.QuerySelect.Order.Add(Довідники.Склади_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Склади_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Склади_Select.MoveNext())
            {
                Довідники.Склади_Pointer? cur = Склади_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Склади_Записи Record = new Склади_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Склади_Const.Код].ToString() ?? "",
                        Назва = Fields[Склади_Const.Назва].ToString() ?? "",
                        ТипСкладу = Перелічення.ПсевдонімиПерелічення.ТипиСкладів_Alias( ((Перелічення.ТипиСкладів)(Fields[Склади_Const.ТипСкладу] != DBNull.Value ? Fields[Склади_Const.ТипСкладу] : 0)) ),
                        НалаштуванняАдресногоЗберігання = Перелічення.ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_Alias( ((Перелічення.НалаштуванняАдресногоЗберігання)(Fields[Склади_Const.НалаштуванняАдресногоЗберігання] != DBNull.Value ? Fields[Склади_Const.НалаштуванняАдресногоЗберігання] : 0)) ),
                        
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
    public class Склади_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Склади_Select Склади_Select = new Довідники.Склади_Select();
            Склади_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Склади_Const.Код,
                /*Назва*/ Довідники.Склади_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Склади_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Склади_Select.QuerySelect.Order.Add(Довідники.Склади_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Склади_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Склади_Select.MoveNext())
            {
                Довідники.Склади_Pointer? cur = Склади_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Склади_ЗаписиШвидкийВибір Record = new Склади_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Склади_Const.Код].ToString() ?? "",
                        Назва = Fields[Склади_Const.Назва].ToString() ?? "",
                        
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
    public class ВидиЦін_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Валюта*/ Валюта,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЦін_Select ВидиЦін_Select = new Довідники.ВидиЦін_Select();
            ВидиЦін_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ВидиЦін_Const.Код,
                /*Назва*/ Довідники.ВидиЦін_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ВидиЦін_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВидиЦін_Select.MoveNext())
            {
                Довідники.ВидиЦін_Pointer? cur = ВидиЦін_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВидиЦін_Записи Record = new ВидиЦін_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Валюта = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[ВидиЦін_Const.Код].ToString() ?? "",
                        Назва = Fields[ВидиЦін_Const.Назва].ToString() ?? "",
                        
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
    public class ВидиЦін_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЦін_Select ВидиЦін_Select = new Довідники.ВидиЦін_Select();
            ВидиЦін_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ВидиЦін_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ВидиЦін_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ВидиЦін_Select.QuerySelect.Order.Add(Довідники.ВидиЦін_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЦін_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВидиЦін_Select.MoveNext())
            {
                Довідники.ВидиЦін_Pointer? cur = ВидиЦін_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВидиЦін_ЗаписиШвидкийВибір Record = new ВидиЦін_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[ВидиЦін_Const.Назва].ToString() ?? "",
                        
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
    public class ВидиЦінПостачальників_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЦінПостачальників_Select ВидиЦінПостачальників_Select = new Довідники.ВидиЦінПостачальників_Select();
            ВидиЦінПостачальників_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ВидиЦінПостачальників_Const.Код,
                /*Назва*/ Довідники.ВидиЦінПостачальників_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ВидиЦінПостачальників_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ВидиЦінПостачальників_Select.QuerySelect.Order.Add(Довідники.ВидиЦінПостачальників_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЦінПостачальників_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВидиЦінПостачальників_Select.MoveNext())
            {
                Довідники.ВидиЦінПостачальників_Pointer? cur = ВидиЦінПостачальників_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВидиЦінПостачальників_Записи Record = new ВидиЦінПостачальників_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[ВидиЦінПостачальників_Const.Код].ToString() ?? "",
                        Назва = Fields[ВидиЦінПостачальників_Const.Назва].ToString() ?? "",
                        
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
    public class ВидиЦінПостачальників_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЦінПостачальників_Select ВидиЦінПостачальників_Select = new Довідники.ВидиЦінПостачальників_Select();
            ВидиЦінПостачальників_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ВидиЦінПостачальників_Const.Код,
                /*Назва*/ Довідники.ВидиЦінПостачальників_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ВидиЦінПостачальників_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ВидиЦінПостачальників_Select.QuerySelect.Order.Add(Довідники.ВидиЦінПостачальників_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЦінПостачальників_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВидиЦінПостачальників_Select.MoveNext())
            {
                Довідники.ВидиЦінПостачальників_Pointer? cur = ВидиЦінПостачальників_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВидиЦінПостачальників_ЗаписиШвидкийВибір Record = new ВидиЦінПостачальників_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[ВидиЦінПостачальників_Const.Код].ToString() ?? "",
                        Назва = Fields[ВидиЦінПостачальників_Const.Назва].ToString() ?? "",
                        
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
    public class Користувачі_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Користувачі_Select Користувачі_Select = new Довідники.Користувачі_Select();
            Користувачі_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Користувачі_Const.Код,
                /*Назва*/ Довідники.Користувачі_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Користувачі_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Користувачі_Select.QuerySelect.Order.Add(Довідники.Користувачі_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Користувачі_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Користувачі_Select.MoveNext())
            {
                Довідники.Користувачі_Pointer? cur = Користувачі_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Користувачі_Записи Record = new Користувачі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Користувачі_Const.Код].ToString() ?? "",
                        Назва = Fields[Користувачі_Const.Назва].ToString() ?? "",
                        
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
    public class Користувачі_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Користувачі_Select Користувачі_Select = new Довідники.Користувачі_Select();
            Користувачі_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.Користувачі_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Користувачі_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Користувачі_Select.QuerySelect.Order.Add(Довідники.Користувачі_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Користувачі_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Користувачі_Select.MoveNext())
            {
                Довідники.Користувачі_Pointer? cur = Користувачі_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Користувачі_ЗаписиШвидкийВибір Record = new Користувачі_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[Користувачі_Const.Назва].ToString() ?? "",
                        
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
    public class ФізичніОсоби_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ФізичніОсоби_Select ФізичніОсоби_Select = new Довідники.ФізичніОсоби_Select();
            ФізичніОсоби_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ФізичніОсоби_Const.Код,
                /*Назва*/ Довідники.ФізичніОсоби_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ФізичніОсоби_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ФізичніОсоби_Select.QuerySelect.Order.Add(Довідники.ФізичніОсоби_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ФізичніОсоби_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ФізичніОсоби_Select.MoveNext())
            {
                Довідники.ФізичніОсоби_Pointer? cur = ФізичніОсоби_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ФізичніОсоби_Записи Record = new ФізичніОсоби_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[ФізичніОсоби_Const.Код].ToString() ?? "",
                        Назва = Fields[ФізичніОсоби_Const.Назва].ToString() ?? "",
                        
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
    public class ФізичніОсоби_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ФізичніОсоби_Select ФізичніОсоби_Select = new Довідники.ФізичніОсоби_Select();
            ФізичніОсоби_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ФізичніОсоби_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ФізичніОсоби_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ФізичніОсоби_Select.QuerySelect.Order.Add(Довідники.ФізичніОсоби_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ФізичніОсоби_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ФізичніОсоби_Select.MoveNext())
            {
                Довідники.ФізичніОсоби_Pointer? cur = ФізичніОсоби_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ФізичніОсоби_ЗаписиШвидкийВибір Record = new ФізичніОсоби_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[ФізичніОсоби_Const.Назва].ToString() ?? "",
                        
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
    public class СтруктураПідприємства_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СтруктураПідприємства_Select СтруктураПідприємства_Select = new Довідники.СтруктураПідприємства_Select();
            СтруктураПідприємства_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.СтруктураПідприємства_Const.Код,
                /*Назва*/ Довідники.СтруктураПідприємства_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СтруктураПідприємства_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              СтруктураПідприємства_Select.QuerySelect.Order.Add(Довідники.СтруктураПідприємства_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await СтруктураПідприємства_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СтруктураПідприємства_Select.MoveNext())
            {
                Довідники.СтруктураПідприємства_Pointer? cur = СтруктураПідприємства_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СтруктураПідприємства_Записи Record = new СтруктураПідприємства_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[СтруктураПідприємства_Const.Код].ToString() ?? "",
                        Назва = Fields[СтруктураПідприємства_Const.Назва].ToString() ?? "",
                        
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
    public class СтруктураПідприємства_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СтруктураПідприємства_Select СтруктураПідприємства_Select = new Довідники.СтруктураПідприємства_Select();
            СтруктураПідприємства_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.СтруктураПідприємства_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СтруктураПідприємства_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              СтруктураПідприємства_Select.QuerySelect.Order.Add(Довідники.СтруктураПідприємства_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await СтруктураПідприємства_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СтруктураПідприємства_Select.MoveNext())
            {
                Довідники.СтруктураПідприємства_Pointer? cur = СтруктураПідприємства_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СтруктураПідприємства_ЗаписиШвидкийВибір Record = new СтруктураПідприємства_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[СтруктураПідприємства_Const.Назва].ToString() ?? "",
                        
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
    public class КраїниСвіту_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.КраїниСвіту_Select КраїниСвіту_Select = new Довідники.КраїниСвіту_Select();
            КраїниСвіту_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.КраїниСвіту_Const.Код,
                /*Назва*/ Довідники.КраїниСвіту_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) КраїниСвіту_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              КраїниСвіту_Select.QuerySelect.Order.Add(Довідники.КраїниСвіту_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await КраїниСвіту_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (КраїниСвіту_Select.MoveNext())
            {
                Довідники.КраїниСвіту_Pointer? cur = КраїниСвіту_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    КраїниСвіту_Записи Record = new КраїниСвіту_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[КраїниСвіту_Const.Код].ToString() ?? "",
                        Назва = Fields[КраїниСвіту_Const.Назва].ToString() ?? "",
                        
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
    public class КраїниСвіту_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.КраїниСвіту_Select КраїниСвіту_Select = new Довідники.КраїниСвіту_Select();
            КраїниСвіту_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.КраїниСвіту_Const.Код,
                /*Назва*/ Довідники.КраїниСвіту_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) КраїниСвіту_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              КраїниСвіту_Select.QuerySelect.Order.Add(Довідники.КраїниСвіту_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await КраїниСвіту_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (КраїниСвіту_Select.MoveNext())
            {
                Довідники.КраїниСвіту_Pointer? cur = КраїниСвіту_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    КраїниСвіту_ЗаписиШвидкийВибір Record = new КраїниСвіту_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[КраїниСвіту_Const.Код].ToString() ?? "",
                        Назва = Fields[КраїниСвіту_Const.Назва].ToString() ?? "",
                        
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
    public class Файли_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string НазваФайлу = "";
        string Розмір = "";
        string ДатаСтворення = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*НазваФайлу*/ НазваФайлу,
                /*Розмір*/ Розмір,
                /*ДатаСтворення*/ ДатаСтворення,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*НазваФайлу*/ typeof(string),  
                /*Розмір*/ typeof(string),  
                /*ДатаСтворення*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Назва файлу", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*НазваФайлу*/
            treeView.AppendColumn(new TreeViewColumn("Розмір", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Розмір*/
            treeView.AppendColumn(new TreeViewColumn("Дата створення", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ДатаСтворення*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Файли_Select Файли_Select = new Довідники.Файли_Select();
            Файли_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Файли_Const.Код,
                /*Назва*/ Довідники.Файли_Const.Назва,
                /*НазваФайлу*/ Довідники.Файли_Const.НазваФайлу,
                /*Розмір*/ Довідники.Файли_Const.Розмір,
                /*ДатаСтворення*/ Довідники.Файли_Const.ДатаСтворення,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Файли_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Файли_Select.QuerySelect.Order.Add(Довідники.Файли_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Файли_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Файли_Select.MoveNext())
            {
                Довідники.Файли_Pointer? cur = Файли_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Файли_Записи Record = new Файли_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Файли_Const.Код].ToString() ?? "",
                        Назва = Fields[Файли_Const.Назва].ToString() ?? "",
                        НазваФайлу = Fields[Файли_Const.НазваФайлу].ToString() ?? "",
                        Розмір = Fields[Файли_Const.Розмір].ToString() ?? "",
                        ДатаСтворення = Fields[Файли_Const.ДатаСтворення].ToString() ?? "",
                        
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
    public class Файли_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Файли_Select Файли_Select = new Довідники.Файли_Select();
            Файли_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.Файли_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Файли_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Файли_Select.QuerySelect.Order.Add(Довідники.Файли_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Файли_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Файли_Select.MoveNext())
            {
                Довідники.Файли_Pointer? cur = Файли_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Файли_ЗаписиШвидкийВибір Record = new Файли_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[Файли_Const.Назва].ToString() ?? "",
                        
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
    public class ХарактеристикиНоменклатури_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Номенклатура = "";
        string Назва = "";
        string Залишки = "";
        string ВРезерві = "";
        string ВРезервіПідЗамовлення = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Номенклатура*/ Номенклатура,
                /*Назва*/ Назва,
                /*Залишки*/ Залишки,
                /*ВРезерві*/ ВРезерві,
                /*ВРезервіПідЗамовлення*/ ВРезервіПідЗамовлення,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Номенклатура*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Залишки*/ typeof(string), 
                /*ВРезерві*/ typeof(string), 
                /*ВРезервіПідЗамовлення*/ typeof(string), 
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Назва*/
            

            /* Додаткові поля */
            treeView.AppendColumn(new TreeViewColumn("Залишки", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Залишки*/
            treeView.AppendColumn(new TreeViewColumn("В резерві", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ВРезерві*/
            treeView.AppendColumn(new TreeViewColumn("Під замовлення", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*ВРезервіПідЗамовлення*/
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ХарактеристикиНоменклатури_Select ХарактеристикиНоменклатури_Select = new Довідники.ХарактеристикиНоменклатури_Select();
            ХарактеристикиНоменклатури_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ХарактеристикиНоменклатури_Const.Код,
                /*Назва*/ Довідники.ХарактеристикиНоменклатури_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ХарактеристикиНоменклатури_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ХарактеристикиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ХарактеристикиНоменклатури_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                ХарактеристикиНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Номенклатура_Const.TABLE, Довідники.ХарактеристикиНоменклатури_Const.Номенклатура, ХарактеристикиНоменклатури_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Номенклатура_Const.Назва, "join_tab_1_field_1"));
                  
                /* Additional Field */
                ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури, SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності}) AS ВНаявності FROM {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах WHERE ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid GROUP BY ХарактеристикаНоменклатури ) SELECT ROUND(ВНаявності, 1) FROM Залишки)", "Залишки"));
                /*
                WITH Залишки AS
(
    SELECT
        ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності}) AS ВНаявності
    FROM
        {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах
    WHERE
        ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND
        ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid
    GROUP BY ХарактеристикаНоменклатури
)
SELECT
    ROUND(ВНаявності, 1)
FROM
    Залишки
                */
            
                /* Additional Field */
                ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу}) AS ВРезервіЗіСкладу FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid GROUP BY ХарактеристикаНоменклатури ) SELECT ROUND(ВРезервіЗіСкладу, 1) FROM Залишки)", "ВРезерві"));
                /*
                WITH Залишки AS
(
    SELECT
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу}) AS ВРезервіЗіСкладу
    FROM
        {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
    WHERE
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid
    GROUP BY ХарактеристикаНоменклатури
)
SELECT
    ROUND(ВРезервіЗіСкладу, 1)
FROM
    Залишки
                */
            
                /* Additional Field */
                ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення}) AS ВРезервіПідЗамовлення FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid GROUP BY ХарактеристикаНоменклатури ) SELECT ROUND(ВРезервіПідЗамовлення, 1) FROM Залишки)", "ВРезервіПідЗамовлення"));
                /*
                WITH Залишки AS
(
    SELECT
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення}) AS ВРезервіПідЗамовлення
    FROM
        {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
    WHERE
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid
    GROUP BY ХарактеристикаНоменклатури
)
SELECT
    ROUND(ВРезервіПідЗамовлення, 1)
FROM
    Залишки
                */
            

            /* SELECT */
            await ХарактеристикиНоменклатури_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ХарактеристикиНоменклатури_Select.MoveNext())
            {
                Довідники.ХарактеристикиНоменклатури_Pointer? cur = ХарактеристикиНоменклатури_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ХарактеристикиНоменклатури_Записи Record = new ХарактеристикиНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Номенклатура = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[ХарактеристикиНоменклатури_Const.Код].ToString() ?? "",
                        Назва = Fields[ХарактеристикиНоменклатури_Const.Назва].ToString() ?? "",
                        Залишки = Fields["Залишки"].ToString() ?? "",
                        ВРезерві = Fields["ВРезерві"].ToString() ?? "",
                        ВРезервіПідЗамовлення = Fields["ВРезервіПідЗамовлення"].ToString() ?? "",
                        
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
    public class ХарактеристикиНоменклатури_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Номенклатура = "";
        string Назва = "";
        string Залишки = "";
        string ВРезерві = "";
        string ВРезервіПідЗамовлення = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Номенклатура*/ Номенклатура,
                /*Назва*/ Назва,
                /*Залишки*/ Залишки,
                /*ВРезерві*/ ВРезерві,
                /*ВРезервіПідЗамовлення*/ ВРезервіПідЗамовлення,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Номенклатура*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Залишки*/ typeof(string), 
                /*ВРезерві*/ typeof(string), 
                /*ВРезервіПідЗамовлення*/ typeof(string), 
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Назва*/
            

            /* Додаткові поля */
            treeView.AppendColumn(new TreeViewColumn("Залишки", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Залишки*/
            treeView.AppendColumn(new TreeViewColumn("В резерві", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ВРезерві*/
            treeView.AppendColumn(new TreeViewColumn("Під замовлення", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*ВРезервіПідЗамовлення*/
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ХарактеристикиНоменклатури_Select ХарактеристикиНоменклатури_Select = new Довідники.ХарактеристикиНоменклатури_Select();
            ХарактеристикиНоменклатури_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ХарактеристикиНоменклатури_Const.Код,
                /*Назва*/ Довідники.ХарактеристикиНоменклатури_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ХарактеристикиНоменклатури_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ХарактеристикиНоменклатури_Select.QuerySelect.Order.Add(Довідники.ХарактеристикиНоменклатури_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                ХарактеристикиНоменклатури_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Номенклатура_Const.TABLE, Довідники.ХарактеристикиНоменклатури_Const.Номенклатура, ХарактеристикиНоменклатури_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Номенклатура_Const.Назва, "join_tab_1_field_1"));
                  
                /* Additional Field */
                ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури, SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності}) AS ВНаявності FROM {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах WHERE ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid GROUP BY ХарактеристикаНоменклатури ) SELECT ROUND(ВНаявності, 1) FROM Залишки)", "Залишки"));
                /*
                WITH Залишки AS
(
    SELECT
        ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності}) AS ВНаявності
    FROM
        {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах
    WHERE
        ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND
        ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid
    GROUP BY ХарактеристикаНоменклатури
)
SELECT
    ROUND(ВНаявності, 1)
FROM
    Залишки
                */
            
                /* Additional Field */
                ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу}) AS ВРезервіЗіСкладу FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid GROUP BY ХарактеристикаНоменклатури ) SELECT ROUND(ВРезервіЗіСкладу, 1) FROM Залишки)", "ВРезерві"));
                /*
                WITH Залишки AS
(
    SELECT
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу}) AS ВРезервіЗіСкладу
    FROM
        {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
    WHERE
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid
    GROUP BY ХарактеристикаНоменклатури
)
SELECT
    ROUND(ВРезервіЗіСкладу, 1)
FROM
    Залишки
                */
            
                /* Additional Field */
                ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення}) AS ВРезервіПідЗамовлення FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid GROUP BY ХарактеристикаНоменклатури ) SELECT ROUND(ВРезервіПідЗамовлення, 1) FROM Залишки)", "ВРезервіПідЗамовлення"));
                /*
                WITH Залишки AS
(
    SELECT
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS ХарактеристикаНоменклатури,
        SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення}) AS ВРезервіПідЗамовлення
    FROM
        {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
    WHERE
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid
    GROUP BY ХарактеристикаНоменклатури
)
SELECT
    ROUND(ВРезервіПідЗамовлення, 1)
FROM
    Залишки
                */
            

            /* SELECT */
            await ХарактеристикиНоменклатури_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ХарактеристикиНоменклатури_Select.MoveNext())
            {
                Довідники.ХарактеристикиНоменклатури_Pointer? cur = ХарактеристикиНоменклатури_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ХарактеристикиНоменклатури_ЗаписиШвидкийВибір Record = new ХарактеристикиНоменклатури_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Номенклатура = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[ХарактеристикиНоменклатури_Const.Код].ToString() ?? "",
                        Назва = Fields[ХарактеристикиНоменклатури_Const.Назва].ToString() ?? "",
                        Залишки = Fields["Залишки"].ToString() ?? "",
                        ВРезерві = Fields["ВРезерві"].ToString() ?? "",
                        ВРезервіПідЗамовлення = Fields["ВРезервіПідЗамовлення"].ToString() ?? "",
                        
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
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

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
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer )
        {
            RootPath = SelectPath = null;

            Номенклатура_Папки_Записи rootRecord = new Номенклатура_Папки_Записи
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

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
    {Номенклатура_Папки_Const.Назва} AS Назва, 
    {Номенклатура_Папки_Const.Родич} AS Родич, 
    level,
    deletion_label
FROM r
ORDER BY level, {Номенклатура_Папки_Const.Назва} ASC
";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

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
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

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
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer )
        {
            RootPath = SelectPath = null;

            Номенклатура_Папки_ЗаписиШвидкийВибір rootRecord = new Номенклатура_Папки_ЗаписиШвидкийВибір
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

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
    {Номенклатура_Папки_Const.Назва} AS Назва, 
    {Номенклатура_Папки_Const.Родич} AS Родич, 
    level,
    deletion_label
FROM r
ORDER BY level, {Номенклатура_Папки_Const.Назва} ASC
";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

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
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

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
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer )
        {
            RootPath = SelectPath = null;

            Контрагенти_Папки_Записи rootRecord = new Контрагенти_Папки_Записи
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

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
    {Контрагенти_Папки_Const.Назва} AS Назва, 
    {Контрагенти_Папки_Const.Родич} AS Родич, 
    level,
    deletion_label
FROM r
ORDER BY level, {Контрагенти_Папки_Const.Назва} ASC
";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

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
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

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
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer )
        {
            RootPath = SelectPath = null;

            Контрагенти_Папки_ЗаписиШвидкийВибір rootRecord = new Контрагенти_Папки_ЗаписиШвидкийВибір
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

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
    {Контрагенти_Папки_Const.Назва} AS Назва, 
    {Контрагенти_Папки_Const.Родич} AS Родич, 
    level,
    deletion_label
FROM r
ORDER BY level, {Контрагенти_Папки_Const.Назва} ASC
";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

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
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

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
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer )
        {
            RootPath = SelectPath = null;

            Склади_Папки_Записи rootRecord = new Склади_Папки_Записи
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

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
    {Склади_Папки_Const.Назва} AS Назва, 
    {Склади_Папки_Const.Родич} AS Родич, 
    level,
    deletion_label
FROM r
ORDER BY level, {Склади_Папки_Const.Назва} ASC
";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

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
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

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
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer )
        {
            RootPath = SelectPath = null;

            Склади_Папки_ЗаписиШвидкийВибір rootRecord = new Склади_Папки_ЗаписиШвидкийВибір
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

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
    {Склади_Папки_Const.Назва} AS Назва, 
    {Склади_Папки_Const.Родич} AS Родич, 
    level,
    deletion_label
FROM r
ORDER BY level, {Склади_Папки_Const.Назва} ASC
";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

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
    public class Каси_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";
        string Залишок = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Валюта*/ Валюта,
                /*Залишок*/ Залишок,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Залишок*/ typeof(string), 
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            

            /* Додаткові поля */
            treeView.AppendColumn(new TreeViewColumn("Залишок", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Залишок*/
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Каси_Select Каси_Select = new Довідники.Каси_Select();
            Каси_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Каси_Const.Код,
                /*Назва*/ Довідники.Каси_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Каси_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Каси_Select.QuerySelect.Order.Add(Довідники.Каси_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                Каси_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.Каси_Const.Валюта, Каси_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  Каси_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Валюти_Const.Назва, "join_tab_1_field_1"));
                  
                /* Additional Field */
                Каси_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(SELECT ROUND(РухКоштів.{РегістриНакопичення.РухКоштів_Підсумки_TablePart.Сума}, 2) AS Сума FROM {РегістриНакопичення.РухКоштів_Підсумки_TablePart.TABLE} AS РухКоштів WHERE РухКоштів.{РегістриНакопичення.РухКоштів_Підсумки_TablePart.Каса} = {Довідники.Каси_Const.TABLE}.uid)", "Залишок"));
                /*
                SELECT
        ROUND(РухКоштів.{РегістриНакопичення.РухКоштів_Підсумки_TablePart.Сума}, 2) AS Сума
FROM
        {РегістриНакопичення.РухКоштів_Підсумки_TablePart.TABLE} AS РухКоштів
WHERE
        РухКоштів.{РегістриНакопичення.РухКоштів_Підсумки_TablePart.Каса} = {Довідники.Каси_Const.TABLE}.uid
                */
            

            /* SELECT */
            await Каси_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Каси_Select.MoveNext())
            {
                Довідники.Каси_Pointer? cur = Каси_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Каси_Записи Record = new Каси_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Валюта = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[Каси_Const.Код].ToString() ?? "",
                        Назва = Fields[Каси_Const.Назва].ToString() ?? "",
                        Залишок = Fields["Залишок"].ToString() ?? "",
                        
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
    public class Каси_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";
        string Залишок = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Валюта*/ Валюта,
                /*Залишок*/ Залишок,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Залишок*/ typeof(string), 
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            

            /* Додаткові поля */
            treeView.AppendColumn(new TreeViewColumn("Залишок", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Залишок*/
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Каси_Select Каси_Select = new Довідники.Каси_Select();
            Каси_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Каси_Const.Код,
                /*Назва*/ Довідники.Каси_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Каси_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Каси_Select.QuerySelect.Order.Add(Довідники.Каси_Const.Назва, SelectOrder.ASC);
            
                /* Join Table */
                Каси_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, Довідники.Каси_Const.Валюта, Каси_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  Каси_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Валюти_Const.Назва, "join_tab_1_field_1"));
                  
                /* Additional Field */
                Каси_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(SELECT ROUND(РухКоштів.{РегістриНакопичення.РухКоштів_Підсумки_TablePart.Сума}, 2) AS Сума FROM {РегістриНакопичення.РухКоштів_Підсумки_TablePart.TABLE} AS РухКоштів WHERE РухКоштів.{РегістриНакопичення.РухКоштів_Підсумки_TablePart.Каса} = {Довідники.Каси_Const.TABLE}.uid)", "Залишок"));
                /*
                SELECT
        ROUND(РухКоштів.{РегістриНакопичення.РухКоштів_Підсумки_TablePart.Сума}, 2) AS Сума
FROM
        {РегістриНакопичення.РухКоштів_Підсумки_TablePart.TABLE} AS РухКоштів
WHERE
        РухКоштів.{РегістриНакопичення.РухКоштів_Підсумки_TablePart.Каса} = {Довідники.Каси_Const.TABLE}.uid
                */
            

            /* SELECT */
            await Каси_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Каси_Select.MoveNext())
            {
                Довідники.Каси_Pointer? cur = Каси_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Каси_ЗаписиШвидкийВибір Record = new Каси_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Валюта = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[Каси_Const.Код].ToString() ?? "",
                        Назва = Fields[Каси_Const.Назва].ToString() ?? "",
                        Залишок = Fields["Залишок"].ToString() ?? "",
                        
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
    public class БанківськіРахункиОрганізацій_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Валюта*/ Валюта,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.БанківськіРахункиОрганізацій_Select БанківськіРахункиОрганізацій_Select = new Довідники.БанківськіРахункиОрганізацій_Select();
            БанківськіРахункиОрганізацій_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.БанківськіРахункиОрганізацій_Const.Код,
                /*Назва*/ Довідники.БанківськіРахункиОрганізацій_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) БанківськіРахункиОрганізацій_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (БанківськіРахункиОрганізацій_Select.MoveNext())
            {
                Довідники.БанківськіРахункиОрганізацій_Pointer? cur = БанківськіРахункиОрганізацій_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    БанківськіРахункиОрганізацій_Записи Record = new БанківськіРахункиОрганізацій_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Валюта = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[БанківськіРахункиОрганізацій_Const.Код].ToString() ?? "",
                        Назва = Fields[БанківськіРахункиОрганізацій_Const.Назва].ToString() ?? "",
                        
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
    public class БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Валюта*/ Валюта,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.БанківськіРахункиОрганізацій_Select БанківськіРахункиОрганізацій_Select = new Довідники.БанківськіРахункиОрганізацій_Select();
            БанківськіРахункиОрганізацій_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.БанківськіРахункиОрганізацій_Const.Код,
                /*Назва*/ Довідники.БанківськіРахункиОрганізацій_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) БанківськіРахункиОрганізацій_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (БанківськіРахункиОрганізацій_Select.MoveNext())
            {
                Довідники.БанківськіРахункиОрганізацій_Pointer? cur = БанківськіРахункиОрганізацій_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір Record = new БанківськіРахункиОрганізацій_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Валюта = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[БанківськіРахункиОрганізацій_Const.Код].ToString() ?? "",
                        Назва = Fields[БанківськіРахункиОрганізацій_Const.Назва].ToString() ?? "",
                        
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
    public class ДоговориКонтрагентів_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Контрагент = "";
        string ТипДоговору = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Контрагент*/ Контрагент,
                /*ТипДоговору*/ ТипДоговору,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*ТипДоговору*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ТипДоговору*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ДоговориКонтрагентів_Select ДоговориКонтрагентів_Select = new Довідники.ДоговориКонтрагентів_Select();
            ДоговориКонтрагентів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ДоговориКонтрагентів_Const.Код,
                /*Назва*/ Довідники.ДоговориКонтрагентів_Const.Назва,
                /*ТипДоговору*/ Довідники.ДоговориКонтрагентів_Const.ТипДоговору,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ДоговориКонтрагентів_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ДоговориКонтрагентів_Select.MoveNext())
            {
                Довідники.ДоговориКонтрагентів_Pointer? cur = ДоговориКонтрагентів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ДоговориКонтрагентів_Записи Record = new ДоговориКонтрагентів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Контрагент = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[ДоговориКонтрагентів_Const.Код].ToString() ?? "",
                        Назва = Fields[ДоговориКонтрагентів_Const.Назва].ToString() ?? "",
                        ТипДоговору = Перелічення.ПсевдонімиПерелічення.ТипДоговорів_Alias( ((Перелічення.ТипДоговорів)(Fields[ДоговориКонтрагентів_Const.ТипДоговору] != DBNull.Value ? Fields[ДоговориКонтрагентів_Const.ТипДоговору] : 0)) ),
                        
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
    public class ДоговориКонтрагентів_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Контрагент = "";
        string ТипДоговору = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Контрагент*/ Контрагент,
                /*ТипДоговору*/ ТипДоговору,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*ТипДоговору*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ТипДоговору*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ДоговориКонтрагентів_Select ДоговориКонтрагентів_Select = new Довідники.ДоговориКонтрагентів_Select();
            ДоговориКонтрагентів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ДоговориКонтрагентів_Const.Назва,
                /*ТипДоговору*/ Довідники.ДоговориКонтрагентів_Const.ТипДоговору,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ДоговориКонтрагентів_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ДоговориКонтрагентів_Select.MoveNext())
            {
                Довідники.ДоговориКонтрагентів_Pointer? cur = ДоговориКонтрагентів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ДоговориКонтрагентів_ЗаписиШвидкийВибір Record = new ДоговориКонтрагентів_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Контрагент = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Назва = Fields[ДоговориКонтрагентів_Const.Назва].ToString() ?? "",
                        ТипДоговору = Перелічення.ПсевдонімиПерелічення.ТипДоговорів_Alias( ((Перелічення.ТипДоговорів)(Fields[ДоговориКонтрагентів_Const.ТипДоговору] != DBNull.Value ? Fields[ДоговориКонтрагентів_Const.ТипДоговору] : 0)) ),
                        
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
    public class БанківськіРахункиКонтрагентів_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Валюта*/ Валюта,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.БанківськіРахункиКонтрагентів_Select БанківськіРахункиКонтрагентів_Select = new Довідники.БанківськіРахункиКонтрагентів_Select();
            БанківськіРахункиКонтрагентів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.БанківськіРахункиКонтрагентів_Const.Код,
                /*Назва*/ Довідники.БанківськіРахункиКонтрагентів_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) БанківськіРахункиКонтрагентів_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (БанківськіРахункиКонтрагентів_Select.MoveNext())
            {
                Довідники.БанківськіРахункиКонтрагентів_Pointer? cur = БанківськіРахункиКонтрагентів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    БанківськіРахункиКонтрагентів_Записи Record = new БанківськіРахункиКонтрагентів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Валюта = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[БанківськіРахункиКонтрагентів_Const.Код].ToString() ?? "",
                        Назва = Fields[БанківськіРахункиКонтрагентів_Const.Назва].ToString() ?? "",
                        
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
    public class БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string Валюта = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Валюта*/ Валюта,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Валюта*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.БанківськіРахункиКонтрагентів_Select БанківськіРахункиКонтрагентів_Select = new Довідники.БанківськіРахункиКонтрагентів_Select();
            БанківськіРахункиКонтрагентів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.БанківськіРахункиКонтрагентів_Const.Код,
                /*Назва*/ Довідники.БанківськіРахункиКонтрагентів_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) БанківськіРахункиКонтрагентів_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (БанківськіРахункиКонтрагентів_Select.MoveNext())
            {
                Довідники.БанківськіРахункиКонтрагентів_Pointer? cur = БанківськіРахункиКонтрагентів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір Record = new БанківськіРахункиКонтрагентів_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Валюта = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Код = Fields[БанківськіРахункиКонтрагентів_Const.Код].ToString() ?? "",
                        Назва = Fields[БанківськіРахункиКонтрагентів_Const.Назва].ToString() ?? "",
                        
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
    public class СтаттяРухуКоштів_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Код = "";
        string КореспондуючийРахунок = "";
        string ВидРухуКоштів = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Код*/ Код,
                /*КореспондуючийРахунок*/ КореспондуючийРахунок,
                /*ВидРухуКоштів*/ ВидРухуКоштів,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Код*/ typeof(string),  
                /*КореспондуючийРахунок*/ typeof(string),  
                /*ВидРухуКоштів*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("КореспондуючийРахунок", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*КореспондуючийРахунок*/
            treeView.AppendColumn(new TreeViewColumn("ВидРухуКоштів", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ВидРухуКоштів*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СтаттяРухуКоштів_Select СтаттяРухуКоштів_Select = new Довідники.СтаттяРухуКоштів_Select();
            СтаттяРухуКоштів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.СтаттяРухуКоштів_Const.Назва,
                /*Код*/ Довідники.СтаттяРухуКоштів_Const.Код,
                /*КореспондуючийРахунок*/ Довідники.СтаттяРухуКоштів_Const.КореспондуючийРахунок,
                /*ВидРухуКоштів*/ Довідники.СтаттяРухуКоштів_Const.ВидРухуКоштів,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СтаттяРухуКоштів_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              СтаттяРухуКоштів_Select.QuerySelect.Order.Add(Довідники.СтаттяРухуКоштів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await СтаттяРухуКоштів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СтаттяРухуКоштів_Select.MoveNext())
            {
                Довідники.СтаттяРухуКоштів_Pointer? cur = СтаттяРухуКоштів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СтаттяРухуКоштів_Записи Record = new СтаттяРухуКоштів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[СтаттяРухуКоштів_Const.Назва].ToString() ?? "",
                        Код = Fields[СтаттяРухуКоштів_Const.Код].ToString() ?? "",
                        КореспондуючийРахунок = Fields[СтаттяРухуКоштів_Const.КореспондуючийРахунок].ToString() ?? "",
                        ВидРухуКоштів = Перелічення.ПсевдонімиПерелічення.ВидиРухуКоштів_Alias( ((Перелічення.ВидиРухуКоштів)(Fields[СтаттяРухуКоштів_Const.ВидРухуКоштів] != DBNull.Value ? Fields[СтаттяРухуКоштів_Const.ВидРухуКоштів] : 0)) ),
                        
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
    public class СтаттяРухуКоштів_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СтаттяРухуКоштів_Select СтаттяРухуКоштів_Select = new Довідники.СтаттяРухуКоштів_Select();
            СтаттяРухуКоштів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.СтаттяРухуКоштів_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СтаттяРухуКоштів_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              СтаттяРухуКоштів_Select.QuerySelect.Order.Add(Довідники.СтаттяРухуКоштів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await СтаттяРухуКоштів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СтаттяРухуКоштів_Select.MoveNext())
            {
                Довідники.СтаттяРухуКоштів_Pointer? cur = СтаттяРухуКоштів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СтаттяРухуКоштів_ЗаписиШвидкийВибір Record = new СтаттяРухуКоштів_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[СтаттяРухуКоштів_Const.Назва].ToString() ?? "",
                        
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
    public class СеріїНоменклатури_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Номер = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Номер*/ Номер,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Номер*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Номер*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СеріїНоменклатури_Select СеріїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            СеріїНоменклатури_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Номер*/ Довідники.СеріїНоменклатури_Const.Номер,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СеріїНоменклатури_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              СеріїНоменклатури_Select.QuerySelect.Order.Add(Довідники.СеріїНоменклатури_Const.Номер, SelectOrder.ASC);
            

            /* SELECT */
            await СеріїНоменклатури_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СеріїНоменклатури_Select.MoveNext())
            {
                Довідники.СеріїНоменклатури_Pointer? cur = СеріїНоменклатури_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СеріїНоменклатури_Записи Record = new СеріїНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Номер = Fields[СеріїНоменклатури_Const.Номер].ToString() ?? "",
                        
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
    public class СеріїНоменклатури_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Номер = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Номер*/ Номер,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Номер*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Номер*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СеріїНоменклатури_Select СеріїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            СеріїНоменклатури_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Номер*/ Довідники.СеріїНоменклатури_Const.Номер,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СеріїНоменклатури_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              СеріїНоменклатури_Select.QuerySelect.Order.Add(Довідники.СеріїНоменклатури_Const.Номер, SelectOrder.ASC);
            

            /* SELECT */
            await СеріїНоменклатури_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СеріїНоменклатури_Select.MoveNext())
            {
                Довідники.СеріїНоменклатури_Pointer? cur = СеріїНоменклатури_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СеріїНоменклатури_ЗаписиШвидкийВибір Record = new СеріїНоменклатури_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Номер = Fields[СеріїНоменклатури_Const.Номер].ToString() ?? "",
                        
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
    public class ПартіяТоварівКомпозит_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Дата = "";
        string ТипДокументу = "";
        string ПоступленняТоварівТаПослуг = "";
        string ВведенняЗалишків = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Дата*/ Дата,
                /*ТипДокументу*/ ТипДокументу,
                /*ПоступленняТоварівТаПослуг*/ ПоступленняТоварівТаПослуг,
                /*ВведенняЗалишків*/ ВведенняЗалишків,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Дата*/ typeof(string),  
                /*ТипДокументу*/ typeof(string),  
                /*ПоступленняТоварівТаПослуг*/ typeof(string),  
                /*ВведенняЗалишків*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Дата*/
            treeView.AppendColumn(new TreeViewColumn("ТипДокументу", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ТипДокументу*/
            treeView.AppendColumn(new TreeViewColumn("ПоступленняТоварівТаПослуг", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ПоступленняТоварівТаПослуг*/
            treeView.AppendColumn(new TreeViewColumn("ВведенняЗалишків", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ВведенняЗалишків*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ПартіяТоварівКомпозит_Select ПартіяТоварівКомпозит_Select = new Довідники.ПартіяТоварівКомпозит_Select();
            ПартіяТоварівКомпозит_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ПартіяТоварівКомпозит_Const.Назва,
                /*Дата*/ Довідники.ПартіяТоварівКомпозит_Const.Дата,
                /*ТипДокументу*/ Довідники.ПартіяТоварівКомпозит_Const.ТипДокументу,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ПартіяТоварівКомпозит_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПартіяТоварівКомпозит_Select.MoveNext())
            {
                Довідники.ПартіяТоварівКомпозит_Pointer? cur = ПартіяТоварівКомпозит_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПартіяТоварівКомпозит_Записи Record = new ПартіяТоварівКомпозит_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        ПоступленняТоварівТаПослуг = Fields["join_tab_1_field_1"].ToString() ?? "",
                        ВведенняЗалишків = Fields["join_tab_2_field_1"].ToString() ?? "",
                        Назва = Fields[ПартіяТоварівКомпозит_Const.Назва].ToString() ?? "",
                        Дата = Fields[ПартіяТоварівКомпозит_Const.Дата].ToString() ?? "",
                        ТипДокументу = Перелічення.ПсевдонімиПерелічення.ТипДокументуПартіяТоварівКомпозит_Alias( ((Перелічення.ТипДокументуПартіяТоварівКомпозит)(Fields[ПартіяТоварівКомпозит_Const.ТипДокументу] != DBNull.Value ? Fields[ПартіяТоварівКомпозит_Const.ТипДокументу] : 0)) ),
                        
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
    public class ПартіяТоварівКомпозит_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Дата = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Дата*/ Дата,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Дата*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Дата*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ПартіяТоварівКомпозит_Select ПартіяТоварівКомпозит_Select = new Довідники.ПартіяТоварівКомпозит_Select();
            ПартіяТоварівКомпозит_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ПартіяТоварівКомпозит_Const.Назва,
                /*Дата*/ Довідники.ПартіяТоварівКомпозит_Const.Дата,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ПартіяТоварівКомпозит_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ПартіяТоварівКомпозит_Select.QuerySelect.Order.Add(Довідники.ПартіяТоварівКомпозит_Const.Дата, SelectOrder.ASC);
            

            /* SELECT */
            await ПартіяТоварівКомпозит_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПартіяТоварівКомпозит_Select.MoveNext())
            {
                Довідники.ПартіяТоварівКомпозит_Pointer? cur = ПартіяТоварівКомпозит_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПартіяТоварівКомпозит_ЗаписиШвидкийВибір Record = new ПартіяТоварівКомпозит_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[ПартіяТоварівКомпозит_Const.Назва].ToString() ?? "",
                        Дата = Fields[ПартіяТоварівКомпозит_Const.Дата].ToString() ?? "",
                        
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
    public class ВидиЗапасів_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЗапасів_Select ВидиЗапасів_Select = new Довідники.ВидиЗапасів_Select();
            ВидиЗапасів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ВидиЗапасів_Const.Код,
                /*Назва*/ Довідники.ВидиЗапасів_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ВидиЗапасів_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ВидиЗапасів_Select.QuerySelect.Order.Add(Довідники.ВидиЗапасів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЗапасів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВидиЗапасів_Select.MoveNext())
            {
                Довідники.ВидиЗапасів_Pointer? cur = ВидиЗапасів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВидиЗапасів_Записи Record = new ВидиЗапасів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[ВидиЗапасів_Const.Код].ToString() ?? "",
                        Назва = Fields[ВидиЗапасів_Const.Назва].ToString() ?? "",
                        
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
    public class ВидиЗапасів_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЗапасів_Select ВидиЗапасів_Select = new Довідники.ВидиЗапасів_Select();
            ВидиЗапасів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ВидиЗапасів_Const.Код,
                /*Назва*/ Довідники.ВидиЗапасів_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ВидиЗапасів_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ВидиЗапасів_Select.QuerySelect.Order.Add(Довідники.ВидиЗапасів_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ВидиЗапасів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВидиЗапасів_Select.MoveNext())
            {
                Довідники.ВидиЗапасів_Pointer? cur = ВидиЗапасів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВидиЗапасів_ЗаписиШвидкийВибір Record = new ВидиЗапасів_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[ВидиЗапасів_Const.Код].ToString() ?? "",
                        Назва = Fields[ВидиЗапасів_Const.Назва].ToString() ?? "",
                        
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
    public class Банки_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*ПовнаНазва*/ ПовнаНазва,
                /*КодМФО*/ КодМФО,
                /*КодЄДРПОУ*/ КодЄДРПОУ,
                /*НомерЛіцензії*/ НомерЛіцензії,
                /*ДатаЛіцензії*/ ДатаЛіцензії,
                /*Статус*/ Статус,
                /*ТипНаселеногоПункту*/ ТипНаселеногоПункту,
                /*УнікальнийКодБанку*/ УнікальнийКодБанку,
                /*ПоштовийІндекс*/ ПоштовийІндекс,
                /*НазваНаселеногоПункту*/ НазваНаселеногоПункту,
                /*Адреса*/ Адреса,
                /*НомерТелефону*/ НомерТелефону,
                /*ДатаВідкриттяУстанови*/ ДатаВідкриттяУстанови,
                /*ДатаЗакриттяУстанови*/ ДатаЗакриттяУстанови,
                /*КодНБУ*/ КодНБУ,
                /*КодСтатусу*/ КодСтатусу,
                /*ДатаЗапису*/ ДатаЗапису,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*ПовнаНазва*/ typeof(string),  
                /*КодМФО*/ typeof(string),  
                /*КодЄДРПОУ*/ typeof(string),  
                /*НомерЛіцензії*/ typeof(string),  
                /*ДатаЛіцензії*/ typeof(string),  
                /*Статус*/ typeof(string),  
                /*ТипНаселеногоПункту*/ typeof(string),  
                /*УнікальнийКодБанку*/ typeof(string),  
                /*ПоштовийІндекс*/ typeof(string),  
                /*НазваНаселеногоПункту*/ typeof(string),  
                /*Адреса*/ typeof(string),  
                /*НомерТелефону*/ typeof(string),  
                /*ДатаВідкриттяУстанови*/ typeof(string),  
                /*ДатаЗакриттяУстанови*/ typeof(string),  
                /*КодНБУ*/ typeof(string),  
                /*КодСтатусу*/ typeof(string),  
                /*ДатаЗапису*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
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
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Банки_Select Банки_Select = new Довідники.Банки_Select();
            Банки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Банки_Const.Код,
                /*Назва*/ Довідники.Банки_Const.Назва,
                /*ПовнаНазва*/ Довідники.Банки_Const.ПовнаНазва,
                /*КодМФО*/ Довідники.Банки_Const.КодМФО,
                /*КодЄДРПОУ*/ Довідники.Банки_Const.КодЄДРПОУ,
                /*НомерЛіцензії*/ Довідники.Банки_Const.НомерЛіцензії,
                /*ДатаЛіцензії*/ Довідники.Банки_Const.ДатаЛіцензії,
                /*Статус*/ Довідники.Банки_Const.Статус,
                /*ТипНаселеногоПункту*/ Довідники.Банки_Const.ТипНаселеногоПункту,
                /*УнікальнийКодБанку*/ Довідники.Банки_Const.УнікальнийКодБанку,
                /*ПоштовийІндекс*/ Довідники.Банки_Const.ПоштовийІндекс,
                /*НазваНаселеногоПункту*/ Довідники.Банки_Const.НазваНаселеногоПункту,
                /*Адреса*/ Довідники.Банки_Const.Адреса,
                /*НомерТелефону*/ Довідники.Банки_Const.НомерТелефону,
                /*ДатаВідкриттяУстанови*/ Довідники.Банки_Const.ДатаВідкриттяУстанови,
                /*ДатаЗакриттяУстанови*/ Довідники.Банки_Const.ДатаЗакриттяУстанови,
                /*КодНБУ*/ Довідники.Банки_Const.КодНБУ,
                /*КодСтатусу*/ Довідники.Банки_Const.КодСтатусу,
                /*ДатаЗапису*/ Довідники.Банки_Const.ДатаЗапису,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Банки_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Банки_Select.QuerySelect.Order.Add(Довідники.Банки_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Банки_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Банки_Select.MoveNext())
            {
                Довідники.Банки_Pointer? cur = Банки_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Банки_Записи Record = new Банки_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Банки_Const.Код].ToString() ?? "",
                        Назва = Fields[Банки_Const.Назва].ToString() ?? "",
                        ПовнаНазва = Fields[Банки_Const.ПовнаНазва].ToString() ?? "",
                        КодМФО = Fields[Банки_Const.КодМФО].ToString() ?? "",
                        КодЄДРПОУ = Fields[Банки_Const.КодЄДРПОУ].ToString() ?? "",
                        НомерЛіцензії = Fields[Банки_Const.НомерЛіцензії].ToString() ?? "",
                        ДатаЛіцензії = Fields[Банки_Const.ДатаЛіцензії].ToString() ?? "",
                        Статус = Fields[Банки_Const.Статус].ToString() ?? "",
                        ТипНаселеногоПункту = Fields[Банки_Const.ТипНаселеногоПункту].ToString() ?? "",
                        УнікальнийКодБанку = Fields[Банки_Const.УнікальнийКодБанку].ToString() ?? "",
                        ПоштовийІндекс = Fields[Банки_Const.ПоштовийІндекс].ToString() ?? "",
                        НазваНаселеногоПункту = Fields[Банки_Const.НазваНаселеногоПункту].ToString() ?? "",
                        Адреса = Fields[Банки_Const.Адреса].ToString() ?? "",
                        НомерТелефону = Fields[Банки_Const.НомерТелефону].ToString() ?? "",
                        ДатаВідкриттяУстанови = Fields[Банки_Const.ДатаВідкриттяУстанови].ToString() ?? "",
                        ДатаЗакриттяУстанови = Fields[Банки_Const.ДатаЗакриттяУстанови].ToString() ?? "",
                        КодНБУ = Fields[Банки_Const.КодНБУ].ToString() ?? "",
                        КодСтатусу = Fields[Банки_Const.КодСтатусу].ToString() ?? "",
                        ДатаЗапису = Fields[Банки_Const.ДатаЗапису].ToString() ?? "",
                        
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
    public class Банки_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Банки_Select Банки_Select = new Довідники.Банки_Select();
            Банки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Банки_Const.Код,
                /*Назва*/ Довідники.Банки_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Банки_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Банки_Select.QuerySelect.Order.Add(Довідники.Банки_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Банки_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Банки_Select.MoveNext())
            {
                Довідники.Банки_Pointer? cur = Банки_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Банки_ЗаписиШвидкийВибір Record = new Банки_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Банки_Const.Код].ToString() ?? "",
                        Назва = Fields[Банки_Const.Назва].ToString() ?? "",
                        
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
    public class СкладськіПриміщення_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Склад = "";
        string НалаштуванняАдресногоЗберігання = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Склад*/ Склад,
                /*НалаштуванняАдресногоЗберігання*/ НалаштуванняАдресногоЗберігання,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*НалаштуванняАдресногоЗберігання*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Налаштування", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*НалаштуванняАдресногоЗберігання*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіПриміщення_Select СкладськіПриміщення_Select = new Довідники.СкладськіПриміщення_Select();
            СкладськіПриміщення_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.СкладськіПриміщення_Const.Назва,
                /*НалаштуванняАдресногоЗберігання*/ Довідники.СкладськіПриміщення_Const.НалаштуванняАдресногоЗберігання,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СкладськіПриміщення_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СкладськіПриміщення_Select.MoveNext())
            {
                Довідники.СкладськіПриміщення_Pointer? cur = СкладськіПриміщення_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СкладськіПриміщення_Записи Record = new СкладськіПриміщення_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Склад = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Назва = Fields[СкладськіПриміщення_Const.Назва].ToString() ?? "",
                        НалаштуванняАдресногоЗберігання = Перелічення.ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_Alias( ((Перелічення.НалаштуванняАдресногоЗберігання)(Fields[СкладськіПриміщення_Const.НалаштуванняАдресногоЗберігання] != DBNull.Value ? Fields[СкладськіПриміщення_Const.НалаштуванняАдресногоЗберігання] : 0)) ),
                        
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
    public class СкладськіПриміщення_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Склад = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Склад*/ Склад,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Склад*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Склад*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіПриміщення_Select СкладськіПриміщення_Select = new Довідники.СкладськіПриміщення_Select();
            СкладськіПриміщення_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.СкладськіПриміщення_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СкладськіПриміщення_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СкладськіПриміщення_Select.MoveNext())
            {
                Довідники.СкладськіПриміщення_Pointer? cur = СкладськіПриміщення_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СкладськіПриміщення_ЗаписиШвидкийВибір Record = new СкладськіПриміщення_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Склад = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Назва = Fields[СкладськіПриміщення_Const.Назва].ToString() ?? "",
                        
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
    public class СкладськіКомірки_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Приміщення*/ Приміщення,
                /*Лінія*/ Лінія,
                /*Позиція*/ Позиція,
                /*Стелаж*/ Стелаж,
                /*Ярус*/ Ярус,
                /*ТипСкладськоїКомірки*/ ТипСкладськоїКомірки,
                /*Типорозмір*/ Типорозмір,
                /*Папка*/ Папка,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Приміщення*/ typeof(string),  
                /*Лінія*/ typeof(string),  
                /*Позиція*/ typeof(string),  
                /*Стелаж*/ typeof(string),  
                /*Ярус*/ typeof(string),  
                /*ТипСкладськоїКомірки*/ typeof(string),  
                /*Типорозмір*/ typeof(string),  
                /*Папка*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Приміщення", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Приміщення*/
            treeView.AppendColumn(new TreeViewColumn("Лінія", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Лінія*/
            treeView.AppendColumn(new TreeViewColumn("Позиція", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Позиція*/
            treeView.AppendColumn(new TreeViewColumn("Стелаж", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Стелаж*/
            treeView.AppendColumn(new TreeViewColumn("Ярус", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Ярус*/
            treeView.AppendColumn(new TreeViewColumn("Тип комірки", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*ТипСкладськоїКомірки*/
            treeView.AppendColumn(new TreeViewColumn("Типорозмір", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*Типорозмір*/
            treeView.AppendColumn(new TreeViewColumn("Папка", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true, SortColumnId = 10 } ); /*Папка*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіКомірки_Select СкладськіКомірки_Select = new Довідники.СкладськіКомірки_Select();
            СкладськіКомірки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.СкладськіКомірки_Const.Назва,
                /*Лінія*/ Довідники.СкладськіКомірки_Const.Лінія,
                /*Позиція*/ Довідники.СкладськіКомірки_Const.Позиція,
                /*Стелаж*/ Довідники.СкладськіКомірки_Const.Стелаж,
                /*Ярус*/ Довідники.СкладськіКомірки_Const.Ярус,
                /*ТипСкладськоїКомірки*/ Довідники.СкладськіКомірки_Const.ТипСкладськоїКомірки,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СкладськіКомірки_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СкладськіКомірки_Select.MoveNext())
            {
                Довідники.СкладськіКомірки_Pointer? cur = СкладськіКомірки_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СкладськіКомірки_Записи Record = new СкладськіКомірки_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Приміщення = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Типорозмір = Fields["join_tab_2_field_1"].ToString() ?? "",
                        Папка = Fields["join_tab_3_field_1"].ToString() ?? "",
                        Назва = Fields[СкладськіКомірки_Const.Назва].ToString() ?? "",
                        Лінія = Fields[СкладськіКомірки_Const.Лінія].ToString() ?? "",
                        Позиція = Fields[СкладськіКомірки_Const.Позиція].ToString() ?? "",
                        Стелаж = Fields[СкладськіКомірки_Const.Стелаж].ToString() ?? "",
                        Ярус = Fields[СкладськіКомірки_Const.Ярус].ToString() ?? "",
                        ТипСкладськоїКомірки = Перелічення.ПсевдонімиПерелічення.ТипиСкладськихКомірок_Alias( ((Перелічення.ТипиСкладськихКомірок)(Fields[СкладськіКомірки_Const.ТипСкладськоїКомірки] != DBNull.Value ? Fields[СкладськіКомірки_Const.ТипСкладськоїКомірки] : 0)) ),
                        
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
    public class СкладськіКомірки_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Приміщення = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Приміщення*/ Приміщення,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Приміщення*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Приміщення", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Приміщення*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіКомірки_Select СкладськіКомірки_Select = new Довідники.СкладськіКомірки_Select();
            СкладськіКомірки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.СкладськіКомірки_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) СкладськіКомірки_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (СкладськіКомірки_Select.MoveNext())
            {
                Довідники.СкладськіКомірки_Pointer? cur = СкладськіКомірки_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СкладськіКомірки_ЗаписиШвидкийВибір Record = new СкладськіКомірки_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Приміщення = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Назва = Fields[СкладськіКомірки_Const.Назва].ToString() ?? "",
                        
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
    public class ОбластьЗберігання_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        string Приміщення = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Приміщення*/ Приміщення,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Приміщення*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Приміщення", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Приміщення*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ОбластьЗберігання_Select ОбластьЗберігання_Select = new Довідники.ОбластьЗберігання_Select();
            ОбластьЗберігання_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ОбластьЗберігання_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ОбластьЗберігання_Select.QuerySelect.Where = (List<Where>)where;
            }

            
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

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ОбластьЗберігання_Select.MoveNext())
            {
                Довідники.ОбластьЗберігання_Pointer? cur = ОбластьЗберігання_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ОбластьЗберігання_Записи Record = new ОбластьЗберігання_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Приміщення = Fields["join_tab_1_field_1"].ToString() ?? "",
                        Назва = Fields[ОбластьЗберігання_Const.Назва].ToString() ?? "",
                        
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
    public class ТипорозміриКомірок_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                /*Висота*/ Висота,
                /*Глибина*/ Глибина,
                /*Вантажопідйомність*/ Вантажопідйомність,
                /*Обєм*/ Обєм,
                /*Ширина*/ Ширина,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Висота*/ typeof(string),  
                /*Глибина*/ typeof(string),  
                /*Вантажопідйомність*/ typeof(string),  
                /*Обєм*/ typeof(string),  
                /*Ширина*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Висота", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Висота*/
            treeView.AppendColumn(new TreeViewColumn("Глибина", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Глибина*/
            treeView.AppendColumn(new TreeViewColumn("Вантажопідйомність", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Вантажопідйомність*/
            treeView.AppendColumn(new TreeViewColumn("Обєм", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Обєм*/
            treeView.AppendColumn(new TreeViewColumn("Ширина", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Ширина*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ТипорозміриКомірок_Select ТипорозміриКомірок_Select = new Довідники.ТипорозміриКомірок_Select();
            ТипорозміриКомірок_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ТипорозміриКомірок_Const.Назва,
                /*Висота*/ Довідники.ТипорозміриКомірок_Const.Висота,
                /*Глибина*/ Довідники.ТипорозміриКомірок_Const.Глибина,
                /*Вантажопідйомність*/ Довідники.ТипорозміриКомірок_Const.Вантажопідйомність,
                /*Обєм*/ Довідники.ТипорозміриКомірок_Const.Обєм,
                /*Ширина*/ Довідники.ТипорозміриКомірок_Const.Ширина,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ТипорозміриКомірок_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ТипорозміриКомірок_Select.QuerySelect.Order.Add(Довідники.ТипорозміриКомірок_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ТипорозміриКомірок_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ТипорозміриКомірок_Select.MoveNext())
            {
                Довідники.ТипорозміриКомірок_Pointer? cur = ТипорозміриКомірок_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ТипорозміриКомірок_Записи Record = new ТипорозміриКомірок_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[ТипорозміриКомірок_Const.Назва].ToString() ?? "",
                        Висота = Fields[ТипорозміриКомірок_Const.Висота].ToString() ?? "",
                        Глибина = Fields[ТипорозміриКомірок_Const.Глибина].ToString() ?? "",
                        Вантажопідйомність = Fields[ТипорозміриКомірок_Const.Вантажопідйомність].ToString() ?? "",
                        Обєм = Fields[ТипорозміриКомірок_Const.Обєм].ToString() ?? "",
                        Ширина = Fields[ТипорозміриКомірок_Const.Ширина].ToString() ?? "",
                        
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
    public class ТипорозміриКомірок_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Назва*/ Назва,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ТипорозміриКомірок_Select ТипорозміриКомірок_Select = new Довідники.ТипорозміриКомірок_Select();
            ТипорозміриКомірок_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ТипорозміриКомірок_Const.Назва,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ТипорозміриКомірок_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              ТипорозміриКомірок_Select.QuerySelect.Order.Add(Довідники.ТипорозміриКомірок_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await ТипорозміриКомірок_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ТипорозміриКомірок_Select.MoveNext())
            {
                Довідники.ТипорозміриКомірок_Pointer? cur = ТипорозміриКомірок_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ТипорозміриКомірок_ЗаписиШвидкийВибір Record = new ТипорозміриКомірок_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[ТипорозміриКомірок_Const.Назва].ToString() ?? "",
                        
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
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

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
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer , UnigueID? owner)
        {
            RootPath = SelectPath = null;

            СкладськіКомірки_Папки_Записи rootRecord = new СкладськіКомірки_Папки_Записи
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

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
    {СкладськіКомірки_Папки_Const.Назва} AS Назва, 
    {СкладськіКомірки_Папки_Const.Родич} AS Родич, 
    level,
    deletion_label
FROM r
ORDER BY level, {СкладськіКомірки_Папки_Const.Назва} ASC
";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

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
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

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
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer , UnigueID? owner)
        {
            RootPath = SelectPath = null;

            СкладськіКомірки_Папки_ЗаписиШвидкийВибір rootRecord = new СкладськіКомірки_Папки_ЗаписиШвидкийВибір
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

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
    {СкладськіКомірки_Папки_Const.Назва} AS Назва, 
    {СкладськіКомірки_Папки_Const.Родич} AS Родич, 
    level,
    deletion_label
FROM r
ORDER BY level, {СкладськіКомірки_Папки_Const.Назва} ASC
";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary<string, TreeIter> nodeDictionary = new Dictionary<string, TreeIter>();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

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
    public class Блокнот_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ДатаЗапису = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*ДатаЗапису*/ ДатаЗапису,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*ДатаЗапису*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ДатаЗапису*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Блокнот_Select Блокнот_Select = new Довідники.Блокнот_Select();
            Блокнот_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Блокнот_Const.Код,
                /*Назва*/ Довідники.Блокнот_Const.Назва,
                /*ДатаЗапису*/ Довідники.Блокнот_Const.ДатаЗапису,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Блокнот_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Блокнот_Select.QuerySelect.Order.Add(Довідники.Блокнот_Const.ДатаЗапису, SelectOrder.ASC);
            

            /* SELECT */
            await Блокнот_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Блокнот_Select.MoveNext())
            {
                Довідники.Блокнот_Pointer? cur = Блокнот_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Блокнот_Записи Record = new Блокнот_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Блокнот_Const.Код].ToString() ?? "",
                        Назва = Fields[Блокнот_Const.Назва].ToString() ?? "",
                        ДатаЗапису = Fields[Блокнот_Const.ДатаЗапису].ToString() ?? "",
                        
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
    public class Блокнот_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        string Назва = "";
        string ДатаЗапису = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*ДатаЗапису*/ ДатаЗапису,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*ДатаЗапису*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*ДатаЗапису*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Блокнот_Select Блокнот_Select = new Довідники.Блокнот_Select();
            Блокнот_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Блокнот_Const.Код,
                /*Назва*/ Довідники.Блокнот_Const.Назва,
                /*ДатаЗапису*/ Довідники.Блокнот_Const.ДатаЗапису,
                
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) Блокнот_Select.QuerySelect.Where = (List<Where>)where;
            }

            
              /* ORDER */
              Блокнот_Select.QuerySelect.Order.Add(Довідники.Блокнот_Const.ДатаЗапису, SelectOrder.ASC);
            

            /* SELECT */
            await Блокнот_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (Блокнот_Select.MoveNext())
            {
                Довідники.Блокнот_Pointer? cur = Блокнот_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Блокнот_ЗаписиШвидкийВибір Record = new Блокнот_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[Блокнот_Const.Код].ToString() ?? "",
                        Назва = Fields[Блокнот_Const.Назва].ToString() ?? "",
                        ДатаЗапису = Fields[Блокнот_Const.ДатаЗапису].ToString() ?? "",
                        
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
    
    #region DOCUMENT "ЗамовленняПостачальнику"
    
      
    public class ЗамовленняПостачальнику_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Склад*/ Склад,
                /*Валюта*/ Валюта,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗамовленняПостачальнику_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ЗамовленняПостачальнику_Select ЗамовленняПостачальнику_Select = new Документи.ЗамовленняПостачальнику_Select();
            ЗамовленняПостачальнику_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ЗамовленняПостачальнику_Const.Назва,
                /*НомерДок*/ Документи.ЗамовленняПостачальнику_Const.НомерДок,
                /*ДатаДок*/ Документи.ЗамовленняПостачальнику_Const.ДатаДок,
                /*СумаДокументу*/ Документи.ЗамовленняПостачальнику_Const.СумаДокументу,
                /*Коментар*/ Документи.ЗамовленняПостачальнику_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ЗамовленняПостачальнику_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ЗамовленняПостачальнику_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ЗамовленняПостачальнику_Select.MoveNext())
            {
                Документи.ЗамовленняПостачальнику_Pointer? cur = ЗамовленняПостачальнику_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ЗамовленняПостачальнику_Записи Record = new ЗамовленняПостачальнику_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ЗамовленняПостачальнику_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ЗамовленняПостачальнику_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ЗамовленняПостачальнику_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[ЗамовленняПостачальнику_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[ЗамовленняПостачальнику_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ПоступленняТоварівТаПослуг_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Контрагент*/ Контрагент,
                /*Валюта*/ Валюта,
                /*Каса*/ Каса,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Каса*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПоступленняТоварівТаПослуг_Select ПоступленняТоварівТаПослуг_Select = new Документи.ПоступленняТоварівТаПослуг_Select();
            ПоступленняТоварівТаПослуг_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ПоступленняТоварівТаПослуг_Const.Назва,
                /*НомерДок*/ Документи.ПоступленняТоварівТаПослуг_Const.НомерДок,
                /*ДатаДок*/ Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок,
                /*СумаДокументу*/ Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу,
                /*Коментар*/ Документи.ПоступленняТоварівТаПослуг_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПоступленняТоварівТаПослуг_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ПоступленняТоварівТаПослуг_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПоступленняТоварівТаПослуг_Select.MoveNext())
            {
                Документи.ПоступленняТоварівТаПослуг_Pointer? cur = ПоступленняТоварівТаПослуг_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПоступленняТоварівТаПослуг_Записи Record = new ПоступленняТоварівТаПослуг_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Каса = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_6_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ПоступленняТоварівТаПослуг_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ПоступленняТоварівТаПослуг_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ПоступленняТоварівТаПослуг_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[ПоступленняТоварівТаПослуг_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[ПоступленняТоварівТаПослуг_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ЗамовленняКлієнта_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Валюта*/ Валюта,
                /*Каса*/ Каса,
                /*Склад*/ Склад,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Каса*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗамовленняКлієнта_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ЗамовленняКлієнта_Select ЗамовленняКлієнта_Select = new Документи.ЗамовленняКлієнта_Select();
            ЗамовленняКлієнта_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ЗамовленняКлієнта_Const.Назва,
                /*НомерДок*/ Документи.ЗамовленняКлієнта_Const.НомерДок,
                /*ДатаДок*/ Документи.ЗамовленняКлієнта_Const.ДатаДок,
                /*СумаДокументу*/ Документи.ЗамовленняКлієнта_Const.СумаДокументу,
                /*Коментар*/ Документи.ЗамовленняКлієнта_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ЗамовленняКлієнта_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ЗамовленняКлієнта_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ЗамовленняКлієнта_Select.MoveNext())
            {
                Документи.ЗамовленняКлієнта_Pointer? cur = ЗамовленняКлієнта_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ЗамовленняКлієнта_Записи Record = new ЗамовленняКлієнта_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Каса = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_6_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ЗамовленняКлієнта_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ЗамовленняКлієнта_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ЗамовленняКлієнта_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[ЗамовленняКлієнта_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[ЗамовленняКлієнта_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class РеалізаціяТоварівТаПослуг_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Валюта*/ Валюта,
                /*Каса*/ Каса,
                /*Склад*/ Склад,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Каса*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РеалізаціяТоварівТаПослуг_Select РеалізаціяТоварівТаПослуг_Select = new Документи.РеалізаціяТоварівТаПослуг_Select();
            РеалізаціяТоварівТаПослуг_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.РеалізаціяТоварівТаПослуг_Const.Назва,
                /*НомерДок*/ Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок,
                /*ДатаДок*/ Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок,
                /*СумаДокументу*/ Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу,
                /*Коментар*/ Документи.РеалізаціяТоварівТаПослуг_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) РеалізаціяТоварівТаПослуг_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await РеалізаціяТоварівТаПослуг_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (РеалізаціяТоварівТаПослуг_Select.MoveNext())
            {
                Документи.РеалізаціяТоварівТаПослуг_Pointer? cur = РеалізаціяТоварівТаПослуг_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    РеалізаціяТоварівТаПослуг_Записи Record = new РеалізаціяТоварівТаПослуг_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Каса = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_6_field_1"].ToString() ?? "", /**/
                        Назва = Fields[РеалізаціяТоварівТаПослуг_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[РеалізаціяТоварівТаПослуг_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[РеалізаціяТоварівТаПослуг_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[РеалізаціяТоварівТаПослуг_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[РеалізаціяТоварівТаПослуг_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ВстановленняЦінНоменклатури_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Валюта*/ Валюта,
                /*ВидЦіни*/ ВидЦіни,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*ВидЦіни*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ВстановленняЦінНоменклатури_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ВстановленняЦінНоменклатури_Select ВстановленняЦінНоменклатури_Select = new Документи.ВстановленняЦінНоменклатури_Select();
            ВстановленняЦінНоменклатури_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ВстановленняЦінНоменклатури_Const.Назва,
                /*НомерДок*/ Документи.ВстановленняЦінНоменклатури_Const.НомерДок,
                /*ДатаДок*/ Документи.ВстановленняЦінНоменклатури_Const.ДатаДок,
                /*Коментар*/ Документи.ВстановленняЦінНоменклатури_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ВстановленняЦінНоменклатури_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ВстановленняЦінНоменклатури_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВстановленняЦінНоменклатури_Select.MoveNext())
            {
                Документи.ВстановленняЦінНоменклатури_Pointer? cur = ВстановленняЦінНоменклатури_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВстановленняЦінНоменклатури_Записи Record = new ВстановленняЦінНоменклатури_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        ВидЦіни = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ВстановленняЦінНоменклатури_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ВстановленняЦінНоменклатури_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ВстановленняЦінНоменклатури_Const.ДатаДок].ToString() ?? "", /**/
                        Коментар = Fields[ВстановленняЦінНоменклатури_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ПрихіднийКасовийОрдер_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Валюта*/ Валюта,
                /*Каса*/ Каса,
                /*Контрагент*/ Контрагент,
                /*СумаДокументу*/ СумаДокументу,
                /*ГосподарськаОперація*/ ГосподарськаОперація,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Каса*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*ГосподарськаОперація*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПрихіднийКасовийОрдер_Select ПрихіднийКасовийОрдер_Select = new Документи.ПрихіднийКасовийОрдер_Select();
            ПрихіднийКасовийОрдер_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ПрихіднийКасовийОрдер_Const.Назва,
                /*НомерДок*/ Документи.ПрихіднийКасовийОрдер_Const.НомерДок,
                /*ДатаДок*/ Документи.ПрихіднийКасовийОрдер_Const.ДатаДок,
                /*СумаДокументу*/ Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу,
                /*ГосподарськаОперація*/ Документи.ПрихіднийКасовийОрдер_Const.ГосподарськаОперація,
                /*Коментар*/ Документи.ПрихіднийКасовийОрдер_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПрихіднийКасовийОрдер_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ПрихіднийКасовийОрдер_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПрихіднийКасовийОрдер_Select.MoveNext())
            {
                Документи.ПрихіднийКасовийОрдер_Pointer? cur = ПрихіднийКасовийОрдер_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПрихіднийКасовийОрдер_Записи Record = new ПрихіднийКасовийОрдер_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Каса = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ПрихіднийКасовийОрдер_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ПрихіднийКасовийОрдер_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ПрихіднийКасовийОрдер_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[ПрихіднийКасовийОрдер_Const.СумаДокументу].ToString() ?? "", /**/
                        ГосподарськаОперація = Перелічення.ПсевдонімиПерелічення.ГосподарськіОперації_Alias( ((Перелічення.ГосподарськіОперації)(Fields[ПрихіднийКасовийОрдер_Const.ГосподарськаОперація] != DBNull.Value ? Fields[ПрихіднийКасовийОрдер_Const.ГосподарськаОперація] : 0)) ), /**/
                        Коментар = Fields[ПрихіднийКасовийОрдер_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class РозхіднийКасовийОрдер_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Валюта*/ Валюта,
                /*Каса*/ Каса,
                /*СумаДокументу*/ СумаДокументу,
                /*ГосподарськаОперація*/ ГосподарськаОперація,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Каса*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*ГосподарськаОперація*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РозхіднийКасовийОрдер_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РозхіднийКасовийОрдер_Select РозхіднийКасовийОрдер_Select = new Документи.РозхіднийКасовийОрдер_Select();
            РозхіднийКасовийОрдер_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.РозхіднийКасовийОрдер_Const.Назва,
                /*НомерДок*/ Документи.РозхіднийКасовийОрдер_Const.НомерДок,
                /*ДатаДок*/ Документи.РозхіднийКасовийОрдер_Const.ДатаДок,
                /*СумаДокументу*/ Документи.РозхіднийКасовийОрдер_Const.СумаДокументу,
                /*ГосподарськаОперація*/ Документи.РозхіднийКасовийОрдер_Const.ГосподарськаОперація,
                /*Коментар*/ Документи.РозхіднийКасовийОрдер_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) РозхіднийКасовийОрдер_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await РозхіднийКасовийОрдер_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (РозхіднийКасовийОрдер_Select.MoveNext())
            {
                Документи.РозхіднийКасовийОрдер_Pointer? cur = РозхіднийКасовийОрдер_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    РозхіднийКасовийОрдер_Записи Record = new РозхіднийКасовийОрдер_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Каса = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Назва = Fields[РозхіднийКасовийОрдер_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[РозхіднийКасовийОрдер_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[РозхіднийКасовийОрдер_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[РозхіднийКасовийОрдер_Const.СумаДокументу].ToString() ?? "", /**/
                        ГосподарськаОперація = Перелічення.ПсевдонімиПерелічення.ГосподарськіОперації_Alias( ((Перелічення.ГосподарськіОперації)(Fields[РозхіднийКасовийОрдер_Const.ГосподарськаОперація] != DBNull.Value ? Fields[РозхіднийКасовийОрдер_Const.ГосподарськаОперація] : 0)) ), /**/
                        Коментар = Fields[РозхіднийКасовийОрдер_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ПереміщенняТоварів_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*СкладВідправник*/ СкладВідправник,
                /*СкладОтримувач*/ СкладОтримувач,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*СкладВідправник*/ typeof(string),  
                /*СкладОтримувач*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПереміщенняТоварів_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПереміщенняТоварів_Select ПереміщенняТоварів_Select = new Документи.ПереміщенняТоварів_Select();
            ПереміщенняТоварів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ПереміщенняТоварів_Const.Назва,
                /*НомерДок*/ Документи.ПереміщенняТоварів_Const.НомерДок,
                /*ДатаДок*/ Документи.ПереміщенняТоварів_Const.ДатаДок,
                /*Коментар*/ Документи.ПереміщенняТоварів_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПереміщенняТоварів_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ПереміщенняТоварів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПереміщенняТоварів_Select.MoveNext())
            {
                Документи.ПереміщенняТоварів_Pointer? cur = ПереміщенняТоварів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПереміщенняТоварів_Записи Record = new ПереміщенняТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        СкладВідправник = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        СкладОтримувач = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ПереміщенняТоварів_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ПереміщенняТоварів_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ПереміщенняТоварів_Const.ДатаДок].ToString() ?? "", /**/
                        Коментар = Fields[ПереміщенняТоварів_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ПоверненняТоварівПостачальнику_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Валюта*/ Валюта,
                /*Каса*/ Каса,
                /*Склад*/ Склад,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Каса*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПоверненняТоварівПостачальнику_Select ПоверненняТоварівПостачальнику_Select = new Документи.ПоверненняТоварівПостачальнику_Select();
            ПоверненняТоварівПостачальнику_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ПоверненняТоварівПостачальнику_Const.Назва,
                /*НомерДок*/ Документи.ПоверненняТоварівПостачальнику_Const.НомерДок,
                /*ДатаДок*/ Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок,
                /*СумаДокументу*/ Документи.ПоверненняТоварівПостачальнику_Const.СумаДокументу,
                /*Коментар*/ Документи.ПоверненняТоварівПостачальнику_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПоверненняТоварівПостачальнику_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ПоверненняТоварівПостачальнику_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПоверненняТоварівПостачальнику_Select.MoveNext())
            {
                Документи.ПоверненняТоварівПостачальнику_Pointer? cur = ПоверненняТоварівПостачальнику_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПоверненняТоварівПостачальнику_Записи Record = new ПоверненняТоварівПостачальнику_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Каса = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_6_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ПоверненняТоварівПостачальнику_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ПоверненняТоварівПостачальнику_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ПоверненняТоварівПостачальнику_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[ПоверненняТоварівПостачальнику_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[ПоверненняТоварівПостачальнику_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ПоверненняТоварівВідКлієнта_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Валюта*/ Валюта,
                /*Каса*/ Каса,
                /*Контрагент*/ Контрагент,
                /*Склад*/ Склад,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Каса*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПоверненняТоварівВідКлієнта_Select ПоверненняТоварівВідКлієнта_Select = new Документи.ПоверненняТоварівВідКлієнта_Select();
            ПоверненняТоварівВідКлієнта_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ПоверненняТоварівВідКлієнта_Const.Назва,
                /*НомерДок*/ Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок,
                /*ДатаДок*/ Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок,
                /*СумаДокументу*/ Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу,
                /*Коментар*/ Документи.ПоверненняТоварівВідКлієнта_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПоверненняТоварівВідКлієнта_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ПоверненняТоварівВідКлієнта_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПоверненняТоварівВідКлієнта_Select.MoveNext())
            {
                Документи.ПоверненняТоварівВідКлієнта_Pointer? cur = ПоверненняТоварівВідКлієнта_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПоверненняТоварівВідКлієнта_Записи Record = new ПоверненняТоварівВідКлієнта_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Каса = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_6_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ПоверненняТоварівВідКлієнта_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ПоверненняТоварівВідКлієнта_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ПоверненняТоварівВідКлієнта_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[ПоверненняТоварівВідКлієнта_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[ПоверненняТоварівВідКлієнта_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class АктВиконанихРобіт_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Валюта*/ Валюта,
                /*Каса*/ Каса,
                /*Контрагент*/ Контрагент,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Каса*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.АктВиконанихРобіт_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.АктВиконанихРобіт_Select АктВиконанихРобіт_Select = new Документи.АктВиконанихРобіт_Select();
            АктВиконанихРобіт_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.АктВиконанихРобіт_Const.Назва,
                /*НомерДок*/ Документи.АктВиконанихРобіт_Const.НомерДок,
                /*ДатаДок*/ Документи.АктВиконанихРобіт_Const.ДатаДок,
                /*СумаДокументу*/ Документи.АктВиконанихРобіт_Const.СумаДокументу,
                /*Коментар*/ Документи.АктВиконанихРобіт_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) АктВиконанихРобіт_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await АктВиконанихРобіт_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (АктВиконанихРобіт_Select.MoveNext())
            {
                Документи.АктВиконанихРобіт_Pointer? cur = АктВиконанихРобіт_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    АктВиконанихРобіт_Записи Record = new АктВиконанихРобіт_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Каса = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Назва = Fields[АктВиконанихРобіт_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[АктВиконанихРобіт_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[АктВиконанихРобіт_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[АктВиконанихРобіт_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[АктВиконанихРобіт_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ВведенняЗалишків_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Контрагент*/ Контрагент,
                /*Валюта*/ Валюта,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ВведенняЗалишків_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ВведенняЗалишків_Select ВведенняЗалишків_Select = new Документи.ВведенняЗалишків_Select();
            ВведенняЗалишків_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ВведенняЗалишків_Const.Назва,
                /*НомерДок*/ Документи.ВведенняЗалишків_Const.НомерДок,
                /*ДатаДок*/ Документи.ВведенняЗалишків_Const.ДатаДок,
                /*Коментар*/ Документи.ВведенняЗалишків_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ВведенняЗалишків_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ВведенняЗалишків_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВведенняЗалишків_Select.MoveNext())
            {
                Документи.ВведенняЗалишків_Pointer? cur = ВведенняЗалишків_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВведенняЗалишків_Записи Record = new ВведенняЗалишків_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ВведенняЗалишків_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ВведенняЗалишків_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ВведенняЗалишків_Const.ДатаДок].ToString() ?? "", /**/
                        Коментар = Fields[ВведенняЗалишків_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class НадлишкиТоварів_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.НадлишкиТоварів_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.НадлишкиТоварів_Select НадлишкиТоварів_Select = new Документи.НадлишкиТоварів_Select();
            НадлишкиТоварів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.НадлишкиТоварів_Const.Назва,
                /*НомерДок*/ Документи.НадлишкиТоварів_Const.НомерДок,
                /*ДатаДок*/ Документи.НадлишкиТоварів_Const.ДатаДок,
                /*Коментар*/ Документи.НадлишкиТоварів_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) НадлишкиТоварів_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await НадлишкиТоварів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (НадлишкиТоварів_Select.MoveNext())
            {
                Документи.НадлишкиТоварів_Pointer? cur = НадлишкиТоварів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    НадлишкиТоварів_Записи Record = new НадлишкиТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Назва = Fields[НадлишкиТоварів_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[НадлишкиТоварів_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[НадлишкиТоварів_Const.ДатаДок].ToString() ?? "", /**/
                        Коментар = Fields[НадлишкиТоварів_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ПересортицяТоварів_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПересортицяТоварів_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПересортицяТоварів_Select ПересортицяТоварів_Select = new Документи.ПересортицяТоварів_Select();
            ПересортицяТоварів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ПересортицяТоварів_Const.Назва,
                /*НомерДок*/ Документи.ПересортицяТоварів_Const.НомерДок,
                /*ДатаДок*/ Документи.ПересортицяТоварів_Const.ДатаДок,
                /*Коментар*/ Документи.ПересортицяТоварів_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПересортицяТоварів_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ПересортицяТоварів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПересортицяТоварів_Select.MoveNext())
            {
                Документи.ПересортицяТоварів_Pointer? cur = ПересортицяТоварів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПересортицяТоварів_Записи Record = new ПересортицяТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ПересортицяТоварів_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ПересортицяТоварів_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ПересортицяТоварів_Const.ДатаДок].ToString() ?? "", /**/
                        Коментар = Fields[ПересортицяТоварів_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ПерерахунокТоварів_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Склад = "";
        string Відповідальний = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Відповідальний*/ Відповідальний,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*Відповідальний*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Номер", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("Дата", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Відповідальний", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Відповідальний*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПерерахунокТоварів_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПерерахунокТоварів_Select ПерерахунокТоварів_Select = new Документи.ПерерахунокТоварів_Select();
            ПерерахунокТоварів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ПерерахунокТоварів_Const.Назва,
                /*НомерДок*/ Документи.ПерерахунокТоварів_Const.НомерДок,
                /*ДатаДок*/ Документи.ПерерахунокТоварів_Const.ДатаДок,
                /*Коментар*/ Документи.ПерерахунокТоварів_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПерерахунокТоварів_Select.QuerySelect.Where = (List<Where>)where;

            
              /* ORDER */
              ПерерахунокТоварів_Select.QuerySelect.Order.Add(Документи.ПерерахунокТоварів_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                ПерерахунокТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.ПерерахунокТоварів_Const.Організація, ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ПерерахунокТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                ПерерахунокТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Склади_Const.TABLE, Документи.ПерерахунокТоварів_Const.Склад, ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  ПерерахунокТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "join_tab_2_field_1"));
                  
                /* Join Table */
                ПерерахунокТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.ФізичніОсоби_Const.TABLE, Документи.ПерерахунокТоварів_Const.Відповідальний, ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_3"));
                
                  /* Field */
                  ПерерахунокТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_3." + Довідники.ФізичніОсоби_Const.Назва, "join_tab_3_field_1"));
                  
                /* Join Table */
                ПерерахунокТоварів_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.ПерерахунокТоварів_Const.Автор, ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_4"));
                
                  /* Field */
                  ПерерахунокТоварів_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_4." + Довідники.Користувачі_Const.Назва, "join_tab_4_field_1"));
                  

            /* SELECT */
            await ПерерахунокТоварів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПерерахунокТоварів_Select.MoveNext())
            {
                Документи.ПерерахунокТоварів_Pointer? cur = ПерерахунокТоварів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПерерахунокТоварів_Записи Record = new ПерерахунокТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Відповідальний = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ПерерахунокТоварів_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ПерерахунокТоварів_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ПерерахунокТоварів_Const.ДатаДок].ToString() ?? "", /**/
                        Коментар = Fields[ПерерахунокТоварів_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ПсуванняТоварів_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПсуванняТоварів_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПсуванняТоварів_Select ПсуванняТоварів_Select = new Документи.ПсуванняТоварів_Select();
            ПсуванняТоварів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ПсуванняТоварів_Const.Назва,
                /*НомерДок*/ Документи.ПсуванняТоварів_Const.НомерДок,
                /*ДатаДок*/ Документи.ПсуванняТоварів_Const.ДатаДок,
                /*СумаДокументу*/ Документи.ПсуванняТоварів_Const.СумаДокументу,
                /*Коментар*/ Документи.ПсуванняТоварів_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПсуванняТоварів_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ПсуванняТоварів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПсуванняТоварів_Select.MoveNext())
            {
                Документи.ПсуванняТоварів_Pointer? cur = ПсуванняТоварів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПсуванняТоварів_Записи Record = new ПсуванняТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ПсуванняТоварів_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ПсуванняТоварів_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ПсуванняТоварів_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[ПсуванняТоварів_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[ПсуванняТоварів_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ВнутрішнєСпоживанняТоварів_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Валюта*/ Валюта,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ВнутрішнєСпоживанняТоварів_Select ВнутрішнєСпоживанняТоварів_Select = new Документи.ВнутрішнєСпоживанняТоварів_Select();
            ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ВнутрішнєСпоживанняТоварів_Const.Назва,
                /*НомерДок*/ Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок,
                /*ДатаДок*/ Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок,
                /*СумаДокументу*/ Документи.ВнутрішнєСпоживанняТоварів_Const.СумаДокументу,
                /*Коментар*/ Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ВнутрішнєСпоживанняТоварів_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ВнутрішнєСпоживанняТоварів_Select.MoveNext())
            {
                Документи.ВнутрішнєСпоживанняТоварів_Pointer? cur = ВнутрішнєСпоживанняТоварів_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ВнутрішнєСпоживанняТоварів_Записи Record = new ВнутрішнєСпоживанняТоварів_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ВнутрішнєСпоживанняТоварів_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[ВнутрішнєСпоживанняТоварів_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[ВнутрішнєСпоживанняТоварів_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[ВнутрішнєСпоживанняТоварів_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[ВнутрішнєСпоживанняТоварів_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class РахунокФактура_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Валюта*/ Валюта,
                /*Каса*/ Каса,
                /*Склад*/ Склад,
                /*СумаДокументу*/ СумаДокументу,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Контрагент*/ typeof(string),  
                /*Валюта*/ typeof(string),  
                /*Каса*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*СумаДокументу*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РахунокФактура_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РахунокФактура_Select РахунокФактура_Select = new Документи.РахунокФактура_Select();
            РахунокФактура_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.РахунокФактура_Const.Назва,
                /*НомерДок*/ Документи.РахунокФактура_Const.НомерДок,
                /*ДатаДок*/ Документи.РахунокФактура_Const.ДатаДок,
                /*СумаДокументу*/ Документи.РахунокФактура_Const.СумаДокументу,
                /*Коментар*/ Документи.РахунокФактура_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) РахунокФактура_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await РахунокФактура_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (РахунокФактура_Select.MoveNext())
            {
                Документи.РахунокФактура_Pointer? cur = РахунокФактура_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    РахунокФактура_Записи Record = new РахунокФактура_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Контрагент = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Валюта = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Каса = Fields["join_tab_4_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_5_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_6_field_1"].ToString() ?? "", /**/
                        Назва = Fields[РахунокФактура_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[РахунокФактура_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[РахунокФактура_Const.ДатаДок].ToString() ?? "", /**/
                        СумаДокументу = Fields[РахунокФактура_Const.СумаДокументу].ToString() ?? "", /**/
                        Коментар = Fields[РахунокФактура_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class РозміщенняТоварівНаСкладі_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*ДатаДок*/ ДатаДок,
                /*НомерДок*/ НомерДок,
                /*Склад*/ Склад,
                /*ДокументПоступлення*/ ДокументПоступлення,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*ДокументПоступлення*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РозміщенняТоварівНаСкладі_Select РозміщенняТоварівНаСкладі_Select = new Документи.РозміщенняТоварівНаСкладі_Select();
            РозміщенняТоварівНаСкладі_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.РозміщенняТоварівНаСкладі_Const.Назва,
                /*ДатаДок*/ Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок,
                /*НомерДок*/ Документи.РозміщенняТоварівНаСкладі_Const.НомерДок,
                /*Коментар*/ Документи.РозміщенняТоварівНаСкладі_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) РозміщенняТоварівНаСкладі_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await РозміщенняТоварівНаСкладі_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (РозміщенняТоварівНаСкладі_Select.MoveNext())
            {
                Документи.РозміщенняТоварівНаСкладі_Pointer? cur = РозміщенняТоварівНаСкладі_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    РозміщенняТоварівНаСкладі_Записи Record = new РозміщенняТоварівНаСкладі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Склад = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        ДокументПоступлення = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Назва = Fields[РозміщенняТоварівНаСкладі_Const.Назва].ToString() ?? "", /**/
                        ДатаДок = Fields[РозміщенняТоварівНаСкладі_Const.ДатаДок].ToString() ?? "", /**/
                        НомерДок = Fields[РозміщенняТоварівНаСкладі_Const.НомерДок].ToString() ?? "", /**/
                        Коментар = Fields[РозміщенняТоварівНаСкладі_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ПереміщенняТоварівНаСкладі_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*ДатаДок*/ ДатаДок,
                /*НомерДок*/ НомерДок,
                /*Склад*/ Склад,
                /*Організація*/ Організація,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ПереміщенняТоварівНаСкладі_Select ПереміщенняТоварівНаСкладі_Select = new Документи.ПереміщенняТоварівНаСкладі_Select();
            ПереміщенняТоварівНаСкладі_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ПереміщенняТоварівНаСкладі_Const.Назва,
                /*ДатаДок*/ Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок,
                /*НомерДок*/ Документи.ПереміщенняТоварівНаСкладі_Const.НомерДок,
                /*Коментар*/ Документи.ПереміщенняТоварівНаСкладі_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПереміщенняТоварівНаСкладі_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ПереміщенняТоварівНаСкладі_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ПереміщенняТоварівНаСкладі_Select.MoveNext())
            {
                Документи.ПереміщенняТоварівНаСкладі_Pointer? cur = ПереміщенняТоварівНаСкладі_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ПереміщенняТоварівНаСкладі_Записи Record = new ПереміщенняТоварівНаСкладі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Склад = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Організація = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ПереміщенняТоварівНаСкладі_Const.Назва].ToString() ?? "", /**/
                        ДатаДок = Fields[ПереміщенняТоварівНаСкладі_Const.ДатаДок].ToString() ?? "", /**/
                        НомерДок = Fields[ПереміщенняТоварівНаСкладі_Const.НомерДок].ToString() ?? "", /**/
                        Коментар = Fields[ПереміщенняТоварівНаСкладі_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class ЗбіркаТоварівНаСкладі_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*ДатаДок*/ ДатаДок,
                /*НомерДок*/ НомерДок,
                /*Склад*/ Склад,
                /*ДокументРеалізації*/ ДокументРеалізації,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*ДокументРеалізації*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ЗбіркаТоварівНаСкладі_Select ЗбіркаТоварівНаСкладі_Select = new Документи.ЗбіркаТоварівНаСкладі_Select();
            ЗбіркаТоварівНаСкладі_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ЗбіркаТоварівНаСкладі_Const.Назва,
                /*ДатаДок*/ Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок,
                /*НомерДок*/ Документи.ЗбіркаТоварівНаСкладі_Const.НомерДок,
                /*Коментар*/ Документи.ЗбіркаТоварівНаСкладі_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ЗбіркаТоварівНаСкладі_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await ЗбіркаТоварівНаСкладі_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ЗбіркаТоварівНаСкладі_Select.MoveNext())
            {
                Документи.ЗбіркаТоварівНаСкладі_Pointer? cur = ЗбіркаТоварівНаСкладі_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ЗбіркаТоварівНаСкладі_Записи Record = new ЗбіркаТоварівНаСкладі_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Склад = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        ДокументРеалізації = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Назва = Fields[ЗбіркаТоварівНаСкладі_Const.Назва].ToString() ?? "", /**/
                        ДатаДок = Fields[ЗбіркаТоварівНаСкладі_Const.ДатаДок].ToString() ?? "", /**/
                        НомерДок = Fields[ЗбіркаТоварівНаСкладі_Const.НомерДок].ToString() ?? "", /**/
                        Коментар = Fields[ЗбіркаТоварівНаСкладі_Const.Коментар].ToString() ?? "" /**/
                        
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
    
      
    public class РозміщенняНоменклатуриПоКоміркам_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*ДатаДок*/ ДатаДок,
                /*НомерДок*/ НомерДок,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Склад*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.РозміщенняНоменклатуриПоКоміркам_Select РозміщенняНоменклатуриПоКоміркам_Select = new Документи.РозміщенняНоменклатуриПоКоміркам_Select();
            РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.РозміщенняНоменклатуриПоКоміркам_Const.Назва,
                /*ДатаДок*/ Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок,
                /*НомерДок*/ Документи.РозміщенняНоменклатуриПоКоміркам_Const.НомерДок,
                /*Коментар*/ Документи.РозміщенняНоменклатуриПоКоміркам_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Where = (List<Where>)where;

            
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
            await РозміщенняНоменклатуриПоКоміркам_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (РозміщенняНоменклатуриПоКоміркам_Select.MoveNext())
            {
                Документи.РозміщенняНоменклатуриПоКоміркам_Pointer? cur = РозміщенняНоменклатуриПоКоміркам_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    РозміщенняНоменклатуриПоКоміркам_Записи Record = new РозміщенняНоменклатуриПоКоміркам_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Склад = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_3_field_1"].ToString() ?? "", /**/
                        Назва = Fields[РозміщенняНоменклатуриПоКоміркам_Const.Назва].ToString() ?? "", /**/
                        ДатаДок = Fields[РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок].ToString() ?? "", /**/
                        НомерДок = Fields[РозміщенняНоменклатуриПоКоміркам_Const.НомерДок].ToString() ?? "", /**/
                        Коментар = Fields[РозміщенняНоменклатуриПоКоміркам_Const.Коментар].ToString() ?? "" /**/
                        
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
    
    #region DOCUMENT "КорегуванняБоргу"
    
      
    public class КорегуванняБоргу_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        
        string Назва = "";
        string НомерДок = "";
        string ДатаДок = "";
        string Організація = "";
        string Автор = "";
        string Коментар = "";

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                /*Назва*/ Назва,
                /*НомерДок*/ НомерДок,
                /*ДатаДок*/ ДатаДок,
                /*Організація*/ Організація,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                /*Назва*/ typeof(string),  
                /*НомерДок*/ typeof(string),  
                /*ДатаДок*/ typeof(string),  
                /*Організація*/ typeof(string),  
                /*Автор*/ typeof(string),  
                /*Коментар*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("НомерДок", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true } ); /*НомерДок*/
            treeView.AppendColumn(new TreeViewColumn("ДатаДок", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true } ); /*ДатаДок*/
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.КорегуванняБоргу_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.КорегуванняБоргу_Select КорегуванняБоргу_Select = new Документи.КорегуванняБоргу_Select();
            КорегуванняБоргу_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.КорегуванняБоргу_Const.Назва,
                /*НомерДок*/ Документи.КорегуванняБоргу_Const.НомерДок,
                /*ДатаДок*/ Документи.КорегуванняБоргу_Const.ДатаДок,
                /*Коментар*/ Документи.КорегуванняБоргу_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) КорегуванняБоргу_Select.QuerySelect.Where = (List<Where>)where;

            
              /* ORDER */
              КорегуванняБоргу_Select.QuerySelect.Order.Add(Документи.КорегуванняБоргу_Const.ДатаДок, SelectOrder.ASC);
            
                /* Join Table */
                КорегуванняБоргу_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Організації_Const.TABLE, Документи.КорегуванняБоргу_Const.Організація, КорегуванняБоргу_Select.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  КорегуванняБоргу_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "join_tab_1_field_1"));
                  
                /* Join Table */
                КорегуванняБоргу_Select.QuerySelect.Joins.Add(
                    new Join(Довідники.Користувачі_Const.TABLE, Документи.КорегуванняБоргу_Const.Автор, КорегуванняБоргу_Select.QuerySelect.Table, "join_tab_2"));
                
                  /* Field */
                  КорегуванняБоргу_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_2." + Довідники.Користувачі_Const.Назва, "join_tab_2_field_1"));
                  

            /* SELECT */
            await КорегуванняБоргу_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (КорегуванняБоргу_Select.MoveNext())
            {
                Документи.КорегуванняБоргу_Pointer? cur = КорегуванняБоргу_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    КорегуванняБоргу_Записи Record = new КорегуванняБоргу_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Організація = Fields["join_tab_1_field_1"].ToString() ?? "", /**/
                        Автор = Fields["join_tab_2_field_1"].ToString() ?? "", /**/
                        Назва = Fields[КорегуванняБоргу_Const.Назва].ToString() ?? "", /**/
                        НомерДок = Fields[КорегуванняБоргу_Const.НомерДок].ToString() ?? "", /**/
                        ДатаДок = Fields[КорегуванняБоргу_Const.ДатаДок].ToString() ?? "", /**/
                        Коментар = Fields[КорегуванняБоргу_Const.Коментар].ToString() ?? "" /**/
                        
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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Type, 
                /*Проведений документ*/ Spend,
                /*Назва*/ Назва, /*Дата*/ Дата, /*Номер*/ Номер, /*Організація*/ Організація, /*Контрагент*/ Контрагент, /*Склад*/ Склад, /*Каса*/ Каса, /*Валюта*/ Валюта, /*Сума*/ Сума, /*Автор*/ Автор, /*Коментар*/ Коментар,  
            };
        }

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                typeof(Gdk.Pixbuf), /* Image */
                typeof(string), /* ID */
                typeof(string), /* Type */
                typeof(bool), /* Spend Проведений документ */
                typeof(string), /*Назва*/
                typeof(string), /*Дата*/
                typeof(string), /*Номер*/
                typeof(string), /*Організація*/
                typeof(string), /*Контрагент*/
                typeof(string), /*Склад*/
                typeof(string), /*Каса*/
                typeof(string), /*Валюта*/
                typeof(string), /*Сума*/
                typeof(string), /*Автор*/
                typeof(string), /*Коментар*/
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            Dictionary<string, List<Where>> WhereDict = [];
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", WhereDict);
            else
                treeView.Data["Where"] = WhereDict;
            
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗамовленняПостачальнику_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ЗамовленняПостачальнику", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПоступленняТоварівТаПослуг", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗамовленняКлієнта_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ЗамовленняКлієнта", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РеалізаціяТоварівТаПослуг", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ВстановленняЦінНоменклатури_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ВстановленняЦінНоменклатури", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПрихіднийКасовийОрдер", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РозхіднийКасовийОрдер_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РозхіднийКасовийОрдер", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПереміщенняТоварів_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПереміщенняТоварів", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПоверненняТоварівПостачальнику", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПоверненняТоварівВідКлієнта", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.АктВиконанихРобіт_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("АктВиконанихРобіт", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ВведенняЗалишків_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ВведенняЗалишків", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПерерахунокТоварів_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПерерахунокТоварів", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПсуванняТоварів_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПсуванняТоварів", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ВнутрішнєСпоживанняТоварів", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РахунокФактура_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РахунокФактура", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РозміщенняТоварівНаСкладі", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПереміщенняТоварівНаСкладі", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ЗбіркаТоварівНаСкладі", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РозміщенняНоменклатуриПоКоміркам", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.КорегуванняБоргу_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("КорегуванняБоргу", [where]);
            }
              
        }

        public static void ОчиститиВідбір(TreeView treeView)
        {
            if (treeView.Data.ContainsKey("Where"))
                treeView.Data["Where"] = null;
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            return new Dictionary<string, string>()
            {
                {"ЗамовленняПостачальнику", "Замовлення постачальнику"},
                {"ПоступленняТоварівТаПослуг", "Поступлення товарів та послуг"},
                {"ЗамовленняКлієнта", "Замовлення клієнта"},
                {"РеалізаціяТоварівТаПослуг", "Реалізація товарів та послуг"},
                {"ВстановленняЦінНоменклатури", "Встановлення цін номенклатури"},
                {"ПрихіднийКасовийОрдер", "Прихідний касовий ордер"},
                {"РозхіднийКасовийОрдер", "Розхідний касовий ордер"},
                {"ПереміщенняТоварів", "Переміщення товарів"},
                {"ПоверненняТоварівПостачальнику", "Повернення товарів постачальнику"},
                {"ПоверненняТоварівВідКлієнта", "Повернення товарів від клієнта"},
                {"АктВиконанихРобіт", "Акт виконаних робіт"},
                {"ВведенняЗалишків", "Введення залишків"},
                {"ПерерахунокТоварів", "Перерахунок товарів"},
                {"ПсуванняТоварів", "Псування товарів"},
                {"ВнутрішнєСпоживанняТоварів", "Внутрішнє споживання товарів"},
                {"РахунокФактура", "Рахунок фактура"},
                {"РозміщенняТоварівНаСкладі", "Розміщення товарів на складі"},
                {"ПереміщенняТоварівНаСкладі", "Переміщення товарів на складі"},
                {"ЗбіркаТоварівНаСкладі", "Збірка товарів на складі"},
                {"РозміщенняНоменклатуриПоКоміркам", "Розміщення номенклатури по коміркам"},
                {"КорегуванняБоргу", "Корегування боргу контрагентів"},
                
            };
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static async ValueTask LoadRecords(TreeView treeView) 
        {
            SelectPath = CurrentPath = null;

            List<string> allQuery = [];
            Dictionary<string, object> paramQuery = [];

          
              //Документ: ЗамовленняПостачальнику
              {
                  Query query = new Query(Документи.ЗамовленняПостачальнику_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ЗамовленняПостачальнику", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПоступленняТоварівТаПослуг
              {
                  Query query = new Query(Документи.ПоступленняТоварівТаПослуг_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПоступленняТоварівТаПослуг", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ЗамовленняКлієнта
              {
                  Query query = new Query(Документи.ЗамовленняКлієнта_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ЗамовленняКлієнта", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: РеалізаціяТоварівТаПослуг
              {
                  Query query = new Query(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РеалізаціяТоварівТаПослуг", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ВстановленняЦінНоменклатури
              {
                  Query query = new Query(Документи.ВстановленняЦінНоменклатури_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ВстановленняЦінНоменклатури", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПрихіднийКасовийОрдер
              {
                  Query query = new Query(Документи.ПрихіднийКасовийОрдер_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПрихіднийКасовийОрдер", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: РозхіднийКасовийОрдер
              {
                  Query query = new Query(Документи.РозхіднийКасовийОрдер_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РозхіднийКасовийОрдер", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПереміщенняТоварів
              {
                  Query query = new Query(Документи.ПереміщенняТоварів_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПереміщенняТоварів", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПоверненняТоварівПостачальнику
              {
                  Query query = new Query(Документи.ПоверненняТоварівПостачальнику_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПоверненняТоварівПостачальнику", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПоверненняТоварівВідКлієнта
              {
                  Query query = new Query(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПоверненняТоварівВідКлієнта", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: АктВиконанихРобіт
              {
                  Query query = new Query(Документи.АктВиконанихРобіт_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("АктВиконанихРобіт", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ВведенняЗалишків
              {
                  Query query = new Query(Документи.ВведенняЗалишків_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ВведенняЗалишків", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПерерахунокТоварів
              {
                  Query query = new Query(Документи.ПерерахунокТоварів_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПерерахунокТоварів", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
                  }
                  
                  query.FieldAndAlias.Add(new NameValue<string>("'ПерерахунокТоварів'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПерерахунокТоварів_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.ФізичніОсоби_Const.TABLE, Документи.ПерерахунокТоварів_Const.Відповідальний, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.ФізичніОсоби_Const.Назва, "Контрагент"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПерерахунокТоварів_Const.Склад, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Склади_Const.Назва, "Склад"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПерерахунокТоварів_Const.Автор, query.Table, "join_tab_4"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_4." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              
              //Документ: ПсуванняТоварів
              {
                  Query query = new Query(Документи.ПсуванняТоварів_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПсуванняТоварів", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ВнутрішнєСпоживанняТоварів
              {
                  Query query = new Query(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ВнутрішнєСпоживанняТоварів", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: РахунокФактура
              {
                  Query query = new Query(Документи.РахунокФактура_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РахунокФактура", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: РозміщенняТоварівНаСкладі
              {
                  Query query = new Query(Документи.РозміщенняТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РозміщенняТоварівНаСкладі", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПереміщенняТоварівНаСкладі
              {
                  Query query = new Query(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПереміщенняТоварівНаСкладі", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ЗбіркаТоварівНаСкладі
              {
                  Query query = new Query(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ЗбіркаТоварівНаСкладі", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: РозміщенняНоменклатуриПоКоміркам
              {
                  Query query = new Query(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РозміщенняНоменклатуриПоКоміркам", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: КорегуванняБоргу
              {
                  Query query = new Query(Документи.КорегуванняБоргу_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("КорегуванняБоргу", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
                  }
                  
                  query.FieldAndAlias.Add(new NameValue<string>("'КорегуванняБоргу'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.КорегуванняБоргу_Const.TABLE + "." + Документи.КорегуванняБоргу_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.КорегуванняБоргу_Const.TABLE + "." + Документи.КорегуванняБоргу_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.КорегуванняБоргу_Const.TABLE + "." + Документи.КорегуванняБоргу_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.КорегуванняБоргу_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта"));
                        
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.КорегуванняБоргу_Const.Автор, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.КорегуванняБоргу_Const.TABLE + "." + Документи.КорегуванняБоргу_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            unionAllQuery += "\nORDER BY Дата";

            var recordResult = await Config.Kernel.DataBase.SelectRequest(unionAllQuery, paramQuery);

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Dictionary<string, object> row in recordResult.ListRow)
            {
                Журнали_Повний record = new Журнали_Повний
                {
                    ID = row["uid"].ToString() ?? "",
                    Type = row["type"].ToString() ?? "",
                    DeletionLabel = (bool)row["deletion_label"],
                    Spend = (bool)row["spend"],
                    Назва = row["Назва"] != DBNull.Value ? (row["Назва"].ToString() ?? "") : "",
                    Дата = row["Дата"] != DBNull.Value ? (row["Дата"].ToString() ?? "") : "",
                    Номер = row["Номер"] != DBNull.Value ? (row["Номер"].ToString() ?? "") : "",
                    Організація = row["Організація"] != DBNull.Value ? (row["Організація"].ToString() ?? "") : "",
                    Контрагент = row["Контрагент"] != DBNull.Value ? (row["Контрагент"].ToString() ?? "") : "",
                    Склад = row["Склад"] != DBNull.Value ? (row["Склад"].ToString() ?? "") : "",
                    Каса = row["Каса"] != DBNull.Value ? (row["Каса"].ToString() ?? "") : "",
                    Валюта = row["Валюта"] != DBNull.Value ? (row["Валюта"].ToString() ?? "") : "",
                    Сума = row["Сума"] != DBNull.Value ? (row["Сума"].ToString() ?? "") : "",
                    Автор = row["Автор"] != DBNull.Value ? (row["Автор"].ToString() ?? "") : "",
                    Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"].ToString() ?? "") : "",
                    
                };

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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Type, 
                /*Проведений документ*/ Spend,
                /*Назва*/ Назва, /*Дата*/ Дата, /*Номер*/ Номер, /*Організація*/ Організація, /*Контрагент*/ Контрагент, /*Склад*/ Склад, /*Каса*/ Каса, /*Валюта*/ Валюта, /*Сума*/ Сума, /*Автор*/ Автор, /*Коментар*/ Коментар,  
            };
        }

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                typeof(Gdk.Pixbuf), /* Image */
                typeof(string), /* ID */
                typeof(string), /* Type */
                typeof(bool), /* Spend Проведений документ */
                typeof(string), /*Назва*/
                typeof(string), /*Дата*/
                typeof(string), /*Номер*/
                typeof(string), /*Організація*/
                typeof(string), /*Контрагент*/
                typeof(string), /*Склад*/
                typeof(string), /*Каса*/
                typeof(string), /*Валюта*/
                typeof(string), /*Сума*/
                typeof(string), /*Автор*/
                typeof(string), /*Коментар*/
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            Dictionary<string, List<Where>> WhereDict = [];
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", WhereDict);
            else
                treeView.Data["Where"] = WhereDict;
            
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗамовленняПостачальнику_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ЗамовленняПостачальнику", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПоступленняТоварівТаПослуг", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПоверненняТоварівПостачальнику", [where]);
            }
              
        }

        public static void ОчиститиВідбір(TreeView treeView)
        {
            if (treeView.Data.ContainsKey("Where"))
                treeView.Data["Where"] = null;
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            return new Dictionary<string, string>()
            {
                {"ЗамовленняПостачальнику", "Замовлення постачальнику"},
                {"ПоступленняТоварівТаПослуг", "Поступлення товарів та послуг"},
                {"ПоверненняТоварівПостачальнику", "Повернення товарів постачальнику"},
                
            };
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static async ValueTask LoadRecords(TreeView treeView) 
        {
            SelectPath = CurrentPath = null;

            List<string> allQuery = [];
            Dictionary<string, object> paramQuery = [];

          
              //Документ: ЗамовленняПостачальнику
              {
                  Query query = new Query(Документи.ЗамовленняПостачальнику_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ЗамовленняПостачальнику", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПоступленняТоварівТаПослуг
              {
                  Query query = new Query(Документи.ПоступленняТоварівТаПослуг_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПоступленняТоварівТаПослуг", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПоверненняТоварівПостачальнику
              {
                  Query query = new Query(Документи.ПоверненняТоварівПостачальнику_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПоверненняТоварівПостачальнику", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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

            var recordResult = await Config.Kernel.DataBase.SelectRequest(unionAllQuery, paramQuery);

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Dictionary<string, object> row in recordResult.ListRow)
            {
                Журнали_Закупівлі record = new Журнали_Закупівлі
                {
                    ID = row["uid"].ToString() ?? "",
                    Type = row["type"].ToString() ?? "",
                    DeletionLabel = (bool)row["deletion_label"],
                    Spend = (bool)row["spend"],
                    Назва = row["Назва"] != DBNull.Value ? (row["Назва"].ToString() ?? "") : "",
                    Дата = row["Дата"] != DBNull.Value ? (row["Дата"].ToString() ?? "") : "",
                    Номер = row["Номер"] != DBNull.Value ? (row["Номер"].ToString() ?? "") : "",
                    Організація = row["Організація"] != DBNull.Value ? (row["Організація"].ToString() ?? "") : "",
                    Контрагент = row["Контрагент"] != DBNull.Value ? (row["Контрагент"].ToString() ?? "") : "",
                    Склад = row["Склад"] != DBNull.Value ? (row["Склад"].ToString() ?? "") : "",
                    Каса = row["Каса"] != DBNull.Value ? (row["Каса"].ToString() ?? "") : "",
                    Валюта = row["Валюта"] != DBNull.Value ? (row["Валюта"].ToString() ?? "") : "",
                    Сума = row["Сума"] != DBNull.Value ? (row["Сума"].ToString() ?? "") : "",
                    Автор = row["Автор"] != DBNull.Value ? (row["Автор"].ToString() ?? "") : "",
                    Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"].ToString() ?? "") : "",
                    
                };

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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Type, 
                /*Проведений документ*/ Spend,
                /*Назва*/ Назва, /*Дата*/ Дата, /*Номер*/ Номер, /*Організація*/ Організація, /*Контрагент*/ Контрагент, /*Склад*/ Склад, /*Каса*/ Каса, /*Валюта*/ Валюта, /*Сума*/ Сума, /*Автор*/ Автор, /*Коментар*/ Коментар,  
            };
        }

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                typeof(Gdk.Pixbuf), /* Image */
                typeof(string), /* ID */
                typeof(string), /* Type */
                typeof(bool), /* Spend Проведений документ */
                typeof(string), /*Назва*/
                typeof(string), /*Дата*/
                typeof(string), /*Номер*/
                typeof(string), /*Організація*/
                typeof(string), /*Контрагент*/
                typeof(string), /*Склад*/
                typeof(string), /*Каса*/
                typeof(string), /*Валюта*/
                typeof(string), /*Сума*/
                typeof(string), /*Автор*/
                typeof(string), /*Коментар*/
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            Dictionary<string, List<Where>> WhereDict = [];
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", WhereDict);
            else
                treeView.Data["Where"] = WhereDict;
            
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗамовленняКлієнта_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ЗамовленняКлієнта", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РеалізаціяТоварівТаПослуг", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПоверненняТоварівВідКлієнта", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.АктВиконанихРобіт_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("АктВиконанихРобіт", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РахунокФактура_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РахунокФактура", [where]);
            }
              
        }

        public static void ОчиститиВідбір(TreeView treeView)
        {
            if (treeView.Data.ContainsKey("Where"))
                treeView.Data["Where"] = null;
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            return new Dictionary<string, string>()
            {
                {"ЗамовленняКлієнта", "Замовлення клієнта"},
                {"РеалізаціяТоварівТаПослуг", "Реалізація товарів та послуг"},
                {"ПоверненняТоварівВідКлієнта", "Повернення товарів від клієнта"},
                {"АктВиконанихРобіт", "Акт виконаних робіт"},
                {"РахунокФактура", "Рахунок фактура"},
                
            };
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static async ValueTask LoadRecords(TreeView treeView) 
        {
            SelectPath = CurrentPath = null;

            List<string> allQuery = [];
            Dictionary<string, object> paramQuery = [];

          
              //Документ: ЗамовленняКлієнта
              {
                  Query query = new Query(Документи.ЗамовленняКлієнта_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ЗамовленняКлієнта", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: РеалізаціяТоварівТаПослуг
              {
                  Query query = new Query(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РеалізаціяТоварівТаПослуг", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПоверненняТоварівВідКлієнта
              {
                  Query query = new Query(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПоверненняТоварівВідКлієнта", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: АктВиконанихРобіт
              {
                  Query query = new Query(Документи.АктВиконанихРобіт_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("АктВиконанихРобіт", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: РахунокФактура
              {
                  Query query = new Query(Документи.РахунокФактура_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РахунокФактура", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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

            var recordResult = await Config.Kernel.DataBase.SelectRequest(unionAllQuery, paramQuery);

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Dictionary<string, object> row in recordResult.ListRow)
            {
                Журнали_Продажі record = new Журнали_Продажі
                {
                    ID = row["uid"].ToString() ?? "",
                    Type = row["type"].ToString() ?? "",
                    DeletionLabel = (bool)row["deletion_label"],
                    Spend = (bool)row["spend"],
                    Назва = row["Назва"] != DBNull.Value ? (row["Назва"].ToString() ?? "") : "",
                    Дата = row["Дата"] != DBNull.Value ? (row["Дата"].ToString() ?? "") : "",
                    Номер = row["Номер"] != DBNull.Value ? (row["Номер"].ToString() ?? "") : "",
                    Організація = row["Організація"] != DBNull.Value ? (row["Організація"].ToString() ?? "") : "",
                    Контрагент = row["Контрагент"] != DBNull.Value ? (row["Контрагент"].ToString() ?? "") : "",
                    Склад = row["Склад"] != DBNull.Value ? (row["Склад"].ToString() ?? "") : "",
                    Каса = row["Каса"] != DBNull.Value ? (row["Каса"].ToString() ?? "") : "",
                    Валюта = row["Валюта"] != DBNull.Value ? (row["Валюта"].ToString() ?? "") : "",
                    Сума = row["Сума"] != DBNull.Value ? (row["Сума"].ToString() ?? "") : "",
                    Автор = row["Автор"] != DBNull.Value ? (row["Автор"].ToString() ?? "") : "",
                    Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"].ToString() ?? "") : "",
                    
                };

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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Type, 
                /*Проведений документ*/ Spend,
                /*Назва*/ Назва, /*Дата*/ Дата, /*Номер*/ Номер, /*Організація*/ Організація, /*Контрагент*/ Контрагент, /*Каса*/ Каса, /*Каса2*/ Каса2, /*Валюта*/ Валюта, /*Сума*/ Сума, /*Автор*/ Автор, /*Коментар*/ Коментар,  
            };
        }

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                typeof(Gdk.Pixbuf), /* Image */
                typeof(string), /* ID */
                typeof(string), /* Type */
                typeof(bool), /* Spend Проведений документ */
                typeof(string), /*Назва*/
                typeof(string), /*Дата*/
                typeof(string), /*Номер*/
                typeof(string), /*Організація*/
                typeof(string), /*Контрагент*/
                typeof(string), /*Каса*/
                typeof(string), /*Каса2*/
                typeof(string), /*Валюта*/
                typeof(string), /*Сума*/
                typeof(string), /*Автор*/
                typeof(string), /*Коментар*/
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            Dictionary<string, List<Where>> WhereDict = [];
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", WhereDict);
            else
                treeView.Data["Where"] = WhereDict;
            
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПрихіднийКасовийОрдер", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РозхіднийКасовийОрдер_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РозхіднийКасовийОрдер", [where]);
            }
              
        }

        public static void ОчиститиВідбір(TreeView treeView)
        {
            if (treeView.Data.ContainsKey("Where"))
                treeView.Data["Where"] = null;
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            return new Dictionary<string, string>()
            {
                {"ПрихіднийКасовийОрдер", "Прихідний касовий ордер"},
                {"РозхіднийКасовийОрдер", "Розхідний касовий ордер"},
                
            };
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static async ValueTask LoadRecords(TreeView treeView) 
        {
            SelectPath = CurrentPath = null;

            List<string> allQuery = [];
            Dictionary<string, object> paramQuery = [];

          
              //Документ: ПрихіднийКасовийОрдер
              {
                  Query query = new Query(Документи.ПрихіднийКасовийОрдер_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПрихіднийКасовийОрдер", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: РозхіднийКасовийОрдер
              {
                  Query query = new Query(Документи.РозхіднийКасовийОрдер_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РозхіднийКасовийОрдер", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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

            var recordResult = await Config.Kernel.DataBase.SelectRequest(unionAllQuery, paramQuery);

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Dictionary<string, object> row in recordResult.ListRow)
            {
                Журнали_Каса record = new Журнали_Каса
                {
                    ID = row["uid"].ToString() ?? "",
                    Type = row["type"].ToString() ?? "",
                    DeletionLabel = (bool)row["deletion_label"],
                    Spend = (bool)row["spend"],
                    Назва = row["Назва"] != DBNull.Value ? (row["Назва"].ToString() ?? "") : "",
                    Дата = row["Дата"] != DBNull.Value ? (row["Дата"].ToString() ?? "") : "",
                    Номер = row["Номер"] != DBNull.Value ? (row["Номер"].ToString() ?? "") : "",
                    Організація = row["Організація"] != DBNull.Value ? (row["Організація"].ToString() ?? "") : "",
                    Контрагент = row["Контрагент"] != DBNull.Value ? (row["Контрагент"].ToString() ?? "") : "",
                    Каса = row["Каса"] != DBNull.Value ? (row["Каса"].ToString() ?? "") : "",
                    Каса2 = row["Каса2"] != DBNull.Value ? (row["Каса2"].ToString() ?? "") : "",
                    Валюта = row["Валюта"] != DBNull.Value ? (row["Валюта"].ToString() ?? "") : "",
                    Сума = row["Сума"] != DBNull.Value ? (row["Сума"].ToString() ?? "") : "",
                    Автор = row["Автор"] != DBNull.Value ? (row["Автор"].ToString() ?? "") : "",
                    Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"].ToString() ?? "") : "",
                    
                };

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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Type, 
                /*Проведений документ*/ Spend,
                /*Назва*/ Назва, /*Дата*/ Дата, /*Номер*/ Номер, /*Організація*/ Організація, /*СкладВідправник*/ СкладВідправник, /*СкладОтримувач*/ СкладОтримувач, /*Автор*/ Автор, /*Коментар*/ Коментар,  
            };
        }

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                typeof(Gdk.Pixbuf), /* Image */
                typeof(string), /* ID */
                typeof(string), /* Type */
                typeof(bool), /* Spend Проведений документ */
                typeof(string), /*Назва*/
                typeof(string), /*Дата*/
                typeof(string), /*Номер*/
                typeof(string), /*Організація*/
                typeof(string), /*СкладВідправник*/
                typeof(string), /*СкладОтримувач*/
                typeof(string), /*Автор*/
                typeof(string), /*Коментар*/
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            Dictionary<string, List<Where>> WhereDict = [];
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", WhereDict);
            else
                treeView.Data["Where"] = WhereDict;
            
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПереміщенняТоварів_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПереміщенняТоварів", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ВведенняЗалишків_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ВведенняЗалишків", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПерерахунокТоварів_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПерерахунокТоварів", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПсуванняТоварів_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПсуванняТоварів", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ВнутрішнєСпоживанняТоварів", [where]);
            }
              
        }

        public static void ОчиститиВідбір(TreeView treeView)
        {
            if (treeView.Data.ContainsKey("Where"))
                treeView.Data["Where"] = null;
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            return new Dictionary<string, string>()
            {
                {"ПереміщенняТоварів", "Переміщення товарів"},
                {"ВведенняЗалишків", "Введення залишків"},
                {"ПерерахунокТоварів", "Перерахунок товарів"},
                {"ПсуванняТоварів", "Псування товарів"},
                {"ВнутрішнєСпоживанняТоварів", "Внутрішнє споживання товарів"},
                
            };
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static async ValueTask LoadRecords(TreeView treeView) 
        {
            SelectPath = CurrentPath = null;

            List<string> allQuery = [];
            Dictionary<string, object> paramQuery = [];

          
              //Документ: ПереміщенняТоварів
              {
                  Query query = new Query(Документи.ПереміщенняТоварів_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПереміщенняТоварів", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ВведенняЗалишків
              {
                  Query query = new Query(Документи.ВведенняЗалишків_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ВведенняЗалишків", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ВнутрішнєСпоживанняТоварів
              {
                  Query query = new Query(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ВнутрішнєСпоживанняТоварів", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПсуванняТоварів
              {
                  Query query = new Query(Документи.ПсуванняТоварів_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПсуванняТоварів", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПерерахунокТоварів
              {
                  Query query = new Query(Документи.ПерерахунокТоварів_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПерерахунокТоварів", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
                  }
                  
                  query.FieldAndAlias.Add(new NameValue<string>("'ПерерахунокТоварів'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.НомерДок, "Номер"));
                            
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Організації_Const.TABLE, Документи.ПерерахунокТоварів_Const.Організація, query.Table, "join_tab_1"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_1." + Довідники.Організації_Const.Назва, "Організація"));
                              
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Склади_Const.TABLE, Документи.ПерерахунокТоварів_Const.Склад, query.Table, "join_tab_2"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_2." + Довідники.Склади_Const.Назва, "СкладВідправник"));
                              
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue<string>("''", "СкладОтримувач"));
                        
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(Довідники.Користувачі_Const.TABLE, Документи.ПерерахунокТоварів_Const.Автор, query.Table, "join_tab_3"));
                              
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue<string>("join_tab_3." + Довідники.Користувачі_Const.Назва, "Автор"));
                              
                              query.FieldAndAlias.Add(
                                  new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.Коментар, "Коментар"));
                            

                  allQuery.Add(query.Construct());
              }
              

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            unionAllQuery += "\nORDER BY Дата";

            var recordResult = await Config.Kernel.DataBase.SelectRequest(unionAllQuery, paramQuery);

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Dictionary<string, object> row in recordResult.ListRow)
            {
                Журнали_Склад record = new Журнали_Склад
                {
                    ID = row["uid"].ToString() ?? "",
                    Type = row["type"].ToString() ?? "",
                    DeletionLabel = (bool)row["deletion_label"],
                    Spend = (bool)row["spend"],
                    Назва = row["Назва"] != DBNull.Value ? (row["Назва"].ToString() ?? "") : "",
                    Дата = row["Дата"] != DBNull.Value ? (row["Дата"].ToString() ?? "") : "",
                    Номер = row["Номер"] != DBNull.Value ? (row["Номер"].ToString() ?? "") : "",
                    Організація = row["Організація"] != DBNull.Value ? (row["Організація"].ToString() ?? "") : "",
                    СкладВідправник = row["СкладВідправник"] != DBNull.Value ? (row["СкладВідправник"].ToString() ?? "") : "",
                    СкладОтримувач = row["СкладОтримувач"] != DBNull.Value ? (row["СкладОтримувач"].ToString() ?? "") : "",
                    Автор = row["Автор"] != DBNull.Value ? (row["Автор"].ToString() ?? "") : "",
                    Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"].ToString() ?? "") : "",
                    
                };

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
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Type, 
                /*Проведений документ*/ Spend,
                /*Назва*/ Назва, /*Дата*/ Дата, /*Номер*/ Номер, /*Організація*/ Організація, /*Склад*/ Склад, /*Автор*/ Автор, /*Коментар*/ Коментар,  
            };
        }

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                typeof(Gdk.Pixbuf), /* Image */
                typeof(string), /* ID */
                typeof(string), /* Type */
                typeof(bool), /* Spend Проведений документ */
                typeof(string), /*Назва*/
                typeof(string), /*Дата*/
                typeof(string), /*Номер*/
                typeof(string), /*Організація*/
                typeof(string), /*Склад*/
                typeof(string), /*Автор*/
                typeof(string), /*Коментар*/
                
            ]);

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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, string типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ДодатиВідбірПоПеріоду(treeView, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(типПеріоду), start, stop);
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            Dictionary<string, List<Where>> WhereDict = [];
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", WhereDict);
            else
                treeView.Data["Where"] = WhereDict;
            
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РозміщенняТоварівНаСкладі", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ПереміщенняТоварівНаСкладі", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ЗбіркаТоварівНаСкладі", [where]);
            }
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("РозміщенняНоменклатуриПоКоміркам", [where]);
            }
              
        }

        public static void ОчиститиВідбір(TreeView treeView)
        {
            if (treeView.Data.ContainsKey("Where"))
                treeView.Data["Where"] = null;
        }

        // Список документів які входять в журнал
        public static Dictionary<string, string> AllowDocument()
        {
            return new Dictionary<string, string>()
            {
                {"РозміщенняТоварівНаСкладі", "Розміщення товарів на складі"},
                {"ПереміщенняТоварівНаСкладі", "Переміщення товарів на складі"},
                {"ЗбіркаТоварівНаСкладі", "Збірка товарів на складі"},
                {"РозміщенняНоменклатуриПоКоміркам", "Розміщення номенклатури по коміркам"},
                
            };
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static async ValueTask LoadRecords(TreeView treeView) 
        {
            SelectPath = CurrentPath = null;

            List<string> allQuery = [];
            Dictionary<string, object> paramQuery = [];

          
              //Документ: РозміщенняТоварівНаСкладі
              {
                  Query query = new Query(Документи.РозміщенняТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РозміщенняТоварівНаСкладі", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ПереміщенняТоварівНаСкладі
              {
                  Query query = new Query(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ПереміщенняТоварівНаСкладі", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: ЗбіркаТоварівНаСкладі
              {
                  Query query = new Query(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ЗбіркаТоварівНаСкладі", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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
              
              //Документ: РозміщенняНоменклатуриПоКоміркам
              {
                  Query query = new Query(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("РозміщенняНоменклатуриПоКоміркам", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
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

            var recordResult = await Config.Kernel.DataBase.SelectRequest(unionAllQuery, paramQuery);

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Dictionary<string, object> row in recordResult.ListRow)
            {
                Журнали_АдреснеЗберігання record = new Журнали_АдреснеЗберігання
                {
                    ID = row["uid"].ToString() ?? "",
                    Type = row["type"].ToString() ?? "",
                    DeletionLabel = (bool)row["deletion_label"],
                    Spend = (bool)row["spend"],
                    Назва = row["Назва"] != DBNull.Value ? (row["Назва"].ToString() ?? "") : "",
                    Дата = row["Дата"] != DBNull.Value ? (row["Дата"].ToString() ?? "") : "",
                    Номер = row["Номер"] != DBNull.Value ? (row["Номер"].ToString() ?? "") : "",
                    Організація = row["Організація"] != DBNull.Value ? (row["Організація"].ToString() ?? "") : "",
                    Склад = row["Склад"] != DBNull.Value ? (row["Склад"].ToString() ?? "") : "",
                    Автор = row["Автор"] != DBNull.Value ? (row["Автор"].ToString() ?? "") : "",
                    Коментар = row["Коментар"] != DBNull.Value ? (row["Коментар"].ToString() ?? "") : "",
                    
                };

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
    
      
    public class ЦіниНоменклатури_Записи : ТабличнийСписок
    {
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
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Період,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*ВидЦіни*/ ВидЦіни,
                /*Ціна*/ Ціна,
                /*Пакування*/ Пакування,
                /*Валюта*/ Валюта,
                 
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*ВидЦіни*/ typeof(string),
                /*Ціна*/ typeof(string),
                /*Пакування*/ typeof(string),
                /*Валюта*/ typeof(string),
                
            ]);

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

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.ЦіниНоменклатури_RecordsSet ЦіниНоменклатури_RecordsSet = new РегістриВідомостей.ЦіниНоменклатури_RecordsSet();

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ЦіниНоменклатури_RecordsSet.QuerySelect.Where = (List<Where>)where;
            }

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
            await ЦіниНоменклатури_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

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
    
      
    public class КурсиВалют_Записи : ТабличнийСписок
    {
        string ID = "";
        string Період = "";
        
        string Валюта = "";
        string Курс = "";
        string Кратність = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Період,
                /*Валюта*/ Валюта,
                /*Курс*/ Курс,
                /*Кратність*/ Кратність,
                 
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Валюта*/ typeof(string),
                /*Курс*/ typeof(string),
                /*Кратність*/ typeof(string),
                
            ]);

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

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.КурсиВалют_RecordsSet КурсиВалют_RecordsSet = new РегістриВідомостей.КурсиВалют_RecordsSet();

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) КурсиВалют_RecordsSet.QuerySelect.Where = (List<Where>)where;
            }

            /* DEFAULT ORDER */
            КурсиВалют_RecordsSet.QuerySelect.Order.Add("period", SelectOrder.ASC);

            
                /* Join Table */
                КурсиВалют_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.Валюти_Const.TABLE, РегістриВідомостей.КурсиВалют_Const.Валюта, КурсиВалют_RecordsSet.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  КурсиВалют_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Валюти_Const.Назва, "join_tab_1_field_1"));
                  

            /* Read */
            await КурсиВалют_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

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
    
      
    public class ШтрихкодиНоменклатури_Записи : ТабличнийСписок
    {
        string ID = "";
        string Період = "";
        
        string Штрихкод = "";
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Пакування = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Період,
                /*Штрихкод*/ Штрихкод,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*Пакування*/ Пакування,
                 
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Штрихкод*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*Пакування*/ typeof(string),
                
            ]);

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

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet ШтрихкодиНоменклатури_RecordsSet = new РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet();

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Where = (List<Where>)where;
            }

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
            await ШтрихкодиНоменклатури_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

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
    
      
    public class ФайлиДокументів_Записи : ТабличнийСписок
    {
        string ID = "";
        string Період = "";
        
        string Файл = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Період,
                /*Файл*/ Файл,
                 
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Файл*/ typeof(string),
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 2));
            /* */
            treeView.AppendColumn(new TreeViewColumn("Файл", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Файл*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.ФайлиДокументів_RecordsSet ФайлиДокументів_RecordsSet = new РегістриВідомостей.ФайлиДокументів_RecordsSet();

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) ФайлиДокументів_RecordsSet.QuerySelect.Where = (List<Where>)where;
            }

            /* DEFAULT ORDER */
            ФайлиДокументів_RecordsSet.QuerySelect.Order.Add("period", SelectOrder.ASC);

            
                /* Join Table */
                ФайлиДокументів_RecordsSet.QuerySelect.Joins.Add(
                    new Join(Довідники.Файли_Const.TABLE, РегістриВідомостей.ФайлиДокументів_Const.Файл, ФайлиДокументів_RecordsSet.QuerySelect.Table, "join_tab_1"));
                
                  /* Field */
                  ФайлиДокументів_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue<string>("join_tab_1." + Довідники.Файли_Const.Назва, "join_tab_1_field_1"));
                  

            /* Read */
            await ФайлиДокументів_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

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

  