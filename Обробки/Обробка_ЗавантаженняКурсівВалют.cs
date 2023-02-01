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

using StorageAndTrade_1_0;
using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриВідомостей;

using System.Xml.XPath;

namespace StorageAndTrade
{
    class Обробка_ЗавантаженняКурсівВалют : VBox
    {
        #region Fields

        Button bClose;
        Button bDownload;
        Button bStop;
        ScrolledWindow scrollMessage;
        VBox vBoxMessage;
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
            HBox hBoxBotton = new HBox();

            bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            bDownload = new Button("Завантаження");
            bDownload.Clicked += OnDownload;

            hBoxBotton.PackStart(bDownload, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            hBoxBotton.PackStart(bStop, false, false, 10);

            //Дата
            hBoxBotton.PackStart(ЗавантаженняНаВказануДату, false, false, 5);
            hBoxBotton.PackStart(ДатаЗавантаженняКурсу, false, false, 5);

            ЗавантаженняНаВказануДату.Activated += OnЗавантаженняНаВказануДату_Activated;

            PackStart(hBoxBotton, false, false, 10);

            scrollMessage = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollMessage.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollMessage.Add(vBoxMessage = new VBox());

            PackStart(scrollMessage, true, true, 0);

            ShowAll();
        }

        /// <summary>
        /// Функція яка викликається після завершення завантаження у фоновому режимі
        /// </summary>
        public System.Action? CallBack_EndBackgroundWork { get; set; }

        public void OnDownload(object? sender, EventArgs args)
        {
            ClearMessage();

            CancellationTokenThread = new CancellationTokenSource();
            Thread thread = new Thread(new ThreadStart(DownloadExCurr));
            thread.Start();
        }

        void DownloadExCurr()
        {
            ButtonSensitive(false);

            bool isOK = false;

            string link = Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняКурсівВалют_Const;

            if (String.IsNullOrEmpty(link))
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

                ФункціїДляФоновихЗавдань.ДодатиЗаписВІсторіюЗавантаженняКурсуВалют("OK", link);
            }
            catch (Exception ex)
            {
                CreateMessage(TypeMessage.Ok, "Помилка завантаження або аналізу ХМЛ файлу: " + ex.Message);

                ФункціїДляФоновихЗавдань.ДодатиЗаписВІсторіюЗавантаженняКурсуВалют("Помилка", link, ex.Message);
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

                    Валюти_Pointer валюти_Pointer = валюти_Select.FindByField(Валюти_Const.Код_R030, Код_R030);
                    if (валюти_Pointer.IsEmpty())
                    {
                        Валюти_Objest валюти_Objest = new Валюти_Objest();
                        валюти_Objest.New();
                        валюти_Objest.Код = (++Константи.НумераціяДовідників.Валюти_Const).ToString("D6");
                        валюти_Objest.Назва = НазваВалюти;
                        валюти_Objest.Код_R030 = Код_R030;
                        валюти_Objest.КороткаНазва = Коротко;
                        валюти_Objest.Save();

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
                    Dictionary<string, object> paramQuery = new Dictionary<string, object>();
                    paramQuery.Add("Валюта", валюти_Pointer.UnigueID.UGuid);
                    paramQuery.Add("ДатаКурсу", ДатаКурсу);

                    string[] columnsName;
                    List<Dictionary<string, object>> listRow;

                    Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

                    if (listRow.Count == 0)
                    {
                        КурсиВалют_Objest курсиВалют_Objest = new КурсиВалют_Objest();
                        курсиВалют_Objest.New();
                        курсиВалют_Objest.Period = ДатаКурсу;
                        курсиВалют_Objest.Валюта = валюти_Pointer;
                        курсиВалют_Objest.Кратність = 1;
                        курсиВалют_Objest.Курс = Курс;
                        курсиВалют_Objest.Save();

                        CreateMessage(TypeMessage.Ok, $"Додано новий курс валюти: {НазваВалюти} - курс {Курс}");
                    }
                    else
                    {
                        Dictionary<string, object> Рядок = listRow[0];

                        КурсиВалют_Objest курсиВалют_Objest = new КурсиВалют_Objest();
                        if (курсиВалют_Objest.Read(new UnigueID(Рядок["uid"])))
                        {
                            курсиВалют_Objest.Курс = Курс;
                            курсиВалют_Objest.Save();

                            CreateMessage(TypeMessage.Ok, $"Перезаписано курс валюти: {НазваВалюти} - курс {Курс}");
                        }
                    }
                }
            }

            ButtonSensitive(true);

            if (CallBack_EndBackgroundWork != null)
            {
                Gtk.Application.Invoke
                (
                    delegate
                    {
                        CallBack_EndBackgroundWork.Invoke();
                    }
                );
            }
        }

        void ButtonSensitive(bool sensitive)
        {
            Gtk.Application.Invoke
            (
                delegate
                {
                    bClose.Sensitive = sensitive;
                    bDownload.Sensitive = sensitive;
                    bStop.Sensitive = !sensitive;
                }
            );
        }

        void CreateMessage(TypeMessage typeMsg, string message)
        {
            Gtk.Application.Invoke
            (
                delegate
                {
                    HBox hBoxInfo = new HBox();
                    vBoxMessage.PackStart(hBoxInfo, false, false, 2);

                    switch (typeMsg)
                    {
                        case TypeMessage.Ok:
                            {
                                hBoxInfo.PackStart(new Image("images/16/ok.png"), false, false, 5);
                                break;
                            }
                        case TypeMessage.Error:
                            {
                                hBoxInfo.PackStart(new Image("images/16/error.png"), false, false, 5);
                                break;
                            }
                        case TypeMessage.Info:
                            {
                                hBoxInfo.PackStart(new Image("images/16/info.png"), false, false, 5);
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
            );
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