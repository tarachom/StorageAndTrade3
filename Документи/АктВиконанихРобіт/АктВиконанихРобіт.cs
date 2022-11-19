using Gtk;

using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class АктВиконанихРобіт : VBox
    {
        public АктВиконанихРобіт_Pointer? SelectPointerItem { get; set; }
        public АктВиконанихРобіт_Pointer? DocumentPointerItem { get; set; }
        public System.Action<АктВиконанихРобіт_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();

        public АктВиконанихРобіт(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new АктВиконанихРобіт_Pointer());

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
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.АктВиконанихРобіт_Записи.Store);
            ТабличніСписки.АктВиконанихРобіт_Записи.AddColumns(TreeViewGrid);

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

            MenuToolButton naOsnoviButton = new MenuToolButton(Stock.New) { Label = "Ввести на основі", IsImportant = true };
            naOsnoviButton.Menu = ToolbarNaOsnoviSubMenu();
            toolbar.Add(naOsnoviButton);
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

            MenuItem newDocKasovyiOrderButton = new MenuItem("Прихідний касовий ордер");
            newDocKasovyiOrderButton.Activated += OnNewDocNaOsnovi_KasovyiOrder;
            Menu.Append(newDocKasovyiOrderButton);

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
            ТабличніСписки.АктВиконанихРобіт_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.АктВиконанихРобіт_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.АктВиконанихРобіт_Записи.LoadRecords();

            if (ТабличніСписки.АктВиконанихРобіт_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.АктВиконанихРобіт_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.АктВиконанихРобіт_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.АктВиконанихРобіт_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Акт виконаних робіт *", () =>
                {
                    АктВиконанихРобіт_Елемент page = new АктВиконанихРобіт_Елемент
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
                АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest = new АктВиконанихРобіт_Objest();
                if (АктВиконанихРобіт_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{АктВиконанихРобіт_Objest.Назва}", () =>
                    {
                        АктВиконанихРобіт_Елемент page = new АктВиконанихРобіт_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            АктВиконанихРобіт_Objest = АктВиконанихРобіт_Objest,
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

                SelectPointerItem = new АктВиконанихРобіт_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new АктВиконанихРобіт_Pointer(new UnigueID(uid)));

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

                        АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest = new АктВиконанихРобіт_Objest();
                        if (АктВиконанихРобіт_Objest.Read(new UnigueID(uid)))
                            АктВиконанихРобіт_Objest.Delete();
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

                        АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest = new АктВиконанихРобіт_Objest();
                        if (АктВиконанихРобіт_Objest.Read(new UnigueID(uid)))
                        {
                            АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest_Новий = АктВиконанихРобіт_Objest.Copy();
                            АктВиконанихРобіт_Objest_Новий.Назва += " *";
                            АктВиконанихРобіт_Objest_Новий.ДатаДок = DateTime.Now;
                            АктВиконанихРобіт_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.АктВиконанихРобіт_Const).ToString("D8");
                            АктВиконанихРобіт_Objest_Новий.Save();

                            //Зчитати та скопіювати табличну частину Товари
                            АктВиконанихРобіт_Objest.Послуги_TablePart.Read();
                            АктВиконанихРобіт_Objest_Новий.Послуги_TablePart.Records = АктВиконанихРобіт_Objest.Послуги_TablePart.Copy();
                            АктВиконанихРобіт_Objest_Новий.Послуги_TablePart.Save(true);

                            SelectPointerItem = АктВиконанихРобіт_Objest_Новий.GetDocumentPointer();
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
            ТабличніСписки.АктВиконанихРобіт_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
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

                        page.CreateReport(new АктВиконанихРобіт_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            АктВиконанихРобіт_Pointer АктВиконанихРобіт_Pointer = new АктВиконанихРобіт_Pointer(new UnigueID(uid));
            АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest = АктВиконанихРобіт_Pointer.GetDocumentObject(true);

            //Збереження для запуску тригерів
            АктВиконанихРобіт_Objest.Save();

            if (spendDoc)
            {
                try
                {
                    if (!АктВиконанихРобіт_Objest.SpendTheDocument(АктВиконанихРобіт_Objest.ДатаДок))
                    {
                        АктВиконанихРобіт_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    АктВиконанихРобіт_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                АктВиконанихРобіт_Objest.ClearSpendTheDocument();
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

        void OnNewDocNaOsnovi_KasovyiOrder(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    АктВиконанихРобіт_Pointer актВиконанихРобіт_Pointer = new АктВиконанихРобіт_Pointer(new UnigueID(uid));
                    АктВиконанихРобіт_Objest актВиконанихРобіт_Objest = актВиконанихРобіт_Pointer.GetDocumentObject(false);

                    //
                    //Новий документ
                    //

                    ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Новий = new ПрихіднийКасовийОрдер_Objest();
                    прихіднийКасовийОрдер_Новий.New();
                    прихіднийКасовийОрдер_Новий.ДатаДок = DateTime.Now;
                    прихіднийКасовийОрдер_Новий.НомерДок = (++Константи.НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
                    прихіднийКасовийОрдер_Новий.Назва = $"Прихідний касовий ордер №{прихіднийКасовийОрдер_Новий.НомерДок} від {прихіднийКасовийОрдер_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
                    прихіднийКасовийОрдер_Новий.Організація = актВиконанихРобіт_Objest.Організація;
                    прихіднийКасовийОрдер_Новий.Валюта = актВиконанихРобіт_Objest.Валюта;
                    прихіднийКасовийОрдер_Новий.Каса = актВиконанихРобіт_Objest.Каса;
                    прихіднийКасовийОрдер_Новий.Контрагент = актВиконанихРобіт_Objest.Контрагент;
                    прихіднийКасовийОрдер_Новий.Договір = актВиконанихРобіт_Objest.Договір;
                    прихіднийКасовийОрдер_Новий.Основа = new UuidAndText(актВиконанихРобіт_Objest.UnigueID.UGuid, актВиконанихРобіт_Objest.TypeDocument);
                    прихіднийКасовийОрдер_Новий.СумаДокументу = актВиконанихРобіт_Objest.СумаДокументу;
                    прихіднийКасовийОрдер_Новий.Save();

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

        #endregion
    }
}