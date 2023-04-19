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

        public void CreateReport(DocumentPointer ДокументВказівник)
        {
            List<string> allowRegisterAccumulation = Config.Kernel!.Conf.Documents[ДокументВказівник.TypeDocument].AllowRegisterAccumulation;

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ДокументВказівник", ДокументВказівник.UnigueID.UGuid);

            foreach (string regAccumName in allowRegisterAccumulation)
            {
                bool exist = false;
                string blockCaption = "";

                string[] columnsName = new string[] { };
                List<Dictionary<string, object>> listRow = new List<Dictionary<string, object>>();

                Dictionary<string, string> visibleColumn = new Dictionary<string, string>();
                Dictionary<string, string>? dataColumn = null;
                Dictionary<string, float>? textAlignColumn = null;
                Dictionary<string, TreeCellDataFunc>? funcColumn = null;

                switch (regAccumName)
                {
                    case "ТовариНаСкладах":
                        {
                            exist = true;
                            blockCaption = "Товари на складах";

                            visibleColumn = РухДокументівПоРегістрах.ТовариНаСкладах_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ТовариНаСкладах_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ТовариНаСкладах_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ТовариНаСкладах_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ТовариНаСкладах_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ПартіїТоварів":
                        {
                            exist = true;
                            blockCaption = "Партії товарів";

                            visibleColumn = РухДокументівПоРегістрах.ПартіїТоварів_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ПартіїТоварів_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ПартіїТоварів_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ПартіїТоварів_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ПартіїТоварів_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ЗамовленняКлієнтів":
                        {
                            exist = true;
                            blockCaption = "Замовлення клієнтів";

                            visibleColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ЗамовленняКлієнтів_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "РозрахункиЗКлієнтами":
                        {
                            exist = true;
                            blockCaption = "Розрахунки з клієнтами";

                            visibleColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.РозрахункиЗКлієнтами_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ВільніЗалишки":
                        {
                            exist = true;
                            blockCaption = "Вільні залишки";

                            visibleColumn = РухДокументівПоРегістрах.ВільніЗалишки_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ВільніЗалишки_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ВільніЗалишки_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ВільніЗалишки_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ВільніЗалишки_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ЗамовленняПостачальникам":
                        {
                            exist = true;
                            blockCaption = "Замовлення постачальникам";

                            visibleColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ЗамовленняПостачальникам_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "РозрахункиЗПостачальниками":
                        {
                            exist = true;
                            blockCaption = "Розрахунки з постачальниками";

                            visibleColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.РозрахункиЗПостачальниками_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "РухКоштів":
                        {
                            exist = true;
                            blockCaption = "Рух коштів";

                            visibleColumn = РухДокументівПоРегістрах.РухКоштів_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РухКоштів_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.РухКоштів_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.РухКоштів_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.РухКоштів_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "Закупівлі":
                        {
                            exist = true;
                            blockCaption = "Закупівлі";

                            visibleColumn = РухДокументівПоРегістрах.Закупівлі_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.Закупівлі_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.Закупівлі_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.Закупівлі_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.Закупівлі_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "Продажі":
                        {
                            exist = true;
                            blockCaption = "Продажі";

                            visibleColumn = РухДокументівПоРегістрах.Продажі_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.Продажі_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.Продажі_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.Продажі_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.Продажі_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ТовариВКомірках":
                        {
                            exist = true;
                            blockCaption = "ТовариВКомірках";

                            visibleColumn = РухДокументівПоРегістрах.ТовариВКомірках_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ТовариВКомірках_КолонкиДаних();
                            textAlignColumn = РухДокументівПоРегістрах.ТовариВКомірках_ПозиціяТекстуВКолонці();
                            funcColumn = РухДокументівПоРегістрах.ТовариВКомірках_ФункціяДляКолонки();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ТовариВКомірках_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    default:
                        {
                            exist = false;
                            break;
                        }
                }

                if (exist)
                {
                    ListStore listStore;
                    ФункціїДляЗвітів.СтворитиМодельДаних(out listStore, columnsName);

                    TreeView treeView = new TreeView(listStore);
                    treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

                    ФункціїДляЗвітів.СтворитиКолонкиДляДерева(treeView, columnsName, visibleColumn, dataColumn, textAlignColumn, funcColumn);
                    ФункціїДляЗвітів.ЗаповнитиМодельДаними(listStore, columnsName, listRow);

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