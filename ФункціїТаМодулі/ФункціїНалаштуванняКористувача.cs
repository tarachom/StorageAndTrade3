using Gtk;
using InterfaceGtk;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;

namespace StorageAndTrade
{
    class ФункціїНалаштуванняКористувача
    {
        public static async ValueTask ОтриматиПеріодДляЖурналу(string НазваЖурналу, PeriodControl Період)
        {
            var періодДляЖурналу = await ОтриматиПеріодДляЖурналу(НазваЖурналу);

            if (періодДляЖурналу.ДатаСтарт != null)
                Період.DateStart = періодДляЖурналу.ДатаСтарт.Value;

            if (періодДляЖурналу.ДатаСтоп != null)
                Період.DateStop = періодДляЖурналу.ДатаСтоп.Value;

            if (!string.IsNullOrEmpty(періодДляЖурналу.Період) && Enum.TryParse<ПеріодДляЖурналу.ТипПеріоду>(періодДляЖурналу.Період, out ПеріодДляЖурналу.ТипПеріоду result))
                Період.Period = result;
            else
                Період.Period = Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(ЖурналиДокументів.ОсновнийТипПеріоду_Const);

        }

        public static async ValueTask<(string Період, DateTime? ДатаСтарт, DateTime? ДатаСтоп)> ОтриматиПеріодДляЖурналу(string НазваЖурналу)
        {
            Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart НалаштуванняКористувача = new();

            ЗаповнитиВідбір(НалаштуванняКористувача, НазваЖурналу);

            //Вибирати тільки одне значення
            НалаштуванняКористувача.QuerySelect.Limit = 1;

            await НалаштуванняКористувача.Read();
            if (НалаштуванняКористувача.Records.Count != 0)
            {
                var record = НалаштуванняКористувача.Records[0];

                string періодЗначення = record.ПеріодЗначення;
                DateTime? датаСтарт = (record.ДатаСтарт != DateTime.MinValue) ? record.ДатаСтарт : null;
                DateTime? датаСтоп = (record.ДатаСтоп != DateTime.MinValue) ? record.ДатаСтоп : null;

                return (періодЗначення, датаСтарт, датаСтоп);
            }
            else
                return ("", null, null);
        }

        static void ЗаповнитиВідбір(Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart НалаштуванняКористувача, string НазваЖурналу)
        {
            //Відбір по користувачу
            НалаштуванняКористувача.QuerySelect.Where.Add(new Where(Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart.Користувач, Comparison.EQ, Program.Користувач.UnigueID.UGuid));

            //Відбір по журналу
            НалаштуванняКористувача.QuerySelect.Where.Add(new Where(Comparison.AND, Системні.НалаштуванняКористувача_ПеріодиЖурналів_TablePart.Журнал, Comparison.EQ, НазваЖурналу));
        }

        public static async ValueTask ЗаписатиПеріодДляЖурналу(string НазваЖурналу, string Період, DateTime? ДатаСтарт = null, DateTime? ДатаСтоп = null)
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