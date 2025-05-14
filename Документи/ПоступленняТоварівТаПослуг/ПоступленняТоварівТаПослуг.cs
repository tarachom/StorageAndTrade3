
/*

        ПоступленняТоварівТаПослуг.cs

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
    public class ПоступленняТоварівТаПослуг : ДокументЖурнал
    {
        public ПоступленняТоварівТаПослуг()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбір(TreeViewGrid, ПоступленняТоварівТаПослуг_Функції.Відбори(searchText));

            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await ПоступленняТоварівТаПослуг_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);

        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await ПоступленняТоварівТаПослуг_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await ПоступленняТоварівТаПослуг_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new ПоступленняТоварівТаПослуг_Pointer(unigueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.ПоступленняТоварівТаПослуг";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, ПоступленняТоварівТаПослуг_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПоступленняТоварівТаПослуг_Pointer(unigueID));
        }

        protected override bool IsExportXML() { return true; } //Дозволити експорт документу

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            ПоступленняТоварівТаПослуг_Pointer Вказівник = new ПоступленняТоварівТаПослуг_Pointer(unigueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await ПоступленняТоварівТаПослуг_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await ПоступленняТоварівТаПослуг_Друк.PDF(unigueID);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            {
                MenuItem doc = new MenuItem(РозхіднийКасовийОрдер_Const.FULLNAME);
                doc.Activated += НаОснові_РозхіднийКасовийОрдер;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ПоверненняТоварівПостачальнику_Const.FULLNAME);
                doc.Activated += НаОснові_ПоверненняТоварівПостачальнику;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(РозміщенняТоварівНаСкладі_Const.FULLNAME);
                doc.Activated += НаОснові_РозміщенняТоварівНаСкладі;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ВнутрішнєСпоживанняТоварів_Const.FULLNAME);
                doc.Activated += НаОснові_ВнутрішнєСпоживанняТоварів;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ПереміщенняТоварів_Const.FULLNAME);
                doc.Activated += НаОснові_ПереміщенняТоварів;
                Menu.Append(doc);
            }

            Menu.ShowAll();
            return Menu;
        }

        async void НаОснові_РозхіднийКасовийОрдер(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(false);
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
                Новий.ГосподарськаОперація = ГосподарськіОперації.ОплатаПостачальнику;

                await Новий.Save();

                await РозхіднийКасовийОрдер_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_ПоверненняТоварівПостачальнику(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ПоверненняТоварівПостачальнику_Objest Новий = new ПоверненняТоварівПостачальнику_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Каса = Обєкт.Каса;
                Новий.Контрагент = Обєкт.Контрагент;
                Новий.Договір = Обєкт.Договір;
                Новий.Склад = Обєкт.Склад;
                Новий.СумаДокументу = Обєкт.СумаДокументу;
                Новий.Основа = Обєкт.GetBasis();

                if (await Новий.Save())
                {
                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new ПоверненняТоварівПостачальнику_Товари_TablePart.Record()
                        {
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Серія = record.Серія,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість,
                            Ціна = record.Ціна,
                            Сума = record.Сума,
                            ДокументПоступлення = Обєкт.GetDocumentPointer()
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                await ПоверненняТоварівПостачальнику_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_РозміщенняТоварівНаСкладі(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                РозміщенняТоварівНаСкладі_Objest Новий = new РозміщенняТоварівНаСкладі_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Склад = Обєкт.Склад;
                Новий.Автор = Обєкт.Автор;
                Новий.Підрозділ = Обєкт.Підрозділ;
                Новий.Основа = Обєкт.GetBasis();
                Новий.ДокументПоступлення = Обєкт.GetDocumentPointer();

                if (await Новий.Save())
                {
                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new РозміщенняТоварівНаСкладі_Товари_TablePart.Record()
                        {
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Серія = record.Серія,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                await РозміщенняТоварівНаСкладі_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_ВнутрішнєСпоживанняТоварів(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ВнутрішнєСпоживанняТоварів_Objest Новий = new ВнутрішнєСпоживанняТоварів_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Склад = Обєкт.Склад;
                Новий.Валюта = Обєкт.Валюта;
                Новий.Автор = Обєкт.Автор;
                Новий.Підрозділ = Обєкт.Підрозділ;
                Новий.Основа = Обєкт.GetBasis();

                if (await Новий.Save())
                {
                    int sequenceNumber = 0;
                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record()
                        {
                            НомерРядка = ++sequenceNumber,
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Серія = record.Серія,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість,
                            Ціна = record.Ціна,
                            Сума = record.Сума
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                await ВнутрішнєСпоживанняТоварів_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_ПереміщенняТоварів(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ПереміщенняТоварів_Objest Новий = new ПереміщенняТоварів_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.СкладВідправник = Обєкт.Склад;
                Новий.Автор = Обєкт.Автор;
                Новий.Підрозділ = Обєкт.Підрозділ;
                Новий.Основа = Обєкт.GetBasis();

                if (await Новий.Save())
                {
                    int sequenceNumber = 0;
                    //Товари
                    foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new ПереміщенняТоварів_Товари_TablePart.Record()
                        {
                            НомерРядка = ++sequenceNumber,
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Серія = record.Серія,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                await ПереміщенняТоварів_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        #endregion
    }
}