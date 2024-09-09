/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
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