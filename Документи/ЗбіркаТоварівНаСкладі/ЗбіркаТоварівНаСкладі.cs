
/*
        ЗбіркаТоварівНаСкладі.cs
*/

using Gtk;
using InterfaceGtk3;
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
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid, ЗбіркаТоварівНаСкладі_Функції.Відбори(searchText));

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await ЗбіркаТоварівНаСкладі_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await ЗбіркаТоварівНаСкладі_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await ЗбіркаТоварівНаСкладі_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ЗбіркаТоварівНаСкладі_Pointer(uniqueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗбіркаТоварівНаСкладі";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ЗбіркаТоварівНаСкладі_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UniqueID uniqueID, bool spendDoc)
        {
            ЗбіркаТоварівНаСкладі_Objest? Обєкт = await new ЗбіркаТоварівНаСкладі_Pointer(uniqueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗбіркаТоварівНаСкладі_Pointer(uniqueID));
        }

        protected override async ValueTask ExportXML(UniqueID uniqueID, string pathToFolder)
        {
            ЗбіркаТоварівНаСкладі_Pointer Вказівник = new ЗбіркаТоварівНаСкладі_Pointer(uniqueID);
            await Вказівник.GetPresentation();

            await ЗбіркаТоварівНаСкладі_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}