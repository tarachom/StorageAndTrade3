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
                        switch (uid_text)
                        {
                            case "ЗамовленняКлієнта":
                                {
                                    ЗамовленняКлієнта page = new ЗамовленняКлієнта() { SelectPointerItem = new ЗамовленняКлієнта_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Замовлення клієнтів", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                            case "РахунокФактура":
                                {
                                    РахунокФактура page = new РахунокФактура() { SelectPointerItem = new РахунокФактура_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Рахунок фактура", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                            case "ЗамовленняПостачальнику":
                                {
                                    ЗамовленняПостачальнику page = new ЗамовленняПостачальнику() { SelectPointerItem = new ЗамовленняПостачальнику_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Замовлення постачальнику", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                            case "РеалізаціяТоварівТаПослуг":
                                {
                                    РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг() { SelectPointerItem = new РеалізаціяТоварівТаПослуг_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Реалізація товарів та послуг", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                            case "ПоступленняТоварівТаПослуг":
                                {
                                    ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг() { SelectPointerItem = new ПоступленняТоварівТаПослуг_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Поступлення товарів та послуг", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                            case "РозхіднийКасовийОрдер":
                                {
                                    РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер() { SelectPointerItem = new РозхіднийКасовийОрдер_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Розхідний касовий ордер", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                            case "ПрихіднийКасовийОрдер":
                                {
                                    ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер() { SelectPointerItem = new ПрихіднийКасовийОрдер_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Прихідний касовий ордер", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                            case "ПереміщенняТоварів":
                                {
                                    ПереміщенняТоварів page = new ПереміщенняТоварів() { SelectPointerItem = new ПереміщенняТоварів_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Переміщення товарів", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                            case "ПоверненняТоварівВідКлієнта":
                                {
                                    ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта() { SelectPointerItem = new ПоверненняТоварівВідКлієнта_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Повернення товарів від клієнта", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                            case "ПоверненняТоварівПостачальнику":
                                {
                                    ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику() { SelectPointerItem = new ПоверненняТоварівПостачальнику_Pointer(unigueID) };
                                    Program.GeneralForm?.CreateNotebookPage("Повернення товарів постачальнику", () => { return page; });
                                    page.LoadRecords();
                                    break;
                                }
                        }
                    }
                    else
                    {
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
                            case "Договір_Назва":
                                {
                                    ДоговориКонтрагентів page = new ДоговориКонтрагентів();
                                    page.SelectPointerItem = new ДоговориКонтрагентів_Pointer(unigueID);

                                    Program.GeneralForm?.CreateNotebookPage("Договори контрагентів", () => { return page; });

                                    page.LoadRecords();

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