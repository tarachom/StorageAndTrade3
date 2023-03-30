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

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПоступленняТоварівТаПослуг : VBox
    {
        public ПоступленняТоварівТаПослуг_Pointer? SelectPointerItem { get; set; }
        public ПоступленняТоварівТаПослуг_Pointer? DocumentPointerItem { get; set; }
        public System.Action<ПоступленняТоварівТаПослуг_Pointer>? CallBack_OnSelectPointer { get; set; }
        public Перелічення.ТипПеріодуДляЖурналівДокументів PeriodWhere { get; set; } = 0;

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public ПоступленняТоварівТаПослуг(bool IsSelectPointer = false) : base()
        {
            BorderWidth = 0;

            //Кнопки
            HBox hBoxButton = new HBox();
            PackStart(hBoxButton, false, false, 10);

            //Як форма відкрита для вибору
            if (IsSelectPointer)
            {
                Button bEmptyPointer = new Button("Вибрати пустий елемент");
                bEmptyPointer.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new ПоступленняТоварівТаПослуг_Pointer());

                    Program.GeneralForm?.CloseCurrentPageNotebook();
                };

                hBoxButton.PackStart(bEmptyPointer, false, false, 10);
            }

            //Відбір по періоду
            hBoxButton.PackStart(new Label("Період:"), false, false, 5);

            ComboBoxPeriodWhere = ТабличніСписки.Інтерфейс.СписокВідбірПоПеріоду();
            ComboBoxPeriodWhere.Changed += OnComboBoxPeriodWhereChanged;

            hBoxButton.PackStart(ComboBoxPeriodWhere, false, false, 0);

            //Пошук 2
            hBoxButton.PackStart(ПошукПовнотекстовий, false, false, 2);
            ПошукПовнотекстовий.Select = LoadRecords_OnSearch;
            ПошукПовнотекстовий.Clear = () => { OnComboBoxPeriodWhereChanged(null, new EventArgs()); };

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.Store);
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.ButtonReleaseEvent += OnButtonReleaseEvent;
            TreeViewGrid.KeyReleaseEvent += OnKeyReleaseEvent;
            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        #region Toolbar & Menu

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton addButton = new ToolButton(Stock.Add) { TooltipText = "Додати" };
            addButton.Clicked += OnAddClick;
            toolbar.Add(addButton);

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

            //Separator
            ToolItem toolItemSeparator = new ToolItem();
            toolItemSeparator.Add(new Separator(Orientation.Horizontal));
            toolbar.Add(toolItemSeparator);

            MenuToolButton provodkyButton = new MenuToolButton(Stock.Find) { Label = "Проводки", IsImportant = true };
            provodkyButton.Clicked += OnReportSpendTheDocumentClick;
            provodkyButton.Menu = ToolbarProvodkySubMenu();
            toolbar.Add(provodkyButton);

            MenuToolButton naOsnoviButton = new MenuToolButton(Stock.New) { Label = "Ввести на основі", IsImportant = true };
            naOsnoviButton.Clicked += OnNaOsnoviClick;
            naOsnoviButton.Menu = ToolbarNaOsnoviSubMenu();
            toolbar.Add(naOsnoviButton);

            MenuToolButton printingButton = new MenuToolButton(Stock.Print) { TooltipText = "Друк" };
            printingButton.Clicked += OnPrintingClick;
            printingButton.Menu = ToolbarPrintingSubMenu();
            toolbar.Add(printingButton);

            MenuToolButton exportButton = new MenuToolButton(Stock.Convert) { Label = "Експорт", IsImportant = true };
            exportButton.Clicked += OnExportClick;
            exportButton.Menu = ToolbarExportSubMenu();
            toolbar.Add(exportButton);
        }

        Menu ToolbarProvodkySubMenu()
        {
            Menu Menu = new Menu();

            MenuItem spendTheDocumentButton = new MenuItem("Провести документ");
            spendTheDocumentButton.Activated += OnSpendTheDocument;
            Menu.Append(spendTheDocumentButton);

            MenuItem clearSpendButton = new MenuItem("Відмінити проведення");
            clearSpendButton.Activated += OnClearSpend;
            Menu.Append(clearSpendButton);

            Menu.ShowAll();

            return Menu;
        }

        Menu ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            MenuItem newDocKasovyiOrderButton = new MenuItem("Розхідний касовий ордер");
            newDocKasovyiOrderButton.Activated += OnNewDocNaOsnovi_РозхіднийКасовийОрдер;
            Menu.Append(newDocKasovyiOrderButton);

            MenuItem newDocPovernenjaPostachalnykuButton = new MenuItem("Повернення товарів постачальнику");
            newDocPovernenjaPostachalnykuButton.Activated += OnNewDocNaOsnovi_ПоверненняТоварівПостачальнику;
            Menu.Append(newDocPovernenjaPostachalnykuButton);

            MenuItem newDocRozmisctenjaNaSkaldyButton = new MenuItem("Розміщення товарів на складі");
            newDocRozmisctenjaNaSkaldyButton.Activated += OnNewDocNaOsnovi_РозміщенняТоварівНаСкладі;
            Menu.Append(newDocRozmisctenjaNaSkaldyButton);

            MenuItem newDocVnSpozivButton = new MenuItem("Внутрішнє споживання товарів");
            newDocVnSpozivButton.Activated += OnNewDocNaOsnovi_ВнутрішнєСпоживанняТоварів;
            Menu.Append(newDocVnSpozivButton);

            MenuItem newDocPeremischennyaTovariv = new MenuItem("Переміщення товарів");
            newDocPeremischennyaTovariv.Activated += OnNewDocNaOsnovi_ПереміщенняТоварів;
            Menu.Append(newDocPeremischennyaTovariv);

            Menu.ShowAll();

            return Menu;
        }

        Menu PopUpContextMenu()
        {
            Menu Menu = new Menu();

            MenuItem spendTheDocumentButton = new MenuItem("Провести документ");
            spendTheDocumentButton.Activated += OnSpendTheDocument;
            Menu.Append(spendTheDocumentButton);

            MenuItem clearSpendButton = new MenuItem("Відмінити проведення");
            clearSpendButton.Activated += OnClearSpend;
            Menu.Append(clearSpendButton);

            Menu.ShowAll();

            return Menu;
        }

        Menu ToolbarPrintingSubMenu()
        {
            Menu Menu = new Menu();

            MenuItem printButton = new MenuItem("Документ");
            printButton.Activated += OnPrintingInvoiceClick;
            Menu.Append(printButton);

            Menu.ShowAll();

            return Menu;
        }

        Menu ToolbarExportSubMenu()
        {
            Menu Menu = new Menu();

            MenuItem exportXMLButton = new MenuItem("Формат XML");
            exportXMLButton.Activated += OnExportXMLClick;
            Menu.Append(exportXMLButton);

            Menu.ShowAll();

            return Menu;
        }

        #endregion

        public void SetValue()
        {
            if (PeriodWhere != 0)
                ComboBoxPeriodWhere.ActiveId = PeriodWhere.ToString();
            else if ((int)Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
        }

        public void LoadRecords()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords();

            if (ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.Where.Add(
                new Where(ПоступленняТоварівТаПослуг_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords();

            if (ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Поступлення товарів та послуг: *", () =>
                {
                    ПоступленняТоварівТаПослуг_Елемент page = new ПоступленняТоварівТаПослуг_Елемент
                    {
                        PageList = this,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else
            {
                ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest = new ПоступленняТоварівТаПослуг_Objest();
                if (ПоступленняТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПоступленняТоварівТаПослуг_Objest.Назва}", () =>
                    {
                        ПоступленняТоварівТаПослуг_Елемент page = new ПоступленняТоварівТаПослуг_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ПоступленняТоварівТаПослуг_Objest = ПоступленняТоварівТаПослуг_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        #region  TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                SelectPointerItem = new ПоступленняТоварівТаПослуг_Pointer(unigueID);
            }
        }

        void OnButtonReleaseEvent(object? sender, ButtonReleaseEventArgs args)
        {
            if (args.Event.Button == 3 && TreeViewGrid.Selection.CountSelectedRows() != 0)
                PopUpContextMenu().Popup();
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    if (DocumentPointerItem == null)
                        OpenPageElement(false, uid);
                    else
                    {
                        if (CallBack_OnSelectPointer != null)
                            CallBack_OnSelectPointer.Invoke(new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)));

                        Program.GeneralForm?.CloseCurrentPageNotebook();
                    }
                }
            }
        }

        void OnKeyReleaseEvent(object? sender, KeyReleaseEventArgs args)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.Insert:
                    {
                        OpenPageElement(true);
                        break;
                    }
                case Gdk.Key.F5:
                    {
                        LoadRecords();
                        break;
                    }
                case Gdk.Key.KP_Enter:
                case Gdk.Key.Return:
                    {
                        OnEditClick(null, new EventArgs());
                        break;
                    }
                case Gdk.Key.End:
                case Gdk.Key.Home:
                case Gdk.Key.Up:
                case Gdk.Key.Down:
                case Gdk.Key.Prior:
                case Gdk.Key.Next:
                    {
                        OnRowActivated(TreeViewGrid, new RowActivatedArgs());
                        break;
                    }
            }
        }

        #endregion

        #region ToolBar

        void OnAddClick(object? sender, EventArgs args)
        {
            OpenPageElement(true);
        }

        void OnEditClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    if (TreeViewGrid.Model.GetIter(out iter, itemPath))
                    {
                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                        OpenPageElement(false, uid);
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

                        ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest = new ПоступленняТоварівТаПослуг_Objest();
                        if (ПоступленняТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                            ПоступленняТоварівТаПослуг_Objest.Delete();
                        else
                            Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
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

                        ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest = new ПоступленняТоварівТаПослуг_Objest();
                        if (ПоступленняТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                        {
                            ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest_Новий = ПоступленняТоварівТаПослуг_Objest.Copy(true);
                            ПоступленняТоварівТаПослуг_Objest_Новий.Save();
                            ПоступленняТоварівТаПослуг_Objest_Новий.Товари_TablePart.Save(true);

                            SelectPointerItem = ПоступленняТоварівТаПослуг_Objest_Новий.GetDocumentPointer();
                        }
                        else
                            Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                    }

                    LoadRecords();
                }
            }
        }

        void OnComboBoxPeriodWhereChanged(object? sender, EventArgs args)
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        void OnReportSpendTheDocumentClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    Program.GeneralForm?.CreateNotebookPage($"Проводки", () =>
                    {
                        Звіт_РухДокументівПоРегістрах page = new Звіт_РухДокументівПоРегістрах();

                        page.CreateReport(new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            ПоступленняТоварівТаПослуг_Pointer ПоступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
            ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest = ПоступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);

            //Збереження для запуску тригерів
            ПоступленняТоварівТаПослуг_Objest.Save();

            if (spendDoc)
            {
                try
                {
                    if (!ПоступленняТоварівТаПослуг_Objest.SpendTheDocument(ПоступленняТоварівТаПослуг_Objest.ДатаДок))
                    {
                        ПоступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ПоступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ПоступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
        }

        //
        // Проведення або очищення проводок для вибраних документів
        //

        void SpendTheDocumentOrClear(bool spend)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    SpendTheDocument(uid, spend);
                }

                LoadRecords();
            }
        }

        void OnSpendTheDocument(object? sender, EventArgs args)
        {
            SpendTheDocumentOrClear(true);
        }

        void OnClearSpend(object? sender, EventArgs args)
        {
            SpendTheDocumentOrClear(false);
        }

        //
        // На основі
        //

        void OnNaOsnoviClick(object? sender, EventArgs arg)
        {
            if (sender != null)
            {
                MenuToolButton naOsnoviButton = (MenuToolButton)sender;
                Menu Menu = (Menu)naOsnoviButton.Menu;
                Menu.Popup();
            }
        }

        void OnNewDocNaOsnovi_РозхіднийКасовийОрдер(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Pointer.GetDocumentObject(false);

                    //
                    //Новий документ
                    //

                    РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Новий = new РозхіднийКасовийОрдер_Objest();
                    розхіднийКасовийОрдер_Новий.New();
                    розхіднийКасовийОрдер_Новий.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    розхіднийКасовийОрдер_Новий.Валюта = поступленняТоварівТаПослуг_Objest.Валюта;
                    розхіднийКасовийОрдер_Новий.Каса = поступленняТоварівТаПослуг_Objest.Каса;
                    розхіднийКасовийОрдер_Новий.Контрагент = поступленняТоварівТаПослуг_Objest.Контрагент;
                    розхіднийКасовийОрдер_Новий.Договір = поступленняТоварівТаПослуг_Objest.Договір;
                    розхіднийКасовийОрдер_Новий.СумаДокументу = поступленняТоварівТаПослуг_Objest.СумаДокументу;
                    розхіднийКасовийОрдер_Новий.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();
                    розхіднийКасовийОрдер_Новий.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ОплатаПостачальнику;
                    розхіднийКасовийОрдер_Новий.Save();

                    Program.GeneralForm?.CreateNotebookPage($"{розхіднийКасовийОрдер_Новий.Назва}", () =>
                    {
                        РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент
                        {
                            IsNew = false,
                            РозхіднийКасовийОрдер_Objest = розхіднийКасовийОрдер_Новий,
                        };

                        page.SetValue();

                        return page;
                    });
                }
            }
        }

        void OnNewDocNaOsnovi_ПоверненняТоварівПостачальнику(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    ПоверненняТоварівПостачальнику_Objest поверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
                    поверненняТоварівПостачальнику_Objest.New();
                    поверненняТоварівПостачальнику_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    поверненняТоварівПостачальнику_Objest.Валюта = поступленняТоварівТаПослуг_Objest.Валюта;
                    поверненняТоварівПостачальнику_Objest.Каса = поступленняТоварівТаПослуг_Objest.Каса;
                    поверненняТоварівПостачальнику_Objest.Контрагент = поступленняТоварівТаПослуг_Objest.Контрагент;
                    поверненняТоварівПостачальнику_Objest.Договір = поступленняТоварівТаПослуг_Objest.Договір;
                    поверненняТоварівПостачальнику_Objest.Склад = поступленняТоварівТаПослуг_Objest.Склад;
                    поверненняТоварівПостачальнику_Objest.СумаДокументу = поступленняТоварівТаПослуг_Objest.СумаДокументу;
                    поверненняТоварівПостачальнику_Objest.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();
                    поверненняТоварівПостачальнику_Objest.Save();

                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                    {
                        поверненняТоварівПостачальнику_Objest.Товари_TablePart.Records.Add(new ПоверненняТоварівПостачальнику_Товари_TablePart.Record()
                        {
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Серія = record.Серія,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість,
                            Ціна = record.Ціна,
                            Сума = record.Сума,
                            ДокументПоступлення = поступленняТоварівТаПослуг_Objest.GetDocumentPointer()
                        });
                    }

                    поверненняТоварівПостачальнику_Objest.Товари_TablePart.Save(false);

                    Program.GeneralForm?.CreateNotebookPage($"{поверненняТоварівПостачальнику_Objest.Назва}", () =>
                    {
                        ПоверненняТоварівПостачальнику_Елемент page = new ПоверненняТоварівПостачальнику_Елемент
                        {
                            IsNew = false,
                            ПоверненняТоварівПостачальнику_Objest = поверненняТоварівПостачальнику_Objest
                        };

                        page.SetValue();

                        return page;
                    });
                }
            }
        }

        void OnNewDocNaOsnovi_РозміщенняТоварівНаСкладі(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    РозміщенняТоварівНаСкладі_Objest розміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
                    розміщенняТоварівНаСкладі_Objest.New();
                    розміщенняТоварівНаСкладі_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    розміщенняТоварівНаСкладі_Objest.Склад = поступленняТоварівТаПослуг_Objest.Склад;
                    розміщенняТоварівНаСкладі_Objest.Автор = поступленняТоварівТаПослуг_Objest.Автор;
                    розміщенняТоварівНаСкладі_Objest.Підрозділ = поступленняТоварівТаПослуг_Objest.Підрозділ;
                    розміщенняТоварівНаСкладі_Objest.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();
                    розміщенняТоварівНаСкладі_Objest.ДокументПоступлення = поступленняТоварівТаПослуг_Pointer;
                    розміщенняТоварівНаСкладі_Objest.Save();

                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                    {
                        розміщенняТоварівНаСкладі_Objest.Товари_TablePart.Records.Add(new РозміщенняТоварівНаСкладі_Товари_TablePart.Record()
                        {
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Серія = record.Серія,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість
                        });
                    }

                    розміщенняТоварівНаСкладі_Objest.Товари_TablePart.Save(true);

                    Program.GeneralForm?.CreateNotebookPage($"{розміщенняТоварівНаСкладі_Objest.Назва}", () =>
                    {
                        РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
                        {
                            IsNew = false,
                            РозміщенняТоварівНаСкладі_Objest = розміщенняТоварівНаСкладі_Objest
                        };

                        page.SetValue();

                        return page;
                    });
                }
            }
        }

        void OnNewDocNaOsnovi_ВнутрішнєСпоживанняТоварів(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
                    внутрішнєСпоживанняТоварів_Objest.New();
                    внутрішнєСпоживанняТоварів_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    внутрішнєСпоживанняТоварів_Objest.Склад = поступленняТоварівТаПослуг_Objest.Склад;
                    внутрішнєСпоживанняТоварів_Objest.Валюта = поступленняТоварівТаПослуг_Objest.Валюта;
                    внутрішнєСпоживанняТоварів_Objest.Автор = поступленняТоварівТаПослуг_Objest.Автор;
                    внутрішнєСпоживанняТоварів_Objest.Підрозділ = поступленняТоварівТаПослуг_Objest.Підрозділ;
                    внутрішнєСпоживанняТоварів_Objest.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();
                    внутрішнєСпоживанняТоварів_Objest.Save();

                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                    {
                        внутрішнєСпоживанняТоварів_Objest.Товари_TablePart.Records.Add(new ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record()
                        {
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Серія = record.Серія,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість,
                            Ціна = record.Ціна,
                            Сума = record.Сума
                        });
                    }

                    внутрішнєСпоживанняТоварів_Objest.Товари_TablePart.Save(true);

                    Program.GeneralForm?.CreateNotebookPage($"{внутрішнєСпоживанняТоварів_Objest.Назва}", () =>
                    {
                        ВнутрішнєСпоживанняТоварів_Елемент page = new ВнутрішнєСпоживанняТоварів_Елемент
                        {
                            IsNew = false,
                            ВнутрішнєСпоживанняТоварів_Objest = внутрішнєСпоживанняТоварів_Objest
                        };

                        page.SetValue();

                        return page;
                    });
                }
            }
        }

        void OnNewDocNaOsnovi_ПереміщенняТоварів(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    ПереміщенняТоварів_Objest переміщенняТоварів_Objest = new ПереміщенняТоварів_Objest();
                    переміщенняТоварів_Objest.New();
                    переміщенняТоварів_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    переміщенняТоварів_Objest.СкладВідправник = поступленняТоварівТаПослуг_Objest.Склад;
                    переміщенняТоварів_Objest.Автор = поступленняТоварівТаПослуг_Objest.Автор;
                    переміщенняТоварів_Objest.Підрозділ = поступленняТоварівТаПослуг_Objest.Підрозділ;
                    переміщенняТоварів_Objest.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();
                    переміщенняТоварів_Objest.Save();

                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                    {
                        переміщенняТоварів_Objest.Товари_TablePart.Records.Add(new ПереміщенняТоварів_Товари_TablePart.Record()
                        {
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Серія = record.Серія,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість
                        });
                    }

                    переміщенняТоварів_Objest.Товари_TablePart.Save(true);

                    Program.GeneralForm?.CreateNotebookPage($"{переміщенняТоварів_Objest.Назва}", () =>
                    {
                        ПереміщенняТоварів_Елемент page = new ПереміщенняТоварів_Елемент
                        {
                            IsNew = false,
                            ПереміщенняТоварів_Objest = переміщенняТоварів_Objest
                        };

                        page.SetValue();

                        return page;
                    });
                }
            }
        }

        //
        // Export
        //

        void OnExportClick(object? sender, EventArgs arg)
        {
            if (sender != null)
            {
                MenuToolButton menuToolButton = (MenuToolButton)sender;
                Menu Menu = (Menu)menuToolButton.Menu;
                Menu.Popup();
            }
        }

        void OnExportXMLClick(object? sender, EventArgs arg)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"ПоступленняТоварівТаПослуг{uid}.xml");
                    ПоступленняТоварівТаПослуг_Export.ToXmlFile(new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)), pathToSave);
                }

                LoadRecords();
            }
        }

        //
        // Друк
        //

        void OnPrintingClick(object? sender, EventArgs arg)
        {
            if (sender != null)
            {
                MenuToolButton menuToolButton = (MenuToolButton)sender;
                Menu Menu = (Menu)menuToolButton.Menu;
                Menu.Popup();
            }
        }

        void OnPrintingInvoiceClick(object? sender, EventArgs arg)
        {

        }

        #endregion
    }
}