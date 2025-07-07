
/*     
        СтаттяРухуКоштів_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class СтаттяРухуКоштів_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public СтаттяРухуКоштів_ШвидкийВибір() : base()
        {
            ТабличніСписки.СтаттяРухуКоштів_Записи.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СтаттяРухуКоштів_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СтаттяРухуКоштів_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СтаттяРухуКоштів_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СтаттяРухуКоштів_Записи.ДодатиВідбір(TreeViewGrid, СтаттяРухуКоштів_Функції.Відбори(searchText), true);

            await ТабличніСписки.СтаттяРухуКоштів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            СтаттяРухуКоштів page = new СтаттяРухуКоштів()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, СтаттяРухуКоштів_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СтаттяРухуКоштів_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СтаттяРухуКоштів_Функції.SetDeletionLabel(unigueID);
        }
    }
}
