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

namespace StorageAndTrade
{
    class Звіт_РухДокументівПоРегістрах : VBox
    {
        public Звіт_РухДокументівПоРегістрах() : base()
        {
            ShowAll();
        }

        public async void CreateReport(DocumentPointer ДокументВказівник)
        {
            List<string> allowRegisterAccumulation = Config.Kernel.Conf.Documents[ДокументВказівник.TypeDocument].AllowRegisterAccumulation;

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "ДокументВказівник", ДокументВказівник.UnigueID.UGuid }
            };

            foreach (string regAccumName in allowRegisterAccumulation)
            {
                bool exist = false;
                string blockCaption = "";

                Dictionary<string, string> visibleColumn = new Dictionary<string, string>();
                Dictionary<string, string>? dataColumn = null;
                Dictionary<string, string>? typeColumn = null;
                Dictionary<string, float>? textAlignColumn = null;
                Dictionary<string, TreeCellDataFunc>? funcColumn = null;

                SelectRequestAsync_Record? recordResult = null;

                switch (regAccumName)
                {
                    case "ТовариНаСкладах":
                        {
                            exist = true;
                            blockCaption = "Товари на складах";

                            visibleColumn = РухДокументівПоРегістрах.ТовариНаСкладах.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ТовариНаСкладах.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.ТовариНаСкладах.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ТовариНаСкладах.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ТовариНаСкладах.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.ТовариНаСкладах.Запит, paramQuery);
                            break;
                        }
                    case "ПартіїТоварів":
                        {
                            exist = true;
                            blockCaption = "Партії товарів";

                            visibleColumn = РухДокументівПоРегістрах.ПартіїТоварів.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ПартіїТоварів.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.ПартіїТоварів.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ПартіїТоварів.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ПартіїТоварів.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.ПартіїТоварів.Запит, paramQuery);
                            break;
                        }
                    case "ЗамовленняКлієнтів":
                        {
                            exist = true;
                            blockCaption = "Замовлення клієнтів";

                            visibleColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.ЗамовленняКлієнтів.Запит, paramQuery);
                            break;
                        }
                    case "РозрахункиЗКлієнтами":
                        {
                            exist = true;
                            blockCaption = "Розрахунки з клієнтами";

                            visibleColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.РозрахункиЗКлієнтами.Запит, paramQuery);
                            break;
                        }
                    case "ВільніЗалишки":
                        {
                            exist = true;
                            blockCaption = "Вільні залишки";

                            visibleColumn = РухДокументівПоРегістрах.ВільніЗалишки.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ВільніЗалишки.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.ВільніЗалишки.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ВільніЗалишки.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ВільніЗалишки.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.ВільніЗалишки.Запит, paramQuery);
                            break;
                        }
                    case "ЗамовленняПостачальникам":
                        {
                            exist = true;
                            blockCaption = "Замовлення постачальникам";

                            visibleColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.ЗамовленняПостачальникам.Запит, paramQuery);
                            break;
                        }
                    case "РозрахункиЗПостачальниками":
                        {
                            exist = true;
                            blockCaption = "Розрахунки з постачальниками";

                            visibleColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.РозрахункиЗПостачальниками.Запит, paramQuery);
                            break;
                        }
                    case "РухКоштів":
                        {
                            exist = true;
                            blockCaption = "Рух коштів";

                            visibleColumn = РухДокументівПоРегістрах.РухКоштів.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РухКоштів.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.РухКоштів.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.РухКоштів.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.РухКоштів.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.РухКоштів.Запит, paramQuery);
                            break;
                        }
                    case "Закупівлі":
                        {
                            exist = true;
                            blockCaption = "Закупівлі";

                            visibleColumn = РухДокументівПоРегістрах.Закупівлі.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.Закупівлі.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.Закупівлі.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.Закупівлі.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.Закупівлі.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.Закупівлі.Запит, paramQuery);
                            break;
                        }
                    case "Продажі":
                        {
                            exist = true;
                            blockCaption = "Продажі";

                            visibleColumn = РухДокументівПоРегістрах.Продажі.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.Продажі.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.Продажі.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.Продажі.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.Продажі.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.Продажі.Запит, paramQuery);
                            break;
                        }
                    case "ТовариВКомірках":
                        {
                            exist = true;
                            blockCaption = "ТовариВКомірках";

                            visibleColumn = РухДокументівПоРегістрах.ТовариВКомірках.ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ТовариВКомірках.КолонкиДаних();
                            typeColumn = РухДокументівПоРегістрах.ТовариВКомірках.ТипиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ТовариВКомірках.ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ТовариВКомірках.ФункціяДляКолонки();

                            recordResult = await Config.Kernel.DataBase.SelectRequestAsync(РухДокументівПоРегістрах.ТовариВКомірках.Запит, paramQuery);
                            break;
                        }
                    default:
                        {
                            exist = false;
                            break;
                        }
                }

                if (exist && recordResult != null)
                {
                    ListStore listStore;
                    ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, recordResult.ColumnsName);

                    TreeView treeView = new TreeView(listStore);
                    treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

                    ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, recordResult.ColumnsName, visibleColumn, dataColumn, typeColumn, textAlignColumn, funcColumn);
                    ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, recordResult.ColumnsName, recordResult.ListRow);

                    WriteBlock(blockCaption, treeView);
                }
            }
        }

        void WriteBlock(string blockName, TreeView treeView)
        {
            VBox vBox = new VBox();

            Expander expander = new Expander(blockName) { Expanded = true };
            expander.Add(vBox);

            HBox hBox = new HBox();
            vBox.PackStart(hBox, false, false, 10);

            hBox.PackStart(treeView, false, false, 10);

            PackStart(expander, false, false, 10);

            ShowAll();
        }

    }
}