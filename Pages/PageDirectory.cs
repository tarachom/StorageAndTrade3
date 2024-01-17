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

Довідники

*/


using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class PageDirectory : VBox
    {
        public PageDirectory() : base()
        {
            //Всі Довідники
            {
                HBox hBox = new HBox(false, 0);
                PackStart(hBox, false, false, 10);

                Expander expander = new Expander("Всі довідники");
                hBox.PackStart(expander, false, false, 5);

                VBox vBox = new VBox(false, 0);
                expander.Add(vBox);

                vBox.PackStart(new Label("Довідники"), false, false, 2);

                ListBox listBox = new ListBox();
                listBox.ButtonPressEvent += (object? sender, ButtonPressEventArgs args) =>
                {
                    if (args.Event.Type == Gdk.EventType.DoubleButtonPress && listBox.SelectedRows.Length != 0)
                        ФункціїДляДовідників.ВідкритиДовідникВідповідноДоВиду(listBox.SelectedRows[0].Name, null);
                };

                ScrolledWindow scrollList = new ScrolledWindow() { WidthRequest = 300, HeightRequest = 300, ShadowType = ShadowType.In };
                scrollList.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                scrollList.Add(listBox);

                vBox.PackStart(scrollList, false, false, 2);

                foreach (KeyValuePair<string, ConfigurationDirectories> directories in Config.Kernel.Conf.Directories)
                {
                    string title = string.IsNullOrEmpty(directories.Value.FullName) ? directories.Value.Name : directories.Value.FullName;

                    ListBoxRow row = new ListBoxRow() { Name = directories.Key };
                    row.Add(new Label(title) { Halign = Align.Start });

                    listBox.Add(row);
                }
            }

            //Список
            HBox hBoxList = new HBox(false, 0);
            PackStart(hBoxList, false, false, 10);

            VBox vLeft = new VBox(false, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            //Link.AddCaption(vLeft, "Основні");
            {
                Link.AddLink(vLeft, $"{Контрагенти_Const.FULLNAME}", async () =>
                {
                    Контрагенти page = new Контрагенти();
                    Program.GeneralForm?.CreateNotebookPage($"{Контрагенти_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{Номенклатура_Const.FULLNAME}", async () =>
                {
                    Номенклатура page = new Номенклатура();
                    Program.GeneralForm?.CreateNotebookPage($"{Номенклатура_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{Організації_Const.FULLNAME}", async () =>
                {
                    Організації page = new Організації();
                    Program.GeneralForm?.CreateNotebookPage($"{Організації_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{Склади_Const.FULLNAME}", async () =>
                {
                    Склади page = new Склади();
                    Program.GeneralForm?.CreateNotebookPage($"{Склади_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{Валюти_Const.FULLNAME}", async () =>
                {
                    Валюти page = new Валюти();
                    Program.GeneralForm?.CreateNotebookPage($"{Валюти_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{Каси_Const.FULLNAME}", async () =>
                {
                    Каси page = new Каси();
                    Program.GeneralForm?.CreateNotebookPage($"{Каси_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{Блокнот_Const.FULLNAME}", async () =>
                {
                    Блокнот page = new Блокнот();
                    Program.GeneralForm?.CreateNotebookPage($"{Блокнот_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                /*
                Link.AddLink(vLeft, $"{ВидиЦін_Const.FULLNAME}", async () =>
                {
                    ВидиЦін page = new ВидиЦін();
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиЦін_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{ВидиЦінПостачальників_Const.FULLNAME}", async () =>
                {
                    ВидиЦінПостачальників page = new ВидиЦінПостачальників();
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиЦінПостачальників_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });
                
                Link.AddLink(vLeft, $"{БанківськіРахункиОрганізацій_Const.FULLNAME}", async () =>
                {
                    БанківськіРахункиОрганізацій page = new БанківськіРахункиОрганізацій();
                    Program.GeneralForm?.CreateNotebookPage($"{БанківськіРахункиОрганізацій_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{СтруктураПідприємства_Const.FULLNAME}", async () =>
                {
                    СтруктураПідприємства page = new СтруктураПідприємства();
                    Program.GeneralForm?.CreateNotebookPage($"{СтруктураПідприємства_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });
                */
            }

            //Link.AddSeparator(hBoxList);

            //VBox vRight = new VBox(false, 0);
            //hBoxList.PackStart(vRight, false, false, 5);

            /*Link.AddCaption(vRight, "Партнери");
            {
                Link.AddLink(vRight, $"{ДоговориКонтрагентів_Const.FULLNAME}", async () =>
                {
                    ДоговориКонтрагентів page = new ДоговориКонтрагентів();
                    Program.GeneralForm?.CreateNotebookPage($"{ДоговориКонтрагентів_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });
                
                Link.AddLink(vRight, $"{БанківськіРахункиКонтрагентів_Const.FULLNAME}", async () =>
                {
                    БанківськіРахункиКонтрагентів page = new БанківськіРахункиКонтрагентів();
                    Program.GeneralForm?.CreateNotebookPage($"{БанківськіРахункиКонтрагентів_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });
            }*/

            /*Link.AddCaption(vRight, "Товари та послуги");
            {
                Link.AddLink(vRight, $"{ХарактеристикиНоменклатури_Const.FULLNAME}", async () =>
                {
                    ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();
                    Program.GeneralForm?.CreateNotebookPage($"{ХарактеристикиНоменклатури_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vRight, $"{ПакуванняОдиниціВиміру_Const.FULLNAME}", async () =>
                {
                    ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру();
                    Program.GeneralForm?.CreateNotebookPage($"{ПакуванняОдиниціВиміру_Const.FULLNAME}", () => {return page; });
                    await page.LoadRecords();
                });
                

                Link.AddLink(vRight, $"{ВидиНоменклатури_Const.FULLNAME}", async () =>
                {
                    ВидиНоменклатури page = new ВидиНоменклатури();
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиНоменклатури_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });
                
                Link.AddLink(vRight, $"{ПартіяТоварівКомпозит_Const.FULLNAME}", async () =>
                {
                    ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит();
                    Program.GeneralForm?.CreateNotebookPage($"{ПартіяТоварівКомпозит_Const.FULLNAME}", () => {return page;});
                    await page.LoadRecords();
                });
                
                Link.AddLink(vRight, $"{СеріїНоменклатури_Const.FULLNAME}", async () =>
                {
                    СеріїНоменклатури page = new СеріїНоменклатури();
                    Program.GeneralForm?.CreateNotebookPage($"{СеріїНоменклатури_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vRight, $"{Виробники_Const.FULLNAME}", async () =>
                {
                    Виробники page = new Виробники();
                    Program.GeneralForm?.CreateNotebookPage($"{Виробники_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });
            }
            */

            //Link.AddCaption(vRight, "Додаткові");
            //{
                /*
                Link.AddLink(vRight, $"{Банки_Const.FULLNAME}", async () =>
                {
                    Банки page = new Банки();
                    Program.GeneralForm?.CreateNotebookPage($"{Банки_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vRight, $"{Користувачі_Const.FULLNAME}", async () =>
                {
                    Користувачі page = new Користувачі();
                    Program.GeneralForm?.CreateNotebookPage($"{Користувачі_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{ФізичніОсоби_Const.FULLNAME}", async () =>
                {
                    ФізичніОсоби page = new ФізичніОсоби();
                    Program.GeneralForm?.CreateNotebookPage($"{ФізичніОсоби_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });
                
                Link.AddLink(vLeft, $"{Файли_Const.FULLNAME}", async () =>
                {
                    Файли page = new Файли();
                    Program.GeneralForm?.CreateNotebookPage($"{Файли_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{СтаттяРухуКоштів_Const.FULLNAME}", async () =>
                {
                    СтаттяРухуКоштів page = new СтаттяРухуКоштів();
                    Program.GeneralForm?.CreateNotebookPage($"{СтаттяРухуКоштів_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vLeft, $"{ВидиЗапасів_Const.FULLNAME}", async () =>
                {
                    ВидиЗапасів page = new ВидиЗапасів();
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиЗапасів_Const.FULLNAME}", () => { return page;s });
                    await page.LoadRecords();
                });
                
                Link.AddLink(vLeft, $"{КраїниСвіту_Const.FULLNAME}", async () =>
                {
                    КраїниСвіту page = new КраїниСвіту();
                    Program.GeneralForm?.CreateNotebookPage($"{КраїниСвіту_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });
                */
            //}

            //Link.AddCaption(vRight, "Адресне зберігання");
            //{
                /*
                Link.AddLink(vRight, $"{СкладськіПриміщення_Const.FULLNAME}", async () =>
                {
                    СкладськіПриміщення page = new СкладськіПриміщення();
                    Program.GeneralForm?.CreateNotebookPage($"{СкладськіПриміщення_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

                Link.AddLink(vRight, $"{СкладськіКомірки_Const.FULLNAME}", async () =>
                {
                    СкладськіКомірки page = new СкладськіКомірки();
                    Program.GeneralForm?.CreateNotebookPage($"{СкладськіКомірки_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });

               
                Link.AddLink(vRight, $"{ТипорозміриКомірок_Const.FULLNAME}", async () =>
                {
                    ТипорозміриКомірок page = new ТипорозміриКомірок();
                    Program.GeneralForm?.CreateNotebookPage($"{ТипорозміриКомірок_Const.FULLNAME}", () => { return page; });
                    await page.LoadRecords();
                });
                */
            //}

            ShowAll();
        }


    }
}