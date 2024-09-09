
/*     
        Виробники.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class Виробники : ДовідникЖурнал
    {
        public Виробники() 
        {
            ТабличніСписки.Виробники_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Виробники_Записи.SelectPointerItem = SelectPointerItem;
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

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Виробники_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Виробники_Елемент page = new Виробники_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
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
            Виробники_Objest Виробники_Objest = new Виробники_Objest();
            if (await Виробники_Objest.Read(unigueID))
                await Виробники_Objest.SetDeletionLabel(!Виробники_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Виробники_Objest Виробники_Objest = new Виробники_Objest();
            if (await Виробники_Objest.Read(unigueID))
            {
                Виробники_Objest Виробники_Objest_Новий = await Виробники_Objest.Copy(true);
                await Виробники_Objest_Новий.Save();

                return Виробники_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion
    }
}
