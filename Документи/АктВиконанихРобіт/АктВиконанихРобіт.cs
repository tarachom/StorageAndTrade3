
/*

        АктВиконанихРобіт.cs
        
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class АктВиконанихРобіт : ДокументЖурнал
    {
        public АктВиконанихРобіт()
        {
            ТабличніСписки.АктВиконанихРобіт_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.АктВиконанихРобіт_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.АктВиконанихРобіт_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.АктВиконанихРобіт_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.АктВиконанихРобіт_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.АктВиконанихРобіт_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.АктВиконанихРобіт_Записи.ДодатиВідбір(TreeViewGrid, АктВиконанихРобіт_Функції.Відбори(searchText));

            await ТабличніСписки.АктВиконанихРобіт_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.АктВиконанихРобіт_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await АктВиконанихРобіт_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await АктВиконанихРобіт_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await АктВиконанихРобіт_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.АктВиконанихРобіт";

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
            АктВиконанихРобіт_Objest? Обєкт = await new АктВиконанихРобіт_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new АктВиконанихРобіт_Pointer(unigueID));
        }

        protected override bool IsExportXML() { return true; } //Дозволити експорт документу

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            АктВиконанихРобіт_Pointer Вказівник = new АктВиконанихРобіт_Pointer(unigueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await АктВиконанихРобіт_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await АктВиконанихРобіт_Друк.PDF(unigueID);
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
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                АктВиконанихРобіт_Objest? Обєкт = await new АктВиконанихРобіт_Pointer(unigueID).GetDocumentObject(false);
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

                ПрихіднийКасовийОрдер_Елемент page = new ПрихіднийКасовийОрдер_Елемент();
                await page.Елемент.Read(Новий.UnigueID);
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                page.SetValue();
            }
        }
    }

    #endregion
}
