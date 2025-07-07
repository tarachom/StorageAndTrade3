
/*

        ПоверненняТоварівВідКлієнта.cs

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ПоверненняТоварівВідКлієнта : ДокументЖурнал
    {
        public ПоверненняТоварівВідКлієнта()
        {
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.ДодатиВідбір(TreeViewGrid, ПоверненняТоварівВідКлієнта_Функції.Відбори(searchText));

            await ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПоверненняТоварівВідКлієнта_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПоверненняТоварівВідКлієнта_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ПоверненняТоварівВідКлієнта_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ПоверненняТоварівВідКлієнта_Pointer(unigueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.ПоверненняТоварівВідКлієнта";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ПоверненняТоварівВідКлієнта_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПоверненняТоварівВідКлієнта_Objest? Обєкт = await new ПоверненняТоварівВідКлієнта_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПоверненняТоварівВідКлієнта_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ПоверненняТоварівВідКлієнта_Pointer Вказівник = new ПоверненняТоварівВідКлієнта_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await ПоверненняТоварівВідКлієнта_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await ПоверненняТоварівВідКлієнта_Друк.PDF(unigueID);
        }

        #endregion
    }
}