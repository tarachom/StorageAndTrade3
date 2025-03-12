/*

        ПартіяТоварівКомпозит_ШвидкийВибір.cs
        ШвидкийВибір

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode.Довідники;
using GeneratedCode.РегістриНакопичення;
using ТабличніСписки = GeneratedCode.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Номенклатура_PointerControl НоменклатураВідбір = new Номенклатура_PointerControl() { WidthPresentation = 100 };

        public ПартіяТоварівКомпозит_ШвидкийВибір()
        {
            ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Відбір
            HBoxTop.PackStart(НоменклатураВідбір, false, false, 2);
            НоменклатураВідбір.Caption = $"{Номенклатура_Const.FULLNAME}:";
            НоменклатураВідбір.AfterSelectFunc = async () => { await LoadRecords(); };
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.SelectPointerItem = null;
            ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.ОчиститиВідбір(TreeViewGrid);

            if (!НоменклатураВідбір.Pointer.IsEmpty())
            {
                /*
                Якщо вибрана Номенклатура тоді показуємо тільки партії, які є в наявності.
                Використовується таблиця Підсумки з регістру ПартіїТоварів
                */

                //Відбір партій які є в наявності по номенклатурі
                string query = @$"
SELECT DISTINCT
    ПартіїТоварів_Підсумки.{ПартіїТоварів_Підсумки_TablePart.ПартіяТоварівКомпозит}
FROM 
    {ПартіїТоварів_Підсумки_TablePart.TABLE} AS ПартіїТоварів_Підсумки
WHERE
    ПартіїТоварів_Підсумки.{ПартіїТоварів_Підсумки_TablePart.Номенклатура} = '{НоменклатураВідбір.Pointer.UnigueID}' AND
    ПартіїТоварів_Підсумки.{ПартіїТоварів_Підсумки_TablePart.Кількість} > 0
";

                //Додатково показується партія яка вже вибрана, навіть якщо її немає в залишку через об'єднання (UNION)
                if (DirectoryPointerItem != null && !DirectoryPointerItem.IsEmpty())
                    query = @$"( {query} ) UNION ( SELECT '{DirectoryPointerItem}' )";

                ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.ДодатиВідбір(TreeViewGrid,
                    new Where("uid", Comparison.IN, query, true));
            }

            await ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.ПартіяТоварівКомпозит_Записи.ДодатиВідбір(TreeViewGrid, ПартіяТоварівКомпозит_Функції.Відбори(searchText), true);

            await ТабличніСписки.ПартіяТоварівКомпозит_ЗаписиШвидкийВибір.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПартіяТоварівКомпозит_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПартіяТоварівКомпозит_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПартіяТоварівКомпозит_Функції.SetDeletionLabel(unigueID);
        }
    }
}