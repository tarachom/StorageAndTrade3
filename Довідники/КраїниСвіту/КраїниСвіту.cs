
/*     
        КраїниСвіту.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    public class КраїниСвіту : ДовідникЖурнал
    {
        public КраїниСвіту() : base()
        {
            ТабличніСписки.КраїниСвіту_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.КраїниСвіту_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.КраїниСвіту_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.КраїниСвіту_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.КраїниСвіту_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.КраїниСвіту_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.КраїниСвіту_Записи.ДодатиВідбір(TreeViewGrid, КраїниСвіту_Функції.Відбори(searchText), true);

            await ТабличніСписки.КраїниСвіту_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.КраїниСвіту_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await КраїниСвіту_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await КраїниСвіту_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await КраїниСвіту_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, КраїниСвіту_Const.POINTER);
            await LoadRecords();
        }

        #endregion
    }
}
