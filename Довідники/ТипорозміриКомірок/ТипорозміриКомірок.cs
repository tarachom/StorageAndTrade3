
/*     
        ТипорозміриКомірок.cs
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
    public class ТипорозміриКомірок : ДовідникЖурнал
    {
        public ТипорозміриКомірок() : base()
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ТипорозміриКомірок_Записи.ДодатиВідбір(TreeViewGrid, ТипорозміриКомірок_Функції.Відбори(searchText), true);

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ТипорозміриКомірок_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await ТипорозміриКомірок_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await ТипорозміриКомірок_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await ТипорозміриКомірок_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ТипорозміриКомірок_Pointer(uniqueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ТипорозміриКомірок_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
