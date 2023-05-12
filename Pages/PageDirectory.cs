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
                HBox hBoxAll = new HBox(false, 0);
                PackStart(hBoxAll, false, false, 10);

                Expander expanderAll = new Expander("Всі довідники");
                hBoxAll.PackStart(expanderAll, false, false, 5);

                VBox vBoxAll = new VBox(false, 0);
                expanderAll.Add(vBoxAll);

                vBoxAll.PackStart(new Label("Довідники"), false, false, 2);

                ListBox listBox = new ListBox();
                listBox.ButtonPressEvent += (object? sender, ButtonPressEventArgs args) =>
                {
                    if (args.Event.Type == Gdk.EventType.DoubleButtonPress && listBox.SelectedRows.Length != 0)
                        ФункціїДляДовідників.ВідкритиДовідникВідповідноДоВиду(listBox.SelectedRows[0].Name, null, false);
                };

                ScrolledWindow scrollList = new ScrolledWindow() { WidthRequest = 300, HeightRequest = 300, ShadowType = ShadowType.In };
                scrollList.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                scrollList.Add(listBox);

                vBoxAll.PackStart(scrollList, false, false, 2);

                foreach (KeyValuePair<string, ConfigurationDirectories> directories in Config.Kernel!.Conf.Directories)
                {
                    string title = String.IsNullOrEmpty(directories.Value.FullName) ? directories.Value.Name : directories.Value.FullName;

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

            Link.AddCaption(vLeft, "Підприємство");
            {
                Link.AddLink(vLeft, $"{Організації_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Організації_Const.FULLNAME}", () =>
                    {
                        Організації page = new Організації();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{Склади_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Склади_Const.FULLNAME}", () =>
                    {
                        Склади page = new Склади();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{Валюти_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Валюти_Const.FULLNAME}", () =>
                    {
                        Валюти page = new Валюти();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{Каси_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Каси_Const.FULLNAME}", () =>
                    {
                        Каси page = new Каси();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{ВидиЦін_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиЦін_Const.FULLNAME}", () =>
                    {
                        ВидиЦін page = new ВидиЦін();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{ВидиЦінПостачальників_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиЦінПостачальників_Const.FULLNAME}", () =>
                    {
                        ВидиЦінПостачальників page = new ВидиЦінПостачальників();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{БанківськіРахункиОрганізацій_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{БанківськіРахункиОрганізацій_Const.FULLNAME}", () =>
                    {
                        БанківськіРахункиОрганізацій page = new БанківськіРахункиОрганізацій();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{СтруктураПідприємства_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СтруктураПідприємства_Const.FULLNAME}", () =>
                    {
                        СтруктураПідприємства page = new СтруктураПідприємства();
                        page.LoadRecords();
                        return page;
                    });
                });
            }

            Link.AddCaption(vLeft, "Додаткові");
            {
                Link.AddLink(vLeft, $"{Банки_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Банки_Const.FULLNAME}", () =>
                    {
                        Банки page = new Банки();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{Користувачі_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Користувачі_Const.FULLNAME}", () =>
                    {
                        Користувачі page = new Користувачі();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{ФізичніОсоби_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ФізичніОсоби_Const.FULLNAME}", () =>
                    {
                        ФізичніОсоби page = new ФізичніОсоби();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{Файли_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Файли_Const.FULLNAME}", () =>
                    {
                        Файли page = new Файли();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{СтаттяРухуКоштів_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СтаттяРухуКоштів_Const.FULLNAME}", () =>
                    {
                        СтаттяРухуКоштів page = new СтаттяРухуКоштів();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{ВидиЗапасів_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиЗапасів_Const.FULLNAME}", () =>
                    {
                        ВидиЗапасів page = new ВидиЗапасів();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vLeft, $"{КраїниСвіту_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{КраїниСвіту_Const.FULLNAME}", () =>
                    {
                        КраїниСвіту page = new КраїниСвіту();
                        page.LoadRecords();
                        return page;
                    });
                });
            }

            Link.AddSeparator(hBoxList);

            VBox vRight = new VBox(false, 0);
            hBoxList.PackStart(vRight, false, false, 5);

            Link.AddCaption(vRight, "Партнери");
            {
                Link.AddLink(vRight, $"{Контрагенти_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Контрагенти_Const.FULLNAME}", () =>
                    {
                        Контрагенти page = new Контрагенти();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{ДоговориКонтрагентів_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ДоговориКонтрагентів_Const.FULLNAME}", () =>
                    {
                        ДоговориКонтрагентів page = new ДоговориКонтрагентів();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{БанківськіРахункиКонтрагентів_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{БанківськіРахункиКонтрагентів_Const.FULLNAME}", () =>
                    {
                        БанківськіРахункиКонтрагентів page = new БанківськіРахункиКонтрагентів();
                        page.LoadRecords();
                        return page;
                    });
                });
            }

            Link.AddCaption(vRight, "Товари та послуги");
            {
                Link.AddLink(vRight, $"{Номенклатура_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Номенклатура_Const.FULLNAME}", () =>
                    {
                        Номенклатура page = new Номенклатура();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{ХарактеристикиНоменклатури_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ХарактеристикиНоменклатури_Const.FULLNAME}", () =>
                    {
                        ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{ПакуванняОдиниціВиміру_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПакуванняОдиниціВиміру_Const.FULLNAME}", () =>
                    {
                        ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{ВидиНоменклатури_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ВидиНоменклатури_Const.FULLNAME}", () =>
                    {
                        ВидиНоменклатури page = new ВидиНоменклатури();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{ПартіяТоварівКомпозит_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ПартіяТоварівКомпозит_Const.FULLNAME}", () =>
                    {
                        ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{СеріїНоменклатури_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СеріїНоменклатури_Const.FULLNAME}", () =>
                    {
                        СеріїНоменклатури page = new СеріїНоменклатури();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{Виробники_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Виробники_Const.FULLNAME}", () =>
                    {
                        Виробники page = new Виробники();
                        page.LoadRecords();
                        return page;
                    });
                });
            }

            Link.AddCaption(vRight, "Внутрішні");
            {
                Link.AddLink(vRight, $"{Блокнот_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{Блокнот_Const.FULLNAME}", () =>
                    {
                        Блокнот page = new Блокнот();
                        page.LoadRecords();
                        return page;
                    });
                });
            }

            Link.AddCaption(vRight, "Адресне зберігання");
            {
                Link.AddLink(vRight, $"{СкладськіПриміщення_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СкладськіПриміщення_Const.FULLNAME}", () =>
                    {
                        СкладськіПриміщення page = new СкладськіПриміщення();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{СкладськіКомірки_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{СкладськіКомірки_Const.FULLNAME}", () =>
                    {
                        СкладськіКомірки page = new СкладськіКомірки();
                        page.LoadRecords();
                        return page;
                    });
                });

                Link.AddLink(vRight, $"{ТипорозміриКомірок_Const.FULLNAME}", () =>
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ТипорозміриКомірок_Const.FULLNAME}", () =>
                    {
                        ТипорозміриКомірок page = new ТипорозміриКомірок();
                        page.LoadRecords();
                        return page;
                    });
                });
            }

            ShowAll();
        }


    }
}