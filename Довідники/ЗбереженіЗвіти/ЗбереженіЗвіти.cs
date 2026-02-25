
/*     
        ЗбереженіЗвіти.cs
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
    public class ЗбереженіЗвіти : ДовідникЖурнал
    {
        Користувачі_PointerControl Власник = new Користувачі_PointerControl() { Caption = "Користувач:" };

        public ЗбереженіЗвіти() : base()
        {
            ТабличніСписки.ЗбереженіЗвіти_Записи.AddColumns(TreeViewGrid);

            HBoxTop.PackStart(Власник, false, false, 2);
            Власник.Pointer = Program.Користувач;
            Власник.AfterSelectFunc = async () => await BeforeLoadRecords();
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗбереженіЗвіти_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UniqueID.IsEmpty())
                ТабличніСписки.ЗбереженіЗвіти_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ЗбереженіЗвіти_Const.Користувач, Comparison.EQ, Власник.Pointer.UniqueID.UGuid));

            await ТабличніСписки.ЗбереженіЗвіти_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗбереженіЗвіти_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UniqueID.IsEmpty())
                ТабличніСписки.ЗбереженіЗвіти_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ЗбереженіЗвіти_Const.Користувач, Comparison.EQ, Власник.Pointer.UniqueID.UGuid));

            //Відбори
            ТабличніСписки.ЗбереженіЗвіти_Записи.ДодатиВідбір(TreeViewGrid, ЗбереженіЗвіти_Функції.Відбори(searchText));

            await ТабличніСписки.ЗбереженіЗвіти_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ЗбереженіЗвіти_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ЗбереженіЗвіти_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ЗбереженіЗвіти_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await ЗбереженіЗвіти_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await ЗбереженіЗвіти_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await ЗбереженіЗвіти_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ЗбереженіЗвіти_Pointer(uniqueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ЗбереженіЗвіти_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
