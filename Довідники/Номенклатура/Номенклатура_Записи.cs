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

для виводу списку

*/

using Gtk;
using AccountingSoftware;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade_1_0.Довідники.ТабличніСписки
{
    public class Номенклатура_Записи
    {
        string Image = "images/doc.png";
        string ID = "";

        string Код = "";
        string Назва = "";
        string ОдиницяВиміру = "";
        string ТипНоменклатури = "";
        string Залишок = "";
        string ЗалишокВКомірках = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, ОдиницяВиміру, ТипНоменклатури, Залишок, ЗалишокВКомірках };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* ОдиницяВиміру */
            , typeof(string) /* ТипНоменклатури */
            , typeof(string)  /* Залишок */
            , typeof(string)  /* ЗалишокВКомірках */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 }); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 }); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Пакування", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 }); /*ОдиницяВиміру*/
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText() { Xpad = 4 }, "text", 5) { SortColumnId = 5 }); /*ТипНоменклатури*/
            
            treeView.AppendColumn(new TreeViewColumn("Залишок", new CellRendererText() { Xpad = 4, Xalign = 1 }, "text", 6) { SortColumnId = 6, Alignment = 1 }); /*Залишок*/
            treeView.AppendColumn(new TreeViewColumn("В комірках", new CellRendererText() { Xpad = 4, Xalign = 1 }, "text", 7) { SortColumnId = 7, Alignment = 1 }); /*ЗалишокВКомірках*/

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
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

            /* Field Залишок */
            Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
              new NameValue<string>(@$"
(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN
	(
        WITH Залишки AS 
        (
            SELECT
                ТовариНаСкладах.{ТовариНаСкладах_Підсумки_TablePart.Номенклатура} AS Номенклатура,
                SUM(ТовариНаСкладах.{ТовариНаСкладах_Підсумки_TablePart.ВНаявності} ) AS ВНаявності
            FROM
                {ТовариНаСкладах_Підсумки_TablePart.TABLE} AS ТовариНаСкладах
            WHERE
                ТовариНаСкладах.{ТовариНаСкладах_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid
            GROUP BY Номенклатура
        )
        SELECT 
            ROUND(ВНаявності, 1) AS ВНаявності 
        FROM 
            Залишки
    )
END)
", "salishok"));

            /* Field ЗалишокВКомірках */
            Номенклатура_Select.QuerySelect.FieldAndAlias.Add(
              new NameValue<string>(@$"
(CASE WHEN {Довідники.Номенклатура_Const.TABLE}.{Довідники.Номенклатура_Const.ТипНоменклатури} = {(int)Перелічення.ТипиНоменклатури.Товар} THEN
	(
        WITH Залишки AS 
        (
            SELECT
                ТовариВКомірках.{ТовариВКомірках_Підсумки_TablePart.Номенклатура} AS Номенклатура,
                SUM(ТовариВКомірках.{ТовариВКомірках_Підсумки_TablePart.ВНаявності} ) AS ВНаявності
            FROM
                {ТовариВКомірках_Підсумки_TablePart.TABLE} AS ТовариВКомірках
            WHERE
                ТовариВКомірках.{ТовариВКомірках_Підсумки_TablePart.Номенклатура} = {Довідники.Номенклатура_Const.TABLE}.uid
            GROUP BY Номенклатура
        )
        SELECT 
            ROUND(ВНаявності, 1) AS ВНаявності 
        FROM 
            Залишки
    )
END)
", "salishok_v_komirkach"));

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
                        Код = cur.Fields?[Номенклатура_Const.Код]?.ToString() ?? "", /**/
                        Назва = cur.Fields?[Номенклатура_Const.Назва]?.ToString() ?? "", /**/
                        ТипНоменклатури = ((Перелічення.ТипиНоменклатури)(cur.Fields?[Номенклатура_Const.ТипНоменклатури]! != DBNull.Value ? cur.Fields?[Номенклатура_Const.ТипНоменклатури]! : 0)).ToString(), /**/
                        Залишок = cur.Fields?["salishok"]?.ToString() ?? "", /**/
                        ЗалишокВКомірках = cur.Fields?["salishok_v_komirkach"]?.ToString() ?? ""
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

}