﻿
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
 * Конфігурації ""Зберігання та Торгівля" для України"
 * Автор Тарахомин Юрій Іванович, accounting.org.ua
 * Дата конфігурації: 05.11.2024 14:56:28
 *
 *
 * Цей код згенерований в Конфігураторі 3. Шаблон Gtk.xslt
 *
 */

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Перелічення;
using StorageAndTrade;

namespace StorageAndTrade_1_0.Довідники.ТабличніСписки
{
    
    #region DIRECTORY "Організації"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Організації_Select.QuerySelect.Where = (List<Where>)where;

            Організації_Select.QuerySelect.Order.Add(
               Довідники.Організації_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Номенклатура"
      
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
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* ОдиницяВиміру, pointer */
                      Switch sw = new();
                      ПакуванняОдиниціВиміру_PointerControl ОдиницяВиміру = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("ОдиницяВиміру", ОдиницяВиміру, sw));
                      ДодатиЕлементВФільтр(listBox, "Пакування:", ОдиницяВиміру, sw);
                  }
                  
                  { /* ТипНоменклатури, enum */
                      Switch sw = new();
                      ComboBoxText ТипНоменклатури = new();
                          foreach (var item in ПсевдонімиПерелічення.ТипиНоменклатури_List()) ТипНоменклатури.Append(item.Value.ToString(), item.Name);
                          ТипНоменклатури.Active = 0;
                          
                      widgets.Add(new("ТипНоменклатури", ТипНоменклатури, sw));
                      ДодатиЕлементВФільтр(listBox, "Тип:", ТипНоменклатури, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "ОдиницяВиміру" => Номенклатура_Const.ОдиницяВиміру,
                                  "ТипНоменклатури" => Номенклатура_Const.ТипНоменклатури,
                                   _ => null };
                                  object? value = widget.Item1 switch { "ОдиницяВиміру" => ((ПакуванняОдиниціВиміру_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "ТипНоменклатури" => (int)Enum.Parse<ТипиНоменклатури>(((ComboBoxText)widget.Item2).ActiveId),
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Номенклатура_Select.QuerySelect.Where = (List<Where>)where;

            Номенклатура_Select.QuerySelect.Order.Add(
               Довідники.Номенклатура_Const.Назва, SelectOrder.ASC);
            Довідники.ПакуванняОдиниціВиміру_Pointer.GetJoin(Номенклатура_Select.QuerySelect, Довідники.Номенклатура_Const.ОдиницяВиміру,
                Номенклатура_Select.QuerySelect.Table, "join_tab_1", "ОдиницяВиміру");
            
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
                        Код = Fields[Номенклатура_Const.Код].ToString() ?? "",
                            Назва = Fields[Номенклатура_Const.Назва].ToString() ?? "",
                            ОдиницяВиміру = Fields["ОдиницяВиміру"].ToString() ?? "",
                            ТипНоменклатури = Перелічення.ПсевдонімиПерелічення.ТипиНоменклатури_Alias((
                              (Перелічення.ТипиНоменклатури)(Fields[Номенклатура_Const.ТипНоменклатури] != DBNull.Value ? Fields[Номенклатура_Const.ТипНоменклатури] : 0)) ),
                            Залишок = Fields["Залишок"].ToString() ?? "",
                        ВРезерві = Fields["ВРезерві"].ToString() ?? "",
                        ВРезервіПідЗамовлення = Fields["ВРезервіПідЗамовлення"].ToString() ?? "",
                        ЗалишокВКомірках = Fields["ЗалишокВКомірках"].ToString() ?? "",
                        
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Номенклатура_Select.QuerySelect.Where = (List<Where>)where;

            Номенклатура_Select.QuerySelect.Order.Add(
               Довідники.Номенклатура_Const.Назва, SelectOrder.ASC);
            Довідники.ПакуванняОдиниціВиміру_Pointer.GetJoin(Номенклатура_Select.QuerySelect, Довідники.Номенклатура_Const.ОдиницяВиміру,
                Номенклатура_Select.QuerySelect.Table, "join_tab_1", "ОдиницяВиміру");
            
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
                        Код = Fields[Номенклатура_Const.Код].ToString() ?? "",
                            Назва = Fields[Номенклатура_Const.Назва].ToString() ?? "",
                            ОдиницяВиміру = Fields["ОдиницяВиміру"].ToString() ?? "",
                            Залишок = Fields["Залишок"].ToString() ?? "",
                        ВРезерві = Fields["ВРезерві"].ToString() ?? "",
                        ВРезервіПідЗамовлення = Fields["ВРезервіПідЗамовлення"].ToString() ?? "",
                        
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Виробники"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Виробники_Select.QuerySelect.Where = (List<Where>)where;

            Виробники_Select.QuerySelect.Order.Add(
               Довідники.Виробники_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиНоменклатури"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ВидиНоменклатури_Select.QuerySelect.Where = (List<Where>)where;

            ВидиНоменклатури_Select.QuerySelect.Order.Add(
               Довідники.ВидиНоменклатури_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ПакуванняОдиниціВиміру"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ПакуванняОдиниціВиміру_Select.QuerySelect.Where = (List<Where>)where;

            ПакуванняОдиниціВиміру_Select.QuerySelect.Order.Add(
               Довідники.ПакуванняОдиниціВиміру_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Валюти"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Код_R030, string */
                      Switch sw = new();
                      Entry Код_R030 = new() { WidthRequest = 400 };
                      widgets.Add(new("Код_R030", Код_R030, sw));
                      ДодатиЕлементВФільтр(listBox, "R030:", Код_R030, sw);
                  }
                  
                  { /* ВиводитиКурсНаСтартову, boolean */
                      Switch sw = new();
                      CheckButton ВиводитиКурсНаСтартову = new();
                          ВиводитиКурсНаСтартову.Clicked += (object? sender, EventArgs args) => sw.Active = ВиводитиКурсНаСтартову.Active;
                          
                      widgets.Add(new("ВиводитиКурсНаСтартову", ВиводитиКурсНаСтартову, sw));
                      ДодатиЕлементВФільтр(listBox, "Показувати на стартовій:", ВиводитиКурсНаСтартову, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Код_R030" => Валюти_Const.Код_R030,
                                  "ВиводитиКурсНаСтартову" => Валюти_Const.ВиводитиКурсНаСтартову,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Код_R030" => ((Entry)widget.Item2).Text,
                                  "ВиводитиКурсНаСтартову" => ((CheckButton)widget.Item2).Active,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Валюти_Select.QuerySelect.Where = (List<Where>)where;

            Валюти_Select.QuerySelect.Order.Add(
               Довідники.Валюти_Const.Код, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    public class Валюти_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        
        string Назва = "";
        
        string КороткаНазва = "";
        

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*КороткаНазва*/ КороткаНазва,
                
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
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Коротка назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*КороткаНазва*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Валюти_Select Валюти_Select = new Довідники.Валюти_Select();
            Валюти_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Валюти_Const.Код,
                /*Назва*/ Довідники.Валюти_Const.Назва,
                /*КороткаНазва*/ Довідники.Валюти_Const.КороткаНазва,
                
            ]);

            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) Валюти_Select.QuerySelect.Where = (List<Where>)where;

            Валюти_Select.QuerySelect.Order.Add(
               Довідники.Валюти_Const.Код, SelectOrder.ASC);
            

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
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Контрагенти"
      
    public class Контрагенти_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        
        string Назва = "";
        
        string Папка = "";
        
        string Постачальник = "";
        
        string Покупець = "";
        

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Папка*/ Папка,
                /*Постачальник*/ Постачальник,
                /*Покупець*/ Покупець,
                
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
                /*Постачальник*/ typeof(string),  
                /*Покупець*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Папка", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Папка*/
            treeView.AppendColumn(new TreeViewColumn("Постачальник", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Постачальник*/
            treeView.AppendColumn(new TreeViewColumn("Покупець", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Покупець*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Постачальник, boolean */
                      Switch sw = new();
                      CheckButton Постачальник = new();
                          Постачальник.Clicked += (object? sender, EventArgs args) => sw.Active = Постачальник.Active;
                          
                      widgets.Add(new("Постачальник", Постачальник, sw));
                      ДодатиЕлементВФільтр(listBox, "Постачальник:", Постачальник, sw);
                  }
                  
                  { /* Покупець, boolean */
                      Switch sw = new();
                      CheckButton Покупець = new();
                          Покупець.Clicked += (object? sender, EventArgs args) => sw.Active = Покупець.Active;
                          
                      widgets.Add(new("Покупець", Покупець, sw));
                      ДодатиЕлементВФільтр(listBox, "Покупець:", Покупець, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Постачальник" => Контрагенти_Const.Постачальник,
                                  "Покупець" => Контрагенти_Const.Покупець,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Постачальник" => ((CheckButton)widget.Item2).Active,
                                  "Покупець" => ((CheckButton)widget.Item2).Active,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Контрагенти_Select Контрагенти_Select = new Довідники.Контрагенти_Select();
            Контрагенти_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.Контрагенти_Const.Код,
                /*Назва*/ Довідники.Контрагенти_Const.Назва,
                /*Постачальник*/ Довідники.Контрагенти_Const.Постачальник,
                /*Покупець*/ Довідники.Контрагенти_Const.Покупець,
                
            ]);

            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) Контрагенти_Select.QuerySelect.Where = (List<Where>)where;

            Контрагенти_Select.QuerySelect.Order.Add(
               Довідники.Контрагенти_Const.Назва, SelectOrder.ASC);
            Довідники.Контрагенти_Папки_Pointer.GetJoin(Контрагенти_Select.QuerySelect, Довідники.Контрагенти_Const.Папка,
                Контрагенти_Select.QuerySelect.Table, "join_tab_1", "Папка");
            

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
                        Код = Fields[Контрагенти_Const.Код].ToString() ?? "",
                            Назва = Fields[Контрагенти_Const.Назва].ToString() ?? "",
                            Папка = Fields["Папка"].ToString() ?? "",
                            Постачальник = (Fields[Контрагенти_Const.Постачальник] != DBNull.Value ? (bool)Fields[Контрагенти_Const.Постачальник] : false) ? "Так" : "",
                            Покупець = (Fields[Контрагенти_Const.Покупець] != DBNull.Value ? (bool)Fields[Контрагенти_Const.Покупець] : false) ? "Так" : "",
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Контрагенти_Select.QuerySelect.Where = (List<Where>)where;

            Контрагенти_Select.QuerySelect.Order.Add(
               Довідники.Контрагенти_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Склади"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* ТипСкладу, enum */
                      Switch sw = new();
                      ComboBoxText ТипСкладу = new();
                          foreach (var item in ПсевдонімиПерелічення.ТипиСкладів_List()) ТипСкладу.Append(item.Value.ToString(), item.Name);
                          ТипСкладу.Active = 0;
                          
                      widgets.Add(new("ТипСкладу", ТипСкладу, sw));
                      ДодатиЕлементВФільтр(listBox, "Тип cкладу:", ТипСкладу, sw);
                  }
                  
                  { /* НалаштуванняАдресногоЗберігання, enum */
                      Switch sw = new();
                      ComboBoxText НалаштуванняАдресногоЗберігання = new();
                          foreach (var item in ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_List()) НалаштуванняАдресногоЗберігання.Append(item.Value.ToString(), item.Name);
                          НалаштуванняАдресногоЗберігання.Active = 0;
                          
                      widgets.Add(new("НалаштуванняАдресногоЗберігання", НалаштуванняАдресногоЗберігання, sw));
                      ДодатиЕлементВФільтр(listBox, "Адресне зберігання:", НалаштуванняАдресногоЗберігання, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "ТипСкладу" => Склади_Const.ТипСкладу,
                                  "НалаштуванняАдресногоЗберігання" => Склади_Const.НалаштуванняАдресногоЗберігання,
                                   _ => null };
                                  object? value = widget.Item1 switch { "ТипСкладу" => (int)Enum.Parse<ТипиСкладів>(((ComboBoxText)widget.Item2).ActiveId),
                                  "НалаштуванняАдресногоЗберігання" => (int)Enum.Parse<НалаштуванняАдресногоЗберігання>(((ComboBoxText)widget.Item2).ActiveId),
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Склади_Select.QuerySelect.Where = (List<Where>)where;

            Склади_Select.QuerySelect.Order.Add(
               Довідники.Склади_Const.Назва, SelectOrder.ASC);
            

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
                            ТипСкладу = Перелічення.ПсевдонімиПерелічення.ТипиСкладів_Alias((
                              (Перелічення.ТипиСкладів)(Fields[Склади_Const.ТипСкладу] != DBNull.Value ? Fields[Склади_Const.ТипСкладу] : 0)) ),
                            НалаштуванняАдресногоЗберігання = Перелічення.ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_Alias((
                              (Перелічення.НалаштуванняАдресногоЗберігання)(Fields[Склади_Const.НалаштуванняАдресногоЗберігання] != DBNull.Value ? Fields[Склади_Const.НалаштуванняАдресногоЗберігання] : 0)) ),
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Склади_Select.QuerySelect.Where = (List<Where>)where;

            Склади_Select.QuerySelect.Order.Add(
               Довідники.Склади_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиЦін"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Валюта" => ВидиЦін_Const.Валюта,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ВидиЦін_Select.QuerySelect.Where = (List<Where>)where;

            ВидиЦін_Select.QuerySelect.Order.Add(
               Довідники.ВидиЦін_Const.Назва, SelectOrder.ASC);
            Довідники.Валюти_Pointer.GetJoin(ВидиЦін_Select.QuerySelect, Довідники.ВидиЦін_Const.Валюта,
                ВидиЦін_Select.QuerySelect.Table, "join_tab_1", "Валюта");
            

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
                        Код = Fields[ВидиЦін_Const.Код].ToString() ?? "",
                            Назва = Fields[ВидиЦін_Const.Назва].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ВидиЦін_Select ВидиЦін_Select = new Довідники.ВидиЦін_Select();
            ВидиЦін_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ВидиЦін_Const.Назва,
                
            ]);

            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ВидиЦін_Select.QuerySelect.Where = (List<Where>)where;

            ВидиЦін_Select.QuerySelect.Order.Add(
               Довідники.ВидиЦін_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиЦінПостачальників"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ВидиЦінПостачальників_Select.QuerySelect.Where = (List<Where>)where;

            ВидиЦінПостачальників_Select.QuerySelect.Order.Add(
               Довідники.ВидиЦінПостачальників_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Користувачі"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Користувачі_Select.QuerySelect.Where = (List<Where>)where;

            Користувачі_Select.QuerySelect.Order.Add(
               Довідники.Користувачі_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ФізичніОсоби"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ФізичніОсоби_Select.QuerySelect.Where = (List<Where>)where;

            ФізичніОсоби_Select.QuerySelect.Order.Add(
               Довідники.ФізичніОсоби_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СтруктураПідприємства"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) СтруктураПідприємства_Select.QuerySelect.Where = (List<Where>)where;

            СтруктураПідприємства_Select.QuerySelect.Order.Add(
               Довідники.СтруктураПідприємства_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "КраїниСвіту"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) КраїниСвіту_Select.QuerySelect.Where = (List<Where>)where;

            КраїниСвіту_Select.QuerySelect.Order.Add(
               Довідники.КраїниСвіту_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Файли"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Файли_Select.QuerySelect.Where = (List<Where>)where;

            Файли_Select.QuerySelect.Order.Add(
               Довідники.Файли_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Файли_Select Файли_Select = new Довідники.Файли_Select();
            Файли_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.Файли_Const.Назва,
                
            ]);

            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) Файли_Select.QuerySelect.Where = (List<Where>)where;

            Файли_Select.QuerySelect.Order.Add(
               Довідники.Файли_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ХарактеристикиНоменклатури"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ХарактеристикиНоменклатури_Select.QuerySelect.Where = (List<Where>)where;

            ХарактеристикиНоменклатури_Select.QuerySelect.Order.Add(
               Довідники.ХарактеристикиНоменклатури_Const.Назва, SelectOrder.ASC);
            Довідники.Номенклатура_Pointer.GetJoin(ХарактеристикиНоменклатури_Select.QuerySelect, Довідники.ХарактеристикиНоменклатури_Const.Номенклатура,
                ХарактеристикиНоменклатури_Select.QuerySelect.Table, "join_tab_1", "Номенклатура");
            
                /* Additional Field */
                ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} AS Характеристика, SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності}) AS ВНаявності FROM {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах WHERE ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid GROUP BY Характеристика ) SELECT ROUND(ВНаявності, 1) FROM Залишки)", "Залишки"));
                /*
                WITH Залишки AS
(
    SELECT
        ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} AS Характеристика,
        SUM(ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ВНаявності}) AS ВНаявності
    FROM
        {РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах
    WHERE
        ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} 
		AND
        ТовариНаСкладах.{РегістриНакопичення.ТовариНаСкладах_Підсумки_TablePart.ХарактеристикаНоменклатури} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid
    GROUP BY Характеристика
)
SELECT
    ROUND(ВНаявності, 1)
FROM
    Залишки



                */
            
                /* Additional Field */
                ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS Характеристика, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу}) AS ВРезервіЗіСкладу FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid GROUP BY Характеристика ) SELECT ROUND(ВРезервіЗіСкладу, 1) FROM Залишки)", "ВРезерві"));
                /*
                WITH Залишки AS
(
    SELECT
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS Характеристика,
        SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіЗіСкладу}) AS ВРезервіЗіСкладу
    FROM
        {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
    WHERE
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} 
		AND
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid
    GROUP BY Характеристика
)
SELECT
    ROUND(ВРезервіЗіСкладу, 1)
FROM
    Залишки
                */
            
                /* Additional Field */
                ХарактеристикиНоменклатури_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS Характеристика, SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення}) AS ВРезервіПідЗамовлення FROM {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки WHERE ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} AND ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid GROUP BY Характеристика ) SELECT ROUND(ВРезервіПідЗамовлення, 1) FROM Залишки)", "ВРезервіПідЗамовлення"));
                /*
                WITH Залишки AS
(
    SELECT
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} AS Характеристика,
        SUM(ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ВРезервіПідЗамовлення}) AS ВРезервіПідЗамовлення
    FROM
        {РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.TABLE} AS ВільніЗалишки
    WHERE
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.Номенклатура} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.{Довідники.ХарактеристикиНоменклатури_Const.Номенклатура} 
     		 AND
        ВільніЗалишки.{РегістриНакопичення.ВільніЗалишки_Підсумки_TablePart.ХарактеристикаНоменклатури} = 
            {Довідники.ХарактеристикиНоменклатури_Const.TABLE}.uid
    GROUP BY Характеристика
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
                        Код = Fields[ХарактеристикиНоменклатури_Const.Код].ToString() ?? "",
                            Номенклатура = Fields["Номенклатура"].ToString() ?? "",
                            Назва = Fields[ХарактеристикиНоменклатури_Const.Назва].ToString() ?? "",
                            Залишки = Fields["Залишки"].ToString() ?? "",
                        ВРезерві = Fields["ВРезерві"].ToString() ?? "",
                        ВРезервіПідЗамовлення = Fields["ВРезервіПідЗамовлення"].ToString() ?? "",
                        
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Номенклатура_Папки"
      
    public class Номенклатура_Папки_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "Дерево";
        
        string Код = "";
        

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                /*Назва*/ Назва,
                /*Код*/ Код,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Код*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Код*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Номенклатура_Папки_SelectHierarchical Номенклатура_Папки_Select = new Довідники.Номенклатура_Папки_SelectHierarchical();
            Номенклатура_Папки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.Номенклатура_Папки_Const.Назва,
                /*Код*/ Довідники.Номенклатура_Папки_Const.Код,
                
            ]);

            
            if (OpenFolder != null) 
              ДодатиВідбір(treeView, new Where("uid", Comparison.NOT, OpenFolder.UGuid));
            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) Номенклатура_Папки_Select.QuerySelect.Where = (List<Where>)where;

            Номенклатура_Папки_Select.QuerySelect.Order.Add(
               Довідники.Номенклатура_Папки_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Номенклатура_Папки_Select.Select();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            
            Dictionary<string, TreeIter> nodeDictionary = new();
            TreeIter rootIter = Store.AppendValues(new Номенклатура_Папки_Записи(){ ID = Guid.Empty.ToString() }.ToArray());
            TreePath rootPath = Store.GetPath(rootIter);
            

            while (Номенклатура_Папки_Select.MoveNext())
            {
                Довідники.Номенклатура_Папки_Pointer? cur = Номенклатура_Папки_Select.Current;
                
                string Parent = Номенклатура_Папки_Select.Parent?.ToString() ?? Guid.Empty.ToString();
                int Level = Номенклатура_Папки_Select.Level;
                
                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Номенклатура_Папки_Записи Record = new Номенклатура_Папки_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[Номенклатура_Папки_Const.Назва].ToString() ?? "",
                            Код = Fields[Номенклатура_Папки_Const.Код].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Level == 1 ? Store.AppendValues(rootIter, Record.ToArray()) : Store.AppendValues(nodeDictionary[Parent], Record.ToArray());
                        nodeDictionary.Add(Record.ID, CurrentIter);
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            treeView.ExpandToPath(rootPath);
            treeView.SetCursor(rootPath, treeView.Columns[0], false);
            if (SelectPath != null) treeView.ExpandToPath(SelectPath);
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
            treeView.ActivateRow(SelectPath != null ? SelectPath: rootPath, treeView.Columns[0]);
            
        }
    }
	    
    public class Номенклатура_Папки_ЗаписиШвидкийВибір : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "Дерево";
        
        string Код = "";
        

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                /*Назва*/ Назва,
                /*Код*/ Код,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Код*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Код*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Номенклатура_Папки_SelectHierarchical Номенклатура_Папки_Select = new Довідники.Номенклатура_Папки_SelectHierarchical();
            Номенклатура_Папки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.Номенклатура_Папки_Const.Назва,
                /*Код*/ Довідники.Номенклатура_Папки_Const.Код,
                
            ]);

            
            if (OpenFolder != null) 
              ДодатиВідбір(treeView, new Where("uid", Comparison.NOT, OpenFolder.UGuid));
            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) Номенклатура_Папки_Select.QuerySelect.Where = (List<Where>)where;

            Номенклатура_Папки_Select.QuerySelect.Order.Add(
               Довідники.Номенклатура_Папки_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Номенклатура_Папки_Select.Select();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            
            Dictionary<string, TreeIter> nodeDictionary = new();
            TreeIter rootIter = Store.AppendValues(new Номенклатура_Папки_ЗаписиШвидкийВибір(){ ID = Guid.Empty.ToString() }.ToArray());
            TreePath rootPath = Store.GetPath(rootIter);
            

            while (Номенклатура_Папки_Select.MoveNext())
            {
                Довідники.Номенклатура_Папки_Pointer? cur = Номенклатура_Папки_Select.Current;
                
                string Parent = Номенклатура_Папки_Select.Parent?.ToString() ?? Guid.Empty.ToString();
                int Level = Номенклатура_Папки_Select.Level;
                
                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Номенклатура_Папки_ЗаписиШвидкийВибір Record = new Номенклатура_Папки_ЗаписиШвидкийВибір
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[Номенклатура_Папки_Const.Назва].ToString() ?? "",
                            Код = Fields[Номенклатура_Папки_Const.Код].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Level == 1 ? Store.AppendValues(rootIter, Record.ToArray()) : Store.AppendValues(nodeDictionary[Parent], Record.ToArray());
                        nodeDictionary.Add(Record.ID, CurrentIter);
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            treeView.ExpandToPath(rootPath);
            treeView.SetCursor(rootPath, treeView.Columns[0], false);
            if (SelectPath != null) treeView.ExpandToPath(SelectPath);
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
            treeView.ActivateRow(SelectPath != null ? SelectPath: rootPath, treeView.Columns[0]);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Контрагенти_Папки"
      
    public class Контрагенти_Папки_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "Дерево";
        
        string Код = "";
        

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                /*Назва*/ Назва,
                /*Код*/ Код,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Код*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Код*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Контрагенти_Папки_SelectHierarchical Контрагенти_Папки_Select = new Довідники.Контрагенти_Папки_SelectHierarchical();
            Контрагенти_Папки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.Контрагенти_Папки_Const.Назва,
                /*Код*/ Довідники.Контрагенти_Папки_Const.Код,
                
            ]);

            
            if (OpenFolder != null) 
              ДодатиВідбір(treeView, new Where("uid", Comparison.NOT, OpenFolder.UGuid));
            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) Контрагенти_Папки_Select.QuerySelect.Where = (List<Where>)where;

            Контрагенти_Папки_Select.QuerySelect.Order.Add(
               Довідники.Контрагенти_Папки_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Контрагенти_Папки_Select.Select();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            
            Dictionary<string, TreeIter> nodeDictionary = new();
            TreeIter rootIter = Store.AppendValues(new Контрагенти_Папки_Записи(){ ID = Guid.Empty.ToString() }.ToArray());
            TreePath rootPath = Store.GetPath(rootIter);
            

            while (Контрагенти_Папки_Select.MoveNext())
            {
                Довідники.Контрагенти_Папки_Pointer? cur = Контрагенти_Папки_Select.Current;
                
                string Parent = Контрагенти_Папки_Select.Parent?.ToString() ?? Guid.Empty.ToString();
                int Level = Контрагенти_Папки_Select.Level;
                
                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Контрагенти_Папки_Записи Record = new Контрагенти_Папки_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[Контрагенти_Папки_Const.Назва].ToString() ?? "",
                            Код = Fields[Контрагенти_Папки_Const.Код].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Level == 1 ? Store.AppendValues(rootIter, Record.ToArray()) : Store.AppendValues(nodeDictionary[Parent], Record.ToArray());
                        nodeDictionary.Add(Record.ID, CurrentIter);
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            treeView.ExpandToPath(rootPath);
            treeView.SetCursor(rootPath, treeView.Columns[0], false);
            if (SelectPath != null) treeView.ExpandToPath(SelectPath);
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
            treeView.ActivateRow(SelectPath != null ? SelectPath: rootPath, treeView.Columns[0]);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Склади_Папки"
      
    public class Склади_Папки_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "Дерево";
        
        string Код = "";
        

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                /*Назва*/ Назва,
                /*Код*/ Код,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Код*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Код*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.Склади_Папки_SelectHierarchical Склади_Папки_Select = new Довідники.Склади_Папки_SelectHierarchical();
            Склади_Папки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.Склади_Папки_Const.Назва,
                /*Код*/ Довідники.Склади_Папки_Const.Код,
                
            ]);

            
            if (OpenFolder != null) 
              ДодатиВідбір(treeView, new Where("uid", Comparison.NOT, OpenFolder.UGuid));
            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) Склади_Папки_Select.QuerySelect.Where = (List<Where>)where;

            Склади_Папки_Select.QuerySelect.Order.Add(
               Довідники.Склади_Папки_Const.Назва, SelectOrder.ASC);
            

            /* SELECT */
            await Склади_Папки_Select.Select();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            
            Dictionary<string, TreeIter> nodeDictionary = new();
            TreeIter rootIter = Store.AppendValues(new Склади_Папки_Записи(){ ID = Guid.Empty.ToString() }.ToArray());
            TreePath rootPath = Store.GetPath(rootIter);
            

            while (Склади_Папки_Select.MoveNext())
            {
                Довідники.Склади_Папки_Pointer? cur = Склади_Папки_Select.Current;
                
                string Parent = Склади_Папки_Select.Parent?.ToString() ?? Guid.Empty.ToString();
                int Level = Склади_Папки_Select.Level;
                
                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    Склади_Папки_Записи Record = new Склади_Папки_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[Склади_Папки_Const.Назва].ToString() ?? "",
                            Код = Fields[Склади_Папки_Const.Код].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Level == 1 ? Store.AppendValues(rootIter, Record.ToArray()) : Store.AppendValues(nodeDictionary[Parent], Record.ToArray());
                        nodeDictionary.Add(Record.ID, CurrentIter);
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            treeView.ExpandToPath(rootPath);
            treeView.SetCursor(rootPath, treeView.Columns[0], false);
            if (SelectPath != null) treeView.ExpandToPath(SelectPath);
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
            treeView.ActivateRow(SelectPath != null ? SelectPath: rootPath, treeView.Columns[0]);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Каси"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Валюта" => Каси_Const.Валюта,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Каси_Select.QuerySelect.Where = (List<Where>)where;

            Каси_Select.QuerySelect.Order.Add(
               Довідники.Каси_Const.Назва, SelectOrder.ASC);
            Довідники.Валюти_Pointer.GetJoin(Каси_Select.QuerySelect, Довідники.Каси_Const.Валюта,
                Каси_Select.QuerySelect.Table, "join_tab_1", "Валюта");
            
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
                        Код = Fields[Каси_Const.Код].ToString() ?? "",
                            Назва = Fields[Каси_Const.Назва].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Залишок = Fields["Залишок"].ToString() ?? "",
                        
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    public class Каси_ЗаписиШвидкийВибір : ТабличнийСписок
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Каси_Select.QuerySelect.Where = (List<Where>)where;

            Каси_Select.QuerySelect.Order.Add(
               Довідники.Каси_Const.Назва, SelectOrder.ASC);
            Довідники.Валюти_Pointer.GetJoin(Каси_Select.QuerySelect, Довідники.Каси_Const.Валюта,
                Каси_Select.QuerySelect.Table, "join_tab_1", "Валюта");
            

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
                        Код = Fields[Каси_Const.Код].ToString() ?? "",
                            Назва = Fields[Каси_Const.Назва].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "БанківськіРахункиОрганізацій"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Валюта" => БанківськіРахункиОрганізацій_Const.Валюта,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) БанківськіРахункиОрганізацій_Select.QuerySelect.Where = (List<Where>)where;

            БанківськіРахункиОрганізацій_Select.QuerySelect.Order.Add(
               Довідники.БанківськіРахункиОрганізацій_Const.Назва, SelectOrder.ASC);
            Довідники.Валюти_Pointer.GetJoin(БанківськіРахункиОрганізацій_Select.QuerySelect, Довідники.БанківськіРахункиОрганізацій_Const.Валюта,
                БанківськіРахункиОрганізацій_Select.QuerySelect.Table, "join_tab_1", "Валюта");
            

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
                        Код = Fields[БанківськіРахункиОрганізацій_Const.Код].ToString() ?? "",
                            Назва = Fields[БанківськіРахункиОрганізацій_Const.Назва].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ДоговориКонтрагентів"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* ТипДоговору, enum */
                      Switch sw = new();
                      ComboBoxText ТипДоговору = new();
                          foreach (var item in ПсевдонімиПерелічення.ТипДоговорів_List()) ТипДоговору.Append(item.Value.ToString(), item.Name);
                          ТипДоговору.Active = 0;
                          
                      widgets.Add(new("ТипДоговору", ТипДоговору, sw));
                      ДодатиЕлементВФільтр(listBox, "Тип:", ТипДоговору, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Контрагент" => ДоговориКонтрагентів_Const.Контрагент,
                                  "ТипДоговору" => ДоговориКонтрагентів_Const.ТипДоговору,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "ТипДоговору" => (int)Enum.Parse<ТипДоговорів>(((ComboBoxText)widget.Item2).ActiveId),
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ДоговориКонтрагентів_Select.QuerySelect.Where = (List<Where>)where;

            ДоговориКонтрагентів_Select.QuerySelect.Order.Add(
               Довідники.ДоговориКонтрагентів_Const.Назва, SelectOrder.ASC);
            Довідники.Контрагенти_Pointer.GetJoin(ДоговориКонтрагентів_Select.QuerySelect, Довідники.ДоговориКонтрагентів_Const.Контрагент,
                ДоговориКонтрагентів_Select.QuerySelect.Table, "join_tab_1", "Контрагент");
            

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
                        Код = Fields[ДоговориКонтрагентів_Const.Код].ToString() ?? "",
                            Назва = Fields[ДоговориКонтрагентів_Const.Назва].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            ТипДоговору = Перелічення.ПсевдонімиПерелічення.ТипДоговорів_Alias((
                              (Перелічення.ТипДоговорів)(Fields[ДоговориКонтрагентів_Const.ТипДоговору] != DBNull.Value ? Fields[ДоговориКонтрагентів_Const.ТипДоговору] : 0)) ),
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ДоговориКонтрагентів_Select.QuerySelect.Where = (List<Where>)where;

            ДоговориКонтрагентів_Select.QuerySelect.Order.Add(
               Довідники.ДоговориКонтрагентів_Const.Назва, SelectOrder.ASC);
            Довідники.Контрагенти_Pointer.GetJoin(ДоговориКонтрагентів_Select.QuerySelect, Довідники.ДоговориКонтрагентів_Const.Контрагент,
                ДоговориКонтрагентів_Select.QuerySelect.Table, "join_tab_1", "Контрагент");
            

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
                        Назва = Fields[ДоговориКонтрагентів_Const.Назва].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            ТипДоговору = Перелічення.ПсевдонімиПерелічення.ТипДоговорів_Alias((
                              (Перелічення.ТипДоговорів)(Fields[ДоговориКонтрагентів_Const.ТипДоговору] != DBNull.Value ? Fields[ДоговориКонтрагентів_Const.ТипДоговору] : 0)) ),
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "БанківськіРахункиКонтрагентів"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Валюта" => БанківськіРахункиКонтрагентів_Const.Валюта,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) БанківськіРахункиКонтрагентів_Select.QuerySelect.Where = (List<Where>)where;

            БанківськіРахункиКонтрагентів_Select.QuerySelect.Order.Add(
               Довідники.БанківськіРахункиКонтрагентів_Const.Назва, SelectOrder.ASC);
            Довідники.Валюти_Pointer.GetJoin(БанківськіРахункиКонтрагентів_Select.QuerySelect, Довідники.БанківськіРахункиКонтрагентів_Const.Валюта,
                БанківськіРахункиКонтрагентів_Select.QuerySelect.Table, "join_tab_1", "Валюта");
            

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
                        Код = Fields[БанківськіРахункиКонтрагентів_Const.Код].ToString() ?? "",
                            Назва = Fields[БанківськіРахункиКонтрагентів_Const.Назва].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СтаттяРухуКоштів"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* ВидРухуКоштів, enum */
                      Switch sw = new();
                      ComboBoxText ВидРухуКоштів = new();
                          foreach (var item in ПсевдонімиПерелічення.ВидиРухуКоштів_List()) ВидРухуКоштів.Append(item.Value.ToString(), item.Name);
                          ВидРухуКоштів.Active = 0;
                          
                      widgets.Add(new("ВидРухуКоштів", ВидРухуКоштів, sw));
                      ДодатиЕлементВФільтр(listBox, "ВидРухуКоштів:", ВидРухуКоштів, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "ВидРухуКоштів" => СтаттяРухуКоштів_Const.ВидРухуКоштів,
                                   _ => null };
                                  object? value = widget.Item1 switch { "ВидРухуКоштів" => (int)Enum.Parse<ВидиРухуКоштів>(((ComboBoxText)widget.Item2).ActiveId),
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) СтаттяРухуКоштів_Select.QuerySelect.Where = (List<Where>)where;

            СтаттяРухуКоштів_Select.QuerySelect.Order.Add(
               Довідники.СтаттяРухуКоштів_Const.Назва, SelectOrder.ASC);
            

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
                            ВидРухуКоштів = Перелічення.ПсевдонімиПерелічення.ВидиРухуКоштів_Alias((
                              (Перелічення.ВидиРухуКоштів)(Fields[СтаттяРухуКоштів_Const.ВидРухуКоштів] != DBNull.Value ? Fields[СтаттяРухуКоштів_Const.ВидРухуКоштів] : 0)) ),
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СеріїНоменклатури"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СеріїНоменклатури_Select СеріїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            СеріїНоменклатури_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Номер*/ Довідники.СеріїНоменклатури_Const.Номер,
                
            ]);

            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) СеріїНоменклатури_Select.QuerySelect.Where = (List<Where>)where;

            СеріїНоменклатури_Select.QuerySelect.Order.Add(
               Довідники.СеріїНоменклатури_Const.Номер, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ПартіяТоварівКомпозит"
      
    public class ПартіяТоварівКомпозит_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "";
        
        string Дата = "";
        
        string ТипДокументу = "";
        
        string ПоступленняТоварівТаПослуг = "";
        
        string ВведенняЗалишків = "";
        
        string Залишки = "";

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
                /*Залишки*/ Залишки,
                
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
                /*Залишки*/ typeof(string), 
                
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
            treeView.AppendColumn(new TreeViewColumn("Залишки", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Залишки*/
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* ТипДокументу, enum */
                      Switch sw = new();
                      ComboBoxText ТипДокументу = new();
                          foreach (var item in ПсевдонімиПерелічення.ТипДокументуПартіяТоварівКомпозит_List()) ТипДокументу.Append(item.Value.ToString(), item.Name);
                          ТипДокументу.Active = 0;
                          
                      widgets.Add(new("ТипДокументу", ТипДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "ТипДокументу:", ТипДокументу, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "ТипДокументу" => ПартіяТоварівКомпозит_Const.ТипДокументу,
                                   _ => null };
                                  object? value = widget.Item1 switch { "ТипДокументу" => (int)Enum.Parse<ТипДокументуПартіяТоварівКомпозит>(((ComboBoxText)widget.Item2).ActiveId),
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ПартіяТоварівКомпозит_Select.QuerySelect.Where = (List<Where>)where;

            ПартіяТоварівКомпозит_Select.QuerySelect.Order.Add(
               Довідники.ПартіяТоварівКомпозит_Const.Дата, SelectOrder.ASC);
            Документи.ПоступленняТоварівТаПослуг_Pointer.GetJoin(ПартіяТоварівКомпозит_Select.QuerySelect, Довідники.ПартіяТоварівКомпозит_Const.ПоступленняТоварівТаПослуг,
                ПартіяТоварівКомпозит_Select.QuerySelect.Table, "join_tab_1", "ПоступленняТоварівТаПослуг");
            Документи.ВведенняЗалишків_Pointer.GetJoin(ПартіяТоварівКомпозит_Select.QuerySelect, Довідники.ПартіяТоварівКомпозит_Const.ВведенняЗалишків,
                ПартіяТоварівКомпозит_Select.QuerySelect.Table, "join_tab_2", "ВведенняЗалишків");
            
                /* Additional Field */
                ПартіяТоварівКомпозит_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue<string>(@$"(WITH Залишки AS ( SELECT ПартіїТоварів.{РегістриНакопичення.ПартіїТоварів_Підсумки_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит, SUM(ПартіїТоварів.{РегістриНакопичення.ПартіїТоварів_Підсумки_TablePart.Кількість} ) AS Кількість FROM {РегістриНакопичення.ПартіїТоварів_Підсумки_TablePart.TABLE} AS ПартіїТоварів WHERE ПартіїТоварів.{РегістриНакопичення.ПартіїТоварів_Підсумки_TablePart.ПартіяТоварівКомпозит} = {Довідники.ПартіяТоварівКомпозит_Const.TABLE}.uid GROUP BY ПартіяТоварівКомпозит ) SELECT ROUND(Кількість, 1) FROM Залишки)", "Залишки"));
                /*
                WITH Залишки AS 
(
    SELECT
        ПартіїТоварів.{РегістриНакопичення.ПартіїТоварів_Підсумки_TablePart.ПартіяТоварівКомпозит} AS ПартіяТоварівКомпозит,
        SUM(ПартіїТоварів.{РегістриНакопичення.ПартіїТоварів_Підсумки_TablePart.Кількість} ) AS Кількість
    FROM
        {РегістриНакопичення.ПартіїТоварів_Підсумки_TablePart.TABLE} AS ПартіїТоварів
    WHERE
        ПартіїТоварів.{РегістриНакопичення.ПартіїТоварів_Підсумки_TablePart.ПартіяТоварівКомпозит} = 
	    {Довідники.ПартіяТоварівКомпозит_Const.TABLE}.uid
    GROUP BY ПартіяТоварівКомпозит
)
SELECT 
    ROUND(Кількість, 1)
FROM 
    Залишки
                */
            

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
                        Назва = Fields[ПартіяТоварівКомпозит_Const.Назва].ToString() ?? "",
                            Дата = Fields[ПартіяТоварівКомпозит_Const.Дата].ToString() ?? "",
                            ТипДокументу = Перелічення.ПсевдонімиПерелічення.ТипДокументуПартіяТоварівКомпозит_Alias((
                              (Перелічення.ТипДокументуПартіяТоварівКомпозит)(Fields[ПартіяТоварівКомпозит_Const.ТипДокументу] != DBNull.Value ? Fields[ПартіяТоварівКомпозит_Const.ТипДокументу] : 0)) ),
                            ПоступленняТоварівТаПослуг = Fields["ПоступленняТоварівТаПослуг"].ToString() ?? "",
                            ВведенняЗалишків = Fields["ВведенняЗалишків"].ToString() ?? "",
                            Залишки = Fields["Залишки"].ToString() ?? "",
                        
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ПартіяТоварівКомпозит_Select.QuerySelect.Where = (List<Where>)where;

            ПартіяТоварівКомпозит_Select.QuerySelect.Order.Add(
               Довідники.ПартіяТоварівКомпозит_Const.Дата, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ВидиЗапасів"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ВидиЗапасів_Select.QuerySelect.Where = (List<Where>)where;

            ВидиЗапасів_Select.QuerySelect.Order.Add(
               Довідники.ВидиЗапасів_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Банки"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Банки_Select.QuerySelect.Where = (List<Where>)where;

            Банки_Select.QuerySelect.Order.Add(
               Довідники.Банки_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Банки_Select.QuerySelect.Where = (List<Where>)where;

            Банки_Select.QuerySelect.Order.Add(
               Довідники.Банки_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СкладськіПриміщення"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* НалаштуванняАдресногоЗберігання, enum */
                      Switch sw = new();
                      ComboBoxText НалаштуванняАдресногоЗберігання = new();
                          foreach (var item in ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_List()) НалаштуванняАдресногоЗберігання.Append(item.Value.ToString(), item.Name);
                          НалаштуванняАдресногоЗберігання.Active = 0;
                          
                      widgets.Add(new("НалаштуванняАдресногоЗберігання", НалаштуванняАдресногоЗберігання, sw));
                      ДодатиЕлементВФільтр(listBox, "Налаштування:", НалаштуванняАдресногоЗберігання, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Склад" => СкладськіПриміщення_Const.Склад,
                                  "НалаштуванняАдресногоЗберігання" => СкладськіПриміщення_Const.НалаштуванняАдресногоЗберігання,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "НалаштуванняАдресногоЗберігання" => (int)Enum.Parse<НалаштуванняАдресногоЗберігання>(((ComboBoxText)widget.Item2).ActiveId),
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) СкладськіПриміщення_Select.QuerySelect.Where = (List<Where>)where;

            СкладськіПриміщення_Select.QuerySelect.Order.Add(
               Довідники.СкладськіПриміщення_Const.Назва, SelectOrder.ASC);
            Довідники.Склади_Pointer.GetJoin(СкладськіПриміщення_Select.QuerySelect, Довідники.СкладськіПриміщення_Const.Склад,
                СкладськіПриміщення_Select.QuerySelect.Table, "join_tab_1", "Склад");
            

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
                        Назва = Fields[СкладськіПриміщення_Const.Назва].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            НалаштуванняАдресногоЗберігання = Перелічення.ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_Alias((
                              (Перелічення.НалаштуванняАдресногоЗберігання)(Fields[СкладськіПриміщення_Const.НалаштуванняАдресногоЗберігання] != DBNull.Value ? Fields[СкладськіПриміщення_Const.НалаштуванняАдресногоЗберігання] : 0)) ),
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СкладськіКомірки"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Приміщення, pointer */
                      Switch sw = new();
                      СкладськіПриміщення_PointerControl Приміщення = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Приміщення", Приміщення, sw));
                      ДодатиЕлементВФільтр(listBox, "Приміщення:", Приміщення, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Приміщення" => СкладськіКомірки_Const.Приміщення,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Приміщення" => ((СкладськіПриміщення_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) СкладськіКомірки_Select.QuerySelect.Where = (List<Where>)where;

            СкладськіКомірки_Select.QuerySelect.Order.Add(
               Довідники.СкладськіКомірки_Const.Назва, SelectOrder.ASC);
            Довідники.СкладськіПриміщення_Pointer.GetJoin(СкладськіКомірки_Select.QuerySelect, Довідники.СкладськіКомірки_Const.Приміщення,
                СкладськіКомірки_Select.QuerySelect.Table, "join_tab_1", "Приміщення");
            Довідники.ТипорозміриКомірок_Pointer.GetJoin(СкладськіКомірки_Select.QuerySelect, Довідники.СкладськіКомірки_Const.Типорозмір,
                СкладськіКомірки_Select.QuerySelect.Table, "join_tab_2", "Типорозмір");
            Довідники.СкладськіКомірки_Папки_Pointer.GetJoin(СкладськіКомірки_Select.QuerySelect, Довідники.СкладськіКомірки_Const.Папка,
                СкладськіКомірки_Select.QuerySelect.Table, "join_tab_3", "Папка");
            

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
                        Назва = Fields[СкладськіКомірки_Const.Назва].ToString() ?? "",
                            Приміщення = Fields["Приміщення"].ToString() ?? "",
                            Лінія = Fields[СкладськіКомірки_Const.Лінія].ToString() ?? "",
                            Позиція = Fields[СкладськіКомірки_Const.Позиція].ToString() ?? "",
                            Стелаж = Fields[СкладськіКомірки_Const.Стелаж].ToString() ?? "",
                            Ярус = Fields[СкладськіКомірки_Const.Ярус].ToString() ?? "",
                            ТипСкладськоїКомірки = Перелічення.ПсевдонімиПерелічення.ТипиСкладськихКомірок_Alias((
                              (Перелічення.ТипиСкладськихКомірок)(Fields[СкладськіКомірки_Const.ТипСкладськоїКомірки] != DBNull.Value ? Fields[СкладськіКомірки_Const.ТипСкладськоїКомірки] : 0)) ),
                            Типорозмір = Fields["Типорозмір"].ToString() ?? "",
                            Папка = Fields["Папка"].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіКомірки_Select СкладськіКомірки_Select = new Довідники.СкладськіКомірки_Select();
            СкладськіКомірки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.СкладськіКомірки_Const.Назва,
                
            ]);

            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) СкладськіКомірки_Select.QuerySelect.Where = (List<Where>)where;

            СкладськіКомірки_Select.QuerySelect.Order.Add(
               Довідники.СкладськіКомірки_Const.Назва, SelectOrder.ASC);
            Довідники.СкладськіПриміщення_Pointer.GetJoin(СкладськіКомірки_Select.QuerySelect, Довідники.СкладськіКомірки_Const.Приміщення,
                СкладськіКомірки_Select.QuerySelect.Table, "join_tab_1", "Приміщення");
            

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
                        Назва = Fields[СкладськіКомірки_Const.Назва].ToString() ?? "",
                            Приміщення = Fields["Приміщення"].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ОбластьЗберігання"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ОбластьЗберігання_Select ОбластьЗберігання_Select = new Довідники.ОбластьЗберігання_Select();
            ОбластьЗберігання_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.ОбластьЗберігання_Const.Назва,
                
            ]);

            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ОбластьЗберігання_Select.QuerySelect.Where = (List<Where>)where;

            ОбластьЗберігання_Select.QuerySelect.Order.Add(
               Довідники.ОбластьЗберігання_Const.Назва, SelectOrder.ASC);
            Довідники.СкладськіПриміщення_Pointer.GetJoin(ОбластьЗберігання_Select.QuerySelect, Довідники.ОбластьЗберігання_Const.Приміщення,
                ОбластьЗберігання_Select.QuerySelect.Table, "join_tab_1", "Приміщення");
            

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
                        Назва = Fields[ОбластьЗберігання_Const.Назва].ToString() ?? "",
                            Приміщення = Fields["Приміщення"].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ТипорозміриКомірок"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) ТипорозміриКомірок_Select.QuerySelect.Where = (List<Where>)where;

            ТипорозміриКомірок_Select.QuerySelect.Order.Add(
               Довідники.ТипорозміриКомірок_Const.Назва, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "СкладськіКомірки_Папки"
      
    public class СкладськіКомірки_Папки_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Назва = "Дерево";
        
        string Код = "";
        
        string Власник = "";
        

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                /*Назва*/ Назва,
                /*Код*/ Код,
                /*Власник*/ Власник,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Назва*/ typeof(string),  
                /*Код*/ typeof(string),  
                /*Власник*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Власник", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Власник*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.СкладськіКомірки_Папки_SelectHierarchical СкладськіКомірки_Папки_Select = new Довідники.СкладськіКомірки_Папки_SelectHierarchical();
            СкладськіКомірки_Папки_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Назва*/ Довідники.СкладськіКомірки_Папки_Const.Назва,
                /*Код*/ Довідники.СкладськіКомірки_Папки_Const.Код,
                
            ]);

            
            if (OpenFolder != null) 
              ДодатиВідбір(treeView, new Where("uid", Comparison.NOT, OpenFolder.UGuid));
            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) СкладськіКомірки_Папки_Select.QuerySelect.Where = (List<Where>)where;

            СкладськіКомірки_Папки_Select.QuerySelect.Order.Add(
               Довідники.СкладськіКомірки_Папки_Const.Назва, SelectOrder.ASC);
            Довідники.СкладськіПриміщення_Pointer.GetJoin(СкладськіКомірки_Папки_Select.QuerySelect, Довідники.СкладськіКомірки_Папки_Const.Власник,
                СкладськіКомірки_Папки_Select.QuerySelect.Table, "join_tab_1", "Власник");
            

            /* SELECT */
            await СкладськіКомірки_Папки_Select.Select();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            
            Dictionary<string, TreeIter> nodeDictionary = new();
            TreeIter rootIter = Store.AppendValues(new СкладськіКомірки_Папки_Записи(){ ID = Guid.Empty.ToString() }.ToArray());
            TreePath rootPath = Store.GetPath(rootIter);
            

            while (СкладськіКомірки_Папки_Select.MoveNext())
            {
                Довідники.СкладськіКомірки_Папки_Pointer? cur = СкладськіКомірки_Папки_Select.Current;
                
                string Parent = СкладськіКомірки_Папки_Select.Parent?.ToString() ?? Guid.Empty.ToString();
                int Level = СкладськіКомірки_Папки_Select.Level;
                
                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    СкладськіКомірки_Папки_Записи Record = new СкладськіКомірки_Папки_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[СкладськіКомірки_Папки_Const.Назва].ToString() ?? "",
                            Код = Fields[СкладськіКомірки_Папки_Const.Код].ToString() ?? "",
                            Власник = Fields["Власник"].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Level == 1 ? Store.AppendValues(rootIter, Record.ToArray()) : Store.AppendValues(nodeDictionary[Parent], Record.ToArray());
                        nodeDictionary.Add(Record.ID, CurrentIter);
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            treeView.ExpandToPath(rootPath);
            treeView.SetCursor(rootPath, treeView.Columns[0], false);
            if (SelectPath != null) treeView.ExpandToPath(SelectPath);
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
            treeView.ActivateRow(SelectPath != null ? SelectPath: rootPath, treeView.Columns[0]);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "Блокнот"
      
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
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
            var where = treeView.Data["Where"];
            if (where != null) Блокнот_Select.QuerySelect.Where = (List<Where>)where;

            Блокнот_Select.QuerySelect.Order.Add(
               Довідники.Блокнот_Const.ДатаЗапису, SelectOrder.ASC);
            

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
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
        }
    }
	    
    #endregion
    
    #region DIRECTORY "ЗбереженіЗвіти"
      
    public class ЗбереженіЗвіти_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        
        string Код = "";
        
        string Додано = "";
        
        string Назва = "";
        
        string Користувач = "";
        

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Додано*/ Додано,
                /*Назва*/ Назва,
                /*Користувач*/ Користувач,
                
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                /*Код*/ typeof(string),  
                /*Додано*/ typeof(string),  
                /*Назва*/ typeof(string),  
                /*Користувач*/ typeof(string),  
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 } ); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Додано", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Додано*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Користувач", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Користувач*/
            

            /* Додаткові поля */
            

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? OpenFolder = null)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.ЗбереженіЗвіти_Select ЗбереженіЗвіти_Select = new Довідники.ЗбереженіЗвіти_Select();
            ЗбереженіЗвіти_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Код*/ Довідники.ЗбереженіЗвіти_Const.Код,
                /*Додано*/ Довідники.ЗбереженіЗвіти_Const.Додано,
                /*Назва*/ Довідники.ЗбереженіЗвіти_Const.Назва,
                
            ]);

            

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ЗбереженіЗвіти_Select.QuerySelect.Where = (List<Where>)where;

            ЗбереженіЗвіти_Select.QuerySelect.Order.Add(
               Довідники.ЗбереженіЗвіти_Const.Код, SelectOrder.DESC);
            Довідники.Користувачі_Pointer.GetJoin(ЗбереженіЗвіти_Select.QuerySelect, Довідники.ЗбереженіЗвіти_Const.Користувач,
                ЗбереженіЗвіти_Select.QuerySelect.Table, "join_tab_1", "Користувач");
            

            /* SELECT */
            await ЗбереженіЗвіти_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            

            while (ЗбереженіЗвіти_Select.MoveNext())
            {
                Довідники.ЗбереженіЗвіти_Pointer? cur = ЗбереженіЗвіти_Select.Current;
                
                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ЗбереженіЗвіти_Записи Record = new ЗбереженіЗвіти_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Код = Fields[ЗбереженіЗвіти_Const.Код].ToString() ?? "",
                            Додано = Fields[ЗбереженіЗвіти_Const.Додано].ToString() ?? "",
                            Назва = Fields[ЗбереженіЗвіти_Const.Назва].ToString() ?? "",
                            Користувач = Fields["Користувач"].ToString() ?? "",
                            
                    };
                    
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ЗамовленняПостачальнику_Const.Організація,
                                  "Контрагент" => ЗамовленняПостачальнику_Const.Контрагент,
                                  "Склад" => ЗамовленняПостачальнику_Const.Склад,
                                  "Валюта" => ЗамовленняПостачальнику_Const.Валюта,
                                  "СумаДокументу" => ЗамовленняПостачальнику_Const.СумаДокументу,
                                  "Автор" => ЗамовленняПостачальнику_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ЗамовленняПостачальнику_Select.QuerySelect.Order.Add(
               Документи.ЗамовленняПостачальнику_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ЗамовленняПостачальнику_Select.QuerySelect, Документи.ЗамовленняПостачальнику_Const.Організація,
                ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Контрагенти_Pointer.GetJoin(ЗамовленняПостачальнику_Select.QuerySelect, Документи.ЗамовленняПостачальнику_Const.Контрагент,
                ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_2", "Контрагент");
            Довідники.Склади_Pointer.GetJoin(ЗамовленняПостачальнику_Select.QuerySelect, Документи.ЗамовленняПостачальнику_Const.Склад,
                ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_3", "Склад");
            Довідники.Валюти_Pointer.GetJoin(ЗамовленняПостачальнику_Select.QuerySelect, Документи.ЗамовленняПостачальнику_Const.Валюта,
                ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_4", "Валюта");
            Довідники.Користувачі_Pointer.GetJoin(ЗамовленняПостачальнику_Select.QuerySelect, Документи.ЗамовленняПостачальнику_Const.Автор,
                ЗамовленняПостачальнику_Select.QuerySelect.Table, "join_tab_5", "Автор");
            

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
                        Назва = Fields[ЗамовленняПостачальнику_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ЗамовленняПостачальнику_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ЗамовленняПостачальнику_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            СумаДокументу = Fields[ЗамовленняПостачальнику_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ЗамовленняПостачальнику_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ПоступленняТоварівТаПослуг_Const.Організація,
                                  "Склад" => ПоступленняТоварівТаПослуг_Const.Склад,
                                  "Контрагент" => ПоступленняТоварівТаПослуг_Const.Контрагент,
                                  "Валюта" => ПоступленняТоварівТаПослуг_Const.Валюта,
                                  "Каса" => ПоступленняТоварівТаПослуг_Const.Каса,
                                  "СумаДокументу" => ПоступленняТоварівТаПослуг_Const.СумаДокументу,
                                  "Автор" => ПоступленняТоварівТаПослуг_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ПоступленняТоварівТаПослуг_Select.QuerySelect.Order.Add(
               Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ПоступленняТоварівТаПослуг_Select.QuerySelect, Документи.ПоступленняТоварівТаПослуг_Const.Організація,
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Склади_Pointer.GetJoin(ПоступленняТоварівТаПослуг_Select.QuerySelect, Документи.ПоступленняТоварівТаПослуг_Const.Склад,
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_2", "Склад");
            Довідники.Контрагенти_Pointer.GetJoin(ПоступленняТоварівТаПослуг_Select.QuerySelect, Документи.ПоступленняТоварівТаПослуг_Const.Контрагент,
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_3", "Контрагент");
            Довідники.Валюти_Pointer.GetJoin(ПоступленняТоварівТаПослуг_Select.QuerySelect, Документи.ПоступленняТоварівТаПослуг_Const.Валюта,
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_4", "Валюта");
            Довідники.Каси_Pointer.GetJoin(ПоступленняТоварівТаПослуг_Select.QuerySelect, Документи.ПоступленняТоварівТаПослуг_Const.Каса,
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_5", "Каса");
            Довідники.Користувачі_Pointer.GetJoin(ПоступленняТоварівТаПослуг_Select.QuerySelect, Документи.ПоступленняТоварівТаПослуг_Const.Автор,
                ПоступленняТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_6", "Автор");
            

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
                        Назва = Fields[ПоступленняТоварівТаПослуг_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ПоступленняТоварівТаПослуг_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ПоступленняТоварівТаПослуг_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            СумаДокументу = Fields[ПоступленняТоварівТаПослуг_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ПоступленняТоварівТаПослуг_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ЗамовленняКлієнта_Const.Організація,
                                  "Контрагент" => ЗамовленняКлієнта_Const.Контрагент,
                                  "Валюта" => ЗамовленняКлієнта_Const.Валюта,
                                  "Каса" => ЗамовленняКлієнта_Const.Каса,
                                  "Склад" => ЗамовленняКлієнта_Const.Склад,
                                  "СумаДокументу" => ЗамовленняКлієнта_Const.СумаДокументу,
                                  "Автор" => ЗамовленняКлієнта_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ЗамовленняКлієнта_Select.QuerySelect.Order.Add(
               Документи.ЗамовленняКлієнта_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ЗамовленняКлієнта_Select.QuerySelect, Документи.ЗамовленняКлієнта_Const.Організація,
                ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Контрагенти_Pointer.GetJoin(ЗамовленняКлієнта_Select.QuerySelect, Документи.ЗамовленняКлієнта_Const.Контрагент,
                ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_2", "Контрагент");
            Довідники.Валюти_Pointer.GetJoin(ЗамовленняКлієнта_Select.QuerySelect, Документи.ЗамовленняКлієнта_Const.Валюта,
                ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_3", "Валюта");
            Довідники.Каси_Pointer.GetJoin(ЗамовленняКлієнта_Select.QuerySelect, Документи.ЗамовленняКлієнта_Const.Каса,
                ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_4", "Каса");
            Довідники.Склади_Pointer.GetJoin(ЗамовленняКлієнта_Select.QuerySelect, Документи.ЗамовленняКлієнта_Const.Склад,
                ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_5", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(ЗамовленняКлієнта_Select.QuerySelect, Документи.ЗамовленняКлієнта_Const.Автор,
                ЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_6", "Автор");
            

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
                        Назва = Fields[ЗамовленняКлієнта_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ЗамовленняКлієнта_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ЗамовленняКлієнта_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            СумаДокументу = Fields[ЗамовленняКлієнта_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ЗамовленняКлієнта_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => РеалізаціяТоварівТаПослуг_Const.Організація,
                                  "Контрагент" => РеалізаціяТоварівТаПослуг_Const.Контрагент,
                                  "Валюта" => РеалізаціяТоварівТаПослуг_Const.Валюта,
                                  "Каса" => РеалізаціяТоварівТаПослуг_Const.Каса,
                                  "Склад" => РеалізаціяТоварівТаПослуг_Const.Склад,
                                  "СумаДокументу" => РеалізаціяТоварівТаПослуг_Const.СумаДокументу,
                                  "Автор" => РеалізаціяТоварівТаПослуг_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            РеалізаціяТоварівТаПослуг_Select.QuerySelect.Order.Add(
               Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(РеалізаціяТоварівТаПослуг_Select.QuerySelect, Документи.РеалізаціяТоварівТаПослуг_Const.Організація,
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Контрагенти_Pointer.GetJoin(РеалізаціяТоварівТаПослуг_Select.QuerySelect, Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент,
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_2", "Контрагент");
            Довідники.Валюти_Pointer.GetJoin(РеалізаціяТоварівТаПослуг_Select.QuerySelect, Документи.РеалізаціяТоварівТаПослуг_Const.Валюта,
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_3", "Валюта");
            Довідники.Каси_Pointer.GetJoin(РеалізаціяТоварівТаПослуг_Select.QuerySelect, Документи.РеалізаціяТоварівТаПослуг_Const.Каса,
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_4", "Каса");
            Довідники.Склади_Pointer.GetJoin(РеалізаціяТоварівТаПослуг_Select.QuerySelect, Документи.РеалізаціяТоварівТаПослуг_Const.Склад,
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_5", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(РеалізаціяТоварівТаПослуг_Select.QuerySelect, Документи.РеалізаціяТоварівТаПослуг_Const.Автор,
                РеалізаціяТоварівТаПослуг_Select.QuerySelect.Table, "join_tab_6", "Автор");
            

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
                        Назва = Fields[РеалізаціяТоварівТаПослуг_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[РеалізаціяТоварівТаПослуг_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[РеалізаціяТоварівТаПослуг_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            СумаДокументу = Fields[РеалізаціяТоварівТаПослуг_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[РеалізаціяТоварівТаПослуг_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* ВидЦіни, pointer */
                      Switch sw = new();
                      ВидиЦін_PointerControl ВидЦіни = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("ВидЦіни", ВидЦіни, sw));
                      ДодатиЕлементВФільтр(listBox, "Вид ціни:", ВидЦіни, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ВстановленняЦінНоменклатури_Const.Організація,
                                  "Валюта" => ВстановленняЦінНоменклатури_Const.Валюта,
                                  "ВидЦіни" => ВстановленняЦінНоменклатури_Const.ВидЦіни,
                                  "Автор" => ВстановленняЦінНоменклатури_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "ВидЦіни" => ((ВидиЦін_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ВстановленняЦінНоменклатури_Select.QuerySelect.Order.Add(
               Документи.ВстановленняЦінНоменклатури_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ВстановленняЦінНоменклатури_Select.QuerySelect, Документи.ВстановленняЦінНоменклатури_Const.Організація,
                ВстановленняЦінНоменклатури_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Валюти_Pointer.GetJoin(ВстановленняЦінНоменклатури_Select.QuerySelect, Документи.ВстановленняЦінНоменклатури_Const.Валюта,
                ВстановленняЦінНоменклатури_Select.QuerySelect.Table, "join_tab_2", "Валюта");
            Довідники.ВидиЦін_Pointer.GetJoin(ВстановленняЦінНоменклатури_Select.QuerySelect, Документи.ВстановленняЦінНоменклатури_Const.ВидЦіни,
                ВстановленняЦінНоменклатури_Select.QuerySelect.Table, "join_tab_3", "ВидЦіни");
            Довідники.Користувачі_Pointer.GetJoin(ВстановленняЦінНоменклатури_Select.QuerySelect, Документи.ВстановленняЦінНоменклатури_Const.Автор,
                ВстановленняЦінНоменклатури_Select.QuerySelect.Table, "join_tab_4", "Автор");
            

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
                        Назва = Fields[ВстановленняЦінНоменклатури_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ВстановленняЦінНоменклатури_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ВстановленняЦінНоменклатури_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            ВидЦіни = Fields["ВидЦіни"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ВстановленняЦінНоменклатури_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ПрихіднийКасовийОрдер_Const.Організація,
                                  "Валюта" => ПрихіднийКасовийОрдер_Const.Валюта,
                                  "Каса" => ПрихіднийКасовийОрдер_Const.Каса,
                                  "Контрагент" => ПрихіднийКасовийОрдер_Const.Контрагент,
                                  "СумаДокументу" => ПрихіднийКасовийОрдер_Const.СумаДокументу,
                                  "Автор" => ПрихіднийКасовийОрдер_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ПрихіднийКасовийОрдер_Select.QuerySelect.Order.Add(
               Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ПрихіднийКасовийОрдер_Select.QuerySelect, Документи.ПрихіднийКасовийОрдер_Const.Організація,
                ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Валюти_Pointer.GetJoin(ПрихіднийКасовийОрдер_Select.QuerySelect, Документи.ПрихіднийКасовийОрдер_Const.Валюта,
                ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_2", "Валюта");
            Довідники.Каси_Pointer.GetJoin(ПрихіднийКасовийОрдер_Select.QuerySelect, Документи.ПрихіднийКасовийОрдер_Const.Каса,
                ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_3", "Каса");
            Довідники.Контрагенти_Pointer.GetJoin(ПрихіднийКасовийОрдер_Select.QuerySelect, Документи.ПрихіднийКасовийОрдер_Const.Контрагент,
                ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_4", "Контрагент");
            Довідники.Користувачі_Pointer.GetJoin(ПрихіднийКасовийОрдер_Select.QuerySelect, Документи.ПрихіднийКасовийОрдер_Const.Автор,
                ПрихіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_5", "Автор");
            

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
                        Назва = Fields[ПрихіднийКасовийОрдер_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ПрихіднийКасовийОрдер_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ПрихіднийКасовийОрдер_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            СумаДокументу = Fields[ПрихіднийКасовийОрдер_Const.СумаДокументу].ToString() ?? "",
                            ГосподарськаОперація = Перелічення.ПсевдонімиПерелічення.ГосподарськіОперації_Alias((
                               (Перелічення.ГосподарськіОперації)(Fields[ПрихіднийКасовийОрдер_Const.ГосподарськаОперація] != DBNull.Value ? Fields[ПрихіднийКасовийОрдер_Const.ГосподарськаОперація] : 0)) ),
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ПрихіднийКасовийОрдер_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => РозхіднийКасовийОрдер_Const.Організація,
                                  "Контрагент" => РозхіднийКасовийОрдер_Const.Контрагент,
                                  "Валюта" => РозхіднийКасовийОрдер_Const.Валюта,
                                  "Каса" => РозхіднийКасовийОрдер_Const.Каса,
                                  "СумаДокументу" => РозхіднийКасовийОрдер_Const.СумаДокументу,
                                  "Автор" => РозхіднийКасовийОрдер_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            РозхіднийКасовийОрдер_Select.QuerySelect.Order.Add(
               Документи.РозхіднийКасовийОрдер_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(РозхіднийКасовийОрдер_Select.QuerySelect, Документи.РозхіднийКасовийОрдер_Const.Організація,
                РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Контрагенти_Pointer.GetJoin(РозхіднийКасовийОрдер_Select.QuerySelect, Документи.РозхіднийКасовийОрдер_Const.Контрагент,
                РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_2", "Контрагент");
            Довідники.Валюти_Pointer.GetJoin(РозхіднийКасовийОрдер_Select.QuerySelect, Документи.РозхіднийКасовийОрдер_Const.Валюта,
                РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_3", "Валюта");
            Довідники.Каси_Pointer.GetJoin(РозхіднийКасовийОрдер_Select.QuerySelect, Документи.РозхіднийКасовийОрдер_Const.Каса,
                РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_4", "Каса");
            Довідники.Користувачі_Pointer.GetJoin(РозхіднийКасовийОрдер_Select.QuerySelect, Документи.РозхіднийКасовийОрдер_Const.Автор,
                РозхіднийКасовийОрдер_Select.QuerySelect.Table, "join_tab_5", "Автор");
            

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
                        Назва = Fields[РозхіднийКасовийОрдер_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[РозхіднийКасовийОрдер_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[РозхіднийКасовийОрдер_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            СумаДокументу = Fields[РозхіднийКасовийОрдер_Const.СумаДокументу].ToString() ?? "",
                            ГосподарськаОперація = Перелічення.ПсевдонімиПерелічення.ГосподарськіОперації_Alias((
                               (Перелічення.ГосподарськіОперації)(Fields[РозхіднийКасовийОрдер_Const.ГосподарськаОперація] != DBNull.Value ? Fields[РозхіднийКасовийОрдер_Const.ГосподарськаОперація] : 0)) ),
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[РозхіднийКасовийОрдер_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* СкладВідправник, pointer */
                      Switch sw = new();
                      Склади_PointerControl СкладВідправник = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("СкладВідправник", СкладВідправник, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад відправник:", СкладВідправник, sw);
                  }
                  
                  { /* СкладОтримувач, pointer */
                      Switch sw = new();
                      Склади_PointerControl СкладОтримувач = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("СкладОтримувач", СкладОтримувач, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад отримувач:", СкладОтримувач, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ПереміщенняТоварів_Const.Організація,
                                  "СкладВідправник" => ПереміщенняТоварів_Const.СкладВідправник,
                                  "СкладОтримувач" => ПереміщенняТоварів_Const.СкладОтримувач,
                                  "Автор" => ПереміщенняТоварів_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СкладВідправник" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СкладОтримувач" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ПереміщенняТоварів_Select.QuerySelect.Order.Add(
               Документи.ПереміщенняТоварів_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ПереміщенняТоварів_Select.QuerySelect, Документи.ПереміщенняТоварів_Const.Організація,
                ПереміщенняТоварів_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Склади_Pointer.GetJoin(ПереміщенняТоварів_Select.QuerySelect, Документи.ПереміщенняТоварів_Const.СкладВідправник,
                ПереміщенняТоварів_Select.QuerySelect.Table, "join_tab_2", "СкладВідправник");
            Довідники.Склади_Pointer.GetJoin(ПереміщенняТоварів_Select.QuerySelect, Документи.ПереміщенняТоварів_Const.СкладОтримувач,
                ПереміщенняТоварів_Select.QuerySelect.Table, "join_tab_3", "СкладОтримувач");
            Довідники.Користувачі_Pointer.GetJoin(ПереміщенняТоварів_Select.QuerySelect, Документи.ПереміщенняТоварів_Const.Автор,
                ПереміщенняТоварів_Select.QuerySelect.Table, "join_tab_4", "Автор");
            

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
                        Назва = Fields[ПереміщенняТоварів_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ПереміщенняТоварів_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ПереміщенняТоварів_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            СкладВідправник = Fields["СкладВідправник"].ToString() ?? "",
                            СкладОтримувач = Fields["СкладОтримувач"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ПереміщенняТоварів_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ПоверненняТоварівПостачальнику_Const.Організація,
                                  "Контрагент" => ПоверненняТоварівПостачальнику_Const.Контрагент,
                                  "Валюта" => ПоверненняТоварівПостачальнику_Const.Валюта,
                                  "Каса" => ПоверненняТоварівПостачальнику_Const.Каса,
                                  "Склад" => ПоверненняТоварівПостачальнику_Const.Склад,
                                  "СумаДокументу" => ПоверненняТоварівПостачальнику_Const.СумаДокументу,
                                  "Автор" => ПоверненняТоварівПостачальнику_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ПоверненняТоварівПостачальнику_Select.QuerySelect.Order.Add(
               Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ПоверненняТоварівПостачальнику_Select.QuerySelect, Документи.ПоверненняТоварівПостачальнику_Const.Організація,
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Контрагенти_Pointer.GetJoin(ПоверненняТоварівПостачальнику_Select.QuerySelect, Документи.ПоверненняТоварівПостачальнику_Const.Контрагент,
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_2", "Контрагент");
            Довідники.Валюти_Pointer.GetJoin(ПоверненняТоварівПостачальнику_Select.QuerySelect, Документи.ПоверненняТоварівПостачальнику_Const.Валюта,
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_3", "Валюта");
            Довідники.Каси_Pointer.GetJoin(ПоверненняТоварівПостачальнику_Select.QuerySelect, Документи.ПоверненняТоварівПостачальнику_Const.Каса,
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_4", "Каса");
            Довідники.Склади_Pointer.GetJoin(ПоверненняТоварівПостачальнику_Select.QuerySelect, Документи.ПоверненняТоварівПостачальнику_Const.Склад,
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_5", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(ПоверненняТоварівПостачальнику_Select.QuerySelect, Документи.ПоверненняТоварівПостачальнику_Const.Автор,
                ПоверненняТоварівПостачальнику_Select.QuerySelect.Table, "join_tab_6", "Автор");
            

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
                        Назва = Fields[ПоверненняТоварівПостачальнику_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ПоверненняТоварівПостачальнику_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ПоверненняТоварівПостачальнику_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            СумаДокументу = Fields[ПоверненняТоварівПостачальнику_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ПоверненняТоварівПостачальнику_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ПоверненняТоварівВідКлієнта_Const.Організація,
                                  "Валюта" => ПоверненняТоварівВідКлієнта_Const.Валюта,
                                  "Каса" => ПоверненняТоварівВідКлієнта_Const.Каса,
                                  "Контрагент" => ПоверненняТоварівВідКлієнта_Const.Контрагент,
                                  "Склад" => ПоверненняТоварівВідКлієнта_Const.Склад,
                                  "СумаДокументу" => ПоверненняТоварівВідКлієнта_Const.СумаДокументу,
                                  "Автор" => ПоверненняТоварівВідКлієнта_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ПоверненняТоварівВідКлієнта_Select.QuerySelect.Order.Add(
               Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ПоверненняТоварівВідКлієнта_Select.QuerySelect, Документи.ПоверненняТоварівВідКлієнта_Const.Організація,
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Валюти_Pointer.GetJoin(ПоверненняТоварівВідКлієнта_Select.QuerySelect, Документи.ПоверненняТоварівВідКлієнта_Const.Валюта,
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_2", "Валюта");
            Довідники.Каси_Pointer.GetJoin(ПоверненняТоварівВідКлієнта_Select.QuerySelect, Документи.ПоверненняТоварівВідКлієнта_Const.Каса,
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_3", "Каса");
            Довідники.Контрагенти_Pointer.GetJoin(ПоверненняТоварівВідКлієнта_Select.QuerySelect, Документи.ПоверненняТоварівВідКлієнта_Const.Контрагент,
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_4", "Контрагент");
            Довідники.Склади_Pointer.GetJoin(ПоверненняТоварівВідКлієнта_Select.QuerySelect, Документи.ПоверненняТоварівВідКлієнта_Const.Склад,
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_5", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(ПоверненняТоварівВідКлієнта_Select.QuerySelect, Документи.ПоверненняТоварівВідКлієнта_Const.Автор,
                ПоверненняТоварівВідКлієнта_Select.QuerySelect.Table, "join_tab_6", "Автор");
            

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
                        Назва = Fields[ПоверненняТоварівВідКлієнта_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ПоверненняТоварівВідКлієнта_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ПоверненняТоварівВідКлієнта_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            СумаДокументу = Fields[ПоверненняТоварівВідКлієнта_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ПоверненняТоварівВідКлієнта_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => АктВиконанихРобіт_Const.Організація,
                                  "Валюта" => АктВиконанихРобіт_Const.Валюта,
                                  "Каса" => АктВиконанихРобіт_Const.Каса,
                                  "Контрагент" => АктВиконанихРобіт_Const.Контрагент,
                                  "СумаДокументу" => АктВиконанихРобіт_Const.СумаДокументу,
                                  "Автор" => АктВиконанихРобіт_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            АктВиконанихРобіт_Select.QuerySelect.Order.Add(
               Документи.АктВиконанихРобіт_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(АктВиконанихРобіт_Select.QuerySelect, Документи.АктВиконанихРобіт_Const.Організація,
                АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Валюти_Pointer.GetJoin(АктВиконанихРобіт_Select.QuerySelect, Документи.АктВиконанихРобіт_Const.Валюта,
                АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_2", "Валюта");
            Довідники.Каси_Pointer.GetJoin(АктВиконанихРобіт_Select.QuerySelect, Документи.АктВиконанихРобіт_Const.Каса,
                АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_3", "Каса");
            Довідники.Контрагенти_Pointer.GetJoin(АктВиконанихРобіт_Select.QuerySelect, Документи.АктВиконанихРобіт_Const.Контрагент,
                АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_4", "Контрагент");
            Довідники.Користувачі_Pointer.GetJoin(АктВиконанихРобіт_Select.QuerySelect, Документи.АктВиконанихРобіт_Const.Автор,
                АктВиконанихРобіт_Select.QuerySelect.Table, "join_tab_5", "Автор");
            

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
                        Назва = Fields[АктВиконанихРобіт_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[АктВиконанихРобіт_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[АктВиконанихРобіт_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            СумаДокументу = Fields[АктВиконанихРобіт_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[АктВиконанихРобіт_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ВведенняЗалишків_Const.Організація,
                                  "Склад" => ВведенняЗалишків_Const.Склад,
                                  "Контрагент" => ВведенняЗалишків_Const.Контрагент,
                                  "Валюта" => ВведенняЗалишків_Const.Валюта,
                                  "Автор" => ВведенняЗалишків_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ВведенняЗалишків_Select.QuerySelect.Order.Add(
               Документи.ВведенняЗалишків_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ВведенняЗалишків_Select.QuerySelect, Документи.ВведенняЗалишків_Const.Організація,
                ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Склади_Pointer.GetJoin(ВведенняЗалишків_Select.QuerySelect, Документи.ВведенняЗалишків_Const.Склад,
                ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_2", "Склад");
            Довідники.Контрагенти_Pointer.GetJoin(ВведенняЗалишків_Select.QuerySelect, Документи.ВведенняЗалишків_Const.Контрагент,
                ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_3", "Контрагент");
            Довідники.Валюти_Pointer.GetJoin(ВведенняЗалишків_Select.QuerySelect, Документи.ВведенняЗалишків_Const.Валюта,
                ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_4", "Валюта");
            Довідники.Користувачі_Pointer.GetJoin(ВведенняЗалишків_Select.QuerySelect, Документи.ВведенняЗалишків_Const.Автор,
                ВведенняЗалишків_Select.QuerySelect.Table, "join_tab_5", "Автор");
            

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
                        Назва = Fields[ВведенняЗалишків_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ВведенняЗалишків_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ВведенняЗалишків_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ВведенняЗалишків_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => НадлишкиТоварів_Const.Організація,
                                  "Склад" => НадлишкиТоварів_Const.Склад,
                                  "Автор" => НадлишкиТоварів_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            НадлишкиТоварів_Select.QuerySelect.Order.Add(
               Документи.НадлишкиТоварів_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(НадлишкиТоварів_Select.QuerySelect, Документи.НадлишкиТоварів_Const.Організація,
                НадлишкиТоварів_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Склади_Pointer.GetJoin(НадлишкиТоварів_Select.QuerySelect, Документи.НадлишкиТоварів_Const.Склад,
                НадлишкиТоварів_Select.QuerySelect.Table, "join_tab_2", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(НадлишкиТоварів_Select.QuerySelect, Документи.НадлишкиТоварів_Const.Автор,
                НадлишкиТоварів_Select.QuerySelect.Table, "join_tab_3", "Автор");
            

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
                        Назва = Fields[НадлишкиТоварів_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[НадлишкиТоварів_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[НадлишкиТоварів_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[НадлишкиТоварів_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ПересортицяТоварів_Const.Організація,
                                  "Склад" => ПересортицяТоварів_Const.Склад,
                                  "Автор" => ПересортицяТоварів_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ПересортицяТоварів_Select.QuerySelect.Order.Add(
               Документи.ПересортицяТоварів_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ПересортицяТоварів_Select.QuerySelect, Документи.ПересортицяТоварів_Const.Організація,
                ПересортицяТоварів_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Склади_Pointer.GetJoin(ПересортицяТоварів_Select.QuerySelect, Документи.ПересортицяТоварів_Const.Склад,
                ПересортицяТоварів_Select.QuerySelect.Table, "join_tab_2", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(ПересортицяТоварів_Select.QuerySelect, Документи.ПересортицяТоварів_Const.Автор,
                ПересортицяТоварів_Select.QuerySelect.Table, "join_tab_3", "Автор");
            

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
                        Назва = Fields[ПересортицяТоварів_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ПересортицяТоварів_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ПересортицяТоварів_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ПересортицяТоварів_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* Відповідальний, pointer */
                      Switch sw = new();
                      ФізичніОсоби_PointerControl Відповідальний = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Відповідальний", Відповідальний, sw));
                      ДодатиЕлементВФільтр(listBox, "Відповідальний:", Відповідальний, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ПерерахунокТоварів_Const.Організація,
                                  "Склад" => ПерерахунокТоварів_Const.Склад,
                                  "Відповідальний" => ПерерахунокТоварів_Const.Відповідальний,
                                  "Автор" => ПерерахунокТоварів_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Відповідальний" => ((ФізичніОсоби_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ПерерахунокТоварів_Select.QuerySelect.Order.Add(
               Документи.ПерерахунокТоварів_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ПерерахунокТоварів_Select.QuerySelect, Документи.ПерерахунокТоварів_Const.Організація,
                ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Склади_Pointer.GetJoin(ПерерахунокТоварів_Select.QuerySelect, Документи.ПерерахунокТоварів_Const.Склад,
                ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_2", "Склад");
            Довідники.ФізичніОсоби_Pointer.GetJoin(ПерерахунокТоварів_Select.QuerySelect, Документи.ПерерахунокТоварів_Const.Відповідальний,
                ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_3", "Відповідальний");
            Довідники.Користувачі_Pointer.GetJoin(ПерерахунокТоварів_Select.QuerySelect, Документи.ПерерахунокТоварів_Const.Автор,
                ПерерахунокТоварів_Select.QuerySelect.Table, "join_tab_4", "Автор");
            

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
                        Назва = Fields[ПерерахунокТоварів_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ПерерахунокТоварів_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ПерерахунокТоварів_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            Відповідальний = Fields["Відповідальний"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ПерерахунокТоварів_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ПсуванняТоварів_Const.Організація,
                                  "Склад" => ПсуванняТоварів_Const.Склад,
                                  "СумаДокументу" => ПсуванняТоварів_Const.СумаДокументу,
                                  "Автор" => ПсуванняТоварів_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ПсуванняТоварів_Select.QuerySelect.Order.Add(
               Документи.ПсуванняТоварів_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ПсуванняТоварів_Select.QuerySelect, Документи.ПсуванняТоварів_Const.Організація,
                ПсуванняТоварів_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Склади_Pointer.GetJoin(ПсуванняТоварів_Select.QuerySelect, Документи.ПсуванняТоварів_Const.Склад,
                ПсуванняТоварів_Select.QuerySelect.Table, "join_tab_2", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(ПсуванняТоварів_Select.QuerySelect, Документи.ПсуванняТоварів_Const.Автор,
                ПсуванняТоварів_Select.QuerySelect.Table, "join_tab_3", "Автор");
            

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
                        Назва = Fields[ПсуванняТоварів_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ПсуванняТоварів_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ПсуванняТоварів_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            СумаДокументу = Fields[ПсуванняТоварів_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ПсуванняТоварів_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ВнутрішнєСпоживанняТоварів_Const.Організація,
                                  "Склад" => ВнутрішнєСпоживанняТоварів_Const.Склад,
                                  "Валюта" => ВнутрішнєСпоживанняТоварів_Const.Валюта,
                                  "СумаДокументу" => ВнутрішнєСпоживанняТоварів_Const.СумаДокументу,
                                  "Автор" => ВнутрішнєСпоживанняТоварів_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Order.Add(
               Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ВнутрішнєСпоживанняТоварів_Select.QuerySelect, Документи.ВнутрішнєСпоживанняТоварів_Const.Організація,
                ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Склади_Pointer.GetJoin(ВнутрішнєСпоживанняТоварів_Select.QuerySelect, Документи.ВнутрішнєСпоживанняТоварів_Const.Склад,
                ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table, "join_tab_2", "Склад");
            Довідники.Валюти_Pointer.GetJoin(ВнутрішнєСпоживанняТоварів_Select.QuerySelect, Документи.ВнутрішнєСпоживанняТоварів_Const.Валюта,
                ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table, "join_tab_3", "Валюта");
            Довідники.Користувачі_Pointer.GetJoin(ВнутрішнєСпоживанняТоварів_Select.QuerySelect, Документи.ВнутрішнєСпоживанняТоварів_Const.Автор,
                ВнутрішнєСпоживанняТоварів_Select.QuerySelect.Table, "join_tab_4", "Автор");
            

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
                        Назва = Fields[ВнутрішнєСпоживанняТоварів_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ВнутрішнєСпоживанняТоварів_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ВнутрішнєСпоживанняТоварів_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            СумаДокументу = Fields[ВнутрішнєСпоживанняТоварів_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ВнутрішнєСпоживанняТоварів_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => РахунокФактура_Const.Організація,
                                  "Контрагент" => РахунокФактура_Const.Контрагент,
                                  "Валюта" => РахунокФактура_Const.Валюта,
                                  "Каса" => РахунокФактура_Const.Каса,
                                  "Склад" => РахунокФактура_Const.Склад,
                                  "СумаДокументу" => РахунокФактура_Const.СумаДокументу,
                                  "Автор" => РахунокФактура_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            РахунокФактура_Select.QuerySelect.Order.Add(
               Документи.РахунокФактура_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(РахунокФактура_Select.QuerySelect, Документи.РахунокФактура_Const.Організація,
                РахунокФактура_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Контрагенти_Pointer.GetJoin(РахунокФактура_Select.QuerySelect, Документи.РахунокФактура_Const.Контрагент,
                РахунокФактура_Select.QuerySelect.Table, "join_tab_2", "Контрагент");
            Довідники.Валюти_Pointer.GetJoin(РахунокФактура_Select.QuerySelect, Документи.РахунокФактура_Const.Валюта,
                РахунокФактура_Select.QuerySelect.Table, "join_tab_3", "Валюта");
            Довідники.Каси_Pointer.GetJoin(РахунокФактура_Select.QuerySelect, Документи.РахунокФактура_Const.Каса,
                РахунокФактура_Select.QuerySelect.Table, "join_tab_4", "Каса");
            Довідники.Склади_Pointer.GetJoin(РахунокФактура_Select.QuerySelect, Документи.РахунокФактура_Const.Склад,
                РахунокФактура_Select.QuerySelect.Table, "join_tab_5", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(РахунокФактура_Select.QuerySelect, Документи.РахунокФактура_Const.Автор,
                РахунокФактура_Select.QuerySelect.Table, "join_tab_6", "Автор");
            

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
                        Назва = Fields[РахунокФактура_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[РахунокФактура_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[РахунокФактура_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            СумаДокументу = Fields[РахунокФактура_Const.СумаДокументу].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[РахунокФактура_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* ДокументПоступлення, pointer */
                      Switch sw = new();
                      ПоступленняТоварівТаПослуг_PointerControl ДокументПоступлення = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("ДокументПоступлення", ДокументПоступлення, sw));
                      ДодатиЕлементВФільтр(listBox, "Документ поступлення:", ДокументПоступлення, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Склад" => РозміщенняТоварівНаСкладі_Const.Склад,
                                  "ДокументПоступлення" => РозміщенняТоварівНаСкладі_Const.ДокументПоступлення,
                                  "Автор" => РозміщенняТоварівНаСкладі_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "ДокументПоступлення" => ((ПоступленняТоварівТаПослуг_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            РозміщенняТоварівНаСкладі_Select.QuerySelect.Order.Add(
               Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Склади_Pointer.GetJoin(РозміщенняТоварівНаСкладі_Select.QuerySelect, Документи.РозміщенняТоварівНаСкладі_Const.Склад,
                РозміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_1", "Склад");
            Документи.ПоступленняТоварівТаПослуг_Pointer.GetJoin(РозміщенняТоварівНаСкладі_Select.QuerySelect, Документи.РозміщенняТоварівНаСкладі_Const.ДокументПоступлення,
                РозміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_2", "ДокументПоступлення");
            Довідники.Користувачі_Pointer.GetJoin(РозміщенняТоварівНаСкладі_Select.QuerySelect, Документи.РозміщенняТоварівНаСкладі_Const.Автор,
                РозміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_3", "Автор");
            

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
                        Назва = Fields[РозміщенняТоварівНаСкладі_Const.Назва].ToString() ?? "",
                            ДатаДок = Fields[РозміщенняТоварівНаСкладі_Const.ДатаДок].ToString() ?? "",
                            НомерДок = Fields[РозміщенняТоварівНаСкладі_Const.НомерДок].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            ДокументПоступлення = Fields["ДокументПоступлення"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[РозміщенняТоварівНаСкладі_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Склад" => ПереміщенняТоварівНаСкладі_Const.Склад,
                                  "Організація" => ПереміщенняТоварівНаСкладі_Const.Організація,
                                  "Автор" => ПереміщенняТоварівНаСкладі_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ПереміщенняТоварівНаСкладі_Select.QuerySelect.Order.Add(
               Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Склади_Pointer.GetJoin(ПереміщенняТоварівНаСкладі_Select.QuerySelect, Документи.ПереміщенняТоварівНаСкладі_Const.Склад,
                ПереміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_1", "Склад");
            Довідники.Організації_Pointer.GetJoin(ПереміщенняТоварівНаСкладі_Select.QuerySelect, Документи.ПереміщенняТоварівНаСкладі_Const.Організація,
                ПереміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_2", "Організація");
            Довідники.Користувачі_Pointer.GetJoin(ПереміщенняТоварівНаСкладі_Select.QuerySelect, Документи.ПереміщенняТоварівНаСкладі_Const.Автор,
                ПереміщенняТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_3", "Автор");
            

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
                        Назва = Fields[ПереміщенняТоварівНаСкладі_Const.Назва].ToString() ?? "",
                            ДатаДок = Fields[ПереміщенняТоварівНаСкладі_Const.ДатаДок].ToString() ?? "",
                            НомерДок = Fields[ПереміщенняТоварівНаСкладі_Const.НомерДок].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ПереміщенняТоварівНаСкладі_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* ДокументРеалізації, pointer */
                      Switch sw = new();
                      РеалізаціяТоварівТаПослуг_PointerControl ДокументРеалізації = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("ДокументРеалізації", ДокументРеалізації, sw));
                      ДодатиЕлементВФільтр(listBox, "Документ реалізації:", ДокументРеалізації, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Склад" => ЗбіркаТоварівНаСкладі_Const.Склад,
                                  "ДокументРеалізації" => ЗбіркаТоварівНаСкладі_Const.ДокументРеалізації,
                                  "Автор" => ЗбіркаТоварівНаСкладі_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "ДокументРеалізації" => ((РеалізаціяТоварівТаПослуг_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            ЗбіркаТоварівНаСкладі_Select.QuerySelect.Order.Add(
               Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Склади_Pointer.GetJoin(ЗбіркаТоварівНаСкладі_Select.QuerySelect, Документи.ЗбіркаТоварівНаСкладі_Const.Склад,
                ЗбіркаТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_1", "Склад");
            Документи.РеалізаціяТоварівТаПослуг_Pointer.GetJoin(ЗбіркаТоварівНаСкладі_Select.QuerySelect, Документи.ЗбіркаТоварівНаСкладі_Const.ДокументРеалізації,
                ЗбіркаТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_2", "ДокументРеалізації");
            Довідники.Користувачі_Pointer.GetJoin(ЗбіркаТоварівНаСкладі_Select.QuerySelect, Документи.ЗбіркаТоварівНаСкладі_Const.Автор,
                ЗбіркаТоварівНаСкладі_Select.QuerySelect.Table, "join_tab_3", "Автор");
            

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
                        Назва = Fields[ЗбіркаТоварівНаСкладі_Const.Назва].ToString() ?? "",
                            ДатаДок = Fields[ЗбіркаТоварівНаСкладі_Const.ДатаДок].ToString() ?? "",
                            НомерДок = Fields[ЗбіркаТоварівНаСкладі_Const.НомерДок].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            ДокументРеалізації = Fields["ДокументРеалізації"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ЗбіркаТоварівНаСкладі_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => РозміщенняНоменклатуриПоКоміркам_Const.Організація,
                                  "Склад" => РозміщенняНоменклатуриПоКоміркам_Const.Склад,
                                  "Автор" => РозміщенняНоменклатуриПоКоміркам_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Order.Add(
               Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Організація,
                РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Склади_Pointer.GetJoin(РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Склад,
                РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Table, "join_tab_2", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Автор,
                РозміщенняНоменклатуриПоКоміркам_Select.QuerySelect.Table, "join_tab_3", "Автор");
            

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
                        Назва = Fields[РозміщенняНоменклатуриПоКоміркам_Const.Назва].ToString() ?? "",
                            ДатаДок = Fields[РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок].ToString() ?? "",
                            НомерДок = Fields[РозміщенняНоменклатуриПоКоміркам_Const.НомерДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[РозміщенняНоменклатуриПоКоміркам_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => КорегуванняБоргу_Const.Організація,
                                  "Автор" => КорегуванняБоргу_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

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

            КорегуванняБоргу_Select.QuerySelect.Order.Add(
               Документи.КорегуванняБоргу_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(КорегуванняБоргу_Select.QuerySelect, Документи.КорегуванняБоргу_Const.Організація,
                КорегуванняБоргу_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Користувачі_Pointer.GetJoin(КорегуванняБоргу_Select.QuerySelect, Документи.КорегуванняБоргу_Const.Автор,
                КорегуванняБоргу_Select.QuerySelect.Table, "join_tab_2", "Автор");
            

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
                        Назва = Fields[КорегуванняБоргу_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[КорегуванняБоргу_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[КорегуванняБоргу_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[КорегуванняБоргу_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
        }
    }
	    
    #endregion
    
    #region DOCUMENT "ЗакриттяЗамовленняКлієнта"
    
      
    public class ЗакриттяЗамовленняКлієнта_Записи : ТабличнийСписок
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
        string ПричинаЗакриттяЗамовлення = "";
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
                /*ПричинаЗакриттяЗамовлення*/ ПричинаЗакриттяЗамовлення,
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
                /*ПричинаЗакриттяЗамовлення*/ typeof(string),  
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
            treeView.AppendColumn(new TreeViewColumn("Причина", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true } ); /*ПричинаЗакриттяЗамовлення*/
            treeView.AppendColumn(new TreeViewColumn("Автор", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true } ); /*Автор*/
            treeView.AppendColumn(new TreeViewColumn("Коментар", new CellRendererText() { Xpad = 4 }, "text", 14) { MinWidth = 20, Resizable = true } ); /*Коментар*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗакриттяЗамовленняКлієнта_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            
                  List<Tuple<string, Widget, Switch>> widgets = [];
                  
                  { /* Організація, pointer */
                      Switch sw = new();
                      Організації_PointerControl Організація = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Організація", Організація, sw));
                      ДодатиЕлементВФільтр(listBox, "Організація:", Організація, sw);
                  }
                  
                  { /* Контрагент, pointer */
                      Switch sw = new();
                      Контрагенти_PointerControl Контрагент = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Контрагент", Контрагент, sw));
                      ДодатиЕлементВФільтр(listBox, "Контрагент:", Контрагент, sw);
                  }
                  
                  { /* Валюта, pointer */
                      Switch sw = new();
                      Валюти_PointerControl Валюта = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Валюта", Валюта, sw));
                      ДодатиЕлементВФільтр(listBox, "Валюта:", Валюта, sw);
                  }
                  
                  { /* Каса, pointer */
                      Switch sw = new();
                      Каси_PointerControl Каса = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Каса", Каса, sw));
                      ДодатиЕлементВФільтр(listBox, "Каса:", Каса, sw);
                  }
                  
                  { /* Склад, pointer */
                      Switch sw = new();
                      Склади_PointerControl Склад = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Склад", Склад, sw));
                      ДодатиЕлементВФільтр(listBox, "Склад:", Склад, sw);
                  }
                  
                  { /* СумаДокументу, numeric */
                      Switch sw = new();
                      NumericControl СумаДокументу = new();
                      widgets.Add(new("СумаДокументу", СумаДокументу, sw));
                      ДодатиЕлементВФільтр(listBox, "Сума:", СумаДокументу, sw);
                  }
                  
                  { /* ПричинаЗакриттяЗамовлення, enum */
                      Switch sw = new();
                      ComboBoxText ПричинаЗакриттяЗамовлення = new();
                          foreach (var item in ПсевдонімиПерелічення.ПричиниЗакриттяЗамовленняКлієнта_List()) ПричинаЗакриттяЗамовлення.Append(item.Value.ToString(), item.Name);
                          ПричинаЗакриттяЗамовлення.Active = 0;
                          
                      widgets.Add(new("ПричинаЗакриттяЗамовлення", ПричинаЗакриттяЗамовлення, sw));
                      ДодатиЕлементВФільтр(listBox, "Причина:", ПричинаЗакриттяЗамовлення, sw);
                  }
                  
                  { /* Автор, pointer */
                      Switch sw = new();
                      Користувачі_PointerControl Автор = new() { Caption = "", AfterSelectFunc = () => sw.Active = true };
                      widgets.Add(new("Автор", Автор, sw));
                      ДодатиЕлементВФільтр(listBox, "Автор:", Автор, sw);
                  }
                  
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List<Where> listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { "Організація" => ЗакриттяЗамовленняКлієнта_Const.Організація,
                                  "Контрагент" => ЗакриттяЗамовленняКлієнта_Const.Контрагент,
                                  "Валюта" => ЗакриттяЗамовленняКлієнта_Const.Валюта,
                                  "Каса" => ЗакриттяЗамовленняКлієнта_Const.Каса,
                                  "Склад" => ЗакриттяЗамовленняКлієнта_Const.Склад,
                                  "СумаДокументу" => ЗакриттяЗамовленняКлієнта_Const.СумаДокументу,
                                  "ПричинаЗакриттяЗамовлення" => ЗакриттяЗамовленняКлієнта_Const.ПричинаЗакриттяЗамовлення,
                                  "Автор" => ЗакриттяЗамовленняКлієнта_Const.Автор,
                                   _ => null };
                                  object? value = widget.Item1 switch { "Організація" => ((Організації_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Контрагент" => ((Контрагенти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Валюта" => ((Валюти_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Каса" => ((Каси_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "Склад" => ((Склади_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                  "СумаДокументу" => ((NumericControl)widget.Item2).Value,
                                  "ПричинаЗакриттяЗамовлення" => (int)Enum.Parse<ПричиниЗакриттяЗамовленняКлієнта>(((ComboBoxText)widget.Item2).ActiveId),
                                  "Автор" => ((Користувачі_PointerControl)widget.Item2).Pointer.UnigueID.UGuid,
                                   _ => null };
                                  if (field != null && value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.ЗакриттяЗамовленняКлієнта_Select ЗакриттяЗамовленняКлієнта_Select = new Документи.ЗакриттяЗамовленняКлієнта_Select();
            ЗакриттяЗамовленняКлієнта_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                /*Назва*/ Документи.ЗакриттяЗамовленняКлієнта_Const.Назва,
                /*НомерДок*/ Документи.ЗакриттяЗамовленняКлієнта_Const.НомерДок,
                /*ДатаДок*/ Документи.ЗакриттяЗамовленняКлієнта_Const.ДатаДок,
                /*СумаДокументу*/ Документи.ЗакриттяЗамовленняКлієнта_Const.СумаДокументу,
                /*ПричинаЗакриттяЗамовлення*/ Документи.ЗакриттяЗамовленняКлієнта_Const.ПричинаЗакриттяЗамовлення,
                /*Коментар*/ Документи.ЗакриттяЗамовленняКлієнта_Const.Коментар,
                
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ЗакриттяЗамовленняКлієнта_Select.QuerySelect.Where = (List<Where>)where;

            ЗакриттяЗамовленняКлієнта_Select.QuerySelect.Order.Add(
               Документи.ЗакриттяЗамовленняКлієнта_Const.ДатаДок, SelectOrder.ASC);
            Довідники.Організації_Pointer.GetJoin(ЗакриттяЗамовленняКлієнта_Select.QuerySelect, Документи.ЗакриттяЗамовленняКлієнта_Const.Організація,
                ЗакриттяЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_1", "Організація");
            Довідники.Контрагенти_Pointer.GetJoin(ЗакриттяЗамовленняКлієнта_Select.QuerySelect, Документи.ЗакриттяЗамовленняКлієнта_Const.Контрагент,
                ЗакриттяЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_2", "Контрагент");
            Довідники.Валюти_Pointer.GetJoin(ЗакриттяЗамовленняКлієнта_Select.QuerySelect, Документи.ЗакриттяЗамовленняКлієнта_Const.Валюта,
                ЗакриттяЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_3", "Валюта");
            Довідники.Каси_Pointer.GetJoin(ЗакриттяЗамовленняКлієнта_Select.QuerySelect, Документи.ЗакриттяЗамовленняКлієнта_Const.Каса,
                ЗакриттяЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_4", "Каса");
            Довідники.Склади_Pointer.GetJoin(ЗакриттяЗамовленняКлієнта_Select.QuerySelect, Документи.ЗакриттяЗамовленняКлієнта_Const.Склад,
                ЗакриттяЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_5", "Склад");
            Довідники.Користувачі_Pointer.GetJoin(ЗакриттяЗамовленняКлієнта_Select.QuerySelect, Документи.ЗакриттяЗамовленняКлієнта_Const.Автор,
                ЗакриттяЗамовленняКлієнта_Select.QuerySelect.Table, "join_tab_6", "Автор");
            

            /* SELECT */
            await ЗакриттяЗамовленняКлієнта_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (ЗакриттяЗамовленняКлієнта_Select.MoveNext())
            {
                Документи.ЗакриттяЗамовленняКлієнта_Pointer? cur = ЗакриттяЗамовленняКлієнта_Select.Current;

                if (cur != null)
                {
                    Dictionary<string, object> Fields = cur.Fields!;
                    ЗакриттяЗамовленняКлієнта_Записи Record = new ЗакриттяЗамовленняКлієнта_Записи
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        Назва = Fields[ЗакриттяЗамовленняКлієнта_Const.Назва].ToString() ?? "",
                            НомерДок = Fields[ЗакриттяЗамовленняКлієнта_Const.НомерДок].ToString() ?? "",
                            ДатаДок = Fields[ЗакриттяЗамовленняКлієнта_Const.ДатаДок].ToString() ?? "",
                            Організація = Fields["Організація"].ToString() ?? "",
                            Контрагент = Fields["Контрагент"].ToString() ?? "",
                            Валюта = Fields["Валюта"].ToString() ?? "",
                            Каса = Fields["Каса"].ToString() ?? "",
                            Склад = Fields["Склад"].ToString() ?? "",
                            СумаДокументу = Fields[ЗакриттяЗамовленняКлієнта_Const.СумаДокументу].ToString() ?? "",
                            ПричинаЗакриттяЗамовлення = Перелічення.ПсевдонімиПерелічення.ПричиниЗакриттяЗамовленняКлієнта_Alias((
                               (Перелічення.ПричиниЗакриттяЗамовленняКлієнта)(Fields[ЗакриттяЗамовленняКлієнта_Const.ПричинаЗакриттяЗамовлення] != DBNull.Value ? Fields[ЗакриттяЗамовленняКлієнта_Const.ПричинаЗакриттяЗамовлення] : 0)) ),
                            Автор = Fields["Автор"].ToString() ?? "",
                            Коментар = Fields[ЗакриттяЗамовленняКлієнта_Const.Коментар].ToString() ?? "",
                            
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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
                /*Назва*/ Назва,
                /*Дата*/ Дата,
                /*Номер*/ Номер,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Склад*/ Склад,
                /*Каса*/ Каса,
                /*Валюта*/ Валюта,
                /*Сума*/ Сума,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                 
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
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗакриттяЗамовленняКлієнта_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ЗакриттяЗамовленняКлієнта", [where]);
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
                {"ЗакриттяЗамовленняКлієнта", "Закриття замовлення клієнта"},
                
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВстановленняЦінНоменклатури_Const.TABLE + "." + Документи.ВстановленняЦінНоменклатури_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВстановленняЦінНоменклатури_Const.TABLE + "." + Документи.ВстановленняЦінНоменклатури_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВстановленняЦінНоменклатури_Const.TABLE + "." + Документи.ВстановленняЦінНоменклатури_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ВстановленняЦінНоменклатури_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент")); /* Empty Field - Контрагент*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад")); /* Empty Field - Склад*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        Довідники.Валюти_Pointer.GetJoin(query, Документи.ВстановленняЦінНоменклатури_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума")); /* Empty Field - Сума*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.ВстановленняЦінНоменклатури_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВстановленняЦінНоменклатури_Const.TABLE + "." + Документи.ВстановленняЦінНоменклатури_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад")); /* Empty Field - Склад*/
                        Довідники.Каси_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад")); /* Empty Field - Склад*/
                        Довідники.Каси_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПереміщенняТоварів_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент")); /* Empty Field - Контрагент*/
                        Довідники.Склади_Pointer.GetJoin(query, Документи.ПереміщенняТоварів_Const.СкладВідправник, query.Table, "join_tab_6", "Склад");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта")); /* Empty Field - Валюта*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума")); /* Empty Field - Сума*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПереміщенняТоварів_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад")); /* Empty Field - Склад*/
                        Довідники.Каси_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ВведенняЗалишків_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ВведенняЗалишків_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ВведенняЗалишків_Const.Склад, query.Table, "join_tab_6", "Склад");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        Довідники.Валюти_Pointer.GetJoin(query, Документи.ВведенняЗалишків_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума")); /* Empty Field - Сума*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.ВведенняЗалишків_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПерерахунокТоварів_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.ФізичніОсоби_Pointer.GetJoin(query, Документи.ПерерахунокТоварів_Const.Відповідальний, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПерерахунокТоварів_Const.Склад, query.Table, "join_tab_6", "Склад");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта")); /* Empty Field - Валюта*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума")); /* Empty Field - Сума*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПерерахунокТоварів_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПсуванняТоварів_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент")); /* Empty Field - Контрагент*/
                        Довідники.Склади_Pointer.GetJoin(query, Документи.ПсуванняТоварів_Const.Склад, query.Table, "join_tab_6", "Склад");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта")); /* Empty Field - Валюта*/
                        
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПсуванняТоварів_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ВнутрішнєСпоживанняТоварів_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент")); /* Empty Field - Контрагент*/
                        Довідники.Склади_Pointer.GetJoin(query, Документи.ВнутрішнєСпоживанняТоварів_Const.Склад, query.Table, "join_tab_6", "Склад");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        Довідники.Валюти_Pointer.GetJoin(query, Документи.ВнутрішнєСпоживанняТоварів_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ВнутрішнєСпоживанняТоварів_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РозміщенняТоварівНаСкладі_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент")); /* Empty Field - Контрагент*/
                        Довідники.Склади_Pointer.GetJoin(query, Документи.РозміщенняТоварівНаСкладі_Const.Склад, query.Table, "join_tab_6", "Склад");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта")); /* Empty Field - Валюта*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума")); /* Empty Field - Сума*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.РозміщенняТоварівНаСкладі_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПереміщенняТоварівНаСкладі_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент")); /* Empty Field - Контрагент*/
                        Довідники.Склади_Pointer.GetJoin(query, Документи.ПереміщенняТоварівНаСкладі_Const.Склад, query.Table, "join_tab_6", "Склад");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта")); /* Empty Field - Валюта*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума")); /* Empty Field - Сума*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПереміщенняТоварівНаСкладі_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ЗбіркаТоварівНаСкладі_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент")); /* Empty Field - Контрагент*/
                        Довідники.Склади_Pointer.GetJoin(query, Документи.ЗбіркаТоварівНаСкладі_Const.Склад, query.Table, "join_tab_6", "Склад");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта")); /* Empty Field - Валюта*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума")); /* Empty Field - Сума*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.ЗбіркаТоварівНаСкладі_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент")); /* Empty Field - Контрагент*/
                        Довідники.Склади_Pointer.GetJoin(query, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Склад, query.Table, "join_tab_6", "Склад");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта")); /* Empty Field - Валюта*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума")); /* Empty Field - Сума*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.КорегуванняБоргу_Const.TABLE + "." + Документи.КорегуванняБоргу_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.КорегуванняБоргу_Const.TABLE + "." + Документи.КорегуванняБоргу_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.КорегуванняБоргу_Const.TABLE + "." + Документи.КорегуванняБоргу_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.КорегуванняБоргу_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Контрагент")); /* Empty Field - Контрагент*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад")); /* Empty Field - Склад*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Каса")); /* Empty Field - Каса*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Валюта")); /* Empty Field - Валюта*/
                        
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Сума")); /* Empty Field - Сума*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.КорегуванняБоргу_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.КорегуванняБоргу_Const.TABLE + "." + Документи.КорегуванняБоргу_Const.Коментар, "Коментар"));
                            
                  allQuery.Add(query.Construct());
              }
              
              //Документ: ЗакриттяЗамовленняКлієнта
              {
                  Query query = new Query(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ЗакриттяЗамовленняКлієнта", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
                  }
                  
                  query.FieldAndAlias.Add(new NameValue<string>("'ЗакриттяЗамовленняКлієнта'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.СумаДокументу + "::text", "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.Коментар, "Коментар"));
                            
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
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
          
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
                /*Назва*/ Назва,
                /*Дата*/ Дата,
                /*Номер*/ Номер,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Склад*/ Склад,
                /*Каса*/ Каса,
                /*Валюта*/ Валюта,
                /*Сума*/ Сума,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                 
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ЗамовленняПостачальнику_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняПостачальнику_Const.TABLE + "." + Документи.ЗамовленняПостачальнику_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПоступленняТоварівТаПослуг_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоступленняТоварівТаПослуг_Const.TABLE + "." + Документи.ПоступленняТоварівТаПослуг_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПоверненняТоварівПостачальнику_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівПостачальнику_Const.TABLE + "." + Документи.ПоверненняТоварівПостачальнику_Const.Коментар, "Коментар"));
                            
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
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
          
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
                /*Назва*/ Назва,
                /*Дата*/ Дата,
                /*Номер*/ Номер,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Склад*/ Склад,
                /*Каса*/ Каса,
                /*Валюта*/ Валюта,
                /*Сума*/ Сума,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                 
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
              
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.ЗакриттяЗамовленняКлієнта_Const.ДатаДок, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("ЗакриттяЗамовленняКлієнта", [where]);
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
                {"ЗакриттяЗамовленняКлієнта", "Закриття замовлення клієнта"},
                
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ЗамовленняКлієнта_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗамовленняКлієнта_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.РеалізаціяТоварівТаПослуг_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РеалізаціяТоварівТаПослуг_Const.TABLE + "." + Документи.РеалізаціяТоварівТаПослуг_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПоверненняТоварівВідКлієнта_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПоверненняТоварівВідКлієнта_Const.TABLE + "." + Документи.ПоверненняТоварівВідКлієнта_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "Склад")); /* Empty Field - Склад*/
                        Довідники.Каси_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.АктВиконанихРобіт_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.АктВиконанихРобіт_Const.TABLE + "." + Документи.АктВиконанихРобіт_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.РахунокФактура_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РахунокФактура_Const.TABLE + "." + Документи.РахунокФактура_Const.Коментар, "Коментар"));
                            
                  allQuery.Add(query.Construct());
              }
              
              //Документ: ЗакриттяЗамовленняКлієнта
              {
                  Query query = new Query(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary<string, List<Where>>)dataWhere;
                      if (dictWhere.TryGetValue("ЗакриттяЗамовленняКлієнта", out List<Where>? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
                  }
                  
                  query.FieldAndAlias.Add(new NameValue<string>("'ЗакриттяЗамовленняКлієнта'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Склад, query.Table, "join_tab_6", "Склад");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Каса, query.Table, "join_tab_7", "Каса");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ЗакриттяЗамовленняКлієнта_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗакриттяЗамовленняКлієнта_Const.TABLE + "." + Документи.ЗакриттяЗамовленняКлієнта_Const.Коментар, "Коментар"));
                            
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
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
          
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
                /*Назва*/ Назва,
                /*Дата*/ Дата,
                /*Номер*/ Номер,
                /*Організація*/ Організація,
                /*Контрагент*/ Контрагент,
                /*Каса*/ Каса,
                /*Каса2*/ Каса2,
                /*Валюта*/ Валюта,
                /*Сума*/ Сума,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                 
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Каса, query.Table, "join_tab_6", "Каса");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.КасаВідправник, query.Table, "join_tab_7", "Каса2");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПрихіднийКасовийОрдер_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПрихіднийКасовийОрдер_Const.TABLE + "." + Документи.ПрихіднийКасовийОрдер_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Контрагенти_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Контрагент, query.Table, "join_tab_5", "Контрагент");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Каса, query.Table, "join_tab_6", "Каса");
                            Довідники.Каси_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.КасаОтримувач, query.Table, "join_tab_7", "Каса2");
                            Довідники.Валюти_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Валюта, query.Table, "join_tab_8", "Валюта");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.СумаДокументу, "Сума"));
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.РозхіднийКасовийОрдер_Const.Автор, query.Table, "join_tab_10", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозхіднийКасовийОрдер_Const.TABLE + "." + Документи.РозхіднийКасовийОрдер_Const.Коментар, "Коментар"));
                            
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
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
          
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
                /*Назва*/ Назва,
                /*Дата*/ Дата,
                /*Номер*/ Номер,
                /*Організація*/ Організація,
                /*СкладВідправник*/ СкладВідправник,
                /*СкладОтримувач*/ СкладОтримувач,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                 
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПереміщенняТоварів_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПереміщенняТоварів_Const.СкладВідправник, query.Table, "join_tab_5", "СкладВідправник");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПереміщенняТоварів_Const.СкладОтримувач, query.Table, "join_tab_6", "СкладОтримувач");
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПереміщенняТоварів_Const.Автор, query.Table, "join_tab_7", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварів_Const.TABLE + "." + Документи.ПереміщенняТоварів_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ВведенняЗалишків_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "СкладВідправник")); /* Empty Field - СкладВідправник*/
                        Довідники.Склади_Pointer.GetJoin(query, Документи.ВведенняЗалишків_Const.Склад, query.Table, "join_tab_6", "СкладОтримувач");
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ВведенняЗалишків_Const.Автор, query.Table, "join_tab_7", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВведенняЗалишків_Const.TABLE + "." + Документи.ВведенняЗалишків_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ВнутрішнєСпоживанняТоварів_Const.Організація, query.Table, "join_tab_4", "Організація");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "СкладВідправник")); /* Empty Field - СкладВідправник*/
                        Довідники.Склади_Pointer.GetJoin(query, Документи.ВнутрішнєСпоживанняТоварів_Const.Склад, query.Table, "join_tab_6", "СкладОтримувач");
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ВнутрішнєСпоживанняТоварів_Const.Автор, query.Table, "join_tab_7", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ВнутрішнєСпоживанняТоварів_Const.TABLE + "." + Документи.ВнутрішнєСпоживанняТоварів_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПсуванняТоварів_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПсуванняТоварів_Const.Склад, query.Table, "join_tab_5", "СкладВідправник");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "СкладОтримувач")); /* Empty Field - СкладОтримувач*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПсуванняТоварів_Const.Автор, query.Table, "join_tab_7", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПсуванняТоварів_Const.TABLE + "." + Документи.ПсуванняТоварів_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПерерахунокТоварів_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПерерахунокТоварів_Const.Склад, query.Table, "join_tab_5", "СкладВідправник");
                            
                          query.FieldAndAlias.Add(new NameValue<string>("''", "СкладОтримувач")); /* Empty Field - СкладОтримувач*/
                        Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПерерахунокТоварів_Const.Автор, query.Table, "join_tab_7", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПерерахунокТоварів_Const.TABLE + "." + Документи.ПерерахунокТоварів_Const.Коментар, "Коментар"));
                            
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
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
          
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
                /*Назва*/ Назва,
                /*Дата*/ Дата,
                /*Номер*/ Номер,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Автор*/ Автор,
                /*Коментар*/ Коментар,
                 
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РозміщенняТоварівНаСкладі_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.РозміщенняТоварівНаСкладі_Const.Склад, query.Table, "join_tab_5", "Склад");
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.РозміщенняТоварівНаСкладі_Const.Автор, query.Table, "join_tab_6", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.РозміщенняТоварівНаСкладі_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ПереміщенняТоварівНаСкладі_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ПереміщенняТоварівНаСкладі_Const.Склад, query.Table, "join_tab_5", "Склад");
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ПереміщенняТоварівНаСкладі_Const.Автор, query.Table, "join_tab_6", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ПереміщенняТоварівНаСкладі_Const.TABLE + "." + Документи.ПереміщенняТоварівНаСкладі_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.ЗбіркаТоварівНаСкладі_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.ЗбіркаТоварівНаСкладі_Const.Склад, query.Table, "join_tab_5", "Склад");
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.ЗбіркаТоварівНаСкладі_Const.Автор, query.Table, "join_tab_6", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.ЗбіркаТоварівНаСкладі_Const.TABLE + "." + Документи.ЗбіркаТоварівНаСкладі_Const.Коментар, "Коментар"));
                            
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
                  
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.Назва, "Назва"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.ДатаДок, "Дата"));
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.НомерДок, "Номер"));
                            Довідники.Організації_Pointer.GetJoin(query, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Організація, query.Table, "join_tab_4", "Організація");
                            Довідники.Склади_Pointer.GetJoin(query, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Склад, query.Table, "join_tab_5", "Склад");
                            Довідники.Користувачі_Pointer.GetJoin(query, Документи.РозміщенняНоменклатуриПоКоміркам_Const.Автор, query.Table, "join_tab_6", "Автор");
                            
                              query.FieldAndAlias.Add(new NameValue<string>(Документи.РозміщенняНоменклатуриПоКоміркам_Const.TABLE + "." + Документи.РозміщенняНоменклатуриПоКоміркам_Const.Коментар, "Коментар"));
                            
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
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
          
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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.ЦіниНоменклатури_RecordsSet ЦіниНоменклатури_RecordsSet = new РегістриВідомостей.ЦіниНоменклатури_RecordsSet();
            ЦіниНоменклатури_RecordsSet.FillJoin(["period"]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ЦіниНоменклатури_RecordsSet.QuerySelect.Where = (List<Where>)where;

            await ЦіниНоменклатури_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (ЦіниНоменклатури_RecordsSet.Record record in ЦіниНоменклатури_RecordsSet.Records)
            {
                ЦіниНоменклатури_Записи row = new ЦіниНоменклатури_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        ВидЦіни = record.ВидЦіни.Назва,
                        Ціна = record.Ціна.ToString() ?? "",
                        Пакування = record.Пакування.Назва,
                        Валюта = record.Валюта.Назва,
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.КурсиВалют_RecordsSet КурсиВалют_RecordsSet = new РегістриВідомостей.КурсиВалют_RecordsSet();
            КурсиВалют_RecordsSet.FillJoin(["period"]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) КурсиВалют_RecordsSet.QuerySelect.Where = (List<Where>)where;

            await КурсиВалют_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (КурсиВалют_RecordsSet.Record record in КурсиВалют_RecordsSet.Records)
            {
                КурсиВалют_Записи row = new КурсиВалют_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Валюта = record.Валюта.Назва,
                        Курс = record.Курс.ToString() ?? "",
                        Кратність = record.Кратність.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet ШтрихкодиНоменклатури_RecordsSet = new РегістриВідомостей.ШтрихкодиНоменклатури_RecordsSet();
            ШтрихкодиНоменклатури_RecordsSet.FillJoin(["period"]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ШтрихкодиНоменклатури_RecordsSet.QuerySelect.Where = (List<Where>)where;

            await ШтрихкодиНоменклатури_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (ШтрихкодиНоменклатури_RecordsSet.Record record in ШтрихкодиНоменклатури_RecordsSet.Records)
            {
                ШтрихкодиНоменклатури_Записи row = new ШтрихкодиНоменклатури_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Штрихкод = record.Штрихкод.ToString() ?? "",
                        Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        Пакування = record.Пакування.Назва,
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.ФайлиДокументів_RecordsSet ФайлиДокументів_RecordsSet = new РегістриВідомостей.ФайлиДокументів_RecordsSet();
            ФайлиДокументів_RecordsSet.FillJoin(["period"]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ФайлиДокументів_RecordsSet.QuerySelect.Where = (List<Where>)where;

            await ФайлиДокументів_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (ФайлиДокументів_RecordsSet.Record record in ФайлиДокументів_RecordsSet.Records)
            {
                ФайлиДокументів_Записи row = new ФайлиДокументів_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Файл = record.Файл.Назва,
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
        }
    }
	    
    #endregion
    
    #region REGISTER "РозміщенняНоменклатуриПоКоміркамНаСкладі"
    
      
    public class РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи : ТабличнийСписок
    {
        string ID = "";
        string Період = "";
        
        string Номенклатура = "";
        string Склад = "";
        string Приміщення = "";
        string Комірка = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Період,
                /*Номенклатура*/ Номенклатура,
                /*Склад*/ Склад,
                /*Приміщення*/ Приміщення,
                /*Комірка*/ Комірка,
                 
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
                /*Склад*/ typeof(string),
                /*Приміщення*/ typeof(string),
                /*Комірка*/ typeof(string),
                
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 2));
            /* */
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Приміщення", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Приміщення*/
            treeView.AppendColumn(new TreeViewColumn("Комірка", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Комірка*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet = new РегістриВідомостей.РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet();
            РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.FillJoin(["period"]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.QuerySelect.Where = (List<Where>)where;

            await РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Record record in РозміщенняНоменклатуриПоКоміркамНаСкладі_RecordsSet.Records)
            {
                РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи row = new РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Номенклатура = record.Номенклатура.Назва,
                        Склад = record.Склад.Назва,
                        Приміщення = record.Приміщення.Назва,
                        Комірка = record.Комірка.Назва,
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null)
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
        }
    }
	    
    #endregion
    
}

namespace StorageAndTrade_1_0.РегістриНакопичення.ТабличніСписки
{
    
    #region REGISTER "ТовариНаСкладах"
    
      
    public class ТовариНаСкладах_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Склад = "";
        string Серія = "";
        string ВНаявності = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*Склад*/ Склад,
                /*Серія*/ Серія,
                /*ВНаявності*/ ВНаявності,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*Склад*/ typeof(string),
                /*Серія*/ typeof(string),
                /*ВНаявності*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Серія", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*Серія*/
            treeView.AppendColumn(new TreeViewColumn("В наявності", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*ВНаявності*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.ТовариНаСкладах_RecordsSet ТовариНаСкладах_RecordsSet = new РегістриНакопичення.ТовариНаСкладах_RecordsSet();
             ТовариНаСкладах_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ТовариНаСкладах_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await ТовариНаСкладах_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (ТовариНаСкладах_RecordsSet.Record record in ТовариНаСкладах_RecordsSet.Records)
            {
                ТовариНаСкладах_Записи row = new ТовариНаСкладах_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        Склад = record.Склад.Назва,
                        Серія = record.Серія.Назва,
                        ВНаявності = record.ВНаявності.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "ЗамовленняКлієнтів"
    
      
    public class ЗамовленняКлієнтів_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string ЗамовленняКлієнта = "";
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Склад = "";
        string Замовлено = "";
        string Сума = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*ЗамовленняКлієнта*/ ЗамовленняКлієнта,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*Склад*/ Склад,
                /*Замовлено*/ Замовлено,
                /*Сума*/ Сума,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*ЗамовленняКлієнта*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*Склад*/ typeof(string),
                /*Замовлено*/ typeof(string),
                /*Сума*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Замовлення клієнта", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ЗамовленняКлієнта*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Замовлено", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*Замовлено*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true, SortColumnId = 10 } ); /*Сума*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet ЗамовленняКлієнтів_RecordsSet = new РегістриНакопичення.ЗамовленняКлієнтів_RecordsSet();
             ЗамовленняКлієнтів_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ЗамовленняКлієнтів_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await ЗамовленняКлієнтів_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (ЗамовленняКлієнтів_RecordsSet.Record record in ЗамовленняКлієнтів_RecordsSet.Records)
            {
                ЗамовленняКлієнтів_Записи row = new ЗамовленняКлієнтів_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    ЗамовленняКлієнта = record.ЗамовленняКлієнта.Назва,
                        Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        Склад = record.Склад.Назва,
                        Замовлено = record.Замовлено.ToString() ?? "",
                        Сума = record.Сума.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "РозрахункиЗКлієнтами"
    
      
    public class РозрахункиЗКлієнтами_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string Валюта = "";
        string Контрагент = "";
        string Сума = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*Валюта*/ Валюта,
                /*Контрагент*/ Контрагент,
                /*Сума*/ Сума,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*Валюта*/ typeof(string),
                /*Контрагент*/ typeof(string),
                /*Сума*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Сума*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet РозрахункиЗКлієнтами_RecordsSet = new РегістриНакопичення.РозрахункиЗКлієнтами_RecordsSet();
             РозрахункиЗКлієнтами_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) РозрахункиЗКлієнтами_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await РозрахункиЗКлієнтами_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (РозрахункиЗКлієнтами_RecordsSet.Record record in РозрахункиЗКлієнтами_RecordsSet.Records)
            {
                РозрахункиЗКлієнтами_Записи row = new РозрахункиЗКлієнтами_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    Валюта = record.Валюта.Назва,
                        Контрагент = record.Контрагент.Назва,
                        Сума = record.Сума.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "Закупівлі"
    
      
    public class Закупівлі_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string Організація = "";
        string Склад = "";
        string Контрагент = "";
        string Договір = "";
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Кількість = "";
        string Сума = "";
        string Собівартість = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Контрагент*/ Контрагент,
                /*Договір*/ Договір,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*Кількість*/ Кількість,
                /*Сума*/ Сума,
                /*Собівартість*/ Собівартість,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*Організація*/ typeof(string),
                /*Склад*/ typeof(string),
                /*Контрагент*/ typeof(string),
                /*Договір*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*Кількість*/ typeof(string),
                /*Сума*/ typeof(string),
                /*Собівартість*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Договір", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*Договір*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true, SortColumnId = 10 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Кількість", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true, SortColumnId = 11 } ); /*Кількість*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true, SortColumnId = 12 } ); /*Сума*/
            treeView.AppendColumn(new TreeViewColumn("Собівартість", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true, SortColumnId = 13 } ); /*Собівартість*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.Закупівлі_RecordsSet Закупівлі_RecordsSet = new РегістриНакопичення.Закупівлі_RecordsSet();
             Закупівлі_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) Закупівлі_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await Закупівлі_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Закупівлі_RecordsSet.Record record in Закупівлі_RecordsSet.Records)
            {
                Закупівлі_Записи row = new Закупівлі_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    Організація = record.Організація.Назва,
                        Склад = record.Склад.Назва,
                        Контрагент = record.Контрагент.Назва,
                        Договір = record.Договір.Назва,
                        Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        Кількість = record.Кількість.ToString() ?? "",
                        Сума = record.Сума.ToString() ?? "",
                        Собівартість = record.Собівартість.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "ВільніЗалишки"
    
      
    public class ВільніЗалишки_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Склад = "";
        string ВНаявності = "";
        string ВРезервіЗіСкладу = "";
        string ВРезервіПідЗамовлення = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*Склад*/ Склад,
                /*ВНаявності*/ ВНаявності,
                /*ВРезервіЗіСкладу*/ ВРезервіЗіСкладу,
                /*ВРезервіПідЗамовлення*/ ВРезервіПідЗамовлення,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*Склад*/ typeof(string),
                /*ВНаявності*/ typeof(string),
                /*ВРезервіЗіСкладу*/ typeof(string),
                /*ВРезервіПідЗамовлення*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("В наявності", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*ВНаявності*/
            treeView.AppendColumn(new TreeViewColumn("В резерві зі складу", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*ВРезервіЗіСкладу*/
            treeView.AppendColumn(new TreeViewColumn("В резерві під замовлення", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true, SortColumnId = 10 } ); /*ВРезервіПідЗамовлення*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.ВільніЗалишки_RecordsSet ВільніЗалишки_RecordsSet = new РегістриНакопичення.ВільніЗалишки_RecordsSet();
             ВільніЗалишки_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ВільніЗалишки_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await ВільніЗалишки_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (ВільніЗалишки_RecordsSet.Record record in ВільніЗалишки_RecordsSet.Records)
            {
                ВільніЗалишки_Записи row = new ВільніЗалишки_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        Склад = record.Склад.Назва,
                        ВНаявності = record.ВНаявності.ToString() ?? "",
                        ВРезервіЗіСкладу = record.ВРезервіЗіСкладу.ToString() ?? "",
                        ВРезервіПідЗамовлення = record.ВРезервіПідЗамовлення.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "ЗамовленняПостачальникам"
    
      
    public class ЗамовленняПостачальникам_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string ЗамовленняПостачальнику = "";
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Склад = "";
        string Замовлено = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*ЗамовленняПостачальнику*/ ЗамовленняПостачальнику,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*Склад*/ Склад,
                /*Замовлено*/ Замовлено,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*ЗамовленняПостачальнику*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*Склад*/ typeof(string),
                /*Замовлено*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Замовлення постачальнику", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*ЗамовленняПостачальнику*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Замовлено", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*Замовлено*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet ЗамовленняПостачальникам_RecordsSet = new РегістриНакопичення.ЗамовленняПостачальникам_RecordsSet();
             ЗамовленняПостачальникам_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ЗамовленняПостачальникам_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await ЗамовленняПостачальникам_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (ЗамовленняПостачальникам_RecordsSet.Record record in ЗамовленняПостачальникам_RecordsSet.Records)
            {
                ЗамовленняПостачальникам_Записи row = new ЗамовленняПостачальникам_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    ЗамовленняПостачальнику = record.ЗамовленняПостачальнику.Назва,
                        Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        Склад = record.Склад.Назва,
                        Замовлено = record.Замовлено.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "РозрахункиЗПостачальниками"
    
      
    public class РозрахункиЗПостачальниками_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string Контрагент = "";
        string Валюта = "";
        string Сума = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*Контрагент*/ Контрагент,
                /*Валюта*/ Валюта,
                /*Сума*/ Сума,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*Контрагент*/ typeof(string),
                /*Валюта*/ typeof(string),
                /*Сума*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Сума*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet РозрахункиЗПостачальниками_RecordsSet = new РегістриНакопичення.РозрахункиЗПостачальниками_RecordsSet();
             РозрахункиЗПостачальниками_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) РозрахункиЗПостачальниками_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await РозрахункиЗПостачальниками_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (РозрахункиЗПостачальниками_RecordsSet.Record record in РозрахункиЗПостачальниками_RecordsSet.Records)
            {
                РозрахункиЗПостачальниками_Записи row = new РозрахункиЗПостачальниками_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    Контрагент = record.Контрагент.Назва,
                        Валюта = record.Валюта.Назва,
                        Сума = record.Сума.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "РухКоштів"
    
      
    public class РухКоштів_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string Організація = "";
        string Каса = "";
        string Валюта = "";
        string Сума = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*Організація*/ Організація,
                /*Каса*/ Каса,
                /*Валюта*/ Валюта,
                /*Сума*/ Сума,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*Організація*/ typeof(string),
                /*Каса*/ typeof(string),
                /*Валюта*/ typeof(string),
                /*Сума*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Каса", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Каса*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*Сума*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.РухКоштів_RecordsSet РухКоштів_RecordsSet = new РегістриНакопичення.РухКоштів_RecordsSet();
             РухКоштів_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) РухКоштів_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await РухКоштів_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (РухКоштів_RecordsSet.Record record in РухКоштів_RecordsSet.Records)
            {
                РухКоштів_Записи row = new РухКоштів_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    Організація = record.Організація.Назва,
                        Каса = record.Каса.Назва,
                        Валюта = record.Валюта.Назва,
                        Сума = record.Сума.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "ПартіїТоварів"
    
      
    public class ПартіїТоварів_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string Організація = "";
        string ПартіяТоварівКомпозит = "";
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Серія = "";
        string Склад = "";
        string Рядок = "";
        string Кількість = "";
        string Собівартість = "";
        string СписанаСобівартість = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*Організація*/ Організація,
                /*ПартіяТоварівКомпозит*/ ПартіяТоварівКомпозит,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*Серія*/ Серія,
                /*Склад*/ Склад,
                /*Рядок*/ Рядок,
                /*Кількість*/ Кількість,
                /*Собівартість*/ Собівартість,
                /*СписанаСобівартість*/ СписанаСобівартість,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*Організація*/ typeof(string),
                /*ПартіяТоварівКомпозит*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*Серія*/ typeof(string),
                /*Склад*/ typeof(string),
                /*Рядок*/ typeof(string),
                /*Кількість*/ typeof(string),
                /*Собівартість*/ typeof(string),
                /*СписанаСобівартість*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Партія", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ПартіяТоварівКомпозит*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Серія", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*Серія*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true, SortColumnId = 10 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Рядок", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true, SortColumnId = 11 } ); /*Рядок*/
            treeView.AppendColumn(new TreeViewColumn("Кількість", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true, SortColumnId = 12 } ); /*Кількість*/
            treeView.AppendColumn(new TreeViewColumn("Собівартість", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true, SortColumnId = 13 } ); /*Собівартість*/
            treeView.AppendColumn(new TreeViewColumn("Списана собівартість", new CellRendererText() { Xpad = 4 }, "text", 14) { MinWidth = 20, Resizable = true, SortColumnId = 14 } ); /*СписанаСобівартість*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.ПартіїТоварів_RecordsSet ПартіїТоварів_RecordsSet = new РегістриНакопичення.ПартіїТоварів_RecordsSet();
             ПартіїТоварів_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ПартіїТоварів_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await ПартіїТоварів_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (ПартіїТоварів_RecordsSet.Record record in ПартіїТоварів_RecordsSet.Records)
            {
                ПартіїТоварів_Записи row = new ПартіїТоварів_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    Організація = record.Організація.Назва,
                        ПартіяТоварівКомпозит = record.ПартіяТоварівКомпозит.Назва,
                        Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        Серія = record.Серія.Назва,
                        Склад = record.Склад.Назва,
                        Рядок = record.Рядок.ToString() ?? "",
                        Кількість = record.Кількість.ToString() ?? "",
                        Собівартість = record.Собівартість.ToString() ?? "",
                        СписанаСобівартість = record.СписанаСобівартість.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "Продажі"
    
      
    public class Продажі_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string Організація = "";
        string Склад = "";
        string Контрагент = "";
        string Договір = "";
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Кількість = "";
        string Сума = "";
        string Дохід = "";
        string Собівартість = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*Організація*/ Організація,
                /*Склад*/ Склад,
                /*Контрагент*/ Контрагент,
                /*Договір*/ Договір,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*Кількість*/ Кількість,
                /*Сума*/ Сума,
                /*Дохід*/ Дохід,
                /*Собівартість*/ Собівартість,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*Організація*/ typeof(string),
                /*Склад*/ typeof(string),
                /*Контрагент*/ typeof(string),
                /*Договір*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*Кількість*/ typeof(string),
                /*Сума*/ typeof(string),
                /*Дохід*/ typeof(string),
                /*Собівартість*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Організація", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Організація*/
            treeView.AppendColumn(new TreeViewColumn("Склад", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*Склад*/
            treeView.AppendColumn(new TreeViewColumn("Контрагент", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Контрагент*/
            treeView.AppendColumn(new TreeViewColumn("Договір", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*Договір*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true, SortColumnId = 10 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Кількість", new CellRendererText() { Xpad = 4 }, "text", 11) { MinWidth = 20, Resizable = true, SortColumnId = 11 } ); /*Кількість*/
            treeView.AppendColumn(new TreeViewColumn("Сума", new CellRendererText() { Xpad = 4 }, "text", 12) { MinWidth = 20, Resizable = true, SortColumnId = 12 } ); /*Сума*/
            treeView.AppendColumn(new TreeViewColumn("Дохід", new CellRendererText() { Xpad = 4 }, "text", 13) { MinWidth = 20, Resizable = true, SortColumnId = 13 } ); /*Дохід*/
            treeView.AppendColumn(new TreeViewColumn("Собівартість", new CellRendererText() { Xpad = 4 }, "text", 14) { MinWidth = 20, Resizable = true, SortColumnId = 14 } ); /*Собівартість*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.Продажі_RecordsSet Продажі_RecordsSet = new РегістриНакопичення.Продажі_RecordsSet();
             Продажі_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) Продажі_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await Продажі_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Продажі_RecordsSet.Record record in Продажі_RecordsSet.Records)
            {
                Продажі_Записи row = new Продажі_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    Організація = record.Організація.Назва,
                        Склад = record.Склад.Назва,
                        Контрагент = record.Контрагент.Назва,
                        Договір = record.Договір.Назва,
                        Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        Кількість = record.Кількість.ToString() ?? "",
                        Сума = record.Сума.ToString() ?? "",
                        Дохід = record.Дохід.ToString() ?? "",
                        Собівартість = record.Собівартість.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
    #region REGISTER "ТовариВКомірках"
    
      
    public class ТовариВКомірках_Записи : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        
        string Номенклатура = "";
        string ХарактеристикаНоменклатури = "";
        string Пакування = "";
        string Комірка = "";
        string Серія = "";
        string ВНаявності = "";

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                /*Номенклатура*/ Номенклатура,
                /*ХарактеристикаНоменклатури*/ ХарактеристикаНоменклатури,
                /*Пакування*/ Пакування,
                /*Комірка*/ Комірка,
                /*Серія*/ Серія,
                /*ВНаявності*/ ВНаявності,
                 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                /*Номенклатура*/ typeof(string),
                /*ХарактеристикаНоменклатури*/ typeof(string),
                /*Пакування*/ typeof(string),
                /*Комірка*/ typeof(string),
                /*Серія*/ typeof(string),
                /*ВНаявності*/ typeof(string),
                
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 } ); /*Номенклатура*/
            treeView.AppendColumn(new TreeViewColumn("Характеристика", new CellRendererText() { Xpad = 4 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6 } ); /*ХарактеристикаНоменклатури*/
            treeView.AppendColumn(new TreeViewColumn("Пакування", new CellRendererText() { Xpad = 4 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7 } ); /*Пакування*/
            treeView.AppendColumn(new TreeViewColumn("Комірка", new CellRendererText() { Xpad = 4 }, "text", 8) { MinWidth = 20, Resizable = true, SortColumnId = 8 } ); /*Комірка*/
            treeView.AppendColumn(new TreeViewColumn("Серія", new CellRendererText() { Xpad = 4 }, "text", 9) { MinWidth = 20, Resizable = true, SortColumnId = 9 } ); /*Серія*/
            treeView.AppendColumn(new TreeViewColumn("В наявності", new CellRendererText() { Xpad = 4 }, "text", 10) { MinWidth = 20, Resizable = true, SortColumnId = 10 } ); /*ВНаявності*/
            
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true, bool position_last = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.ТовариВКомірках_RecordsSet ТовариВКомірках_RecordsSet = new РегістриНакопичення.ТовариВКомірках_RecordsSet();
             ТовариВКомірках_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) ТовариВКомірках_RecordsSet.QuerySelect.Where = (List<Where>)where;

            /* Read */
            await ТовариВКомірках_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (ТовариВКомірках_RecordsSet.Record record in ТовариВКомірках_RecordsSet.Records)
            {
                ТовариВКомірках_Записи row = new ТовариВКомірках_Записи
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    Номенклатура = record.Номенклатура.Назва,
                        ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури.Назва,
                        Пакування = record.Пакування.Назва,
                        Комірка = record.Комірка.Назва,
                        Серія = record.Серія.Назва,
                        ВНаявності = record.ВНаявності.ToString() ?? "",
                        
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                else if (CurrentPath != null)
                    treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
            }
        }
    }
	    
    #endregion
    
}

  