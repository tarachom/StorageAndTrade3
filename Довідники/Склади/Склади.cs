
/*     
        Склади.cs 
        Список з Деревом
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Склади : ДовідникЖурнал
    {
        Склади_Папки ДеревоПапок;

        public Склади() : base()
        {
            AddIsHierarchy();

            //Дерево папок
            ДеревоПапок = new Склади_Папки()
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

            CreateLink(HBoxTop, СкладськіПриміщення_Const.FULLNAME, async () =>
            {
                СкладськіПриміщення page = new СкладськіПриміщення();

                if (SelectPointerItem != null)
                    page.СкладВласник.Pointer = new Склади_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{СкладськіПриміщення_Const.FULLNAME}", () => page);
                await page.SetValue();
            });

            ТабличніСписки.Склади_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            if (IsHierarchy.Active)
            {
                if (DirectoryPointerItem != null || SelectPointerItem != null)
                {
                    Склади_Objest? Обєкт = await new Склади_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                    if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.Папка.UnigueID;
                }

                await ДеревоПапок.LoadRecords();
            }
            else
                await BeforeLoadRecords_OnTree();
        }

        public override async ValueTask LoadRecords_OnTree()
        {
            ТабличніСписки.Склади_Записи.ОчиститиВідбір(TreeViewGrid);

            if (IsHierarchy.Active)
                ТабличніСписки.Склади_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Склади_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Склади_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Склади_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Склади_Записи.ДодатиВідбір(TreeViewGrid, Склади_Функції.Відбори(searchText), true);

            await ТабличніСписки.Склади_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Склади_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.Склади_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.Склади_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Склади_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Склади_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Склади_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new Склади_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, Склади_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
