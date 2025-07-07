
/*     
        ВидиЦінПостачальників.cs
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
    public class ВидиЦінПостачальників : ДовідникЖурнал
    {
        public ВидиЦінПостачальників() : base()
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиЦінПостачальників_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВидиЦінПостачальників_Записи.ДодатиВідбір(TreeViewGrid, ВидиЦінПостачальників_Функції.Відбори(searchText), true);

            await ТабличніСписки.ВидиЦінПостачальників_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ВидиЦінПостачальників_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ВидиЦінПостачальників_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ВидиЦінПостачальників_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ВидиЦінПостачальників_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ВидиЦінПостачальників_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ВидиЦінПостачальників_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ВидиЦінПостачальників_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ВидиЦінПостачальників_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
