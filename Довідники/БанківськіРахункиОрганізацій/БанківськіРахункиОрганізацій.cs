
/*     
        БанківськіРахункиОрганізацій.cs
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
    public class БанківськіРахункиОрганізацій : ДовідникЖурнал
    {
        public БанківськіРахункиОрганізацій() : base()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.ДодатиВідбір(TreeViewGrid, БанківськіРахункиОрганізацій_Функції.Відбори(searchText), true);

            await ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.БанківськіРахункиОрганізацій_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await БанківськіРахункиОрганізацій_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await БанківськіРахункиОрганізацій_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await БанківськіРахункиОрганізацій_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, БанківськіРахункиОрганізацій_Const.POINTER);
            await LoadRecords();
        }

        #endregion
    }
}
