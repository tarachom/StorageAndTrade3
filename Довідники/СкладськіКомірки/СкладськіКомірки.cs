
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
using GLib;

namespace StorageAndTrade
{
    public class СкладськіКомірки : ДовідникЖурнал
    {
        СкладськіКомірки_Папки ДеревоПапок;
        public СкладськіПриміщення_PointerControl Власник = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення:" };

        public СкладськіКомірки()
        {
            AddIsHierarchy();

            //Дерево папок
            ДеревоПапок = new СкладськіКомірки_Папки()
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

            HBoxTop.PackStart(Власник, false, false, 2); //Власник
            Власник.AfterSelectFunc = () => ДеревоПапок.Власник.Pointer = Власник.Pointer;

            ТабличніСписки.СкладськіКомірки_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            if (IsHierarchy.Active)
            {
                if (DirectoryPointerItem != null || SelectPointerItem != null)
                {
                    СкладськіКомірки_Objest? Обєкт = await new СкладськіКомірки_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                    if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.Папка.UnigueID;
                }

                await ДеревоПапок.LoadRecords();
            }
            else
                await BeforeLoadRecords_OnTree();
        }

        public override async ValueTask LoadRecords_OnTree()
        {
            ТабличніСписки.СкладськіКомірки_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            if (IsHierarchy.Active)
                ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Папка, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.СкладськіКомірки_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіКомірки_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіКомірки_Const.Приміщення, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));

            //Відбори
            ТабличніСписки.СкладськіКомірки_Записи.ДодатиВідбір(TreeViewGrid, СкладськіКомірки_Функції.Відбори(searchText));

            await ТабличніСписки.СкладськіКомірки_Записи.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.СкладськіКомірки_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.СкладськіКомірки_Записи.CreateFilter(TreeViewGrid, filterControl);
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

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, СкладськіКомірки_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
