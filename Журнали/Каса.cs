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
using System.Reflection;

using StorageAndTrade_1_0;
using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;

namespace StorageAndTrade
{
    class Журнал_Каса : Журнал
    {
        public Журнал_Каса() : base()
        {
            ТабличніСписки.Журнали_Каса.AddColumns(TreeViewGrid);
        }

        protected override Assembly ExecutingAssembly { get; } = Assembly.GetExecutingAssembly();
        protected override string NameSpageCodeGeneration { get; } = Config.NameSpageCodeGeneration;

        public override async void LoadRecords()
        {
            ТабличніСписки.Журнали_Каса.SelectPointerItem = SelectPointerItem;

            await ТабличніСписки.Журнали_Каса.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Журнали_Каса.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Каса.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.Журнали_Каса.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Каса.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        public override void OpenTypeListDocs(Widget relative_to)
        {
            ФункціїДляЖурналів.ВідкритиСписокДокументів(relative_to, ТабличніСписки.Журнали_Каса.AllowDocument(),
                Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(ComboBoxPeriodWhere.ActiveId));
        }

        public override void PeriodWhereChanged()
        {
            ТабличніСписки.Журнали_Каса.ДодатиВідбірПоПеріоду(TreeViewGrid, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }
    }
}