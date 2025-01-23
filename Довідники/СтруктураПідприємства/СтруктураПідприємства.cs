
/*     
        СтруктураПідприємства.cs
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
    public class СтруктураПідприємства : ДовідникЖурнал
    {
        public СтруктураПідприємства() : base()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.AddColumns(TreeViewGrid);
            Config.Kernel.DirectoryObjectChanged += async (object? sender, Dictionary<string, List<Guid>> directory) =>
            {
                if (directory.Any((x) => x.Key == СтруктураПідприємства_Const.TYPE))
                    await LoadRecords();
            };
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СтруктураПідприємства_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СтруктураПідприємства_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СтруктураПідприємства_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СтруктураПідприємства_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СтруктураПідприємства_Записи.ДодатиВідбір(TreeViewGrid, СтруктураПідприємства_Функції.Відбори(searchText), true);

            await ТабличніСписки.СтруктураПідприємства_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.СтруктураПідприємства_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СтруктураПідприємства_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СтруктураПідприємства_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СтруктураПідприємства_Функції.Copy(unigueID);
        }

        #endregion
    }
}
