

/*     файл:     test.cs     */

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class test : ДовідникЖурнал
    {
        public test() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.test_Записи.Store;
            ТабличніСписки.test_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.test_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.test_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.test_Записи.Where.Clear();

            ТабличніСписки.test_Записи.LoadRecords();

            if (ТабличніСписки.test_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.test_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.test_Записи.Where.Clear();

            //Назва
            ТабличніСписки.test_Записи.Where.Add(
                new Where(test_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.test_Записи.LoadRecords();

            if (ТабличніСписки.test_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.test_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{test_Const.FULLNAME} *", () =>
                {
                    test_Елемент page = new test_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                test_Objest test_Objest = new test_Objest();
                if (test_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{test_Objest.Назва}", () =>
                    {
                        test_Елемент page = new test_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            test_Objest = test_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            test_Objest test_Objest = new test_Objest();
            if (test_Objest.Read(unigueID))
                test_Objest.SetDeletionLabel(!test_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            test_Objest test_Objest = new test_Objest();
            if (test_Objest.Read(unigueID))
            {
                test_Objest test_Objest_Новий = test_Objest.Copy(true);
                test_Objest_Новий.Save();

                return test_Objest_Новий.UnigueID;
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
