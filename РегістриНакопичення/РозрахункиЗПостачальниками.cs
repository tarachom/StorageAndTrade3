

/*     
        РозрахункиЗПостачальниками.cs
        Список

        Табличний список - Записи
*/

using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade
{
    public class РозрахункиЗПостачальниками : РегістриНакопиченняЖурнал
    {
        public РозрахункиЗПостачальниками() : base()
        {
            ТабличніСписки.РозрахункиЗПостачальниками_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.РозрахункиЗПостачальниками_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозрахункиЗПостачальниками_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозрахункиЗПостачальниками_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РозрахункиЗПостачальниками_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозрахункиЗПостачальниками_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РозрахункиЗПостачальниками_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозрахункиЗПостачальниками_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.РозрахункиЗПостачальниками_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.РозрахункиЗПостачальниками_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.РозрахункиЗПостачальниками_Записи.LoadRecords(TreeViewGrid);
        }
        
        const string КлючНалаштуванняКористувача = "РегістриНакопичення.РозрахункиЗПостачальниками";

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
    