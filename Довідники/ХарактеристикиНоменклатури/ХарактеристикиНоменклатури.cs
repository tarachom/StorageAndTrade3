
/*     
        ХарактеристикиНоменклатури.cs
        Список
*/

using Gtk;
using InterfaceGtk;
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
                ШтрихкодиНоменклатури page = new ШтрихкодиНоменклатури();
                page.НоменклатураВласник.Pointer = Власник.Pointer;

                if (SelectPointerItem != null)
                    page.ХарактеристикиНоменклатуриВласник.Pointer = new ХарактеристикиНоменклатури_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ШтрихкодиНоменклатури_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            HBoxTop.PackStart(Власник, false, false, 2);
            Власник.AfterSelectFunc = async () => await LoadRecords();
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
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

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ХарактеристикиНоменклатури_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ХарактеристикиНоменклатури_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null, Власник.Pointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ХарактеристикиНоменклатури_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ХарактеристикиНоменклатури_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ХарактеристикиНоменклатури_Const.POINTER);
            await LoadRecords();
        }

        #endregion
    }
}
