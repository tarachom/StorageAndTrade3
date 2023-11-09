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

Документи

*/

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class PageDocuments : VBox
    {
        public PageDocuments() : base()
        {
            //Всі Документи
            {
                HBox hBoxAll = new HBox(false, 0);
                PackStart(hBoxAll, false, false, 10);

                Expander expanderAll = new Expander("Всі документи");
                hBoxAll.PackStart(expanderAll, false, false, 5);

                VBox vBoxAll = new VBox(false, 0);
                expanderAll.Add(vBoxAll);

                vBoxAll.PackStart(new Label("Документи"), false, false, 2);

                ListBox listBox = new ListBox();
                listBox.ButtonPressEvent += (object? sender, ButtonPressEventArgs args) =>
                {
                    if (args.Event.Type == Gdk.EventType.DoubleButtonPress && listBox.SelectedRows.Length != 0)
                        ФункціїДляДокументів.ВідкритиДокументВідповідноДоВиду(listBox.SelectedRows[0].Name, new UnigueID(), 0, false);
                };

                ScrolledWindow scrollList = new ScrolledWindow() { WidthRequest = 300, HeightRequest = 300, ShadowType = ShadowType.In };
                scrollList.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                scrollList.Add(listBox);

                vBoxAll.PackStart(scrollList, false, false, 2);

                foreach (KeyValuePair<string, ConfigurationDocuments> documents in Config.Kernel!.Conf.Documents)
                {
                    string title = string.IsNullOrEmpty(documents.Value.FullName) ? documents.Value.Name : documents.Value.FullName;

                    ListBoxRow row = new ListBoxRow() { Name = documents.Key };
                    row.Add(new Label(title) { Halign = Align.Start });

                    listBox.Add(row);
                }
            }

            //Список
            HBox hBoxList = new HBox(false, 0);

            VBox vLeft = new VBox(false, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            Link.AddCaption(vLeft, "Продажі", PageJournals.Продажі);
            {
                Link.AddLink(vLeft, $"{ЗамовленняКлієнта_Const.FULLNAME}", () =>
                {
                    ЗамовленняКлієнта page = new ЗамовленняКлієнта();
                    Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняКлієнта_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vLeft, $"{РахунокФактура_Const.FULLNAME}", () =>
                {
                    РахунокФактура page = new РахунокФактура();
                    Program.GeneralForm?.CreateNotebookPage($"{РахунокФактура_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vLeft, $"{РеалізаціяТоварівТаПослуг_Const.FULLNAME}", () =>
                {
                    РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг();
                    Program.GeneralForm?.CreateNotebookPage($"{РеалізаціяТоварівТаПослуг_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vLeft, $"{АктВиконанихРобіт_Const.FULLNAME}", () =>
                {
                    АктВиконанихРобіт page = new АктВиконанихРобіт();
                    Program.GeneralForm?.CreateNotebookPage($"{АктВиконанихРобіт_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vLeft, $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME}", () =>
                {
                    ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта();
                    Program.GeneralForm?.CreateNotebookPage($"{ПоверненняТоварівВідКлієнта_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });
            }

            Link.AddCaption(vLeft, "Закупівлі", PageJournals.Закупівлі);
            {
                Link.AddLink(vLeft, $"{ЗамовленняПостачальнику_Const.FULLNAME}", () =>
                {
                    ЗамовленняПостачальнику page = new ЗамовленняПостачальнику();
                    Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняПостачальнику_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vLeft, $"{ПоступленняТоварівТаПослуг_Const.FULLNAME}", () =>
                {
                    ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг();
                    Program.GeneralForm?.CreateNotebookPage($"{ПоступленняТоварівТаПослуг_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vLeft, $"{ПоверненняТоварівПостачальнику_Const.FULLNAME}", () =>
                {
                    ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику();
                    Program.GeneralForm?.CreateNotebookPage($"{ПоверненняТоварівПостачальнику_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });
            }

            Link.AddCaption(vLeft, "Ціноутворення");
            {
                Link.AddLink(vLeft, $"{ВстановленняЦінНоменклатури_Const.FULLNAME}", () =>
                {
                    ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури();
                    Program.GeneralForm?.CreateNotebookPage($"{ВстановленняЦінНоменклатури_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });
            }

            Link.AddSeparator(hBoxList);

            VBox vRight = new VBox(false, 0);
            hBoxList.PackStart(vRight, false, false, 5);

            Link.AddCaption(vRight, "Каса", PageJournals.Каса);
            {
                Link.AddLink(vRight, $"{ПрихіднийКасовийОрдер_Const.FULLNAME}", () =>
                {
                    ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер();
                    Program.GeneralForm?.CreateNotebookPage($"{ПрихіднийКасовийОрдер_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vRight, $"{РозхіднийКасовийОрдер_Const.FULLNAME}", () =>
                {
                    РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер();
                    Program.GeneralForm?.CreateNotebookPage($"{РозхіднийКасовийОрдер_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });
            }

            Link.AddCaption(vRight, "Склад", PageJournals.Склад);
            {
                Link.AddLink(vRight, $"{ПереміщенняТоварів_Const.FULLNAME}", () =>
                {
                    ПереміщенняТоварів page = new ПереміщенняТоварів();
                    Program.GeneralForm?.CreateNotebookPage($"{ПереміщенняТоварів_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vRight, $"{ВведенняЗалишків_Const.FULLNAME}", () =>
                {
                    ВведенняЗалишків page = new ВведенняЗалишків();
                    Program.GeneralForm?.CreateNotebookPage($"{ВведенняЗалишків_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vRight, $"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME}", () =>
                {
                    ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів();
                    Program.GeneralForm?.CreateNotebookPage($"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vRight, $"{ПсуванняТоварів_Const.FULLNAME}", () =>
                {
                    ПсуванняТоварів page = new ПсуванняТоварів();
                    Program.GeneralForm?.CreateNotebookPage($"{ПсуванняТоварів_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });
            }

            Link.AddCaption(vRight, "Адресне зберігання", PageJournals.АдреснеЗберігання);
            {
                Link.AddLink(vRight, $"{РозміщенняТоварівНаСкладі_Const.FULLNAME}", () =>
                {
                    РозміщенняТоварівНаСкладі page = new РозміщенняТоварівНаСкладі();
                    Program.GeneralForm?.CreateNotebookPage($"{РозміщенняТоварівНаСкладі_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vRight, $"{ЗбіркаТоварівНаСкладі_Const.FULLNAME}", () =>
                {
                    ЗбіркаТоварівНаСкладі page = new ЗбіркаТоварівНаСкладі();
                    Program.GeneralForm?.CreateNotebookPage($"{ЗбіркаТоварівНаСкладі_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });

                Link.AddLink(vRight, $"{ПереміщенняТоварівНаСкладі_Const.FULLNAME}", () =>
                {
                    ПереміщенняТоварівНаСкладі page = new ПереміщенняТоварівНаСкладі();
                    Program.GeneralForm?.CreateNotebookPage($"{ПереміщенняТоварівНаСкладі_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });
            }

            Link.AddCaption(vRight, "Налаштування адресного зберігання");
            {
                Link.AddLink(vRight, $"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}", () =>
                {
                    РозміщенняНоменклатуриПоКоміркам page = new РозміщенняНоменклатуриПоКоміркам();
                    Program.GeneralForm?.CreateNotebookPage($"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}", () => { return page; });
                    page.SetValue();
                });
            }

            PackStart(hBoxList, false, false, 10);
            ShowAll();
        }
    }
}