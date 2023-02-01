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
using StorageAndTrade_1_0.РегістриНакопичення;
using Константи = StorageAndTrade_1_0.Константи;

namespace StorageAndTrade
{
    class PageHome : VBox
    {
        Label lbDateLastDownload = new Label();
        Switch autoDownloadCursOnStart;
        VBox vBoxCursyValut;

        public PageHome() : base()
        {
            //Завантаження курсів валют
            HBox hBoxDownloadCurs = new HBox();

            hBoxDownloadCurs.PackStart(new Label("Курси валют НБУ"), false, false, 5);
            Button bDownloadCurs = new Button("Оновити");
            bDownloadCurs.Clicked += (object? sender, EventArgs args) =>
            {
                Обробка_ЗавантаженняКурсівВалют page = new Обробка_ЗавантаженняКурсівВалют();
                page.CallBack_EndBackgroundWork = StartDesktop;

                Program.GeneralForm?.CreateNotebookPage("Завантаження курсів валют НБУ", () => { return page; });

                page.OnDownload(null, new EventArgs());
            };

            hBoxDownloadCurs.PackStart(bDownloadCurs, false, false, 5);
            hBoxDownloadCurs.PackStart(lbDateLastDownload, false, false, 5);

            PackStart(hBoxDownloadCurs, false, false, 5);

            //Завантажувати при запуску
            {
                VBox vBoxSwitch = new VBox();

                HBox hBoxSwitch = new HBox();
                vBoxSwitch.PackStart(hBoxSwitch, false, false, 0);

                autoDownloadCursOnStart = new Switch()
                {
                    HeightRequest = 20,
                    Active = Константи.ЗавантаженняДанихІзСайтів.АвтоматичноЗавантажуватиКурсиВалютПриЗапуску_Const
                };

                autoDownloadCursOnStart.ButtonReleaseEvent += (object? sender, ButtonReleaseEventArgs args) =>
                {
                    Константи.ЗавантаженняДанихІзСайтів.АвтоматичноЗавантажуватиКурсиВалютПриЗапуску_Const = !autoDownloadCursOnStart.Active;
                };

                hBoxSwitch.PackStart(autoDownloadCursOnStart, false, false, 0);
                hBoxSwitch.PackStart(new Label("Автоматично обновляти при запуску"), false, false, 10);

                hBoxDownloadCurs.PackEnd(vBoxSwitch, false, false, 10);
            }

            HBox hBoxCursyValut = new HBox();
            PackStart(hBoxCursyValut, false, false, 5);

            vBoxCursyValut = new VBox() { WidthRequest = 200 };
            hBoxCursyValut.PackStart(vBoxCursyValut, false, false, 5);

            VBox vBoxDirectory = new VBox(false, 0);
            PackStart(vBoxDirectory, false, false, 5);

            AddLink(vBoxDirectory, "Довідник - Валюти", PageDirectory.Валюти);
            AddLink(vBoxDirectory, "Історія завантажень", КурсиВалют_Історія);

            ShowAll();
        }

        public void StartDesktop()
        {
            DateTime? ДатуОстанньогоЗавантаження = ФункціїДляФоновихЗавдань.ОтриматиДатуОстанньогоЗавантаженняКурсуВалют();
            lbDateLastDownload.Text = "Оновлення: " + (ДатуОстанньогоЗавантаження != null ? ДатуОстанньогоЗавантаження.ToString() : "...");

            foreach (Widget child in vBoxCursyValut.Children)
                vBoxCursyValut.Remove(child);

            List<Dictionary<string, object>> ListRow = ФункціїДляФоновихЗавдань.ОтриматиКурсиВалютДляСтартовоїСторінки();

            foreach (Dictionary<string, object> Row in ListRow)
            {
                HBox hBoxItemValuta = new HBox();
                hBoxItemValuta.PackStart(new Label(Row["ВалютаНазва"].ToString()), false, false, 0);
                hBoxItemValuta.PackEnd(new Label(Row["Курс"].ToString()), false, false, 0);
                vBoxCursyValut.PackStart(hBoxItemValuta, false, false, 5);
            }

            ShowAll();
        }

        public void StartAutoWork()
        {
            if (Константи.ЗавантаженняДанихІзСайтів.АвтоматичноЗавантажуватиКурсиВалютПриЗапуску_Const)
            {
                //Завантаження кожного разу при запуску
                Обробка_ЗавантаженняКурсівВалют page = new Обробка_ЗавантаженняКурсівВалют();
                page.IsBackgroundWork = true;
                page.CallBack_EndBackgroundWork = StartDesktop;

                page.OnDownload(null, new EventArgs());

                //Завантаження один раз на день
                /*
                DateTime? ДатуОстанньогоЗавантаження = ФункціїДляФоновихЗавдань.ОтриматиДатуОстанньогоЗавантаженняКурсуВалют();

                if (ДатуОстанньогоЗавантаження == null)
                    ДатуОстанньогоЗавантаження = DateTime.MinValue;

                DateOnly ДатаЗавантаження = DateOnly.FromDateTime(ДатуОстанньогоЗавантаження.Value);
                DateOnly ДатаСьогоднішня = DateOnly.FromDateTime(DateTime.Now);

                if (ДатаЗавантаження != ДатаСьогоднішня)
                {
                    ЗавантаженняКурсівВалют page = new ЗавантаженняКурсівВалют();
                    page.IsBackgroundWork = true;
                    page.CallBack_EndBackgroundWork = StartDesktop;

                    page.OnDownload(null, new EventArgs());
                }
                */
            }
        }

        void КурсиВалют_Історія(object? sender, EventArgs args)
        {
            КурсиВалют_ІсторіяЗавантаження page = new КурсиВалют_ІсторіяЗавантаження();
            page.LoadRecords();
            Program.GeneralForm?.CreateNotebookPage("Курси валют - історія завантаження", () => { return page; });
        }

        void AddLink(VBox vbox, string uri, EventHandler? clickAction = null)
        {
            LinkButton lb = new LinkButton(uri, " " + uri) { Halign = Align.Start, Image = new Image("images/doc.png"), AlwaysShowImage = true };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += clickAction;
        }

        public void StartBackgroundTask()
        {
            Program.CancellationTokenBackgroundTask = new CancellationTokenSource();

            Thread ThreadBackgroundTask = new Thread(new ThreadStart(CalculationVirtualBalances));
            ThreadBackgroundTask.Start();
        }

        void CalculationVirtualBalances()
        {
            int counter = 0;

            while (!Program.CancellationTokenBackgroundTask!.IsCancellationRequested)
            {
                if (counter > 5)
                {
                    if (!Константи.Системні.ЗупинитиФоновіЗадачі_Const)
                    {
                        Config.Kernel!.DataBase.SpetialTableRegAccumTrigerExecute(
                            VirtualTablesСalculation.Execute,
                            VirtualTablesСalculation.ExecuteFinalCalculation);
                    }

                    counter = 0;
                }

                counter++;

                Thread.Sleep(1000);
            }
        }
    }
}