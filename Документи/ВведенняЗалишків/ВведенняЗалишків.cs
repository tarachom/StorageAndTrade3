
/*

        ВведенняЗалишків.cs

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ВведенняЗалишків : ДокументЖурнал
    {
        public ВведенняЗалишків()
        {
            ТабличніСписки.ВведенняЗалишків_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВведенняЗалишків_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ВведенняЗалишків_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВведенняЗалишків_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВведенняЗалишків_Записи.ДодатиВідбір(TreeViewGrid, ВведенняЗалишків_Функції.Відбори(searchText));

            await ТабличніСписки.ВведенняЗалишків_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ВведенняЗалишків_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ВведенняЗалишків_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ВведенняЗалишків_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ВведенняЗалишків_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ВведенняЗалишків_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ВведенняЗалишків";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ВведенняЗалишків_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ВведенняЗалишків_Objest? Обєкт = await new ВведенняЗалишків_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ВведенняЗалишків_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ВведенняЗалишків_Pointer Вказівник = new ВведенняЗалишків_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await ВведенняЗалишків_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}