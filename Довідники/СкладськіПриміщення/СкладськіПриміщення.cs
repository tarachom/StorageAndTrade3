
/*     
        СкладськіПриміщення.cs
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
    public class СкладськіПриміщення : ДовідникЖурнал
    {
        public Склади_PointerControl СкладВласник = new Склади_PointerControl() { Caption = "Склад:" };

        public СкладськіПриміщення()
        {
            //Власник
            HBoxTop.PackStart(СкладВласник, false, false, 2);
            СкладВласник.AfterSelectFunc = async () => await BeforeLoadRecords();

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

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіПриміщення_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
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

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.СкладськіПриміщення_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.СкладськіПриміщення_Записи.CreateFilter(TreeViewGrid, filterControl);
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

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new СкладськіПриміщення_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, СкладськіПриміщення_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}