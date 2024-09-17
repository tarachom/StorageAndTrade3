
/*     
        Банки.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Банки : ДовідникЖурнал
    {
        public Банки() : base()
        {
            CreateLink(HBoxTop, "Завантаження списку банків", () =>
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Завантаження списку Банків", () => new Обробка_ЗавантаженняБанків());
            });

            ТабличніСписки.Банки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Банки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Банки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Банки_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Банки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Банки_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Банки_Записи.ДодатиВідбір(TreeViewGrid, Банки_Функції.Відбори(searchText), true);

            await ТабличніСписки.Банки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Банки_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Банки_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Банки_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Банки_Функції.Copy(unigueID);
        }

        #endregion
    }
}
