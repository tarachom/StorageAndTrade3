
/*     
        СеріїНоменклатури.cs
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
    public class СеріїНоменклатури : ДовідникЖурнал
    {
        public СеріїНоменклатури() : base()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СеріїНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СеріїНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, СеріїНоменклатури_Функції.Відбори(searchText), true);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.СеріїНоменклатури_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.СеріїНоменклатури_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СеріїНоменклатури_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СеріїНоменклатури_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СеріїНоменклатури_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new СеріїНоменклатури_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, СеріїНоменклатури_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
