/*

        ДоговориКонтрагентів

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ДоговориКонтрагентів : ДовідникЖурнал
    {
        public Контрагенти_PointerControl КонтрагентВласник = new Контрагенти_PointerControl();

        public ДоговориКонтрагентів()
        {
            //Власник
            HBoxTop.PackStart(КонтрагентВласник, false, false, 2);
            КонтрагентВласник.Caption = $"{Контрагенти_Const.FULLNAME}:";
            КонтрагентВласник.AfterSelectFunc = async () => await BeforeLoadRecords();

            ТабличніСписки.ДоговориКонтрагентів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ДоговориКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.ДоговориКонтрагентів_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ДоговориКонтрагентів_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid, ДоговориКонтрагентів_Функції.Відбори(searchText));

            await ТабличніСписки.ДоговориКонтрагентів_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ДоговориКонтрагентів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ДоговориКонтрагентів_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ДоговориКонтрагентів_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ДоговориКонтрагентів_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ДоговориКонтрагентів_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ДоговориКонтрагентів_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}