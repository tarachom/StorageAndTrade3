/*     
        Контрагенти_Папки.cs
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
    public class Контрагенти_Папки : ДовідникЖурнал
    {
        public Контрагенти_Папки() : base()
        {
            ТабличніСписки.Контрагенти_Папки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Контрагенти_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Контрагенти_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Контрагенти_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Контрагенти_Папки_Записи.ДодатиВідбір(TreeViewGrid, Контрагенти_Папки_Функції.Відбори(searchText), true);

            await ТабличніСписки.Контрагенти_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Контрагенти_Папки_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.Контрагенти_Папки_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.Контрагенти_Папки_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Контрагенти_Папки_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Контрагенти_Папки_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Контрагенти_Папки_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new Контрагенти_Папки_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, Контрагенти_Папки_Const.POINTER);
            if (!CompositeMode) await BeforeLoadRecords();
        }

        #endregion
    }
}
