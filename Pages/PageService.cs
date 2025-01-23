/*

1. Очистка помічених на видалення
2. Масове перепроведення документів

*/

using GeneratedCode;

namespace StorageAndTrade
{
    class PageService : InterfaceGtk.PageService
    {
        public PageService() : base(Config.Kernel, Config.NameSpageProgram, Config.NameSpageCodeGeneration) { }

        const string КлючНалаштуванняКористувача = "PageService";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
        }
    }
}