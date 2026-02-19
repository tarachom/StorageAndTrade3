
/*
        Повний.cs
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode;
using ТабличніСписки = GeneratedCode.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    class Журнал_Повний : Журнал
    {
        public Журнал_Повний() : base(Config.NamespaceCodeGeneration)
        {
            ТабличніСписки.Журнали_Повний.AddColumns(TreeViewGrid);
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.Журнали_Повний.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);
            await ТабличніСписки.Журнали_Повний.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public async ValueTask UpdateRecords(List<ObjectChanged> recordsChanged)
        {
            await ValueTask.FromResult(true);
        }

        protected override void OpenTypeListDocs(Widget relative_to)
        {
            new ФункціїДляДинамічногоВідкриття().ВідкритиСписокДокументівДляЖурналу(relative_to, ТабличніСписки.Журнали_Повний.AllowDocument());
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

        const string КлючНалаштуванняКористувача = "Журнали.Повний";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
            NotebookFunction.AddChangeFuncJournal(Program.GeneralNotebook, Name, UpdateRecords, [.. ТабличніСписки.Журнали_Повний.AllowDocument().Keys]);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }
    }
}