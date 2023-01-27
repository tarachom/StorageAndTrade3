#region Info

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

#endregion

using Gtk;

using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ВведенняЗалишків : VBox
    {
        public ВведенняЗалишків_Pointer? SelectPointerItem { get; set; }
        public ВведенняЗалишків_Pointer? DocumentPointerItem { get; set; }
        public System.Action<ВведенняЗалишків_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public ВведенняЗалишків(bool IsSelectPointer = false) : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            //Як форма відкрита для вибору
            if (IsSelectPointer)
            {
                Button bEmptyPointer = new Button("Вибрати пустий елемент");
                bEmptyPointer.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new ВведенняЗалишків_Pointer());

                    Program.GeneralForm?.CloseCurrentPageNotebook();
                };

                hBoxBotton.PackStart(bEmptyPointer, false, false, 10);
            }

            //Відбір по періоду
            hBoxBotton.PackStart(new Label("Період:"), false, false, 5);

            ComboBoxPeriodWhere = ТабличніСписки.Інтерфейс.СписокВідбірПоПеріоду();
            ComboBoxPeriodWhere.Changed += OnComboBoxPeriodWhereChanged;

            hBoxBotton.PackStart(ComboBoxPeriodWhere, false, false, 0);

            PackStart(hBoxBotton, false, false, 10);

            //Пошук 2
            hBoxBotton.PackStart(ПошукПовнотекстовий, false, false, 2);
            ПошукПовнотекстовий.Select = LoadRecords_OnSearch;
            ПошукПовнотекстовий.Clear = () => { OnComboBoxPeriodWhereChanged(null, new EventArgs()); };

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ВведенняЗалишків_Записи.Store);
            ТабличніСписки.ВведенняЗалишків_Записи.AddColumns(TreeViewGrid);

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
            ТабличніСписки.ВведенняЗалишків_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВведенняЗалишків_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ВведенняЗалишків_Записи.LoadRecords();

            if (ТабличніСписки.ВведенняЗалишків_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВведенняЗалишків_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ВведенняЗалишків_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВведенняЗалишків_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ВведенняЗалишків_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ВведенняЗалишків_Записи.Where.Add(
                new Where(ВведенняЗалишків_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ВведенняЗалишків_Записи.LoadRecords();
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Введення залишків: *", () =>
                {
                    ВведенняЗалишків_Елемент page = new ВведенняЗалишків_Елемент
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
                ВведенняЗалишків_Objest ВведенняЗалишків_Objest = new ВведенняЗалишків_Objest();
                if (ВведенняЗалишків_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВведенняЗалишків_Objest.Назва}", () =>
                    {
                        ВведенняЗалишків_Елемент page = new ВведенняЗалишків_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ВведенняЗалишків_Objest = ВведенняЗалишків_Objest,
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

                SelectPointerItem = new ВведенняЗалишків_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new ВведенняЗалишків_Pointer(new UnigueID(uid)));

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

                        ВведенняЗалишків_Objest ВведенняЗалишків_Objest = new ВведенняЗалишків_Objest();
                        if (ВведенняЗалишків_Objest.Read(new UnigueID(uid)))
                            ВведенняЗалишків_Objest.Delete();
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

                        ВведенняЗалишків_Objest ВведенняЗалишків_Objest = new ВведенняЗалишків_Objest();
                        if (ВведенняЗалишків_Objest.Read(new UnigueID(uid)))
                        {
                            ВведенняЗалишків_Objest ВведенняЗалишків_Objest_Новий = ВведенняЗалишків_Objest.Copy();
                            ВведенняЗалишків_Objest_Новий.Назва += " *";
                            ВведенняЗалишків_Objest_Новий.ДатаДок = DateTime.Now;
                            ВведенняЗалишків_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ВведенняЗалишків_Const).ToString("D8");
                            ВведенняЗалишків_Objest_Новий.Save();

                            //Зчитати та скопіювати табличну частину Товари
                            ВведенняЗалишків_Objest.Товари_TablePart.Read();
                            ВведенняЗалишків_Objest_Новий.Товари_TablePart.Records = ВведенняЗалишків_Objest.Товари_TablePart.Copy();
                            ВведенняЗалишків_Objest_Новий.Товари_TablePart.Save(true);

                            //Зчитати та скопіювати табличну частину Каси
                            ВведенняЗалишків_Objest.Каси_TablePart.Read();
                            ВведенняЗалишків_Objest_Новий.Каси_TablePart.Records = ВведенняЗалишків_Objest.Каси_TablePart.Copy();
                            ВведенняЗалишків_Objest_Новий.Каси_TablePart.Save(true);

                            //Зчитати та скопіювати табличну частину БанківськіРахунки
                            ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Read();
                            ВведенняЗалишків_Objest_Новий.БанківськіРахунки_TablePart.Records = ВведенняЗалишків_Objest.БанківськіРахунки_TablePart.Copy();
                            ВведенняЗалишків_Objest_Новий.БанківськіРахунки_TablePart.Save(true);

                            //Зчитати та скопіювати табличну частину РозрахункиЗКонтрагентами
                            ВведенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Read();
                            ВведенняЗалишків_Objest_Новий.РозрахункиЗКонтрагентами_TablePart.Records = ВведенняЗалишків_Objest.РозрахункиЗКонтрагентами_TablePart.Copy();
                            ВведенняЗалишків_Objest_Новий.РозрахункиЗКонтрагентами_TablePart.Save(true);

                            SelectPointerItem = ВведенняЗалишків_Objest_Новий.GetDocumentPointer();
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
            ТабличніСписки.ВведенняЗалишків_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
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

                        page.CreateReport(new ВведенняЗалишків_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            ВведенняЗалишків_Pointer ВведенняЗалишків_Pointer = new ВведенняЗалишків_Pointer(new UnigueID(uid));
            ВведенняЗалишків_Objest ВведенняЗалишків_Objest = ВведенняЗалишків_Pointer.GetDocumentObject(true);

            //Збереження для запуску тригерів
            ВведенняЗалишків_Objest.Save();

            if (spendDoc)
            {
                try
                {
                    if (!ВведенняЗалишків_Objest.SpendTheDocument(ВведенняЗалишків_Objest.ДатаДок))
                    {
                        ВведенняЗалишків_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ВведенняЗалишків_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ВведенняЗалишків_Objest.ClearSpendTheDocument();
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

                    string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"ВведенняЗалишків{uid}.xml");
                    ВведенняЗалишків_Export.ToXmlFile(new ВведенняЗалишків_Pointer(new UnigueID(uid)), pathToSave);
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