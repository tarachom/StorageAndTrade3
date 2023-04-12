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
using Конфа = StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade_1_0.Довідники
{
    class Організації_Triggers
    {
        public static void New(Організації_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Організації_Const).ToString("D6");
        }

        public static void Copying(Організації_Objest ДовідникОбєкт, Організації_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Організації_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Організації_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Організації_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(Організації_Objest ДовідникОбєкт)
        {

        }
    }

    class Номенклатура_Triggers
    {
        public static void New(Номенклатура_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Номенклатура_Const).ToString("D6");
        }

        public static void Copying(Номенклатура_Objest ДовідникОбєкт, Номенклатура_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Номенклатура_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Номенклатура_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Номенклатура_Objest ДовідникОбєкт, bool label)
        {
            // Помітка на виделення всіх характеристик елементу номенклатури у випадку label = true
            // Якщо мітка знімається, то з характеристик мітка не має зніматися

            if (label == true)
            {
                ХарактеристикиНоменклатури_Select характеристикиНоменклатури_Select = new ХарактеристикиНоменклатури_Select();
                характеристикиНоменклатури_Select.QuerySelect.Where.Add(new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                характеристикиНоменклатури_Select.QuerySelect.Where.Add(new Where(Comparison.AND, ХарактеристикиНоменклатури_Const.DELETION_LABEL, Comparison.NOT, true));
                характеристикиНоменклатури_Select.Select();

                while (характеристикиНоменклатури_Select.MoveNext())
                {
                    ХарактеристикиНоменклатури_Objest? xарактеристикиНоменклатури_Objest = характеристикиНоменклатури_Select.Current?.GetDirectoryObject();
                    if (xарактеристикиНоменклатури_Objest != null)
                        xарактеристикиНоменклатури_Objest.SetDeletionLabel();
                }
            }
        }

        public static void BeforeDelete(Номенклатура_Objest ДовідникОбєкт)
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

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("Номенклатура", ДовідникОбєкт.UnigueID.UGuid);

            Конфа.Config.Kernel!.DataBase.ExecuteSQL(query, paramQuery);

            //
            //Очистка характеристик
            //


        }
    }

    class Виробники_Triggers
    {
        public static void New(Виробники_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Виробники_Const).ToString("D6");
        }

        public static void Copying(Виробники_Objest ДовідникОбєкт, Виробники_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Виробники_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Виробники_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Виробники_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(Виробники_Objest ДовідникОбєкт)
        {

        }
    }

    class ВидиНоменклатури_Triggers
    {
        public static void New(ВидиНоменклатури_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиНоменклатури_Const).ToString("D6");
        }

        public static void Copying(ВидиНоменклатури_Objest ДовідникОбєкт, ВидиНоменклатури_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(ВидиНоменклатури_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ВидиНоменклатури_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ВидиНоменклатури_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ВидиНоменклатури_Objest ДовідникОбєкт)
        {

        }
    }

    class ПакуванняОдиниціВиміру_Triggers
    {
        public static void New(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ПакуванняОдиниціВиміру_Const).ToString("D6");
        }

        public static void Copying(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт, ПакуванняОдиниціВиміру_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {

        }
    }

    class Валюти_Triggers
    {
        public static void New(Валюти_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Валюти_Const).ToString("D6");
        }

        public static void Copying(Валюти_Objest ДовідникОбєкт, Валюти_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Валюти_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Валюти_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Валюти_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(Валюти_Objest ДовідникОбєкт)
        {
            //Очистити регістр КурсиВалют
            //при видаленні валюти

            string query = $@"
DELETE FROM 
	{РегістриВідомостей.КурсиВалют_Const.TABLE} AS КурсиВалют
WHERE
    КурсиВалют.{РегістриВідомостей.КурсиВалют_Const.Валюта} = @Валюта
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("Валюта", ДовідникОбєкт.UnigueID.UGuid);

            Конфа.Config.Kernel!.DataBase.ExecuteSQL(query, paramQuery);
        }
    }

    class Контрагенти_Triggers
    {
        public static void New(Контрагенти_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Контрагенти_Const).ToString("D6");
        }

        public static void Copying(Контрагенти_Objest ДовідникОбєкт, Контрагенти_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Контрагенти_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Контрагенти_Objest ДовідникОбєкт)
        {
            ФункціїДляДовідників.СтворитиДоговориКонтрагентаЗаЗамовчуванням(ДовідникОбєкт.GetDirectoryPointer());
        }

        public static void SetDeletionLabel(Контрагенти_Objest ДовідникОбєкт, bool label)
        {
            // Помітка на виделення всіх договорів
            if (label == true)
            {
                ДоговориКонтрагентів_Select договориКонтрагентів_Select = new ДоговориКонтрагентів_Select();
                договориКонтрагентів_Select.QuerySelect.Where.Add(new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                договориКонтрагентів_Select.QuerySelect.Where.Add(new Where(Comparison.AND, ДоговориКонтрагентів_Const.DELETION_LABEL, Comparison.NOT, true));
                договориКонтрагентів_Select.Select();

                while (договориКонтрагентів_Select.MoveNext())
                {
                    ДоговориКонтрагентів_Objest? договориКонтрагентів_Objest = договориКонтрагентів_Select.Current?.GetDirectoryObject();
                    if (договориКонтрагентів_Objest != null)
                        договориКонтрагентів_Objest.SetDeletionLabel();
                }
            }
        }

        public static void BeforeDelete(Контрагенти_Objest ДовідникОбєкт)
        {
            ДоговориКонтрагентів_Select договориКонтрагентів_Select = new ДоговориКонтрагентів_Select();
            договориКонтрагентів_Select.QuerySelect.Where.Add(new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            договориКонтрагентів_Select.Select();

            while (договориКонтрагентів_Select.MoveNext())
            {
                ДоговориКонтрагентів_Objest? договориКонтрагентів_Objest = договориКонтрагентів_Select.Current?.GetDirectoryObject();
                if (договориКонтрагентів_Objest != null)
                    договориКонтрагентів_Objest.Delete();
            }
        }
    }

    class Склади_Triggers
    {
        public static void New(Склади_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Склади_Const).ToString("D6");
        }

        public static void Copying(Склади_Objest ДовідникОбєкт, Склади_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Склади_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Склади_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Склади_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(Склади_Objest ДовідникОбєкт)
        {

        }
    }

    class ВидиЦін_Triggers
    {
        public static void New(ВидиЦін_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиЦін_Const).ToString("D6");
        }

        public static void Copying(ВидиЦін_Objest ДовідникОбєкт, ВидиЦін_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(ВидиЦін_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ВидиЦін_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ВидиЦін_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ВидиЦін_Objest ДовідникОбєкт)
        {

        }
    }

    class ВидиЦінПостачальників_Triggers
    {
        public static void New(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиЦінПостачальників_Const).ToString("D6");
        }

        public static void Copying(ВидиЦінПостачальників_Objest ДовідникОбєкт, ВидиЦінПостачальників_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ВидиЦінПостачальників_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {

        }
    }

    class Користувачі_Triggers
    {
        public static void New(Користувачі_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Користувачі_Const).ToString("D6");
        }

        public static void Copying(Користувачі_Objest ДовідникОбєкт, Користувачі_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Користувачі_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Користувачі_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Користувачі_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(Користувачі_Objest ДовідникОбєкт)
        {

        }
    }

    class ФізичніОсоби_Triggers
    {
        public static void New(ФізичніОсоби_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ФізичніОсоби_Const).ToString("D6");
        }

        public static void Copying(ФізичніОсоби_Objest ДовідникОбєкт, ФізичніОсоби_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(ФізичніОсоби_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ФізичніОсоби_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ФізичніОсоби_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ФізичніОсоби_Objest ДовідникОбєкт)
        {

        }
    }

    class СтруктураПідприємства_Triggers
    {
        public static void New(СтруктураПідприємства_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СтруктураПідприємства_Const).ToString("D6");
        }

        public static void Copying(СтруктураПідприємства_Objest ДовідникОбєкт, СтруктураПідприємства_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(СтруктураПідприємства_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СтруктураПідприємства_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(СтруктураПідприємства_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(СтруктураПідприємства_Objest ДовідникОбєкт)
        {

        }
    }

    class КраїниСвіту_Triggers
    {
        public static void New(КраїниСвіту_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.КраїниСвіту_Const).ToString("D6");
        }

        public static void Copying(КраїниСвіту_Objest ДовідникОбєкт, КраїниСвіту_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(КраїниСвіту_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(КраїниСвіту_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(КраїниСвіту_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(КраїниСвіту_Objest ДовідникОбєкт)
        {

        }
    }

    class Файли_Triggers
    {
        public static void New(Файли_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Файли_Const).ToString("D6");
        }

        public static void Copying(Файли_Objest ДовідникОбєкт, Файли_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Файли_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Файли_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Файли_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(Файли_Objest ДовідникОбєкт)
        {

        }
    }

    class ХарактеристикиНоменклатури_Triggers
    {
        public static void New(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ХарактеристикиНоменклатури_Const).ToString("D6");
        }

        public static void Copying(ХарактеристикиНоменклатури_Objest ДовідникОбєкт, ХарактеристикиНоменклатури_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ХарактеристикиНоменклатури_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {

        }
    }

    class Номенклатура_Папки_Triggers
    {
        public static void New(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Номенклатура_Папки_Const).ToString("D6");
        }

        public static void Copying(Номенклатура_Папки_Objest ДовідникОбєкт, Номенклатура_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Номенклатура_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Номенклатура_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Номенклатура_Папки_Objest ДовідникОбєкт, bool label)
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
                номенклатура_Select.Select();

                while (номенклатура_Select.MoveNext())
                {
                    Номенклатура_Objest? номенклатура_Objest = номенклатура_Select.Current?.GetDirectoryObject();
                    if (номенклатура_Objest != null)
                        номенклатура_Objest.SetDeletionLabel();
                }

                //
                //Вкладені папки помічаються на видалення
                //

                Номенклатура_Папки_Select номенклатура_Папки_Select = new Номенклатура_Папки_Select();
                номенклатура_Папки_Select.QuerySelect.Where.Add(new Where(Номенклатура_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                номенклатура_Папки_Select.Select();

                while (номенклатура_Папки_Select.MoveNext())
                {
                    Номенклатура_Папки_Objest? номенклатура_Папки_Objest = номенклатура_Папки_Select.Current?.GetDirectoryObject();
                    if (номенклатура_Папки_Objest != null)
                        номенклатура_Папки_Objest.SetDeletionLabel();

                }
            }
        }

        public static void BeforeDelete(Номенклатура_Папки_Objest ДовідникОбєкт)
        {
            //
            //Елементи переносяться на верхній рівень
            //

            Номенклатура_Select номенклатура_Select = new Номенклатура_Select();
            номенклатура_Select.QuerySelect.Where.Add(new Where(Номенклатура_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            номенклатура_Select.Select();

            while (номенклатура_Select.MoveNext())
            {
                Номенклатура_Objest? номенклатура_Objest = номенклатура_Select.Current?.GetDirectoryObject();
                if (номенклатура_Objest != null)
                {
                    //Ставиться помітка на видалення для елементу
                    номенклатура_Objest.SetDeletionLabel();

                    номенклатура_Objest.Папка = new Номенклатура_Папки_Pointer();
                    номенклатура_Objest.Save();
                }
            }

            //
            //Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
            //

            Номенклатура_Папки_Select номенклатура_Папки_Select = new Номенклатура_Папки_Select();
            номенклатура_Папки_Select.QuerySelect.Where.Add(new Where(Номенклатура_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            номенклатура_Папки_Select.Select();

            while (номенклатура_Папки_Select.MoveNext())
            {
                Номенклатура_Папки_Objest? номенклатура_Папки_Objest = номенклатура_Папки_Select.Current?.GetDirectoryObject();
                if (номенклатура_Папки_Objest != null)
                    номенклатура_Папки_Objest.Delete();

            }
        }
    }

    class Контрагенти_Папки_Triggers
    {
        public static void New(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Контрагенти_Папки_Const).ToString("D6");
        }

        public static void Copying(Контрагенти_Папки_Objest ДовідникОбєкт, Контрагенти_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Контрагенти_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Контрагенти_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Контрагенти_Папки_Objest ДовідникОбєкт, bool label)
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
                контрагенти_Select.Select();

                while (контрагенти_Select.MoveNext())
                {
                    Контрагенти_Objest? контрагенти_Objest = контрагенти_Select.Current?.GetDirectoryObject();
                    if (контрагенти_Objest != null)
                        контрагенти_Objest.SetDeletionLabel();
                }

                //
                //Вкладені папки помічаються на видалення
                //

                Контрагенти_Папки_Select контрагенти_Папки_Select = new Контрагенти_Папки_Select();
                контрагенти_Папки_Select.QuerySelect.Where.Add(new Where(Контрагенти_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                контрагенти_Папки_Select.Select();

                while (контрагенти_Папки_Select.MoveNext())
                {
                    Контрагенти_Папки_Objest? контрагенти_Папки_Objest = контрагенти_Папки_Select.Current?.GetDirectoryObject();
                    if (контрагенти_Папки_Objest != null)
                        контрагенти_Папки_Objest.SetDeletionLabel();

                }
            }
        }

        public static void BeforeDelete(Контрагенти_Папки_Objest ДовідникОбєкт)
        {
            //
            //Елементи переносяться на верхній рівень
            //

            Контрагенти_Select контрагенти_Select = new Контрагенти_Select();
            контрагенти_Select.QuerySelect.Where.Add(new Where(Контрагенти_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            контрагенти_Select.Select();

            while (контрагенти_Select.MoveNext())
            {
                Контрагенти_Objest? контрагенти_Objest = контрагенти_Select.Current?.GetDirectoryObject();

                if (контрагенти_Objest != null)
                {
                    //Ставиться помітка на видалення для елементу
                    контрагенти_Objest.SetDeletionLabel();

                    //Зміна папки
                    контрагенти_Objest.Папка = new Контрагенти_Папки_Pointer();
                    контрагенти_Objest.Save();
                }
            }

            //
            //Вкладені папки видаляються. Для кожної папки буде викликана функція BeforeDelete
            //

            Контрагенти_Папки_Select контрагенти_Папки_Select = new Контрагенти_Папки_Select();
            контрагенти_Папки_Select.QuerySelect.Where.Add(new Where(Контрагенти_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            контрагенти_Папки_Select.Select();

            while (контрагенти_Папки_Select.MoveNext())
            {
                Контрагенти_Папки_Objest? контрагенти_Папки_Objest = контрагенти_Папки_Select.Current?.GetDirectoryObject();
                if (контрагенти_Папки_Objest != null)
                    контрагенти_Папки_Objest.Delete();

            }
        }
    }

    class Склади_Папки_Triggers
    {
        public static void New(Склади_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Склади_Папки_Const).ToString("D6");
        }

        public static void Copying(Склади_Папки_Objest ДовідникОбєкт, Склади_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Склади_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Склади_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Склади_Папки_Objest ДовідникОбєкт, bool label)
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
                склади_Select.Select();

                while (склади_Select.MoveNext())
                {
                    Склади_Objest? склади_Objest = склади_Select.Current?.GetDirectoryObject();
                    if (склади_Objest != null)
                        склади_Objest.SetDeletionLabel();
                }

                //
                //Вкладені папки помічаються на видалення
                //

                Склади_Папки_Select cклади_Папки_Select = new Склади_Папки_Select();
                cклади_Папки_Select.QuerySelect.Where.Add(new Where(Склади_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                cклади_Папки_Select.Select();

                while (cклади_Папки_Select.MoveNext())
                {
                    Склади_Папки_Objest? cклади_Папки_Objest = cклади_Папки_Select.Current?.GetDirectoryObject();
                    if (cклади_Папки_Objest != null)
                        cклади_Папки_Objest.SetDeletionLabel();
                }
            }
        }

        public static void BeforeDelete(Склади_Папки_Objest ДовідникОбєкт)
        {
            //
            //Елементи переносяться на верхній рівень
            //

            Склади_Select склади_Select = new Склади_Select();
            склади_Select.QuerySelect.Where.Add(new Where(Склади_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            склади_Select.Select();

            while (склади_Select.MoveNext())
            {
                Склади_Objest? склади_Objest = склади_Select.Current?.GetDirectoryObject();

                if (склади_Objest != null)
                {
                    //Ставиться помітка на видалення для елементу
                    склади_Objest.SetDeletionLabel();

                    склади_Objest.Папка = new Склади_Папки_Pointer();
                    склади_Objest.Save();
                }
            }

            //
            //Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
            //

            Склади_Папки_Select cклади_Папки_Select = new Склади_Папки_Select();
            cклади_Папки_Select.QuerySelect.Where.Add(new Where(Склади_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            cклади_Папки_Select.Select();

            while (cклади_Папки_Select.MoveNext())
            {
                Склади_Папки_Objest? cклади_Папки_Objest = cклади_Папки_Select.Current?.GetDirectoryObject();
                if (cклади_Папки_Objest != null)
                    cклади_Папки_Objest.Delete();
            }
        }
    }

    class Каси_Triggers
    {
        public static void New(Каси_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Каси_Const).ToString("D6");
        }

        public static void Copying(Каси_Objest ДовідникОбєкт, Каси_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Каси_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Каси_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Каси_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(Каси_Objest ДовідникОбєкт)
        {

        }
    }

    class БанківськіРахункиОрганізацій_Triggers
    {
        public static void New(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.БанківськіРахункиОрганізацій_Const).ToString("D6");
        }

        public static void Copying(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт, БанківськіРахункиОрганізацій_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {

        }
    }

    class ДоговориКонтрагентів_Triggers
    {
        public static void New(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ДоговориКонтрагентів_Const).ToString("D6");
        }

        public static void Copying(ДоговориКонтрагентів_Objest ДовідникОбєкт, ДоговориКонтрагентів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            string НазваПереліченняЗКонфігурації =
                Конфа.Config.Kernel!.Conf.Enums["ТипДоговорів"].Fields[ДовідникОбєкт.ТипДоговору.ToString()].Desc;

            ДовідникОбєкт.ТипДоговоруПредставлення = НазваПереліченняЗКонфігурації;
        }

        public static void AfterSave(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ДоговориКонтрагентів_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {

        }
    }

    class БанківськіРахункиКонтрагентів_Triggers
    {
        public static void New(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.БанківськіРахункиКонтрагентів_Const).ToString("D6");
        }

        public static void Copying(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт, БанківськіРахункиКонтрагентів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {

        }
    }

    class СтаттяРухуКоштів_Triggers
    {
        public static void New(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СтаттяРухуКоштів_Const).ToString("D6");
        }

        public static void Copying(СтаттяРухуКоштів_Objest ДовідникОбєкт, СтаттяРухуКоштів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(СтаттяРухуКоштів_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {

        }
    }

    class СеріїНоменклатури_Triggers
    {
        public static void New(СеріїНоменклатури_Objest ДовідникОбєкт)
        {

        }

        public static void Copying(СеріїНоменклатури_Objest ДовідникОбєкт, СеріїНоменклатури_Objest Основа)
        {
            ДовідникОбєкт.Номер = Guid.NewGuid().ToString();
            ДовідникОбєкт.Коментар = "Копія - " + Основа.Номер;
        }

        public static void BeforeSave(СеріїНоменклатури_Objest ДовідникОбєкт)
        {
            СеріїНоменклатури_Select серіїНоменклатури_Select = new СеріїНоменклатури_Select();
            серіїНоменклатури_Select.QuerySelect.Where.Add(new Where(СеріїНоменклатури_Const.Номер, Comparison.EQ, ДовідникОбєкт.Номер));
            серіїНоменклатури_Select.QuerySelect.Where.Add(new Where(Comparison.AND, "uid", Comparison.NOT, ДовідникОбєкт.UnigueID.UGuid));

            if (серіїНоменклатури_Select.SelectSingle())
            {
                ДовідникОбєкт.Коментар = $"Помилка: Серійний номер [ {ДовідникОбєкт.Номер} ] вже існує в базі даних. " + ДовідникОбєкт.Коментар;
                ДовідникОбєкт.Номер = Guid.NewGuid().ToString();
            }
        }

        public static void AfterSave(СеріїНоменклатури_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(СеріїНоменклатури_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(СеріїНоменклатури_Objest ДовідникОбєкт)
        {

        }
    }

    class ПартіяТоварівКомпозит_Triggers
    {
        public static void New(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {

        }

        public static void Copying(ПартіяТоварівКомпозит_Objest ДовідникОбєкт, ПартіяТоварівКомпозит_Objest Основа)
        {

        }

        public static void BeforeSave(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ПартіяТоварівКомпозит_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {

        }
    }

    class ВидиЗапасів_Triggers
    {
        public static void New(ВидиЗапасів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ВидиЗапасів_Const).ToString("D6");
        }

        public static void Copying(ВидиЗапасів_Objest ДовідникОбєкт, ВидиЗапасів_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(ВидиЗапасів_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ВидиЗапасів_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ВидиЗапасів_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ВидиЗапасів_Objest ДовідникОбєкт)
        {

        }
    }

    class Банки_Triggers
    {
        public static void New(Банки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Банки_Const).ToString("D6");
        }

        public static void Copying(Банки_Objest ДовідникОбєкт, Банки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Банки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Банки_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Банки_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(Банки_Objest ДовідникОбєкт)
        {

        }
    }

    class СкладськіПриміщення_Triggers
    {
        public static void New(СкладськіПриміщення_Objest ДовідникОбєкт)
        {

        }

        public static void Copying(СкладськіПриміщення_Objest ДовідникОбєкт, СкладськіПриміщення_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(СкладськіПриміщення_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СкладськіПриміщення_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(СкладськіПриміщення_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(СкладськіПриміщення_Objest ДовідникОбєкт)
        {

        }
    }

    class СкладськіКомірки_Triggers
    {
        public static void New(СкладськіКомірки_Objest ДовідникОбєкт)
        {

        }

        public static void Copying(СкладськіКомірки_Objest ДовідникОбєкт, СкладськіКомірки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(СкладськіКомірки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СкладськіКомірки_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(СкладськіКомірки_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(СкладськіКомірки_Objest ДовідникОбєкт)
        {

        }
    }

    class ТипорозміриКомірок_Triggers
    {
        public static void New(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {

        }

        public static void Copying(ТипорозміриКомірок_Objest ДовідникОбєкт, ТипорозміриКомірок_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(ТипорозміриКомірок_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {

        }
    }

    class СкладськіКомірки_Папки_Triggers
    {
        public static void New(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СкладськіКомірки_Папки_Const).ToString("D6");
        }

        public static void Copying(СкладськіКомірки_Папки_Objest ДовідникОбєкт, СкладськіКомірки_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(СкладськіКомірки_Папки_Objest ДовідникОбєкт, bool label)
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
                cкладськіКомірки_Select.Select();

                while (cкладськіКомірки_Select.MoveNext())
                {
                    СкладськіКомірки_Objest? cкладськіКомірки_Objest = cкладськіКомірки_Select.Current?.GetDirectoryObject();
                    if (cкладськіКомірки_Objest != null)
                        cкладськіКомірки_Objest.SetDeletionLabel();
                }

                //
                //Вкладені папки помічаються на видалення
                //

                СкладськіКомірки_Папки_Select cкладськіКомірки_Папки_Select = new СкладськіКомірки_Папки_Select();
                cкладськіКомірки_Папки_Select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                cкладськіКомірки_Папки_Select.Select();

                while (cкладськіКомірки_Папки_Select.MoveNext())
                {
                    СкладськіКомірки_Папки_Objest? cкладськіКомірки_Папки_Objest = cкладськіКомірки_Папки_Select.Current?.GetDirectoryObject();
                    if (cкладськіКомірки_Папки_Objest != null)
                        cкладськіКомірки_Папки_Objest.SetDeletionLabel();
                }
            }
        }

        public static void BeforeDelete(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            //
            //Елементи помічаються на видалення
            //

            СкладськіКомірки_Select cкладськіКомірки_Select = new СкладськіКомірки_Select();
            cкладськіКомірки_Select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            cкладськіКомірки_Select.QuerySelect.Where.Add(new Where(Comparison.AND, СкладськіКомірки_Const.DELETION_LABEL, Comparison.NOT, true));
            cкладськіКомірки_Select.Select();

            while (cкладськіКомірки_Select.MoveNext())
            {
                СкладськіКомірки_Objest? cкладськіКомірки_Objest = cкладськіКомірки_Select.Current?.GetDirectoryObject();
                if (cкладськіКомірки_Objest != null)
                {
                    cкладськіКомірки_Objest.SetDeletionLabel();

                    cкладськіКомірки_Objest.Папка = new СкладськіКомірки_Папки_Pointer();
                    cкладськіКомірки_Objest.Save();
                }
            }

            //
            //Вкладені папки помічаються на видалення
            //

            СкладськіКомірки_Папки_Select cкладськіКомірки_Папки_Select = new СкладськіКомірки_Папки_Select();
            cкладськіКомірки_Папки_Select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
            cкладськіКомірки_Папки_Select.Select();

            while (cкладськіКомірки_Папки_Select.MoveNext())
            {
                СкладськіКомірки_Папки_Objest? cкладськіКомірки_Папки_Objest = cкладськіКомірки_Папки_Select.Current?.GetDirectoryObject();
                if (cкладськіКомірки_Папки_Objest != null)
                    cкладськіКомірки_Папки_Objest.Delete();
            }
        }
    }

    class Блокнот_Triggers
    {
        public static void New(Блокнот_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Блокнот_Const).ToString("D6");
            ДовідникОбєкт.ДатаЗапису = DateTime.Now;
        }

        public static void Copying(Блокнот_Objest ДовідникОбєкт, Блокнот_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
        }

        public static void BeforeSave(Блокнот_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Блокнот_Objest ДовідникОбєкт)
        {

        }

        public static void SetDeletionLabel(Блокнот_Objest ДовідникОбєкт, bool label)
        {

        }

        public static void BeforeDelete(Блокнот_Objest ДовідникОбєкт)
        {

        }
    }

}

namespace StorageAndTrade_1_0.Документи
{
    class ЗамовленняПостачальнику_Triggers
    {
        public static void New(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
        }

        public static void Copying(ЗамовленняПостачальнику_Objest ДокументОбєкт, ЗамовленняПостачальнику_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Замовлення постачальнику №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ЗамовленняПостачальнику_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {

        }
    }

    class ПоступленняТоварівТаПослуг_Triggers
    {
        public static void New(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
        }

        public static void Copying(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт, ПоступленняТоварівТаПослуг_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Поступлення товарів та послуг №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт, bool label)
        {
            // Помітка на виделення всіх партій
            if (label == true)
            {
                ПартіяТоварівКомпозит_Select партіяТоварівКомпозит_Select = new ПартіяТоварівКомпозит_Select();
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.ПоступленняТоварівТаПослуг, Comparison.EQ, ДокументОбєкт.UnigueID.UGuid));
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(Comparison.AND, ПартіяТоварівКомпозит_Const.DELETION_LABEL, Comparison.NOT, true));
                партіяТоварівКомпозит_Select.Select();

                while (партіяТоварівКомпозит_Select.MoveNext())
                {
                    ПартіяТоварівКомпозит_Objest? партіяТоварівКомпозит_Objest = партіяТоварівКомпозит_Select.Current?.GetDirectoryObject();
                    if (партіяТоварівКомпозит_Objest != null)
                        партіяТоварівКомпозит_Objest.SetDeletionLabel();
                }
            }
        }

        public static void BeforeDelete(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {

        }
    }

    class ЗамовленняКлієнта_Triggers
    {
        public static void New(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
        }

        public static void Copying(ЗамовленняКлієнта_Objest ДокументОбєкт, ЗамовленняКлієнта_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Замовлення клієнта №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ЗамовленняКлієнта_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {

        }
    }

    class РеалізаціяТоварівТаПослуг_Triggers
    {
        public static void New(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
        }

        public static void Copying(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт, РеалізаціяТоварівТаПослуг_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Реалізація товарів та послуг №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {

        }
    }

    class ВстановленняЦінНоменклатури_Triggers
    {
        public static void New(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВстановленняЦінНоменклатури_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(ВстановленняЦінНоменклатури_Objest ДокументОбєкт, ВстановленняЦінНоменклатури_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Встановлення цін номенклатури №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ВстановленняЦінНоменклатури_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {

        }
    }

    class ПрихіднийКасовийОрдер_Triggers
    {
        public static void New(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(ПрихіднийКасовийОрдер_Objest ДокументОбєкт, ПрихіднийКасовийОрдер_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Прихідний касовий ордер №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ПрихіднийКасовийОрдер_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {

        }
    }

    class РозхіднийКасовийОрдер_Triggers
    {
        public static void New(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(РозхіднийКасовийОрдер_Objest ДокументОбєкт, РозхіднийКасовийОрдер_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Розхідний касовий ордер №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(РозхіднийКасовийОрдер_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {

        }
    }

    class ПереміщенняТоварів_Triggers
    {
        public static void New(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПереміщенняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(ПереміщенняТоварів_Objest ДокументОбєкт, ПереміщенняТоварів_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Переміщення товарів №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ПереміщенняТоварів_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ПереміщенняТоварів_Objest ДокументОбєкт)
        {

        }
    }

    class ПоверненняТоварівПостачальнику_Triggers
    {
        public static void New(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоверненняТоварівПостачальнику_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
        }

        public static void Copying(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт, ПоверненняТоварівПостачальнику_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Повернення товарів постачальнику №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {

        }
    }

    class ПоверненняТоварівВідКлієнта_Triggers
    {
        public static void New(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоверненняТоварівВідКлієнта_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
        }

        public static void Copying(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт, ПоверненняТоварівВідКлієнта_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Повернення товарів від клієнта №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {

        }
    }

    class АктВиконанихРобіт_Triggers
    {
        public static void New(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.АктВиконанихРобіт_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
        }

        public static void Copying(АктВиконанихРобіт_Objest ДокументОбєкт, АктВиконанихРобіт_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Акт виконаних робіт №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(АктВиконанихРобіт_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(АктВиконанихРобіт_Objest ДокументОбєкт)
        {

        }
    }

    class ВведенняЗалишків_Triggers
    {
        public static void New(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВведенняЗалишків_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(ВведенняЗалишків_Objest ДокументОбєкт, ВведенняЗалишків_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Введення залишків №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ВведенняЗалишків_Objest ДокументОбєкт, bool label)
        {
            // Помітка на виделення всіх партій
            if (label == true)
            {
                ПартіяТоварівКомпозит_Select партіяТоварівКомпозит_Select = new ПартіяТоварівКомпозит_Select();
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.ВведенняЗалишків, Comparison.EQ, ДокументОбєкт.UnigueID.UGuid));
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(Comparison.AND, ПартіяТоварівКомпозит_Const.DELETION_LABEL, Comparison.NOT, true));
                партіяТоварівКомпозит_Select.Select();

                while (партіяТоварівКомпозит_Select.MoveNext())
                {
                    ПартіяТоварівКомпозит_Objest? партіяТоварівКомпозит_Objest = партіяТоварівКомпозит_Select.Current?.GetDirectoryObject();
                    if (партіяТоварівКомпозит_Objest != null)
                        партіяТоварівКомпозит_Objest.SetDeletionLabel();
                }
            }
        }

        public static void BeforeDelete(ВведенняЗалишків_Objest ДокументОбєкт)
        {

        }
    }

    class ПсуванняТоварів_Triggers
    {
        public static void New(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПсуванняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(ПсуванняТоварів_Objest ДокументОбєкт, ПсуванняТоварів_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Псування товарів №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ПсуванняТоварів_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ПсуванняТоварів_Objest ДокументОбєкт)
        {

        }
    }

    class ВнутрішнєСпоживанняТоварів_Triggers
    {
        public static void New(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВнутрішнєСпоживанняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт, ВнутрішнєСпоживанняТоварів_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Внутрішнє споживання товарів №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {

        }
    }

    class РахунокФактура_Triggers
    {
        public static void New(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РахунокФактура_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;
        }

        public static void Copying(РахунокФактура_Objest ДокументОбєкт, РахунокФактура_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Рахунок фактура №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РахунокФактура_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(РахунокФактура_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(РахунокФактура_Objest ДокументОбєкт)
        {

        }
    }

    class РозміщенняТоварівНаСкладі_Triggers
    {
        public static void New(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозміщенняТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт, РозміщенняТоварівНаСкладі_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Розміщення товарів на складі №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {

        }
    }

    class ПереміщенняТоварівНаСкладі_Triggers
    {
        public static void New(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПереміщенняТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт, ПереміщенняТоварівНаСкладі_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Переміщення товарів на складі №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {

        }
    }

    class ЗбіркаТоварівНаСкладі_Triggers
    {
        public static void New(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗбіркаТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт, ЗбіркаТоварівНаСкладі_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Збірка товарів на складі №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {

        }
    }

    class РозміщенняНоменклатуриПоКоміркам_Triggers
    {
        public static void New(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозміщенняНоменклатуриПоКоміркам_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
        }

        public static void Copying(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт, РозміщенняНоменклатуриПоКоміркам_Objest Основа)
        {
            ДокументОбєкт.Коментар = "Копія: " + Основа.Назва;
        }

        public static void BeforeSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Розміщення номенклатури по комірках №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {

        }

        public static void SetDeletionLabel(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт, bool label)
        {

        }

        public static void BeforeDelete(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {

        }
    }
}