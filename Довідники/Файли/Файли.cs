
/*     
        Файли.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    public class Файли : ДовідникЖурнал
    {
        public Файли() : base()
        {
            ТабличніСписки.Файли_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Файли_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Файли_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Файли_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Файли_Записи.ДодатиВідбір(TreeViewGrid, Файли_Функції.Відбори(searchText), true);

            await ТабличніСписки.Файли_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Файли_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.Файли_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Файли_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Файли_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Файли_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, Файли_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
