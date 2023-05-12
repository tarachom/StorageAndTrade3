/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
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

using Перелічення = StorageAndTrade_1_0.Перелічення;
using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    class Журнал_Повний : Журнал
    {
        public Журнал_Повний() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.Журнали_Повний.Store;
            ТабличніСписки.Журнали_Повний.AddColumns(TreeViewGrid);

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

        }

        public override void LoadRecords()
        {
            ТабличніСписки.Журнали_Повний.SelectPointerItem = SelectPointerItem;

            // ТабличніСписки.Журнали_Повний.Store.Clear();
            // ТабличніСписки.Журнали_Повний.Limit = 50;
            // ТабличніСписки.Журнали_Повний.Offset = 0;
            
            ТабличніСписки.Журнали_Повний.LoadRecords();

            if (ТабличніСписки.Журнали_Повний.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Повний.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.Журнали_Повний.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Повний.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        public override void OpenTypeListDocs(Widget relative_to)
        {
            ФункціїДляЖурналів.ВідкритиСписокДокументів(relative_to, ТабличніСписки.Журнали_Повний.AllowDocument(),
                Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
        }

        public override void PeriodWhereChanged()
        {
            ТабличніСписки.Журнали_Повний.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }
    }
}

/*

scrollTree.Vadjustment.ValueChanged += (object? sender, EventArgs args) =>
{
    Console.WriteLine(
        scrollTree.Vadjustment.Value + " " + 
        scrollTree.Vadjustment.Upper + " " + 
        scrollTree.Vadjustment.PageIncrement + " " + 
        scrollTree.Vadjustment.PageSize);
};

*/