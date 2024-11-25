
/*     
        СеріїНоменклатури_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class СеріїНоменклатури_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public СеріїНоменклатури_ШвидкийВибір() : base()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.SelectPointerItem = null;
            ТабличніСписки.СеріїНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СеріїНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СеріїНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СеріїНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, СеріїНоменклатури_Функції.Відбори(searchText), true);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            СеріїНоменклатури page = new СеріїНоменклатури()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, СеріїНоменклатури_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СеріїНоменклатури_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СеріїНоменклатури_Функції.SetDeletionLabel(unigueID);
        }
    }
}
