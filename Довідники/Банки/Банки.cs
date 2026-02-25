
/*     
        Банки.cs
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
    public class Банки : ДовідникЖурнал
    {
        public Банки() : base()
        {
            CreateLink(HBoxTop, "Завантаження списку банків", () => NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Завантаження списку Банків", () => new Обробка_ЗавантаженняБанків()));

            ТабличніСписки.Банки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Банки_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Банки_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Банки_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Банки_Записи.ДодатиВідбір(TreeViewGrid, Банки_Функції.Відбори(searchText), true);

            await ТабличніСписки.Банки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Банки_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.Банки_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.Банки_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await Банки_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await Банки_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await Банки_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new Банки_Pointer(uniqueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, Банки_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
