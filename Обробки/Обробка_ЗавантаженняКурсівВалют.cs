
/*
        Обробка_ЗавантаженняКурсівВалют.cs
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode;
using Константи = GeneratedCode.Константи;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриВідомостей;

using System.Xml.XPath;

namespace StorageAndTrade
{
    class Обробка_ЗавантаженняКурсівВалют : Обробка
    {
        #region Fields

        Button bDownload;
        Button bStop;
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

        public Обробка_ЗавантаженняКурсівВалют()
        {
            bDownload = new Button("Завантаження");
            bDownload.Clicked += OnDownload;

            HBoxTop.PackStart(bDownload, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            HBoxTop.PackStart(bStop, false, false, 10);

            //Дата
            HBoxTop.PackStart(ЗавантаженняНаВказануДату, false, false, 5);
            HBoxTop.PackStart(ДатаЗавантаженняКурсу, false, false, 5);

            ЗавантаженняНаВказануДату.Activated += OnЗавантаженняНаВказануДату_Activated;

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

            Лог.CreateMessage($"Завантаження ХМЛ файлу з курсами валют з офіційного сайту: bank.gov.ua", LogMessage.TypeMessage.Info);

            XPathDocument xPathDoc;
            XPathNavigator? xPathDocNavigator = null;

            try
            {
                xPathDoc = new XPathDocument(link);
                xPathDocNavigator = xPathDoc.CreateNavigator();

                isOK = true;

                Лог.CreateMessage("OK", LogMessage.TypeMessage.Ok);
                await ФункціїДляФоновихЗавдань.ДодатиЗаписВІсторіюЗавантаженняКурсуВалют("OK", link);
            }
            catch (Exception ex)
            {
                Лог.CreateMessage("Помилка завантаження або аналізу ХМЛ файлу: " + ex.Message, LogMessage.TypeMessage.Error);
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
                    DateTime.TryParse(КурсВалюти?.Current?.SelectSingleNode("exchangedate")?.Value ?? DateTime.Now.ToString(), out DateTime ДатаКурсу);

                    if (ДатаКурсу != ПоточнаДатаКурсу)
                    {
                        Лог.CreateMessage($"Курс на дату: {ДатаКурсу}", LogMessage.TypeMessage.Ok);

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

                        Лог.CreateMessage($"Додано новий елемент довідника Валюти: {НазваВалюти}, код {Код_R030}", LogMessage.TypeMessage.Ok);
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

                        Лог.CreateMessage($"Додано новий курс валюти: {НазваВалюти} - курс {Курс}", LogMessage.TypeMessage.Ok);
                    }
                    else
                    {
                        Dictionary<string, object> Рядок = recordResult.ListRow[0];

                        КурсиВалют_Objest курсиВалют_Objest = new КурсиВалют_Objest();
                        if (await курсиВалют_Objest.Read(new UnigueID(Рядок["uid"])))
                        {
                            курсиВалют_Objest.Курс = Курс;
                            await курсиВалют_Objest.Save();

                            Лог.CreateMessage($"Перезаписано курс валюти: {НазваВалюти} - курс {Курс}", LogMessage.TypeMessage.Ok);
                        }
                    }
                }
            }

            ButtonSensitive(true);

            CallBack_EndBackgroundWork?.Invoke();

            Лог.CreateEmptyMsg();
            Лог.CreateMessage("Готово!");
            
            await Task.Delay(1000);
            Лог.CreateEmptyMsg();
        }

        void ButtonSensitive(bool sensitive)
        {
            bDownload.Sensitive = sensitive;
            bStop.Sensitive = !sensitive;
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