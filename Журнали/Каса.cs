
/*
        Каса.cs
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0;
using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    class Журнал_Каса : Журнал
    {
        public Журнал_Каса() : base(Config.NameSpageCodeGeneration)
        {
            ТабличніСписки.Журнали_Каса.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Журнали_Каса.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Журнали_Каса.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period);
            await ТабличніСписки.Журнали_Каса.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            await ValueTask.FromResult(true);
        }

        protected override void OpenTypeListDocs(Widget relative_to)
        {
            new ФункціїДляДинамічногоВідкриття().ВідкритиСписокДокументівДляЖурналу(relative_to, ТабличніСписки.Журнали_Каса.AllowDocument());
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

        const string КлючНалаштуванняКористувача = "Журнали.Каса";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }
    }
}