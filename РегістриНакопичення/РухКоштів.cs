

/*     
        РухКоштів.cs
        Список

        Табличний список - Записи
*/

using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade
{
    public class РухКоштів : РегістриНакопиченняЖурнал
    {
        public РухКоштів() 
        {
            ТабличніСписки.РухКоштів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.РухКоштів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РухКоштів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РухКоштів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РухКоштів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РухКоштів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РухКоштів_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РухКоштів_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.РухКоштів_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.РухКоштів_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.РухКоштів_Записи.LoadRecords(TreeViewGrid);
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.РухКоштів";

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
