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
    class Журнал_АдреснеЗберігання : Журнал
    {
        public Журнал_АдреснеЗберігання() : base()
        {
            ТабличніСписки.Журнали_АдреснеЗберігання.AddColumns(TreeViewGrid);
        }

        protected override Assembly ExecutingAssembly { get; } = Assembly.GetExecutingAssembly();
        protected override string NameSpageCodeGeneration { get; } = Config.NameSpageCodeGeneration;

        protected override async void LoadRecords()
        {
            ТабличніСписки.Журнали_АдреснеЗберігання.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.Журнали_АдреснеЗберігання.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period);
            await ТабличніСписки.Журнали_АдреснеЗберігання.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.Журнали_АдреснеЗберігання.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_АдреснеЗберігання.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.Журнали_АдреснеЗберігання.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.Журнали_АдреснеЗберігання.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenTypeListDocs(Widget relative_to)
        {
            ФункціїДляЖурналів.ВідкритиСписокДокументів(relative_to, ТабличніСписки.Журнали_АдреснеЗберігання.AllowDocument());
        }

        const string КлючНалаштуванняКористувача = "Журнали.АдреснеЗберігання";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            await ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            LoadRecords();
        }
    }
}