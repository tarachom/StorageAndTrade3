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
    class РозхіднийКасовийОрдер : VBox
    {
        public РозхіднийКасовийОрдер_Pointer? SelectPointerItem { get; set; }
        public РозхіднийКасовийОрдер_Pointer? DocumentPointerItem { get; set; }
        public System.Action<РозхіднийКасовийОрдер_Pointer>? CallBack_OnSelectPointer { get; set; }
        public Перелічення.ТипПеріодуДляЖурналівДокументів PeriodWhere { get; set; } = 0;

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public РозхіднийКасовийОрдер(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new РозхіднийКасовийОрдер_Pointer());

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

            TreeViewGrid = new TreeView(ТабличніСписки.РозхіднийКасовийОрдер_Записи.Store);
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.AddColumns(TreeViewGrid);

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
            if (PeriodWhere != 0)
                ComboBoxPeriodWhere.ActiveId = PeriodWhere.ToString();
            else if ((int)Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
        }

        public void LoadRecords()
        {
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозхіднийКасовийОрдер_Записи.LoadRecords();

            if (ТабличніСписки.РозхіднийКасовийОрдер_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозхіднийКасовийОрдер_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РозхіднийКасовийОрдер_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозхіднийКасовийОрдер_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.РозхіднийКасовийОрдер_Записи.Where.Clear();

            //Назва
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.Where.Add(
                new Where(РозхіднийКасовийОрдер_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.РозхіднийКасовийОрдер_Записи.LoadRecords();

            if (ТабличніСписки.РозхіднийКасовийОрдер_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозхіднийКасовийОрдер_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        #region  TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                SelectPointerItem = new РозхіднийКасовийОрдер_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new РозхіднийКасовийОрдер_Pointer(new UnigueID(uid)));

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

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент
                {
                    PageList = this,
                    IsNew = true
                };

                Program.GeneralForm?.CreateNotebookPage($"Розхідний касовий ордер: *", () => { return page; }, true);

                page.SetValue();
            }
            else
            {
                РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
                if (РозхіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
                {
                    РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент
                    {
                        PageList = this,
                        IsNew = false,
                        РозхіднийКасовийОрдер_Objest = РозхіднийКасовийОрдер_Objest,
                    };

                    Program.GeneralForm?.CreateNotebookPage($"{РозхіднийКасовийОрдер_Objest.Назва}", () => { return page; }, true);

                    page.SetValue();
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
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

                        РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
                        if (РозхіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
                            РозхіднийКасовийОрдер_Objest.Delete();
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

                        РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest = new РозхіднийКасовийОрдер_Objest();
                        if (РозхіднийКасовийОрдер_Objest.Read(new UnigueID(uid)))
                        {
                            РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest_Новий = РозхіднийКасовийОрдер_Objest.Copy();
                            РозхіднийКасовийОрдер_Objest_Новий.Назва += " *";
                            РозхіднийКасовийОрдер_Objest_Новий.ДатаДок = DateTime.Now;
                            РозхіднийКасовийОрдер_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");
                            РозхіднийКасовийОрдер_Objest_Новий.Автор = Program.Користувач;
                            РозхіднийКасовийОрдер_Objest_Новий.Save();

                            SelectPointerItem = РозхіднийКасовийОрдер_Objest_Новий.GetDocumentPointer();
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
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
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

                        page.CreateReport(new РозхіднийКасовийОрдер_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            РозхіднийКасовийОрдер_Pointer РозхіднийКасовийОрдер_Pointer = new РозхіднийКасовийОрдер_Pointer(new UnigueID(uid));
            РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдер_Objest = РозхіднийКасовийОрдер_Pointer.GetDocumentObject(true);

            //Збереження для запуску тригерів
            РозхіднийКасовийОрдер_Objest.Save();

            if (spendDoc)
            {
                try
                {
                    if (!РозхіднийКасовийОрдер_Objest.SpendTheDocument(РозхіднийКасовийОрдер_Objest.ДатаДок))
                    {
                        РозхіднийКасовийОрдер_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    РозхіднийКасовийОрдер_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                РозхіднийКасовийОрдер_Objest.ClearSpendTheDocument();
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

                    string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"РозхіднийКасовийОрдер{uid}.xml");
                    РозхіднийКасовийОрдер_Export.ToXmlFile(new РозхіднийКасовийОрдер_Pointer(new UnigueID(uid)), pathToSave);
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