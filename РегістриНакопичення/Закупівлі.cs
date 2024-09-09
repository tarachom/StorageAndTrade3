

/*     
        Закупівлі.cs
        Список

        Табличний список - Записи
*/

using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade
{
    public class Закупівлі : РегістриНакопиченняЖурнал
    {
        public Закупівлі() 
        {
            ТабличніСписки.Закупівлі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Закупівлі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Закупівлі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.Закупівлі_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Закупівлі_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Закупівлі_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.Закупівлі_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Закупівлі_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Закупівлі_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.Закупівлі_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.Закупівлі_Записи.LoadRecords(TreeViewGrid);
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.Закупівлі";

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
