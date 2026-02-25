/*

        Каси_ШвидкийВибір.cs
        ШвидкийВибір

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Каси_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Каси_ШвидкийВибір()
        {
            ТабличніСписки.Каси_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Каси_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Каси_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.Каси_Записи.ДодатиВідбір(TreeViewGrid, Каси_Функції.Відбори(searchText), true);

            await ТабличніСписки.Каси_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UniqueID? uniqueID = null)
        {
            Каси page = new Каси()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {Каси_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await Каси_Функції.OpenPageElement(IsNew, uniqueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await Каси_Функції.SetDeletionLabel(uniqueID);
        }
    }
}