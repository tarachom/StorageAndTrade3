using Gtk;

using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ЗамовленняПостачальнику : VBox
    {
        public ЗамовленняПостачальнику_Pointer? SelectPointerItem { get; set; }
        public ЗамовленняПостачальнику_Pointer? DocumentPointerItem { get; set; }
        public System.Action<ЗамовленняПостачальнику_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();

        public ЗамовленняПостачальнику(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new ЗамовленняПостачальнику_Pointer());

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

            TreeViewGrid = new TreeView(ТабличніСписки.ЗамовленняПостачальнику_Записи.Store);
            ТабличніСписки.ЗамовленняПостачальнику_Записи.AddColumns(TreeViewGrid);

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

            MenuItem newDocPryhydnaNakladnaButton = new MenuItem("Поступлення товарів та послуг");
            newDocPryhydnaNakladnaButton.Activated += OnNewDocNaOsnovi_PryhydnaNakladna;
            Menu.Append(newDocPryhydnaNakladnaButton);

            MenuItem newDocKasovyiOrderButton = new MenuItem("Розхідний касовий ордер");
            newDocKasovyiOrderButton.Activated += OnNewDocNaOsnovi_KasovyiOrder;
            Menu.Append(newDocKasovyiOrderButton);

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
            ТабличніСписки.ЗамовленняПостачальнику_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗамовленняПостачальнику_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗамовленняПостачальнику_Записи.LoadRecords();

            if (ТабличніСписки.ЗамовленняПостачальнику_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняПостачальнику_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ЗамовленняПостачальнику_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняПостачальнику_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Замовлення постачальнику: *", () =>
                {
                    ЗамовленняПостачальнику_Елемент page = new ЗамовленняПостачальнику_Елемент
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
                ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest = new ЗамовленняПостачальнику_Objest();
                if (ЗамовленняПостачальнику_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняПостачальнику_Objest.Назва}", () =>
                    {
                        ЗамовленняПостачальнику_Елемент page = new ЗамовленняПостачальнику_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ЗамовленняПостачальнику_Objest = ЗамовленняПостачальнику_Objest,
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

                SelectPointerItem = new ЗамовленняПостачальнику_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new ЗамовленняПостачальнику_Pointer(new UnigueID(uid)));

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

                        ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest = new ЗамовленняПостачальнику_Objest();
                        if (ЗамовленняПостачальнику_Objest.Read(new UnigueID(uid)))
                            ЗамовленняПостачальнику_Objest.Delete();
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

                        ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest = new ЗамовленняПостачальнику_Objest();
                        if (ЗамовленняПостачальнику_Objest.Read(new UnigueID(uid)))
                        {
                            ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest_Новий = ЗамовленняПостачальнику_Objest.Copy();
                            ЗамовленняПостачальнику_Objest_Новий.Назва += " *";
                            ЗамовленняПостачальнику_Objest_Новий.ДатаДок = DateTime.Now;
                            ЗамовленняПостачальнику_Objest_Новий.НомерДок = (++Константи.НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
                            ЗамовленняПостачальнику_Objest_Новий.Save();

                            //Зчитати та скопіювати табличну частину Товари
                            ЗамовленняПостачальнику_Objest.Товари_TablePart.Read();
                            ЗамовленняПостачальнику_Objest_Новий.Товари_TablePart.Records = ЗамовленняПостачальнику_Objest.Товари_TablePart.Copy();
                            ЗамовленняПостачальнику_Objest_Новий.Товари_TablePart.Save(true);

                            SelectPointerItem = ЗамовленняПостачальнику_Objest_Новий.GetDocumentPointer();
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
            ТабличніСписки.ЗамовленняПостачальнику_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
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

                        page.CreateReport(new ЗамовленняПостачальнику_Pointer(new UnigueID(uid)));

                        return page;
                    });
                }
            }
        }

        void SpendTheDocument(string uid, bool spendDoc)
        {
            ЗамовленняПостачальнику_Pointer ЗамовленняПостачальнику_Pointer = new ЗамовленняПостачальнику_Pointer(new UnigueID(uid));
            ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest = ЗамовленняПостачальнику_Pointer.GetDocumentObject(true);

            //Збереження для запуску тригерів
            ЗамовленняПостачальнику_Objest.Save();

            if (spendDoc)
            {
                try
                {
                    if (!ЗамовленняПостачальнику_Objest.SpendTheDocument(ЗамовленняПостачальнику_Objest.ДатаДок))
                    {
                        ЗамовленняПостачальнику_Objest.ClearSpendTheDocument();
                        ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                }
                catch (Exception exp)
                {
                    ЗамовленняПостачальнику_Objest.ClearSpendTheDocument();
                    Message.Error(Program.GeneralForm, exp.Message);
                    return;
                }
            }
            else
                ЗамовленняПостачальнику_Objest.ClearSpendTheDocument();
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

                    ЗамовленняПостачальнику_Pointer замовленняПостачальнику_Pointer = new ЗамовленняПостачальнику_Pointer(new UnigueID(uid));
                    ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = замовленняПостачальнику_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Новий = new ПоступленняТоварівТаПослуг_Objest();
                    поступленняТоварівТаПослуг_Новий.New();
                    поступленняТоварівТаПослуг_Новий.ДатаДок = DateTime.Now;
                    поступленняТоварівТаПослуг_Новий.НомерДок = (++Константи.НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
                    поступленняТоварівТаПослуг_Новий.Назва = $"Поступлення товарів та послуг №{поступленняТоварівТаПослуг_Новий.НомерДок} від {поступленняТоварівТаПослуг_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
                    поступленняТоварівТаПослуг_Новий.Організація = замовленняПостачальнику_Objest.Організація;
                    поступленняТоварівТаПослуг_Новий.Валюта = замовленняПостачальнику_Objest.Валюта;
                    поступленняТоварівТаПослуг_Новий.Каса = замовленняПостачальнику_Objest.Каса;
                    поступленняТоварівТаПослуг_Новий.Контрагент = замовленняПостачальнику_Objest.Контрагент;
                    поступленняТоварівТаПослуг_Новий.Договір = замовленняПостачальнику_Objest.Договір;
                    поступленняТоварівТаПослуг_Новий.Склад = замовленняПостачальнику_Objest.Склад;
                    поступленняТоварівТаПослуг_Новий.СумаДокументу = замовленняПостачальнику_Objest.СумаДокументу;
                    поступленняТоварівТаПослуг_Новий.ФормаОплати = замовленняПостачальнику_Objest.ФормаОплати;
                    поступленняТоварівТаПослуг_Новий.ЗамовленняПостачальнику = замовленняПостачальнику_Objest.GetDocumentPointer();
                    поступленняТоварівТаПослуг_Новий.Основа = new UuidAndText(замовленняПостачальнику_Objest.UnigueID.UGuid, замовленняПостачальнику_Objest.TypeDocument);
                    поступленняТоварівТаПослуг_Новий.Save();

                    //Товари
                    foreach (ЗамовленняПостачальнику_Товари_TablePart.Record record_замовлення in замовленняПостачальнику_Objest.Товари_TablePart.Records)
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
                        record_поступлення.ЗамовленняПостачальнику = замовленняПостачальнику_Objest.GetDocumentPointer();
                        record_поступлення.Склад = замовленняПостачальнику_Objest.Склад;
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

                    ЗамовленняПостачальнику_Pointer замовленняПостачальнику_Pointer = new ЗамовленняПостачальнику_Pointer(new UnigueID(uid));
                    ЗамовленняПостачальнику_Objest замовленняПостачальнику_Objest = замовленняПостачальнику_Pointer.GetDocumentObject(true);

                    //
                    //Новий документ
                    //

                    РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Новий = new РозхіднийКасовийОрдер_Objest();
                    розхіднийКасовийОрдер_Новий.New();
                    розхіднийКасовийОрдер_Новий.ДатаДок = DateTime.Now;
                    розхіднийКасовийОрдер_Новий.НомерДок = (++Константи.НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");
                    розхіднийКасовийОрдер_Новий.Назва = $"Розхідний касовий ордер №{розхіднийКасовийОрдер_Новий.НомерДок} від {розхіднийКасовийОрдер_Новий.ДатаДок.ToString("dd.MM.yyyy")}";
                    розхіднийКасовийОрдер_Новий.Організація = замовленняПостачальнику_Objest.Організація;
                    розхіднийКасовийОрдер_Новий.Валюта = замовленняПостачальнику_Objest.Валюта;
                    розхіднийКасовийОрдер_Новий.Каса = замовленняПостачальнику_Objest.Каса;
                    розхіднийКасовийОрдер_Новий.Контрагент = замовленняПостачальнику_Objest.Контрагент;
                    розхіднийКасовийОрдер_Новий.Договір = замовленняПостачальнику_Objest.Договір;
                    розхіднийКасовийОрдер_Новий.СумаДокументу = замовленняПостачальнику_Objest.СумаДокументу;
                    розхіднийКасовийОрдер_Новий.Основа = new UuidAndText(замовленняПостачальнику_Objest.UnigueID.UGuid, замовленняПостачальнику_Objest.TypeDocument);
                    розхіднийКасовийОрдер_Новий.Save();


                    Program.GeneralForm?.CreateNotebookPage($"{розхіднийКасовийОрдер_Новий.Назва}", () =>
                    {
                        РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент
                        {
                            IsNew = false,
                            РозхіднийКасовийОрдер_Objest = розхіднийКасовийОрдер_Новий,
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