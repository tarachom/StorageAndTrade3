
/*     
        Валюти.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриВідомостей;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Валюти : ДовідникЖурнал
    {
        public Валюти() : base()
        {
            CreateLink(HBoxTop, КурсиВалют_Const.FULLNAME, async () =>
            {
                КурсиВалют page = new КурсиВалют();
                page.ВалютаВласник.Pointer = new Валюти_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID());

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, КурсиВалют_Const.FULLNAME, () => page);
                await page.SetValue();
            });

            //Завантаження курсів валют НБУ
            CreateLink(HBoxTop, "Завантаження курсів валют НБУ", () => 
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Завантаження курсів валют НБУ", () => new Обробка_ЗавантаженняКурсівВалют()));

            ТабличніСписки.Валюти_Записи.AddColumns(TreeViewGrid);
            Config.Kernel.DirectoryObjectChanged += async (object? sender, Dictionary<string, List<Guid>> directory) =>
            {
                if (directory.Any((x) => x.Key == Валюти_Const.TYPE))
                    await LoadRecords();
            };
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Валюти_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Валюти_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Валюти_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Валюти_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Валюти_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Валюти_Записи.ДодатиВідбір(TreeViewGrid, Валюти_Функції.Відбори(searchText), true);

            await ТабличніСписки.Валюти_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.Валюти_Записи.CreateFilter(TreeViewGrid);
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

        #endregion
    }
}
