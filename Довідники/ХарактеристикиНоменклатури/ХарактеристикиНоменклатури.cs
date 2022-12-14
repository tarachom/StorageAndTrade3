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
    class ХарактеристикиНоменклатури : VBox
    {
        public ХарактеристикиНоменклатури_Pointer? SelectPointerItem { get; set; }
        public ХарактеристикиНоменклатури_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<ХарактеристикиНоменклатури_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        public Номенклатура_PointerControl НоменклатураВласник = new Номенклатура_PointerControl();

        public ХарактеристикиНоменклатури(bool IsSelectPointer = false) : base()
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
                        CallBack_OnSelectPointer.Invoke(new ХарактеристикиНоменклатури_Pointer());

                    Program.GeneralForm?.CloseCurrentPageNotebook();
                };

                hBoxBotton.PackStart(bEmptyPointer, false, false, 10);
            }

            PackStart(hBoxBotton, false, false, 10);

            //Власник
            hBoxBotton.PackStart(НоменклатураВласник, false, false, 2);
            НоменклатураВласник.Caption = "Номенклатура власник:";
            НоменклатураВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ХарактеристикиНоменклатури_Записи.Store);
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.AddColumns(TreeViewGrid);

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
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.Where.Clear();
            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.Where.Add(
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords();

            if (ТабличніСписки.ХарактеристикиНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ХарактеристикиНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"Характеристики: *", () =>
                {
                    ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
                    {
                        PageList = this,
                        IsNew = true,
                        НоменклатураДляНового = НоменклатураВласник.Pointer
                    };

                    page.SetValue();

                    return page;
                });
            }
            else
            {
                ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest = new ХарактеристикиНоменклатури_Objest();
                if (ХарактеристикиНоменклатури_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"Характеристики: {ХарактеристикиНоменклатури_Objest.Назва}", () =>
                    {
                        ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ХарактеристикиНоменклатури_Objest = ХарактеристикиНоменклатури_Objest,
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

                SelectPointerItem = new StorageAndTrade_1_0.Довідники.ХарактеристикиНоменклатури_Pointer(unigueID);
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
                                CallBack_OnSelectPointer.Invoke(new ХарактеристикиНоменклатури_Pointer(new UnigueID(uid)));

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

                        ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest = new ХарактеристикиНоменклатури_Objest();
                        if (ХарактеристикиНоменклатури_Objest.Read(new UnigueID(uid)))
                            ХарактеристикиНоменклатури_Objest.Delete();
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

                        ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest = new ХарактеристикиНоменклатури_Objest();
                        if (ХарактеристикиНоменклатури_Objest.Read(new UnigueID(uid)))
                        {
                            ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest_Новий = ХарактеристикиНоменклатури_Objest.Copy();
                            ХарактеристикиНоменклатури_Objest_Новий.Назва += " - Копія";
                            ХарактеристикиНоменклатури_Objest_Новий.Код = (++НумераціяДовідників.ХарактеристикиНоменклатури_Const).ToString("D6");
                            ХарактеристикиНоменклатури_Objest_Новий.Save();

                            SelectPointerItem = ХарактеристикиНоменклатури_Objest_Новий.GetDirectoryPointer();
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