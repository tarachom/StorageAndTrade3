
/*     
        Файли_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Файли_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Файли_ШвидкийВибір() : base()
        {
            ТабличніСписки.Файли_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Файли_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.Файли_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Файли_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Файли_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Файли_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Файли_Записи.ДодатиВідбір(TreeViewGrid, Файли_Функції.Відбори(searchText), true);

            await ТабличніСписки.Файли_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Файли page = new Файли()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Файли_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Файли_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Файли_Функції.SetDeletionLabel(unigueID);
        }
    }
}
