

/*     
        РозрахункиЗПостачальниками.cs
        Список

        Табличний список - Записи
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = GeneratedCode.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade.РегістриНакопичення
{
    public class РозрахункиЗПостачальниками : РегістриНакопиченняЖурнал
    {
        public РозрахункиЗПостачальниками() : base()
        {
            ТабличніСписки.РозрахункиЗПостачальниками_Записи.AddColumns(TreeViewGrid);
            
            HBoxTop.PackStart(new Label("Таблиці розрахунків:"), false, false, 0);
            CreateLink(HBoxTop, "Залишки", async () => await РозрахункиЗПостачальниками_Залишки_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
            CreateLink(HBoxTop, "Залишки та обороти", async () => await РозрахункиЗПостачальниками_ЗалишкиТаОбороти_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.РозрахункиЗПостачальниками_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозрахункиЗПостачальниками_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РозрахункиЗПостачальниками_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.РозрахункиЗПостачальниками_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.РозрахункиЗПостачальниками_Записи.LoadRecords(TreeViewGrid);
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.РозрахункиЗПостачальниками";

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
