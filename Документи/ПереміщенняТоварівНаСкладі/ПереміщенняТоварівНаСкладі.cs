
/*

        ПереміщенняТоварівНаСкладі.cs

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ПереміщенняТоварівНаСкладі : ДокументЖурнал
    {
        public ПереміщенняТоварівНаСкладі()
        {
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid, ПереміщенняТоварівНаСкладі_Функції.Відбори(searchText));

            await ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПереміщенняТоварівНаСкладі_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПереміщенняТоварівНаСкладі_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ПереміщенняТоварівНаСкладі_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ПереміщенняТоварівНаСкладі";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ПереміщенняТоварівНаСкладі_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПереміщенняТоварівНаСкладі_Objest? Обєкт = await new ПереміщенняТоварівНаСкладі_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПереміщенняТоварівНаСкладі_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ПереміщенняТоварівНаСкладі_Pointer Вказівник = new ПереміщенняТоварівНаСкладі_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await ПереміщенняТоварівНаСкладі_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}