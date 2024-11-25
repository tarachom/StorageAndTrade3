
/*     
        ВидиЦінПостачальників_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ВидиЦінПостачальників_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public ВидиЦінПостачальників_ШвидкийВибір() : base()
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.SelectPointerItem = null;
            ТабличніСписки.ВидиЦінПостачальників_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиЦінПостачальників_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиЦінПостачальників_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВидиЦінПостачальників_Записи.ДодатиВідбір(TreeViewGrid, ВидиЦінПостачальників_Функції.Відбори(searchText), true);

            await ТабличніСписки.ВидиЦінПостачальників_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ВидиЦінПостачальників page = new ВидиЦінПостачальників()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ВидиЦінПостачальників_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ВидиЦінПостачальників_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ВидиЦінПостачальників_Функції.SetDeletionLabel(unigueID);
        }
    }
}
