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


    public class Каси_Записи
    {
        string Image = "images/doc.png";
        string ID = "";

        string Код = "";
        string Назва = "";
        string Валюта = "";
        string Залишок = "";

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID 
            /* */ , Код, Назва, Валюта, Залишок };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            , typeof(string) /* Код */
            , typeof(string) /* Назва */
            , typeof(string) /* Валюта */
            , typeof(string)  /* Залишок */
            );

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf() { Ypad = 4 }, "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
            treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 2) { SortColumnId = 2 }); /*Код*/
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 3) { SortColumnId = 3 }); /*Назва*/
            treeView.AppendColumn(new TreeViewColumn("Валюта", new CellRendererText() { Xpad = 4 }, "text", 4) { SortColumnId = 4 }); /*Валюта*/
            treeView.AppendColumn(new TreeViewColumn("Залишок", new CellRendererText() { Xpad = 4, Xalign = 1 }, "text", 5) { SortColumnId = 5, Alignment = 1 }); /*Залишок*/

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
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
                        Назва = cur.Fields?[Каси_Const.Назва]?.ToString() ?? "", /**/
                        Залишок = cur.Fields?["salishok"]?.ToString() ?? "", /**/
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