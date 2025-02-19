
/*     
        ТипорозміриКомірок.cs
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
    public class ТипорозміриКомірок : ДовідникЖурнал
    {
        public ТипорозміриКомірок() : base()
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.AddColumns(TreeViewGrid);
            Config.Kernel.DirectoryObjectChanged += async (object? sender, Dictionary<string, List<Guid>> directory) =>
            {
                if (directory.Any((x) => x.Key == ТипорозміриКомірок_Const.TYPE))
                    await LoadRecords();
            };
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ТипорозміриКомірок_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ТипорозміриКомірок_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ТипорозміриКомірок_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ТипорозміриКомірок_Записи.ДодатиВідбір(TreeViewGrid, ТипорозміриКомірок_Функції.Відбори(searchText), true);

            await ТабличніСписки.ТипорозміриКомірок_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ТипорозміриКомірок_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ТипорозміриКомірок_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ТипорозміриКомірок_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ТипорозміриКомірок_Функції.Copy(unigueID);
        }

        #endregion
    }
}
