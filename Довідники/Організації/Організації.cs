
/*     
        Організації.cs
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
    public class Організації : ДовідникЖурнал
    {
        public Організації() : base()
        {
            ТабличніСписки.Організації_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Організації_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Організації_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Організації_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Організації_Записи.ДодатиВідбір(TreeViewGrid, Організації_Функції.Відбори(searchText), true);

            await ТабличніСписки.Організації_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Організації_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.Організації_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.Організації_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await Організації_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await Організації_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await Організації_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new Організації_Pointer(uniqueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, Організації_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
