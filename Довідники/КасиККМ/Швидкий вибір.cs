
/*     
        КасиККМ_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class КасиККМ_ШвидкийВибір : ДовідникШвидкийВибір
    {
        
        
        public КасиККМ_ШвидкийВибір() : base()
        {
            ТабличніСписки.КасиККМ_Записи.AddColumns(TreeViewGrid);
            
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.КасиККМ_Записи.ОчиститиВідбір(TreeViewGrid);
            
            await ТабличніСписки.КасиККМ_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.КасиККМ_Записи.ОчиститиВідбір(TreeViewGrid);
                        
            //Відбори
            ТабличніСписки.КасиККМ_Записи.ДодатиВідбір(TreeViewGrid, КасиККМ_Функції.Відбори(searchText));

            await ТабличніСписки.КасиККМ_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            КасиККМ page = new КасиККМ()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };
            
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, КасиККМ_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await  КасиККМ_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await КасиККМ_Функції.SetDeletionLabel(unigueID);
        }
    }
}
    