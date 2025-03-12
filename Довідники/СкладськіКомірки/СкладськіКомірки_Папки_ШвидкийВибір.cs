
/*     
        СкладськіКомірки_Папки_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class СкладськіКомірки_Папки_ШвидкийВибір : ДовідникШвидкийВибір
    {
        
        public СкладськіПриміщення_PointerControl Власник = new СкладськіПриміщення_PointerControl() { Caption = "Власник:" };
        
        
        public СкладськіКомірки_Папки_ШвидкийВибір() : base()
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.AddColumns(TreeViewGrid);
            
            HBoxTop.PackStart(Власник, false, false, 2); //Власник
            Власник.AfterSelectFunc = async () => await LoadRecords();
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.SelectPointerItem = null;
            ТабличніСписки.СкладськіКомірки_Папки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіКомірки_Папки_Записи.ОчиститиВідбір(TreeViewGrid);
            
            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Папки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Папки_Const.Власник, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));
            
            await ТабличніСписки.СкладськіКомірки_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.ОчиститиВідбір(TreeViewGrid);
            
            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Папки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Папки_Const.Власник, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));
                        
            //Відбори
            ТабличніСписки.СкладськіКомірки_Папки_Записи.ДодатиВідбір(TreeViewGrid, СкладськіКомірки_Папки_Функції.Відбори(searchText));

            await ТабличніСписки.СкладськіКомірки_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            СкладськіКомірки_Папки page = new СкладськіКомірки_Папки()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };
            
            page.Власник.Pointer = Власник.Pointer;
            
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, СкладськіКомірки_Папки_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await  СкладськіКомірки_Папки_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer, Власник.Pointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СкладськіКомірки_Папки_Функції.SetDeletionLabel(unigueID);
        }
    }
}
    