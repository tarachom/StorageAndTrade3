
/*     
        Склади_Папки_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Склади_Папки_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Склади_Папки_ШвидкийВибір() : base()
        {
            ТабличніСписки.Склади_Папки_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Склади_Папки_Записи.SelectPointerItem = null;
            ТабличніСписки.Склади_Папки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Склади_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Склади_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Склади_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Склади_Папки_Записи.ДодатиВідбір(TreeViewGrid, Склади_Папки_Функції.Відбори(searchText), true);

            await ТабличніСписки.Склади_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Склади_Папки page = new Склади_Папки()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Склади_Папки_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Склади_Папки_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Склади_Папки_Функції.SetDeletionLabel(unigueID);
        }
    }
}
