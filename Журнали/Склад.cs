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
    class Журнал_Склад : Журнал
    {
        public Журнал_Склад() : base()
        {
            ТабличніСписки.Журнали_Склад.AddColumns(TreeViewGrid);
        }

        protected override Assembly ExecutingAssembly { get; } = Assembly.GetExecutingAssembly();
        protected override string NameSpageCodeGeneration { get; } = Config.NameSpageCodeGeneration;

        public override async void LoadRecords()
        {
            ТабличніСписки.Журнали_Склад.SelectPointerItem = SelectPointerItem;

            await ТабличніСписки.Журнали_Склад.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Журнали_Склад.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Склад.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.Журнали_Склад.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_Склад.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        public override void OpenTypeListDocs(Widget relative_to)
        {
            ФункціїДляЖурналів.ВідкритиСписокДокументів(relative_to, ТабличніСписки.Журнали_Склад.AllowDocument(), Період.Period.ToString());
        }

        const string КлючНалаштуванняКористувача = "Журнали.Склад";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        public override async void PeriodChanged()
        {
            ТабличніСписки.Журнали_Склад.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period);
            LoadRecords();

            await ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
        }
    }
}