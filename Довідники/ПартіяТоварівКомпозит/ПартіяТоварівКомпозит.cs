
/*     
        ПартіяТоварівКомпозит.cs
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
    public class ПартіяТоварівКомпозит : ДовідникЖурнал
    {
        public ПартіяТоварівКомпозит() : base()
        {
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПартіяТоварівКомпозит_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ПартіяТоварівКомпозит_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.ДодатиВідбір(TreeViewGrid, ПартіяТоварівКомпозит_Функції.Відбори(searchText), true);

            await ТабличніСписки.ПартіяТоварівКомпозит_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ПартіяТоварівКомпозит_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПартіяТоварівКомпозит_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПартіяТоварівКомпозит_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ПартіяТоварівКомпозит_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ПартіяТоварівКомпозит_Const.POINTER);
            await LoadRecords();
        }

        #endregion
    }
}
