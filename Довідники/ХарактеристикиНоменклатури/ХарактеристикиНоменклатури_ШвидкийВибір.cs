
/*     
        ХарактеристикиНоменклатури_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ХарактеристикиНоменклатури_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Номенклатура_PointerControl НоменклатураВласник = new Номенклатура_PointerControl() { WidthPresentation = 300 };

        public ХарактеристикиНоменклатури_ШвидкийВибір()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.AddColumns(TreeViewGrid);

            //Власник
            HBoxTop.PackStart(НоменклатураВласник, false, false, 2);
            НоменклатураВласник.Caption = $"{Номенклатура_Const.FULLNAME}:";
            НоменклатураВласник.AfterSelectFunc = async () => { await LoadRecords(); };
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.SelectPointerItem = null;
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ХарактеристикиНоменклатури_Функції.Відбори(searchText));

            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            page.НоменклатураВласник.Pointer = НоменклатураВласник.Pointer;

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ХарактеристикиНоменклатури_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
            {
                IsNew = IsNew,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer
            };

            if (IsNew)
            {
                await page.Елемент.New();
                page.НоменклатураДляНового = НоменклатураВласник.Pointer;
            }
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ХарактеристикиНоменклатури_Функції.SetDeletionLabel(unigueID);
        }
    }
}