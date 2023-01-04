using Gtk;

using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ЗамовленняКлієнта : VBox
    {
        public ЗамовленняКлієнта_Pointer? SelectPointerItem { get; set; }
        public ЗамовленняКлієнта_Pointer? DocumentPointerItem { get; set; }
        public System.Action<ЗамовленняКлієнта_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();

        public ЗамовленняКлієнта(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new ЗамовленняКлієнта_Pointer());

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

            TreeViewGrid = new TreeView(ТабличніСписки.ЗамовленняКлієнта_Записи.Store);
            ТабличніСписки.ЗамовленняКлієнта_Записи.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.ButtonReleaseEvent += OnButtonReleaseEvent;
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
            naOsnoviButton.Clicked += OnNaOsnoviClick;
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

            MenuItem newDocRoshidnaNakladnaButton = new MenuItem("Реалізація товарів та послуг");
            newDocRoshidnaNakladnaButton.Activated += OnNewDocNaOsnovi_RoshidnaNakladna;
            Menu.Append(newDocRoshidnaNakladnaButton);

            MenuItem newDocSamovlenjaPostachalnykuButton = new MenuItem("Замовлення постачальнику");
            newDocSamovlenjaPostachalnykuButton.Activated += OnNewDocNaOsnovi_SamovlenjaPostachalnyku;
            Menu.Append(newDocSamovlenjaPostachalnykuButton);

            MenuItem newDocPryhydnaNakladnaButton = new MenuItem("Поступлення товарів та послуг");
            newDocPryhydnaNakladnaButton.Activated += OnNewDocNaOsnovi_PryhydnaNakladna;
            Menu.Append(newDocPryhydnaNakladnaButton);

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

        public void SetValue()
        {
            if ((int)Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
            else
                ComboBoxPeriodWhere.Active = 0;
        }

        public void LoadRecords()
        {
            ТабличніСписки.ЗамовленняКлієнта_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗамовленняКлієнта_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗамовленняКлієнта_Записи.LoadRecords();

            if (ТабличніСписки.ЗамовленняКлієнта_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняКлієнта_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ЗамовленняКлієнта_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняКлієнта_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Замовлення клієнта: *", () =>
                {
                    ЗамовленняКлієнта_Елемент page = new ЗамовленняКлієнта_Елемент
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
                ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
                if (ЗамовленняКлієнта_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняКлієнта_Objest.Назва}", () =>
                    {
                        ЗамовленняКлієнта_Елемент page = new ЗамовленняКлієнта_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ЗамовленняКлієнта_Objest = ЗамовленняКлієнта_Objest,
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

                SelectPointerItem = new ЗамовленняКлієнта_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new ЗамовленняКлієнта_Pointer(new UnigueID(uid)));

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

                        ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
                        if (ЗамовленняКлієнта_Objest.Read(new UnigueID(uid)))
                            ЗамовленняКлієнта_Objest.Delete();
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

                        ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
                        if (ЗамовленняКлієнта_Objest.Read(new UnigueID(uid)))
                        {
                            ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest_Новий = ЗамовленняКлієнта_Objest.Copy();
                            ЗамовленняКлієнта_Objest_Новий.Назва += " *";
                            ЗамовленняКлієнта_Objest_Новий.ДатаДок = DateTime.Now;
                            ЗамовленняКлієнта_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");
                            ЗамовленняКлієнта_Objest_Новий.Save();

                            //Зчитати та скопіювати табличну частину Товари
                            ЗамовленняКлієнта_Objest.Товари_TablePart.Read();
                            ЗамовленняКлієнта_Objest_Новий.Товари_TablePart.Records = ЗамовленняКлієнта_Objest.Товари_TablePart.Copy();
                            ЗамовленняКлієнта_Objest_Новий.Товари_TablePart.Save(true);

                            SelectPointerItem = ЗамовленняКлієнта_Objest_Новий.GetDocumentPointer();
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
            ТабличніСписки.ЗамовленняКлієнта_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
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

                        page.CreateReport(new ЗамовленняКлієнта_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            ЗамовленняКлієнта_Pointer ЗамовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
            ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = ЗамовленняКлієнта_Pointer.GetDocumentObject(true);

            //Збереження для запуску тригерів
            ЗамовленняКлієнта_Objest.Save();

            if (spendDoc)
            {
                try
                {
                    if (!ЗамовленняКлієнта_Objest.SpendTheDocument(ЗамовленняКлієнта_Objest.ДатаДок))
                    {
                        ЗамовленняКлієнта_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ЗамовленняКлієнта_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ЗамовленняКлієнта_Objest.ClearSpendTheDocument();
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

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Новий = new РеалізаціяТоварівТаПослуг_Objest();
                    реалізаціяТоварівТаПослуг_Новий.New();
                    реалізаціяТоварівТаПослуг_Новий.ДатаДок = DateTime.Now;
                    реалізаціяТоварівТаПослуг_Новий.НомерДок = (++Константи.НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");
                    реалізаціяТоварівТаПослуг_Новий.Назва = $"Реалізація товарів та послуг №{реалізаціяТоварівТаПослуг_Новий.НомерДок} від {реалізаціяТоварівТаПослуг_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
                    реалізаціяТоварівТаПослуг_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    реалізаціяТоварівТаПослуг_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    реалізаціяТоварівТаПослуг_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    реалізаціяТоварівТаПослуг_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    реалізаціяТоварівТаПослуг_Новий.Договір = замовленняКлієнта_Objest.Договір;
                    реалізаціяТоварівТаПослуг_Новий.Склад = замовленняКлієнта_Objest.Склад;
                    реалізаціяТоварівТаПослуг_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    реалізаціяТоварівТаПослуг_Новий.Статус = Перелічення.СтатусиРеалізаціїТоварівТаПослуг.ДоОплати;
                    реалізаціяТоварівТаПослуг_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
                    реалізаціяТоварівТаПослуг_Новий.Основа = new UuidAndText(замовленняКлієнта_Objest.UnigueID.UGuid, замовленняКлієнта_Objest.TypeDocument);
                    реалізаціяТоварівТаПослуг_Новий.Save();

                    //Товари
                    foreach (ЗамовленняКлієнта_Товари_TablePart.Record record_замовлення in замовленняКлієнта_Objest.Товари_TablePart.Records)
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
                        record_реалізація.ЗамовленняКлієнта = замовленняКлієнта_Objest.GetDocumentPointer();
                        record_реалізація.Склад = замовленняКлієнта_Objest.Склад;
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

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    ЗамовленняПостачальнику_Objest замовленняПостачальнику_Новий = new ЗамовленняПостачальнику_Objest();
                    замовленняПостачальнику_Новий.New();
                    замовленняПостачальнику_Новий.ДатаДок = DateTime.Now;
                    замовленняПостачальнику_Новий.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
                    замовленняПостачальнику_Новий.Назва = $"Замовлення постачальнику №{замовленняПостачальнику_Новий.НомерДок} від {замовленняПостачальнику_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
                    замовленняПостачальнику_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    замовленняПостачальнику_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    замовленняПостачальнику_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    замовленняПостачальнику_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    замовленняПостачальнику_Новий.Договір = ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(замовленняКлієнта_Objest.Контрагент, Перелічення.ТипДоговорів.ЗПостачальниками) ?? замовленняКлієнта_Objest.Договір;
                    замовленняПостачальнику_Новий.Склад = замовленняКлієнта_Objest.Склад;
                    замовленняПостачальнику_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    замовленняПостачальнику_Новий.Статус = Перелічення.СтатусиЗамовленьПостачальникам.Підтверджений;
                    замовленняПостачальнику_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
                    замовленняПостачальнику_Новий.Основа = new UuidAndText(замовленняКлієнта_Objest.UnigueID.UGuid, замовленняКлієнта_Objest.TypeDocument);
                    замовленняПостачальнику_Новий.Save();

                    //Товари
                    foreach (ЗамовленняКлієнта_Товари_TablePart.Record record_замовлення in замовленняКлієнта_Objest.Товари_TablePart.Records)
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
                        record_замовленняПостачальнику.Склад = замовленняКлієнта_Objest.Склад;
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

        void OnNewDocNaOsnovi_PryhydnaNakladna(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Новий = new ПоступленняТоварівТаПослуг_Objest();
                    поступленняТоварівТаПослуг_Новий.New();
                    поступленняТоварівТаПослуг_Новий.ДатаДок = DateTime.Now;
                    поступленняТоварівТаПослуг_Новий.НомерДок = (++Константи.НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
                    поступленняТоварівТаПослуг_Новий.Назва = $"Поступлення товарів та послуг №{поступленняТоварівТаПослуг_Новий.НомерДок} від {поступленняТоварівТаПослуг_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
                    поступленняТоварівТаПослуг_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    поступленняТоварівТаПослуг_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    поступленняТоварівТаПослуг_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    поступленняТоварівТаПослуг_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    поступленняТоварівТаПослуг_Новий.Договір = ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(замовленняКлієнта_Objest.Контрагент, Перелічення.ТипДоговорів.ЗПостачальниками) ?? замовленняКлієнта_Objest.Договір;
                    поступленняТоварівТаПослуг_Новий.Склад = замовленняКлієнта_Objest.Склад;
                    поступленняТоварівТаПослуг_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    поступленняТоварівТаПослуг_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
                    поступленняТоварівТаПослуг_Новий.Основа = new UuidAndText(замовленняКлієнта_Objest.UnigueID.UGuid, замовленняКлієнта_Objest.TypeDocument);
                    поступленняТоварівТаПослуг_Новий.Save();

                    //Товари
                    foreach (ЗамовленняКлієнта_Товари_TablePart.Record record_замовлення in замовленняКлієнта_Objest.Товари_TablePart.Records)
                    {
                        ПоступленняТоварівТаПослуг_Товари_TablePart.Record record_поступлення = new ПоступленняТоварівТаПослуг_Товари_TablePart.Record();
                        поступленняТоварівТаПослуг_Новий.Товари_TablePart.Records.Add(record_поступлення);

                        record_поступлення.Номенклатура = record_замовлення.Номенклатура;
                        record_поступлення.ХарактеристикаНоменклатури = record_замовлення.ХарактеристикаНоменклатури;
                        record_поступлення.Пакування = record_замовлення.Пакування;
                        record_поступлення.КількістьУпаковок = record_замовлення.КількістьУпаковок;
                        record_поступлення.Кількість = record_замовлення.Кількість;
                        record_поступлення.Ціна = record_замовлення.Ціна;
                        record_поступлення.Сума = record_замовлення.Сума;
                        record_поступлення.Скидка = record_замовлення.Скидка;
                        //record_поступлення.ЗамовленняПостачальнику = замовленняПостачальнику_Objest.GetDocumentPointer();
                        record_поступлення.Склад = замовленняКлієнта_Objest.Склад;
                    }

                    поступленняТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

                    Program.GeneralForm?.CreateNotebookPage($"{поступленняТоварівТаПослуг_Новий.Назва}", () =>
                    {
                        ПоступленняТоварівТаПослуг_Елемент page = new ПоступленняТоварівТаПослуг_Елемент
                        {
                            IsNew = false,
                            ПоступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Новий,
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