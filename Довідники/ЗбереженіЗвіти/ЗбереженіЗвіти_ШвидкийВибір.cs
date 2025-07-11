
/*     
        ЗбереженіЗвіти_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ЗбереженіЗвіти_ШвидкийВибір : ДовідникШвидкийВибір
    {


        public ЗбереженіЗвіти_ШвидкийВибір() : base()
        {
            ТабличніСписки.ЗбереженіЗвіти_Записи.AddColumns(TreeViewGrid);

        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗбереженіЗвіти_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ЗбереженіЗвіти_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗбереженіЗвіти_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ЗбереженіЗвіти_Записи.ДодатиВідбір(TreeViewGrid, ЗбереженіЗвіти_Функції.Відбори(searchText));

            await ТабличніСписки.ЗбереженіЗвіти_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ЗбереженіЗвіти page = new ЗбереженіЗвіти()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ЗбереженіЗвіти_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ЗбереженіЗвіти_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);

        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ЗбереженіЗвіти_Функції.SetDeletionLabel(unigueID);

        }
    }
}
