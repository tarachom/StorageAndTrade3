

/*     
        ЗакриттяЗамовленняКлієнта.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ЗакриттяЗамовленняКлієнта : ДокументЖурнал
    {
        public ЗакриттяЗамовленняКлієнта() : base()
        {
            ТабличніСписки.ЗакриттяЗамовленняКлієнта_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗакриттяЗамовленняКлієнта_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗакриттяЗамовленняКлієнта_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗакриттяЗамовленняКлієнта_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ЗакриттяЗамовленняКлієнта_Записи.ДодатиВідбір(TreeViewGrid, ЗакриттяЗамовленняКлієнта_Функції.Відбори(searchText));

            await ТабличніСписки.ЗакриттяЗамовленняКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ЗакриттяЗамовленняКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ЗакриттяЗамовленняКлієнта_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ЗакриттяЗамовленняКлієнта_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ЗакриттяЗамовленняКлієнта_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ЗакриттяЗамовленняКлієнта_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗакриттяЗамовленняКлієнта";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ЗакриттяЗамовленняКлієнта_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ЗакриттяЗамовленняКлієнта_Objest? Обєкт = await new ЗакриттяЗамовленняКлієнта_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗакриттяЗамовленняКлієнта_Pointer(unigueID));
        }

        protected override bool IsExportXML() { return false; } //Дозволити експорт документу
        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ЗакриттяЗамовленняКлієнта_Pointer Вказівник = new ЗакриттяЗамовленняКлієнта_Pointer(unigueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await ЗакриттяЗамовленняКлієнта_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        /*
        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await ЗакриттяЗамовленняКлієнта_Друк.PDF(unigueID);
        }
        */

        #endregion
    }
}
