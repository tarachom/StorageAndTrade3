
/*     
        СкладськіКомірки.cs 
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
    public class СкладськіКомірки : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        СкладськіКомірки_Папки ДеревоПапок;

        public СкладськіПриміщення_PointerControl Власник = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення:" };


        public СкладськіКомірки()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += async (object? sender, EventArgs args) => await LoadRecords();
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Дерево папок
            ДеревоПапок = new СкладськіКомірки_Папки() { WidthRequest = 500, CallBack_RowActivated = LoadRecords_TreeCallBack };
            HPanedTable.Pack2(ДеревоПапок, false, true);

            ТабличніСписки.СкладськіКомірки_Записи.AddColumns(TreeViewGrid);
            Config.Kernel.DirectoryObjectChanged += async (object? sender, Dictionary<string, List<Guid>> directory) =>
            {
                if (directory.Any((x) => x.Key == СкладськіКомірки_Const.TYPE))
                    await LoadRecords();
            };

            HBoxTop.PackStart(Власник, false, false, 2); //Власник
            Власник.AfterSelectFunc = () => ДеревоПапок.Власник.Pointer = Власник.Pointer;
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                СкладськіКомірки_Objest? Обєкт = await new СкладськіКомірки_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.Папка.UnigueID;
            }

            await ДеревоПапок.SetValue();
        }

        async void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.СкладськіКомірки_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.СкладськіКомірки_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.СкладськіКомірки_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.СкладськіКомірки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіКомірки_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            //Відбори
            ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid, СкладськіКомірки_Функції.Відбори(searchText));

            await ТабличніСписки.СкладськіКомірки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.СкладськіКомірки_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await СкладськіКомірки_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null, Власник.Pointer);

        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СкладськіКомірки_Функції.SetDeletionLabel(unigueID);

        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await СкладськіКомірки_Функції.Copy(unigueID);

        }

        #endregion
    }
}
