

/*     
        ЦіниНоменклатури.cs
        Список

        Табличний список - Записи
*/

using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.РегістриВідомостей;
using ТабличніСписки = StorageAndTrade_1_0.РегістриВідомостей.ТабличніСписки;

namespace StorageAndTrade
{
    public class ЦіниНоменклатури : РегістриВідомостейЖурнал
    {
        public ЦіниНоменклатури() : base()
        {
            ТабличніСписки.ЦіниНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.ЦіниНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЦіниНоменклатури_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЦіниНоменклатури_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ЦіниНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЦіниНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ЦіниНоменклатури_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЦіниНоменклатури_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ЦіниНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ЦіниНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.ЦіниНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ЦіниНоменклатури_Const.FULLNAME} *", () =>
                {
                    ЦіниНоменклатури_Елемент page = new ЦіниНоменклатури_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                ЦіниНоменклатури_Objest ЦіниНоменклатури_Objest = new ЦіниНоменклатури_Objest();
                if (await ЦіниНоменклатури_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ЦіниНоменклатури_Objest.Period}", () =>
                    {
                        ЦіниНоменклатури_Елемент page = new ЦіниНоменклатури_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ЦіниНоменклатури_Objest = ЦіниНоменклатури_Objest,
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async ValueTask Delete(UnigueID unigueID)
        {
            ЦіниНоменклатури_Objest ЦіниНоменклатури_Objest = new ЦіниНоменклатури_Objest();
            if (await ЦіниНоменклатури_Objest.Read(unigueID))
                await ЦіниНоменклатури_Objest.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ЦіниНоменклатури_Objest ЦіниНоменклатури_Objest = new ЦіниНоменклатури_Objest();
            if (await ЦіниНоменклатури_Objest.Read(unigueID))
            {
                ЦіниНоменклатури_Objest ЦіниНоменклатури_Objest_Новий = ЦіниНоменклатури_Objest.Copy();
                await ЦіниНоменклатури_Objest_Новий.Save();
                return ЦіниНоменклатури_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "РегістриВідомостей.ЦіниНоменклатури";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            LoadRecords();
        }

        #endregion
    }
}
    