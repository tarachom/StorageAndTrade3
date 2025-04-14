
/*     
        Контрагенти.cs 
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
    public class Контрагенти : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        Контрагенти_Папки ДеревоПапок;

        public Контрагенти() : base()
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
            ДеревоПапок = new Контрагенти_Папки()
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

            CreateLink(HBoxTop, ДоговориКонтрагентів_Const.FULLNAME, async () =>
            {
                ДоговориКонтрагентів page = new ДоговориКонтрагентів();

                if (SelectPointerItem != null)
                    page.КонтрагентВласник.Pointer = new Контрагенти_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ДоговориКонтрагентів_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            ТабличніСписки.Контрагенти_Записи.AddColumns(TreeViewGrid);
            ТабличніСписки.Контрагенти_Записи.Сторінки(TreeViewGrid, new Сторінки.Налаштування() { PageSize = 300, Тип = Сторінки.ТипЖурналу.Довідники });
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            if (checkButtonIsHierarchy.Active)
            {
                if (DirectoryPointerItem != null || SelectPointerItem != null)
                {
                    Контрагенти_Objest? Обєкт = await new Контрагенти_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                    if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.Папка.UnigueID;
                }

                await ДеревоПапок.LoadRecords();
            }
            else
                await LoadRecords_TreeCallBack();
        }

        async ValueTask LoadRecords_TreeCallBack()
        {
            ТабличніСписки.Контрагенти_Записи.ОчиститиВідбір(TreeViewGrid);

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.Контрагенти_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Контрагенти_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Контрагенти_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
            PagesShow(LoadRecords_TreeCallBack);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Контрагенти_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Контрагенти_Записи.ДодатиВідбір(TreeViewGrid, Контрагенти_Функції.Відбори(searchText), true);

            await ТабличніСписки.Контрагенти_Записи.LoadRecords(TreeViewGrid, OpenFolder);
            PagesShow(() => LoadRecords_OnSearch(searchText));
        }

        async ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.Контрагенти_Записи.LoadRecords(TreeViewGrid);
            PagesShow(LoadRecords_OnFilter);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.Контрагенти_Записи.CreateFilter(TreeViewGrid, () => PagesShow(LoadRecords_OnFilter));
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

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, Контрагенти_Const.POINTER);
            await LoadRecords();
        }

        #endregion
    }
}
