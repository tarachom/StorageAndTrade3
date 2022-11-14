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

        public ПоступленняТоварівТаПослуг(bool IsSelectPointer = false) : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

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

                hBoxBotton.PackStart(bEmptyPointer, false, false, 10);
            }

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.Store);
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);

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

            ToolButton addButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true };
            addButton.Clicked += OnAddClick;
            toolbar.Add(addButton);

            ToolButton upButton = new ToolButton(Stock.Edit) { Label = "Редагувати", IsImportant = true };
            upButton.Clicked += OnEditClick;
            toolbar.Add(upButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { Label = "Копіювати", IsImportant = true };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { Label = "Обновити", IsImportant = true };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);

            ToolButton reportSpendTheDocumentButton = new ToolButton(Stock.Refresh) { Label = "Проводки", IsImportant = true };
            reportSpendTheDocumentButton.Clicked += OnReportSpendTheDocumentClick;
            toolbar.Add(reportSpendTheDocumentButton);

            //Відбір
            ToolItem seperatorOne = new ToolItem();
            seperatorOne.Add(new Separator(Orientation.Vertical));
            toolbar.Add(seperatorOne);

            HBox hBoxPeriodWhere = new HBox();
            hBoxPeriodWhere.PackStart(new Label("Період:"), false, false, 5);

            ComboBoxPeriodWhere = ТабличніСписки.Інтерфейс.СписокВідбірПоПеріоду();
            ComboBoxPeriodWhere.Changed += OnComboBoxPeriodWhereChanged;

            hBoxPeriodWhere.PackStart(ComboBoxPeriodWhere, false, false, 0);

            ToolItem periodWhere = new ToolItem();
            periodWhere.Add(hBoxPeriodWhere);
            toolbar.Add(periodWhere);
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
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords();

            if (ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
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
                });
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
                    });
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

        #endregion

    }
}