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

namespace StorageAndTrade
{
    class ФункціїДляЗвітів
    {
        public static void OpenPageDirectoryOrDocument(object sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
            {
                TreeView treeView = (TreeView)sender;

                TreePath itemPath;
                TreeViewColumn treeColumn;

                treeView.GetCursor(out itemPath, out treeColumn);

                if (treeColumn.Data.ContainsKey("Column"))
                {
                    TreeIter iter;
                    treeView.Model.GetIter(out iter, itemPath);

                    //
                    //NameValue<int> 
                    //  @Name це назва колонки 
                    //  @Value - номер колонки з даними
                    //

                    //Два ключі Column і ColumnDataNum йдуть в парі
                    string columnName = treeColumn.Data["Column"]?.ToString() ?? "";
                    int columnDataNum = int.Parse(treeColumn.Data["ColumnDataNum"]?.ToString() ?? "0");

                    //Тип даних (Документи.*, Документи.<Назва>, Довідники.*, Довідники.<Назва>)
                    string columnDataType = treeColumn.Data["ColumnDataType"]?.ToString() ?? "";

                    string valueDataCell = treeView.Model.GetValue(iter, columnDataNum).ToString()!;

                    //
                    //Вміст колонки даних може бути двох видів:
                    // 1. Guid
                    // 2. Guid:Вид (Документу або Довідника)
                    // 

                    string uid, vyd = "";
                    if (valueDataCell.IndexOf(":") != -1)
                    {
                        // 2. Guid:Вид
                        string[] uid_and_text = valueDataCell.Split(":");
                        uid = uid_and_text[0];
                        vyd = uid_and_text[1];
                    }
                    else
                    {
                        // 1. Guid
                        uid = valueDataCell;
                    }

                    Guid guid;
                    if (!Guid.TryParse(uid, out guid))
                        return;

                    UnigueID unigueID = new UnigueID(uid);
                    if (unigueID.IsEmpty())
                        return;

                    //
                    //Тип даних (Документи.*, Документи.<Назва>, Довідники.*, Довідники.<Назва>)
                    //

                    string pointer, type = "";
                    if (columnDataType.IndexOf(".") != -1)
                    {
                        string[] pointer_and_type = columnDataType.Split(".");
                        pointer = pointer_and_type[0];
                        type = pointer_and_type[1];
                    }
                    else
                        return;

                    if ((type == "" || type == "*") && vyd != "")
                        type = vyd;

                    if (pointer == "Документи")
                        ФункціїДляДокументів.ВідкритиДокументВідповідноДоВиду(type, new UnigueID(uid));
                    else if (pointer == "Довідники")
                        ФункціїДляДовідників.ВідкритиДовідникВідповідноДоВиду(type, unigueID);
                }
            }
        }

        public static void СтворитиМодельДаних(out ListStore listStore, string[] columnsName)
        {
            Type[] type = new Type[columnsName.Length];
            for (int i = 0; i < columnsName.Length; i++)
                type[i] = typeof(string);

            listStore = new ListStore(type);
        }

        /*
        Заготовка на майбутнє.
        Додати новий параметр з набором даних для колонки: наприклад період звіту
        Ці дані можна використовувати для відкриття журналу з даним періодом
        */

