
/*
        РозхіднийКасовийОрдер.cs
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
    public class РозхіднийКасовийОрдер : ДокументЖурнал
    {
        public РозхіднийКасовийОрдер()
        {
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозхіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.ДодатиВідбір(TreeViewGrid, РозхіднийКасовийОрдер_Функції.Відбори(searchText));

            await ТабличніСписки.РозхіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.РозхіднийКасовийОрдер_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.РозхіднийКасовийОрдер_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.РозхіднийКасовийОрдер_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await РозхіднийКасовийОрдер_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await РозхіднийКасовийОрдер_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await РозхіднийКасовийОрдер_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new РозхіднийКасовийОрдер_Pointer(unigueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.РозхіднийКасовийОрдер";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, РозхіднийКасовийОрдер_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            РозхіднийКасовийОрдер_Objest? Обєкт = await new РозхіднийКасовийОрдер_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РозхіднийКасовийОрдер_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            РозхіднийКасовийОрдер_Pointer Вказівник = new РозхіднийКасовийОрдер_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await РозхіднийКасовийОрдер_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await РозхіднийКасовийОрдер_Друк.PDF(unigueID);
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
                MenuItem doc = new MenuItem(ПрихіднийКасовийОрдер_Const.FULLNAME);
                doc.Activated += НаОснові_ПрихіднийКасовийОрдер;
                Menu.Append(doc);
            }

            Menu.ShowAll();
            return Menu;
        }

        async void НаОснові_РозхіднийКасовийОрдер(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                РозхіднийКасовийОрдер_Objest? Обєкт = await new РозхіднийКасовийОрдер_Pointer(unigueID).GetDocumentObject(false);
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
                Новий.ГосподарськаОперація = Обєкт.ГосподарськаОперація;

                await Новий.Save();

                await РозхіднийКасовийОрдер_Функції.OpenPageElement(false, Новий.UnigueID);
            }
        }

        async void НаОснові_ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            foreach (UnigueID unigueID in GetSelectedRows())
            {
                РозхіднийКасовийОрдер_Objest? Обєкт = await new РозхіднийКасовийОрдер_Pointer(unigueID).GetDocumentObject(false);
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

        #endregion
    }
}