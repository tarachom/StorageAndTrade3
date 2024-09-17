
/*     
        Номенклатура_Папки.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Номенклатура_Папки : ДовідникЖурнал
    {
        public Номенклатура_Папки() : base()
        {
            ТабличніСписки.Номенклатура_Папки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Номенклатура_Папки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Номенклатура_Папки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Номенклатура_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Номенклатура_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Номенклатура_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Номенклатура_Папки_Записи.ДодатиВідбір(TreeViewGrid, Номенклатура_Папки_Функції.Відбори(searchText), true);

            await ТабличніСписки.Номенклатура_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Номенклатура_Папки_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Номенклатура_Папки_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Номенклатура_Папки_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Номенклатура_Папки_Функції.Copy(unigueID);
        }

        #endregion
    }
}
