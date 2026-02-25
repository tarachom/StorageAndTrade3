
/*

        ПсуванняТоварів.cs

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ПсуванняТоварів : ДокументЖурнал
    {
        public ПсуванняТоварів()
        {
            ТабличніСписки.ПсуванняТоварів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПсуванняТоварів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПсуванняТоварів_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПсуванняТоварів_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПсуванняТоварів_Записи.ДодатиВідбір(TreeViewGrid, ПсуванняТоварів_Функції.Відбори(searchText));

            await ТабличніСписки.ПсуванняТоварів_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ПсуванняТоварів_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ПсуванняТоварів_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ПсуванняТоварів_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await ПсуванняТоварів_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await ПсуванняТоварів_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await ПсуванняТоварів_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ПсуванняТоварів_Pointer(uniqueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.ПсуванняТоварів";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ПсуванняТоварів_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UniqueID uniqueID, bool spendDoc)
        {
            ПсуванняТоварів_Objest? Обєкт = await new ПсуванняТоварів_Pointer(uniqueID).GetDocumentObject(true);
            if (Обєкт == null) return;

            if (spendDoc)
            {
                if (!await Обєкт.SpendTheDocument(Обєкт.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(Обєкт.UniqueID);
            }
            else
                await Обєкт.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UniqueID uniqueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПсуванняТоварів_Pointer(uniqueID));
        }

        protected override async ValueTask ExportXML(UniqueID uniqueID, string pathToFolder)
        {
            ПсуванняТоварів_Pointer Вказівник = new ПсуванняТоварів_Pointer(uniqueID);
            await Вказівник.GetPresentation();

            await ПсуванняТоварів_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}