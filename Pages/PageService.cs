/*

1. Очистка помічених на видалення
2. Масове перепроведення документів

*/

using AccountingSoftware;
using GeneratedCode;

namespace StorageAndTrade
{
    class PageService : InterfaceGtk3.PageService
    {
        public PageService() : base(Config.Kernel, Config.NameSpageProgram, Config.NameSpageCodeGeneration) { }

        protected override CompositePointerControl CreateCompositeControl(string caption, UuidAndText uuidAndText) =>
            new CompositePointerControl() { Caption = caption, Pointer = uuidAndText, ClearSensetive = false, TypeSelectSensetive = false };

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