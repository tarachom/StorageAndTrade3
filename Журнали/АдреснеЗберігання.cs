
/*
        АдреснеЗберігання.cs
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;
using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    class Журнал_АдреснеЗберігання : Журнал
    {
        public Журнал_АдреснеЗберігання() : base(Config.NameSpageCodeGeneration)
        {
            ТабличніСписки.Журнали_АдреснеЗберігання.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Журнали_АдреснеЗберігання.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Журнали_АдреснеЗберігання.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period);
            await ТабличніСписки.Журнали_АдреснеЗберігання.LoadRecords(TreeViewGrid);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            await ValueTask.FromResult(true);
        }

        protected override void OpenTypeListDocs(Widget relative_to)
        {
            new ФункціїДляДинамічногоВідкриття().ВідкритиСписокДокументівДляЖурналу(relative_to, ТабличніСписки.Журнали_АдреснеЗберігання.AllowDocument());
        }

        protected override void ErrorSpendTheDocument(UnigueID unigueID)
        {
            ФункціїДляПовідомлень.ПоказатиПовідомлення(unigueID);
        }

        protected override void ReportSpendTheDocument(DocumentPointer documentPointer)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(documentPointer);
        }

        protected override void OpenDoc(string typeDoc, UnigueID unigueID)
        {
            new ФункціїДляДинамічногоВідкриття().ВідкритиДокументВідповідноДоВиду(typeDoc, unigueID);
        }

        const string КлючНалаштуванняКористувача = "Журнали.АдреснеЗберігання";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
            NotebookFunction.AddChangeFuncJournal(Program.GeneralNotebook, Name, LoadRecords, [.. ТабличніСписки.Журнали_АдреснеЗберігання.AllowDocument().Keys]);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }
    }
}