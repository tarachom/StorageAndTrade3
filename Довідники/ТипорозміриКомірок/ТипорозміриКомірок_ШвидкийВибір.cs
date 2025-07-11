
/*     
        ТипорозміриКомірок_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ТипорозміриКомірок_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public ТипорозміриКомірок_ШвидкийВибір() : base()
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ТипорозміриКомірок_Записи.ДодатиВідбір(TreeViewGrid, ТипорозміриКомірок_Функції.Відбори(searchText), true);

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ТипорозміриКомірок page = new ТипорозміриКомірок()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ТипорозміриКомірок_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ТипорозміриКомірок_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ТипорозміриКомірок_Функції.SetDeletionLabel(unigueID);
        }
    }
}
