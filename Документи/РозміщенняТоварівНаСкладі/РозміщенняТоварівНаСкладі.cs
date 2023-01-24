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
    class РозміщенняТоварівНаСкладі : VBox
    {
        public РозміщенняТоварівНаСкладі_Pointer? SelectPointerItem { get; set; }
        public РозміщенняТоварівНаСкладі_Pointer? DocumentPointerItem { get; set; }
        public System.Action<РозміщенняТоварівНаСкладі_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();

        public РозміщенняТоварівНаСкладі(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new РозміщенняТоварівНаСкладі_Pointer());

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

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.Store);
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);

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

            MenuToolButton printingButton = new MenuToolButton(Stock.Print){ TooltipText = "Друк" };
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

            MenuItem invoiceButton = new MenuItem("Накладна на розміщення товарів");
            invoiceButton.Activated += OnPrintingInvoiceClick;
            Menu.Append(invoiceButton);

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
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.LoadRecords();

            if (ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Розміщення товарів на складі: *", () =>
                {
                    РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
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
                РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
                if (РозміщенняТоварівНаСкладі_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{РозміщенняТоварівНаСкладі_Objest.Назва}", () =>
                    {
                        РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            РозміщенняТоварівНаСкладі_Objest = РозміщенняТоварівНаСкладі_Objest,
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

                SelectPointerItem = new РозміщенняТоварівНаСкладі_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new РозміщенняТоварівНаСкладі_Pointer(new UnigueID(uid)));

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

                        РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
                        if (РозміщенняТоварівНаСкладі_Objest.Read(new UnigueID(uid)))
                            РозміщенняТоварівНаСкладі_Objest.Delete();
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

                        РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
                        if (РозміщенняТоварівНаСкладі_Objest.Read(new UnigueID(uid)))
                        {
                            РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest_Новий = РозміщенняТоварівНаСкладі_Objest.Copy();
                            РозміщенняТоварівНаСкладі_Objest_Новий.Назва += " *";
                            РозміщенняТоварівНаСкладі_Objest_Новий.ДатаДок = DateTime.Now;
                            РозміщенняТоварівНаСкладі_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.РозміщенняТоварівНаСкладі_Const).ToString("D8");
                            РозміщенняТоварівНаСкладі_Objest_Новий.Save();

                            //Зчитати та скопіювати табличну частину Товари
                            РозміщенняТоварівНаСкладі_Objest.Товари_TablePart.Read();
                            РозміщенняТоварівНаСкладі_Objest_Новий.Товари_TablePart.Records = РозміщенняТоварівНаСкладі_Objest.Товари_TablePart.Copy();
                            РозміщенняТоварівНаСкладі_Objest_Новий.Товари_TablePart.Save(true);

                            SelectPointerItem = РозміщенняТоварівНаСкладі_Objest_Новий.GetDocumentPointer();
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
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
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

                        page.CreateReport(new РозміщенняТоварівНаСкладі_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            РозміщенняТоварівНаСкладі_Pointer РозміщенняТоварівНаСкладі_Pointer = new РозміщенняТоварівНаСкладі_Pointer(new UnigueID(uid));
            РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = РозміщенняТоварівНаСкладі_Pointer.GetDocumentObject(true);

            //Збереження для запуску тригерів
            РозміщенняТоварівНаСкладі_Objest.Save();

            if (spendDoc)
            {
                try
                {
                    if (!РозміщенняТоварівНаСкладі_Objest.SpendTheDocument(РозміщенняТоварівНаСкладі_Objest.ДатаДок))
                    {
                        РозміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    РозміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                РозміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
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