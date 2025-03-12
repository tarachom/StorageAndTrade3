/*

        ДоговориКонтрагентів_ШвидкийВибір.cs
        ШвидкийВибір

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode.Довідники;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ДоговориКонтрагентів_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Контрагенти_PointerControl КонтрагентВласник = new Контрагенти_PointerControl() { Caption = "Контрагент:", WidthPresentation = 100 };

        public ДоговориКонтрагентів_ШвидкийВибір() 
        {
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Власник
            HBoxTop.PackStart(КонтрагентВласник, false, false, 2);
            КонтрагентВласник.AfterSelectFunc = async () => await LoadRecords();
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!КонтрагентВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where(ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, КонтрагентВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.ДоговориКонтрагентів_Записи.ДодатиВідбір(TreeViewGrid, ДоговориКонтрагентів_Функції.Відбори(searchText));

            await ТабличніСписки.ДоговориКонтрагентів_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ДоговориКонтрагентів page = new ДоговориКонтрагентів()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            page.КонтрагентВласник.Pointer = КонтрагентВласник.Pointer;

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ДоговориКонтрагентів_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ДоговориКонтрагентів_Елемент page = new ДоговориКонтрагентів_Елемент
            {
                IsNew = IsNew,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer
            };

            if (IsNew)
            {
                await page.Елемент.New();
                page.КонтрагентиДляНового = КонтрагентВласник.Pointer;
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
            await ДоговориКонтрагентів_Функції.SetDeletionLabel(unigueID);

        }
    }
}