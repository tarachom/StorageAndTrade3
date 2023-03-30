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

namespace StorageAndTrade_1_0.Довідники
{
    class Організації_Triggers
    {
        public static void New(Організації_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Організації_Const).ToString("D6");
        }

        public static void BeforeSave(Організації_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Організації_Objest ДовідникОбєкт)
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

        public static void BeforeSave(Номенклатура_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Номенклатура_Objest ДовідникОбєкт)
        {

        }

        public static void BeforeDelete(Номенклатура_Objest ДовідникОбєкт)
        {
            //
            //Очистка штрих-кодів
            //


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

        public static void BeforeSave(Виробники_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Виробники_Objest ДовідникОбєкт)
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

        public static void BeforeSave(ВидиНоменклатури_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ВидиНоменклатури_Objest ДовідникОбєкт)
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

        public static void BeforeSave(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ПакуванняОдиниціВиміру_Objest ДовідникОбєкт)
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

        public static void BeforeSave(Валюти_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Валюти_Objest ДовідникОбєкт)
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

        public static void BeforeSave(Контрагенти_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Контрагенти_Objest ДовідникОбєкт)
        {
            ФункціїДляДовідників.СтворитиДоговориКонтрагентаЗаЗамовчуванням(ДовідникОбєкт.GetDirectoryPointer());
        }

        public static void BeforeDelete(Контрагенти_Objest ДовідникОбєкт)
        {

        }
    }

    class Склади_Triggers
    {
        public static void New(Склади_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Склади_Const).ToString("D6");
        }

        public static void BeforeSave(Склади_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Склади_Objest ДовідникОбєкт)
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

        public static void BeforeSave(ВидиЦін_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ВидиЦін_Objest ДовідникОбєкт)
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

        public static void BeforeSave(ВидиЦінПостачальників_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ВидиЦінПостачальників_Objest ДовідникОбєкт)
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

        public static void BeforeSave(Користувачі_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Користувачі_Objest ДовідникОбєкт)
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

        public static void BeforeSave(ФізичніОсоби_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ФізичніОсоби_Objest ДовідникОбєкт)
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

        public static void BeforeSave(СтруктураПідприємства_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СтруктураПідприємства_Objest ДовідникОбєкт)
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

        public static void BeforeSave(КраїниСвіту_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(КраїниСвіту_Objest ДовідникОбєкт)
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

        public static void BeforeSave(Файли_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Файли_Objest ДовідникОбєкт)
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

        public static void BeforeSave(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ХарактеристикиНоменклатури_Objest ДовідникОбєкт)
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

        public static void BeforeSave(Номенклатура_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Номенклатура_Папки_Objest ДовідникОбєкт)
        {

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

        public static void BeforeSave(Контрагенти_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Контрагенти_Папки_Objest ДовідникОбєкт)
        {

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

        public static void BeforeSave(Склади_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Склади_Папки_Objest ДовідникОбєкт)
        {

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

        public static void BeforeSave(Каси_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Каси_Objest ДовідникОбєкт)
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

        public static void BeforeSave(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {

        }

        public static void BeforeDelete(БанківськіРахункиОрганізацій_Objest ДовідникОбєкт)
        {

        }
    }

    class СтаттяРухуКоштів_Triggers
    {
        public static void New(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СтаттяРухуКоштів_Const).ToString("D6");
        }

        public static void BeforeSave(СтаттяРухуКоштів_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СтаттяРухуКоштів_Objest ДовідникОбєкт)
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

        public static void BeforeDelete(СеріїНоменклатури_Objest ДовідникОбєкт)
        {

        }
    }

    class ПартіяТоварівКомпозит_Triggers
    {
        public static void New(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {

        }

        public static void BeforeSave(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ПартіяТоварівКомпозит_Objest ДовідникОбєкт)
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

        }

        public static void BeforeSave(ВидиЗапасів_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ВидиЗапасів_Objest ДовідникОбєкт)
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

        public static void BeforeSave(Банки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Банки_Objest ДовідникОбєкт)
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

        public static void BeforeSave(СкладськіПриміщення_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СкладськіПриміщення_Objest ДовідникОбєкт)
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

        public static void BeforeSave(СкладськіКомірки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СкладськіКомірки_Objest ДовідникОбєкт)
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

        public static void BeforeSave(ТипорозміриКомірок_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(ТипорозміриКомірок_Objest ДовідникОбєкт)
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

        public static void BeforeSave(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {

        }

        public static void BeforeDelete(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {

        }
    }

    class ДоговориКонтрагентів_Triggers
    {
        public static void New(ДоговориКонтрагентів_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.ДоговориКонтрагентів_Const).ToString("D6");
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

        public static void BeforeSave(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {

        }

        public static void BeforeDelete(БанківськіРахункиКонтрагентів_Objest ДовідникОбєкт)
        {

        }
    }

    class Блокнот_Triggers
    {
        public static void New(Блокнот_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.Блокнот_Const).ToString("D6");
            ДовідникОбєкт.ДатаЗапису = DateTime.Now;
        }

        public static void BeforeSave(Блокнот_Objest ДовідникОбєкт)
        {

        }

        public static void AfterSave(Блокнот_Objest ДовідникОбєкт)
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

        public static void BeforeSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Замовлення постачальнику №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Поступлення товарів та послуг №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Замовлення клієнта №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Реалізація товарів та послуг №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Встановлення цін номенклатури №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Прихідний касовий ордер №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Розхідний касовий ордер №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Переміщення товарів №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Повернення товарів постачальнику №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Повернення товарів від клієнта №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Акт виконаних робіт №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Введення залишків №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Внутрішнє споживання товарів №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Рахунок фактура №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РахунокФактура_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Розміщення товарів на складі №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Переміщення товарів на складі №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Збірка товарів на складі №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Розміщення номенклатури по комірках №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
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

        public static void BeforeSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"Псування товарів №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
        }

        public static void AfterSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {

        }

        public static void BeforeDelete(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.ClearSpendTheDocument();
        }
    }

}