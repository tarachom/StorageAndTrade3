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
    class РахунокФактура : VBox
    {
        public РахунокФактура_Pointer? SelectPointerItem { get; set; }
        public РахунокФактура_Pointer? DocumentPointerItem { get; set; }
        public System.Action<РахунокФактура_Pointer>? CallBack_OnSelectPointer { get; set; }
        public Перелічення.ТипПеріодуДляЖурналівДокументів PeriodWhere { get; set; } = 0;

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public РахунокФактура(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new РахунокФактура_Pointer());

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

            TreeViewGrid = new TreeView(ТабличніСписки.РахунокФактура_Записи.Store);
            ТабличніСписки.РахунокФактура_Записи.AddColumns(TreeViewGrid);

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

            MenuItem newDocRoshidnaNakladnaButton = new MenuItem("Реалізація товарів та послуг");
            newDocRoshidnaNakladnaButton.Activated += OnNewDocNaOsnovi_RoshidnaNakladna;
            Menu.Append(newDocRoshidnaNakladnaButton);

            MenuItem newDocSamovlenjaPostachalnykuButton = new MenuItem("Замовлення постачальнику");
            newDocSamovlenjaPostachalnykuButton.Activated += OnNewDocNaOsnovi_SamovlenjaPostachalnyku;
            Menu.Append(newDocSamovlenjaPostachalnykuButton);

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
            ТабличніСписки.РахунокФактура_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РахунокФактура_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РахунокФактура_Записи.LoadRecords();

            if (ТабличніСписки.РахунокФактура_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РахунокФактура_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РахунокФактура_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РахунокФактура_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.РахунокФактура_Записи.Where.Clear();

            //Назва
            ТабличніСписки.РахунокФактура_Записи.Where.Add(
                new Where(РахунокФактура_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.РахунокФактура_Записи.LoadRecords();

            if (ТабличніСписки.РахунокФактура_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РахунокФактура_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Рахунок фактура: *", () =>
                {
                    РахунокФактура_Елемент page = new РахунокФактура_Елемент
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
                РахунокФактура_Objest РахунокФактура_Objest = new РахунокФактура_Objest();
                if (РахунокФактура_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{РахунокФактура_Objest.Назва}", () =>
                    {
                        РахунокФактура_Елемент page = new РахунокФактура_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            РахунокФактура_Objest = РахунокФактура_Objest
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

                SelectPointerItem = new РахунокФактура_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new РахунокФактура_Pointer(new UnigueID(uid)));

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

                        РахунокФактура_Objest РахунокФактура_Objest = new РахунокФактура_Objest();
                        if (РахунокФактура_Objest.Read(new UnigueID(uid)))
                            РахунокФактура_Objest.Delete();
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

                        РахунокФактура_Objest РахунокФактура_Objest = new РахунокФактура_Objest();
                        if (РахунокФактура_Objest.Read(new UnigueID(uid)))
                        {
                            РахунокФактура_Objest РахунокФактура_Objest_Новий = РахунокФактура_Objest.Copy(true);
                            РахунокФактура_Objest_Новий.Save();
                            РахунокФактура_Objest_Новий.Товари_TablePart.Save(true);

                            SelectPointerItem = РахунокФактура_Objest_Новий.GetDocumentPointer();
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
            ТабличніСписки.РахунокФактура_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
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

                        page.CreateReport(new РахунокФактура_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            РахунокФактура_Pointer РахунокФактура_Pointer = new РахунокФактура_Pointer(new UnigueID(uid));
            РахунокФактура_Objest РахунокФактура_Objest = РахунокФактура_Pointer.GetDocumentObject(true);

            //Збереження для запуску тригерів
            РахунокФактура_Objest.Save();

            if (spendDoc)
            {
                try
                {
                    if (!РахунокФактура_Objest.SpendTheDocument(РахунокФактура_Objest.ДатаДок))
                    {
                        РахунокФактура_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    РахунокФактура_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                РахунокФактура_Objest.ClearSpendTheDocument();
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

        void OnNewDocNaOsnovi_RoshidnaNakladna(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РахунокФактура_Pointer РахунокФактура_Pointer = new РахунокФактура_Pointer(new UnigueID(uid));
                    РахунокФактура_Objest рахунокФактура_Objest = РахунокФактура_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Новий = new РеалізаціяТоварівТаПослуг_Objest();
                    реалізаціяТоварівТаПослуг_Новий.New();
                    реалізаціяТоварівТаПослуг_Новий.Організація = рахунокФактура_Objest.Організація;
                    реалізаціяТоварівТаПослуг_Новий.Валюта = рахунокФактура_Objest.Валюта;
                    реалізаціяТоварівТаПослуг_Новий.Каса = рахунокФактура_Objest.Каса;
                    реалізаціяТоварівТаПослуг_Новий.Контрагент = рахунокФактура_Objest.Контрагент;
                    реалізаціяТоварівТаПослуг_Новий.Договір = рахунокФактура_Objest.Договір;
                    реалізаціяТоварівТаПослуг_Новий.Склад = рахунокФактура_Objest.Склад;
                    реалізаціяТоварівТаПослуг_Новий.СумаДокументу = рахунокФактура_Objest.СумаДокументу;
                    реалізаціяТоварівТаПослуг_Новий.Статус = Перелічення.СтатусиРеалізаціїТоварівТаПослуг.ДоОплати;
                    реалізаціяТоварівТаПослуг_Новий.ФормаОплати = рахунокФактура_Objest.ФормаОплати;
                    реалізаціяТоварівТаПослуг_Новий.Основа = рахунокФактура_Objest.GetBasis();
                    реалізаціяТоварівТаПослуг_Новий.Save();

                    //Товари
                    foreach (РахунокФактура_Товари_TablePart.Record record_замовлення in рахунокФактура_Objest.Товари_TablePart.Records)
                    {
                        РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record_реалізація = new РеалізаціяТоварівТаПослуг_Товари_TablePart.Record();
                        реалізаціяТоварівТаПослуг_Новий.Товари_TablePart.Records.Add(record_реалізація);

                        record_реалізація.Номенклатура = record_замовлення.Номенклатура;
                        record_реалізація.ХарактеристикаНоменклатури = record_замовлення.ХарактеристикаНоменклатури;
                        record_реалізація.Пакування = record_замовлення.Пакування;
                        record_реалізація.КількістьУпаковок = record_замовлення.КількістьУпаковок;
                        record_реалізація.Кількість = record_замовлення.Кількість;
                        record_реалізація.Ціна = record_замовлення.Ціна;
                        record_реалізація.Сума = record_замовлення.Сума;
                        record_реалізація.Скидка = record_замовлення.Скидка;
                        record_реалізація.РахунокФактура = рахунокФактура_Objest.GetDocumentPointer();
                        record_реалізація.Склад = рахунокФактура_Objest.Склад;
                        record_реалізація.ВидЦіни = record_замовлення.ВидЦіни;
                    }

                    реалізаціяТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

                    Program.GeneralForm?.CreateNotebookPage($"{реалізаціяТоварівТаПослуг_Новий.Назва}", () =>
                    {
                        РеалізаціяТоварівТаПослуг_Елемент page = new РеалізаціяТоварівТаПослуг_Елемент
                        {
                            IsNew = false,
                            РеалізаціяТоварівТаПослуг_Objest = реалізаціяТоварівТаПослуг_Новий,
                        };

                        page.SetValue();

                        return page;
                    });
                }
            }
        }

        void OnNewDocNaOsnovi_SamovlenjaPostachalnyku(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РахунокФактура_Pointer рахунокФактура_Pointer = new РахунокФактура_Pointer(new UnigueID(uid));
                    РахунокФактура_Objest рахунокФактура_Objest = рахунокФактура_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    ЗамовленняПостачальнику_Objest замовленняПостачальнику_Новий = new ЗамовленняПостачальнику_Objest();
                    замовленняПостачальнику_Новий.New();
                    замовленняПостачальнику_Новий.Організація = рахунокФактура_Objest.Організація;
                    замовленняПостачальнику_Новий.Валюта = рахунокФактура_Objest.Валюта;
                    замовленняПостачальнику_Новий.Каса = рахунокФактура_Objest.Каса;
                    замовленняПостачальнику_Новий.Контрагент = рахунокФактура_Objest.Контрагент;
                    замовленняПостачальнику_Новий.Договір = ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(рахунокФактура_Objest.Контрагент, Перелічення.ТипДоговорів.ЗПостачальниками) ?? рахунокФактура_Objest.Договір;
                    замовленняПостачальнику_Новий.Склад = рахунокФактура_Objest.Склад;
                    замовленняПостачальнику_Новий.СумаДокументу = рахунокФактура_Objest.СумаДокументу;
                    замовленняПостачальнику_Новий.Статус = Перелічення.СтатусиЗамовленьПостачальникам.Підтверджений;
                    замовленняПостачальнику_Новий.ФормаОплати = рахунокФактура_Objest.ФормаОплати;
                    замовленняПостачальнику_Новий.Основа = рахунокФактура_Objest.GetBasis();
                    замовленняПостачальнику_Новий.Save();

                    //Товари
                    foreach (РахунокФактура_Товари_TablePart.Record record_замовлення in рахунокФактура_Objest.Товари_TablePart.Records)
                    {
                        ЗамовленняПостачальнику_Товари_TablePart.Record record_замовленняПостачальнику = new ЗамовленняПостачальнику_Товари_TablePart.Record();
                        замовленняПостачальнику_Новий.Товари_TablePart.Records.Add(record_замовленняПостачальнику);

                        record_замовленняПостачальнику.Номенклатура = record_замовлення.Номенклатура;
                        record_замовленняПостачальнику.ХарактеристикаНоменклатури = record_замовлення.ХарактеристикаНоменклатури;
                        record_замовленняПостачальнику.Пакування = record_замовлення.Пакування;
                        record_замовленняПостачальнику.КількістьУпаковок = record_замовлення.КількістьУпаковок;
                        record_замовленняПостачальнику.Кількість = record_замовлення.Кількість;
                        record_замовленняПостачальнику.Ціна = record_замовлення.Ціна;
                        record_замовленняПостачальнику.Сума = record_замовлення.Сума;
                        record_замовленняПостачальнику.Скидка = record_замовлення.Скидка;
                        record_замовленняПостачальнику.Склад = рахунокФактура_Objest.Склад;
                    }

                    замовленняПостачальнику_Новий.Товари_TablePart.Save(false);

                    Program.GeneralForm?.CreateNotebookPage($"{замовленняПостачальнику_Новий.Назва}", () =>
                    {
                        ЗамовленняПостачальнику_Елемент page = new ЗамовленняПостачальнику_Елемент
                        {
                            IsNew = false,
                            ЗамовленняПостачальнику_Objest = замовленняПостачальнику_Новий,
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

                    string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"РахунокФактура{uid}.xml");
                    РахунокФактура_Export.ToXmlFile(new РахунокФактура_Pointer(new UnigueID(uid)), pathToSave);
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