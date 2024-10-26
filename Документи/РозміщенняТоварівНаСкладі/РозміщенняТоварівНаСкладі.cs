
/*
        РозміщенняТоварівНаСкладі.cs
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class РозміщенняТоварівНаСкладі : ДокументЖурнал
    {
        public РозміщенняТоварівНаСкладі() 
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid, РозміщенняТоварівНаСкладі_Функції.Відбори(searchText));

            await ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await РозміщенняТоварівНаСкладі_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await РозміщенняТоварівНаСкладі_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await РозміщенняТоварівНаСкладі_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.РозміщенняТоварівНаСкладі";

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
            РозміщенняТоварівНаСкладі_Objest? Обєкт = await new РозміщенняТоварівНаСкладі_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РозміщенняТоварівНаСкладі_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            РозміщенняТоварівНаСкладі_Pointer Вказівник = new РозміщенняТоварівНаСкладі_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await РозміщенняТоварівНаСкладі_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}