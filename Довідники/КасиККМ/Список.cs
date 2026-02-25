
/*     
        КасиККМ.cs
        Список
*/

using Gtk;
using InterfaceGtk3;
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

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await КасиККМ_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await КасиККМ_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await КасиККМ_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new КасиККМ_Pointer(uniqueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, КасиККМ_Const.POINTER);
             await BeforeLoadRecords();
        }

        #endregion
    }
}
    