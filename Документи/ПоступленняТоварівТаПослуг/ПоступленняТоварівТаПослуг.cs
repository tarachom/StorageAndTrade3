
/*

        ПоступленняТоварівТаПослуг.cs

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;
using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    public class ПоступленняТоварівТаПослуг : ДокументЖурнал
    {
        public ПоступленняТоварівТаПослуг()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);
            Config.Kernel.DocumentObjectChanged += async (object? sender, Dictionary<string, List<Guid>> document) =>
            {
                if (document.Any((x) => x.Key == ПоступленняТоварівТаПослуг_Const.TYPE))
                    await LoadRecords();
            };
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбір(TreeViewGrid, ПоступленняТоварівТаПослуг_Функції.Відбори(searchText));

            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CreateFilter(TreeViewGrid);
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

        const string КлючНалаштуванняКористувача = "Документи.ПоступленняТоварівТаПослуг";

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

                РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент();
                await page.Елемент.Read(Новий.UnigueID);
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                page.SetValue();
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

                ПоверненняТоварівПостачальнику_Елемент page = new ПоверненняТоварівПостачальнику_Елемент();
                await page.Елемент.Read(Новий.UnigueID);
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                page.SetValue();
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

                РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент();
                await page.Елемент.Read(Новий.UnigueID);
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                page.SetValue();
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

                ВнутрішнєСпоживанняТоварів_Елемент page = new ВнутрішнєСпоживанняТоварів_Елемент();
                await page.Елемент.Read(Новий.UnigueID);
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                page.SetValue();
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

                ПереміщенняТоварів_Елемент page = new ПереміщенняТоварів_Елемент();
                await page.Елемент.Read(Новий.UnigueID);
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                page.SetValue();
            }
        }

        #endregion
    }
}