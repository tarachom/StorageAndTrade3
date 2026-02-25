
/*     
        ХарактеристикиНоменклатури.cs
        Список
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;
using GeneratedCode.РегістриВідомостей;

namespace StorageAndTrade
{
    public class ХарактеристикиНоменклатури : ДовідникЖурнал
    {
        public Номенклатура_PointerControl Власник = new Номенклатура_PointerControl() { Caption = "Номенклатура:" };

        public ХарактеристикиНоменклатури() : base()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.AddColumns(TreeViewGrid);

            CreateLink(HBoxTop, ШтрихкодиНоменклатури_Const.FULLNAME, async () =>
            {
                РегістриВідомостей.ШтрихкодиНоменклатури page = new РегістриВідомостей.ШтрихкодиНоменклатури();
                page.НоменклатураВласник.Pointer = Власник.Pointer;

                if (SelectPointerItem != null)
                    page.ХарактеристикиНоменклатуриВласник.Pointer = new ХарактеристикиНоменклатури_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ШтрихкодиНоменклатури_Const.FULLNAME}", () => page);
                await page.SetValue();
            });

            HBoxTop.PackStart(Власник, false, false, 2);
            Власник.AfterSelectFunc = async () => await BeforeLoadRecords();
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UniqueID.IsEmpty())
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, Власник.Pointer.UniqueID.UGuid));

            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UniqueID.IsEmpty())
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, Власник.Pointer.UniqueID.UGuid));

            //Відбори
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ХарактеристикиНоменклатури_Функції.Відбори(searchText));

            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await ХарактеристикиНоменклатури_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords, null, Власник.Pointer);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await ХарактеристикиНоменклатури_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await ХарактеристикиНоменклатури_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ХарактеристикиНоменклатури_Pointer(uniqueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ХарактеристикиНоменклатури_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
