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
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class FormStorageAndTrade : Window
    {
        public Configurator.ConfigurationParam? OpenConfigurationParam { get; set; }
        CancellationTokenSource? CancellationTokenBackgroundTask;

        Guid KernelUser { get; set; } = Guid.Empty;
        Guid KernelSession { get; set; } = Guid.Empty;

        Notebook topNotebook = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false, TabPos = PositionType.Top };
        Statusbar statusBar = new Statusbar();

        public FormStorageAndTrade() : base("")
        {
            SetDefaultSize(1200, 900);
            SetPosition(WindowPosition.Center);
            Maximize();

            DeleteEvent += delegate { Program.Quit(); };

            if (File.Exists(Program.IcoFileName))
                SetDefaultIconFromFile(Program.IcoFileName);

            HeaderBar headerBar = new HeaderBar();
            headerBar.ShowCloseButton = true;
            headerBar.Title = "\"Зберігання та Торгівля\" для України";
            headerBar.Subtitle = "Облік складу, торгівлі та фінансів";
            Titlebar = headerBar;

            //Повнотекстовий пошук
            Button buttonFind = new Button();
            buttonFind.Add(new Image(AppContext.BaseDirectory + "images/find.png"));
            buttonFind.Clicked += OnButtonFindClicked;
            headerBar.PackEnd(buttonFind);

            VBox vBox = new VBox();
            Add(vBox);

            HBox hBox = new HBox();
            vBox.PackStart(hBox, true, true, 0);

            CreateLeftMenu(hBox);

            hBox.PackStart(topNotebook, true, true, 0);
            vBox.PackStart(statusBar, false, false, 0);

            ShowAll();
        }

        public void SetCurrentUser()
        {
            KernelUser = Config.Kernel!.User;
            KernelSession = Config.Kernel!.Session;

            Користувачі_Pointer ЗнайденийКористувач = new Користувачі_Select().FindByField(Користувачі_Const.КодВСпеціальнійТаблиці, KernelUser);

            if (ЗнайденийКористувач.IsEmpty())
            {
                Користувачі_Objest НовийКористувач = new Користувачі_Objest();
                НовийКористувач.New();
                НовийКористувач.Код = (++НумераціяДовідників.Користувачі_Const).ToString("D6");
                НовийКористувач.КодВСпеціальнійТаблиці = Config.Kernel!.User;
                НовийКористувач.Назва = Config.Kernel!.DataBase.SpetialTableUsersGetFullName(KernelUser);
                НовийКористувач.Save();

                Program.Користувач = НовийКористувач.GetDirectoryPointer();
            }
            else
                Program.Користувач = ЗнайденийКористувач;
        }

        public void OpenFirstPages()
        {
            CreateNotebookPage("Стартова", () =>
            {
                PageHome page = new PageHome();

                //Блок курсів валют
                page.БлокКурсиВалют.StartDesktop();
                page.БлокКурсиВалют.StartAutoWork();

                //Активні користувачі
                page.АктивніКористувачі.AutoRefreshRun();

                return page;
            });

            //
            // Перевірка констант
            //

            if (!ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const)
            {
                CreateNotebookPage("Початкове заповнення", () =>
                {
                    return new Обробка_ПочатковеЗаповнення();
                });
            }
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

        #region BackgroundTask

        public void StartBackgroundTask()
        {
            Program.ListCancellationToken.Add(CancellationTokenBackgroundTask = new CancellationTokenSource());

            Thread ThreadBackgroundTask = new Thread(new ThreadStart(CalculationVirtualBalances));
            ThreadBackgroundTask.Start();
        }

        /*

        Схема роботи:

        1. В процесі запису в регістр залишків - додається запис у таблицю тригерів.
           Запис в таблицю тригерів містить дату запису в регістр, назву регістру.

        2. Раз на 5 сек викликається процедура SpetialTableRegAccumTrigerExecute і
           відбувається розрахунок віртуальних таблиць регістрів залишків.

           Розраховуються тільки змінені регістри на дату проведення документу і
           додатково на дату якщо змінена дата документу і документ уже був проведений.

           Додатково розраховуються підсумки в кінці всіх розрахунків.

        */

        void CalculationVirtualBalances()
        {
            int counter = 0;

            //Обновлення сесії
            UpdateSession();

            while (!CancellationTokenBackgroundTask!.IsCancellationRequested)
            {
                //Раз на 5 сек
                if (counter >= 5)
                {
                    //Обновлення сесії
                    UpdateSession();

                    //Зупинка розрахунків використовується при масовому перепроведенні документів щоб
                    //провести всі документ, а тоді вже розраховувати регістри
                    if (!Системні.ЗупинитиФоновіЗадачі_Const)
                    {
                        //Виконання обчислень
                        Config.Kernel!.DataBase.SpetialTableRegAccumTrigerExecute(
                            KernelSession,
                            VirtualTablesСalculation.Execute,
                            VirtualTablesСalculation.ExecuteFinalCalculation);
                    }

                    counter = 0;
                }

                counter++;

                //Затримка на 1 сек
                Thread.Sleep(1000);
            }

            //Закрити поточну сесію
            Config.Kernel!.DataBase.SpetialTableActiveUsersCloseSession(KernelSession);
        }

        void UpdateSession()
        {
            if (!Config.Kernel!.DataBase.SpetialTableActiveUsersUpdateSession(KernelSession))
            {
                // Log Off
            }
        }

        #endregion

        #region LeftMenu

        void CreateLeftMenu(HBox hbox)
        {
            VBox vbox = new VBox();
            vbox.BorderWidth = 0;

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
                Image = new Image(AppContext.BaseDirectory + image),
                AlwaysShowImage = true
            };

            lb.Image.Valign = Align.End;
            lb.Clicked += ClikAction;

            vBox.PackStart(lb, false, false, 10);
        }

        void Документи(object? sender, EventArgs args)
        {
            if (sender != null)
            {
                LinkButton lb = (LinkButton)sender;

                Popover po = new Popover(lb) { Position = PositionType.Right };
                po.Add(new PageDocuments());
                po.ShowAll();
            }
        }

        void Журнали(object? sender, EventArgs args)
        {
            if (sender != null)
            {
                LinkButton lb = (LinkButton)sender;

                Popover po = new Popover(lb) { Position = PositionType.Right };
                po.Add(new PageJournals());
                po.ShowAll();
            }
        }

        void Звіти(object? sender, EventArgs args)
        {
            if (sender != null)
            {
                LinkButton lb = (LinkButton)sender;

                Popover po = new Popover(lb) { Position = PositionType.Right };
                po.Add(new PageReports());
                po.ShowAll();
            }
        }

        void Довідники(object? sender, EventArgs args)
        {
            if (sender != null)
            {
                LinkButton lb = (LinkButton)sender;

                Popover po = new Popover(lb) { Position = PositionType.Right };
                po.Add(new PageDirectory());
                po.ShowAll();
            }
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

        /// <summary>
        /// Закрити поточну сторінку блокноту
        /// </summary>
        public void CloseCurrentPageNotebook()
        {
            topNotebook.RemovePage(topNotebook.CurrentPage);
        }

        /// <summary>
        /// Перейменувати поточну сторінку блокноту
        /// </summary>
        /// <param name="name">Назва</param>
        public void RenameCurrentPageNotebook(string name)
        {
            HBox hBoxLabel = CreateLabelPageWidget(name, topNotebook.CurrentPageWidget.Name, topNotebook);
            topNotebook.SetTabLabel(topNotebook.CurrentPageWidget, hBoxLabel);
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

            Label label = new Label { Text = caption, Expand = false, Halign = Align.Start };
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
                NotebookCloseTabToCode(notebook, ((Widget)sender!).Name);
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
        public void NotebookCloseTabToCode(Notebook notebook, string codePage)
        {
            notebook.Foreach(
                (Widget wg) =>
                {
                    if (wg.Name == codePage)
                        notebook.DetachTab(wg);
                });
        }

        /// <summary>
        /// Встановлення поточної сторінки по коду
        /// </summary>
        /// <param name="notebook">Блокнот</param>
        /// <param name="codePage">Код</param>
        public void NotebookCurrentPageToCode(Notebook notebook, string codePage)
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
        /// Створити сторінку в блокноті
        /// </summary>
        /// <param name="tabName">Назва сторінки</param>
        /// <param name="pageWidget">Віджет для сторінки</param>
        /// <param name="insertPage">Вставити сторінку перед поточною</param>
        public void CreateNotebookPage(string tabName, System.Func<Widget>? pageWidget, bool insertPage = false)
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
                scroll.Add((Widget)pageWidget.Invoke());

            topNotebook.ShowAll();
            topNotebook.CurrentPage = numPage;
            topNotebook.GrabFocus();
        }

        #endregion
    }
}