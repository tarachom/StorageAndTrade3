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
using StorageAndTrade_1_0;
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
            ФункціїДляПовідомлень.ОчиститиПовідомлення();

            ButtonSensitive(false);

            int counterDocs = 0;
            Константи.Системні.ЗупинитиФоновіЗадачі_Const = true;

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

                        counterDocs++;
                    }
                }
            }

            if (CancellationTokenPageService != null)
                Program.ListCancellationToken.Remove(CancellationTokenPageService);

            Константи.Системні.ЗупинитиФоновіЗадачі_Const = false;
            ButtonSensitive(true);

            CreateMessage(TypeMessage.None, "Готово!\n\n\n");
            CreateMessage(TypeMessage.Info, "Проведено документів: " + counterDocs);
            Thread.Sleep(1000);
            CreateMessage(TypeMessage.None, "\n\n\n");
        }

        #endregion

        #region Clear DeletionLabel

        void OnClearDeletionLabel(object? sender, EventArgs args)
        {
            /*
            try
            {
                object? doc = Assembly.GetExecutingAssembly().CreateInstance("StorageAndTrade_1_0.Довідники.Номенклатура_Objest");

                if (doc != null)
                {
                    object? o = doc.GetType().InvokeMember("Read", BindingFlags.InvokeMethod, null, doc, new object[] { new UnigueID("e01d5d93-dc8e-453b-866c-b53084854eaa") });
                    if (o != null ? (bool)o : false)
                    {
                        doc.GetType().InvokeMember("SetDeletionLabel", BindingFlags.InvokeMethod, null, doc, new object[] { false });
                    }
                }
                else
                    Console.WriteLine("No create Instance");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            */

            ClearMessage();

            Program.ListCancellationToken.Add(CancellationTokenPageService = new CancellationTokenSource());

            Thread thread = new Thread(new ThreadStart(ClearDeletionLabel));
            thread.Start();
        }

        void ClearDeletionLabel()
        {
            ButtonSensitive(false);

            if (Config.Kernel != null)
            {
                CreateMessage(TypeMessage.Info, "Обробка довідників:");

                foreach (ConfigurationDirectories configurationDirectories in Config.Kernel!.Conf.Directories.Values)
                {
                    if (CancellationTokenPageService!.IsCancellationRequested)
                        break;

                    CreateMessage(TypeMessage.Info, " -> " + configurationDirectories.Name);

                    //Вибірка помічених на видалення
                    string query = @$"SELECT uid FROM {configurationDirectories.Table} WHERE deletion_label = true";

                    string[] columnsName;
                    List<Dictionary<string, object>> listRow;

                    Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

                    if (listRow.Count > 0)
                    {
                        //Пошук залежностей
                        List<string> listTableAndField = Config.Kernel.Conf.SearchForPointers(
                            "Довідники." + configurationDirectories.Name, Configuration.VariantWorkSearchForPointers.Tables);

                        //Обробка довідників
                        foreach (Dictionary<string, object> row in listRow)
                        {
                            long allCount = 0;

                            Guid uid = (Guid)row["uid"];

                            Console.WriteLine(uid);

                            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
                            paramQuery.Add("uid", uid);

                            //Обробка залежностей
                            foreach (string tableAndField in listTableAndField)
                            {
                                string[] splitTableAndField = tableAndField.Split(".");
                                string table = splitTableAndField[0];
                                string field = splitTableAndField[1];

                                string queryFind = $"SELECT count(uid) FROM {table} WHERE {field} = @uid";
                                object? objcount = Config.Kernel.DataBase.ExecuteSQLScalar(queryFind, paramQuery);

                                if (objcount != null)
                                    allCount += (long)objcount;
                            }

                            if (allCount == 0)
                            {
                                object? directoryObject = Assembly.GetExecutingAssembly().CreateInstance($"StorageAndTrade_1_0.Довідники.{configurationDirectories.Name}_Objest");
                                if (directoryObject != null)
                                {
                                    object? objRead = directoryObject.GetType().InvokeMember("Read", BindingFlags.InvokeMethod, null, directoryObject, new object[] { new UnigueID(uid) });
                                    if (objRead != null ? (bool)objRead : false)
                                    {

                                        directoryObject.GetType().InvokeMember("Delete", BindingFlags.InvokeMethod, null, directoryObject, null);
                                        Console.WriteLine("Видалено: " + uid);
                                    }
                                }
                            }
                        }
                    }
                }

                CreateMessage(TypeMessage.Info, "Обробка документів:");

                foreach (ConfigurationDocuments configurationDocuments in Config.Kernel!.Conf.Documents.Values)
                {
                    if (CancellationTokenPageService!.IsCancellationRequested)
                        break;

                    CreateMessage(TypeMessage.Info, " -> " + configurationDocuments.Name);

                    //Вибірка помічених на видалення
                    string query = @$"
SELECT 
    uid, docname 
FROM 
    {configurationDocuments.Table} 
WHERE 
    deletion_label = true";

                    string[] columnsName;
                    List<Dictionary<string, object>> listRow;

                    Config.Kernel.DataBase.SelectRequest(query, null, out columnsName, out listRow);

                    if (listRow.Count > 0)
                    {
                        //Пошук залежностей
                        List<string> listTableAndField = Config.Kernel.Conf.SearchForPointers(
                            "Документи." + configurationDocuments.Name, Configuration.VariantWorkSearchForPointers.Tables);

                        //Обробка документів
                        foreach (Dictionary<string, object> row in listRow)
                        {
                            long allCount = 0;

                            Guid uid = (Guid)row["uid"];
                            string name = (string)row["docname"];

                            Console.WriteLine(name);

                            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
                            paramQuery.Add("uid", uid);

                            //Обробка залежностей
                            foreach (string tableAndField in listTableAndField)
                            {
                                string[] splitTableAndField = tableAndField.Split(".");
                                string table = splitTableAndField[0];
                                string field = splitTableAndField[1];

                                string queryFind = $"SELECT count(uid) FROM {table} WHERE {field} = @uid";
                                object? objcount = Config.Kernel.DataBase.ExecuteSQLScalar(queryFind, paramQuery);

                                if (objcount != null)
                                {
                                    allCount += (long)objcount;
                                    Console.WriteLine($"Знайдено [{tableAndField}]: " + (long)objcount);
                                }
                            }

                            Console.WriteLine("Загально: " + allCount);

                            if (allCount == 0)
                            {
                                string queryFind = $"DELETE FROM {configurationDocuments.Table} WHERE uid = @uid";
                                Config.Kernel.DataBase.ExecuteSQL(queryFind, paramQuery);

                                Console.WriteLine("Видалено: " + name);
                            }
                        }
                    }
                }
            }

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