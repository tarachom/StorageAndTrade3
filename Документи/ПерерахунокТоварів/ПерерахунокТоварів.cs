

/*     
        ПерерахунокТоварів.cs
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
    public class ПерерахунокТоварів : ДокументЖурнал
    {
        public ПерерахунокТоварів()
        {
            ТабличніСписки.ПерерахунокТоварів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПерерахунокТоварів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПерерахунокТоварів_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПерерахунокТоварів_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПерерахунокТоварів_Записи.ДодатиВідбір(TreeViewGrid, ПерерахунокТоварів_Функції.Відбори(searchText));

            await ТабличніСписки.ПерерахунокТоварів_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ПерерахунокТоварів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ПерерахунокТоварів_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПерерахунокТоварів_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПерерахунокТоварів_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ПерерахунокТоварів_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ПерерахунокТоварів";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ПерерахунокТоварів_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПерерахунокТоварів_Objest? Обєкт = await new ПерерахунокТоварів_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПерерахунокТоварів_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ПерерахунокТоварів_Pointer Вказівник = new ПерерахунокТоварів_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await ПерерахунокТоварів_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}
