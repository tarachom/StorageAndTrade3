
/*     
        СтруктураПідприємства.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class СтруктураПідприємства : ДовідникЖурнал
    {
        public СтруктураПідприємства() : base()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СтруктураПідприємства_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СтруктураПідприємства_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СтруктураПідприємства_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СтруктураПідприємства_Записи.ДодатиВідбір(TreeViewGrid, СтруктураПідприємства_Функції.Відбори(searchText), true);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.СтруктураПідприємства_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СтруктураПідприємства_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СтруктураПідприємства_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СтруктураПідприємства_Функції.Copy(unigueID);
        }

        #endregion
    }
}
