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
    class РеалізаціяТоварівТаПослуг : VBox
    {
        public РеалізаціяТоварівТаПослуг_Pointer? SelectPointerItem { get; set; }
        public РеалізаціяТоварівТаПослуг_Pointer? DocumentPointerItem { get; set; }
        public System.Action<РеалізаціяТоварівТаПослуг_Pointer>? CallBack_OnSelectPointer { get; set; }
        public Перелічення.ТипПеріодуДляЖурналівДокументів PeriodWhere { get; set; } = 0;

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public РеалізаціяТоварівТаПослуг(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new РеалізаціяТоварівТаПослуг_Pointer());

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

            TreeViewGrid = new TreeView(ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.Store);
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);

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

            {
                MenuItem doc = new MenuItem("Прихідний касовий ордер");
                doc.Activated += OnNewDocNaOsnovi_ПрихіднийКасовийОрдер;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem("Повернення товарів від клієнта");
                doc.Activated += OnNewDocNaOsnovi_ПоверненняТоварівВідКлієнта;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem("Збірка товарів на складі");
                doc.Activated += OnNewDocNaOsnovi_ЗбіркаТоварівНаСкладі;
                Menu.Append(doc);
            }

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

            MenuItem setDeletionLabel = new MenuItem("Помітка на видалення");
            setDeletionLabel.Activated += OnDeleteClick;
            Menu.Append(setDeletionLabel);

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
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.LoadRecords();

            if (ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.Where.Clear();

            //Назва
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.Where.Add(
                new Where(РеалізаціяТоварівТаПослуг_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.LoadRecords();

            if (ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Реалізація товарів та послуг: *", () =>
                {
                    РеалізаціяТоварівТаПослуг_Елемент page = new РеалізаціяТоварівТаПослуг_Елемент
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
                РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослуг_Objest = new РеалізаціяТоварівТаПослуг_Objest();
                if (РеалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{РеалізаціяТоварівТаПослуг_Objest.Назва}", () =>
                    {
                        РеалізаціяТоварівТаПослуг_Елемент page = new РеалізаціяТоварівТаПослуг_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            РеалізаціяТоварівТаПослуг_Objest = РеалізаціяТоварівТаПослуг_Objest,
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

                SelectPointerItem = new РеалізаціяТоварівТаПослуг_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)));

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
                case Gdk.Key.Delete:
                    {
                        OnDeleteClick(TreeViewGrid, new EventArgs());
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
                if (Message.Request(Program.GeneralForm, "Встановити або зняти помітку на видалення?") == ResponseType.Yes)
                {
                    TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        TreeViewGrid.Model.GetIter(out iter, itemPath);

                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                        РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослуг_Objest = new РеалізаціяТоварівТаПослуг_Objest();
                        if (РеалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                        {
                            РеалізаціяТоварівТаПослуг_Objest.SetDeletionLabel(!РеалізаціяТоварівТаПослуг_Objest.DeletionLabel);

                            SelectPointerItem = РеалізаціяТоварівТаПослуг_Objest.GetDocumentPointer();
                        }
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

                        РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослуг_Objest = new РеалізаціяТоварівТаПослуг_Objest();
                        if (РеалізаціяТоварівТаПослуг_Objest.Read(new UnigueID(uid)))
                        {
                            РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослуг_Objest_Новий = РеалізаціяТоварівТаПослуг_Objest.Copy(true);
                            РеалізаціяТоварівТаПослуг_Objest_Новий.Save();
                            РеалізаціяТоварівТаПослуг_Objest_Новий.Товари_TablePart.Save(true);

                            SelectPointerItem = РеалізаціяТоварівТаПослуг_Objest_Новий.GetDocumentPointer();
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
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
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

                        page.CreateReport(new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            РеалізаціяТоварівТаПослуг_Pointer РеалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
            РеалізаціяТоварівТаПослуг_Objest? РеалізаціяТоварівТаПослуг_Objest = РеалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(true);
            if (РеалізаціяТоварівТаПослуг_Objest == null) return;

            if (spendDoc)
            {
                if (!РеалізаціяТоварівТаПослуг_Objest.SpendTheDocument(РеалізаціяТоварівТаПослуг_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                РеалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();

            SelectPointerItem = РеалізаціяТоварівТаПослуг_Pointer;
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

        void OnNewDocNaOsnovi_ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
                    РеалізаціяТоварівТаПослуг_Objest? реалізаціяТоварівТаПослуг_Objest = реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(false);
                    if (реалізаціяТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Новий = new ПрихіднийКасовийОрдер_Objest();
                    прихіднийКасовийОрдер_Новий.New();
                    прихіднийКасовийОрдер_Новий.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта;
                    прихіднийКасовийОрдер_Новий.Організація = реалізаціяТоварівТаПослуг_Objest.Організація;
                    прихіднийКасовийОрдер_Новий.Валюта = реалізаціяТоварівТаПослуг_Objest.Валюта;
                    прихіднийКасовийОрдер_Новий.Каса = реалізаціяТоварівТаПослуг_Objest.Каса;
                    прихіднийКасовийОрдер_Новий.Контрагент = реалізаціяТоварівТаПослуг_Objest.Контрагент;
                    прихіднийКасовийОрдер_Новий.Договір = реалізаціяТоварівТаПослуг_Objest.Договір;
                    прихіднийКасовийОрдер_Новий.СумаДокументу = реалізаціяТоварівТаПослуг_Objest.СумаДокументу;
                    прихіднийКасовийОрдер_Новий.Основа = реалізаціяТоварівТаПослуг_Objest.GetBasis();

                    if (прихіднийКасовийОрдер_Новий.Save())
                    {
                        Program.GeneralForm?.CreateNotebookPage($"{прихіднийКасовийОрдер_Новий.Назва}", () =>
                        {
                            ПрихіднийКасовийОрдер_Елемент page = new ПрихіднийКасовийОрдер_Елемент
                            {
                                IsNew = false,
                                ПрихіднийКасовийОрдер_Objest = прихіднийКасовийОрдер_Новий,
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        void OnNewDocNaOsnovi_ПоверненняТоварівВідКлієнта(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
                    РеалізаціяТоварівТаПослуг_Objest? реалізаціяТоварівТаПослуг_Objest = реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(true);
                    if (реалізаціяТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
                    поверненняТоварівВідКлієнта_Objest.New();
                    поверненняТоварівВідКлієнта_Objest.Організація = реалізаціяТоварівТаПослуг_Objest.Організація;
                    поверненняТоварівВідКлієнта_Objest.Валюта = реалізаціяТоварівТаПослуг_Objest.Валюта;
                    поверненняТоварівВідКлієнта_Objest.Каса = реалізаціяТоварівТаПослуг_Objest.Каса;
                    поверненняТоварівВідКлієнта_Objest.Контрагент = реалізаціяТоварівТаПослуг_Objest.Контрагент;
                    поверненняТоварівВідКлієнта_Objest.Договір = реалізаціяТоварівТаПослуг_Objest.Договір;
                    поверненняТоварівВідКлієнта_Objest.Склад = реалізаціяТоварівТаПослуг_Objest.Склад;
                    поверненняТоварівВідКлієнта_Objest.СумаДокументу = реалізаціяТоварівТаПослуг_Objest.СумаДокументу;
                    поверненняТоварівВідКлієнта_Objest.Основа = реалізаціяТоварівТаПослуг_Objest.GetBasis();

                    if (поверненняТоварівВідКлієнта_Objest.Save())
                    {
                        //Товари
                        foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record_реалізаціяТоварівТаПослуг in реалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Records)
                        {
                            ПоверненняТоварівВідКлієнта_Товари_TablePart.Record record_повернення = new ПоверненняТоварівВідКлієнта_Товари_TablePart.Record();
                            поверненняТоварівВідКлієнта_Objest.Товари_TablePart.Records.Add(record_повернення);

                            record_повернення.Номенклатура = record_реалізаціяТоварівТаПослуг.Номенклатура;
                            record_повернення.ХарактеристикаНоменклатури = record_реалізаціяТоварівТаПослуг.ХарактеристикаНоменклатури;
                            record_повернення.Серія = record_реалізаціяТоварівТаПослуг.Серія;
                            record_повернення.Пакування = record_реалізаціяТоварівТаПослуг.Пакування;
                            record_повернення.КількістьУпаковок = record_реалізаціяТоварівТаПослуг.КількістьУпаковок;
                            record_повернення.Кількість = record_реалізаціяТоварівТаПослуг.Кількість;
                            record_повернення.Ціна = record_реалізаціяТоварівТаПослуг.Ціна;
                            record_повернення.Сума = record_реалізаціяТоварівТаПослуг.Сума;
                            record_повернення.ДокументРеалізації = реалізаціяТоварівТаПослуг_Objest.GetDocumentPointer();
                        }

                        поверненняТоварівВідКлієнта_Objest.Товари_TablePart.Save(false);

                        Program.GeneralForm?.CreateNotebookPage($"{поверненняТоварівВідКлієнта_Objest.Назва}", () =>
                        {
                            ПоверненняТоварівВідКлієнта_Елемент page = new ПоверненняТоварівВідКлієнта_Елемент
                            {
                                IsNew = false,
                                ПоверненняТоварівВідКлієнта_Objest = поверненняТоварівВідКлієнта_Objest
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        void OnNewDocNaOsnovi_ЗбіркаТоварівНаСкладі(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
                    РеалізаціяТоварівТаПослуг_Objest? реалізаціяТоварівТаПослуг_Objest = реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(true);
                    if (реалізаціяТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ЗбіркаТоварівНаСкладі_Objest збіркаТоварівНаСкладі_Objest = new ЗбіркаТоварівНаСкладі_Objest();
                    збіркаТоварівНаСкладі_Objest.New();
                    збіркаТоварівНаСкладі_Objest.Організація = реалізаціяТоварівТаПослуг_Objest.Організація;
                    збіркаТоварівНаСкладі_Objest.Склад = реалізаціяТоварівТаПослуг_Objest.Склад;
                    збіркаТоварівНаСкладі_Objest.Основа = реалізаціяТоварівТаПослуг_Objest.GetBasis();
                    збіркаТоварівНаСкладі_Objest.ДокументРеалізації = реалізаціяТоварівТаПослуг_Pointer;

                    if (збіркаТоварівНаСкладі_Objest.Save())
                    {
                        //Товари
                        foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record_реалізаціяТоварівТаПослуг in реалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Records)
                        {
                            ЗбіркаТоварівНаСкладі_Товари_TablePart.Record record = new ЗбіркаТоварівНаСкладі_Товари_TablePart.Record();
                            збіркаТоварівНаСкладі_Objest.Товари_TablePart.Records.Add(record);

                            record.Номенклатура = record_реалізаціяТоварівТаПослуг.Номенклатура;
                            record.ХарактеристикаНоменклатури = record_реалізаціяТоварівТаПослуг.ХарактеристикаНоменклатури;
                            record.Серія = record_реалізаціяТоварівТаПослуг.Серія;
                            record.Пакування = record_реалізаціяТоварівТаПослуг.Пакування;
                            record.КількістьУпаковок = record_реалізаціяТоварівТаПослуг.КількістьУпаковок;
                            record.Кількість = record_реалізаціяТоварівТаПослуг.Кількість;
                        }

                        збіркаТоварівНаСкладі_Objest.Товари_TablePart.Save(false);

                        Program.GeneralForm?.CreateNotebookPage($"{збіркаТоварівНаСкладі_Objest.Назва}", () =>
                        {
                            ЗбіркаТоварівНаСкладі_Елемент page = new ЗбіркаТоварівНаСкладі_Елемент
                            {
                                IsNew = false,
                                ЗбіркаТоварівНаСкладі_Objest = збіркаТоварівНаСкладі_Objest
                            };

                            page.SetValue();

                            return page;
                        });
                    }
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

                    string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"РеалізаціяТоварівТаПослуг{uid}.xml");
                    РеалізаціяТоварівТаПослуг_Export.ToXmlFile(new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)), pathToSave);
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