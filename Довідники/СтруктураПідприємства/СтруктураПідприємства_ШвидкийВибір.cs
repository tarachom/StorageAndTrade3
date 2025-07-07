
/*     
        СтруктураПідприємства_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class СтруктураПідприємства_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public СтруктураПідприємства_ШвидкийВибір() : base()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СтруктураПідприємства_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СтруктураПідприємства_Записи.ДодатиВідбір(TreeViewGrid, СтруктураПідприємства_Функції.Відбори(searchText), true);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            СтруктураПідприємства page = new СтруктураПідприємства()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, СтруктураПідприємства_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СтруктураПідприємства_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СтруктураПідприємства_Функції.SetDeletionLabel(unigueID);
        }
    }
}
