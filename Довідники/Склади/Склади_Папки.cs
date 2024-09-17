
/*     
        Склади_Папки.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Склади_Папки : ДовідникЖурнал
    {
        public Склади_Папки() : base()
        {
            ТабличніСписки.Склади_Папки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Склади_Папки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Склади_Папки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Склади_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Склади_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Склади_Папки_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Склади_Папки_Записи.ДодатиВідбір(TreeViewGrid, Склади_Папки_Функції.Відбори(searchText), true);

            await ТабличніСписки.Склади_Папки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Склади_Папки_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Склади_Папки_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Склади_Папки_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Склади_Папки_Функції.Copy(unigueID);
        }

        #endregion
    }
}
