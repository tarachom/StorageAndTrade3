

/*     
        Закупівлі.cs
        Список

        Табличний список - Записи
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = GeneratedCode.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade.РегістриНакопичення
{
    public class Закупівлі : РегістриНакопиченняЖурнал
    {
        public Закупівлі() : base()
        {
            ТабличніСписки.Закупівлі_Записи.AddColumns(TreeViewGrid, ["income"]);
            ТабличніСписки.Закупівлі_Записи.Сторінки(TreeViewGrid, new Сторінки.Налаштування() { PageSize = 300, Тип = Сторінки.ТипЖурналу.РегістриНакопичення });

            HBoxTop.PackStart(new Label("Таблиці розрахунків:"), false, false, 0);
            CreateLink(HBoxTop, "Обороти", async () => await Закупівлі_Обороти_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Закупівлі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.Закупівлі_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
            PagesShow(LoadRecords);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Закупівлі_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.Закупівлі_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.Закупівлі_Записи.LoadRecords(TreeViewGrid);
            PagesShow(async () => await LoadRecords_OnSearch(searchText));
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.Закупівлі";

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
