
/*     
        ФізичніОсоби.cs
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
    public class ФізичніОсоби : ДовідникЖурнал
    {
        public ФізичніОсоби() : base()
        {
            ТабличніСписки.ФізичніОсоби_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ФізичніОсоби_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ФізичніОсоби_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ФізичніОсоби_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ФізичніОсоби_Записи.ДодатиВідбір(TreeViewGrid, ФізичніОсоби_Функції.Відбори(searchText), true);

            await ТабличніСписки.ФізичніОсоби_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ФізичніОсоби_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ФізичніОсоби_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ФізичніОсоби_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ФізичніОсоби_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ФізичніОсоби_Const.POINTER);
            await LoadRecords();
        }

        #endregion
    }
}
