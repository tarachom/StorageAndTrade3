
/*     
        СеріїНоменклатури.cs
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
    public class СеріїНоменклатури : ДовідникЖурнал
    {
        public СеріїНоменклатури() : base()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.AddColumns(TreeViewGrid);
            ТабличніСписки.СеріїНоменклатури_Записи.Сторінки(TreeViewGrid, new Сторінки.Налаштування() { PageSize = 300, Тип = Сторінки.ТипЖурналу.Довідники });
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СеріїНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder);
            PagesShow(LoadRecords);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СеріїНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СеріїНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, СеріїНоменклатури_Функції.Відбори(searchText), true);

            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
            PagesShow(async () => await LoadRecords_OnSearch(searchText));
        }

        async ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.СеріїНоменклатури_Записи.LoadRecords(TreeViewGrid);
            PagesShow(LoadRecords_OnFilter);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.СеріїНоменклатури_Записи.CreateFilter(TreeViewGrid, () => PagesShow(LoadRecords_OnFilter));
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СеріїНоменклатури_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СеріїНоменклатури_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СеріїНоменклатури_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, СеріїНоменклатури_Const.POINTER);
            await LoadRecords();
        }

        #endregion
    }
}
