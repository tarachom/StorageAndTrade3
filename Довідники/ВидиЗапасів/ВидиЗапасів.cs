
/*     
        ВидиЗапасів.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ВидиЗапасів : ДовідникЖурнал
    {
        public ВидиЗапасів() : base()
        {
            ТабличніСписки.ВидиЗапасів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиЗапасів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиЗапасів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиЗапасів_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиЗапасів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВидиЗапасів_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВидиЗапасів_Записи.ДодатиВідбір(TreeViewGrid, ВидиЗапасів_Функції.Відбори(searchText), true);

            await ТабличніСписки.ВидиЗапасів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ВидиЗапасів_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ВидиЗапасів_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ВидиЗапасів_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ВидиЗапасів_Функції.Copy(unigueID);
        }

        #endregion
    }
}
