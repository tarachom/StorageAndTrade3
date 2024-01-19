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
 
Повідомлення про помилки

*/

using Gtk;
using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade
{
    class ФункціїДляПовідомлень
    {
        public static async ValueTask ДодатиПовідомленняПроПомилку(DateTime Дата, string НазваПроцесу, Guid? Обєкт, string ТипОбєкту, string НазваОбєкту, string Повідомлення)
        {
            Системні.ПовідомленняТаПомилки_Помилки_TablePart повідомленняТаПомилки_Помилки_TablePart = new();
            Системні.ПовідомленняТаПомилки_Помилки_TablePart.Record record = new()
            {
                Дата = Дата,
                НазваПроцесу = НазваПроцесу,
                Обєкт = Обєкт != null ? (Guid)Обєкт : Guid.Empty,
                ТипОбєкту = ТипОбєкту,
                НазваОбєкту = НазваОбєкту,
                Повідомлення = Повідомлення
            };

            повідомленняТаПомилки_Помилки_TablePart.Records.Add(record);
            await повідомленняТаПомилки_Помилки_TablePart.Save(false);

            //Автоматичне видалення устарівших
            await ОчиститиУстарівшіПовідомлення();
        }

        public static async ValueTask ОчиститиВсіПовідомлення()
        {
            string query = $@"
DELETE FROM {Системні.ПовідомленняТаПомилки_Помилки_TablePart.TABLE}";

            await Config.Kernel.DataBase.ExecuteSQL(query);
        }

        public static async ValueTask ОчиститиУстарівшіПовідомлення()
        {
            string query = $@"
DELETE FROM {Системні.ПовідомленняТаПомилки_Помилки_TablePart.TABLE}
WHERE {Системні.ПовідомленняТаПомилки_Помилки_TablePart.Дата} < @Дата";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>()
            {
                { "Дата", DateTime.Now.AddDays(-7) }
            };

            await Config.Kernel.DataBase.ExecuteSQL(query, paramQuery);
        }

        public static async ValueTask<SelectRequestAsync_Record> ПрочитатиПовідомленняПроПомилки(UnigueID? ВідбірПоОбєкту = null, int? limit = null)
        {
            string query = $@"
SELECT
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.Дата} AS Дата,
    to_char(Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.Дата}, 'HH24:MI:SS') AS Час,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.НазваПроцесу} AS НазваПроцесу,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.Обєкт} AS Обєкт,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.ТипОбєкту} AS ТипОбєкту,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.НазваОбєкту} AS НазваОбєкту,
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.Повідомлення} AS Повідомлення
FROM
    {Системні.ПовідомленняТаПомилки_Помилки_TablePart.TABLE} AS Помилки
";
            if (ВідбірПоОбєкту != null && !ВідбірПоОбєкту.IsEmpty())
                query += $@"
WHERE
    Помилки.{Системні.ПовідомленняТаПомилки_Помилки_TablePart.Обєкт} = '{ВідбірПоОбєкту}'
";
            query += $@"
ORDER BY 
    Дата DESC
LIMIT 
    {limit ?? 100}
";
            return await Config.Kernel.DataBase.SelectRequestAsync(query);
        }

        public static async void ВідкритиПовідомлення()
        {
            СпільніФорми_ВивідПовідомленняПроПомилки page = new СпільніФорми_ВивідПовідомленняПроПомилки();
            Program.GeneralForm?.CreateNotebookPage("Повідомлення", () => { return page; });
            await page.LoadRecords();
        }

        public static async void ПоказатиПовідомлення(UnigueID? ВідбірПоОбєкту = null, int? limit = null)
        {
            СпільніФорми_ВивідПовідомленняПроПомилки_ШвидкийВивід page = new();

            Popover PopoverSmall = new Popover(Program.GeneralForm?.buttonTerminal) { Position = PositionType.Bottom, BorderWidth = 5 };
            PopoverSmall.Add(page);
            PopoverSmall.Show();

            await page.LoadRecords(ВідбірПоОбєкту, limit);
        }
    }
}