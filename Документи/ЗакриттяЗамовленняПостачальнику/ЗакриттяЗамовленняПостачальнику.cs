

/*     
        ЗакриттяЗамовленняПостачальнику.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class ЗакриттяЗамовленняПостачальнику : ДокументЖурнал
    {
        public ЗакриттяЗамовленняПостачальнику() : base()
        {
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.ОчиститиВідбір(TreeViewGrid);
            
            //Відбори
            ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.ДодатиВідбір(TreeViewGrid, ЗакриттяЗамовленняПостачальнику_Функції.Відбори(searchText));

            await ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ЗакриттяЗамовленняПостачальнику_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ЗакриттяЗамовленняПостачальнику_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ЗакриттяЗамовленняПостачальнику_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ЗакриттяЗамовленняПостачальнику_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗакриттяЗамовленняПостачальнику";

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
            ЗакриттяЗамовленняПостачальнику_Objest? Обєкт = await new ЗакриттяЗамовленняПостачальнику_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗакриттяЗамовленняПостачальнику_Pointer(unigueID));
        }
        
        protected override bool IsExportXML() { return false; } //Дозволити експорт документу
        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ЗакриттяЗамовленняПостачальнику_Pointer Вказівник = new ЗакриттяЗамовленняПостачальнику_Pointer(unigueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await ЗакриттяЗамовленняПостачальнику_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        /*
        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await ЗакриттяЗамовленняПостачальнику_Друк.PDF(unigueID);
        }
        */

        #endregion
    }
}
    