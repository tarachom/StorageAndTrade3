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
using ТабличніСписки = StorageAndTrade_1_0.РегістриВідомостей.ТабличніСписки;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class ШтрихкодиНоменклатури : VBox
    {
        TreeView TreeViewGrid;
        public Номенклатура_PointerControl НоменклатураВласник = new Номенклатура_PointerControl();
        public ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатуриВласник = new ХарактеристикиНоменклатури_PointerControl();

        public ШтрихкодиНоменклатури(bool IsSelectPointer = false) : base()
        {
            BorderWidth = 0;

            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            //Номенклатура Власник
            hBoxTop.PackStart(НоменклатураВласник, false, false, 2);
            НоменклатураВласник.Caption = "Номенклатура:";
            НоменклатураВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

            //Характеристика Власник
            hBoxTop.PackStart(ХарактеристикиНоменклатуриВласник, false, false, 2);
            ХарактеристикиНоменклатуриВласник.Caption = "Характеристика:";
            ХарактеристикиНоменклатуриВласник.BeforeClickOpenFunc = () =>
            {
                ХарактеристикиНоменклатуриВласник.НоменклатураВласник = НоменклатураВласник.Pointer;
            };
            ХарактеристикиНоменклатуриВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

            CreateToolbar();

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.ШтрихкодиНоменклатури_Записи.Store);
            ТабличніСписки.ШтрихкодиНоменклатури_Записи.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
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

            ToolButton deleteButton = new ToolButton(Stock.Delete) { TooltipText = "Видалити" };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { TooltipText = "Обновити" };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);
        }

        public void LoadRecords()
        {
            ТабличніСписки.ШтрихкодиНоменклатури_Записи.Where.Clear();

            if (!НоменклатураВласник.Pointer.IsEmpty())
            {
                ТабличніСписки.ШтрихкодиНоменклатури_Записи.Where.Add(
                    new Where(ШтрихкодиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            if (!ХарактеристикиНоменклатуриВласник.Pointer.IsEmpty())
            {
                ТабличніСписки.ШтрихкодиНоменклатури_Записи.Where.Add(
                    new Where(Comparison.AND, ШтрихкодиНоменклатури_Const.ХарактеристикаНоменклатури, Comparison.EQ, ХарактеристикиНоменклатуриВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.ШтрихкодиНоменклатури_Записи.LoadRecords();

            if (ТабличніСписки.ШтрихкодиНоменклатури_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ШтрихкодиНоменклатури_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        void OpenPageElement(bool IsNew, string uid = "")
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ШтрихкодиНоменклатури_Const.FULLNAME} *", () =>
                {
                    ШтрихкодиНоменклатури_Елемент page = new ШтрихкодиНоменклатури_Елемент
                    {
                        PageList = this,
                        IsNew = true,
                        НоменклатураДляНового = НоменклатураВласник.Pointer,
                        ХарактеристикаДляНового = ХарактеристикиНоменклатуриВласник.Pointer
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else
            {
                ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest = new ШтрихкодиНоменклатури_Objest();
                if (ШтрихкодиНоменклатури_Objest.Read(new UnigueID(uid)))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ШтрихкодиНоменклатури_Objest.Штрихкод}", () =>
                    {
                        ШтрихкодиНоменклатури_Елемент page = new ШтрихкодиНоменклатури_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ШтрихкодиНоменклатури_Objest = ШтрихкодиНоменклатури_Objest,
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

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    OpenPageElement(false, uid);
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

                        ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest = new ШтрихкодиНоменклатури_Objest();
                        if (ШтрихкодиНоменклатури_Objest.Read(new UnigueID(uid)))
                            ШтрихкодиНоменклатури_Objest.Delete();
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