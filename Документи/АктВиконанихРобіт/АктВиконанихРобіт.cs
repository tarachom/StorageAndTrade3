
/*

        АктВиконанихРобіт.cs
        
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class АктВиконанихРобіт : ДокументЖурнал
    {
        public АктВиконанихРобіт()
        {
            ТабличніСписки.АктВиконанихРобіт_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.АктВиконанихРобіт_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.АктВиконанихРобіт_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.АктВиконанихРобіт_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.АктВиконанихРобіт_Записи.ДодатиВідбір(TreeViewGrid, АктВиконанихРобіт_Функції.Відбори(searchText));

            await ТабличніСписки.АктВиконанихРобіт_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.АктВиконанихРобіт_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.АктВиконанихРобіт_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.АктВиконанихРобіт_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await АктВиконанихРобіт_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await АктВиконанихРобіт_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await АктВиконанихРобіт_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new АктВиконанихРобіт_Pointer(uniqueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.АктВиконанихРобіт";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, АктВиконанихРобіт_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UniqueID uniqueID, bool spendDoc)
        {
            АктВиконанихРобіт_Objest? Обєкт = await new АктВиконанихРобіт_Pointer(uniqueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new АктВиконанихРобіт_Pointer(uniqueID));
        }

        protected override bool IsExportXML() { return true; } //Дозволити експорт документу

        protected override async ValueTask ExportXML(UniqueID uniqueID, string pathToFolder)
        {
            АктВиконанихРобіт_Pointer Вказівник = new АктВиконанихРобіт_Pointer(uniqueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await АктВиконанихРобіт_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        protected override bool IsExportExcel() { return true; } //Дозволити експорт документу

        protected override async ValueTask ExportExcel(UniqueID uniqueID, string pathToFolder)
        {
            АктВиконанихРобіт_Pointer Вказівник = new АктВиконанихРобіт_Pointer(uniqueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xslx");

            АктВиконанихРобіт_Objest? Обєкт = await Вказівник.GetDocumentObject(true);
            if (Обєкт != null)
            {
                foreach (АктВиконанихРобіт_Послуги_TablePart.Record record in Обєкт.Послуги_TablePart.Records)
                {

                }
            }

            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        protected override async ValueTask PrintingDoc(UniqueID uniqueID)
        {
            await АктВиконанихРобіт_Друк.PDF(uniqueID);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            {
                MenuItem doc = new MenuItem(ПрихіднийКасовийОрдер_Const.FULLNAME);
                doc.Activated += НаОснові_ПрихіднийКасовийОрдер;
                Menu.Append(doc);
            }

            Menu.ShowAll();

            return Menu;
        }

        async void НаОснові_ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            foreach (UniqueID uniqueID in GetSelectedRows())
            {
                АктВиконанихРобіт_Objest? Обєкт = await new АктВиконанихРобіт_Pointer(uniqueID).GetDocumentObject(false);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ПрихіднийКасовийОрдер_Objest Новий = new ПрихіднийКасовийОрдер_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Каса = Обєкт.Каса;
                Новий.Контрагент = Обєкт.Контрагент;
                Новий.Договір = Обєкт.Договір;
                Новий.Основа = Обєкт.GetBasis();
                Новий.СумаДокументу = Обєкт.СумаДокументу;

                await Новий.Save();

                await ПрихіднийКасовийОрдер_Функції.OpenPageElement(false, Новий.UniqueID);
            }
        }
    }

    #endregion
}
