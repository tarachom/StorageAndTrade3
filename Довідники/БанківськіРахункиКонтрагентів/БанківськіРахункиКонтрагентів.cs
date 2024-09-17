
/*     
        БанківськіРахункиКонтрагентів.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class БанківськіРахункиКонтрагентів : ДовідникЖурнал
    {
        public БанківськіРахункиКонтрагентів() : base()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid, БанківськіРахункиКонтрагентів_Функції.Відбори(searchText), true);

            await ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await БанківськіРахункиКонтрагентів_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await БанківськіРахункиКонтрагентів_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await БанківськіРахункиКонтрагентів_Функції.Copy(unigueID);
        }

        #endregion
    }
}
