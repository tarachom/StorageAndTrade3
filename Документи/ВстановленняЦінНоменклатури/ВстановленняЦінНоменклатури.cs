using Gtk;

using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури : VBox
    {
        public ВстановленняЦінНоменклатури_Pointer? SelectPointerItem { get; set; }
        public ВстановленняЦінНоменклатури_Pointer? DocumentPointerItem { get; set; }
        public System.Action<ВстановленняЦінНоменклатури_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();

        public ВстановленняЦінНоменклатури(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new ВстановленняЦінНоменклатури_Pointer());

                    Program.GeneralForm?.CloseCurrentPageNotebook();
                };

                hBoxBotton.PackStart(bEmptyPointer, false, false, 10);
            }

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ВстановленняЦінНоменклатури_Записи.Store);
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.AddColumns(TreeViewGrid);

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

            MenuToolButton provodkyButton = new MenuToolButton(Stock.Find) { Label = "Проводки", IsImportant = true };
            provodkyButton.Clicked += OnReportSpendTheDocumentClick;
            provodkyButton.Menu = ToolbarProvodkySubMenu();
            toolbar.Add(provodkyButton);

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

        public void SetValue()
        {
            if ((int)Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
            else
                ComboBoxPeriodWhere.Active = 0;
        }

        public void LoadRecords()
        {
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.LoadRecords();

            if (ТабличніСписки.ВстановленняЦінНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВстановленняЦінНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ВстановленняЦінНоменклатури_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВстановленняЦінНоменклатури_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Встановлення цін номенклатури: *", () =>
                {
                    ВстановленняЦінНоменклатури_Елемент page = new ВстановленняЦінНоменклатури_Елемент
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
                ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
                if (ВстановленняЦінНоменклатури_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВстановленняЦінНоменклатури_Objest.Назва}", () =>
                    {
                        ВстановленняЦінНоменклатури_Елемент page = new ВстановленняЦінНоменклатури_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ВстановленняЦінНоменклатури_Objest = ВстановленняЦінНоменклатури_Objest,
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

                SelectPointerItem = new ВстановленняЦінНоменклатури_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new ВстановленняЦінНоменклатури_Pointer(new UnigueID(uid)));

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

                        ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
                        if (ВстановленняЦінНоменклатури_Objest.Read(new UnigueID(uid)))
                            ВстановленняЦінНоменклатури_Objest.Delete();
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

                        ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest = new ВстановленняЦінНоменклатури_Objest();
                        if (ВстановленняЦінНоменклатури_Objest.Read(new UnigueID(uid)))
                        {
                            ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest_Новий = ВстановленняЦінНоменклатури_Objest.Copy();
                            ВстановленняЦінНоменклатури_Objest_Новий.Назва += " *";
                            ВстановленняЦінНоменклатури_Objest_Новий.ДатаДок = DateTime.Now;
                            ВстановленняЦінНоменклатури_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ВстановленняЦінНоменклатури_Const).ToString("D8");
                            ВстановленняЦінНоменклатури_Objest_Новий.Save();

                            //Зчитати та скопіювати табличну частину Товари
                            ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Read();
                            ВстановленняЦінНоменклатури_Objest_Новий.Товари_TablePart.Records = ВстановленняЦінНоменклатури_Objest.Товари_TablePart.Copy();
                            ВстановленняЦінНоменклатури_Objest_Новий.Товари_TablePart.Save(true);

                            SelectPointerItem = ВстановленняЦінНоменклатури_Objest_Новий.GetDocumentPointer();
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
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
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

                        page.CreateReport(new ВстановленняЦінНоменклатури_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            ВстановленняЦінНоменклатури_Pointer ВстановленняЦінНоменклатури_Pointer = new ВстановленняЦінНоменклатури_Pointer(new UnigueID(uid));
            ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатури_Objest = ВстановленняЦінНоменклатури_Pointer.GetDocumentObject(true);

            //Збереження для запуску тригерів
            ВстановленняЦінНоменклатури_Objest.Save();

            if (spendDoc)
            {
                try
                {
                    if (!ВстановленняЦінНоменклатури_Objest.SpendTheDocument(ВстановленняЦінНоменклатури_Objest.ДатаДок))
                    {
                        ВстановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ВстановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ВстановленняЦінНоменклатури_Objest.ClearSpendTheDocument();
        }

        void OnSpendTheDocument(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    SpendTheDocument(uid, true);
                    LoadRecords();
                }
            }
        }

        void OnClearSpend(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    SpendTheDocument(uid, false);
                    LoadRecords();
                }
            }
        }

        #endregion
    }
}