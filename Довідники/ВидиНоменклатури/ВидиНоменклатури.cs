
/*     
        ВидиНоменклатури.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ВидиНоменклатури : ДовідникЖурнал
    {
        public ВидиНоменклатури() : base()
        {
            ТабличніСписки.ВидиНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВидиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВидиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ВидиНоменклатури_Функції.Відбори(searchText), true);

            await ТабличніСписки.ВидиНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ВидиНоменклатури_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ВидиНоменклатури_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ВидиНоменклатури_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ВидиНоменклатури_Функції.Copy(unigueID);
        }

        #endregion
    }
}
