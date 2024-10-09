

/*     
        РозрахункиЗКлієнтами.cs
        Список

        Табличний список - Записи
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade
{
    public class РозрахункиЗКлієнтами : РегістриНакопиченняЖурнал
    {
        public РозрахункиЗКлієнтами() : base()
        {
            ТабличніСписки.РозрахункиЗКлієнтами_Записи.AddColumns(TreeViewGrid);

            HBoxTop.PackStart(new Label("Таблиці розрахунків:"), false, false, 0);
            CreateLink(HBoxTop, "Залишки", async () => await РозрахункиЗКлієнтами_Залишки_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
            CreateLink(HBoxTop, "Залишки та обороти", async () => await РозрахункиЗКлієнтами_ЗалишкиТаОбороти_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.РозрахункиЗКлієнтами_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозрахункиЗКлієнтами_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозрахункиЗКлієнтами_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РозрахункиЗКлієнтами_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.РозрахункиЗКлієнтами_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.РозрахункиЗКлієнтами_Записи.LoadRecords(TreeViewGrid);
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.РозрахункиЗКлієнтами";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }

        #endregion
    }
}
