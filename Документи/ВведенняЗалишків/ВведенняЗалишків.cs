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

        public ВведенняЗалишків() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ВведенняЗалишків_Записи.Store);
            ТабличніСписки.ВведенняЗалишків_Записи.AddColumns(TreeViewGrid);

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
            ТабличніСписки.ВведенняЗалишків_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВведенняЗалишків_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ВведенняЗалишків_Записи.LoadRecords();

            if (ТабличніСписки.ВведенняЗалишків_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВведенняЗалишків_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ВведенняЗалишків_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВведенняЗалишків_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
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
                });
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

                SelectPointerItem = new ВведенняЗалишків_Pointer(unigueID);
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

        #endregion

    }
}