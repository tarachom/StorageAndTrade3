
/*     
        БанківськіРахункиКонтрагентів.cs
        Список
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    public class БанківськіРахункиКонтрагентів : ДовідникЖурнал
    {
        public БанківськіРахункиКонтрагентів() : base()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid, БанківськіРахункиКонтрагентів_Функції.Відбори(searchText), true);

            await ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.БанківськіРахункиКонтрагентів_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await БанківськіРахункиКонтрагентів_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await БанківськіРахункиКонтрагентів_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await БанківськіРахункиКонтрагентів_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new БанківськіРахункиКонтрагентів_Pointer(uniqueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, БанківськіРахункиКонтрагентів_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
