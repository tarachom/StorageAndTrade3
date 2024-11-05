

/*     
        ЗакриттяРахункуФактури.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class ЗакриттяРахункуФактури : ДокументЖурнал
    {
        public ЗакриттяРахункуФактури() : base()
        {
            ТабличніСписки.ЗакриттяРахункуФактури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗакриттяРахункуФактури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗакриттяРахункуФактури_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗакриттяРахункуФактури_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗакриттяРахункуФактури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗакриттяРахункуФактури_Записи.ОчиститиВідбір(TreeViewGrid);
            
            //Відбори
            ТабличніСписки.ЗакриттяРахункуФактури_Записи.ДодатиВідбір(TreeViewGrid, ЗакриттяРахункуФактури_Функції.Відбори(searchText));

            await ТабличніСписки.ЗакриттяРахункуФактури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ЗакриттяРахункуФактури_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ЗакриттяРахункуФактури_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ЗакриттяРахункуФактури_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ЗакриттяРахункуФактури_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗакриттяРахункуФактури";

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
            ЗакриттяРахункуФактури_Objest? Обєкт = await new ЗакриттяРахункуФактури_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗакриттяРахункуФактури_Pointer(unigueID));
        }
        
        protected override bool IsExportXML() { return false; } //Дозволити експорт документу
        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ЗакриттяРахункуФактури_Pointer Вказівник = new ЗакриттяРахункуФактури_Pointer(unigueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await ЗакриттяРахункуФактури_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        /*
        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await ЗакриттяРахункуФактури_Друк.PDF(unigueID);
        }
        */

        #endregion
    }
}
    