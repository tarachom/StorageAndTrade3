
/*     
        БанківськіРахункиОрганізацій_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class БанківськіРахункиОрганізацій_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public БанківськіРахункиОрганізацій_ШвидкийВибір() : base()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.SelectPointerItem = null;
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ДодатиВідбір(TreeViewGrid, БанківськіРахункиОрганізацій_Функції.Відбори(searchText), true);

            await ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            БанківськіРахункиОрганізацій page = new БанківськіРахункиОрганізацій()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, БанківськіРахункиОрганізацій_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await БанківськіРахункиОрганізацій_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await БанківськіРахункиОрганізацій_Функції.SetDeletionLabel(unigueID);
        }
    }
}
