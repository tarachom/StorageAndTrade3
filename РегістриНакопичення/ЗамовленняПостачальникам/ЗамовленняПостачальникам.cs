

/*     
        ЗамовленняПостачальникам.cs
        Список

        Табличний список - Записи
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using ТабличніСписки = GeneratedCode.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade.РегістриНакопичення
{
    public class ЗамовленняПостачальникам : РегістриНакопиченняЖурнал
    {
        public ЗамовленняПостачальникам() : base()
        {
            ТабличніСписки.ЗамовленняПостачальникам_Записи.AddColumns(TreeViewGrid);
            
            HBoxTop.PackStart(new Label("Таблиці розрахунків:"), false, false, 0);
            CreateLink(HBoxTop, "Залишки", async () => await ЗамовленняПостачальникам_Залишки_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗамовленняПостачальникам_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗамовленняПостачальникам_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗамовленняПостачальникам_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ЗамовленняПостачальникам_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.ЗамовленняПостачальникам_Записи.LoadRecords(TreeViewGrid);
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.ЗамовленняПостачальникам";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
