/*     
        Контрагенти_Папки_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Контрагенти_Папки_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Контрагенти_Папки_ШвидкийВибір() : base()
        {
            ТабличніСписки.Контрагенти_Папки_Записи.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Контрагенти_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Контрагенти_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Контрагенти_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Контрагенти_Папки_Записи.ДодатиВідбір(TreeViewGrid, Контрагенти_Папки_Функції.Відбори(searchText), true);

            await ТабличніСписки.Контрагенти_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UniqueID? uniqueID = null)
        {
            Контрагенти_Папки page = new Контрагенти_Папки()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Контрагенти_Папки_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await Контрагенти_Папки_Функції.OpenPageElement(IsNew, uniqueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await Контрагенти_Папки_Функції.SetDeletionLabel(uniqueID);
        }
    }
}
