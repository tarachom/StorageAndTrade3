
/*     
        ВидиЗапасів_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ВидиЗапасів_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public ВидиЗапасів_ШвидкийВибір() : base()
        {
            ТабличніСписки.ВидиЗапасів_Записи.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиЗапасів_Записи.SelectPointerItem = null;
            ТабличніСписки.ВидиЗапасів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиЗапасів_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиЗапасів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВидиЗапасів_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВидиЗапасів_Записи.ДодатиВідбір(TreeViewGrid, ВидиЗапасів_Функції.Відбори(searchText), true);

            await ТабличніСписки.ВидиЗапасів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ВидиЗапасів page = new ВидиЗапасів()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ВидиЗапасів_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ВидиЗапасів_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ВидиЗапасів_Функції.SetDeletionLabel(unigueID);
        }
    }
}
