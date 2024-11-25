
/*     
        ПакуванняОдиниціВиміру_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ПакуванняОдиниціВиміру_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public ПакуванняОдиниціВиміру_ШвидкийВибір() : base()
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.SelectPointerItem = null;
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ПакуванняОдиниціВиміру_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ДодатиВідбір(TreeViewGrid, ПакуванняОдиниціВиміру_Функції.Відбори(searchText), true);

            await ТабличніСписки.ПакуванняОдиниціВиміру_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПакуванняОдиниціВиміру_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПакуванняОдиниціВиміру_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПакуванняОдиниціВиміру_Функції.SetDeletionLabel(unigueID);
        }
    }
}
