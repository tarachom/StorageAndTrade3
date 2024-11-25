
/*     
        Користувачі_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Користувачі_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Користувачі_ШвидкийВибір() : base()
        {
            ТабличніСписки.Користувачі_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Користувачі_Записи.SelectPointerItem = null;
            ТабличніСписки.Користувачі_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Користувачі_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Користувачі_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Користувачі_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Користувачі_Записи.ДодатиВідбір(TreeViewGrid, Користувачі_Функції.Відбори(searchText), true);

            await ТабличніСписки.Користувачі_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Користувачі page = new Користувачі()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Користувачі_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Користувачі_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Користувачі_Функції.SetDeletionLabel(unigueID);
        }
    }
}
