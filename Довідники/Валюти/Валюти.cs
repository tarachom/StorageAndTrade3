
/*     
        Валюти.cs
        Список
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриВідомостей;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Валюти : ДовідникЖурнал
    {
        public Валюти() : base()
        {
            CreateLink(HBoxTop, КурсиВалют_Const.FULLNAME, async () =>
            {
                РегістриВідомостей.КурсиВалют page = new РегістриВідомостей.КурсиВалют();
                page.ВалютаВласник.Pointer = new Валюти_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID());

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, КурсиВалют_Const.FULLNAME, () => page);
                await page.SetValue();
            });

            //Завантаження курсів валют НБУ
            CreateLink(HBoxTop, "Завантаження курсів валют НБУ", () =>
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Завантаження курсів валют НБУ", () => new Обробка_ЗавантаженняКурсівВалют()));

            ТабличніСписки.Валюти_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Валюти_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Валюти_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Валюти_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Валюти_Записи.ДодатиВідбір(TreeViewGrid, Валюти_Функції.Відбори(searchText), true);

            await ТабличніСписки.Валюти_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Валюти_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.Валюти_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.Валюти_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Валюти_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Валюти_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Валюти_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new Валюти_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, Валюти_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
