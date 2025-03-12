

/*     
        ЦіниНоменклатури.cs
        Список

        Табличний список - Записи
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.РегістриВідомостей;
using ТабличніСписки = GeneratedCode.РегістриВідомостей.ТабличніСписки;

namespace StorageAndTrade
{
    public class ЦіниНоменклатури : РегістриВідомостейЖурнал
    {
        public ЦіниНоменклатури() 
        {
            ТабличніСписки.ЦіниНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЦіниНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЦіниНоменклатури_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЦіниНоменклатури_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ЦіниНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЦіниНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ЦіниНоменклатури_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЦіниНоменклатури_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
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

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ЦіниНоменклатури_Елемент page = new ЦіниНоменклатури_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                page.ЦіниНоменклатури_Objest.New();
            else if (unigueID == null || !await page.ЦіниНоменклатури_Objest.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
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

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }

        #endregion
    }
}
