
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
            Config.Kernel.DirectoryObjectChanged += async (object? sender, Dictionary<string, List<Guid>> directory) =>
            {
                if (directory.Any((x) => x.Key == ПакуванняОдиниціВиміру_Const.TYPE))
                    await LoadRecords();
            };
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ПакуванняОдиниціВиміру_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПакуванняОдиниціВиміру_Записи.ДодатиВідбір(TreeViewGrid, ПакуванняОдиниціВиміру_Функції.Відбори(searchText), true);

            await ТабличніСписки.ПакуванняОдиниціВиміру_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ПакуванняОдиниціВиміру_Записи.CreateFilter(TreeViewGrid);
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

        #endregion
    }
}
