
/*     
        Контрагенти.cs 
        Список з Деревом
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Контрагенти : ДовідникЖурнал
    {
        Контрагенти_Папки ДеревоПапок;

        public Контрагенти() : base()
        {
            AddIsHierarchy();

            //Дерево папок
            ДеревоПапок = new Контрагенти_Папки()
            {
                WidthRequest = 500,
                CompositeMode = true,
                CallBack_RowActivated = async () =>
                {
                    if (IsHierarchy.Active)
                        await BeforeLoadRecords_OnTree();
                }
            };
            ДеревоПапок.SetValue();
            HPanedTable.Pack2(ДеревоПапок, false, true);

            CreateLink(HBoxTop, ДоговориКонтрагентів_Const.FULLNAME, async () =>
            {
                ДоговориКонтрагентів page = new ДоговориКонтрагентів();

                if (SelectPointerItem != null)
                    page.КонтрагентВласник.Pointer = new Контрагенти_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ДоговориКонтрагентів_Const.FULLNAME}", () => page);
                await page.SetValue();
            });

            ТабличніСписки.Контрагенти_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            if (IsHierarchy.Active)
            {
                if (DirectoryPointerItem != null || SelectPointerItem != null)
                {
                    Контрагенти_Objest? Обєкт = await new Контрагенти_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                    if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.Папка.UnigueID;
                }

                await ДеревоПапок.LoadRecords();
            }
            else
                await BeforeLoadRecords_OnTree();
        }

        public override async ValueTask LoadRecords_OnTree()
        {
            ТабличніСписки.Контрагенти_Записи.ОчиститиВідбір(TreeViewGrid);

            if (IsHierarchy.Active)
                ТабличніСписки.Контрагенти_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Контрагенти_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Контрагенти_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Контрагенти_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Контрагенти_Записи.ДодатиВідбір(TreeViewGrid, Контрагенти_Функції.Відбори(searchText), true);

            await ТабличніСписки.Контрагенти_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Контрагенти_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.Контрагенти_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.Контрагенти_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Контрагенти_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Контрагенти_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Контрагенти_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new Контрагенти_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, Контрагенти_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
