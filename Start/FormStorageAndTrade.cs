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

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class FormStorageAndTrade : Window
    {
        public Configurator.ConfigurationParam? OpenConfigurationParam { get; set; }

        Notebook topNotebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false, TabPos = PositionType.Top };
        Statusbar statusBar = new Statusbar();
        List<string> historyNotebookSwitchList = []; //Список для збереження історії послідовності відкриття вкладок

        public FormStorageAndTrade() : base("")
        {
            SetDefaultSize(1200, 900);
            SetPosition(WindowPosition.Center);
            Maximize();

            DeleteEvent += delegate { Program.Quit(); };

            if (File.Exists(Program.IcoFileName))
                SetDefaultIconFromFile(Program.IcoFileName);

            //Блок кнопок у шапці головного вікна
            {
                HeaderBar headerBar = new HeaderBar()
                {
                    Title = "\"Зберігання та Торгівля\" для України",
                    Subtitle = "Облік складу, торгівлі та фінансів",
                    ShowCloseButton = true
                };

                //Повідомлення
                Button buttonTerminal = new Button() { Image = new Image(Stock.Index, IconSize.Button), TooltipText = "Повідомлення" };
                buttonTerminal.Clicked += (object? sender, EventArgs args) => { ФункціїДляПовідомлень.ВідкритиТермінал(); };
                headerBar.PackEnd(buttonTerminal);

                //Повнотекстовий пошук
                Button buttonFind = new Button() { Image = new Image(Stock.Find, IconSize.Button), TooltipText = "Пошук" };
                buttonFind.Clicked += OnButtonFindClicked;
                headerBar.PackEnd(buttonFind);

                Titlebar = headerBar;
            }

            VBox vBox = new VBox();
            Add(vBox);

            HBox hBox = new HBox();
            vBox.PackStart(hBox, true, true, 0);

            CreateLeftMenu(hBox);

            hBox.PackStart(topNotebook, true, true, 0);
            topNotebook.SwitchPage += OnSwitchPageTopNotebook;

            vBox.PackStart(statusBar, false, false, 0);
            ShowAll();
        }

        //Переключення сторінок блокноту
        void OnSwitchPageTopNotebook(object? sender, SwitchPageArgs args)
        {
            string currPageUID = args.Page.Name;

            if (historyNotebookSwitchList.Contains(currPageUID))
                historyNotebookSwitchList.Remove(currPageUID);

            /* Поточна сторінка ставиться у кінець списку */
            historyNotebookSwitchList.Add(currPageUID);
        }

        public void SetStatusBar()
        {
            statusBar.Halign = Align.Start;
            statusBar.Add(new Label($" Сервер: {OpenConfigurationParam?.DataBaseServer} "));
            statusBar.Add(new Separator(Orientation.Vertical));
            statusBar.Add(new Label($" База даних: {OpenConfigurationParam?.DataBaseBaseName} "));

            statusBar.ShowAll();
        }

        public async void SetCurrentUser()
        {
            Користувачі_Pointer ЗнайденийКористувач = await new Користувачі_Select().FindByField(Користувачі_Const.КодВСпеціальнійТаблиці, Config.Kernel.User);

            if (ЗнайденийКористувач.IsEmpty())
            {
                Користувачі_Objest НовийКористувач = new Користувачі_Objest
                {
                    КодВСпеціальнійТаблиці = Config.Kernel.User,
                    Назва = await Config.Kernel.DataBase.SpetialTableUsersGetFullName(Config.Kernel.User)
                };

                await НовийКористувач.New();
                await НовийКористувач.Save();

                Program.Користувач = НовийКористувач.GetDirectoryPointer();
            }
            else
                Program.Користувач = ЗнайденийКористувач;
        }

        public void OpenFirstPages()
        {
            PageHome page = new PageHome();
            CreateNotebookPage("Стартова", () => { return page; });

            //Активні користувачі
            page.АктивніКористувачі.AutoRefreshRun();

            //Останні завантажені курси валют
            Task.Run(page.БлокКурсиВалют.StartDesktop);

            //Автоматично завантажити нові курси валют
            Task.Run(page.БлокКурсиВалют.StartAutoWork);

            //Початкове заповнення
            if (!ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const)
                CreateNotebookPage("Початкове заповнення", () => { return new Обробка_ПочатковеЗаповнення(); });
        }

        #region FullTextSearch

        void OnButtonFindClicked(object? sender, EventArgs args)
        {
            Popover PopoverFind = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 5 };

            SearchEntry entryFullTextSearch = new SearchEntry() { WidthRequest = 500 };
            entryFullTextSearch.KeyReleaseEvent += (object? sender, KeyReleaseEventArgs args) =>
            {
                if (args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter)
                {
                    PageFullTextSearch page = new PageFullTextSearch();
                    CreateNotebookPage("Пошук", () => { return page; });
                    page.Find(((SearchEntry)sender!).Text);
                }
            };

            PopoverFind.Add(entryFullTextSearch);
            PopoverFind.ShowAll();
        }

        #endregion

        #region LeftMenu

        void CreateLeftMenu(HBox hbox)
        {
            VBox vbox = new VBox() { BorderWidth = 0 };

            ScrolledWindow scrolLeftMenu = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 170 };
            scrolLeftMenu.SetPolicy(PolicyType.Never, PolicyType.Never);
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
                Image = new Image($"{AppContext.BaseDirectory}{image}"),
                AlwaysShowImage = true
            };

            lb.Image.Valign = Align.End;
            lb.Clicked += ClikAction;

            vBox.PackStart(lb, false, false, 10);
        }

        void Документи(object? sender, EventArgs args)
        {
            Popover po = new Popover((LinkButton)sender!) { Position = PositionType.Right };
            po.Add(new PageDocuments());
            po.ShowAll();
        }

        void Журнали(object? sender, EventArgs args)
        {
            Popover po = new Popover((LinkButton)sender!) { Position = PositionType.Right };
            po.Add(new PageJournals());
            po.ShowAll();
        }

        void Звіти(object? sender, EventArgs args)
        {
            Popover po = new Popover((LinkButton)sender!) { Position = PositionType.Right };
            po.Add(new PageReports());
            po.ShowAll();
        }

        void Довідники(object? sender, EventArgs args)
        {
            Popover po = new Popover((LinkButton)sender!) { Position = PositionType.Right };
            po.Add(new PageDirectory());
            po.ShowAll();
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

        #region Блокнот

        /// <summary>
        /// Створити сторінку в блокноті.
        /// Код сторінки задається в назву віджета - widget.Name = codePage;
        /// </summary>
        /// <param name="tabName">Назва сторінки</param>
        /// <param name="pageWidget">Віджет для сторінки</param>
        /// <param name="insertPage">Вставити сторінку перед поточною</param>
        public void CreateNotebookPage(string tabName, Func<Widget>? pageWidget, bool insertPage = false)
        {
            int numPage;
            string codePage = Guid.NewGuid().ToString();

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In, Name = codePage };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            HBox hBoxLabel = CreateLabelPageWidget(tabName, codePage, topNotebook);

            if (insertPage)
                numPage = topNotebook.InsertPage(scroll, hBoxLabel, topNotebook.CurrentPage);
            else
                numPage = topNotebook.AppendPage(scroll, hBoxLabel);

            if (pageWidget != null)
            {
                Widget widget = pageWidget.Invoke();
                scroll.Add(widget);

                widget.Name = codePage;
            }

            topNotebook.ShowAll();
            topNotebook.CurrentPage = numPage;
            topNotebook.GrabFocus();
        }

        /// <summary>
        /// Заголовок сторінки блокноту
        /// </summary>
        /// <param name="caption">Заголовок</param>
        /// <param name="codePage">Код сторінки</param>
        /// <param name="notebook">Блокнот</param>
        /// <returns></returns>
        public HBox CreateLabelPageWidget(string caption, string codePage, Notebook notebook)
        {
            HBox hBoxLabel = new HBox();

            Label label = new Label { Text = SubstringPageName(caption), TooltipText = caption, Expand = false, Halign = Align.Start };
            hBoxLabel.PackStart(label, false, false, 4);

            //Лінк закриття сторінки
            LinkButton lbClose = new LinkButton("Закрити", " ")
            {
                Halign = Align.Start,
                Image = new Image(AppContext.BaseDirectory + "images/clean.png"),
                AlwaysShowImage = true,
                Name = codePage
            };

            lbClose.Clicked += (object? sender, EventArgs args) =>
            {
                CloseNotebookPageToCode(notebook, ((Widget)sender!).Name);
            };

            hBoxLabel.PackEnd(lbClose, false, false, 0);
            hBoxLabel.ShowAll();

            return hBoxLabel;
        }

        /// <summary>
        /// Закрити сторінку блокноту по коду
        /// </summary>
        /// <param name="notebook">Блокнот</param>
        /// <param name="codePage">Код</param>
        public void CloseNotebookPageToCode(Notebook notebook, string codePage)
        {
            notebook.Foreach(
                (Widget wg) =>
                {
                    if (wg.Name == codePage)
                    {
                        if (historyNotebookSwitchList.Contains(codePage))
                            historyNotebookSwitchList.Remove(codePage);

                        if (historyNotebookSwitchList.Count > 0)
                            CurrentNotebookPageToCode(topNotebook, historyNotebookSwitchList[historyNotebookSwitchList.Count - 1]);

                        notebook.DetachTab(wg);
                    }
                });
        }

        /// <summary>
        /// Закрити сторінку блокноту
        /// </summary>
        /// <param name="codePage">Код</param>
        public void CloseNotebookPageToCode(string codePage)
        {
            CloseNotebookPageToCode(topNotebook, codePage);
        }

        /// <summary>
        /// Перейменувати сторінку по коду
        /// </summary>
        /// <param name="name">Нова назва</param>
        /// <param name="codePage">Код</param>
        public void RenameNotebookPageToCode(string name, string codePage)
        {
            HBox hBoxLabel = CreateLabelPageWidget(name, codePage, topNotebook);

            topNotebook.Foreach(
                (Widget wg) =>
                {
                    if (wg.Name == codePage)
                        topNotebook.SetTabLabel(wg, hBoxLabel);
                });
        }

        /// <summary>
        /// Встановлення поточної сторінки по коду
        /// </summary>
        /// <param name="notebook">Блокнот</param>
        /// <param name="codePage">Код</param>
        public void CurrentNotebookPageToCode(Notebook notebook, string codePage)
        {
            int counter = 0;

            notebook.Foreach(
                (Widget wg) =>
                {
                    if (wg.Name == codePage)
                        notebook.CurrentPage = counter;

                    counter++;
                });
        }

        /// <summary>
        /// Блокування чи розблокування поточної сторінки по коду
        /// </summary>
        /// <param name="notebook">Блокнот</param>
        /// <param name="codePage">Код</param>
        /// <param name="sensitive">Значення</param>
        public void SensitiveNotebookPageToCode(Notebook notebook, string codePage, bool sensitive)
        {
            notebook.Foreach(
                (Widget wg) =>
                {
                    if (wg.Name == codePage)
                        wg.Sensitive = sensitive;
                });
        }

        /// <summary>
        /// Блокування чи розблокування для основного блокноту
        /// </summary>
        /// <param name="notebook">Блокнот</param>
        /// <param name="codePage">Код</param>
        /// <param name="sensitive">Значення</param>
        public void SensitiveNotebookPageToCode(string codePage, bool sensitive)
        {
            SensitiveNotebookPageToCode(topNotebook, codePage, sensitive);
        }

        /// <summary>
        /// Обрізати імя для сторінки
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public string SubstringPageName(string pageName)
        {
            return pageName.Length >= 33 ? pageName.Substring(0, 30) + "..." : pageName;
        }

        #endregion
    }
}