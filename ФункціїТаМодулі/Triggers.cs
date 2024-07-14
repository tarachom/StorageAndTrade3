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
 
Модуль функцій зворотнього виклику.

1. Перед записом
2. Після запису
3. Перед видаленням
 
*/

using AccountingSoftware;
using StorageAndTrade;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade_1_0.Довідники
{
    class Організації_Triggers
    {
        public static async ValueTask New(Організації_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Організації_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Організації_Objest ДовідникОбєкт, Організації_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Організації_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Організації_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Організації_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Організації_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class Номенклатура_Triggers
    {
        public static async ValueTask New(Номенклатура_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Номенклатура_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Номенклатура_Objest ДовідникОбєкт, Номенклатура_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Номенклатура_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Номенклатура_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Номенклатура_Objest ДовідникОбєкт, bool label)
        {
            // Помітка на виделення всіх характеристик елементу номенклатури у випадку label = true
            // Якщо мітка знімається, то з характеристик мітка не має зніматися

            if (label == true)
            {
                ХарактеристикиНоменклатури_Select характеристикиНоменклатури_Select = new ХарактеристикиНоменклатури_Select();
                характеристикиНоменклатури_Select.QuerySelect.Where.Add(new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                характеристикиНоменклатури_Select.QuerySelect.Where.Add(new Where(Comparison.AND, ХарактеристикиНоменклатури_Const.DELETION_LABEL, Comparison.NOT, true));
                await характеристикиНоменклатури_Select.Select();

                while (характеристикиНоменклатури_Select.MoveNext())
                {
                    if (характеристикиНоменклатури_Select.Current != null)
                    {
                        ХарактеристикиНоменклатури_Objest? xарактеристикиНоменклатури_Objest = await характеристикиНоменклатури_Select.Current.GetDirectoryObject();
                        if (xарактеристикиНоменклатури_Objest != null)
                            await xарактеристикиНоменклатури_Objest.SetDeletionLabel();
                    }
                }
            }
        }

        public static async ValueTask BeforeDelete(Номенклатура_Objest ДовідникОбєкт)
        {
            //
            //Очистити регістр штрих-кодів
            //

            string query = $@"
DELETE FROM 
	{РегістриВідомостей.ШтрихкодиНоменклатури_Const.TABLE} AS ШтрихкодиНоменклатури
WHERE
    ШтрихкодиНоменклатури.{РегістриВідомостей.ШтрихкодиНоменклатури_Const.Номенклатура} = @Номенклатура
";

            Dictionary<string, object> paramQuery = new()
            {
                { "Номенклатура", ДовідникОбєкт.UnigueID.UGuid }
            };

            await Config.Kernel.DataBase.ExecuteSQL(query, paramQuery);

            //
            //Очистка характеристик
            //

            //...
        }
    }

    class Виробники_Triggers
    {
        public static async ValueTask New(Виробники_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Виробники_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Виробники_Objest ДовідникОбєкт, Виробники_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Виробники_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Виробники_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Виробники_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Виробники_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВидиНоменклатури_Triggers
    {
        public static async ValueTask New(ВидиНоменклатури_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиНоменклатури_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВидиНоменклатури_Objest ДовідникОбєкт, ВидиНоменклатури_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВидиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВидиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВидиНоменклатури_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВидиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПакуванняОдиниціВиміру_Triggers
    {
        public static async ValueTask New(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ПакуванняОдиниціВиміру_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт, ПакуванняОдиниціВиміру_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {
            if (ДовідникОбєкт.КількістьУпаковок <= 0) ДовідникОбєкт.КількістьУпаковок = 1;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class Валюти_Triggers
    {
        public static async ValueTask New(Валюти_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Валюти_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Валюти_Objest ДовідникОбєкт, Валюти_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Валюти_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Валюти_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Валюти_Objest ДовідникОбєкт, bool label)
        {
            if (label)
                await BeforeDelete(ДовідникОбєкт);
        }

        public static async ValueTask BeforeDelete(Валюти_Objest ДовідникОбєкт)
        {
            //Очистити регістр КурсиВалют
            //при видаленні валюти

            string query = $@"
DELETE FROM 
	{РегістриВідомостей.КурсиВалют_Const.TABLE} AS КурсиВалют
WHERE
    КурсиВалют.{РегістриВідомостей.КурсиВалют_Const.Валюта} = @Валюта
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>
            {
                { "Валюта", ДовідникОбєкт.UnigueID.UGuid }
            };

            await Config.Kernel.DataBase.ExecuteSQL(query, paramQuery);
        }
    }

    class Контрагенти_Triggers
    {
        public static async ValueTask New(Контрагенти_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Контрагенти_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Контрагенти_Objest ДовідникОбєкт, Контрагенти_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Контрагенти_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Контрагенти_Objest ДовідникОбєкт)
        {
            await ФункціїДляДовідників.СтворитиДоговориКонтрагентаЗаЗамовчуванням(ДовідникОбєкт.GetDirectoryPointer());
        }

        public static async ValueTask SetDeletionLabel(Контрагенти_Objest ДовідникОбєкт, bool label)
        {
            // Помітка на виделення всіх договорів
            if (label)
            {
                ДоговориКонтрагентів_Select договориКонтрагентів_Select = new ДоговориКонтрагентів_Select();
                договориКонтрагентів_Select.QuerySelect.Where.Add(new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                договориКонтрагентів_Select.QuerySelect.Where.Add(new Where(Comparison.AND, ДоговориКонтрагентів_Const.DELETION_LABEL, Comparison.NOT, true));
                await договориКонтрагентів_Select.Select();

                while (договориКонтрагентів_Select.MoveNext())
                    if (договориКонтрагентів_Select.Current != null)
                    {
                        ДоговориКонтрагентів_Objest? договориКонтрагентів_Objest = await договориКонтрагентів_Select.Current.GetDirectoryObject();
                        if (договориКонтрагентів_Objest != null)
                            await договориКонтрагентів_Objest.SetDeletionLabel();
                    }
            }
        }

        public static async ValueTask BeforeDelete(Контрагенти_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class Склади_Triggers
    {
        public static async ValueTask New(Склади_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Склади_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Склади_Objest ДовідникОбєкт, Склади_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Склади_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Склади_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Склади_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Склади_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВидиЦін_Triggers
    {
        public static async ValueTask New(ВидиЦін_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиЦін_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВидиЦін_Objest ДовідникОбєкт, ВидиЦін_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВидиЦін_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВидиЦін_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВидиЦін_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВидиЦін_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВидиЦінПостачальників_Triggers
    {
        public static async ValueTask New(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиЦінПостачальників_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВидиЦінПостачальників_Objest ДовідникОбєкт, ВидиЦінПостачальників_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВидиЦінПостачальників_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class Користувачі_Triggers
    {
        public static async ValueTask New(Користувачі_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Користувачі_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Користувачі_Objest ДовідникОбєкт, Користувачі_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Користувачі_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Користувачі_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Користувачі_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Користувачі_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ФізичніОсоби_Triggers
    {
        public static async ValueTask New(ФізичніОсоби_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ФізичніОсоби_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ФізичніОсоби_Objest ДовідникОбєкт, ФізичніОсоби_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ФізичніОсоби_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ФізичніОсоби_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ФізичніОсоби_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ФізичніОсоби_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class СтруктураПідприємства_Triggers
    {
        public static async ValueTask New(СтруктураПідприємства_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СтруктураПідприємства_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СтруктураПідприємства_Objest ДовідникОбєкт, СтруктураПідприємства_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СтруктураПідприємства_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СтруктураПідприємства_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СтруктураПідприємства_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СтруктураПідприємства_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class КраїниСвіту_Triggers
    {
        public static async ValueTask New(КраїниСвіту_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.КраїниСвіту_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(КраїниСвіту_Objest ДовідникОбєкт, КраїниСвіту_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(КраїниСвіту_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(КраїниСвіту_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(КраїниСвіту_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(КраїниСвіту_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class Файли_Triggers
    {
        public static async ValueTask New(Файли_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Файли_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Файли_Objest ДовідникОбєкт, Файли_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Файли_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Файли_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Файли_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Файли_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ХарактеристикиНоменклатури_Triggers
    {
        public static async ValueTask New(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ХарактеристикиНоменклатури_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ХарактеристикиНоменклатури_Objest ДовідникОбєкт, ХарактеристикиНоменклатури_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ХарактеристикиНоменклатури_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class Номенклатура_Папки_Triggers
    {
        public static async ValueTask New(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Номенклатура_Папки_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Номенклатура_Папки_Objest ДовідникОбєкт, Номенклатура_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Номенклатура_Папки_Objest ДовідникОбєкт, bool label)
        {
            //Якщо встановлюється мітка на видалення
            if (label == true)
            {
                //
                //Елементи помічаються на видалення
                //

                Номенклатура_Select номенклатура_Select = new Номенклатура_Select();
                номенклатура_Select.QuerySelect.Where.Add(new Where(Номенклатура_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                номенклатура_Select.QuerySelect.Where.Add(new Where(Comparison.AND, Номенклатура_Const.DELETION_LABEL, Comparison.NOT, true));
                await номенклатура_Select.Select();

                while (номенклатура_Select.MoveNext())
                    if (номенклатура_Select.Current != null)
                    {
                        Номенклатура_Objest? номенклатура_Objest = await номенклатура_Select.Current.GetDirectoryObject();
                        if (номенклатура_Objest != null)
                            await номенклатура_Objest.SetDeletionLabel();
                    }

                //
                //Вкладені папки помічаються на видалення
                //

                Номенклатура_Папки_Select номенклатура_Папки_Select = new Номенклатура_Папки_Select();
                номенклатура_Папки_Select.QuerySelect.Where.Add(new Where(Номенклатура_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await номенклатура_Папки_Select.Select();

                while (номенклатура_Папки_Select.MoveNext())
                    if (номенклатура_Папки_Select.Current != null)
                    {
                        Номенклатура_Папки_Objest? номенклатура_Папки_Objest = await номенклатура_Папки_Select.Current.GetDirectoryObject();
                        if (номенклатура_Папки_Objest != null)
                            await номенклатура_Папки_Objest.SetDeletionLabel();

                    }
            }
        }

        public static async ValueTask BeforeDelete(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            //
            //Елементи переносяться на верхній рівень
            //

            Номенклатура_Select номенклатура_Select = new Номенклатура_Select();
            номенклатура_Select.QuerySelect.Where.Add(new Where(Номенклатура_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            await номенклатура_Select.Select();

            while (номенклатура_Select.MoveNext())
                if (номенклатура_Select.Current != null)
                {
                    Номенклатура_Objest? номенклатура_Objest = await номенклатура_Select.Current.GetDirectoryObject();
                    if (номенклатура_Objest != null)
                    {
                        //Ставиться помітка на видалення для елементу
                        await номенклатура_Objest.SetDeletionLabel();

                        номенклатура_Objest.Папка = new Номенклатура_Папки_Pointer();
                        await номенклатура_Objest.Save();
                    }
                }

            //
            //Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
            //

            Номенклатура_Папки_Select номенклатура_Папки_Select = new Номенклатура_Папки_Select();
            номенклатура_Папки_Select.QuerySelect.Where.Add(new Where(Номенклатура_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            await номенклатура_Папки_Select.Select();

            while (номенклатура_Папки_Select.MoveNext())
                if (номенклатура_Папки_Select.Current != null)
                {
                    Номенклатура_Папки_Objest? номенклатура_Папки_Objest = await номенклатура_Папки_Select.Current.GetDirectoryObject();
                    if (номенклатура_Папки_Objest != null)
                        await номенклатура_Папки_Objest.Delete();
                }
        }
    }

    class Контрагенти_Папки_Triggers
    {
        public static async ValueTask New(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Контрагенти_Папки_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Контрагенти_Папки_Objest ДовідникОбєкт, Контрагенти_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Контрагенти_Папки_Objest ДовідникОбєкт, bool label)
        {
            //Якщо встановлюється мітка на видалення
            if (label == true)
            {
                //
                //Елементи помічаються на видалення
                //

                Контрагенти_Select контрагенти_Select = new Контрагенти_Select();
                контрагенти_Select.QuerySelect.Where.Add(new Where(Контрагенти_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                контрагенти_Select.QuerySelect.Where.Add(new Where(Comparison.AND, Контрагенти_Const.DELETION_LABEL, Comparison.NOT, true));
                await контрагенти_Select.Select();

                while (контрагенти_Select.MoveNext())
                    if (контрагенти_Select.Current != null)
                    {
                        Контрагенти_Objest? контрагенти_Objest = await контрагенти_Select.Current.GetDirectoryObject();
                        if (контрагенти_Objest != null)
                            await контрагенти_Objest.SetDeletionLabel();
                    }

                //
                //Вкладені папки помічаються на видалення
                //

                Контрагенти_Папки_Select контрагенти_Папки_Select = new Контрагенти_Папки_Select();
                контрагенти_Папки_Select.QuerySelect.Where.Add(new Where(Контрагенти_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await контрагенти_Папки_Select.Select();

                while (контрагенти_Папки_Select.MoveNext())
                    if (контрагенти_Папки_Select.Current != null)
                    {
                        Контрагенти_Папки_Objest? контрагенти_Папки_Objest = await контрагенти_Папки_Select.Current.GetDirectoryObject();
                        if (контрагенти_Папки_Objest != null)
                            await контрагенти_Папки_Objest.SetDeletionLabel();

                    }
            }
        }

        public static async ValueTask BeforeDelete(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            //
            //Елементи переносяться на верхній рівень
            //

            Контрагенти_Select контрагенти_Select = new Контрагенти_Select();
            контрагенти_Select.QuerySelect.Where.Add(new Where(Контрагенти_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            await контрагенти_Select.Select();

            while (контрагенти_Select.MoveNext())
                if (контрагенти_Select.Current != null)
                {
                    Контрагенти_Objest? контрагенти_Objest = await контрагенти_Select.Current.GetDirectoryObject();

                    if (контрагенти_Objest != null)
                    {
                        //Ставиться помітка на видалення для елементу
                        await контрагенти_Objest.SetDeletionLabel();

                        //Зміна папки
                        контрагенти_Objest.Папка = new Контрагенти_Папки_Pointer();
                        await контрагенти_Objest.Save();
                    }
                }

            //
            //Вкладені папки видаляються. Для кожної папки буде викликана функція BeforeDelete
            //

            Контрагенти_Папки_Select контрагенти_Папки_Select = new Контрагенти_Папки_Select();
            контрагенти_Папки_Select.QuerySelect.Where.Add(new Where(Контрагенти_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            await контрагенти_Папки_Select.Select();

            while (контрагенти_Папки_Select.MoveNext())
            {
                if (контрагенти_Папки_Select.Current != null)
                {
                    Контрагенти_Папки_Objest? контрагенти_Папки_Objest = await контрагенти_Папки_Select.Current.GetDirectoryObject();
                    if (контрагенти_Папки_Objest != null)
                        await контрагенти_Папки_Objest.Delete();
                }
            }
        }
    }

    class Склади_Папки_Triggers
    {
        public static async ValueTask New(Склади_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Склади_Папки_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Склади_Папки_Objest ДовідникОбєкт, Склади_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Склади_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Склади_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Склади_Папки_Objest ДовідникОбєкт, bool label)
        {
            //Якщо встановлюється мітка на видалення
            if (label == true)
            {
                //
                //Елементи помічаються на видалення
                //

                Склади_Select склади_Select = new Склади_Select();
                склади_Select.QuerySelect.Where.Add(new Where(Склади_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                склади_Select.QuerySelect.Where.Add(new Where(Comparison.AND, Склади_Const.DELETION_LABEL, Comparison.NOT, true));
                await склади_Select.Select();

                while (склади_Select.MoveNext())
                    if (склади_Select.Current != null)
                    {
                        Склади_Objest? склади_Objest = await склади_Select.Current.GetDirectoryObject();
                        if (склади_Objest != null)
                            await склади_Objest.SetDeletionLabel();
                    }

                //
                //Вкладені папки помічаються на видалення
                //

                Склади_Папки_Select cклади_Папки_Select = new Склади_Папки_Select();
                cклади_Папки_Select.QuerySelect.Where.Add(new Where(Склади_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await cклади_Папки_Select.Select();

                while (cклади_Папки_Select.MoveNext())
                    if (cклади_Папки_Select.Current != null)
                    {
                        Склади_Папки_Objest? cклади_Папки_Objest = await cклади_Папки_Select.Current.GetDirectoryObject();
                        if (cклади_Папки_Objest != null)
                            await cклади_Папки_Objest.SetDeletionLabel();
                    }
            }
        }

        public static async ValueTask BeforeDelete(Склади_Папки_Objest ДовідникОбєкт)
        {
            //
            //Елементи переносяться на верхній рівень
            //

            Склади_Select склади_Select = new Склади_Select();
            склади_Select.QuerySelect.Where.Add(new Where(Склади_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            await склади_Select.Select();

            while (склади_Select.MoveNext())
                if (склади_Select.Current != null)
                {
                    Склади_Objest? склади_Objest = await склади_Select.Current.GetDirectoryObject();

                    if (склади_Objest != null)
                    {
                        //Ставиться помітка на видалення для елементу
                        await склади_Objest.SetDeletionLabel();

                        склади_Objest.Папка = new Склади_Папки_Pointer();
                        await склади_Objest.Save();
                    }
                }

            //
            //Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
            //

            Склади_Папки_Select cклади_Папки_Select = new Склади_Папки_Select();
            cклади_Папки_Select.QuerySelect.Where.Add(new Where(Склади_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            await cклади_Папки_Select.Select();

            while (cклади_Папки_Select.MoveNext())
                if (cклади_Папки_Select.Current != null)
                {
                    Склади_Папки_Objest? cклади_Папки_Objest = await cклади_Папки_Select.Current.GetDirectoryObject();
                    if (cклади_Папки_Objest != null)
                        await cклади_Папки_Objest.Delete();
                }
        }
    }

    class Каси_Triggers
    {
        public static async ValueTask New(Каси_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Каси_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Каси_Objest ДовідникОбєкт, Каси_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Каси_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Каси_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Каси_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Каси_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class БанківськіРахункиОрганізацій_Triggers
    {
        public static async ValueTask New(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.БанківськіРахункиОрганізацій_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт, БанківськіРахункиОрганізацій_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ДоговориКонтрагентів_Triggers
    {
        public static async ValueTask New(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ДоговориКонтрагентів_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ДоговориКонтрагентів_Objest ДовідникОбєкт, ДоговориКонтрагентів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            string НазваПереліченняЗКонфігурації =
                Config.Kernel.Conf.Enums["ТипДоговорів"].Fields[ДовідникОбєкт.ТипДоговору.ToString()].Desc;

            ДовідникОбєкт.ТипДоговоруПредставлення = НазваПереліченняЗКонфігурації;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ДоговориКонтрагентів_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class БанківськіРахункиКонтрагентів_Triggers
    {
        public static async ValueTask New(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.БанківськіРахункиКонтрагентів_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт, БанківськіРахункиКонтрагентів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class СтаттяРухуКоштів_Triggers
    {
        public static async ValueTask New(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СтаттяРухуКоштів_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СтаттяРухуКоштів_Objest ДовідникОбєкт, СтаттяРухуКоштів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СтаттяРухуКоштів_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class СеріїНоменклатури_Triggers
    {
        public static async ValueTask New(СеріїНоменклатури_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.ДатаСтворення = DateTime.Now;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СеріїНоменклатури_Objest ДовідникОбєкт, СеріїНоменклатури_Objest Основа)
        {
            ДовідникОбєкт.Номер = Guid.NewGuid().ToString();
            ДовідникОбєкт.Коментар = "Копія - " + Основа.Номер;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СеріїНоменклатури_Objest ДовідникОбєкт)
        {
            СеріїНоменклатури_Select серіїНоменклатури_Select = new СеріїНоменклатури_Select();
            серіїНоменклатури_Select.QuerySelect.Where.Add(new Where(СеріїНоменклатури_Const.Номер, Comparison.EQ, ДовідникОбєкт.Номер));
            серіїНоменклатури_Select.QuerySelect.Where.Add(new Where(Comparison.AND, "uid", Comparison.NOT, ДовідникОбєкт.UnigueID.UGuid));

            if (await серіїНоменклатури_Select.SelectSingle())
            {
                ДовідникОбєкт.Коментар = $"Помилка: Серійний номер [ {ДовідникОбєкт.Номер} ] вже існує в базі даних. " + ДовідникОбєкт.Коментар;
                ДовідникОбєкт.Номер = Guid.NewGuid().ToString();
            }
        }

        public static async ValueTask AfterSave(СеріїНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СеріїНоменклатури_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СеріїНоменклатури_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПартіяТоварівКомпозит_Triggers
    {
        public static async ValueTask New(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПартіяТоварівКомпозит_Objest ДовідникОбєкт, ПартіяТоварівКомпозит_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПартіяТоварівКомпозит_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВидиЗапасів_Triggers
    {
        public static async ValueTask New(ВидиЗапасів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиЗапасів_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВидиЗапасів_Objest ДовідникОбєкт, ВидиЗапасів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВидиЗапасів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВидиЗапасів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВидиЗапасів_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВидиЗапасів_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class Банки_Triggers
    {
        public static async ValueTask New(Банки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Банки_Const).ToString("D6");

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Банки_Objest ДовідникОбєкт, Банки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Банки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Банки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Банки_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Банки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class СкладськіПриміщення_Triggers
    {
        public static async ValueTask New(СкладськіПриміщення_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СкладськіПриміщення_Objest ДовідникОбєкт, СкладськіПриміщення_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СкладськіПриміщення_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СкладськіПриміщення_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СкладськіПриміщення_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СкладськіПриміщення_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class СкладськіКомірки_Triggers
    {
        public static async ValueTask New(СкладськіКомірки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СкладськіКомірки_Objest ДовідникОбєкт, СкладськіКомірки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СкладськіКомірки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СкладськіКомірки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СкладськіКомірки_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(СкладськіКомірки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ТипорозміриКомірок_Triggers
    {
        public static async ValueTask New(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ТипорозміриКомірок_Objest ДовідникОбєкт, ТипорозміриКомірок_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ТипорозміриКомірок_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class СкладськіКомірки_Папки_Triggers
    {
        public static async ValueTask New(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СкладськіКомірки_Папки_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СкладськіКомірки_Папки_Objest ДовідникОбєкт, СкладськіКомірки_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СкладськіКомірки_Папки_Objest ДовідникОбєкт, bool label)
        {
            //Якщо встановлюється мітка на видалення
            if (label == true)
            {
                //
                //Елементи помічаються на видалення
                //

                СкладськіКомірки_Select cкладськіКомірки_Select = new СкладськіКомірки_Select();
                cкладськіКомірки_Select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                cкладськіКомірки_Select.QuerySelect.Where.Add(new Where(Comparison.AND, СкладськіКомірки_Const.DELETION_LABEL, Comparison.NOT, true));
                await cкладськіКомірки_Select.Select();

                while (cкладськіКомірки_Select.MoveNext())
                    if (cкладськіКомірки_Select.Current != null)
                    {
                        СкладськіКомірки_Objest? cкладськіКомірки_Objest = await cкладськіКомірки_Select.Current.GetDirectoryObject();
                        if (cкладськіКомірки_Objest != null)
                            await cкладськіКомірки_Objest.SetDeletionLabel();
                    }

                //
                //Вкладені папки помічаються на видалення
                //

                СкладськіКомірки_Папки_Select cкладськіКомірки_Папки_Select = new СкладськіКомірки_Папки_Select();
                cкладськіКомірки_Папки_Select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await cкладськіКомірки_Папки_Select.Select();

                while (cкладськіКомірки_Папки_Select.MoveNext())
                    if (cкладськіКомірки_Папки_Select.Current != null)
                    {
                        СкладськіКомірки_Папки_Objest? cкладськіКомірки_Папки_Objest = await cкладськіКомірки_Папки_Select.Current.GetDirectoryObject();
                        if (cкладськіКомірки_Папки_Objest != null)
                            await cкладськіКомірки_Папки_Objest.SetDeletionLabel();
                    }
            }
        }

        public static async ValueTask BeforeDelete(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            //
            //Елементи помічаються на видалення
            //

            СкладськіКомірки_Select cкладськіКомірки_Select = new СкладськіКомірки_Select();
            cкладськіКомірки_Select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            cкладськіКомірки_Select.QuerySelect.Where.Add(new Where(Comparison.AND, СкладськіКомірки_Const.DELETION_LABEL, Comparison.NOT, true));
            await cкладськіКомірки_Select.Select();

            while (cкладськіКомірки_Select.MoveNext())
                if (cкладськіКомірки_Select.Current != null)
                {
                    СкладськіКомірки_Objest? cкладськіКомірки_Objest = await cкладськіКомірки_Select.Current.GetDirectoryObject();
                    if (cкладськіКомірки_Objest != null)
                    {
                        await cкладськіКомірки_Objest.SetDeletionLabel();

                        cкладськіКомірки_Objest.Папка = new СкладськіКомірки_Папки_Pointer();
                        await cкладськіКомірки_Objest.Save();
                    }
                }

            //
            //Вкладені папки помічаються на видалення
            //

            СкладськіКомірки_Папки_Select cкладськіКомірки_Папки_Select = new СкладськіКомірки_Папки_Select();
            cкладськіКомірки_Папки_Select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            await cкладськіКомірки_Папки_Select.Select();

            while (cкладськіКомірки_Папки_Select.MoveNext())
                if (cкладськіКомірки_Папки_Select.Current != null)
                {
                    СкладськіКомірки_Папки_Objest? cкладськіКомірки_Папки_Objest = await cкладськіКомірки_Папки_Select.Current.GetDirectoryObject();
                    if (cкладськіКомірки_Папки_Objest != null)
                        await cкладськіКомірки_Папки_Objest.Delete();
                }
        }
    }

    class Блокнот_Triggers
    {
        public static async ValueTask New(Блокнот_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Блокнот_Const).ToString("D6");
            ДовідникОбєкт.ДатаЗапису = DateTime.Now;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(Блокнот_Objest ДовідникОбєкт, Блокнот_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(Блокнот_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(Блокнот_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(Блокнот_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(Блокнот_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

}

namespace StorageAndTrade_1_0.Документи
{
    class ЗамовленняПостачальнику_Triggers
    {
        public static async ValueTask New(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗамовленняПостачальнику_Objest ДокументОбєкт, ЗамовленняПостачальнику_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗамовленняПостачальнику_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗамовленняПостачальнику_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПоступленняТоварівТаПослуг_Triggers
    {
        public static async ValueTask New(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт, ПоступленняТоварівТаПослуг_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПоступленняТоварівТаПослуг_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт, bool label)
        {
            // Помітка на видалення всіх партій
            if (label == true)
            {
                ПартіяТоварівКомпозит_Select партіяТоварівКомпозит_Select = new ПартіяТоварівКомпозит_Select();
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.ПоступленняТоварівТаПослуг, Comparison.EQ, ДокументОбєкт.UnigueID.UGuid));
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(Comparison.AND, ПартіяТоварівКомпозит_Const.DELETION_LABEL, Comparison.NOT, true));
                await партіяТоварівКомпозит_Select.Select();

                while (партіяТоварівКомпозит_Select.MoveNext())
                    if (партіяТоварівКомпозит_Select.Current != null)
                    {
                        ПартіяТоварівКомпозит_Objest? партіяТоварівКомпозит_Objest = await партіяТоварівКомпозит_Select.Current.GetDirectoryObject();
                        if (партіяТоварівКомпозит_Objest != null)
                            await партіяТоварівКомпозит_Objest.SetDeletionLabel();
                    }
            }
        }

        public static async ValueTask BeforeDelete(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ЗамовленняКлієнта_Triggers
    {
        public static async ValueTask New(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗамовленняКлієнта_Objest ДокументОбєкт, ЗамовленняКлієнта_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗамовленняКлієнта_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗамовленняКлієнта_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РеалізаціяТоварівТаПослуг_Triggers
    {
        public static async ValueTask New(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт, РеалізаціяТоварівТаПослуг_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РеалізаціяТоварівТаПослуг_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВстановленняЦінНоменклатури_Triggers
    {
        public static async ValueTask New(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВстановленняЦінНоменклатури_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВстановленняЦінНоменклатури_Objest ДокументОбєкт, ВстановленняЦінНоменклатури_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ВстановленняЦінНоменклатури_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВстановленняЦінНоменклатури_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПрихіднийКасовийОрдер_Triggers
    {
        public static async ValueTask New(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПрихіднийКасовийОрдер_Objest ДокументОбєкт, ПрихіднийКасовийОрдер_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПрихіднийКасовийОрдер_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПрихіднийКасовийОрдер_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РозхіднийКасовийОрдер_Triggers
    {
        public static async ValueTask New(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РозхіднийКасовийОрдер_Objest ДокументОбєкт, РозхіднийКасовийОрдер_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РозхіднийКасовийОрдер_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РозхіднийКасовийОрдер_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПереміщенняТоварів_Triggers
    {
        public static async ValueTask New(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПереміщенняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПереміщенняТоварів_Objest ДокументОбєкт, ПереміщенняТоварів_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПереміщенняТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПереміщенняТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПоверненняТоварівПостачальнику_Triggers
    {
        public static async ValueTask New(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоверненняТоварівПостачальнику_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт, ПоверненняТоварівПостачальнику_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПоверненняТоварівПостачальнику_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПоверненняТоварівВідКлієнта_Triggers
    {
        public static async ValueTask New(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоверненняТоварівВідКлієнта_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт, ПоверненняТоварівВідКлієнта_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class АктВиконанихРобіт_Triggers
    {
        public static async ValueTask New(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.АктВиконанихРобіт_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(АктВиконанихРобіт_Objest ДокументОбєкт, АктВиконанихРобіт_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{АктВиконанихРобіт_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(АктВиконанихРобіт_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВведенняЗалишків_Triggers
    {
        public static async ValueTask New(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВведенняЗалишків_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВведенняЗалишків_Objest ДокументОбєкт, ВведенняЗалишків_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ВведенняЗалишків_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВведенняЗалишків_Objest ДокументОбєкт, bool label)
        {
            // Помітка на виделення всіх партій
            if (label == true)
            {
                ПартіяТоварівКомпозит_Select партіяТоварівКомпозит_Select = new ПартіяТоварівКомпозит_Select();
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.ВведенняЗалишків, Comparison.EQ, ДокументОбєкт.UnigueID.UGuid));
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(Comparison.AND, ПартіяТоварівКомпозит_Const.DELETION_LABEL, Comparison.NOT, true));
                await партіяТоварівКомпозит_Select.Select();

                while (партіяТоварівКомпозит_Select.MoveNext())
                    if (партіяТоварівКомпозит_Select.Current != null)
                    {
                        ПартіяТоварівКомпозит_Objest? партіяТоварівКомпозит_Objest = await партіяТоварівКомпозит_Select.Current.GetDirectoryObject();
                        if (партіяТоварівКомпозит_Objest != null)
                            await партіяТоварівКомпозит_Objest.SetDeletionLabel();
                    }
            }
        }

        public static async ValueTask BeforeDelete(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПерерахунокТоварів_Triggers
    {
        public static async ValueTask New(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПерерахунокТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            //Відповідального можна отримати з Користувача
            var Користувач_Обєкт = await Program.Користувач.GetDirectoryObject();
            if (Користувач_Обєкт != null)
                ДокументОбєкт.Відповідальний = Користувач_Обєкт.ФізичнаОсоба;
        }

        public static async ValueTask Copying(ПерерахунокТоварів_Objest ДокументОбєкт, ПерерахунокТоварів_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПерерахунокТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПерерахунокТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПсуванняТоварів_Triggers
    {
        public static async ValueTask New(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПсуванняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПсуванняТоварів_Objest ДокументОбєкт, ПсуванняТоварів_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПсуванняТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПсуванняТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВнутрішнєСпоживанняТоварів_Triggers
    {
        public static async ValueTask New(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВнутрішнєСпоживанняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт, ВнутрішнєСпоживанняТоварів_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РахунокФактура_Triggers
    {
        public static async ValueTask New(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РахунокФактура_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РахунокФактура_Objest ДокументОбєкт, РахунокФактура_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РахунокФактура_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РахунокФактура_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РахунокФактура_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РахунокФактура_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РозміщенняТоварівНаСкладі_Triggers
    {
        public static async ValueTask New(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозміщенняТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт, РозміщенняТоварівНаСкладі_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РозміщенняТоварівНаСкладі_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПереміщенняТоварівНаСкладі_Triggers
    {
        public static async ValueTask New(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПереміщенняТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт, ПереміщенняТоварівНаСкладі_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПереміщенняТоварівНаСкладі_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ЗбіркаТоварівНаСкладі_Triggers
    {
        public static async ValueTask New(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗбіркаТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт, ЗбіркаТоварівНаСкладі_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗбіркаТоварівНаСкладі_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РозміщенняНоменклатуриПоКоміркам_Triggers
    {
        public static async ValueTask New(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозміщенняНоменклатуриПоКоміркам_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт, РозміщенняНоменклатуриПоКоміркам_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class КорегуванняБоргу_Triggers
    {
        public static async ValueTask New(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.КорегуванняБоргу_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(КорегуванняБоргу_Objest ДокументОбєкт, КорегуванняБоргу_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{КорегуванняБоргу_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(КорегуванняБоргу_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

}