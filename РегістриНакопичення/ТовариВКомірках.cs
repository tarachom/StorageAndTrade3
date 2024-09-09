

/*     
        ТовариВКомірках.cs
        Список

        Табличний список - Записи
*/

using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade
{
    public class ТовариВКомірках : РегістриНакопиченняЖурнал
    {
        public ТовариВКомірках() 
        {
            ТабличніСписки.ТовариВКомірках_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ТовариВКомірках_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ТовариВКомірках_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ТовариВКомірках_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ТовариВКомірках_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ТовариВКомірках_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ТовариВКомірках_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ТовариВКомірках_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ТовариВКомірках_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ТовариВКомірках_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.ТовариВКомірках_Записи.LoadRecords(TreeViewGrid);
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.ТовариВКомірках";

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
