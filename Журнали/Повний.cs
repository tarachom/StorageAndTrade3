
/*
        Повний.cs
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0;
using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    class Журнал_Повний : Журнал
    {
        public Журнал_Повний() : base(Config.NameSpageCodeGeneration)
        {
            ТабличніСписки.Журнали_Повний.AddColumns(TreeViewGrid);
            Config.Kernel.DocumentObjectChanged += async (object? sender, Dictionary<string, List<Guid>> document) =>
            {
                var allowDocument = ТабличніСписки.Журнали_Повний.AllowDocument();

                if (document.Any((x) => allowDocument.ContainsKey(x.Key)))
                    await LoadRecords();
            };
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.Журнали_Повний.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Журнали_Повний.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);
            await ТабличніСписки.Журнали_Повний.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
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
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }
    }
}

/*
// ТабличніСписки.Журнали_Повний.Limit = 50;
// ТабличніСписки.Журнали_Повний.Offset = 0;

// ScrollTree.Vadjustment.ValueChanged += (object? sender, EventArgs args) =>
// {
//     Console.WriteLine(
//     ScrollTree.Vadjustment.Value + " - " +
//     ScrollTree.Vadjustment.Upper + " - " +
//     (ScrollTree.Vadjustment.Upper - ScrollTree.Vadjustment.PageSize) + " - " +
//     ScrollTree.Vadjustment.PageSize);

//     if (ScrollTree.Vadjustment.Upper - ScrollTree.Vadjustment.PageSize == ScrollTree.Vadjustment.Value)
//     {
//         ТабличніСписки.Журнали_Повний.Offset += 50;
//         ТабличніСписки.Журнали_Повний.LoadRecords();
//     }
// };




scrollTree.Vadjustment.ValueChanged += (object? sender, EventArgs args) =>
{
    Console.WriteLine(
        scrollTree.Vadjustment.Value + " " + 
        scrollTree.Vadjustment.Upper + " " + 
        scrollTree.Vadjustment.PageIncrement + " " + 
        scrollTree.Vadjustment.PageSize);
};

*/