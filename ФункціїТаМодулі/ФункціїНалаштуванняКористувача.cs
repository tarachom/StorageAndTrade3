

using InterfaceGtk;

using AccountingSoftware;
using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade
{
    class ФункціїНалаштуванняКористувача
    {
        /// <summary>
        /// Функція отримує збережені налаштування користувача і зразу заповнює PeriodControl
        /// </summary>
        /// <param name="НазваЖурналу">Журнал</param>
        /// <param name="Період">PeriodControl</param>
        public static async ValueTask ОтриматиПеріодДляЖурналу(string НазваЖурналу, PeriodControl Період)
        {
            var періодДляЖурналу = await ОтриматиПеріодДляЖурналу(НазваЖурналу);

            if (періодДляЖурналу.ДатаСтарт != null)
                Період.DateStart = періодДляЖурналу.ДатаСтарт.Value;

            if (періодДляЖурналу.ДатаСтоп != null)
                Період.DateStop = періодДляЖурналу.ДатаСтоп.Value;
            /*
            ВАЖЛИВО!
            Коли змінюється значення Період.Period вкликається функція PeriodChanged()
            Послідовність: SetValue() -> BeforeSetValue() -> PeriodChanged() -> LoadRecords();
            */
            if (!string.IsNullOrEmpty(періодДляЖурналу.Період) && Enum.TryParse<ПеріодДляЖурналу.ТипПеріоду>(періодДляЖурналу.Період, out ПеріодДляЖурналу.ТипПеріоду result))
                Період.Period = result;
            else
                Період.Period = Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(ЖурналиДокументів.ОсновнийТипПеріоду_Const);
        }

        /// <summary>
        /// Функція отримує з табличної частини константи Системні/НалаштуванняКористувача/ПеріодиЖурналів
        /// збережені налаштування користувача
        /// </summary>
        /// <param name="НазваЖурналу">Назва журналу</param>
        /// <returns>Повертає кортеж</returns>
        public static async ValueTask<(string Період, DateTime? ДатаСтарт, DateTime? ДатаСтоп)> ОтриматиПеріодДляЖурналу(string НазваЖурналу)
        {
            Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart Налаштування_TablePart = new();

            ЗаповнитиВідбір(Налаштування_TablePart, НазваЖурналу);

            //Вибирати тільки одне значення
            Налаштування_TablePart.QuerySelect.Limit = 1;

            await Налаштування_TablePart.Read();
            if (Налаштування_TablePart.Records.Count != 0)
            {
                var record = Налаштування_TablePart.Records[0];

                string періодЗначення = record.ПеріодЗначення;
                DateTime? датаСтарт = (record.ДатаСтарт != DateTime.MinValue) ? record.ДатаСтарт : null;
                DateTime? датаСтоп = (record.ДатаСтоп != DateTime.MinValue) ? record.ДатаСтоп : null;

                return (періодЗначення, датаСтарт, датаСтоп);
            }
            else
                return ("", null, null);
        }

        static void ЗаповнитиВідбір(Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart Налаштування_TablePart, string НазваЖурналу)
        {
            //Відбір по користувачу
            Налаштування_TablePart.QuerySelect.Where.Add(
                new Where(Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart.Користувач, Comparison.EQ, Program.Користувач.UnigueID.UGuid));

            //Відбір по журналу
            Налаштування_TablePart.QuerySelect.Where.Add(
                new Where(Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart.Журнал, Comparison.EQ, НазваЖурналу));
        }

        /// <summary>
        /// Функція записує налаштування користувача
        /// </summary>
        /// <param name="НазваЖурналу">Журнал</param>
        /// <param name="Період">Тип періоду</param>
        /// <param name="ДатаСтарт"></param>
        /// <param name="ДатаСтоп"></param>
        public static async void ЗаписатиПеріодДляЖурналу(string НазваЖурналу, string Період, DateTime? ДатаСтарт = null, DateTime? ДатаСтоп = null)
        {
            Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart НалаштуванняКористувача = new();

            //Очистка
            {
                ЗаповнитиВідбір(НалаштуванняКористувача, НазваЖурналу);

                await НалаштуванняКористувача.Read();
                await НалаштуванняКористувача.RemoveAll(НалаштуванняКористувача.Records);
            }

            //Додавання нового
            {
                Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart.Record record = new()
                {
                    Користувач = Program.Користувач,
                    Журнал = НазваЖурналу,
                    ПеріодЗначення = Період
                };

                if (ДатаСтарт != null)
                    record.ДатаСтарт = ДатаСтарт.Value;

                if (ДатаСтоп != null)
                    record.ДатаСтоп = ДатаСтоп.Value;

                НалаштуванняКористувача.Records.Add(record);
                await НалаштуванняКористувача.Save(false);
            }
        }
    }
}