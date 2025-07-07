
/*     
        СтруктураПідприємства.cs
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
    public class СтруктураПідприємства : ДовідникЖурнал
    {
        public СтруктураПідприємства() : base()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СтруктураПідприємства_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СтруктураПідприємства_Записи.ДодатиВідбір(TreeViewGrid, СтруктураПідприємства_Функції.Відбори(searchText), true);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.СтруктураПідприємства_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.СтруктураПідприємства_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СтруктураПідприємства_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СтруктураПідприємства_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СтруктураПідприємства_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new СтруктураПідприємства_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, СтруктураПідприємства_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
