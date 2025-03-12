
/*
        ЗбіркаТоварівНаСкладі.cs
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ЗбіркаТоварівНаСкладі : ДокументЖурнал
    {
        public ЗбіркаТоварівНаСкладі()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid, ЗбіркаТоварівНаСкладі_Функції.Відбори(searchText));

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ЗбіркаТоварівНаСкладі_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ЗбіркаТоварівНаСкладі_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ЗбіркаТоварівНаСкладі_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗбіркаТоварівНаСкладі";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, ЗбіркаТоварівНаСкладі_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ЗбіркаТоварівНаСкладі_Objest? Обєкт = await new ЗбіркаТоварівНаСкладі_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗбіркаТоварівНаСкладі_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ЗбіркаТоварівНаСкладі_Pointer Вказівник = new ЗбіркаТоварівНаСкладі_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await ЗбіркаТоварівНаСкладі_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}