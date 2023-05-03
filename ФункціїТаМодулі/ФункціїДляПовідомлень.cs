﻿/*
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
 
Повідомлення про помилки

*/

using Конфа = StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade
{
    class ФункціїДляПовідомлень
    {
        public static void ДодатиПовідомленняПроПомилку(DateTime Дата, string НазваПроцесу, Guid? Обєкт, string ТипОбєкту, string НазваОбєкту, string Повідомлення)
        {
            Системні.ПовідомленняТаПомилки_Помилки_TablePart повідомленняТаПомилки_Помилки_TablePart =
                new Системні.ПовідомленняТаПомилки_Помилки_TablePart();

            Системні.ПовідомленняТаПомилки_Помилки_TablePart.Record record = new Системні.ПовідомленняТаПомилки_Помилки_TablePart.Record();
            повідомленняТаПомилки_Помилки_TablePart.Records.Add(record);

            record.Дата = DateTime.Now;
            record.НазваПроцесу = НазваПроцесу;
            record.Обєкт = (Обєкт != null ? (Guid)Обєкт : Guid.Empty);
            record.ТипОбєкту = ТипОбєкту;
            record.НазваОбєкту = НазваОбєкту;
            record.Повідомлення = Повідомлення;

            повідомленняТаПомилки_Помилки_TablePart.Save(false);
        }

        public static void ОчиститиПовідомлення()
        {
            string query = $@"
DELETE FROM {Системні.ПовідомленняТаПомилки_Помилки_TablePart.TABLE}";

            Конфа.Config.Kernel!.DataBase.ExecuteSQL(query);
        }

        public static List<Dictionary<string, object>> ПрочитатиПовідомленняПроПомилку()
        {
            string query = $@"
SELECT
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.Дата} AS Дата,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.НазваПроцесу} AS НазваПроцесу,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.Обєкт} AS Обєкт,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.ТипОбєкту} AS ТипОбєкту,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.НазваОбєкту} AS НазваОбєкту,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.Повідомлення} AS Повідомлення
FROM
    {Системні.ПовідомленняТаПомилки_Помилки_TablePart.TABLE} AS Помилки
ORDER BY Дата DESC
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Конфа.Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            return listRow;
        }

        public static void ВідкритиТермінал()
        {
            Program.GeneralForm?.CreateNotebookPage("Повідомлення", () =>
            {
                СпільніФорми_ВивідПовідомленняПроПомилки page = new СпільніФорми_ВивідПовідомленняПроПомилки();
                page.LoadRecords();
                return page;
            }, true);
        }
    }
}