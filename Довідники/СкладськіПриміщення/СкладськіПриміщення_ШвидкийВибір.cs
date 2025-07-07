
/*     
        СкладськіПриміщення_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class СкладськіПриміщення_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Склади_PointerControl СкладВласник = new Склади_PointerControl();

        public СкладськіПриміщення_ШвидкийВибір()
        {
            ТабличніСписки.СкладськіПриміщення_Записи.AddColumns(TreeViewGrid);

            //Власник
            HBoxTop.PackStart(СкладВласник, false, false, 2);
            СкладВласник.Caption = "Склад:";
            СкладВласник.AfterSelectFunc = async () =>  await BeforeLoadRecords(); 
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.СкладськіПриміщення_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.СкладськіПриміщення_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!СкладВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(СкладськіПриміщення_Const.Склад, Comparison.EQ, СкладВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.СкладськіПриміщення_Записи.ДодатиВідбір(TreeViewGrid, СкладськіПриміщення_Функції.Відбори(searchText));

            await ТабличніСписки.СкладськіПриміщення_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            СкладськіПриміщення page = new СкладськіПриміщення()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            page.СкладВласник.Pointer = СкладВласник.Pointer;

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {СкладськіПриміщення_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            СкладськіПриміщення_Елемент page = new СкладськіПриміщення_Елемент
            {
                IsNew = IsNew,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer
            };

            if (IsNew)
            {
                await page.Елемент.New();
                page.СкладДляНового = СкладВласник.Pointer;
            }
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await СкладськіПриміщення_Функції.SetDeletionLabel(unigueID);

        }
    }
}