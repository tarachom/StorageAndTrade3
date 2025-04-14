

/*     
        ЗамовленняКлієнтів.cs
        Список

        Табличний список - Записи
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = GeneratedCode.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade.РегістриНакопичення
{
    public class ЗамовленняКлієнтів : РегістриНакопиченняЖурнал
    {
        public ЗамовленняКлієнтів() : base()
        {
            ТабличніСписки.ЗамовленняКлієнтів_Записи.AddColumns(TreeViewGrid);
            ТабличніСписки.ЗамовленняКлієнтів_Записи.Сторінки(TreeViewGrid, new Сторінки.Налаштування() { PageSize = 300, Тип = Сторінки.ТипЖурналу.РегістриНакопичення });

            HBoxTop.PackStart(new Label("Таблиці розрахунків:"), false, false, 0);
            CreateLink(HBoxTop, "Залишки", async () => await ЗамовленняКлієнтів_Залишки_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗамовленняКлієнтів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗамовленняКлієнтів_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
            PagesShow(LoadRecords);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗамовленняКлієнтів_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ЗамовленняКлієнтів_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.ЗамовленняКлієнтів_Записи.LoadRecords(TreeViewGrid);
            PagesShow(async () => await LoadRecords_OnSearch(searchText));
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.ЗамовленняКлієнтів";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            ClearPages();
            await LoadRecords();
        }

        #endregion
    }
}
