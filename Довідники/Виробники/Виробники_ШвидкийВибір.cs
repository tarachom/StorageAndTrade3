
/*     
        Виробники_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Виробники_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Виробники_ШвидкийВибір() : base()
        {
            ТабличніСписки.Виробники_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Виробники_Записи.SelectPointerItem = null;
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

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Виробники page = new Виробники()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Виробники_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Виробники_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Виробники_Функції.SetDeletionLabel(unigueID);
        }
    }
}
