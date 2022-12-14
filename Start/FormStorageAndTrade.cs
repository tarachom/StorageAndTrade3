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

using Константи = StorageAndTrade_1_0.Константи;

namespace StorageAndTrade
{
    class FormStorageAndTrade : Window
    {
        public ConfigurationParam? OpenConfigurationParam { get; set; }

        Notebook topNotebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false, TabPos = PositionType.Top };
        Statusbar statusBar = new Statusbar();
        //uint previousNotebookCurrentPage = 0;

        public FormStorageAndTrade() : base("\"Зберігання та Торгівля\" для України")
        {
            SetDefaultSize(1200, 900);
            SetPosition(WindowPosition.Center);
            if (File.Exists(Program.IcoFileName)) SetDefaultIconFromFile(Program.IcoFileName);

            DeleteEvent += delegate { Program.Quit(); };

            VBox vbox = new VBox();
            Add(vbox);

            HBox hbox = new HBox();
            vbox.PackStart(hbox, true, true, 0);

            CreateLeftMenu(hbox);

            topNotebook.SwitchPage += OnNotebookSwitchPage;
            topNotebook.PageRemoved += OnNotebookPageRemoved;
            hbox.PackStart(topNotebook, true, true, 0);

            CreateNotebookPage("Стартова", () =>
            {
                PageHome page = new PageHome();
                page.StartBackgroundTask();
                return page;
            });

            vbox.PackStart(statusBar, false, false, 0);

            ShowAll();
        }

        public void CheckValueConstant()
        {
            if (!Константи.ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const)
            {
                CreateNotebookPage("Початкове заповнення", () =>
                {
                    return new PageInitialFilling();
                });
            }
        }

        #region LeftMenu

        void CreateLeftMenu(HBox hbox)
        {
            VBox vbox = new VBox();
            vbox.BorderWidth = 10;

            ScrolledWindow scrolLeftMenu = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 200 };
            scrolLeftMenu.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrolLeftMenu.Add(vbox);

            CreateItemLeftMenu(vbox, "Документи", Документи, "images/documents.png");
            CreateItemLeftMenu(vbox, "Журнали", Журнали, "images/journal.png");
            CreateItemLeftMenu(vbox, "Звіти", Звіти, "images/report.png");
            CreateItemLeftMenu(vbox, "Довідники", Довідники, "images/directory.png");
            CreateItemLeftMenu(vbox, "Налаштування", Налаштування, "images/preferences.png");
            CreateItemLeftMenu(vbox, "Сервіс", Сервіс, "images/service.png");

            hbox.PackStart(scrolLeftMenu, false, false, 0);
        }

        void CreateItemLeftMenu(VBox vBox, string name, EventHandler ClikAction, string image)
        {
            LinkButton lb = new LinkButton(name, name)
            {
                Halign = Align.Start,
                Image = new Image(image),
                AlwaysShowImage = true
            };

            lb.Image.Valign = Align.End;

            lb.Clicked += ClikAction;

            vBox.PackStart(lb, false, false, 10);
        }

        void Документи(object? sender, EventArgs args)
        {
            CreateNotebookPage("Документи", () =>
            {
                PageDocuments page = new PageDocuments();
                return page;
            });
        }

        void Журнали(object? sender, EventArgs args)
        {
            CreateNotebookPage("Журнали", () =>
            {
                PageJournals page = new PageJournals();
                return page;
            });
        }

        void Звіти(object? sender, EventArgs args)
        {
            CreateNotebookPage("Звіти", () =>
            {
                PageReports page = new PageReports();
                return page;
            });
        }

        void Довідники(object? sender, EventArgs args)
        {
            CreateNotebookPage("Довідники", () =>
            {
                PageDirectory page = new PageDirectory();
                return page;
            });
        }

        void Налаштування(object? sender, EventArgs args)
        {
            CreateNotebookPage("Налаштування", () =>
            {
                PageSettings page = new PageSettings();
                page.SetValue();
                return page;
            });
        }

        void Сервіс(object? sender, EventArgs args)
        {
            CreateNotebookPage("Сервіс", () =>
            {
                PageService page = new PageService();
                return page;
            });
        }

        #endregion

        #region Notebook Page

        void OnNotebookSwitchPage(object? sender, SwitchPageArgs args)
        {
            // previousNotebookCurrentPage = args.PageNum;
            // Console.WriteLine("OnSwitchPage " + args.PageNum);
        }

        void OnNotebookPageRemoved(object? sender, PageRemovedArgs args)
        {
            // Console.WriteLine("OnPageRemoved " + args.PageNum);
        }

        public void CloseCurrentPageNotebook()
        {
            topNotebook.RemovePage(topNotebook.CurrentPage);
        }

        public void RenameCurrentPageNotebook(string name)
        {
            topNotebook.SetTabLabelText(topNotebook.CurrentPageWidget, name);
        }

        public void CreateNotebookPage(string tabName, System.Func<Widget>? pageWidget, bool insertPage = false)
        {
            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            int numPage;
            Label label = new Label { Text = tabName, Expand = false, Halign = Align.Start };

            if (insertPage)
                numPage = topNotebook.InsertPage(scroll, label, topNotebook.CurrentPage);
            else
                numPage = topNotebook.AppendPage(scroll, label);

            if (pageWidget != null)
                scroll.Add((Widget)pageWidget.Invoke());

            topNotebook.ShowAll();

            topNotebook.CurrentPage = numPage;
        }

        #endregion
    }
}