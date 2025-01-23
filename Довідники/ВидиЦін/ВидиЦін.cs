
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
            Config.Kernel.DirectoryObjectChanged += async (object? sender, Dictionary<string, List<Guid>> directory) =>
            {
                if (directory.Any((x) => x.Key == ВидиЦін_Const.TYPE))
                    await LoadRecords();
            };
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВидиЦін_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВидиЦін_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ВидиЦін_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ВидиЦін_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
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

        #endregion
    }
}
