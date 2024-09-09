
/*     
        Виробники_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Виробники_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public Виробники_ШвидкийВибір() 
        {
            ТабличніСписки.Виробники_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Виробники_Записи.SelectPointerItem = null;
            ТабличніСписки.Виробники_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.Виробники_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Виробники_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.Виробники_Записи.ОчиститиВідбір(TreeViewGrid);

            //Код
            ТабличніСписки.Виробники_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(Виробники_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.Виробники_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(Comparison.OR, Виробники_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.Виробники_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            Виробники page = new Виробники()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {Виробники_Const.FULLNAME}", () => page);

            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Виробники_Елемент page = new Виробники_Елемент
            {
                IsNew = IsNew,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer
            };

            if (IsNew)
                await page.Елемент.New();
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
            Виробники_Objest Обєкт = new Виробники_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }
    }
}
