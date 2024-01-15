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

using Gtk;
using AccountingSoftware;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade_1_0.Довідники.ТабличніСписки
{

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
                DeletionLabel ? Іконки.ДляТабличногоСписку.Delete : Іконки.ДляТабличногоСписку.Normal,
                ID,
                /*Код*/ Код,
                /*Назва*/ Назва,
                /*Валюта*/ Валюта,
                Залишок

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
                /* Залишок */ typeof(string)  

            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { MinWidth = 20, Resizable = true, SortColumnId = 2 }); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { MinWidth = 20, Resizable = true, SortColumnId = 3 }); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { MinWidth = 20, Resizable = true, SortColumnId = 4 }); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Залишок", new CellRendererText() { Xpad = 4, Xalign = 1 }, "text", 5) { MinWidth = 20, Resizable = true, SortColumnId = 5, Alignment = 1 }); /*Залишок*/

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

            /* Field Залишок */
            Каси_Select.QuerySelect.FieldAndAlias.Add(
              new NameValue<string>(@$"
(
    SELECT
        ROUND(РухКоштів.{РухКоштів_Підсумки_TablePart.Сума}, 2) AS Сума
    FROM
        {РухКоштів_Підсумки_TablePart.TABLE} AS РухКоштів
    WHERE
        РухКоштів.{РухКоштів_Підсумки_TablePart.Каса} = {Довідники.Каси_Const.TABLE}.uid
)
", "salishok"));


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
                        Валюта = Fields["join_tab_1_field_1"].ToString() ?? "", /*Валюта*/
                        Код = Fields[Каси_Const.Код].ToString() ?? "", /**/
                        Назва = Fields[Каси_Const.Назва].ToString() ?? "", /**/
                        Залишок = Fields["salishok"].ToString() ?? "", /**/

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