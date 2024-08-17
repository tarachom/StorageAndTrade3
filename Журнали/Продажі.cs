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
    class Журнал_Продажі : Журнал
    {
        public Журнал_Продажі() : base()
        {
            ТабличніСписки.Журнали_Продажі.AddColumns(TreeViewGrid);
        }

        protected override Assembly ExecutingAssembly { get; } = Assembly.GetExecutingAssembly();
        protected override string NameSpageCodeGeneration { get; } = Config.NameSpageCodeGeneration;

        public override async void LoadRecords()
        {
            ТабличніСписки.Журнали_Продажі.SelectPointerItem = SelectPointerItem;

            await ТабличніСписки.Журнали_Продажі.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Журнали_Продажі.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Продажі.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.Журнали_Продажі.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Продажі.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        public override void OpenTypeListDocs(Widget relative_to)
        {
            ФункціїДляЖурналів.ВідкритиСписокДокументів(relative_to, ТабличніСписки.Журнали_Продажі.AllowDocument(), ComboBoxPeriodWhere.ActiveId);
        }

        const string КлючНалаштуванняКористувача = "Журнали.Продажі";

        protected override async ValueTask BeforeSetValue()
        {
            string періодДляЖурналу = await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача);
            if (!string.IsNullOrEmpty(періодДляЖурналу) && Enum.TryParse<ПеріодДляЖурналу.ТипПеріоду>(періодДляЖурналу, out ПеріодДляЖурналу.ТипПеріоду result))
                PeriodWhere = result;
        }

        public override async void PeriodWhereChanged()
        {
            ТабличніСписки.Журнали_Продажі.ДодатиВідбірПоПеріоду(TreeViewGrid, ComboBoxPeriodWhere.ActiveId);
            LoadRecords();

            await ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, ComboBoxPeriodWhere.ActiveId);
        }
    }
}