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

using StorageAndTrade_1_0.Константи;
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
        SearchControl ПошукПоНазві = new SearchControl();

        public Контрагенти(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new Контрагенти_Pointer());

                    Program.GeneralForm?.CloseCurrentPageNotebook();
                };

                hBoxBotton.PackStart(bEmptyPointer, false, false, 10);
            }

            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += OnCheckButtonIsHierarchyClicked;
            hBoxBotton.PackStart(checkButtonIsHierarchy, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            //Пошук
            hBoxBotton.PackStart(ПошукПоНазві, false, false, 2);
            ПошукПоНазві.QueryFind = ПошуковіЗапити.Контрагенти;
            ПошукПоНазві.Select = (UnigueID uid) =>
            {
                SelectPointerItem = new Контрагенти_Pointer(uid);
                LoadTree();
            };

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
                });
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
                    });
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

                        Контрагенти_Objest Контрагенти_Objest = new Контрагенти_Objest();
                        if (Контрагенти_Objest.Read(new UnigueID(uid)))
                            Контрагенти_Objest.Delete();
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
                            Контрагенти_Objest Контрагенти_Objest_Новий = Контрагенти_Objest.Copy();
                            Контрагенти_Objest_Новий.Назва += " - Копія";
                            Контрагенти_Objest_Новий.Код = (++НумераціяДовідників.Контрагенти_Const).ToString("D6");
                            Контрагенти_Objest_Новий.Save();

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
            LoadRecords();
        }

        #endregion
    }
}