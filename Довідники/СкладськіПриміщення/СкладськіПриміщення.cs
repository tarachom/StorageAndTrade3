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
    class СкладськіПриміщення : VBox
    {
        public СкладськіПриміщення_Pointer? SelectPointerItem { get; set; }
        public СкладськіПриміщення_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<СкладськіПриміщення_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        public Склади_PointerControl СкладВласник = new Склади_PointerControl();
        SearchControl2 ПошукПовнотекстовий = new SearchControl2();

        public СкладськіПриміщення() : base()
        {
            BorderWidth = 0;

            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //Власник
            hBoxTop.PackStart(СкладВласник, false, false, 2);
            СкладВласник.Caption = "Склад:";
            СкладВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

            //Пошук 2
            hBoxTop.PackStart(ПошукПовнотекстовий, false, false, 2);
            ПошукПовнотекстовий.Select = LoadRecords_OnSearch;
            ПошукПовнотекстовий.Clear = LoadRecords;

            //Складські комірки
            LinkButton linkButtonHar = new LinkButton(" Складські комірки") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
            linkButtonHar.Clicked += (object? sender, EventArgs args) =>
            {
                СкладськіКомірки page = new СкладськіКомірки();

                if (SelectPointerItem != null)
                    page.СкладПриміщенняВласник.Pointer = SelectPointerItem;

                Program.GeneralForm?.CreateNotebookPage("Складські комірки", () => { return page; });

                page.LoadTree();
            };

            hBoxTop.PackStart(linkButtonHar, false, false, 10);

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.СкладськіПриміщення_Записи.Store);
            ТабличніСписки.СкладськіПриміщення_Записи.AddColumns(TreeViewGrid);

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

        public void LoadRecords()
        {
            ТабличніСписки.СкладськіПриміщення_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СкладськіПриміщення_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіПриміщення_Записи.Where.Clear();

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_Записи.Where.Add(
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords();

            if (ТабличніСписки.СкладськіПриміщення_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіПриміщення_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.СкладськіПриміщення_Записи.Where.Clear();

            //Назва
            ТабличніСписки.СкладськіПриміщення_Записи.Where.Add(
                new Where(СкладськіПриміщення_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords();

            if (ТабличніСписки.СкладськіПриміщення_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.СкладськіПриміщення_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Складські приміщення: *", () =>
                {
                    СкладськіПриміщення_Елемент page = new СкладськіПриміщення_Елемент
                    {
                        PageList = this,
                        IsNew = true,
                        СкладДляНового = СкладВласник.Pointer
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else
            {
                СкладськіПриміщення_Objest СкладськіПриміщення_Objest = new СкладськіПриміщення_Objest();
                if (СкладськіПриміщення_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"Складські приміщення: {СкладськіПриміщення_Objest.Назва}", () =>
                    {
                        СкладськіПриміщення_Елемент page = new СкладськіПриміщення_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            СкладськіПриміщення_Objest = СкладськіПриміщення_Objest,
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

                SelectPointerItem = new StorageAndTrade_1_0.Довідники.СкладськіПриміщення_Pointer(unigueID);
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
            {
                if (TreeViewGrid.Selection.CountSelectedRows() != 0)
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
                                CallBack_OnSelectPointer.Invoke(new СкладськіПриміщення_Pointer(new UnigueID(uid)));

                            Program.GeneralForm?.CloseCurrentPageNotebook();
                        }
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
                if (Message.Request(Program.GeneralForm, "Видалити?") == ResponseType.Yes)
                {
                    TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        TreeViewGrid.Model.GetIter(out iter, itemPath);

                        string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                        СкладськіПриміщення_Objest СкладськіПриміщення_Objest = new СкладськіПриміщення_Objest();
                        if (СкладськіПриміщення_Objest.Read(new UnigueID(uid)))
                            СкладськіПриміщення_Objest.Delete();
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

                        СкладськіПриміщення_Objest СкладськіПриміщення_Objest = new СкладськіПриміщення_Objest();
                        if (СкладськіПриміщення_Objest.Read(new UnigueID(uid)))
                        {
                            СкладськіПриміщення_Objest СкладськіПриміщення_Objest_Новий = СкладськіПриміщення_Objest.Copy();
                            СкладськіПриміщення_Objest_Новий.Назва += " - Копія";
                            СкладськіПриміщення_Objest_Новий.Save();

                            SelectPointerItem = СкладськіПриміщення_Objest_Новий.GetDirectoryPointer();
                        }
                        else
                            Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                    }

                    LoadRecords();
                }
            }
        }

        #endregion

    }
}