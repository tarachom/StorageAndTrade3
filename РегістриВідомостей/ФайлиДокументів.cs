

/*     
        ФайлиДокументів.cs
        Список

        Табличний список - Записи
*/

using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.РегістриВідомостей;
using ТабличніСписки = StorageAndTrade_1_0.РегістриВідомостей.ТабличніСписки;

namespace StorageAndTrade
{
    public class ФайлиДокументів : РегістриВідомостейЖурнал
    {
        public ФайлиДокументів() : base()
        {
            ТабличніСписки.ФайлиДокументів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ФайлиДокументів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ФайлиДокументів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ФайлиДокументів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ФайлиДокументів_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ФайлиДокументів_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.ФайлиДокументів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            // ФайлиДокументів_Елемент page = new ФайлиДокументів_Елемент
            // {
            //     CallBack_LoadRecords = CallBack_LoadRecords,
            //     IsNew = IsNew
            // };

            // if (IsNew)
            //     await page.Елемент.New();
            // else if (unigueID == null || !await page.Елемент.Read(unigueID))
            // {
            //     Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            //     return;
            // }

            // NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
            // page.SetValue();
        }
        protected override async ValueTask Delete(UnigueID unigueID)
        {
            ФайлиДокументів_Objest Обєкт = new ФайлиДокументів_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ФайлиДокументів_Objest Обєкт = new ФайлиДокументів_Objest();
            if (await Обєкт.Read(unigueID))
            {
                ФайлиДокументів_Objest Новий = Обєкт.Copy();
                await Новий.Save();
                return Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "РегістриВідомостей.ФайлиДокументів";

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
