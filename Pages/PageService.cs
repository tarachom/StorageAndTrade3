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
        CancellationTokenSource? CancellationTokenPageService;

        Button bSpendTheDocument;
        Button bClearDeletionLabel;
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

            bClearDeletionLabel = new Button("Очистити помічені на видалення");
            bClearDeletionLabel.Clicked += OnClearDeletionLabel;
            hBoxBotton.PackStart(bClearDeletionLabel, false, false, 10);

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
                    bClearDeletionLabel.Sensitive = sensitive;
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

        #region SpendTheDocument

        void OnSpendTheDocument(object? sender, EventArgs args)
        {
            ClearMessage();

            Program.ListCancellationToken.Add(CancellationTokenPageService = new CancellationTokenSource());

            Thread thread = new Thread(new ThreadStart(SpendTheDocument));
            thread.Start();
        }

        void SpendTheDocument()
        {
            ButtonSensitive(false);

            int counter = 0;
            Константи.Системні.ЗупинитиФоновіЗадачі_Const = true;

            ФункціїДляПовідомлень.ОчиститиПовідомлення();

            Журнали.Journal_Select journalSelect = new Журнали.Journal_Select();

            // Вибірка всіх документів. Встановлюється максимальний період
            journalSelect.Select(DateTime.Parse("01.01.2000 00:00:00"), DateTime.Now);

            while (journalSelect.MoveNext())
            {
                if (CancellationTokenPageService!.IsCancellationRequested)
                    break;

                //Обробляються тільки не помічені на видалення і проведені
                if (!journalSelect.Current.DeletionLabel && journalSelect.Current.Spend)
                {
                    DocumentObject? doc = journalSelect.GetDocumentObject(true);
                    if (doc != null)
                    {
                        //Для документу викликається функція проведення
                        object? obj = doc.GetType().InvokeMember("SpendTheDocument",
                             BindingFlags.InvokeMethod, null, doc, new object[] { journalSelect.Current.SpendDate });

                        if (obj != null ? (bool)obj : false)
                        {
                            //Документ проведений ОК
                            CreateMessage(TypeMessage.Ok, journalSelect.Current.TypeDocument + " " + journalSelect.Current.SpendDate);
                        }
                        else
                        {
                            //Документ НЕ проведений Error

                            //Вивід помилок в окремому вікні
                            ФункціїДляПовідомлень.ВідкритиТермінал();

                            //Додатково вивід у помилок у це вікно
                            List<Dictionary<string, object>> listRow = ФункціїДляПовідомлень.ПрочитатиПовідомленняПроПомилку();

                            string msg = "";
                            foreach (Dictionary<string, object> row in listRow)
                                msg += row["Повідомлення"].ToString();

                            CreateMessage(TypeMessage.Error, msg);
                            CreateMessage(TypeMessage.Info, "\n\nПроведення документів перервано!\n\n");
                            
                            break;
                        }

                        counter++;
                    }
                }
            }

            if (CancellationTokenPageService != null)
                Program.ListCancellationToken.Remove(CancellationTokenPageService);

            Константи.Системні.ЗупинитиФоновіЗадачі_Const = false;
            ButtonSensitive(true);

            CreateMessage(TypeMessage.None, "Готово!\n\n\n");
            CreateMessage(TypeMessage.Info, "Проведено документів: " + counter);
            Thread.Sleep(1000);
            CreateMessage(TypeMessage.None, "\n\n\n");
        }

        #endregion

        #region Clear DeletionLabel

        void OnClearDeletionLabel(object? sender, EventArgs args)
        {
            ClearMessage();

            Program.ListCancellationToken.Add(CancellationTokenPageService = new CancellationTokenSource());

            Thread thread = new Thread(new ThreadStart(ClearDeletionLabel));
            thread.Start();
        }

        void ClearDeletionLabel()
        {
            ButtonSensitive(false);

            // if (CancellationTokenPageService!.IsCancellationRequested)
            //     break;




            CreateMessage(TypeMessage.None, "Готово!\n\n\n");
            ButtonSensitive(true);

            if (CancellationTokenPageService != null)
                Program.ListCancellationToken.Remove(CancellationTokenPageService);

            Thread.Sleep(1000);
            CreateMessage(TypeMessage.None, "\n\n\n");
        }

        #endregion


        void OnStopClick(object? sender, EventArgs args)
        {
            CancellationTokenPageService?.Cancel();
        }
    }
}