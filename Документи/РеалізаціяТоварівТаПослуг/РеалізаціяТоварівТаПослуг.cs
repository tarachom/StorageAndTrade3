
/*
        РеалізаціяТоварівТаПослуг.cs
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;
using GeneratedCode;

namespace StorageAndTrade
{
    public class РеалізаціяТоварівТаПослуг : ДокументЖурнал
    {
        public РеалізаціяТоварівТаПослуг()
        {
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.ДодатиВідбір(TreeViewGrid, РеалізаціяТоварівТаПослуг_Функції.Відбори(searchText));

            await ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await РеалізаціяТоварівТаПослуг_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await РеалізаціяТоварівТаПослуг_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await РеалізаціяТоварівТаПослуг_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new РеалізаціяТоварівТаПослуг_Pointer(unigueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.РеалізаціяТоварівТаПослуг";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, РеалізаціяТоварівТаПослуг_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            РеалізаціяТоварівТаПослуг_Objest? Обєкт = await new РеалізаціяТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РеалізаціяТоварівТаПослуг_Pointer(unigueID));
        }

        protected override bool IsExportXML() { return true; } //Дозволити експорт документу

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            РеалізаціяТоварівТаПослуг_Pointer Вказівник = new РеалізаціяТоварівТаПослуг_Pointer(unigueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await РеалізаціяТоварівТаПослуг_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await РеалізаціяТоварівТаПослуг_Друк.PDF(unigueID);
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

            {
                MenuItem doc = new MenuItem(ПоверненняТоварівВідКлієнта_Const.FULLNAME);
                doc.Activated += НаОснові_ПоверненняТоварівВідКлієнта;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ЗбіркаТоварівНаСкладі_Const.FULLNAME);
                doc.Activated += НаОснові_ЗбіркаТоварівНаСкладі;
                Menu.Append(doc);
            }

            Menu.ShowAll();

            return Menu;
        }

        async void НаОснові_ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                РеалізаціяТоварівТаПослуг_Objest? Обєкт = await new РеалізаціяТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(false);
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

        async void НаОснові_ПоверненняТоварівВідКлієнта(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                РеалізаціяТоварівТаПослуг_Objest? Обєкт = await new РеалізаціяТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ПоверненняТоварівВідКлієнта_Objest Новий = new ПоверненняТоварівВідКлієнта_Objest();
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
                    int sequenceNumber = 0;
                    //Товари
                    foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new ПоверненняТоварівВідКлієнта_Товари_TablePart.Record()
                        {
                            НомерРядка = ++sequenceNumber,
                            Номенклатура = record.Номенклатура,
                            ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                            Серія = record.Серія,
                            Пакування = record.Пакування,
                            КількістьУпаковок = record.КількістьУпаковок,
                            Кількість = record.Кількість,
                            Ціна = record.Ціна,
                            Сума = record.Сума,
                            ДокументРеалізації = Обєкт.GetDocumentPointer()
                        });
                    }

                    await Новий.Товари_TablePart.Save(false);
                }

                await ПоверненняТоварівВідКлієнта_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_ЗбіркаТоварівНаСкладі(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                РеалізаціяТоварівТаПослуг_Objest? Обєкт = await new РеалізаціяТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(true);
                if (Обєкт == null) continue;

                //
                //Новий документ
                //

                ЗбіркаТоварівНаСкладі_Objest Новий = new ЗбіркаТоварівНаСкладі_Objest();
                await Новий.New();
                Новий.Організація = Обєкт.Організація;
                Новий.Склад = Обєкт.Склад;
                Новий.Основа = Обєкт.GetBasis();
                Новий.ДокументРеалізації = Обєкт.GetDocumentPointer();

                if (await Новий.Save())
                {
                    int sequenceNumber = 0;
                    //Товари
                    foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                    {
                        Новий.Товари_TablePart.Records.Add(new ЗбіркаТоварівНаСкладі_Товари_TablePart.Record()
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

                await ЗбіркаТоварівНаСкладі_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        #endregion
    }
}