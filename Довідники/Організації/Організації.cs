
/*
        Організації.cs
        Дерево
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class Організації : ДовідникЖурнал
    {
        public Організації() 
        {
            ТабличніСписки.Організації_Записи.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Організації_Записи.SelectPointerItem = SelectPointerItem; //!+
            ТабличніСписки.Організації_Записи.DirectoryPointerItem = DirectoryPointerItem;
            ТабличніСписки.Організації_Записи.OpenFolder = OpenFolder; //!+

            ТабличніСписки.Організації_Записи.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.Організації_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Відбори
            ТабличніСписки.Організації_Записи.ДодатиВідбір(TreeViewGrid, Організації_ВідбориДляПошуку.Відбори(searchText), true);

            await ТабличніСписки.Організації_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.Організації_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            Організації_Елемент page = new Організації_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew,
                //РодичДляНового = new Організації_Pointer(DirectoryPointerItem ?? new UnigueID())
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

        #region ToolBar

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Організації_Objest Організації_Objest = new Організації_Objest();
            if (await Організації_Objest.Read(unigueID))
                await Організації_Objest.SetDeletionLabel(!Організації_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Організації_Objest Організації_Objest = new Організації_Objest();
            if (await Організації_Objest.Read(unigueID))
            {
                Організації_Objest Організації_Objest_Новий = await Організації_Objest.Copy(true);
                await Організації_Objest_Новий.Save();

                await Організації_Objest_Новий.Контакти_TablePart.Save(false); // Таблична частина "Контакти"

                return Організації_Objest_Новий.UnigueID;
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
