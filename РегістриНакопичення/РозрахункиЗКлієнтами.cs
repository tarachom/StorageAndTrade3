

/*     
        РозрахункиЗКлієнтами.cs
        Список

        Табличний список - Записи
*/

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
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.РозрахункиЗКлієнтами_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозрахункиЗКлієнтами_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозрахункиЗКлієнтами_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РозрахункиЗКлієнтами_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозрахункиЗКлієнтами_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РозрахункиЗКлієнтами_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозрахункиЗКлієнтами_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.РозрахункиЗКлієнтами_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.РозрахункиЗКлієнтами_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.РозрахункиЗКлієнтами_Записи.LoadRecords(TreeViewGrid);
        }
        
        const string КлючНалаштуванняКористувача = "РегістриНакопичення.РозрахункиЗКлієнтами";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            LoadRecords();           
        }

        #endregion
    }
}
    