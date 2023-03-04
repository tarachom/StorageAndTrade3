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
                    NameValue<int> valueDataColumn = (NameValue<int>)treeColumn.Data["Column"]!;
                    string valueCell = treeView.Model.GetValue(iter, valueDataColumn.Value).ToString()!;

                    //
                    //Вміст колонки може бути двох видів:
                    // 1. Guid
                    // 2. Guid:ВидДокументу
                    // 

                    string uid, uid_text = "";
                    if (valueCell.IndexOf(":") != -1)
                    {
                        // 2. Guid:ВидДокументу
                        string[] uid_and_text = valueCell.Split(":", StringSplitOptions.None);
                        uid = uid_and_text[0];
                        uid_text = uid_and_text[1];
                    }
                    else
                    {
                        // 1. Guid
                        uid = valueCell;
                    }

                    Guid guid;
                    if (!Guid.TryParse(uid, out guid))
                        return;

                    UnigueID unigueID = new UnigueID(uid);
                    if (unigueID.IsEmpty())
                        return;

                    if (valueDataColumn.Name == "Документ")
                    {
                        ФункціїДляЖурналів.ВідкритиЖурналВідповідноДоВидуДокументу(uid_text, new UnigueID(uid));
                    }
                    else
                    {
                        ФункціїДляДовідників.ВідкритиДовідникВідповідноДоВиду(valueDataColumn.Name, unigueID);
                    }
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
            Dictionary<string, float>? xalignColumns = null)
        {
            for (int i = 0; i < columnsName.Length; i++)
            {
                string columnName = columnsName[i];

                //Видимість колонки
                bool isVisibleColumn = visibleColumns.ContainsKey(columnName);

                //Позиція тексту в колонці (0 .. 1)
                float xalign = xalignColumns != null ?
                    (xalignColumns.ContainsKey(columnName) ? xalignColumns[columnName] : 0) : 0;

                TreeViewColumn treeColumn = new TreeViewColumn(
                    isVisibleColumn ? visibleColumns[columnName] : columnName,
                    new CellRendererText() { Xalign = xalign }, "text", i)
                { Visible = isVisibleColumn, Alignment = xalign };

                if (dataColumns != null && dataColumns.ContainsKey(columnName))
                {
                    string dataColumName = dataColumns[columnName];

                    for (int j = 0; j < columnsName.Length; j++)
                    {
                        if (dataColumName == columnsName[j])
                        {
                            treeColumn.Data.Add("Column", new NameValue<int>(columnName, j));
                            break;
                        }
                    }
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
                        Program.GeneralForm!.NotebookCurrentPageToCode(notebook, codePage);

                        onRefreshAction.Invoke(refreshParam, true);

                        Program.GeneralForm!.NotebookCloseTabToCode(notebook, codePage);
                    }
                }
            );
        }

        #endregion
    }
}