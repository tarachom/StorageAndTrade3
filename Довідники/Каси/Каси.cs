
/*     
        Каси.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Каси : ДовідникЖурнал
    {
        public Каси() : base()
        {
            ТабличніСписки.Каси_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Каси_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Каси_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Каси_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Каси_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Каси_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Каси_Записи.ДодатиВідбір(TreeViewGrid, Каси_Функції.Відбори(searchText));

            await ТабличніСписки.Каси_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.Каси_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Каси_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Каси_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Каси_Функції.Copy(unigueID);
        }

        #endregion
    }
}