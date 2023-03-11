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

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ДоговориКонтрагентів : VBox
    {
        public ДоговориКонтрагентів_Pointer? SelectPointerItem { get; set; }
        public ДоговориКонтрагентів_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<ДоговориКонтрагентів_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        public Контрагенти_PointerControl КонтрагентВласник = new Контрагенти_PointerControl();

        public ДоговориКонтрагентів() : base()
        {
            BorderWidth = 0;

            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //Власник
            hBoxTop.PackStart(КонтрагентВласник, false, false, 2);
            КонтрагентВласник.Caption = "Контрагент:";
            КонтрагентВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ДоговориКонтрагентів_Записи.Store);
            ТабличніСписки.ДоговориКонтрагентів_Записи.AddColumns(TreeViewGrid);

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
            ТабличніСписки.ДоговориКонтрагентів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ДоговориКонтрагентів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ДоговориКонтрагентів_Записи.Where.Clear();

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_Записи.Where.Add(
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.ДоговориКонтрагентів_Записи.LoadRecords();

            if (ТабличніСписки.ДоговориКонтрагентів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ДоговориКонтрагентів_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Договір: *", () =>
                {
                    ДоговориКонтрагентів_Елемент page = new ДоговориКонтрагентів_Елемент
                    {
                        PageList = this,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else
            {
                ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest = new ДоговориКонтрагентів_Objest();
                if (ДоговориКонтрагентів_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"Договір: {ДоговориКонтрагентів_Objest.Назва}", () =>
                    {
                        ДоговориКонтрагентів_Елемент page = new ДоговориКонтрагентів_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ДоговориКонтрагентів_Objest = ДоговориКонтрагентів_Objest,
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

                SelectPointerItem = new StorageAndTrade_1_0.Довідники.ДоговориКонтрагентів_Pointer(unigueID);
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
                            CallBack_OnSelectPointer.Invoke(new ДоговориКонтрагентів_Pointer(new UnigueID(uid)));

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

                        ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest = new ДоговориКонтрагентів_Objest();
                        if (ДоговориКонтрагентів_Objest.Read(new UnigueID(uid)))
                            ДоговориКонтрагентів_Objest.Delete();
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

                        ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest = new ДоговориКонтрагентів_Objest();
                        if (ДоговориКонтрагентів_Objest.Read(new UnigueID(uid)))
                        {
                            ДоговориКонтрагентів_Objest ДоговориКонтрагентів_Objest_Новий = ДоговориКонтрагентів_Objest.Copy();
                            ДоговориКонтрагентів_Objest_Новий.Назва += " - Копія";
                            ДоговориКонтрагентів_Objest_Новий.Код = (++НумераціяДовідників.ДоговориКонтрагентів_Const).ToString("D6");
                            ДоговориКонтрагентів_Objest_Новий.Save();

                            SelectPointerItem = ДоговориКонтрагентів_Objest_Новий.GetDirectoryPointer();
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