
/*     
        Контрагенти_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Контрагенти_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Контрагенти_ШвидкийВибір() : base()
        {
            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Контрагенти_Записи.ДодатиВідбір(TreeViewGrid, Контрагенти_Функції.Відбори(searchText), true);

            await ТабличніСписки.Контрагенти_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Контрагенти page = new Контрагенти()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Контрагенти_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Контрагенти_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Контрагенти_Функції.SetDeletionLabel(unigueID);
        }
    }
}
