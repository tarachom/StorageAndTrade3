
/*     
        Виробники.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Виробники : ДовідникЖурнал
    {
        public Виробники() : base()
        {
            ТабличніСписки.Виробники_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Виробники_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Виробники_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Виробники_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Виробники_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Виробники_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Виробники_Записи.ДодатиВідбір(TreeViewGrid, Виробники_Функції.Відбори(searchText), true);

            await ТабличніСписки.Виробники_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Виробники_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Виробники_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Виробники_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Виробники_Функції.Copy(unigueID);
        }

        #endregion
    }
}
