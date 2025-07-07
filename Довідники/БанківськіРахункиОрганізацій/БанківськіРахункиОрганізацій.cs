
/*     
        БанківськіРахункиОрганізацій.cs
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
    public class БанківськіРахункиОрганізацій : ДовідникЖурнал
    {
        public БанківськіРахункиОрганізацій() : base()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ДодатиВідбір(TreeViewGrid, БанківськіРахункиОрганізацій_Функції.Відбори(searchText), true);

            await ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.БанківськіРахункиОрганізацій_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await БанківськіРахункиОрганізацій_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await БанківськіРахункиОрганізацій_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await БанківськіРахункиОрганізацій_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new БанківськіРахункиОрганізацій_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, БанківськіРахункиОрганізацій_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
