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
        Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
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
            bStop.Clicked += (object? sender, EventArgs args) => { CancellationTokenPageService?.Cancel(); };
            hBoxBotton.PackStart(bStop, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            scrollMessage = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollMessage.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollMessage.Add(vBoxMessage);

            PackStart(scrollMessage, true, true, 0);

            ShowAll();
        }

        #region SpendTheDocument

        async void OnSpendTheDocument(object? sender, EventArgs args)
        {
            CancellationTokenPageService = new CancellationTokenSource();
            await SpendTheDocument();
        }

        async ValueTask SpendTheDocument()
        {
            ButtonSensitive(false);

            ClearMessage();

            int counterDocs = 0;
            Константи.Системні.ЗупинитиФоновіЗадачі_Const = true;

            Журнали.JournalSelect journalSelect = new Журнали.JournalSelect();

            // Вибірка всіх документів. Встановлюється максимальний період
            await journalSelect.Select(DateTime.Parse("01.01.2000 00:00:00"), DateTime.Now);
            while (journalSelect.MoveNext())
            {
                if (CancellationTokenPageService!.IsCancellationRequested)
                    break;

                //Обробляються тільки не помічені на видалення і проведені
                if (journalSelect.Current != null && !journalSelect.Current.DeletionLabel && journalSelect.Current.Spend)
                {
                    DocumentObject? doc = await journalSelect.GetDocumentObject(true);
                    if (doc != null)
                    {
                        //Для документу викликається функція проведення
                        object? obj = doc.GetType().InvokeMember("SpendTheDocumentSync", BindingFlags.InvokeMethod, null, doc, [journalSelect.Current.SpendDate]);

                        if (obj != null ? (bool)obj : false)
                        {
                            //Документ проведений ОК
                            CreateMessage(TypeMessage.Ok, journalSelect.Current.TypeDocument + " " + journalSelect.Current.SpendDate);
                        }
                        else
                        {
                            //Документ НЕ проведений Error
                            //
                            //Вивід помилок в окремому вікні
                            ФункціїДляПовідомлень.ПоказатиПовідомлення(doc.UnigueID, 1);

                            //Додатково вивід у помилок у це вікно
                            SelectRequestAsync_Record record = await ФункціїДляПовідомлень.ПрочитатиПовідомленняПроПомилки(doc.UnigueID, 1);

                            string msg = "";
                            foreach (Dictionary<string, object> row in record.ListRow)
                                msg += row["Повідомлення"].ToString();

                            CreateMessage(TypeMessage.Error, msg);
                            CreateMessage(TypeMessage.Info, "\n\nПроведення документів перервано!\n\n");

                            break;
                        }

                        counterDocs++;
                    }
                }
            }

            Константи.Системні.ЗупинитиФоновіЗадачі_Const = false;

            CreateMessage(TypeMessage.None, "Готово!\n\n\n");
            CreateMessage(TypeMessage.Info, "Проведено документів: " + counterDocs);

            await Task.Delay(1000);
            CreateMessage(TypeMessage.None, "\n\n\n\n");

            ButtonSensitive(true);
        }

        #endregion

        #region Clear DeletionLabel

        async void OnClearDeletionLabel(object? sender, EventArgs args)
        {
            CancellationTokenPageService = new CancellationTokenSource();
            await ClearDeletionLabel();
        }

        async ValueTask ClearDeletionLabel()
        {
            ButtonSensitive(false);
            ClearMessage();

            CreateMessage(TypeMessage.Info, "Обробка довідників:");

            foreach (ConfigurationDirectories configurationDirectories in Config.Kernel.Conf.Directories.Values)
            {
                if (CancellationTokenPageService!.IsCancellationRequested)
                    break;

                CreateMessage(TypeMessage.Info, " -> " + configurationDirectories.Name);

                //Вибірка помічених на видалення
                string query = @$"SELECT uid FROM {configurationDirectories.Table} WHERE deletion_label = true";

                var recordResult = await Config.Kernel.DataBase.SelectRequestAsync(query);

                if (recordResult.ListRow.Count > 0)
                {
                    //Пошук залежностей
                    List<ConfigurationDependencies> listDependencies = Config.Kernel.Conf.SearchDependencies("Довідники." + configurationDirectories.Name);

                    string directoryObjestName = $"StorageAndTrade_1_0.Довідники.{configurationDirectories.Name}_Objest";

                    //Обробка довідників
                    foreach (Dictionary<string, object> row in recordResult.ListRow)
                    {
                        UnigueID unigueID = new UnigueID(row["uid"]);
                        string name = "";

                        //Обєкт довідника
                        object? directoryObject = ExecutingAssembly.CreateInstance(directoryObjestName);
                        if (directoryObject != null)
                        {
                            object? objRead = directoryObject.GetType().InvokeMember("ReadSync", BindingFlags.InvokeMethod, null, directoryObject, new object[] { unigueID });
                            if (objRead != null ? (bool)objRead : false)
                            {
                                object? objName = directoryObject.GetType().InvokeMember("GetPresentationSync", BindingFlags.InvokeMethod, null, directoryObject, null);
                                if (objName != null)
                                    name = (string)objName;

                                long allCountDependencies = await SearchDependencies(listDependencies, unigueID.UGuid, name);
                                if (allCountDependencies == 0)
                                {
                                    directoryObject.GetType().InvokeMember("DeleteSync", BindingFlags.InvokeMethod, null, directoryObject, null);
                                    CreateMessage(TypeMessage.Ok, " --> Видалено: " + name + " [" + unigueID.ToString() + "]");
                                }
                            }
                        }
                    }

                }
            }

            CreateMessage(TypeMessage.None, "\n");
            CreateMessage(TypeMessage.Info, "Обробка документів:");

            foreach (ConfigurationDocuments configurationDocuments in Config.Kernel.Conf.Documents.Values)
            {
                if (CancellationTokenPageService!.IsCancellationRequested)
                    break;

                CreateMessage(TypeMessage.Info, " -> " + configurationDocuments.Name);

                //Вибірка помічених на видалення
                string query = @$"SELECT uid, docname FROM {configurationDocuments.Table} WHERE deletion_label = true";

                var recordResult = await Config.Kernel.DataBase.SelectRequestAsync(query);

                if (recordResult.ListRow.Count > 0)
                {
                    //Пошук залежностей
                    List<ConfigurationDependencies> listDependencies = Config.Kernel.Conf.SearchDependencies("Документи." + configurationDocuments.Name);

                    string DocumentObjestName = $"StorageAndTrade_1_0.Документи.{configurationDocuments.Name}_Objest";

                    //Обробка документів
                    foreach (Dictionary<string, object> row in recordResult.ListRow)
                    {
                        UnigueID unigueID = new UnigueID(row["uid"]);
                        string name = (string)row["docname"];

                        object? documentObject = ExecutingAssembly.CreateInstance(DocumentObjestName);
                        if (documentObject != null)
                        {
                            object? objRead = documentObject.GetType().InvokeMember("ReadSync", BindingFlags.InvokeMethod, null, documentObject, [unigueID, false]);
                            if (objRead != null ? (bool)objRead : false)
                            {
                                long allCountDependencies = await SearchDependencies(listDependencies, unigueID.UGuid, name);
                                if (allCountDependencies == 0)
                                {
                                    documentObject.GetType().InvokeMember("DeleteSync", BindingFlags.InvokeMethod, null, documentObject, null);
                                    CreateMessage(TypeMessage.Ok, " --> Видалено: " + name + " [" + unigueID.ToString() + "]");
                                }
                            }
                        }
                    }

                }
            }

            CreateMessage(TypeMessage.None, "\n\n\nГотово!\n\n\n");

            await Task.Delay(1000);
            CreateMessage(TypeMessage.None, "\n\n\n\n");

            ButtonSensitive(true);
        }

        async ValueTask<long> SearchDependencies(List<ConfigurationDependencies> listDependencies, Guid uid, string name)
        {
            long allCountDependencies = 0;

            if (listDependencies.Count > 0)
            {
                Dictionary<string, object> paramQuery = new Dictionary<string, object>
                {
                    { "uid", uid }
                };

                //Обробка залежностей
                foreach (ConfigurationDependencies dependence in listDependencies)
                {
                    string query = "";

                    if (dependence.ConfigurationGroupLevel == ConfigurationDependencies.GroupLevel.Object)
                        query = $"SELECT uid FROM {dependence.Table} WHERE {dependence.Field} = @uid LIMIT 5";
                    else if (dependence.ConfigurationGroupLevel == ConfigurationDependencies.GroupLevel.TablePart)
                        query = $"SELECT DISTINCT owner AS uid FROM {dependence.Table} WHERE {dependence.Field} = @uid LIMIT 5";

                    var recordResult = await Config.Kernel.DataBase.SelectRequestAsync(query, paramQuery);

                    if (recordResult.ListRow.Count > 0)
                    {
                        allCountDependencies += recordResult.ListRow.Count;

                        CreateMessage(TypeMessage.Error, name);
                        CreateMessage(TypeMessage.None, "використовується --> " + dependence.ConfigurationGroupName +
                            ", \"" + dependence.ConfigurationObjectName + "\" " +
                            (dependence.ConfigurationGroupLevel == ConfigurationDependencies.GroupLevel.TablePart ?
                                ", таблична частина \"" + dependence.ConfigurationTablePartName + "\" " : "") +
                            ", поле \"" + dependence.ConfigurationFieldName + "\"");

                        foreach (Dictionary<string, object> row in recordResult.ListRow)
                        {
                            UnigueID unigueID = new UnigueID(row["uid"]);

                            if (dependence.ConfigurationGroupName == "Довідники" || dependence.ConfigurationGroupName == "Документи")
                            {
                                string documentPointerName = $"StorageAndTrade_1_0.{dependence.ConfigurationGroupName}.{dependence.ConfigurationObjectName}_Pointer";
                                object? documentPointer = ExecutingAssembly.CreateInstance(documentPointerName, false, BindingFlags.CreateInstance, null, [unigueID, null!], null, null);
                                if (documentPointer != null)
                                {
                                    object? objPresentation = documentPointer.GetType().InvokeMember("GetPresentationSync", BindingFlags.InvokeMethod, null, documentPointer, null);
                                    if (objPresentation != null)
                                        CreateMessage(TypeMessage.None, (string)objPresentation + $" [{unigueID}]");
                                }
                            }
                        } //foreach
                    } //if
                } //foreach
            } //if

            return allCountDependencies;
        }

        #endregion

        void ButtonSensitive(bool sensitive)
        {
            bSpendTheDocument.Sensitive = sensitive;
            bClearDeletionLabel.Sensitive = sensitive;
            bStop.Sensitive = !sensitive;
        }

        void CreateMessage(TypeMessage typeMsg, string message)
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

            hBoxInfo.PackStart(new Label(message) { Wrap = true, Selectable = true }, false, false, 0);
            hBoxInfo.ShowAll();

            scrollMessage.Vadjustment.Value = scrollMessage.Vadjustment.Upper;
        }

        void ClearMessage()
        {
            foreach (Widget Child in vBoxMessage.Children)
                vBoxMessage.Remove(Child);
        }
    }
}