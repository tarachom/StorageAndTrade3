
/*     
        СеріїНоменклатури.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class СеріїНоменклатури : ДовідникЖурнал
    {
        public СеріїНоменклатури() : base()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СеріїНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СеріїНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СеріїНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СеріїНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, СеріїНоменклатури_Функції.Відбори(searchText), true);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.СеріїНоменклатури_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СеріїНоменклатури_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СеріїНоменклатури_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СеріїНоменклатури_Функції.Copy(unigueID);
        }

        #endregion
    }
}
