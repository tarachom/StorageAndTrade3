
/*     
        Номенклатура.cs 
        Список з Деревом
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриВідомостей;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Номенклатура : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        Номенклатура_Папки ДеревоПапок;

        public Номенклатура() : base()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += async (object? sender, EventArgs args) => await LoadRecords();
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            CreateLink(HBoxTop, ХарактеристикиНоменклатури_Const.FULLNAME, async () =>
            {
                ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();

                if (SelectPointerItem != null)
                    page.НоменклатураВласник.Pointer = new Номенклатура_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ХарактеристикиНоменклатури_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            CreateLink(HBoxTop, ШтрихкодиНоменклатури_Const.FULLNAME, async () =>
            {
                ШтрихкодиНоменклатури page = new ШтрихкодиНоменклатури();

                if (SelectPointerItem != null)
                    page.НоменклатураВласник.Pointer = new Номенклатура_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ШтрихкодиНоменклатури_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            //Дерево папок
            ДеревоПапок = new Номенклатура_Папки() { WidthRequest = 500, CallBack_RowActivated = LoadRecords_TreeCallBack };
            HPanedTable.Pack2(ДеревоПапок, false, true);

            ТабличніСписки.Номенклатура_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                Номенклатура_Objest? Обєкт = await new Номенклатура_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.Папка.UnigueID;
            }

            await ДеревоПапок.SetValue();
        }

        async void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.Номенклатура_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Номенклатура_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Номенклатура_Записи.ОчиститиВідбір(TreeViewGrid);

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(Номенклатура_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Номенклатура_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.Номенклатура_Записи.ДодатиВідбір(TreeViewGrid, Номенклатура_Функції.Відбори(searchText), true);

            await ТабличніСписки.Номенклатура_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Номенклатура_Записи.CreateFilter(TreeViewGrid), false, false, 5);
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

        #endregion
    }
}
