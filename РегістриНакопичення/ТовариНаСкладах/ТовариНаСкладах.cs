

/*     
        ТовариНаСкладах.cs
        Список

        Табличний список - Записи
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = GeneratedCode.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade.РегістриНакопичення
{
    public class ТовариНаСкладах : РегістриНакопиченняЖурнал
    {
        public ТовариНаСкладах() : base()
        {
            ТабличніСписки.ТовариНаСкладах_Записи.AddColumns(TreeViewGrid);
            
            HBoxTop.PackStart(new Label("Таблиці розрахунків:"), false, false, 0);
            CreateLink(HBoxTop, "Залишки", async () => await ТовариНаСкладах_Залишки_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
            CreateLink(HBoxTop, "Залишки та обороти", async () => await ТовариНаСкладах_ЗалишкиТаОбороти_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
            CreateLink(HBoxTop, "Підсумки", async () => await ТовариНаСкладах_Підсумки_Звіт.Сформувати());
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ТовариНаСкладах_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ТовариНаСкладах_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ТовариНаСкладах_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ТовариНаСкладах_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.ТовариНаСкладах_Записи.LoadRecords(TreeViewGrid);
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.ТовариНаСкладах";

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
