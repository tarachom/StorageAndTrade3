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
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public abstract class ДокументЖурнал : VBox
    {
        /// <summary>
        /// Для позиціювання в списку
        /// </summary>
        public UnigueID? SelectPointerItem { get; set; }

        /// <summary>
        /// Для вибору і позиціювання
        /// </summary>
        public UnigueID? DocumentPointerItem { get; set; }

        /// <summary>
        /// Функція зворотнього виклику при виборі
        /// </summary>
        public Action<UnigueID>? CallBack_OnSelectPointer { get; set; }

        /// <summary>
        /// Період для журналу
        /// </summary>
        public Перелічення.ТипПеріодуДляЖурналівДокументів PeriodWhere { get; set; } = 0;

        /// <summary>
        /// Верхній набір меню
        /// </summary>
        protected Toolbar ToolbarTop = new Toolbar();

        /// <summary>
        /// Верхній бок для додаткових кнопок
        /// </summary>
        protected HBox HBoxTop = new HBox();

        /// <summary>
        /// Дерево
        /// </summary>
        protected TreeView TreeViewGrid = new TreeView();

        /// <summary>
        /// Набір варіантів періодів
        /// </summary>
        protected ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();

        /// <summary>
        /// Пошук
        /// </summary>
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public ДокументЖурнал() : base()
        {
            BorderWidth = 0;

            //Кнопки
            PackStart(HBoxTop, false, false, 10);

            //Відбір по періоду
            HBoxTop.PackStart(new Label("Період:"), false, false, 5);

            ComboBoxPeriodWhere = ТабличніСписки.Інтерфейс.СписокВідбірПоПеріоду();
            ComboBoxPeriodWhere.Changed += OnComboBoxPeriodWhereChanged;
            HBoxTop.PackStart(ComboBoxPeriodWhere, false, false, 0);

            //Пошук
            ПошукПовнотекстовий.Select = LoadRecords_OnSearch;
            ПошукПовнотекстовий.Clear = () => { OnComboBoxPeriodWhereChanged(null, new EventArgs()); };
            HBoxTop.PackStart(ПошукПовнотекстовий, false, false, 2);

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

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

        public void SetValue()
        {
            if (PeriodWhere != 0)
                ComboBoxPeriodWhere.ActiveId = PeriodWhere.ToString();
            else if (Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
        }

        #region Toolbar & Menu

        void CreateToolbar()
        {
            PackStart(ToolbarTop, false, false, 0);

            ToolButton addButton = new ToolButton(Stock.Add) { TooltipText = "Додати" };
            addButton.Clicked += OnAddClick;
            ToolbarTop.Add(addButton);

            ToolButton upButton = new ToolButton(Stock.Edit) { TooltipText = "Редагувати" };
            upButton.Clicked += OnEditClick;
            ToolbarTop.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { TooltipText = "Копіювати" };
            copyButton.Clicked += OnCopyClick;
            ToolbarTop.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            ToolbarTop.Add(deleteButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { TooltipText = "Обновити" };
            refreshButton.Clicked += OnRefreshClick;
            ToolbarTop.Add(refreshButton);

            //Separator
            ToolItem toolItemSeparator = new ToolItem
            {
                new Separator(Orientation.Horizontal)
            };
            ToolbarTop.Add(toolItemSeparator);

            MenuToolButton provodkyButton = new MenuToolButton(Stock.Find) { Label = "Проводки", IsImportant = true };
            provodkyButton.Clicked += OnReportSpendTheDocumentClick;
            provodkyButton.Menu = ToolbarProvodkySubMenu();
            ToolbarTop.Add(provodkyButton);

            Menu? menuItem = ToolbarNaOsnoviSubMenu();
            if (menuItem != null)
            {
                MenuToolButton naOsnoviButton = new MenuToolButton(Stock.New) { Label = "Ввести на основі", IsImportant = true };
                naOsnoviButton.Clicked += (object? sender, EventArgs arg) => { ((Menu)((MenuToolButton)sender!).Menu).Popup(); };
                naOsnoviButton.Menu = menuItem;
                ToolbarTop.Add(naOsnoviButton);
            }

            MenuToolButton printingButton = new MenuToolButton(Stock.Print) { TooltipText = "Друк" };
            printingButton.Clicked += (object? sender, EventArgs arg) => { ((Menu)((MenuToolButton)sender!).Menu).Popup(); };
            printingButton.Menu = ToolbarPrintingSubMenu();
            ToolbarTop.Add(printingButton);

            //Експорт
            MenuToolButton exportButton = new MenuToolButton(Stock.Convert) { Label = "Експорт", IsImportant = true };
            exportButton.Clicked += (object? sender, EventArgs arg) => { ((Menu)((MenuToolButton)sender!).Menu).Popup(); };
            exportButton.Menu = ToolbarExportSubMenu();
            ToolbarTop.Add(exportButton);
        }

        Menu ToolbarProvodkySubMenu()
        {
            Menu Menu = new Menu();

            MenuItem spendTheDocumentButton = new MenuItem("Провести документ");
            spendTheDocumentButton.Activated += (object? sender, EventArgs args) => { SpendTheDocumentOrClear(true); };
            Menu.Append(spendTheDocumentButton);

            MenuItem clearSpendButton = new MenuItem("Відмінити проведення");
            clearSpendButton.Activated += (object? sender, EventArgs args) => { SpendTheDocumentOrClear(false); };
            Menu.Append(clearSpendButton);

            Menu.ShowAll();

            return Menu;
        }

        Menu PopUpContextMenu()
        {
            Menu Menu = new Menu();

            MenuItem spendTheDocumentButton = new MenuItem("Провести документ");
            spendTheDocumentButton.Activated += (object? sender, EventArgs args) => { SpendTheDocumentOrClear(true); };
            Menu.Append(spendTheDocumentButton);

            MenuItem clearSpendButton = new MenuItem("Відмінити проведення");
            clearSpendButton.Activated += (object? sender, EventArgs args) => { SpendTheDocumentOrClear(false); };
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

        #region Virtual & Abstract Function

        protected virtual Menu? ToolbarNaOsnoviSubMenu() { return null; }

        public abstract void LoadRecords();

        protected abstract void LoadRecords_OnSearch(string searchText);

        protected abstract void OpenPageElement(bool IsNew, UnigueID? unigueID = null);

        protected abstract ValueTask SetDeletionLabel(UnigueID unigueID);

        protected abstract ValueTask<UnigueID?> Copy(UnigueID unigueID);

        public virtual void CallBack_LoadRecords(UnigueID? selectPointer)
        {
            SelectPointerItem = selectPointer;
            LoadRecords();
        }

        protected abstract void PeriodWhereChanged();

        protected abstract ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc);

        protected abstract DocumentPointer? ReportSpendTheDocument(UnigueID unigueID);

        protected virtual void ExportXML(UnigueID unigueID) { }

        #endregion

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

        void OnButtonReleaseEvent(object? sender, ButtonReleaseEventArgs args)
        {
            if (args.Event.Button == 3 && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    SelectPointerItem = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));
                    PopUpContextMenu().Popup();
                }
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                    if (DocumentPointerItem == null)
                        OpenPageElement(false, unigueID);
                    else
                    {
                        if (CallBack_OnSelectPointer != null)
                            CallBack_OnSelectPointer.Invoke(unigueID);

                        Program.GeneralForm?.CloseNotebookPageToCode(this.Name);
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
                        UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));
                        OpenPageElement(false, unigueID);
                    }
                }
            }
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        async void OnDeleteClick(object? sender, EventArgs args)
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

                        UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                        await SetDeletionLabel(unigueID);

                        SelectPointerItem = unigueID;
                    }

                    LoadRecords();
                }
            }
        }

        async void OnCopyClick(object? sender, EventArgs args)
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

                        UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                        UnigueID? newUnigueID = await Copy(unigueID);

                        if (newUnigueID != null)
                            SelectPointerItem = newUnigueID;
                    }

                    LoadRecords();
                }
            }
        }

        void OnComboBoxPeriodWhereChanged(object? sender, EventArgs args)
        {
            PeriodWhereChanged();
        }

        void OnReportSpendTheDocumentClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                    DocumentPointer? documentPointer = ReportSpendTheDocument(unigueID);

                    if (documentPointer != null)
                        Program.GeneralForm?.CreateNotebookPage($"Проводки", () =>
                        {
                            Звіт_РухДокументівПоРегістрах page = new Звіт_РухДокументівПоРегістрах();
                            page.CreateReport(documentPointer);
                            return page;
                        });
                }
            }
        }

        async void SpendTheDocumentOrClear(bool spend)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                    await SpendTheDocument(unigueID, spend);

                    SelectPointerItem = unigueID;
                }

                LoadRecords();
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

                    UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                    ExportXML(unigueID);
                }

                LoadRecords();
            }
        }

        void OnPrintingInvoiceClick(object? sender, EventArgs arg)
        {

        }

        #endregion
    }
}