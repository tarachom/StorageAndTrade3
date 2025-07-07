
/*     
        Блокнот_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Блокнот_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Блокнот_ШвидкийВибір() : base()
        {
            ТабличніСписки.Блокнот_Записи.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Блокнот_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Блокнот_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Блокнот_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Блокнот_Записи.ДодатиВідбір(TreeViewGrid, Блокнот_Функції.Відбори(searchText), true);

            await ТабличніСписки.Блокнот_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Блокнот page = new Блокнот()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Блокнот_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Блокнот_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Блокнот_Функції.SetDeletionLabel(unigueID);
        }
    }
}
