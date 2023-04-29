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
    class Журнал_Закупівлі : Журнал
    {
        public Журнал_Закупівлі() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.Журнали_Закупівлі.Store;
            ТабличніСписки.Журнали_Закупівлі.AddColumns(TreeViewGrid);
        }

        public override void LoadRecords()
        {
            ТабличніСписки.Журнали_Закупівлі.SelectPointerItem = SelectPointerItem;

            ТабличніСписки.Журнали_Закупівлі.LoadRecords();

            if (ТабличніСписки.Журнали_Закупівлі.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Закупівлі.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.Журнали_Закупівлі.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Закупівлі.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        public override void OpenTypeListDocs(Widget relative_to)
        {
            ФункціїДляЖурналів.ВідкритиСписокДокументів(relative_to, ТабличніСписки.Журнали_Закупівлі.AllowDocument(),
                Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
        }

        public override void PeriodWhereChanged()
        {
            ТабличніСписки.Журнали_Закупівлі.ДодатиВідбірПоПеріоду(Enum.Parse<Перелічення.ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }
    }
}