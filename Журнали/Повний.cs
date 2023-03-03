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
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    class Журнал_Повний : VBox
    {
        public UnigueID? SelectPointerItem { get; set; }
        public System.Action<Валюти_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();

        public Журнал_Повний() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //Відбір по періоду
            hBoxTop.PackStart(new Label("Період:"), false, false, 5);

            ComboBoxPeriodWhere = ТабличніСписки.Інтерфейс.СписокВідбірПоПеріоду();
            ComboBoxPeriodWhere.Changed += OnComboBoxPeriodWhereChanged;

            hBoxTop.PackStart(ComboBoxPeriodWhere, false, false, 0);

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.Журнали_Повний.Store);
            ТабличніСписки.Журнали_Повний.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton upButton = new ToolButton(Stock.Edit) { TooltipText = "Редагувати" };
            upButton.Clicked += OnEditClick;
            toolbar.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { TooltipText = "Копіювати" };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { TooltipText = "Обновити" };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);
        }

        public void SetValue()
        {
            if ((int)Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
            else
                ComboBoxPeriodWhere.Active = 0;
        }

        public void LoadRecords()
        {
            ТабличніСписки.Журнали_Повний.SelectPointerItem = SelectPointerItem;

            ТабличніСписки.Журнали_Повний.LoadRecords();

            if (ТабличніСписки.Журнали_Повний.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Повний.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.Журнали_Повний.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Повний.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                SelectPointerItem = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                }
            }
        }

        #endregion

        #region ToolBar

        void OnComboBoxPeriodWhereChanged(object? sender, EventArgs args)
        {
            ТабличніСписки.Журнали_Повний.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        void OnEditClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    string type = (string)TreeViewGrid.Model.GetValue(iter, 2);

                    UnigueID unigueID = new UnigueID(uid);

                    
                    switch (type)
                    {
                        case "ЗамовленняКлієнта":
                            {
                                ЗамовленняКлієнта page = new ЗамовленняКлієнта() { SelectPointerItem = new ЗамовленняКлієнта_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Замовлення клієнтів", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "РахунокФактура":
                            {
                                РахунокФактура page = new РахунокФактура() { SelectPointerItem = new РахунокФактура_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Рахунок фактура", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "АктВиконанихРобіт":
                            {
                                АктВиконанихРобіт page = new АктВиконанихРобіт() { SelectPointerItem = new АктВиконанихРобіт_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Акт виконаних робіт", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ЗамовленняПостачальнику":
                            {
                                ЗамовленняПостачальнику page = new ЗамовленняПостачальнику() { SelectPointerItem = new ЗамовленняПостачальнику_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Замовлення постачальнику", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "РеалізаціяТоварівТаПослуг":
                            {
                                РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг() { SelectPointerItem = new РеалізаціяТоварівТаПослуг_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Реалізація товарів та послуг", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ПоступленняТоварівТаПослуг":
                            {
                                ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг() { SelectPointerItem = new ПоступленняТоварівТаПослуг_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Поступлення товарів та послуг", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "РозхіднийКасовийОрдер":
                            {
                                РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер() { SelectPointerItem = new РозхіднийКасовийОрдер_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Розхідний касовий ордер", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ПрихіднийКасовийОрдер":
                            {
                                ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер() { SelectPointerItem = new ПрихіднийКасовийОрдер_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Прихідний касовий ордер", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ПереміщенняТоварів":
                            {
                                ПереміщенняТоварів page = new ПереміщенняТоварів() { SelectPointerItem = new ПереміщенняТоварів_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Переміщення товарів", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ПоверненняТоварівВідКлієнта":
                            {
                                ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта() { SelectPointerItem = new ПоверненняТоварівВідКлієнта_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Повернення товарів від клієнта", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ПоверненняТоварівПостачальнику":
                            {
                                ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику() { SelectPointerItem = new ПоверненняТоварівПостачальнику_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Повернення товарів постачальнику", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ВнутрішнєСпоживанняТоварів":
                            {
                                ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів() { SelectPointerItem = new ВнутрішнєСпоживанняТоварів_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Внутрішнє споживання товарів", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ВведенняЗалишків":
                            {
                                ВведенняЗалишків page = new ВведенняЗалишків() { SelectPointerItem = new ВведенняЗалишків_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Введення залишків", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "РозміщенняТоварівНаСкладі":
                            {
                                РозміщенняТоварівНаСкладі page = new РозміщенняТоварівНаСкладі() { SelectPointerItem = new РозміщенняТоварівНаСкладі_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Розміщення товарів на складі", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ЗбіркаТоварівНаСкладі":
                            {
                                ЗбіркаТоварівНаСкладі page = new ЗбіркаТоварівНаСкладі() { SelectPointerItem = new ЗбіркаТоварівНаСкладі_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Збірка товарів на складі", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                        case "ПереміщенняТоварівНаСкладі":
                            {
                                ПереміщенняТоварівНаСкладі page = new ПереміщенняТоварівНаСкладі() { SelectPointerItem = new ПереміщенняТоварівНаСкладі_Pointer(unigueID) };
                                Program.GeneralForm?.CreateNotebookPage("Переміщення товарів на складі", () => { return page; }, true);
                                page.LoadRecords();
                                break;
                            }
                    }

                }
            }
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        void OnDeleteClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                if (Message.Request(Program.GeneralForm, "Видалити?") == ResponseType.Yes)
                {
                    TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        TreeViewGrid.Model.GetIter(out iter, itemPath);

                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    }

                    LoadRecords();
                }
            }
        }

        void OnCopyClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                if (Message.Request(Program.GeneralForm, "Копіювати?") == ResponseType.Yes)
                {
                    TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        TreeViewGrid.Model.GetIter(out iter, itemPath);

                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);


                    }

                    LoadRecords();
                }
            }
        }

        #endregion

    }
}