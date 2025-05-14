
/*

        ЗамовленняКлієнта.cs

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;
using GeneratedCode;

namespace StorageAndTrade
{
    public class ЗамовленняКлієнта : ДокументЖурнал
    {
        public ЗамовленняКлієнта()
        {
            ТабличніСписки.ЗамовленняКлієнта_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ЗамовленняКлієнта_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗамовленняКлієнта_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ЗамовленняКлієнта_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ЗамовленняКлієнта_Записи.ДодатиВідбір(TreeViewGrid, ЗамовленняКлієнта_Функції.Відбори(searchText));

            await ТабличніСписки.ЗамовленняКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ЗамовленняКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ЗамовленняКлієнта_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ЗамовленняКлієнта_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ЗамовленняКлієнта_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ЗамовленняКлієнта_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ЗамовленняКлієнта_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ЗамовленняКлієнта_Pointer(unigueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗамовленняКлієнта";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ЗамовленняКлієнта_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ЗамовленняКлієнта_Objest? Обєкт = await new ЗамовленняКлієнта_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗамовленняКлієнта_Pointer(unigueID));
        }

        protected override bool IsExportXML() { return true; } //Дозволити експорт документу

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ЗамовленняКлієнта_Pointer Вказівник = new ЗамовленняКлієнта_Pointer(unigueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await ЗамовленняКлієнта_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await ЗамовленняКлієнта_Друк.PDF(unigueID);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            {
                MenuItem doc = new MenuItem(РеалізаціяТоварівТаПослуг_Const.FULLNAME);
                doc.Activated += НаОснові_РеалізаціяТоварівТаПослуг;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ЗамовленняПостачальнику_Const.FULLNAME);
                doc.Activated += НаОснові_ЗамовленняПостачальнику;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ПоступленняТоварівТаПослуг_Const.FULLNAME);
                doc.Activated += НаОснові_ПоступленняТоварівТаПослуг;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ПрихіднийКасовийОрдер_Const.FULLNAME);
                doc.Activated += НаОснові_ПрихіднийКасовийОрдер;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ЗакриттяЗамовленняКлієнта_Const.FULLNAME);
                doc.Activated += НаОснові_ЗакриттяЗамовленняКлієнта;
                Menu.Append(doc);
            }

            Menu.ShowAll();

            return Menu;
        }

        async void НаОснові_РеалізаціяТоварівТаПослуг(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ЗамовленняКлієнта_Objest? Обєкт = await new ЗамовленняКлієнта_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                РеалізаціяТоварівТаПослуг_Objest Новий = new РеалізаціяТоварівТаПослуг_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Каса = Обєкт.Каса;
                Новий.Контрагент = Обєкт.Контрагент;
                Новий.Договір = Обєкт.Договір;
                Новий.Склад = Обєкт.Склад;
                Новий.СумаДокументу = Обєкт.СумаДокументу;
                Новий.Статус = СтатусиРеалізаціїТоварівТаПослуг.ДоОплати;
                Новий.ФормаОплати = Обєкт.ФормаОплати;
                Новий.Основа = Обєкт.GetBasis();

                if (await Новий.Save())
                {
                    int sequenceNumber = 0;
                    //Товари
                    foreach (ЗамовленняКлієнта_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new РеалізаціяТоварівТаПослуг_Товари_TablePart.Record()
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
                            ЗамовленняКлієнта = Обєкт.GetDocumentPointer(),
                            Склад = Обєкт.Склад,
                            ВидЦіни = record.ВидЦіни
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                await РеалізаціяТоварівТаПослуг_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_ЗамовленняПостачальнику(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ЗамовленняКлієнта_Objest? Обєкт = await new ЗамовленняКлієнта_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ЗамовленняПостачальнику_Objest Новий = new ЗамовленняПостачальнику_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Каса = Обєкт.Каса;
                Новий.Контрагент = Обєкт.Контрагент;
                Новий.Договір = await ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Обєкт.Контрагент, ТипДоговорів.ЗПостачальниками) ?? Обєкт.Договір;
                Новий.Склад = Обєкт.Склад;
                Новий.СумаДокументу = Обєкт.СумаДокументу;
                Новий.Статус = СтатусиЗамовленьПостачальникам.Підтверджений;
                Новий.ФормаОплати = Обєкт.ФормаОплати;
                Новий.Основа = Обєкт.GetBasis();

                if (await Новий.Save())
                {
                    int sequenceNumber = 0;
                    //Товари
                    foreach (ЗамовленняКлієнта_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new ЗамовленняПостачальнику_Товари_TablePart.Record()
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
                            Склад = Обєкт.Склад,
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                await ЗамовленняПостачальнику_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_ПоступленняТоварівТаПослуг(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ЗамовленняКлієнта_Objest? Обєкт = await new ЗамовленняКлієнта_Pointer(unigueID).GetDocumentObject(true);
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
                Новий.Договір = await ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Обєкт.Контрагент, ТипДоговорів.ЗПостачальниками) ?? Обєкт.Договір;
                Новий.Склад = Обєкт.Склад;
                Новий.СумаДокументу = Обєкт.СумаДокументу;
                Новий.ФормаОплати = Обєкт.ФормаОплати;
                Новий.Основа = Обєкт.GetBasis();

                if (await Новий.Save())
                {
                    int sequenceNumber = 0;
                    //Товари
                    foreach (ЗамовленняКлієнта_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
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
                            Склад = Обєкт.Склад
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                await ПоступленняТоварівТаПослуг_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ЗамовленняКлієнта_Objest? Обєкт = await new ЗамовленняКлієнта_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ПрихіднийКасовийОрдер_Objest Новий = new ПрихіднийКасовийОрдер_Objest();
                await Новий.New();
                Новий.ГосподарськаОперація = ГосподарськіОперації.ПоступленняОплатиВідКлієнта;
                Новий.Організація = Обєкт.Організація;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Каса = Обєкт.Каса;
                Новий.Контрагент = Обєкт.Контрагент;
                Новий.Договір = Обєкт.Договір;
                Новий.СумаДокументу = Обєкт.СумаДокументу;
                Новий.Основа = Обєкт.GetBasis();

                await Новий.Save();

                await ПрихіднийКасовийОрдер_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_ЗакриттяЗамовленняКлієнта(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ЗамовленняКлієнта_Objest? Обєкт = await new ЗамовленняКлієнта_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ЗакриттяЗамовленняКлієнта_Objest Новий = new ЗакриттяЗамовленняКлієнта_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Каса = Обєкт.Каса;
                Новий.Контрагент = Обєкт.Контрагент;
                Новий.Договір = Обєкт.Договір;
                Новий.Склад = Обєкт.Склад;
                Новий.СумаДокументу = Обєкт.СумаДокументу;
                Новий.ЗамовленняКлієнта = Обєкт.GetDocumentPointer();
                Новий.Основа = Обєкт.GetBasis();

                if (await Новий.Save())
                {
                    int sequenceNumber = 0;
                    //Товари
                    foreach (ЗамовленняКлієнта_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new ЗакриттяЗамовленняКлієнта_Товари_TablePart.Record()
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

                await ЗакриттяЗамовленняКлієнта_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        #endregion
    }
}