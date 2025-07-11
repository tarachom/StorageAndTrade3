/*

Блоку курсів валют

*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Константи;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class БлокДляСторінки_КурсиВалют : Форма
    {
        Label lbDateLastDownload = new Label();
        Switch autoDownloadCursOnStart;
        Box vBoxCursyValut;

        public БлокДляСторінки_КурсиВалют() : base()
        {
            Box hBoxCaption = new Box(Orientation.Horizontal, 0);
            hBoxCaption.PackStart(new Label("<b>Курси валют НБУ</b>") { UseMarkup = true }, false, false, 5);
            PackStart(hBoxCaption, false, false, 5);

            Box hBoxDownloadCurs = new Box(Orientation.Horizontal, 0);
            Button bDownloadCurs = new Button("Оновити");
            bDownloadCurs.Clicked += OnDownloadCurs;

            hBoxDownloadCurs.PackStart(bDownloadCurs, false, false, 0);
            hBoxDownloadCurs.PackStart(lbDateLastDownload, false, false, 5);

            PackStart(hBoxDownloadCurs, false, false, 5);

            //Переключатель: Завантажувати при запуску
            {
                Box vBoxSwitch = new Box(Orientation.Vertical, 0);

                Box hBoxSwitch = new Box(Orientation.Horizontal, 0);
                vBoxSwitch.PackStart(hBoxSwitch, false, false, 0);

                autoDownloadCursOnStart = new Switch()
                {
                    HeightRequest = 20,
                    Active = ЗавантаженняДанихІзСайтів.АвтоматичноЗавантажуватиКурсиВалютПриЗапуску_Const
                };

                autoDownloadCursOnStart.ButtonReleaseEvent += (object? sender, ButtonReleaseEventArgs args) =>
                    ЗавантаженняДанихІзСайтів.АвтоматичноЗавантажуватиКурсиВалютПриЗапуску_Const = !autoDownloadCursOnStart.Active;

                hBoxSwitch.PackStart(new Label("Авто"), false, false, 10);
                hBoxSwitch.PackStart(autoDownloadCursOnStart, false, false, 0);

                hBoxDownloadCurs.PackEnd(vBoxSwitch, false, false, 10);
            }

            //
            // Курси вибраних валют
            //

            Box hBoxCursyValut = new Box(Orientation.Horizontal, 0);
            PackStart(hBoxCursyValut, false, false, 5);

            vBoxCursyValut = new Box(Orientation.Vertical, 0) { WidthRequest = 300 };
            hBoxCursyValut.PackStart(vBoxCursyValut, false, false, 0);

            //
            // Лінки на довідник
            //

            Box vBoxDirectory = new Box(Orientation.Vertical, 0);
            PackStart(vBoxDirectory, false, false, 5);

            Box hBoxInfo = new Box(Orientation.Horizontal, 0);
            vBoxDirectory.PackStart(hBoxInfo, false, false, 5);

            CreateLink(hBoxInfo, "Довідник - Валюти", async () =>
            {
                Валюти page = new Валюти();
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{Валюти_Const.FULLNAME}", () => page);
                await page.SetValue();
            });

            CreateLink(hBoxInfo, "Історія завантажень", КурсиВалют_Історія);
        }

        void OnDownloadCurs(object? sender, EventArgs args)
        {
            Обробка_ЗавантаженняКурсівВалют page = new Обробка_ЗавантаженняКурсівВалют { CallBack_EndBackgroundWork = StartDesktop };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Завантаження курсів валют НБУ", () => page);

            Task.Run(() => page.OnDownload(null, new EventArgs()));
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
                Box hBoxItemValuta = new Box(Orientation.Horizontal, 0);
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
            if (ЗавантаженняДанихІзСайтів.АвтоматичноЗавантажуватиКурсиВалютПриЗапуску_Const)
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
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Курси валют - історія завантаження", () => page);
        }
    }
}