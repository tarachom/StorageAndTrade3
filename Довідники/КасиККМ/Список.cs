
/*     
        КасиККМ.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class КасиККМ : ДовідникЖурнал
    {
        

        public КасиККМ() : base()
        {
            ТабличніСписки.КасиККМ_Записи.AddColumns(TreeViewGrid);
            
        }

        #region Override

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
        
        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.КасиККМ_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.КасиККМ_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.КасиККМ_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await КасиККМ_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await КасиККМ_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await КасиККМ_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new КасиККМ_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, КасиККМ_Const.POINTER);
             await BeforeLoadRecords();
        }

        #endregion
    }
}
    