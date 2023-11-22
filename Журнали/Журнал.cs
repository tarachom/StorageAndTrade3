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

using System.Reflection;

using AccountingSoftware;

using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    public abstract class Журнал : VBox
    {
        public UnigueID? SelectPointerItem { get; set; }
        public Перелічення.ТипПеріодуДляЖурналівДокументів PeriodWhere { get; set; } = 0;

        protected TreeView TreeViewGrid = new TreeView();
        protected ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();
        protected ToolButton? TypeDocToolButton; //Список документів
        protected ScrolledWindow ScrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };

        #region Динамічне створення обєктів

        protected Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
        protected string PrefixDocumentObject = "StorageAndTrade_1_0.Документи";

        #endregion

        public Журнал() : base()
        {
            BorderWidth = 0;

            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //Відбір по періоду
            hBoxTop.PackStart(new Label("Період:"), false, false, 5);

            ComboBoxPeriodWhere = ТабличніСписки.Інтерфейс.СписокВідбірПоПеріоду();
            ComboBoxPeriodWhere.Changed += OnComboBoxPeriodWhereChanged;

            hBoxTop.PackStart(ComboBoxPeriodWhere, false, false, 0);

            CreateToolbar();

            ScrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.ButtonReleaseEvent += OnButtonReleaseEvent;
            TreeViewGrid.KeyReleaseEvent += OnKeyReleaseEvent;

            ScrollTree.Add(TreeViewGrid);

            PackStart(ScrollTree, true, true, 0);

            ShowAll();
        }

        #region Menu

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton findButton = new ToolButton(Stock.GoUp) { TooltipText = "Знайти в журналі" };
            findButton.Clicked += OFindClick;
            toolbar.Add(findButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { TooltipText = "Обновити" };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            //Separator
            ToolItem toolItemSeparator = new ToolItem { new Separator(Orientation.Horizontal) };
            toolbar.Add(toolItemSeparator);

            TypeDocToolButton = new ToolButton(Stock.Index) { Label = "Документи", IsImportant = true };
            TypeDocToolButton.Clicked += OnTypeDocsClick;
            toolbar.Add(TypeDocToolButton);

            MenuToolButton provodkyButton = new MenuToolButton(Stock.Find) { Label = "Проводки", IsImportant = true };
            provodkyButton.Clicked += OnReportSpendTheDocumentClick;
            provodkyButton.Menu = ToolbarProvodkySubMenu();
            toolbar.Add(provodkyButton);
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

            MenuItem findButton = new MenuItem("Знайти в журналі");
            findButton.Activated += OFindClick;
            Menu.Append(findButton);

            MenuItem refreshButton = new MenuItem("Обновити");
            refreshButton.Activated += OnRefreshClick;
            Menu.Append(refreshButton);

            MenuItem deleteButton = new MenuItem("Помітка на видалення");
            deleteButton.Activated += OnDeleteClick;
            Menu.Append(deleteButton);

            MenuItem provodkyButton = new MenuItem("Проводки");
            provodkyButton.Activated += OnReportSpendTheDocumentClick;
            Menu.Append(provodkyButton);

            MenuItem spendTheDocumentButton = new MenuItem("Провести документ");
            spendTheDocumentButton.Activated += OnSpendTheDocument;
            Menu.Append(spendTheDocumentButton);

            MenuItem clearSpendButton = new MenuItem("Відмінити проведення");
            clearSpendButton.Activated += OnClearSpend;
            Menu.Append(clearSpendButton);

            Menu.ShowAll();

            return Menu;
        }

        #endregion

        public void SetValue()
        {
            if (PeriodWhere != 0)
                ComboBoxPeriodWhere.ActiveId = PeriodWhere.ToString();
            else if (Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
        }

        public virtual void LoadRecords() { }

        public virtual void OpenTypeListDocs(Widget relative_to) { }

        public virtual void PeriodWhereChanged() { }

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
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    SelectPointerItem = new UnigueID(uid);

                    PopUpContextMenu().Popup();
                }
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
                OpenDocJournal();
        }

        void OpenDocJournal()
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    string typeDoc = (string)TreeViewGrid.Model.GetValue(iter, 2);

                    ФункціїДляДокументів.ВідкритиДокументВідповідноДоВиду(typeDoc, new UnigueID(uid),
                        Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
                }
            }
        }

        void OnKeyReleaseEvent(object? sender, KeyReleaseEventArgs args)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.Insert:
                    {
                        if (TypeDocToolButton != null)
                            OnTypeDocsClick(TypeDocToolButton, new EventArgs());
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
                        OpenDocJournal();
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

        void OnComboBoxPeriodWhereChanged(object? sender, EventArgs args)
        {
            PeriodWhereChanged();
        }

        void OFindClick(object? sender, EventArgs args)
        {
            OpenDocJournal();
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        void OnTypeDocsClick(object? sender, EventArgs args)
        {
            OpenTypeListDocs((ToolButton)sender!);
        }

        void OnDeleteClick(object? sender, EventArgs args)
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

                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                        string typeDoc = (string)TreeViewGrid.Model.GetValue(iter, 2);

                        UnigueID unigueID = new UnigueID(uid);

                        object? docObjest = ExecutingAssembly.CreateInstance($"{PrefixDocumentObject}.{typeDoc}_Objest");
                        if (docObjest != null)
                        {
                            /*
                            Використовується виклик асинхронних методів через синхронні які додатково згенеровані
                            Тобто функція Read викликається через ReadSync, хотя можна викликати Read, 
                            але є проблеми з отриманням результату, треба подумати
                            */
                            object? readObj = docObjest.GetType().InvokeMember("ReadSync", BindingFlags.InvokeMethod, null, docObjest, [unigueID]);
                            if (readObj != null && (bool)readObj)
                            {
                                bool DeletionLabel = (bool)(docObjest.GetType().GetProperty("DeletionLabel")?.GetValue(docObjest) ?? false);
                                docObjest.GetType().InvokeMember("SetDeletionLabelSync", BindingFlags.InvokeMethod, null, docObjest, [!DeletionLabel]);

                                SelectPointerItem = new UnigueID(uid);
                            }
                            else
                                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                        }
                    }

                    LoadRecords();
                }
            }
        }

        void OnReportSpendTheDocumentClick(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    string typeDoc = (string)TreeViewGrid.Model.GetValue(iter, 2);

                    UnigueID unigueID = new UnigueID(uid);

                    object? documentPointer = ExecutingAssembly.CreateInstance($"{PrefixDocumentObject}.{typeDoc}_Pointer", false, BindingFlags.CreateInstance, null, [unigueID, null!], null, null);
                    if (documentPointer != null)
                    {
                        Program.GeneralForm?.CreateNotebookPage($"Проводки", () =>
                        {
                            Звіт_РухДокументівПоРегістрах page = new Звіт_РухДокументівПоРегістрах();
                            page.CreateReport((DocumentPointer)documentPointer);

                            return page;
                        });
                    }
                }
            }
        }

        #endregion

        #region Проведення

        void SpendTheDocument(string uid, string typeDoc, bool spendDoc)
        {
            UnigueID unigueID = new UnigueID(uid);

            object? documentObjest = ExecutingAssembly.CreateInstance($"{PrefixDocumentObject}.{typeDoc}_Objest");
            if (documentObjest != null)
            {
                object? readObj = documentObjest.GetType().InvokeMember("ReadSync", BindingFlags.InvokeMethod, null, documentObjest, [unigueID, true]);
                if (readObj != null && (bool)readObj)
                {
                    if (spendDoc)
                    {
                        DateTime dateDoc = (DateTime)(documentObjest.GetType().GetProperty("ДатаДок")?.GetValue(documentObjest) ?? DateTime.MinValue);

                        object? documentObjestSpend = documentObjest.GetType().InvokeMember("SpendTheDocumentSync", BindingFlags.InvokeMethod, null, documentObjest, [dateDoc]);
                        if (documentObjestSpend != null && !(bool)documentObjestSpend)
                            ФункціїДляПовідомлень.ВідкритиТермінал();
                    }
                    else
                        documentObjest.GetType().InvokeMember("ClearSpendTheDocumentSync", BindingFlags.InvokeMethod, null, documentObjest, null);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
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
                    string typeDoc = (string)TreeViewGrid.Model.GetValue(iter, 2);

                    SpendTheDocument(uid, typeDoc, spend);
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

        #endregion
    }
}