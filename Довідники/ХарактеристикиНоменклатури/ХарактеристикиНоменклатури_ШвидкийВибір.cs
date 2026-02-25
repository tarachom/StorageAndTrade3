
/*     
        ХарактеристикиНоменклатури_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ХарактеристикиНоменклатури_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Номенклатура_PointerControl Власник = new Номенклатура_PointerControl() { Caption = "Номенклатура:" };

        public ХарактеристикиНоменклатури_ШвидкийВибір() : base()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.AddColumns(TreeViewGrid);

            HBoxTop.PackStart(Власник, false, false, 2); //Власник
            Власник.AfterSelectFunc = async () => await BeforeLoadRecords();
        }

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

        protected override async ValueTask OpenPageList(UniqueID? uniqueID = null)
        {
            ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            page.Власник.Pointer = Власник.Pointer;

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ХарактеристикиНоменклатури_Const.FULLNAME, () => page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await ХарактеристикиНоменклатури_Функції.OpenPageElement(IsNew, uniqueID, null, CallBack_OnSelectPointer, Власник.Pointer);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await ХарактеристикиНоменклатури_Функції.SetDeletionLabel(uniqueID);
        }
    }
}
