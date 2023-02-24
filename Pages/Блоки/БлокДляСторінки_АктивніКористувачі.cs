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

/*

Список активних користувачів

*/

using Gtk;

using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class БлокДляСторінки_АктивніКористувачі : VBox
    {
        CancellationTokenSource? CancellationTokenSourceItem;

        enum Columns
        {
            UID,
            UserUID,
            UserName,
            DateLogin,
            DateUp,
            Master
        }

        ListStore Store = new ListStore(
            typeof(string), //UID
            typeof(string), //UserUID
            typeof(string), //UserName
            typeof(string), //DateLogin
            typeof(string), //DateUp
            typeof(bool)    //Master
        );

        TreeView TreeViewGrid;

        public БлокДляСторінки_АктивніКористувачі() : base()
        {
            HBox hBoxCaption = new HBox();
            hBoxCaption.PackStart(new Label("<b>Сесії користувачів</b>") { UseMarkup = true }, false, false, 5);
            PackStart(hBoxCaption, false, false, 5);

            ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, HeightRequest = 250 };
            scrollTree.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            TreeViewGrid = new TreeView(Store);
            AddColumn();

            TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
            TreeViewGrid.ActivateOnSingleClick = true;

            scrollTree.Add(TreeViewGrid);

            PackStart(scrollTree, false, false, 5);

            ShowAll();
        }

        public void AutoRefreshRun()
        {
            Program.ListCancellationToken.Add(CancellationTokenSourceItem = new CancellationTokenSource());

            Thread ThreadAutoRefresh = new Thread(new ThreadStart(LoadRecordsAsync));
            ThreadAutoRefresh.Start();
        }

        public void LoadRecordsAsync()
        {
            int counter = 0;

            while (!CancellationTokenSourceItem!.IsCancellationRequested)
            {
                if (counter == 0 || counter > 6)
                {
                    LoadRecords();
                    counter = 1;
                }

                counter++;

                //Затримка на 1 сек
                Thread.Sleep(1000);
            }
        }

        void LoadRecords()
        {
            Store.Clear();

            List<Dictionary<string, object>> listRow = Config.Kernel!.DataBase.SpetialTableActiveUsersAllSelect();

            foreach (Dictionary<string, object> record in listRow)
            {
                Store.AppendValues(
                    record["uid"].ToString(),
                    record["usersuid"].ToString(),
                    record["username"].ToString(),
                    record["datelogin"].ToString(),
                    record["dateupdate"].ToString(),
                    record["master"]
                );
            }
        }

        #region TreeView

        void AddColumn()
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("UID", new CellRendererText(), "text", (int)Columns.UID) { Visible = false });
            TreeViewGrid.AppendColumn(new TreeViewColumn("UserUID", new CellRendererText(), "text", (int)Columns.UserUID) { Visible = false });
            TreeViewGrid.AppendColumn(new TreeViewColumn("Користувач", new CellRendererText(), "text", (int)Columns.UserName));
            TreeViewGrid.AppendColumn(new TreeViewColumn("Авторизація", new CellRendererText(), "text", (int)Columns.DateLogin));
            TreeViewGrid.AppendColumn(new TreeViewColumn("Підтвердження", new CellRendererText(), "text", (int)Columns.DateUp));
            TreeViewGrid.AppendColumn(new TreeViewColumn("Головний", new CellRendererToggle(), "active", (int)Columns.Master));

            //Пустишка
            TreeViewGrid.AppendColumn(new TreeViewColumn());
        }

        #endregion

    }
}