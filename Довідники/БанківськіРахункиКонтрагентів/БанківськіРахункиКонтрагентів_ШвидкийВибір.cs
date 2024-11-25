
/*     
        БанківськіРахункиКонтрагентів_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class БанківськіРахункиКонтрагентів_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public БанківськіРахункиКонтрагентів_ШвидкийВибір() : base()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.SelectPointerItem = null;
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

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            БанківськіРахункиКонтрагентів page = new БанківськіРахункиКонтрагентів()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, БанківськіРахункиКонтрагентів_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await БанківськіРахункиКонтрагентів_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await БанківськіРахункиКонтрагентів_Функції.SetDeletionLabel(unigueID);
        }
    }
}
