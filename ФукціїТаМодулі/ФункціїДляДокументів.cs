#region Info

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

#endregion

/*

Функції для документів

Функції для шапки
Контекстне меню для табличної частини

*/

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;
using Довідники = StorageAndTrade_1_0.Довідники;
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using РегістриВідомостей = StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    /// <summary>
    /// Спільні функції для документів
    /// </summary>
    class ФункціїДляДокументів
    {
        /// <summary>
        /// Функція обєднує дві дати (з пешої дата, з другої час)
        /// </summary>
        /// <param name="дата">Дата</param>
        /// <param name="час">Час</param>
        /// <returns>Обєднана дата</returns>
        public static DateTime ОбєднатиДатуТаЧас(DateTime дата, DateTime час)
        {
            return new DateTime(дата.Year, дата.Month, дата.Day, час.Hour, час.Minute, час.Second);
        }

        #region ПартіяТоварівКомпозит

        /// <summary>
        /// Функція повертає вказівник на елемент довідника ПартіяТоварівКомпозит
        /// </summary>
        /// <param name="ДокументКлюч">Ключ документу</param>
        /// <param name="ТипДокументу">Тип документу</param>
        /// <param name="ДокументПоступлення"></param>
        /// <param name="ДокументВведенняЗалишків"></param>
        /// <returns></returns>
        public static Довідники.ПартіяТоварівКомпозит_Pointer ОтриматиПартіюТоварівКомпозит(
            Guid ДокументКлюч,
            Перелічення.ТипДокументуПартіяТоварівКомпозит ТипДокументу,
            Документи.ПоступленняТоварівТаПослуг_Objest? ДокументПоступлення,
            Документи.ВведенняЗалишків_Objest? ДокументВведенняЗалишків)
        {
            Довідники.ПартіяТоварівКомпозит_Select партіяТоварівКомпозитВибірка = new Довідники.ПартіяТоварівКомпозит_Select();
            Довідники.ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = 
                партіяТоварівКомпозитВибірка.FindByField(Довідники.ПартіяТоварівКомпозит_Const.ДокументКлюч, ДокументКлюч);

            Довідники.ПартіяТоварівКомпозит_Objest партіяТоварівКомпозитНовий = new Довідники.ПартіяТоварівКомпозит_Objest();
            if (ПартіяТоварівКомпозит.IsEmpty())
                партіяТоварівКомпозитНовий.New();
            else if (!партіяТоварівКомпозитНовий.Read(ПартіяТоварівКомпозит.UnigueID))
                партіяТоварівКомпозитНовий.New();

            партіяТоварівКомпозитНовий.ТипДокументу = ТипДокументу;
            партіяТоварівКомпозитНовий.ДокументКлюч = ДокументКлюч;

            switch (ТипДокументу)
            {
                case Перелічення.ТипДокументуПартіяТоварівКомпозит.ПоступленняТоварівТаПослуг:
                    {
                        партіяТоварівКомпозитНовий.Дата = ДокументПоступлення!.ДатаДок;
                        партіяТоварівКомпозитНовий.Назва = ДокументПоступлення.Назва;
                        партіяТоварівКомпозитНовий.ПоступленняТоварівТаПослуг = ДокументПоступлення.GetDocumentPointer();
                        break;
                    }
                case Перелічення.ТипДокументуПартіяТоварівКомпозит.ВведенняЗалишків:
                    {
                        партіяТоварівКомпозитНовий.Дата = ДокументВведенняЗалишків!.ДатаДок;
                        партіяТоварівКомпозитНовий.Назва = ДокументВведенняЗалишків.Назва;
                        партіяТоварівКомпозитНовий.ВведенняЗалишків = ДокументВведенняЗалишків.GetDocumentPointer();
                        break;
                    }
            }

            партіяТоварівКомпозитНовий.Save();

            return партіяТоварівКомпозитНовий.GetDirectoryPointer();
        }

        #endregion

        /// <summary>
        /// Функція повертає перший із списку договорів - договір контрагента
        /// </summary>
        /// <param name="Контрагент">Контрагент</param>
        /// <param name="ТипДоговору">Тип договору</param>
        /// <returns></returns>
        public static Довідники.ДоговориКонтрагентів_Pointer? ОсновнийДоговірДляКонтрагента(
            Довідники.Контрагенти_Pointer Контрагент, Перелічення.ТипДоговорів ТипДоговору = 0)
        {
            if (Контрагент == null || Контрагент.IsEmpty())
                return null;

            Довідники.ДоговориКонтрагентів_Select договориКонтрагентів = new Довідники.ДоговориКонтрагентів_Select();

            //Відбір по контрагенту
            договориКонтрагентів.QuerySelect.Where.Add(
                new Where(Довідники.ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, Контрагент.UnigueID.UGuid));

            if (ТипДоговору != 0)
            {
                //Відбір по типу договору
                договориКонтрагентів.QuerySelect.Where.Add(
                    new Where(Comparison.AND, Довідники.ДоговориКонтрагентів_Const.ТипДоговору, Comparison.EQ, (int)ТипДоговору));
            }

            if (договориКонтрагентів.SelectSingle())
                return договориКонтрагентів.Current;
            else
                return null;
        }

        /// <summary>
        /// Функція повертає курс валюти на дату
        /// </summary>
        /// <param name="Валюта">Валюта</param>
        /// <param name="ДатаКурсу">Курс</param>
        /// <returns>Курс на дату (дата + час 23 59 59) або 0</returns>
        public static decimal ПоточнийКурсВалюти(Довідники.Валюти_Pointer Валюта, DateTime ДатаКурсу)
        {
            if (Валюта.IsEmpty())
                return 0;

            string query = @$"
SELECT
    {РегістриВідомостей.КурсиВалют_Const.Курс} AS Курс
FROM
    {РегістриВідомостей.КурсиВалют_Const.TABLE} AS КурсиВалют
WHERE
    КурсиВалют.{РегістриВідомостей.КурсиВалют_Const.Валюта} = @valuta AND
    КурсиВалют.period <= @date_curs
ORDER BY КурсиВалют.period DESC
LIMIT 1
";
            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("valuta", Валюта.UnigueID.UGuid);
            paramQuery.Add("date_curs", new DateTime(ДатаКурсу.Year, ДатаКурсу.Month, ДатаКурсу.Day, 23, 59, 59));

            string[] columnsName;
            List<Dictionary<string, object>> listRow;

            Конфа.Config.Kernel!.DataBase.SelectRequest(query, paramQuery, out columnsName, out listRow);

            if (listRow.Count == 1)
            {
                Dictionary<string, object> Рядок = listRow[0];
                return (decimal)Рядок["Курс"];
            }
            else
                return 0;
        }
    }
}
