
/*     
        ХарактеристикиНоменклатури_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
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

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            //Відбори
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ХарактеристикиНоменклатури_Функції.Відбори(searchText));

            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
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

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ХарактеристикиНоменклатури_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer, Власник.Pointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ХарактеристикиНоменклатури_Функції.SetDeletionLabel(unigueID);
        }
    }
}
