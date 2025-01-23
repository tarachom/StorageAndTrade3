
/*
        РозміщенняНоменклатуриПоКоміркам.cs
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;
using GeneratedCode.Документи;
using GeneratedCode;

namespace StorageAndTrade
{
    public class РозміщенняНоменклатуриПоКоміркам : ДокументЖурнал
    {
        public РозміщенняНоменклатуриПоКоміркам()
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.AddColumns(TreeViewGrid);
            Config.Kernel.DocumentObjectChanged += async (object? sender, Dictionary<string, List<Guid>> document) =>
            {
                if (document.Any((x) => x.Key == РозміщенняНоменклатуриПоКоміркам_Const.TYPE))
                    await LoadRecords();
            };
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.ДодатиВідбір(TreeViewGrid, РозміщенняНоменклатуриПоКоміркам_Функції.Відбори(searchText));

            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await РозміщенняНоменклатуриПоКоміркам_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await РозміщенняНоменклатуриПоКоміркам_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await РозміщенняНоменклатуриПоКоміркам_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.РозміщенняНоменклатуриПоКоміркам";

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
            РозміщенняНоменклатуриПоКоміркам_Objest? Обєкт = await new РозміщенняНоменклатуриПоКоміркам_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РозміщенняНоменклатуриПоКоміркам_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            РозміщенняНоменклатуриПоКоміркам_Pointer Вказівник = new РозміщенняНоменклатуриПоКоміркам_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await РозміщенняНоменклатуриПоКоміркам_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}