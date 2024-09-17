
/*     
        Валюти_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Валюти_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Валюти_ШвидкийВибір() : base()
        {
            ТабличніСписки.Валюти_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Валюти_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.Валюти_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Валюти_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Валюти_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Валюти_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Валюти_Записи.ДодатиВідбір(TreeViewGrid, Валюти_Функції.Відбори(searchText), true);

            await ТабличніСписки.Валюти_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Валюти page = new Валюти()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, Валюти_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Валюти_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Валюти_Функції.SetDeletionLabel(unigueID);
        }
    }
}
