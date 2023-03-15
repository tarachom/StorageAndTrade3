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

using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using System.Xml.XPath;

namespace StorageAndTrade
{
    class Обробка_ЗавантаженняБанків : VBox
    {
        #region Fields

        Button bDownload;
        Button bStop;
        ScrolledWindow scrollMessage;
        VBox vBoxMessage;
        CancellationTokenSource? CancellationTokenThread { get; set; }

        enum TypeMessage
        {
            Ok,
            Error,
            Info,
            None
        }

        #endregion

        public Обробка_ЗавантаженняБанків() : base()
        {
            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            bDownload = new Button("Завантаження");
            bDownload.Clicked += OnDownload;

            hBoxTop.PackStart(bDownload, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            hBoxTop.PackStart(bStop, false, false, 10);

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

            Program.ListCancellationToken.Add(CancellationTokenThread = new CancellationTokenSource());
            Thread thread = new Thread(new ThreadStart(DownloadExCurr));
            thread.Start();
        }

        void DownloadExCurr()
        {
            ButtonSensitive(false);

            bool isOK = false;

            string link = Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняСпискуБанків_Const;

            if (String.IsNullOrEmpty(link))
            {
                //За замовчуванням
                link = "https://bank.gov.ua/NBU_BankInfo/get_data_branch_glbank";
            }

            CreateMessage(TypeMessage.Info, $"Завантаження ХМЛ файлу із списком банків з офіційного сайту: bank.gov.ua");

            XPathDocument xPathDoc;
            XPathNavigator? xPathDocNavigator = null;

            try
            {
                xPathDoc = new XPathDocument(link);
                xPathDocNavigator = xPathDoc.CreateNavigator();

                isOK = true;

                CreateMessage(TypeMessage.Ok, "OK");
            }
            catch (Exception ex)
            {
                CreateMessage(TypeMessage.Ok, "Помилка завантаження або аналізу ХМЛ файлу: " + ex.Message);
            }

            if (isOK)
            {
                Банки_Select банки_Select = new Банки_Select();

                XPathNodeIterator? Записи = xPathDocNavigator?.Select("/BANKBRANCH/ROW");
                while (Записи!.MoveNext())
                {
                    if (CancellationTokenThread!.IsCancellationRequested)
                        break;

                    string КодМФО = Записи?.Current?.SelectSingleNode("GLMFO")?.Value ?? "";

                    if (String.IsNullOrEmpty(КодМФО))
                    {
                        CreateMessage(TypeMessage.Error, "Відсутній КодМФО");
                        break;
                    }

                    string НазваГоловноїУстановиАнг = Записи?.Current?.SelectSingleNode("NAME_E")?.Value ?? "";
                    string КодЄДРПОУ = Записи?.Current?.SelectSingleNode("KOD_EDRPOU")?.Value ?? "";
                    string Назва = Записи?.Current?.SelectSingleNode("SHORTNAME")?.Value ?? "";
                    string ПовнаНазва = Записи?.Current?.SelectSingleNode("FULLNAME")?.Value ?? "";
                    string УнікальнийКодБанку = Записи?.Current?.SelectSingleNode("NKB")?.Value ?? "";
                    string КодОбластіОпераційноїДіяльності = Записи?.Current?.SelectSingleNode("KU")?.Value ?? "";
                    string НазваОбластіОпераційноїДіяльності = Записи?.Current?.SelectSingleNode("N_OBL")?.Value ?? "";
                    string КодОбластіЗгідноСтатуту = Записи?.Current?.SelectSingleNode("OBL_UR")?.Value ?? "";
                    string НазваОбластіЗгідноСтатуту = Записи?.Current?.SelectSingleNode("N_OBL_UR")?.Value ?? "";
                    string ПоштовийІндекс = Записи?.Current?.SelectSingleNode("P_IND")?.Value ?? "";
                    string ТипНаселеногоПункту = Записи?.Current?.SelectSingleNode("TNP")?.Value ?? "";
                    string НазваНаселеногоПункту = Записи?.Current?.SelectSingleNode("NP")?.Value ?? "";
                    string Адреса = Записи?.Current?.SelectSingleNode("ADRESS")?.Value ?? "";
                    string КодТелефонногоЗвязку = Записи?.Current?.SelectSingleNode("KODT")?.Value ?? "";
                    string НомерТелефону = Записи?.Current?.SelectSingleNode("TELEFON")?.Value ?? "";
                    string ЧисловийКодСтануУстанови = Записи?.Current?.SelectSingleNode("KSTAN")?.Value ?? "";
                    string НазваСтануУстанови = Записи?.Current?.SelectSingleNode("N_STAN")?.Value ?? "";
                    string ДатаЗміниСтану = Записи?.Current?.SelectSingleNode("D_STAN")?.Value ?? "";
                    string ДатаВідкриттяУстанови = Записи?.Current?.SelectSingleNode("D_OPEN")?.Value ?? "";
                    string ДатаЗакриттяУстанови = Записи?.Current?.SelectSingleNode("D_CLOSE")?.Value ?? "";
                    string КодНБУ = Записи?.Current?.SelectSingleNode("IDNBU")?.Value ?? "";
                    string НомерЛіцензії = Записи?.Current?.SelectSingleNode("NUM_LIC")?.Value ?? "";
                    string ДатаЛіцензії = Записи?.Current?.SelectSingleNode("DT_GRAND_LIC")?.Value ?? "";
                    string КодСтатусу = Записи?.Current?.SelectSingleNode("PR_LIC")?.Value ?? "";
                    string Статус = Записи?.Current?.SelectSingleNode("N_PR_LIC")?.Value ?? "";
                    string ДатаЗапису = Записи?.Current?.SelectSingleNode("DT_LIC")?.Value ?? "";

                    Банки_Pointer банки_Pointer = банки_Select.FindByField(Банки_Const.КодМФО, КодМФО);
                    if (банки_Pointer.IsEmpty())
                    {
                        Банки_Objest банки_Objest = new Банки_Objest();
                        банки_Objest.New();
                        банки_Objest.Код = (++Константи.НумераціяДовідників.Банки_Const).ToString("D6");
                        банки_Objest.НазваГоловноїУстановиАнг = НазваГоловноїУстановиАнг;
                        банки_Objest.КодЄДРПОУ = КодЄДРПОУ;
                        банки_Objest.Назва = Назва;
                        банки_Objest.ПовнаНазва = ПовнаНазва;
                        банки_Objest.УнікальнийКодБанку = УнікальнийКодБанку;
                        банки_Objest.КодОбластіОпераційноїДіяльності = КодОбластіОпераційноїДіяльності;
                        банки_Objest.НазваОбластіОпераційноїДіяльності = НазваОбластіОпераційноїДіяльності;
                        банки_Objest.КодОбластіЗгідноСтатуту = КодОбластіЗгідноСтатуту;
                        банки_Objest.НазваОбластіЗгідноСтатуту = НазваОбластіЗгідноСтатуту;
                        банки_Objest.ПоштовийІндекс = ПоштовийІндекс;
                        банки_Objest.ТипНаселеногоПункту = ТипНаселеногоПункту;
                        банки_Objest.НазваНаселеногоПункту = НазваНаселеногоПункту;
                        банки_Objest.Адреса = Адреса;
                        банки_Objest.КодТелефонногоЗвязку = КодТелефонногоЗвязку;
                        банки_Objest.НомерТелефону = НомерТелефону;
                        банки_Objest.ЧисловийКодСтануУстанови = ЧисловийКодСтануУстанови;
                        банки_Objest.НазваСтануУстанови = НазваСтануУстанови;
                        банки_Objest.ДатаЗміниСтану = ДатаЗміниСтану;
                        банки_Objest.ДатаВідкриттяУстанови = ДатаВідкриттяУстанови;
                        банки_Objest.ДатаЗакриттяУстанови = ДатаЗакриттяУстанови;
                        банки_Objest.КодНБУ = КодНБУ;
                        банки_Objest.НомерЛіцензії = НомерЛіцензії;
                        банки_Objest.ДатаЛіцензії = ДатаЛіцензії;
                        банки_Objest.КодСтатусу = КодСтатусу;
                        банки_Objest.Статус = Статус;
                        банки_Objest.ДатаЗапису = ДатаЗапису;

                        банки_Objest.Save();

                        CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника Банки: {Назва}");
                    }

                }
            }

            ButtonSensitive(true);
        }

        void ButtonSensitive(bool sensitive)
        {
            Gtk.Application.Invoke
            (
                delegate
                {
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
    }
}