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
using ТабличніСписки = StorageAndTrade_1_0.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade
{
    public class СпільніФорми_РухДокументуПоРегістрах : InterfaceGtk.СпільніФорми_РухДокументуПоРегістрах
    {
        public static async void СформуватиЗвіт(DocumentPointer ДокументВказівник)
        {
            СпільніФорми_РухДокументуПоРегістрах page = new();
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Проводки", () => page);
            await page.Заповнити(ДокументВказівник);
        }

        public async ValueTask Заповнити(DocumentPointer ДокументВказівник)
        {
            foreach (string regAccumName in Config.Kernel.Conf.Documents[ДокументВказівник.TypeDocument].AllowRegisterAccumulation)
            {
                TreeView treeView = new TreeView();

                switch (regAccumName)
                {
                    case "ТовариНаСкладах":
                        {
                            ДодатиБлокНаФорму("Товари на складах", treeView);

                            ТабличніСписки.ТовариНаСкладах_Записи.AddColumns(treeView, ["period", "owner"]);
                            ТабличніСписки.ТовариНаСкладах_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.ТовариНаСкладах_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "ПартіїТоварів":
                        {
                            ДодатиБлокНаФорму("Партії товарів", treeView);

                            ТабличніСписки.ПартіїТоварів_Записи.AddColumns(treeView, ["period", "owner"]);
                            ТабличніСписки.ПартіїТоварів_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.ПартіїТоварів_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "ЗамовленняКлієнтів":
                        {
                            ДодатиБлокНаФорму("Замовлення клієнтів", treeView);

                            ТабличніСписки.ЗамовленняКлієнтів_Записи.AddColumns(treeView, ["period", "owner"]);
                            ТабличніСписки.ЗамовленняКлієнтів_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.ЗамовленняКлієнтів_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "РозрахункиЗКлієнтами":
                        {
                            ДодатиБлокНаФорму("Розрахунки з клієнтами", treeView);

                            ТабличніСписки.РозрахункиЗКлієнтами_Записи.AddColumns(treeView, ["period", "owner"]);
                            ТабличніСписки.РозрахункиЗКлієнтами_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.РозрахункиЗКлієнтами_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "ВільніЗалишки":
                        {
                            ДодатиБлокНаФорму("Вільні залишки", treeView);

                            ТабличніСписки.ВільніЗалишки_Записи.AddColumns(treeView, ["period", "owner"]);
                            ТабличніСписки.ВільніЗалишки_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.ВільніЗалишки_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "ЗамовленняПостачальникам":
                        {
                            ДодатиБлокНаФорму("Замовлення постачальникам", treeView);

                            ТабличніСписки.ЗамовленняПостачальникам_Записи.AddColumns(treeView, ["period", "owner"]);
                            ТабличніСписки.ЗамовленняПостачальникам_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.ЗамовленняПостачальникам_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "РозрахункиЗПостачальниками":
                        {
                            ДодатиБлокНаФорму("Розрахунки з постачальниками", treeView);

                            ТабличніСписки.РозрахункиЗПостачальниками_Записи.AddColumns(treeView, ["period", "owner"]);
                            ТабличніСписки.РозрахункиЗПостачальниками_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.РозрахункиЗПостачальниками_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "РухКоштів":
                        {
                            ДодатиБлокНаФорму("Рух коштів", treeView);

                            ТабличніСписки.РухКоштів_Записи.AddColumns(treeView, ["period", "owner"]);
                            ТабличніСписки.РухКоштів_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.РухКоштів_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "Закупівлі":
                        {
                            ДодатиБлокНаФорму("Закупівлі", treeView);

                            ТабличніСписки.Закупівлі_Записи.AddColumns(treeView, ["period", "owner", "income"]);
                            ТабличніСписки.Закупівлі_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.Закупівлі_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "Продажі":
                        {
                            ДодатиБлокНаФорму("Продажі", treeView);

                            ТабличніСписки.Продажі_Записи.AddColumns(treeView, ["period", "owner", "income"]);
                            ТабличніСписки.Продажі_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.Продажі_Записи.LoadRecords(treeView, false);

                            break;
                        }
                    case "ТовариВКомірках":
                        {
                            ДодатиБлокНаФорму("Товари в комірках", treeView);

                            ТабличніСписки.ТовариВКомірках_Записи.AddColumns(treeView, ["period", "owner"]);
                            ТабличніСписки.ТовариВКомірках_Записи.ДодатиВідбірПоДокументу(treeView, ДокументВказівник.UnigueID.UGuid);
                            await ТабличніСписки.ТовариВКомірках_Записи.LoadRecords(treeView, false);

                            break;
                        }
                }
            }
        }
    }
}