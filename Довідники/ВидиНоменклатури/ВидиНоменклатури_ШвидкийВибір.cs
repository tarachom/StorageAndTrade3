
/*     
        ВидиНоменклатури_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ВидиНоменклатури_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public ВидиНоменклатури_ШвидкийВибір() : base()
        {
            ТабличніСписки.ВидиНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиНоменклатури_Записи.SelectPointerItem = null;
            ТабличніСписки.ВидиНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВидиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВидиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ВидиНоменклатури_Функції.Відбори(searchText), true);

            await ТабличніСписки.ВидиНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ВидиНоменклатури page = new ВидиНоменклатури()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ВидиНоменклатури_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ВидиНоменклатури_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ВидиНоменклатури_Функції.SetDeletionLabel(unigueID);
        }
    }
}
