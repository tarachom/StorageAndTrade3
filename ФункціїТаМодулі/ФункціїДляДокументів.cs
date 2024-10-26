/*

Функції для документів

*/

using AccountingSoftware;
using StorageAndTrade_1_0;
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
        public static async ValueTask<Довідники.ПартіяТоварівКомпозит_Pointer> ОтриматиПартіюТоварівКомпозит(
            Guid ДокументКлюч,
            Перелічення.ТипДокументуПартіяТоварівКомпозит ТипДокументу,
            Документи.ПоступленняТоварівТаПослуг_Objest? ДокументПоступлення,
            Документи.ВведенняЗалишків_Objest? ДокументВведенняЗалишків)
        {
            Довідники.ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит = await new Довідники.ПартіяТоварівКомпозит_Select()
                .FindByField(Довідники.ПартіяТоварівКомпозит_Const.ДокументКлюч, ДокументКлюч);

            Довідники.ПартіяТоварівКомпозит_Objest партіяТоварівКомпозитНовий = new();
            if (ПартіяТоварівКомпозит.IsEmpty())
                await партіяТоварівКомпозитНовий.New();
            else if (!await партіяТоварівКомпозитНовий.Read(ПартіяТоварівКомпозит.UnigueID))
                await партіяТоварівКомпозитНовий.New();

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

            await партіяТоварівКомпозитНовий.Save();

            return партіяТоварівКомпозитНовий.GetDirectoryPointer();
        }

        #endregion

        /// <summary>
        /// Функція повертає перший із списку договорів - договір контрагента
        /// </summary>
        /// <param name="Контрагент">Контрагент</param>
        /// <param name="ТипДоговору">Тип договору</param>
        /// <returns></returns>
        public static async ValueTask<Довідники.ДоговориКонтрагентів_Pointer?> ОсновнийДоговірДляКонтрагента(
            Довідники.Контрагенти_Pointer Контрагент, Перелічення.ТипДоговорів ТипДоговору = 0)
        {
            if (Контрагент.IsEmpty())
                return null;

            Довідники.ДоговориКонтрагентів_Select договориКонтрагентів = new();

            //Відбір по контрагенту
            договориКонтрагентів.QuerySelect.Where.Add(new Where(Довідники.ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, Контрагент.UnigueID.UGuid));

            //Відбір по типу договору
            if (ТипДоговору != 0)
                договориКонтрагентів.QuerySelect.Where.Add(new Where(Довідники.ДоговориКонтрагентів_Const.ТипДоговору, Comparison.EQ, (int)ТипДоговору));

            return await договориКонтрагентів.SelectSingle() ? договориКонтрагентів.Current : null;
        }

        /// <summary>
        /// Функція повертає курс валюти на дату
        /// </summary>
        /// <param name="Валюта">Валюта</param>
        /// <param name="ДатаКурсу">Курс</param>
        /// <returns>Курс на дату (дата + час 23 59 59) або 0</returns>
        public static async ValueTask<decimal> ПоточнийКурсВалюти(Довідники.Валюти_Pointer Валюта, DateTime ДатаКурсу)
        {
            if (Валюта.IsEmpty())
                return 0;

            string query = @$"
SELECT
    КурсиВалют.{РегістриВідомостей.КурсиВалют_Const.Курс} AS Курс
FROM
    {РегістриВідомостей.КурсиВалют_Const.TABLE} AS КурсиВалют
WHERE
    КурсиВалют.{РегістриВідомостей.КурсиВалют_Const.Валюта} = @valuta AND
    КурсиВалют.period <= @date_curs
ORDER BY КурсиВалют.period DESC
LIMIT 1
";
            Dictionary<string, object> paramQuery = new()
            {
                { "valuta", Валюта.UnigueID.UGuid },
                { "date_curs", new DateTime(ДатаКурсу.Year, ДатаКурсу.Month, ДатаКурсу.Day, 23, 59, 59) }
            };

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query, paramQuery);
            if (recordResult.Result)
            {
                Dictionary<string, object> Рядок = recordResult.ListRow[0];
                return (decimal)Рядок["Курс"];
            }
            else
                return 0;
        }
    }
}
