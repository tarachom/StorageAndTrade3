
/*     
        ТипорозміриКомірок.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ТипорозміриКомірок : ДовідникЖурнал
    {
        public ТипорозміриКомірок() : base()
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ТипорозміриКомірок_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ТипорозміриКомірок_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ТипорозміриКомірок_Записи.ДодатиВідбір(TreeViewGrid, ТипорозміриКомірок_Функції.Відбори(searchText), true);

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ТипорозміриКомірок_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ТипорозміриКомірок_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ТипорозміриКомірок_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ТипорозміриКомірок_Функції.Copy(unigueID);
        }

        #endregion
    }
}
