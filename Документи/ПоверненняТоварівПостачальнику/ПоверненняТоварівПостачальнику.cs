
/*

        ПоверненняТоварівПостачальнику.cs

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ПоверненняТоварівПостачальнику : ДокументЖурнал
    {
        public ПоверненняТоварівПостачальнику()
        {
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.LoadRecords(TreeViewGrid);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.ДодатиВідбір(TreeViewGrid, ПоверненняТоварівПостачальнику_Функції.Відбори(searchText));

            await ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ПоверненняТоварівПостачальнику_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПоверненняТоварівПостачальнику_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПоверненняТоварівПостачальнику_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ПоверненняТоварівПостачальнику_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ПоверненняТоварівПостачальнику";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ПоверненняТоварівПостачальнику_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПоверненняТоварівПостачальнику_Objest? Обєкт = await new ПоверненняТоварівПостачальнику_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПоверненняТоварівПостачальнику_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ПоверненняТоварівПостачальнику_Pointer Вказівник = new ПоверненняТоварівПостачальнику_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await ПоверненняТоварівПостачальнику_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await ПоверненняТоварівПостачальнику_Друк.PDF(unigueID);
        }

        #endregion
    }
}