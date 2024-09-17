
/*     
        Організації_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Організації_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Організації_ШвидкийВибір() : base()
        {
            ТабличніСписки.Організації_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Організації_Записи.SelectPointerItem = null;
            ТабличніСписки.Організації_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Організації_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Організації_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Організації_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Організації_Записи.ДодатиВідбір(TreeViewGrid, Організації_Функції.Відбори(searchText), true);

            await ТабличніСписки.Організації_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Організації page = new Організації()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Організації_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Організації_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Організації_Функції.SetDeletionLabel(unigueID);
        }
    }
}
