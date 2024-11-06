
/*

        ЗамовленняПостачальнику.cs

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class ЗамовленняПостачальнику : ДокументЖурнал
    {
        public ЗамовленняПостачальнику()
        {
            ТабличніСписки.ЗамовленняПостачальнику_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗамовленняПостачальнику_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗамовленняПостачальнику_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗамовленняПостачальнику_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗамовленняПостачальнику_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗамовленняПостачальнику_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ЗамовленняПостачальнику_Записи.ДодатиВідбір(TreeViewGrid, ЗамовленняПостачальнику_Функції.Відбори(searchText));

            await ТабличніСписки.ЗамовленняПостачальнику_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ЗамовленняПостачальнику_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ЗамовленняПостачальнику_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ЗамовленняПостачальнику_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ЗамовленняПостачальнику_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗамовленняПостачальнику";

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
            ЗамовленняПостачальнику_Objest? Обєкт = await new ЗамовленняПостачальнику_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗамовленняПостачальнику_Pointer(unigueID));
        }

        protected override bool IsExportXML() { return true; } //Дозволити експорт документу

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ЗамовленняПостачальнику_Pointer Вказівник = new ЗамовленняПостачальнику_Pointer(unigueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await ЗамовленняПостачальнику_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await ЗамовленняПостачальнику_Друк.PDF(unigueID);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            {
                MenuItem doc = new MenuItem(ПоступленняТоварівТаПослуг_Const.FULLNAME);
                doc.Activated += НаОснові_ПоступленняТоварівТаПослуг;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(РозхіднийКасовийОрдер_Const.FULLNAME);
                doc.Activated += НаОснові_РозхіднийКасовийОрдер;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ЗакриттяЗамовленняПостачальнику_Const.FULLNAME);
                doc.Activated += НаОснові_ЗакриттяЗамовленняПостачальнику;
                Menu.Append(doc);
            }

            Menu.ShowAll();

            return Menu;
        }

        async void НаОснові_ПоступленняТоварівТаПослуг(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ЗамовленняПостачальнику_Objest? Обєкт = await new ЗамовленняПостачальнику_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ПоступленняТоварівТаПослуг_Objest Новий = new ПоступленняТоварівТаПослуг_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Каса = Обєкт.Каса;
                Новий.Контрагент = Обєкт.Контрагент;
                Новий.Договір = Обєкт.Договір;
                Новий.Склад = Обєкт.Склад;
                Новий.СумаДокументу = Обєкт.СумаДокументу;
                Новий.ФормаОплати = Обєкт.ФормаОплати;
                Новий.ЗамовленняПостачальнику = Обєкт.GetDocumentPointer();
                Новий.Основа = Обєкт.GetBasis();

                if (await Новий.Save())
                {
                    int sequenceNumber = 0;
                    //Товари
                    foreach (ЗамовленняПостачальнику_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new ПоступленняТоварівТаПослуг_Товари_TablePart.Record()
                        {
                            НомерРядка = ++sequenceNumber,
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість,
                            Ціна = record.Ціна,
                            Сума = record.Сума,
                            Скидка = record.Скидка,
                            ЗамовленняПостачальнику = Обєкт.GetDocumentPointer(),
                            Склад = Обєкт.Склад
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                ПоступленняТоварівТаПослуг_Елемент page = new ПоступленняТоварівТаПослуг_Елемент();
                await page.Елемент.Read(Новий.UnigueID);
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                page.SetValue();
            }
        }

        async void НаОснові_РозхіднийКасовийОрдер(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ЗамовленняПостачальнику_Objest? Обєкт = await new ЗамовленняПостачальнику_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                РозхіднийКасовийОрдер_Objest Новий = new РозхіднийКасовийОрдер_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Каса = Обєкт.Каса;
                Новий.Контрагент = Обєкт.Контрагент;
                Новий.Договір = Обєкт.Договір;
                Новий.СумаДокументу = Обєкт.СумаДокументу;
                Новий.Основа = Обєкт.GetBasis();

                await Новий.Save();

                РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент();
                await page.Елемент.Read(Новий.UnigueID);
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                page.SetValue();
            }
        }

        async void НаОснові_ЗакриттяЗамовленняПостачальнику(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ЗамовленняПостачальнику_Objest? Обєкт = await new ЗамовленняПостачальнику_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ЗакриттяЗамовленняПостачальнику_Objest Новий = new ЗакриттяЗамовленняПостачальнику_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Каса = Обєкт.Каса;
                Новий.Контрагент = Обєкт.Контрагент;
                Новий.Договір = Обєкт.Договір;
                Новий.Склад = Обєкт.Склад;
                Новий.СумаДокументу = Обєкт.СумаДокументу;
                Новий.ЗамовленняПостачальнику = Обєкт.GetDocumentPointer();
                Новий.Основа = Обєкт.GetBasis();

                if (await Новий.Save())
                {
                    int sequenceNumber = 0;
                    //Товари
                    foreach (ЗамовленняПостачальнику_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new ЗакриттяЗамовленняПостачальнику_Товари_TablePart.Record()
                        {
                            НомерРядка = ++sequenceNumber,
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість,
                            Ціна = record.Ціна,
                            Сума = record.Сума
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                ЗакриттяЗамовленняПостачальнику_Елемент page = new ЗакриттяЗамовленняПостачальнику_Елемент();
                await page.Елемент.Read(Новий.UnigueID);
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                page.SetValue();
            }
        }

        #endregion
    }
}