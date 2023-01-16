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

        public СкладськіПриміщення(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new СкладськіПриміщення_Pointer());

                    Program.GeneralForm?.CloseCurrentPageNotebook();
                };

                hBoxBotton.PackStart(bEmptyPointer, false, false, 10);
            }

            PackStart(hBoxBotton, false, false, 10);

            //Власник
            hBoxBotton.PackStart(СкладВласник, false, false, 2);
            СкладВласник.Caption = "Склад власник:";
            СкладВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

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
                });
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