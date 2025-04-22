
/*     
        Номенклатура.cs 
        Список з Деревом
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using GeneratedCode.Довідники;
using GeneratedCode.РегістриВідомостей;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Номенклатура : ДовідникЖурнал
    {
        Номенклатура_Папки ДеревоПапок;

        public Номенклатура() : base()
        {
            AddIsHierarchy();

            //Дерево папок
            ДеревоПапок = new Номенклатура_Папки()
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

            CreateLink(HBoxTop, ХарактеристикиНоменклатури_Const.FULLNAME, async () =>
            {
                ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();

                if (SelectPointerItem != null)
                    page.Власник.Pointer = new Номенклатура_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ХарактеристикиНоменклатури_Const.FULLNAME}", () => page);
                await page.SetValue();
            });

            CreateLink(HBoxTop, ШтрихкодиНоменклатури_Const.FULLNAME, async () =>
            {
                РегістриВідомостей.ШтрихкодиНоменклатури page = new РегістриВідомостей.ШтрихкодиНоменклатури();

                if (SelectPointerItem != null)
                    page.НоменклатураВласник.Pointer = new Номенклатура_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ШтрихкодиНоменклатури_Const.FULLNAME}", () => page);
                await page.SetValue();
            });

            ТабличніСписки.Номенклатура_Записи.AddColumns(TreeViewGrid);
            ТабличніСписки.Номенклатура_Записи.Сторінки(TreeViewGrid, new Сторінки.Налаштування() { PageSize = 300, Тип = Сторінки.ТипЖурналу.Довідники });

            Пошук.MinLength = 2;
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            if (IsHierarchy.Active)
            {
                if (DirectoryPointerItem != null || SelectPointerItem != null)
                {
                    Номенклатура_Objest? Обєкт = await new Номенклатура_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                    if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.Папка.UnigueID;
                }

                await ДеревоПапок.LoadRecords();
            }
            else
                await BeforeLoadRecords_OnTree();
        }

        public override async ValueTask LoadRecords_OnTree()
        {
            ТабличніСписки.Номенклатура_Записи.ОчиститиВідбір(TreeViewGrid);

            if (IsHierarchy.Active)
                ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Номенклатура_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Номенклатура_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid, Номенклатура_Функції.Відбори(searchText), true);

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.Номенклатура_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await Номенклатура_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await Номенклатура_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await Номенклатура_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, Номенклатура_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}