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

/*

Журнали

*/

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class PageJournals : VBox
    {
        public PageJournals() : base()
        {
            //Всі Журнали
            {
                HBox hBoxAll = new HBox(false, 0);
                PackStart(hBoxAll, false, false, 10);

                Expander expanderAll = new Expander("Всі журнали");
                hBoxAll.PackStart(expanderAll, false, false, 5);

                VBox vBoxAll = new VBox(false, 0);
                expanderAll.Add(vBoxAll);

                vBoxAll.PackStart(new Label("Журнали"), false, false, 2);

                ListBox listBox = new ListBox();
                listBox.ButtonPressEvent += (object? sender, ButtonPressEventArgs args) =>
                {
                    if (args.Event.Type == Gdk.EventType.DoubleButtonPress && listBox.SelectedRows.Length != 0)
                        ФункціїДляЖурналів.ВідкритиЖурналВідповідноДоВиду(listBox.SelectedRows[0].Name, null,0, false);
                };

                ScrolledWindow scrollList = new ScrolledWindow() { WidthRequest = 300, HeightRequest = 300, ShadowType = ShadowType.In };
                scrollList.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                scrollList.Add(listBox);

                vBoxAll.PackStart(scrollList, false, false, 2);

                foreach (KeyValuePair<string, ConfigurationJournals> journal in Config.Kernel!.Conf.Journals)
                {
                    string title = journal.Value.Name;

                    ListBoxRow row = new ListBoxRow() { Name = journal.Key };
                    row.Add(new Label(title) { Halign = Align.Start });

                    listBox.Add(row);
                }
            }

            //Список
            HBox hBoxList = new HBox(false, 0);

            VBox vLeft = new VBox(false, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            AddLink(vLeft, "Повний (всі документи)", Повний);
            AddLink(vLeft, "Продажі", Продажі);
            AddLink(vLeft, "Закупівлі", Закупівлі);
            AddLink(vLeft, "Каса", Каса);
            AddLink(vLeft, "Склад", Склад);
            AddLink(vLeft, "Адресне зберігання на складах", АдреснеЗберігання);

            PackStart(hBoxList, false, false, 10);

            ShowAll();
        }

        void Повний(object? sender, EventArgs args)
        {
            Журнал_Повний page = new Журнал_Повний();
            Program.GeneralForm?.CreateNotebookPage("Повний", () => { return page; });
            page.SetValue();
        }

        public static void Продажі(object? sender, EventArgs args)
        {
            Журнал_Продажі page = new Журнал_Продажі();
            Program.GeneralForm?.CreateNotebookPage("Продажі", () => { return page; });
            page.SetValue();
        }

        public static void Закупівлі(object? sender, EventArgs args)
        {
            Журнал_Закупівлі page = new Журнал_Закупівлі();
            Program.GeneralForm?.CreateNotebookPage("Закупівлі", () => { return page; });
            page.SetValue();
        }

        public static void Склад(object? sender, EventArgs args)
        {
            Журнал_Склад page = new Журнал_Склад();
            Program.GeneralForm?.CreateNotebookPage("Склад", () => { return page; });
            page.SetValue();
        }

        public static void Каса(object? sender, EventArgs args)
        {
            Журнал_Каса page = new Журнал_Каса();
            Program.GeneralForm?.CreateNotebookPage("Каса", () => { return page; });
            page.SetValue();
        }

        public static void АдреснеЗберігання(object? sender, EventArgs args)
        {
            Журнал_АдреснеЗберігання page = new Журнал_АдреснеЗберігання();
            Program.GeneralForm?.CreateNotebookPage("Адресне зберігання", () => { return page; });
            page.SetValue();
        }

        void AddLink(VBox vbox, string uri, EventHandler? clickAction = null)
        {
            LinkButton lb = new LinkButton(uri, " " + uri) { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
            vbox.PackStart(lb, false, false, 0);

            if (clickAction != null)
                lb.Clicked += clickAction;
        }
    }
}