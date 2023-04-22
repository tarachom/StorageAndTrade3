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

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Контрагенти : VBox
    {
        public Контрагенти_Pointer? SelectPointerItem { get; set; }
        public Контрагенти_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<Контрагенти_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        Контрагенти_Папки_Дерево ДеревоПапок;
        CheckButton checkButtonIsHierarchy = new CheckButton("Враховувати ієрархію папок") { Active = true };
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public Контрагенти() : base()
        {
            BorderWidth = 0;

            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += OnCheckButtonIsHierarchyClicked;
            hBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Пошук 2
            hBoxTop.PackStart(ПошукПовнотекстовий, false, false, 2);
            ПошукПовнотекстовий.Select = LoadRecords_OnSearch;
            ПошукПовнотекстовий.Clear = LoadRecords;

            //Договори
            {
                LinkButton linkButton = new LinkButton(" Договори") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkButton.Clicked += (object? sender, EventArgs args) =>
                {
                    ДоговориКонтрагентів page = new ДоговориКонтрагентів();

                    if (SelectPointerItem != null)
                        page.КонтрагентВласник.Pointer = SelectPointerItem;

                    Program.GeneralForm?.CreateNotebookPage("Договори", () => { return page; });

                    page.LoadRecords();
                };

                hBoxTop.PackStart(linkButton, false, false, 10);
            }

            CreateToolbar();

            HPaned hPaned = new HPaned();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.Контрагенти_Записи.Store);
            ТабличніСписки.Контрагенти_Записи.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.ButtonReleaseEvent += OnButtonReleaseEvent;
            TreeViewGrid.KeyReleaseEvent += OnKeyReleaseEvent;

            scrollTree.Add(TreeViewGrid);

            hPaned.Pack1(scrollTree, true, true);

            ДеревоПапок = new Контрагенти_Папки_Дерево() { WidthRequest = 500 };
            ДеревоПапок.CallBack_RowActivated = LoadRecords;
            hPaned.Pack2(ДеревоПапок, false, true);

            PackStart(hPaned, true, true, 0);

            ShowAll();
        }

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
        }

        Menu PopUpContextMenu()
        {
            Menu Menu = new Menu();

            MenuItem setDeletionLabel = new MenuItem("Помітка на видалення");
            setDeletionLabel.Activated += OnDeleteClick;
            Menu.Append(setDeletionLabel);

            Menu.ShowAll();

            return Menu;
        }

        public void LoadTree()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                string UidSelect = SelectPointerItem != null ? SelectPointerItem.UnigueID.ToString() : DirectoryPointerItem!.UnigueID.ToString();
                UnigueID unigueID = new UnigueID(UidSelect);

                Контрагенти_Objest? контрагенти_Objest = new Контрагенти_Pointer(unigueID).GetDirectoryObject();
                if (контрагенти_Objest != null)
                    ДеревоПапок.Parent_Pointer = контрагенти_Objest.Папка;
            }

            ДеревоПапок.LoadTree();
        }

        public void LoadRecords()
        {
            ТабличніСписки.Контрагенти_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Контрагенти_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Контрагенти_Записи.Where.Clear();
            if (checkButtonIsHierarchy.Active)
            {
                ТабличніСписки.Контрагенти_Записи.Where.Add(new Where(Контрагенти_Const.Папка, Comparison.EQ, ДеревоПапок.Parent_Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.Контрагенти_Записи.LoadRecords();

            if (ТабличніСписки.Контрагенти_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Контрагенти_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.Контрагенти_Записи.Where.Clear();

            //Код
            ТабличніСписки.Контрагенти_Записи.Where.Add(
                new Where(Контрагенти_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.Контрагенти_Записи.Where.Add(
                new Where(Comparison.OR, Контрагенти_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.Контрагенти_Записи.LoadRecords();

            if (ТабличніСписки.Контрагенти_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Контрагенти_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Контрагент: *", () =>
                {
                    Контрагенти_Елемент page = new Контрагенти_Елемент
                    {
                        PageList = this,
                        IsNew = true,
                        РодичДляНового = ДеревоПапок.Parent_Pointer
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else
            {
                Контрагенти_Objest Контрагенти_Objest = new Контрагенти_Objest();
                if (Контрагенти_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"Контрагент: {Контрагенти_Objest.Назва}", () =>
                    {
                        Контрагенти_Елемент page = new Контрагенти_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            Контрагенти_Objest = Контрагенти_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                SelectPointerItem = new StorageAndTrade_1_0.Довідники.Контрагенти_Pointer(unigueID);
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

                    if (DirectoryPointerItem == null)
                        OpenPageElement(false, uid);
                    else
                    {
                        if (CallBack_OnSelectPointer != null)
                            CallBack_OnSelectPointer.Invoke(new Контрагенти_Pointer(new UnigueID(uid)));

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
                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                        OpenPageElement(false, uid);
                    }
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
                if (Message.Request(Program.GeneralForm, "Встановити або зняти помітку на видалення?") == ResponseType.Yes)
                {
                    TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        TreeViewGrid.Model.GetIter(out iter, itemPath);

                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                        Контрагенти_Objest Контрагенти_Objest = new Контрагенти_Objest();
                        if (Контрагенти_Objest.Read(new UnigueID(uid)))
                            Контрагенти_Objest.SetDeletionLabel(!Контрагенти_Objest.DeletionLabel);
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

                        Контрагенти_Objest Контрагенти_Objest = new Контрагенти_Objest();
                        if (Контрагенти_Objest.Read(new UnigueID(uid)))
                        {
                            Контрагенти_Objest Контрагенти_Objest_Новий = Контрагенти_Objest.Copy(true);
                            Контрагенти_Objest_Новий.Save();
                            Контрагенти_Objest_Новий.Контакти_TablePart.Save(true);
                            Контрагенти_Objest_Новий.Файли_TablePart.Save(true);

                            SelectPointerItem = Контрагенти_Objest_Новий.GetDirectoryPointer();
                        }
                        else
                            Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                    }

                    LoadRecords();
                }
            }
        }

        #endregion

        #region Controls

        void OnCheckButtonIsHierarchyClicked(object? sender, EventArgs args)
        {
            LoadTree();
        }

        #endregion
    }
}