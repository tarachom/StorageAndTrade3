
/*     
        СкладськіКомірки_Папки.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

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

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СкладськіКомірки_Папки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіКомірки_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Папки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Папки_Const.Власник, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            await ТабличніСписки.СкладськіКомірки_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіКомірки_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Папки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Папки_Const.Власник, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            //Відбори
            ТабличніСписки.СкладськіКомірки_Папки_Записи.ДодатиВідбір(TreeViewGrid, СкладськіКомірки_Папки_Функції.Відбори(searchText));

            await ТабличніСписки.СкладськіКомірки_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.СкладськіКомірки_Папки_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СкладськіКомірки_Папки_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null, Власник.Pointer);

        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СкладськіКомірки_Папки_Функції.SetDeletionLabel(unigueID);

        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СкладськіКомірки_Папки_Функції.Copy(unigueID);

        }

        #endregion
    }
}
