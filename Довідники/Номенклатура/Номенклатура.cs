
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
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        Номенклатура_Папки ДеревоПапок;

        public Номенклатура() : base()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += async (sender, args) =>
            {
                ClearPages();
                if (checkButtonIsHierarchy.Active)
                    await LoadRecords();
                else
                    await LoadRecords_TreeCallBack();
            };

            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Дерево папок
            ДеревоПапок = new Номенклатура_Папки()
            {
                WidthRequest = 500,
                CallBack_RowActivated = async () =>
                {
                    if (checkButtonIsHierarchy.Active)
                    {
                        ClearPages();
                        await LoadRecords_TreeCallBack();
                    }
                },
                CompositeMode = true
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
            if (checkButtonIsHierarchy.Active)
            {
                if (DirectoryPointerItem != null || SelectPointerItem != null)
                {
                    Номенклатура_Objest? Обєкт = await new Номенклатура_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                    if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.Папка.UnigueID;
                }

                await ДеревоПапок.LoadRecords();
            }
            else
                await LoadRecords_TreeCallBack();
        }

        async ValueTask LoadRecords_TreeCallBack()
        {
            ТабличніСписки.Номенклатура_Записи.ОчиститиВідбір(TreeViewGrid);

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Номенклатура_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
            PagesShow(LoadRecords_TreeCallBack);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Номенклатура_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid, Номенклатура_Функції.Відбори(searchText), true);

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid, OpenFolder);
            PagesShow(async () => await LoadRecords_OnSearch(searchText));
        }

        async ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid);
            PagesShow(LoadRecords_OnFilter);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.Номенклатура_Записи.CreateFilter(TreeViewGrid, () => PagesShow(LoadRecords_OnFilter));
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
            await LoadRecords();
        }

        #endregion
    }
}