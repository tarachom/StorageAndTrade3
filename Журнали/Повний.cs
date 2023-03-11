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
using Константи = StorageAndTrade_1_0.Константи;
using Перелічення = StorageAndTrade_1_0.Перелічення;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    class Журнал_Повний : VBox
    {
        public UnigueID? SelectPointerItem { get; set; }
        public System.Action<Валюти_Pointer>? CallBack_OnSelectPointer { get; set; }

        TreeView TreeViewGrid;
        ComboBoxText ComboBoxPeriodWhere = new ComboBoxText();
        ToolButton? TypeDocToolButton; //Список документів

        public Журнал_Повний() : base()
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

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            TreeViewGrid = new TreeView(ТабличніСписки.Журнали_Повний.Store);
            ТабличніСписки.Журнали_Повний.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.KeyReleaseEvent += OnKeyReleaseEvent;

            scrollTree.Add(TreeViewGrid);

/*
            scrollTree.Vadjustment.ValueChanged += (object? sender, EventArgs args) =>
            {
                Console.WriteLine(
                    scrollTree.Vadjustment.Value + " " + 
                    scrollTree.Vadjustment.Upper + " " + 
                    scrollTree.Vadjustment.PageIncrement + " " + 
                    scrollTree.Vadjustment.PageSize);
            };

*/
            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton upButton = new ToolButton(Stock.GoUp) { TooltipText = "Знайти в журналі відповідго типу документу" };
            upButton.Clicked += OnEditClick;
            toolbar.Add(upButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { TooltipText = "Обновити" };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);

            //Separator
            ToolItem toolItemSeparator = new ToolItem();
            toolItemSeparator.Add(new Separator(Orientation.Horizontal));
            toolbar.Add(toolItemSeparator);

            TypeDocToolButton = new ToolButton(Stock.Find) { Label = "Документи", IsImportant = true };
            TypeDocToolButton.Clicked += OnTypeDocsClick;
            toolbar.Add(TypeDocToolButton);
        }

        public void SetValue()
        {
            if ((int)Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const != 0)
                ComboBoxPeriodWhere.ActiveId = Константи.ЖурналиДокументів.ОсновнийТипПеріоду_Const.ToString();
        }

        public void LoadRecords()
        {
            ТабличніСписки.Журнали_Повний.SelectPointerItem = SelectPointerItem;

            ТабличніСписки.Журнали_Повний.LoadRecords();

            if (ТабличніСписки.Журнали_Повний.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Повний.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.Журнали_Повний.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Повний.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

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

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
                OpenDocTypeJournal();
        }

        void OpenDocTypeJournal()
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);
                    string typeDoc = (string)TreeViewGrid.Model.GetValue(iter, 2);

                    ФункціїДляЖурналів.ВідкритиЖурналВідповідноДоВидуДокументу(typeDoc, new UnigueID(uid),
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
                        OpenDocTypeJournal();
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
            }
        }

        #endregion

        #region ToolBar

        void OnComboBoxPeriodWhereChanged(object? sender, EventArgs args)
        {
            ТабличніСписки.Журнали_Повний.ДодатиВідбірПоПеріоду(
                Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        void OnEditClick(object? sender, EventArgs args)
        {
            OpenDocTypeJournal();
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        void OnTypeDocsClick(object? sender, EventArgs args)
        {
            ФункціїДляЖурналів.ВідкритиСписокДокументів((ToolButton)sender!, ТабличніСписки.Журнали_Повний.AllowDocument(),
                Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
        }

        #endregion

    }
}