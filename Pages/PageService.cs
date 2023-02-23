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

Масове перепроведення документів

*/

using Gtk;

using System.Reflection;
using AccountingSoftware;
using Константи = StorageAndTrade_1_0.Константи;
using Журнали = StorageAndTrade_1_0.Журнали;

namespace StorageAndTrade
{
    class PageService : VBox
    {
        Button bSpendTheDocument;
        Button bStop;
        ScrolledWindow scrollMessage;
        VBox vBoxMessage = new VBox();

        enum TypeMessage
        {
            Ok,
            Error,
            Info,
            None
        }

        public PageService() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            bSpendTheDocument = new Button("Перепровести документи");
            bSpendTheDocument.Clicked += OnSpendTheDocument;

            hBoxBotton.PackStart(bSpendTheDocument, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            hBoxBotton.PackStart(bStop, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            scrollMessage = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollMessage.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollMessage.Add(vBoxMessage);

            PackStart(scrollMessage, true, true, 0);

            ShowAll();
        }

        void ButtonSensitive(bool sensitive)
        {
            Gtk.Application.Invoke
            (
                delegate
                {
                    bSpendTheDocument.Sensitive = sensitive;
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

        #region SpendTheDocument

        void OnSpendTheDocument(object? sender, EventArgs args)
        {
            ClearMessage();

            Program.CancellationTokenPageService = new CancellationTokenSource();
            Thread thread = new Thread(new ThreadStart(SpendTheDocument));
            thread.Start();
        }

        void SpendTheDocument()
        {
            ButtonSensitive(false);

            Константи.Системні.ЗупинитиФоновіЗадачі_Const = true;

            ФункціїДляПовідомлень.ОчиститиПовідомлення();

            int counter = 0;

            Журнали.Journal_Select journalSelect = new Журнали.Journal_Select();
            journalSelect.Select(DateTime.Parse("01.01.2000 00:00:00"), DateTime.Now);

            while (journalSelect.MoveNext())
            {
                if (Program.CancellationTokenPageService!.IsCancellationRequested)
                    break;

                if (journalSelect.Current.Spend)
                {
                    DocumentObject? doc = journalSelect.GetDocumentObject(true);

                    if (doc != null)
                    {
                        if (doc.GetType().GetMember("SpendTheDocument").Length == 1)
                        {
                            try
                            {
                                object? obj = doc.GetType().InvokeMember("SpendTheDocument",
                                     BindingFlags.InvokeMethod, null, doc, new object[] { journalSelect.Current.SpendDate });

                                counter++;

                                bool rezult = obj != null ? (bool)obj : false;

                                if (!rezult)
                                {
                                    List<Dictionary<string, object>> listRow = ФункціїДляПовідомлень.ПрочитатиПовідомленняПроПомилку();
                                    string msg = "";

                                    foreach (Dictionary<string, object> row in listRow)
                                        msg += row["Повідомлення"].ToString();

                                    CreateMessage(TypeMessage.Error, msg);
                                    CreateMessage(TypeMessage.Info, "\n\nПроведення документів перервано!\n\n");

                                    //Очистка проводок документу
                                    doc.GetType().InvokeMember("ClearSpendTheDocument", BindingFlags.InvokeMethod, null, doc, new object[] { });

                                    ФункціїДляПовідомлень.ВідкритиТермінал();
                                    break;
                                }
                                else
                                    CreateMessage(TypeMessage.Ok, journalSelect.Current.TypeDocument + " " + journalSelect.Current.SpendDate);
                            }
                            catch (Exception ex)
                            {
                                CreateMessage(TypeMessage.Error, ex.Message);

                                //Очистка проводок документу
                                doc.GetType().InvokeMember("ClearSpendTheDocument", BindingFlags.InvokeMethod, null, doc, new object[] { });
                            }
                        }
                    }
                }
            }

            CreateMessage(TypeMessage.None, "Готово!\n\n\n");
            CreateMessage(TypeMessage.Info, "Проведено документів: " + counter);

            Константи.Системні.ЗупинитиФоновіЗадачі_Const = false;
            ButtonSensitive(true);

            Thread.Sleep(1000);
            CreateMessage(TypeMessage.None, "\n\n\n");
        }

        #endregion

        void OnStopClick(object? sender, EventArgs args)
        {
            Program.CancellationTokenPageService?.Cancel();
        }
    }
}