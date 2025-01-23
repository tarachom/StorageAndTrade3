
/*     
        СтаттяРухуКоштів.cs
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
    public class СтаттяРухуКоштів : ДовідникЖурнал
    {
        public СтаттяРухуКоштів() : base()
        {
            ТабличніСписки.СтаттяРухуКоштів_Записи.AddColumns(TreeViewGrid);
            Config.Kernel.DirectoryObjectChanged += async (object? sender, Dictionary<string, List<Guid>> directory) =>
            {
                if (directory.Any((x) => x.Key == СтаттяРухуКоштів_Const.TYPE))
                    await LoadRecords();
            };
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.СтаттяРухуКоштів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СтаттяРухуКоштів_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СтаттяРухуКоштів_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.СтаттяРухуКоштів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СтаттяРухуКоштів_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.СтаттяРухуКоштів_Записи.ДодатиВідбір(TreeViewGrid, СтаттяРухуКоштів_Функції.Відбори(searchText), true);

            await ТабличніСписки.СтаттяРухуКоштів_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.СтаттяРухуКоштів_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СтаттяРухуКоштів_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СтаттяРухуКоштів_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СтаттяРухуКоштів_Функції.Copy(unigueID);
        }

        #endregion
    }
}
