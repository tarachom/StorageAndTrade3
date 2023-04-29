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

namespace StorageAndTrade
{
    class Номенклатура_Папки_Дерево_ШвидкийВибір : VBox
    {
        public Popover? PopoverParent { get; set; }
        TreeView TreeViewGrid;
        TreeStore TreeStore = new TreeStore(typeof(string), typeof(string));

        public System.Action? CallBack_RowActivated { get; set; }
        public Номенклатура_Папки_Pointer? DirectoryPointerItem { get; set; }
        public System.Action<Номенклатура_Папки_Pointer>? CallBack_OnSelectPointer { get; set; }
        public Номенклатура_Папки_Pointer Parent_Pointer { get; set; } = new Номенклатура_Папки_Pointer();

        public string UidOpenFolder { get; set; } = "";

        public Номенклатура_Папки_Дерево_ШвидкийВибір() : base()
        {
            BorderWidth = 0;

            //Зверху
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 5);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {Номенклатура_Папки_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =>
                {
                    Номенклатура_Папки_Дерево page = new Номенклатура_Папки_Дерево();
                    page.DirectoryPointerItem = DirectoryPointerItem;
                    page.CallBack_OnSelectPointer = CallBack_OnSelectPointer;
                    page.UidOpenFolder = UidOpenFolder;

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {Номенклатура_Папки_Const.FULLNAME}", () => { return page; }, true);

                    page.LoadTree();
                };

                hBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    Номенклатура_Папки_Елемент page = new Номенклатура_Папки_Елемент { IsNew = true, CallBack_OnSelectPointer = CallBack_OnSelectPointer };

                    Program.GeneralForm?.CreateNotebookPage($"{Номенклатура_Папки_Const.FULLNAME} *", () => { return page; }, true);

                    page.SetValue();
                };

                hBoxTop.PackStart(linkNew, false, false, 0);
            }

            //Очистка
            {
                LinkButton linkClear = new LinkButton(" Очистити") { Image = new Image(AppContext.BaseDirectory + "images/clean.png"), AlwaysShowImage = true };
                linkClear.Clicked += (object? sender, EventArgs args) =>
                {
                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new Номенклатура_Папки_Pointer());

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                };

                hBoxTop.PackEnd(linkClear, false, false, 10);
            }

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 600, HeightRequest = 300 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView();
            Номенклатура_Папки_Дерево_СпільніФункції.AddColumns(TreeViewGrid);

            TreeViewGrid.Selection.Mode = SelectionMode.Single;
            TreeViewGrid.ActivateOnSingleClick = true;
            TreeViewGrid.RowActivated += OnRowActivated;
            TreeViewGrid.ButtonPressEvent += OnButtonPressEvent;
            TreeViewGrid.Model = TreeStore;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, true, true, 0);

            ShowAll();
        }

        public void LoadTree()
        {
            if (DirectoryPointerItem != null)
                Parent_Pointer = DirectoryPointerItem;

            Номенклатура_Папки_Дерево_СпільніФункції.FillTree(TreeViewGrid, TreeStore, UidOpenFolder, Parent_Pointer);
        }

        #region TreeView

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]);

                UnigueID unigueID = new UnigueID((string)TreeViewGrid.Model.GetValue(iter, 1));

                DirectoryPointerItem = new Номенклатура_Папки_Pointer(unigueID);
            }
        }

        void OnButtonPressEvent(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress && TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;

                if (TreeViewGrid.Model.GetIter(out iter, TreeViewGrid.Selection.GetSelectedRows()[0]))
                {
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 0);

                    if (CallBack_OnSelectPointer != null)
                        CallBack_OnSelectPointer.Invoke(new Номенклатура_Папки_Pointer(new UnigueID(uid)));

                    if (PopoverParent != null)
                        PopoverParent.Hide();
                }
            }
        }

        #endregion
    }
}