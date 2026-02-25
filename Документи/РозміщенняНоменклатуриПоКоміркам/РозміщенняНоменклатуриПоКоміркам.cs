
/*
        РозміщенняНоменклатуриПоКоміркам.cs
*/

using Gtk;
using InterfaceGtk3;
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
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.ДодатиВідбір(TreeViewGrid, РозміщенняНоменклатуриПоКоміркам_Функції.Відбори(searchText));

            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            await РозміщенняНоменклатуриПоКоміркам_Функції.OpenPageElement(IsNew, uniqueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            await РозміщенняНоменклатуриПоКоміркам_Функції.SetDeletionLabel(uniqueID);
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            return await РозміщенняНоменклатуриПоКоміркам_Функції.Copy(uniqueID);
        }

        protected override async ValueTask VersionsHistory(UniqueID uniqueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new РозміщенняНоменклатуриПоКоміркам_Pointer(uniqueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.РозміщенняНоменклатуриПоКоміркам";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, РозміщенняНоменклатуриПоКоміркам_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UniqueID uniqueID, bool spendDoc)
        {
            РозміщенняНоменклатуриПоКоміркам_Objest? Обєкт = await new РозміщенняНоменклатуриПоКоміркам_Pointer(uniqueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РозміщенняНоменклатуриПоКоміркам_Pointer(uniqueID));
        }

        protected override async ValueTask ExportXML(UniqueID uniqueID, string pathToFolder)
        {
            РозміщенняНоменклатуриПоКоміркам_Pointer Вказівник = new РозміщенняНоменклатуриПоКоміркам_Pointer(uniqueID);
            await Вказівник.GetPresentation();

            await РозміщенняНоменклатуриПоКоміркам_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        #endregion
    }
}