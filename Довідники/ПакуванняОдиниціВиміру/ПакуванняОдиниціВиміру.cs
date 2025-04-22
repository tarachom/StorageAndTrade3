
/*     
        ПакуванняОдиниціВиміру.cs
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
    public class ПакуванняОдиниціВиміру : ДовідникЖурнал
    {
        public ПакуванняОдиниціВиміру() : base()
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ПакуванняОдиниціВиміру_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ДодатиВідбір(TreeViewGrid, ПакуванняОдиниціВиміру_Функції.Відбори(searchText), true);

            await ТабличніСписки.ПакуванняОдиниціВиміру_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ПакуванняОдиниціВиміру_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПакуванняОдиниціВиміру_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПакуванняОдиниціВиміру_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ПакуванняОдиниціВиміру_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ПакуванняОдиниціВиміру_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
