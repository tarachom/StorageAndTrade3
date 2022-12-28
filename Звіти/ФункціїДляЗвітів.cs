using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

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
                    string uid = treeView.Model.GetValue(iter, valueDataColumn.Value).ToString()!;

                    Guid guid;
                    if (!Guid.TryParse(uid, out guid))
                        return;

                    UnigueID unigueID = new UnigueID(uid);
                    if (unigueID.IsEmpty())
                        return;

                    switch (valueDataColumn.Name)
                    {
                        case "Організація_Назва":
                            {
                                Організації page = new Організації();
                                page.SelectPointerItem = new Організації_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Організації", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case "Номенклатура_Назва":
                            {
                                Номенклатура page = new Номенклатура();
                                page.SelectPointerItem = new Номенклатура_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Номенклатура", () => { return page; });

                                page.LoadTree();

                                break;
                            }
                        case "ХарактеристикаНоменклатури_Назва":
                            {
                                ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();
                                page.SelectPointerItem = new ХарактеристикиНоменклатури_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Характеристика", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case "Серія_Номер":
                            {
                                СеріїНоменклатури page = new СеріїНоменклатури();
                                page.SelectPointerItem = new СеріїНоменклатури_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Серія", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case "Контрагент_Назва":
                            {
                                Контрагенти page = new Контрагенти();
                                page.SelectPointerItem = new Контрагенти_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Контрагенти", () => { return page; });

                                page.LoadTree();

                                break;
                            }
                        case "Валюта_Назва":
                            {
                                Валюти page = new Валюти();
                                page.SelectPointerItem = new Валюти_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Валюти", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case "Каса_Назва":
                            {
                                Каси page = new Каси();
                                page.SelectPointerItem = new Каси_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Каси", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case "Склад_Назва":
                            {
                                Склади page = new Склади();
                                page.SelectPointerItem = new Склади_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Склад", () => { return page; });

                                page.LoadTree();

                                break;
                            }
                        case "ПартіяТоварівКомпозит_Назва":
                            {
                                ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит();
                                page.SelectPointerItem = new ПартіяТоварівКомпозит_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("ПартіяТоварівКомпозит", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case "ЗамовленняКлієнта_Назва":
                            {
                                ЗамовленняКлієнта page = new ЗамовленняКлієнта();
                                page.SelectPointerItem = new ЗамовленняКлієнта_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Замовлення клієнтів", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
                        case "ЗамовленняПостачальнику_Назва":
                            {
                                ЗамовленняПостачальнику page = new ЗамовленняПостачальнику();
                                page.SelectPointerItem = new ЗамовленняПостачальнику_Pointer(unigueID);

                                Program.GeneralForm?.CreateNotebookPage("Замовлення постачальнику", () => { return page; });

                                page.LoadRecords();

                                break;
                            }
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

    }
}