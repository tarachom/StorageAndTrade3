
/*     
        СкладськіКомірки_Папки.cs
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
    public class СкладськіКомірки_Папки : ДовідникЖурнал
    {
        public СкладськіПриміщення_PointerControl Власник = new СкладськіПриміщення_PointerControl() { Caption = "Власник:" };

        public СкладськіКомірки_Папки() : base()
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.AddColumns(TreeViewGrid);

            HBoxTop.PackStart(Власник, false, false, 2);
            Власник.AfterSelectFunc = async () => await LoadRecords();
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UniqueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Папки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Папки_Const.Власник, Comparison.EQ, Власник.Pointer.UniqueID.UGuid));

            await ТабличніСписки.СкладськіКомірки_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UniqueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Папки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Папки_Const.Власник, Comparison.EQ, Власник.Pointer.UniqueID.UGuid));

            //Відбори
            ТабличніСписки.СкладськіКомірки_Папки_Записи.ДодатиВідбір(TreeViewGrid, СкладськіКомірки_Папки_Функції.Відбори(searchText));

            await ТабличніСписки.СкладськіКомірки_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.СкладськіКомірки_Папки_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.СкладськіКомірки_Папки_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await СкладськіКомірки_Папки_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords, null, Власник.Pointer);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await СкладськіКомірки_Папки_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await СкладськіКомірки_Папки_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new СкладськіКомірки_Папки_Pointer(uniqueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, СкладськіКомірки_Папки_Const.POINTER);
            if (!CompositeMode) await BeforeLoadRecords();
        }

        #endregion
    }
}
