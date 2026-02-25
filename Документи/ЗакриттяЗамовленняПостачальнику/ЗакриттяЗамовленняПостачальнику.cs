

/*     
        ЗакриттяЗамовленняПостачальнику.cs
        Список
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ЗакриттяЗамовленняПостачальнику : ДокументЖурнал
    {
        public ЗакриттяЗамовленняПостачальнику() : base()
        {
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.ДодатиВідбір(TreeViewGrid, ЗакриттяЗамовленняПостачальнику_Функції.Відбори(searchText));

            await ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await ЗакриттяЗамовленняПостачальнику_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await ЗакриттяЗамовленняПостачальнику_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await ЗакриттяЗамовленняПостачальнику_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ЗакриттяЗамовленняПостачальнику_Pointer(uniqueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗакриттяЗамовленняПостачальнику";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ЗакриттяЗамовленняПостачальнику_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UniqueID uniqueID, bool spendDoc)
        {
            ЗакриттяЗамовленняПостачальнику_Objest? Обєкт = await new ЗакриттяЗамовленняПостачальнику_Pointer(uniqueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗакриттяЗамовленняПостачальнику_Pointer(uniqueID));
        }

        protected override bool IsExportXML() { return false; } //Дозволити експорт документу
        protected override async ValueTask ExportXML(UniqueID uniqueID, string pathToFolder)
        {
            ЗакриттяЗамовленняПостачальнику_Pointer Вказівник = new ЗакриттяЗамовленняПостачальнику_Pointer(uniqueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await ЗакриттяЗамовленняПостачальнику_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        /*
        protected override async ValueTask PrintingDoc(UniqueID uniqueID)
        {
            await ЗакриттяЗамовленняПостачальнику_Друк.PDF(uniqueID);
        }
        */

        #endregion
    }
}
