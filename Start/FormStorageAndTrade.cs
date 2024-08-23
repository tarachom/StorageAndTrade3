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

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class FormStorageAndTrade : FormGeneral
    {
        public FormStorageAndTrade()
        {

        }

        protected override void ButtonMessageClicked()
        {
            ФункціїДляПовідомлень.ВідкритиПовідомлення();
        }

        protected override void ButtonFindClicked(string text)
        {
            PageFullTextSearch page = new PageFullTextSearch();
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Пошук", () => { return page; });
            page.Find(text);
        }

        public void OpenFirstPages()
        {
            PageHome page = new PageHome();
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Стартова", () => { return page; });

            //Активні користувачі
            page.АктивніКористувачі.AutoRefreshRun();

            //Останні завантажені курси валют
            Task.Run(page.БлокКурсиВалют.StartDesktop);

            //Автоматично завантажити нові курси валют
            Task.Run(page.БлокКурсиВалют.StartAutoWork);

            //Початкове заповнення
            if (!ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const)
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Початкове заповнення", () => { return new Обробка_ПочатковеЗаповнення(); });
        }

        public async void SetCurrentUser()
        {
            Користувачі_Pointer ЗнайденийКористувач = await new Користувачі_Select().FindByField(Користувачі_Const.КодВСпеціальнійТаблиці, Config.Kernel.User);

            if (ЗнайденийКористувач.IsEmpty())
            {
                Користувачі_Objest НовийКористувач = new Користувачі_Objest
                {
                    КодВСпеціальнійТаблиці = Config.Kernel.User,
                    Назва = await Config.Kernel.DataBase.SpetialTableUsersGetFullName(Config.Kernel.User)
                };

                await НовийКористувач.New();
                await НовийКористувач.Save();

                Program.Користувач = НовийКористувач.GetDirectoryPointer();
            }
            else
                Program.Користувач = ЗнайденийКористувач;
        }

        protected override void Документи(LinkButton lb)
        {
            Popover po = new Popover(lb) { Position = PositionType.Right };
            po.Add(new PageDocuments());
            po.ShowAll();
        }

        protected override void Журнали(LinkButton lb)
        {
            Popover po = new Popover(lb) { Position = PositionType.Right };
            po.Add(new PageJournals());
            po.ShowAll();
        }

        protected override void Звіти(LinkButton lb)
        {
            Popover po = new Popover(lb) { Position = PositionType.Right };
            po.Add(new PageReports());
            po.ShowAll();
        }

        protected override void Довідники(LinkButton lb)
        {
            Popover po = new Popover(lb) { Position = PositionType.Right };
            po.Add(new PageDirectory());
            po.ShowAll();
        }

        protected override void Регістри(LinkButton lb)
        {
            
        }

        protected override void Налаштування(LinkButton lb)
        {
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Налаштування", () =>
            {
                PageSettings page = new PageSettings();
                page.SetValue();
                return page;
            });
        }

        protected override void Сервіс(LinkButton lb)
        {
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Сервіс", () =>
            {
                PageService page = new PageService();
                return page;
            });
        }
    }
}