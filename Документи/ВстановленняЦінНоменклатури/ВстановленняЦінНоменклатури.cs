
/*

        ВстановленняЦінНоменклатури.cs

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ВстановленняЦінНоменклатури : ДокументЖурнал
    {
        public ВстановленняЦінНоменклатури()
        {
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ВстановленняЦінНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ВстановленняЦінНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ВстановленняЦінНоменклатури_Функції.Відбори(searchText));

            await ТабличніСписки.ВстановленняЦінНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ВстановленняЦінНоменклатури_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ВстановленняЦінНоменклатури_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ВстановленняЦінНоменклатури_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ВстановленняЦінНоменклатури_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ВстановленняЦінНоменклатури";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ВстановленняЦінНоменклатури_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ВстановленняЦінНоменклатури_Objest? Обєкт = await new ВстановленняЦінНоменклатури_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ВстановленняЦінНоменклатури_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ВстановленняЦінНоменклатури_Pointer Вказівник = new ВстановленняЦінНоменклатури_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await ВстановленняЦінНоменклатури_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}