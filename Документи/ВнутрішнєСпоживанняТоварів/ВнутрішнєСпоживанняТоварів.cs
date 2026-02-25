
/*

        ВнутрішнєСпоживанняТоварів.cs
        
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ВнутрішнєСпоживанняТоварів : ДокументЖурнал
    {
        public ВнутрішнєСпоживанняТоварів()
        {
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.ДодатиВідбір(TreeViewGrid, ВнутрішнєСпоживанняТоварів_Функції.Відбори(searchText));

            await ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ВнутрішнєСпоживанняТоварів_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await ВнутрішнєСпоживанняТоварів_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await ВнутрішнєСпоживанняТоварів_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await ВнутрішнєСпоживанняТоварів_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ВнутрішнєСпоживанняТоварів_Pointer(uniqueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.ВнутрішнєСпоживанняТоварів";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ВнутрішнєСпоживанняТоварів_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UniqueID uniqueID, bool spendDoc)
        {
            ВнутрішнєСпоживанняТоварів_Objest? Обєкт = await new ВнутрішнєСпоживанняТоварів_Pointer(uniqueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ВнутрішнєСпоживанняТоварів_Pointer(uniqueID));
        }

        protected override async ValueTask ExportXML(UniqueID uniqueID, string pathToFolder)
        {
            ВнутрішнєСпоживанняТоварів_Pointer Вказівник = new ВнутрішнєСпоживанняТоварів_Pointer(uniqueID);
            await Вказівник.GetPresentation();

            await ВнутрішнєСпоживанняТоварів_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        protected override async ValueTask PrintingDoc(UniqueID uniqueID)
        {
            await ВнутрішнєСпоживанняТоварів_Друк.PDF(uniqueID);
        }

        #endregion
    }
}