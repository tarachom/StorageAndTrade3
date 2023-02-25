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

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public ПоступленняТоварівТаПослуг(bool IsSelectPointer = false) : base()
        {
            new VBox(false, 0);
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
            newDocKasovyiOrderButton.Activated += OnNewDocNaOsnovi_КасовийОрдер;
            Menu.Append(newDocKasovyiOrderButton);

            MenuItem newDocPovernenjaPostachalnykuButton = new MenuItem("Повернення товарів постачальнику");
            newDocPovernenjaPostachalnykuButton.Activated += OnNewDocNaOsnovi_ПоверненняТоварівПостачальнику;
            Menu.Append(newDocPovernenjaPostachalnykuButton);

            MenuItem newDocRozmisctenjaNaSkaldyButton = new MenuItem("Розміщення товарів на складі");
            newDocRozmisctenjaNaSkaldyButton.Activated += OnNewDocNaOsnovi_РозміщенняТоварівНаСкладі;
            Menu.Append(newDocRozmisctenjaNaSkaldyButton);

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
            if ((int)Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
            else
                ComboBoxPeriodWhere.Active = 0;
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
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    OpenPageElement(false, uid);
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
                            ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest_Новий = ПоступленняТоварівТаПослуг_Objest.Copy();
                            ПоступленняТоварівТаПослуг_Objest_Новий.Назва += " *";
                            ПоступленняТоварівТаПослуг_Objest_Новий.ДатаДок = DateTime.Now;
                            ПоступленняТоварівТаПослуг_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
                            ПоступленняТоварівТаПослуг_Objest_Новий.Save();

                            //Зчитати та скопіювати табличну частину Товари
                            ПоступленняТоварівТаПослуг_Objest.Товари_TablePart.Read();
                            ПоступленняТоварівТаПослуг_Objest_Новий.Товари_TablePart.Records = ПоступленняТоварівТаПослуг_Objest.Товари_TablePart.Copy();
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

        void OnNewDocNaOsnovi_КасовийОрдер(object? sender, EventArgs args)
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
                    розхіднийКасовийОрдер_Новий.ДатаДок = DateTime.Now;
                    розхіднийКасовийОрдер_Новий.НомерДок = (++Константи.НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");
                    розхіднийКасовийОрдер_Новий.Назва = $"Розхідний касовий ордер №{розхіднийКасовийОрдер_Новий.НомерДок} від {розхіднийКасовийОрдер_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
                    розхіднийКасовийОрдер_Новий.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    розхіднийКасовийОрдер_Новий.Валюта = поступленняТоварівТаПослуг_Objest.Валюта;
                    розхіднийКасовийОрдер_Новий.Каса = поступленняТоварівТаПослуг_Objest.Каса;
                    розхіднийКасовийОрдер_Новий.Контрагент = поступленняТоварівТаПослуг_Objest.Контрагент;
                    розхіднийКасовийОрдер_Новий.Договір = поступленняТоварівТаПослуг_Objest.Договір;
                    розхіднийКасовийОрдер_Новий.СумаДокументу = поступленняТоварівТаПослуг_Objest.СумаДокументу;
                    розхіднийКасовийОрдер_Новий.Основа = new UuidAndText(поступленняТоварівТаПослуг_Objest.UnigueID.UGuid, поступленняТоварівТаПослуг_Objest.TypeDocument);
                    розхіднийКасовийОрдер_Новий.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ОплатаПостачальнику;
                    розхіднийКасовийОрдер_Новий.Автор = Program.Користувач;
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
                    поверненняТоварівПостачальнику_Objest.ДатаДок = DateTime.Now;
                    поверненняТоварівПостачальнику_Objest.НомерДок = (++Константи.НумераціяДокументів.ПоверненняТоварівПостачальнику_Const).ToString("D8");
                    поверненняТоварівПостачальнику_Objest.Назва = $"Повернення товарів постачальнику №{поверненняТоварівПостачальнику_Objest.НомерДок} від {поверненняТоварівПостачальнику_Objest.ДатаДок.ToString("dd.MM.yyyy")}";
                    поверненняТоварівПостачальнику_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    поверненняТоварівПостачальнику_Objest.Валюта = поступленняТоварівТаПослуг_Objest.Валюта;
                    поверненняТоварівПостачальнику_Objest.Каса = поступленняТоварівТаПослуг_Objest.Каса;
                    поверненняТоварівПостачальнику_Objest.Контрагент = поступленняТоварівТаПослуг_Objest.Контрагент;
                    поверненняТоварівПостачальнику_Objest.Договір = поступленняТоварівТаПослуг_Objest.Договір;
                    поверненняТоварівПостачальнику_Objest.Склад = поступленняТоварівТаПослуг_Objest.Склад;
                    поверненняТоварівПостачальнику_Objest.СумаДокументу = поступленняТоварівТаПослуг_Objest.СумаДокументу;
                    поверненняТоварівПостачальнику_Objest.Основа = new UuidAndText(поступленняТоварівТаПослуг_Objest.UnigueID.UGuid, поступленняТоварівТаПослуг_Objest.TypeDocument);
                    поверненняТоварівПостачальнику_Objest.Автор = Program.Користувач;
                    поверненняТоварівПостачальнику_Objest.Save();

                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record_реалізаціяТоварівТаПослуг in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                    {
                        ПоверненняТоварівПостачальнику_Товари_TablePart.Record record_повернення = new ПоверненняТоварівПостачальнику_Товари_TablePart.Record();
                        поверненняТоварівПостачальнику_Objest.Товари_TablePart.Records.Add(record_повернення);

                        record_повернення.Номенклатура = record_реалізаціяТоварівТаПослуг.Номенклатура;
                        record_повернення.ХарактеристикаНоменклатури = record_реалізаціяТоварівТаПослуг.ХарактеристикаНоменклатури;
                        record_повернення.Серія = record_реалізаціяТоварівТаПослуг.Серія;
                        record_повернення.Пакування = record_реалізаціяТоварівТаПослуг.Пакування;
                        record_повернення.КількістьУпаковок = record_реалізаціяТоварівТаПослуг.КількістьУпаковок;
                        record_повернення.Кількість = record_реалізаціяТоварівТаПослуг.Кількість;
                        record_повернення.Ціна = record_реалізаціяТоварівТаПослуг.Ціна;
                        record_повернення.Сума = record_реалізаціяТоварівТаПослуг.Сума;
                        record_повернення.ДокументПоступлення = поступленняТоварівТаПослуг_Objest.GetDocumentPointer();
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
                    розміщенняТоварівНаСкладі_Objest.ДатаДок = DateTime.Now;
                    розміщенняТоварівНаСкладі_Objest.НомерДок = (++Константи.НумераціяДокументів.РозміщенняТоварівНаСкладі_Const).ToString("D8");
                    розміщенняТоварівНаСкладі_Objest.Назва = $"Розміщення товарів на складі №{розміщенняТоварівНаСкладі_Objest.НомерДок} від {розміщенняТоварівНаСкладі_Objest.ДатаДок.ToString("dd.MM.yyyy")}";
                    розміщенняТоварівНаСкладі_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    розміщенняТоварівНаСкладі_Objest.Склад = поступленняТоварівТаПослуг_Objest.Склад;
                    розміщенняТоварівНаСкладі_Objest.Автор = поступленняТоварівТаПослуг_Objest.Автор;
                    розміщенняТоварівНаСкладі_Objest.Підрозділ = поступленняТоварівТаПослуг_Objest.Підрозділ;
                    розміщенняТоварівНаСкладі_Objest.Основа = new UuidAndText(поступленняТоварівТаПослуг_Objest.UnigueID.UGuid, поступленняТоварівТаПослуг_Objest.TypeDocument);
                    розміщенняТоварівНаСкладі_Objest.ДокументПоступлення = поступленняТоварівТаПослуг_Pointer;
                    розміщенняТоварівНаСкладі_Objest.Автор = Program.Користувач;
                    розміщенняТоварівНаСкладі_Objest.Save();

                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record_реалізаціяТоварівТаПослуг in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                    {
                        РозміщенняТоварівНаСкладі_Товари_TablePart.Record record = new РозміщенняТоварівНаСкладі_Товари_TablePart.Record();
                        розміщенняТоварівНаСкладі_Objest.Товари_TablePart.Records.Add(record);

                        record.Номенклатура = record_реалізаціяТоварівТаПослуг.Номенклатура;
                        record.ХарактеристикаНоменклатури = record_реалізаціяТоварівТаПослуг.ХарактеристикаНоменклатури;
                        record.Серія = record_реалізаціяТоварівТаПослуг.Серія;
                        record.Пакування = record_реалізаціяТоварівТаПослуг.Пакування;
                        record.КількістьУпаковок = record_реалізаціяТоварівТаПослуг.КількістьУпаковок;
                        record.Кількість = record_реалізаціяТоварівТаПослуг.Кількість;
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