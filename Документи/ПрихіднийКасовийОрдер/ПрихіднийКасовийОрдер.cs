
/*

        ПрихіднийКасовийОрдер.cs

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class ПрихіднийКасовийОрдер : ДокументЖурнал
    {
        public ПрихіднийКасовийОрдер() 
        {
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПрихіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПрихіднийКасовийОрдер_Записи.ДодатиВідбір(TreeViewGrid, ПрихіднийКасовийОрдер_Функції.Відбори(searchText));

            await ТабличніСписки.ПрихіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ПрихіднийКасовийОрдер_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПрихіднийКасовийОрдер_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПрихіднийКасовийОрдер_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ПрихіднийКасовийОрдер_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ПрихіднийКасовийОрдер";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПрихіднийКасовийОрдер_Objest? Обєкт = await new ПрихіднийКасовийОрдер_Pointer(unigueID).GetDocumentObject(true);
            if (Обєкт == null) return;

            if (spendDoc)
            {
                if (!await Обєкт.SpendTheDocument(Обєкт.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(Обєкт.UnigueID);
            }
            else
                await Обєкт.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПрихіднийКасовийОрдер_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ПрихіднийКасовийОрдер_Pointer Вказівник = new ПрихіднийКасовийОрдер_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await ПрихіднийКасовийОрдер_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await ПрихіднийКасовийОрдер_Друк.PDF(unigueID);
        }

        #endregion
    }
}