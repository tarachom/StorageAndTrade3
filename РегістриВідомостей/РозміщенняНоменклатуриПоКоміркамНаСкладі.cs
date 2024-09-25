

/*     
        РозміщенняНоменклатуриПоКоміркамНаСкладі.cs
        Список

        Табличний список - Записи
*/

using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.РегістриВідомостей;
using ТабличніСписки = StorageAndTrade_1_0.РегістриВідомостей.ТабличніСписки;

namespace StorageAndTrade
{
    public class РозміщенняНоменклатуриПоКоміркамНаСкладі : РегістриВідомостейЖурнал
    {
        public РозміщенняНоменклатуриПоКоміркамНаСкладі() : base()
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркамНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            // РозміщенняНоменклатуриПоКоміркамНаСкладі_Елемент page = new РозміщенняНоменклатуриПоКоміркамНаСкладі_Елемент
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
            await ValueTask.FromResult(true);
        }
        protected override async ValueTask Delete(UnigueID unigueID)
        {
            РозміщенняНоменклатуриПоКоміркамНаСкладі_Objest Обєкт = new РозміщенняНоменклатуриПоКоміркамНаСкладі_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            РозміщенняНоменклатуриПоКоміркамНаСкладі_Objest Обєкт = new РозміщенняНоменклатуриПоКоміркамНаСкладі_Objest();
            if (await Обєкт.Read(unigueID))
            {
                РозміщенняНоменклатуриПоКоміркамНаСкладі_Objest Новий = Обєкт.Copy();
                await Новий.Save();
                return Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "РегістриВідомостей.РозміщенняНоменклатуриПоКоміркамНаСкладі";

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
