
/*     
        ВидиЦін.cs
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
    public class ВидиЦін : ДовідникЖурнал
    {
        public ВидиЦін() : base()
        {
            ТабличніСписки.ВидиЦін_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиЦін_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиЦін_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиЦін_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиЦін_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВидиЦін_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВидиЦін_Записи.ДодатиВідбір(TreeViewGrid, ВидиЦін_Функції.Відбори(searchText), true);

            await ТабличніСписки.ВидиЦін_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ВидиЦін_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ВидиЦін_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ВидиЦін_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ВидиЦін_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ВидиЦін_Const.POINTER);
            await LoadRecords();
        }

        #endregion
    }
}
