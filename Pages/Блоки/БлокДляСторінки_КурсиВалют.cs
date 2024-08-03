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

Блоку курсів валют

*/

using Gtk;

using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class БлокДляСторінки_КурсиВалют : VBox
    {
        Label lbDateLastDownload = new Label();
        Switch autoDownloadCursOnStart;
        VBox vBoxCursyValut;

        public БлокДляСторінки_КурсиВалют() : base()
        {
            HBox hBoxCaption = new HBox();
            hBoxCaption.PackStart(new Label("<b>Курси валют НБУ</b>") { UseMarkup = true }, false, false, 5);
            PackStart(hBoxCaption, false, false, 5);

            HBox hBoxDownloadCurs = new HBox();
            Button bDownloadCurs = new Button("Оновити");
            bDownloadCurs.Clicked += OnDownloadCurs;

            hBoxDownloadCurs.PackStart(bDownloadCurs, false, false, 0);
            hBoxDownloadCurs.PackStart(lbDateLastDownload, false, false, 5);

            PackStart(hBoxDownloadCurs, false, false, 5);

            //Переключатель: Завантажувати при запуску
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

                hBoxSwitch.PackStart(new Label("Авто"), false, false, 10);
                hBoxSwitch.PackStart(autoDownloadCursOnStart, false, false, 0);

                hBoxDownloadCurs.PackEnd(vBoxSwitch, false, false, 10);
            }

            //
            // Курси вибраних валют
            //

            HBox hBoxCursyValut = new HBox();
            PackStart(hBoxCursyValut, false, false, 5);

            vBoxCursyValut = new VBox() { WidthRequest = 300 };
            hBoxCursyValut.PackStart(vBoxCursyValut, false, false, 0);

            //
            // Лінки на довідник
            //

            VBox vBoxDirectory = new VBox();
            PackStart(vBoxDirectory, false, false, 5);

            HBox hBoxInfo = new HBox();
            vBoxDirectory.PackStart(hBoxInfo, false, false, 5);

            Link.AddLink(hBoxInfo, "Довідник - Валюти", async () =>
            {
                Валюти page = new Валюти();
                Program.GeneralForm?.CreateNotebookPage($"{Валюти_Const.FULLNAME}", () => { return page; });
                await page.LoadRecords();
            });

            Link.AddLink(hBoxInfo, "Історія завантажень", КурсиВалют_Історія);
        }

        void OnDownloadCurs(object? sender, EventArgs args)
        {
            Обробка_ЗавантаженняКурсівВалют page = new Обробка_ЗавантаженняКурсівВалют { CallBack_EndBackgroundWork = StartDesktop };
            Program.GeneralForm?.CreateNotebookPage("Завантаження курсів валют НБУ", () => { return page; });

            Task.Run(() =>
            {
                page.OnDownload(null, new EventArgs());
            });
        }

        /// <summary>
        /// Інтерфейс стартової сторінки
        /// </summary>
        public async void StartDesktop()
        {
            DateTime? ДатуОстанньогоЗавантаження = await ФункціїДляФоновихЗавдань.ОтриматиДатуОстанньогоЗавантаженняКурсуВалют();
            lbDateLastDownload.Text = "Оновлення: " + (ДатуОстанньогоЗавантаження != null ? ДатуОстанньогоЗавантаження.ToString() : "...");

            var recordResult = await ФункціїДляФоновихЗавдань.ОтриматиКурсиВалютДляСтартовоїСторінки();

            foreach (Widget child in vBoxCursyValut.Children)
                vBoxCursyValut.Remove(child);

            foreach (Dictionary<string, object> Row in recordResult.ListRow)
            {
                HBox hBoxItemValuta = new HBox();
                hBoxItemValuta.PackStart(new Label(Row["ВалютаНазва"].ToString()), false, false, 2);
                hBoxItemValuta.PackEnd(new Label("<b>" + Row["Курс"].ToString() + "</b>") { UseMarkup = true }, false, false, 6);
                vBoxCursyValut.PackStart(hBoxItemValuta, false, false, 4);
            }

            ShowAll();
        }

        /// <summary>
        /// Запуск автоматичного оновлення курсів валют
        /// </summary>
        public void StartAutoWork()
        {
            if (Константи.ЗавантаженняДанихІзСайтів.АвтоматичноЗавантажуватиКурсиВалютПриЗапуску_Const)
            {
                //Завантаження кожного разу при запуску
                Обробка_ЗавантаженняКурсівВалют page = new Обробка_ЗавантаженняКурсівВалют { CallBack_EndBackgroundWork = StartDesktop };
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

        void КурсиВалют_Історія()
        {
            КурсиВалют_ІсторіяЗавантаження page = new КурсиВалют_ІсторіяЗавантаження();
            page.LoadRecords();
            Program.GeneralForm?.CreateNotebookPage("Курси валют - історія завантаження", () => { return page; });
        }
    }
}