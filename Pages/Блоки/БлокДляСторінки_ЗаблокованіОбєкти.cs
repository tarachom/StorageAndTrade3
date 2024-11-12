/*

Заблоковані об'єкти

*/

using AccountingSoftware;
using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class БлокДляСторінки_ЗаблокованіОбєкти : InterfaceGtk.БлокДляСторінки_ЗаблокованіОбєкти
    {
        public БлокДляСторінки_ЗаблокованіОбєкти() : base(Config.Kernel) { }

        protected override async ValueTask<CompositePointerPresentation_Record> CompositePointerPresentation(UuidAndText uuidAndText)
        {
            return await Functions.CompositePointerPresentation(uuidAndText);
        }
    }
}