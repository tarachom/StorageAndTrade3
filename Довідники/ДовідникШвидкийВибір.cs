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

namespace StorageAndTrade
{
    public abstract class ДовідникШвидкийВибір : VBox
    {
        /// <summary>
        /// Вспливаюче вікно
        /// </summary>
        public Popover? PopoverParent { get; set; }

        /// <summary>
        /// Елемент для вибору
        /// </summary>
        public UnigueID? DirectoryPointerItem { get; set; }

        /// <summary>
        /// Функція вибору
        /// </summary>
        public Action<UnigueID>? CallBack_OnSelectPointer { get; set; }

        /// <summary>
        /// Верхній горизонтальний блок
        /// </summary>
        protected HBox HBoxTop = new HBox();

        protected TreeView TreeViewGrid = new TreeView();
        protected SearchControl Пошук = new SearchControl();

        public ДовідникШвидкийВибір(bool visibleSearch = true, int width = 600, int height = 300) : base()
        {
            BorderWidth = 0;

            PackStart(HBoxTop, false, false, 5);

            if (visibleSearch)
            {
                //Пошук 2
                HBoxTop.PackStart(Пошук, false, false, 0);
                Пошук.Select = async (string x) => { await LoadRecords_OnSearch(x); };
                Пошук.Clear = async () => { await LoadRecords(); };
            }

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = width, HeightRequest = height };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        #region Virtual Function

        public virtual ValueTask LoadRecords() { return new ValueTask(); }

        protected virtual ValueTask LoadRecords_OnSearch(string searchText) { return new ValueTask(); }

        #endregion

        #region  TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                DirectoryPointerItem = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    DirectoryPointerItem = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(DirectoryPointerItem);

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                }
            }
        }

        #endregion
    }
}