
/*     
        СкладськіКомірки_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class СкладськіКомірки_ШвидкийВибір : ДовідникШвидкийВибір
    {

        public СкладськіПриміщення_PointerControl Власник = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення:" };


        public СкладськіКомірки_ШвидкийВибір() : base()
        {
            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            HBoxTop.PackStart(Власник, false, false, 2); //Власник
            Власник.AfterSelectFunc = async () => await LoadRecords();

        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            await ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            //Відбори
            ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid, СкладськіКомірки_Функції.Відбори(searchText));

            await ТабличніСписки.СкладськіКомірки_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            СкладськіКомірки page = new СкладськіКомірки()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            page.Власник.Pointer = Власник.Pointer;

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, СкладськіКомірки_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СкладськіКомірки_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer, Власник.Pointer);

        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СкладськіКомірки_Функції.SetDeletionLabel(unigueID);

        }
    }
}