        public static void СтворитиКолонкиДляДерева(TreeView treeView,
            string[] columnsName,
            Dictionary<string, string> visibleColumns,
            Dictionary<string, string>? dataColumns = null,
            Dictionary<string, string>? typeDataColumns = null,
            Dictionary<string, float>? xalignColumns = null,
            Dictionary<string, TreeCellDataFunc>? columnCellDataFunc = null)
        {
            for (int i = 0; i < columnsName.Length; i++)
            {
                string columnName = columnsName[i];

                //Видимість колонки
                bool visible = visibleColumns.ContainsKey(columnName);

                //Назва колонки
                string title = visible ? visibleColumns[columnName] : columnName;

                //Позиція тексту в колонці (0 .. 1)
                float xalign = visible ? (xalignColumns != null && xalignColumns.ContainsKey(columnName) ? xalignColumns[columnName] : 0) : 0;

                CellRendererText cellRendererText = new CellRendererText() { Xalign = xalign };
                TreeViewColumn treeColumn = new TreeViewColumn(title, cellRendererText, "text", i)
                {
                    Visible = visible,
                    Alignment = xalign,
                    Resizable = true,
                    MinWidth = 20
                };

                //
                // Привязка колонки з даними до видимої колонки
                //

                if (visible && (dataColumns != null && dataColumns.ContainsKey(columnName)))
                {
                    string dataColumName = dataColumns[columnName];
                    for (int j = 0; j < columnsName.Length; j++)
                        if (dataColumName == columnsName[j])
                        {
                            treeColumn.Data.Add("Column", columnName);
                            treeColumn.Data.Add("ColumnDataNum", j);

                            //Тип колонки з даними
                            if (typeDataColumns != null && typeDataColumns.ContainsKey(columnName))
                                treeColumn.Data.Add("ColumnDataType", typeDataColumns[columnName]);
                            break;
                        }
                }

                //
                //Функція обробки ячейки для видимої колонки
                //

                if (visible && columnCellDataFunc != null && columnCellDataFunc.ContainsKey(columnName))
                {
                    treeColumn.Data.Add("CellDataFunc", columnName);
                    treeColumn.SetCellDataFunc(cellRendererText, columnCellDataFunc[columnName]);
                }

                treeView.AppendColumn(treeColumn);
            }

            //Додаткова колонка пустишка для заповнення простору
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ЗаповнитиМодельДаними(ListStore listStore, string[] columnsName, List<Dictionary<string, object>> listRow)
        {
            foreach (Dictionary<string, object> row in listRow)
            {
                string[] values = new string[columnsName.Length];
                for (int i = 0; i < columnsName.Length; i++)
                {
                    string column = columnsName[i];
                    values[i] = row[column]?.ToString() ?? "";
                }

                listStore.AppendValues(values);
            }
        }

        #region NotebookReport

        /// <summary>
        /// Створити сторінку в блокноті звіту
        /// </summary>
        /// <param name="notebook">Блокнот</param>
        /// <param name="tabName">Назва сторінки</param>
        /// <param name="codePage">Код</param>
        /// <param name="pageWidget">Віджет</param>
        /// <param name="insertPage">Вставка</param>
        /// <param name="refresh">Процедура обновлення</param>
        public static void CreateNotebookPageReport(Notebook notebook, string tabName, string codePage, System.Func<Widget>? pageWidget,
            bool insertPage = false, System.Action? refresh = null)
        {
            int numPage;
            HBox hBoxLabel = Program.GeneralForm!.CreateLabelPageWidget(tabName, codePage, notebook);

            //Лінк для обновлення звіту
            if (refresh != null)
            {
                LinkButton lbRefresh = new LinkButton("Обновити", " ")
                {
                    Halign = Align.Start,
                    Image = new Image(AppContext.BaseDirectory + "images/refresh.png"),
                    AlwaysShowImage = true,
                    Name = codePage
                };

                lbRefresh.Clicked += (object? sender, EventArgs args) => { refresh.Invoke(); };

                hBoxLabel.PackStart(lbRefresh, false, false, 4);
                hBoxLabel.ShowAll();
            }

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In, Name = codePage };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            if (insertPage)
                numPage = notebook.InsertPage(scroll, hBoxLabel, notebook.CurrentPage);
            else
                numPage = notebook.AppendPage(scroll, hBoxLabel);

            //Переміщення сторінок
            notebook.SetTabReorderable(scroll, true);

            if (pageWidget != null)
                scroll.Add((Widget)pageWidget.Invoke());

            notebook.ShowAll();
            notebook.CurrentPage = numPage;
        }

        /// <summary>
        /// Створити сторінку звіту
        /// </summary>
        /// <param name="notebook">Блокнот</param>
        /// <param name="caption">Назва сторінки</param>
        /// <param name="wgTree">Дерево з даними</param>
        /// <param name="onRefreshAction">Процедура формування звіту</param>
        /// <param name="refreshParam">Параметри процедури формування звіту</param>
        /// <param name="refreshPage">Признак обновлення сторінки</param>
        public static void CreateReportNotebookPage(Notebook notebook, string caption, Widget wgHead, Widget wgTree,
            System.Action<object?, bool>? onRefreshAction = null, object? refreshParam = null, bool refreshPage = false)
        {
            string codePage = Guid.NewGuid().ToString();

            VBox vBox = new VBox();

            ScrolledWindow scrol = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrol.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrol.Add(wgTree);

            HBox hBoxHead = new HBox();
            hBoxHead.PackStart(wgHead, false, false, 5);
            vBox.PackStart(hBoxHead, false, false, 5);

            HBox hBox = new HBox();
            hBox.PackStart(scrol, true, true, 5);
            vBox.PackStart(hBox, true, true, 5);

            CreateNotebookPageReport(notebook, caption, codePage,
                () => { return vBox; }, refreshPage,

                /* Обновлення сторінки */
                () =>
                {
                    if (onRefreshAction != null)
                    {
                        Program.GeneralForm!.CurrentNotebookPageToCode(notebook, codePage);

                        onRefreshAction.Invoke(refreshParam, true);

                        Program.GeneralForm!.CloseNotebookPageToCode(notebook, codePage);
                    }
                }
            );
        }

        #endregion

        #region ФункціяДляКолонки

        public static void ФункціяДляКолонкиБазоваДляЧисла(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            CellRendererText cellText = (CellRendererText)cell;
            if (column.Data.Contains("CellDataFunc"))
                cellText.Foreground = "green";
        }

        public static void ФункціяДляКолонкиВідємнеЧислоЧервоним(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            CellRendererText cellText = (CellRendererText)cell;
            if (column.Data.Contains("CellDataFunc"))
            {
                float result;
                if (float.TryParse(cellText.Text, out result))
                    cellText.Foreground = (result >= 0) ? "green" : "red";
            }
        }

        #endregion
    }
}