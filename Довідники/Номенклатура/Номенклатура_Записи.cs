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
    public class Номенклатура_Записи : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";

        string Код = "";
        string Назва = "";
        string ОдиницяВиміру = "";
        string ТипНоменклатури = "";
        string Залишок = "";
        string ЗалишокВКомірках = "";

        Array ToArray()
        {
            return new object[]
            {
                DeletionLabel ? Іконки.ДляТабличногоСписку.Delete : Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*ОдиницяВиміру*/ ОдиницяВиміру,
                /*ТипНоменклатури*/ ТипНоменклатури,
                Залишок,
                ЗалишокВКомірках
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
                /* Залишок */ typeof(string),
                /* ЗалишокВКомірках */ typeof(string)

            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 }); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Номенклатура", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 }); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Пакування", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 }); /*ОдиницяВиміру*/
            treeView.AppendColumn(new TreeViewColumn("Тип", new CellRendererText() { Xpad = 4 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5 }); /*ТипНоменклатури*/

            treeView.AppendColumn(new TreeViewColumn("Залишок", new CellRendererText() { Xpad = 4, Xalign = 1 }, "text", 6) { MinWidth = 20, Resizable = true, SortColumnId = 6, Alignment = 1 }); /*Залишок*/
            treeView.AppendColumn(new TreeViewColumn("В комірках", new CellRendererText() { Xpad = 4, Xalign = 1 }, "text", 7) { MinWidth = 20, Resizable = true, SortColumnId = 7, Alignment = 1 }); /*ЗалишокВКомірках*/

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
                        ОдиницяВиміру = Fields["join_tab_1_field_1"].ToString() ?? "", /*ОдиницяВиміру*/
                        Код = Fields[Номенклатура_Const.Код].ToString() ?? "", /**/
                        Назва = Fields[Номенклатура_Const.Назва].ToString() ?? "", /**/
                        ТипНоменклатури = Перелічення.ПсевдонімиПерелічення.ТипиНоменклатури_Alias(((Перелічення.ТипиНоменклатури)(Fields[Номенклатура_Const.ТипНоменклатури] != DBNull.Value ? Fields[Номенклатура_Const.ТипНоменклатури] : 0))), /**/
                        Залишок = Fields["salishok"].ToString() ?? "", /**/
                        ЗалишокВКомірках = Fields["salishok_v_komirkach"].ToString() ?? ""

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

}