/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриВідомостей;

using System.Xml.XPath;

namespace StorageAndTrade
{
    class Обробка_ЗавантаженняКурсівВалют : ФормаЕлемент
    {
        #region Fields

        Button bDownload;
        Button bStop;
        ScrolledWindow scrollMessage;
        Box vBoxMessage;
        CancellationTokenSource? CancellationTokenThread { get; set; }

        CheckButton ЗавантаженняНаВказануДату = new CheckButton("Завантажити на дату:");
        DateTimeControl ДатаЗавантаженняКурсу = new DateTimeControl() { OnlyDate = true, Value = DateTime.Now };

        enum TypeMessage
        {
            Ok,
            Error,
            Info,
            None
        }

        #endregion

        public Обробка_ЗавантаженняКурсівВалют() : base()
        {
            //Кнопки
            Box hBoxTop = new Box(Orientation.Horizontal, 0);
            PackStart(hBoxTop, false, false, 10);

            bDownload = new Button("Завантаження");
            bDownload.Clicked += OnDownload;

            hBoxTop.PackStart(bDownload, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            hBoxTop.PackStart(bStop, false, false, 10);

            //Дата
            hBoxTop.PackStart(ЗавантаженняНаВказануДату, false, false, 5);
            hBoxTop.PackStart(ДатаЗавантаженняКурсу, false, false, 5);

            ЗавантаженняНаВказануДату.Activated += OnЗавантаженняНаВказануДату_Activated;

            scrollMessage = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollMessage.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollMessage.Add(vBoxMessage = new Box(Orientation.Vertical, 0));

            PackStart(scrollMessage, true, true, 0);

            ShowAll();
        }

        /// <summary>
        /// Функція яка викликається після завершення завантаження у фоновому режимі
        /// </summary>
        public System.Action? CallBack_EndBackgroundWork { get; set; }

        public async void OnDownload(object? sender, EventArgs args)
        {
            CancellationTokenThread = new CancellationTokenSource();
            await DownloadExCurr();
        }

        async ValueTask DownloadExCurr()
        {
            ButtonSensitive(false);
            ClearMessage();

            bool isOK = false;

            string link = Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Const;

            if (string.IsNullOrEmpty(link))
            {
                //За замовчуванням
                link = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange";
            }

            if (ЗавантаженняНаВказануДату.Active)
            {
                DateTime ДатаКурсу = ДатаЗавантаженняКурсу.Value;
                link += "?date=" + ДатаКурсу.Year.ToString() + ДатаКурсу.Month.ToString("D2") + ДатаКурсу.Day.ToString("D2");
            }

            CreateMessage(TypeMessage.Info, $"Завантаження ХМЛ файлу з курсами валют з офіційного сайту: bank.gov.ua");

            XPathDocument xPathDoc;
            XPathNavigator? xPathDocNavigator = null;

            try
            {
                xPathDoc = new XPathDocument(link);
                xPathDocNavigator = xPathDoc.CreateNavigator();

                isOK = true;

                CreateMessage(TypeMessage.Ok, "OK");
                await ФункціїДляФоновихЗавдань.ДодатиЗаписВІсторіюЗавантаженняКурсуВалют("OK", link);
            }
            catch (Exception ex)
            {
                CreateMessage(TypeMessage.Ok, "Помилка завантаження або аналізу ХМЛ файлу: " + ex.Message);
                await ФункціїДляФоновихЗавдань.ДодатиЗаписВІсторіюЗавантаженняКурсуВалют("Помилка", link, ex.Message);
            }

            if (isOK)
            {
                Валюти_Select валюти_Select = new Валюти_Select();

                DateTime ПоточнаДатаКурсу = DateTime.MinValue;

                XPathNodeIterator? КурсВалюти = xPathDocNavigator?.Select("/exchange/currency");
                while (КурсВалюти!.MoveNext())
                {
                    if (CancellationTokenThread!.IsCancellationRequested)
                        break;

                    string Код_R030 = int.Parse(КурсВалюти?.Current?.SelectSingleNode("r030")?.Value ?? "").ToString("D3");
                    string НазваВалюти = КурсВалюти?.Current?.SelectSingleNode("txt")?.Value ?? "";
                    string Коротко = КурсВалюти?.Current?.SelectSingleNode("cc")?.Value ?? "";
                    decimal Курс = decimal.Parse(КурсВалюти?.Current?.SelectSingleNode("rate")?.Value.Replace(".", ",") ?? "");
                    DateTime ДатаКурсу = DateTime.Parse(КурсВалюти?.Current?.SelectSingleNode("exchangedate")?.Value ?? DateTime.Now.ToString());

                    if (ДатаКурсу != ПоточнаДатаКурсу)
                    {
                        CreateMessage(TypeMessage.Ok, $"Курс на дату: {ДатаКурсу}");

                        ПоточнаДатаКурсу = ДатаКурсу;
                    }

                    Валюти_Pointer валюти_Pointer = await валюти_Select.FindByField(Валюти_Const.Код_R030, Код_R030);
                    if (валюти_Pointer.IsEmpty())
                    {
                        Валюти_Objest валюти_Objest = new Валюти_Objest();
                        await валюти_Objest.New();
                        валюти_Objest.Назва = НазваВалюти;
                        валюти_Objest.Код_R030 = Код_R030;
                        валюти_Objest.КороткаНазва = Коротко;
                        await валюти_Objest.Save();

                        валюти_Pointer = валюти_Objest.GetDirectoryPointer();

                        CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника Валюти: {НазваВалюти}, код {Код_R030}");
                    }

                    string query = $@"
SELECT
    КурсиВалют.uid
FROM
    {КурсиВалют_Const.TABLE} AS КурсиВалют
WHERE
    КурсиВалют.{КурсиВалют_Const.Валюта} = @Валюта AND
    date_trunc('day', КурсиВалют.period::timestamp) = date_trunc('day', @ДатаКурсу::timestamp)
LIMIT 1
";
                    Dictionary<string, object> paramQuery = new Dictionary<string, object>
                    {
                        { "Валюта", валюти_Pointer.UnigueID.UGuid },
                        { "ДатаКурсу", ДатаКурсу }
                    };

                    var recordResult = await Config.Kernel.DataBase.SelectRequest(query, paramQuery);
                    if (!recordResult.Result)
                    {
                        КурсиВалют_Objest курсиВалют_Objest = new КурсиВалют_Objest
                        {
                            Period = ДатаКурсу,
                            Валюта = валюти_Pointer,
                            Кратність = 1,
                            Курс = Курс
                        };
                        курсиВалют_Objest.New();
                        await курсиВалют_Objest.Save();

                        CreateMessage(TypeMessage.Ok, $"Додано новий курс валюти: {НазваВалюти} - курс {Курс}");
                    }
                    else
                    {
                        Dictionary<string, object> Рядок = recordResult.ListRow[0];

                        КурсиВалют_Objest курсиВалют_Objest = new КурсиВалют_Objest();
                        if (await курсиВалют_Objest.Read(new UnigueID(Рядок["uid"])))
                        {
                            курсиВалют_Objest.Курс = Курс;
                            await курсиВалют_Objest.Save();

                            CreateMessage(TypeMessage.Ok, $"Перезаписано курс валюти: {НазваВалюти} - курс {Курс}");
                        }
                    }
                }
            }

            ButtonSensitive(true);

            CallBack_EndBackgroundWork?.Invoke();

            CreateMessage(TypeMessage.None, "\n\n\nГотово!\n\n\n");
            await Task.Delay(1000);
            CreateMessage(TypeMessage.None, "\n\n\n\n");
        }

        void ButtonSensitive(bool sensitive)
        {
            bDownload.Sensitive = sensitive;
            bStop.Sensitive = !sensitive;
        }

        void CreateMessage(TypeMessage typeMsg, string message)
        {
            Box hBoxInfo = new Box(Orientation.Horizontal, 0);
            vBoxMessage.PackStart(hBoxInfo, false, false, 2);

            switch (typeMsg)
            {
                case TypeMessage.Ok:
                    {
                        hBoxInfo.PackStart(new Image(AppContext.BaseDirectory + "images/16/ok.png"), false, false, 5);
                        break;
                    }
                case TypeMessage.Error:
                    {
                        hBoxInfo.PackStart(new Image(AppContext.BaseDirectory + "images/16/error.png"), false, false, 5);
                        break;
                    }
                case TypeMessage.Info:
                    {
                        hBoxInfo.PackStart(new Image(AppContext.BaseDirectory + "images/16/info.png"), false, false, 5);
                        break;
                    }
                case TypeMessage.None:
                    {
                        hBoxInfo.PackStart(new Label(""), false, false, 5);
                        break;
                    }
            }

            hBoxInfo.PackStart(new Label(message) { Wrap = true }, false, false, 0);
            hBoxInfo.ShowAll();

            scrollMessage.Vadjustment.Value = scrollMessage.Vadjustment.Upper;
        }

        void ClearMessage()
        {
            foreach (Widget Child in vBoxMessage.Children)
                vBoxMessage.Remove(Child);
        }

        void OnStopClick(object? sender, EventArgs args)
        {
            CancellationTokenThread?.Cancel();
        }

        void OnЗавантаженняНаВказануДату_Activated(object? sender, EventArgs args)
        {
            ДатаЗавантаженняКурсу.Sensitive = ЗавантаженняНаВказануДату.Active;
        }
    }
}