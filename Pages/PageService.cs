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

using System.Reflection;

using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0;
using Журнали = StorageAndTrade_1_0.Журнали;
using Gtk;

namespace StorageAndTrade
{
    class PageService : InterfaceGtk.PageService
    {
        public PageService() : base(Config.Kernel, Config.NameSpageProgram, Config.NameSpageCodeGeneration) { }

        #region ПроведенняДокументів

        protected override async ValueTask SpendTheDocument(CancellationTokenSource cancellationToken, System.Action CallBack)
        {
            int counterDocs = 0;
            DateTime dateTimeCurrDoc = DateTime.MinValue.Date;

            Лог.CreateMessage($"Період з <b>{Період.DateStartControl.ПочатокДня()}</b> по <b>{Період.DateStopControl.КінецьДня()}</b>", LogMessage.TypeMessage.Info);
            Журнали.JournalSelect journalSelect = new Журнали.JournalSelect();

            Box hBoxFindDoc = Лог.CreateMessage($"Пошук проведених документів:", LogMessage.TypeMessage.Info, true);
            await journalSelect.Select(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня(), null, true);

            Лог.AppendMessage(hBoxFindDoc, $"знайдено {journalSelect.Count()} документів");
            while (journalSelect.MoveNext())
                if (journalSelect.Current != null)
                {
                    if (cancellationToken!.IsCancellationRequested)
                        break;

                    //Список документів які ігноруються при обрахунку регістрів накопичення
                    if (dateTimeCurrDoc != journalSelect.Current.DocDate.Date)
                    {
                        dateTimeCurrDoc = journalSelect.Current.DocDate.Date;
                        await ОчиститиСписокІгноруванняДокументів();
                        Лог.CreateMessage($"{dateTimeCurrDoc.ToString("dd-MM-yyyy")}", LogMessage.TypeMessage.None);
                    }

                    await ДодатиДокументВСписокІгнорування(journalSelect.Current.UnigueID.UGuid, journalSelect.Current.DocName);

                    DocumentObject? doc = await journalSelect.GetDocumentObject(true);
                    if (doc != null)
                    {
                        Box hBox = Лог.CreateMessage($"Проведення <b>{journalSelect.Current.DocName}</b>", LogMessage.TypeMessage.Info);

                        //Для документу викликається функція проведення
                        object? obj = doc.GetType().InvokeMember("SpendTheDocumentSync", BindingFlags.InvokeMethod, null, doc, [journalSelect.Current.SpendDate]);
                        if (obj != null)
                            if ((bool)obj)
                            {
                                //Документ проведений ОК
                                Лог.AppendMessage(hBox, "Проведено");

                                counterDocs++;
                            }
                            else
                            {
                                //Документ НЕ проведений Error
                                Лог.AppendMessage(hBox, "Помилка", LogMessage.TypeMessage.Error);

                                //Додатково вивід помилок у це вікно
                                SelectRequest_Record record = await new ФункціїДляПовідомлень().ПрочитатиПовідомленняПроПомилки(doc.UnigueID, 1);

                                string msg = "";
                                foreach (Dictionary<string, object> row in record.ListRow)
                                    msg += "<i>" + row["message"].ToString() + "</i>";

                                Лог.CreateMessage(msg, LogMessage.TypeMessage.None, true);
                                Лог.CreateWidget(CreateCompositControl("Документи:", journalSelect.Current.GetBasis()), LogMessage.TypeMessage.None, true);
                                Лог.CreateMessage("Проведення документів перервано!", LogMessage.TypeMessage.Info, true);

                                break;
                            }
                    }
                }

            await ОчиститиСписокІгноруванняДокументів();

            CallBack.Invoke();
            
            Лог.CreateEmptyMsg();
            Лог.CreateMessage($"Обробку завершено!", LogMessage.TypeMessage.None, true);
            Лог.CreateMessage($"Проведено документів: {counterDocs}", LogMessage.TypeMessage.Info, true);

            await Task.Delay(1000);
            Лог.CreateEmptyMsg();
        }

        #endregion

        protected override Widget CreateCompositControl(string caption, UuidAndText uuidAndText)
        {
            return new CompositePointerControl()
            {
                Caption = caption,
                ClearSensetive = false,
                TypeSelectSensetive = false,
                Pointer = uuidAndText
            };
        }

        const string КлючНалаштуванняКористувача = "PageService";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
        }
    }
}