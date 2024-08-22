

/*     
        ВільніЗалишки.cs
        Список

        Табличний список - Записи
*/

using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade
{
    public class ВільніЗалишки : РегістриЖурнал
    {
        public ВільніЗалишки() : base()
        {
            ТабличніСписки.ВільніЗалишки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.ВільніЗалишки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВільніЗалишки_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ВільніЗалишки_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ВільніЗалишки_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВільніЗалишки_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ВільніЗалишки_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ВільніЗалишки_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ВільніЗалишки_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ВільніЗалишки_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.ВільніЗалишки_Записи.LoadRecords(TreeViewGrid);
        }
        
        const string КлючНалаштуванняКористувача = "РегістриНакопичення.ВільніЗалишки";

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
    