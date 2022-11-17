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
                                Program.GeneralForm?.CreateNotebookPage("Довідник: Організації", () =>
                                {
                                    Організації page = new Організації();
                                    page.SelectPointerItem = new Організації_Pointer(unigueID);
                                    page.LoadRecords();

                                    return page;
                                });

                                break;
                            }
                        case "Номенклатура_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Довідник: Номенклатура", () =>
                                {
                                    Номенклатура page = new Номенклатура();
                                    page.SelectPointerItem = new Номенклатура_Pointer(unigueID);
                                    page.LoadTree();

                                    return page;
                                });

                                break;
                            }
                        case "ХарактеристикаНоменклатури_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Довідник: Характеристика", () =>
                                {
                                    ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();
                                    page.SelectPointerItem = new ХарактеристикиНоменклатури_Pointer(unigueID);
                                    page.LoadRecords();

                                    return page;
                                });

                                break;
                            }
                        case "Серія_Номер":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Довідник: Серія", () =>
                                {
                                    СеріїНоменклатури page = new СеріїНоменклатури();
                                    page.SelectPointerItem = new СеріїНоменклатури_Pointer(unigueID);
                                    page.LoadRecords();

                                    return page;
                                });

                                break;
                            }
                        case "Контрагент_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Довідник: Контрагенти", () =>
                                {
                                    Контрагенти page = new Контрагенти();
                                    page.SelectPointerItem = new Контрагенти_Pointer(unigueID);
                                    page.LoadTree();

                                    return page;
                                });

                                break;
                            }
                        case "Валюта_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Довідник: Валюти", () =>
                                {
                                    Валюти page = new Валюти();
                                    page.SelectPointerItem = new Валюти_Pointer(unigueID);
                                    page.LoadRecords();

                                    return page;
                                });

                                break;
                            }
                        case "Каса_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Довідник: Каси", () =>
                                {
                                    Каси page = new Каси();
                                    page.SelectPointerItem = new Каси_Pointer(unigueID);
                                    page.LoadRecords();

                                    return page;
                                });

                                break;
                            }
                        case "Склад_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Довідник: Склад", () =>
                                {
                                    Склади page = new Склади();
                                    page.SelectPointerItem = new Склади_Pointer(unigueID);
                                    page.LoadTree();

                                    return page;
                                });

                                break;
                            }
                        case "ПартіяТоварівКомпозит_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Довідник: ПартіяТоварівКомпозит", () =>
                                {
                                    ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит();
                                    page.SelectPointerItem = new ПартіяТоварівКомпозит_Pointer(unigueID);
                                    page.LoadRecords();

                                    return page;
                                });

                                break;
                            }
                        case "ЗамовленняКлієнта_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Документ: Замовлення клієнтів", () =>
                                {
                                    ЗамовленняКлієнта page = new ЗамовленняКлієнта();
                                    page.SelectPointerItem = new ЗамовленняКлієнта_Pointer(unigueID);
                                    page.LoadRecords();

                                    return page;
                                });

                                break;
                            }
                        case "ЗамовленняПостачальнику_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Документ: Замовлення постачальнику", () =>
                                {
                                    ЗамовленняПостачальнику page = new ЗамовленняПостачальнику();
                                    page.SelectPointerItem = new ЗамовленняПостачальнику_Pointer(unigueID);
                                    page.LoadRecords();

                                    return page;
                                });

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
            Dictionary<string, string> dataColumns)
        {
            for (int i = 0; i < columnsName.Length; i++)
            {
                string columnName = columnsName[i];

                bool isVisibleColumn = visibleColumns.ContainsKey(columnName);

                TreeViewColumn treeColumn = new TreeViewColumn(isVisibleColumn ? visibleColumns[columnName] : columnName, new CellRendererText(), "text", i) { Visible = isVisibleColumn };

                if (dataColumns.ContainsKey(columnName))
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