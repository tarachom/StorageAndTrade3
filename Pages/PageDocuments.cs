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

/*

Документи

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class PageDocuments : Box
    {
        public PageDocuments() : base(Orientation.Vertical, 0)
        {
            //Всі Документи
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                PackStart(hBox, false, false, 10);

                Expander expander = new Expander("Всі документи");
                hBox.PackStart(expander, false, false, 5);

                Box vBox = new Box(Orientation.Vertical, 0);
                expander.Add(vBox);

                vBox.PackStart(new Label("Документи"), false, false, 2);

                ListBox listBox = new ListBox();
                listBox.ButtonPressEvent += (object? sender, ButtonPressEventArgs args) =>
                {
                    if (args.Event.Type == Gdk.EventType.DoubleButtonPress && listBox.SelectedRows.Length != 0)
                        ФункціїДляДокументів.ВідкритиДокументВідповідноДоВиду(listBox.SelectedRows[0].Name, new UnigueID());
                };

                ScrolledWindow scrollList = new ScrolledWindow() { WidthRequest = 300, HeightRequest = 300, ShadowType = ShadowType.In };
                scrollList.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                scrollList.Add(listBox);

                vBox.PackStart(scrollList, false, false, 2);

                foreach (KeyValuePair<string, ConfigurationDocuments> documents in Config.Kernel.Conf.Documents)
                {
                    string title = string.IsNullOrEmpty(documents.Value.FullName) ? documents.Value.Name : documents.Value.FullName;

                    ListBoxRow row = new ListBoxRow() { Name = documents.Key };
                    row.Add(new Label(title) { Halign = Align.Start });

                    listBox.Add(row);
                }
            }

            //Список
            Box hBoxList = new Box(Orientation.Horizontal, 0);

            Box vLeft = new Box(Orientation.Vertical, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            Link.AddCaption(vLeft, "Продажі", PageJournals.Продажі);
            {
                Link.AddLink(vLeft, $"{ЗамовленняКлієнта_Const.FULLNAME}", async () =>
                {
                    ЗамовленняКлієнта page = new ЗамовленняКлієнта();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ЗамовленняКлієнта_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vLeft, $"{РахунокФактура_Const.FULLNAME}", async () =>
                {
                    РахунокФактура page = new РахунокФактура();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{РахунокФактура_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vLeft, $"{РеалізаціяТоварівТаПослуг_Const.FULLNAME}", async () =>
                {
                    РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{РеалізаціяТоварівТаПослуг_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vLeft, $"{АктВиконанихРобіт_Const.FULLNAME}", async () =>
                {
                    АктВиконанихРобіт page = new АктВиконанихРобіт();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{АктВиконанихРобіт_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vLeft, $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME}", async () =>
                {
                    ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });
            }

            Link.AddCaption(vLeft, "Закупівлі", PageJournals.Закупівлі);
            {
                Link.AddLink(vLeft, $"{ЗамовленняПостачальнику_Const.FULLNAME}", async () =>
                {
                    ЗамовленняПостачальнику page = new ЗамовленняПостачальнику();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ЗамовленняПостачальнику_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vLeft, $"{ПоступленняТоварівТаПослуг_Const.FULLNAME}", async () =>
                {
                    ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПоступленняТоварівТаПослуг_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vLeft, $"{ПоверненняТоварівПостачальнику_Const.FULLNAME}", async () =>
                {
                    ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПоверненняТоварівПостачальнику_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });
            }

            Link.AddCaption(vLeft, "Ціноутворення");
            {
                Link.AddLink(vLeft, $"{ВстановленняЦінНоменклатури_Const.FULLNAME}", async () =>
                {
                    ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ВстановленняЦінНоменклатури_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });
            }

            Link.AddSeparator(hBoxList);

            Box vRight = new Box(Orientation.Vertical, 0);
            hBoxList.PackStart(vRight, false, false, 5);

            Link.AddCaption(vRight, "Каса", PageJournals.Каса);
            {
                Link.AddLink(vRight, $"{ПрихіднийКасовийОрдер_Const.FULLNAME}", async () =>
                {
                    ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПрихіднийКасовийОрдер_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vRight, $"{РозхіднийКасовийОрдер_Const.FULLNAME}", async () =>
                {
                    РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{РозхіднийКасовийОрдер_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vRight, $"{КорегуванняБоргу_Const.FULLNAME}", async () =>
                {
                    КорегуванняБоргу page = new КорегуванняБоргу();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{КорегуванняБоргу_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });
            }

            Link.AddCaption(vRight, "Склад", PageJournals.Склад);
            {
                Link.AddLink(vRight, $"{ПереміщенняТоварів_Const.FULLNAME}", async () =>
                {
                    ПереміщенняТоварів page = new ПереміщенняТоварів();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПереміщенняТоварів_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vRight, $"{ВведенняЗалишків_Const.FULLNAME}", async () =>
                {
                    ВведенняЗалишків page = new ВведенняЗалишків();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ВведенняЗалишків_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vRight, $"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME}", async () =>
                {
                    ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vRight, $"{ПсуванняТоварів_Const.FULLNAME}", async () =>
                {
                    ПсуванняТоварів page = new ПсуванняТоварів();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПсуванняТоварів_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vRight, $"{ПерерахунокТоварів_Const.FULLNAME}", async () =>
                {
                    ПерерахунокТоварів page = new ПерерахунокТоварів();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПерерахунокТоварів_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });
            }

            Link.AddCaption(vRight, "Адресне зберігання", PageJournals.АдреснеЗберігання);
            {
                Link.AddLink(vRight, $"{РозміщенняТоварівНаСкладі_Const.FULLNAME}", async () =>
                {
                    РозміщенняТоварівНаСкладі page = new РозміщенняТоварівНаСкладі();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{РозміщенняТоварівНаСкладі_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vRight, $"{ЗбіркаТоварівНаСкладі_Const.FULLNAME}", async () =>
                {
                    ЗбіркаТоварівНаСкладі page = new ЗбіркаТоварівНаСкладі();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ЗбіркаТоварівНаСкладі_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });

                Link.AddLink(vRight, $"{ПереміщенняТоварівНаСкладі_Const.FULLNAME}", async () =>
                {
                    ПереміщенняТоварівНаСкладі page = new ПереміщенняТоварівНаСкладі();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПереміщенняТоварівНаСкладі_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });
            }

            /*
            Link.AddCaption(vRight, "Налаштування адресного зберігання");
            {
                Link.AddLink(vRight, $"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}", () =>
                {
                    РозміщенняНоменклатуриПоКоміркам page = new РозміщенняНоменклатуриПоКоміркам();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}", () => { return page; });
                    await page.SetValue();
                });
            }
            */

            PackStart(hBoxList, false, false, 10);
            ShowAll();
        }
    }
}