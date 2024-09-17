
/*     
        СкладськіПриміщення.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class СкладськіПриміщення : ДовідникЖурнал
    {
        public Склади_PointerControl СкладВласник = new Склади_PointerControl() { Caption = "Склад:" };

        public СкладськіПриміщення()
        {
            //Власник
            HBoxTop.PackStart(СкладВласник, false, false, 2);
            СкладВласник.AfterSelectFunc = async () => await LoadRecords();

            CreateLink(HBoxTop, СкладськіКомірки_Const.FULLNAME, async () =>
            {
                СкладськіКомірки page = new СкладськіКомірки();

                if (SelectPointerItem != null)
                    page.Власник.Pointer = new СкладськіПриміщення_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{СкладськіКомірки_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            ТабличніСписки.СкладськіПриміщення_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіПриміщення_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СкладськіПриміщення_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіПриміщення_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіПриміщення_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid, СкладськіПриміщення_Функції.Відбори(searchText));

            await ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.СкладськіПриміщення_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СкладськіПриміщення_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СкладськіПриміщення_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СкладськіПриміщення_Функції.Copy(unigueID);
        }

        #endregion
    }
}