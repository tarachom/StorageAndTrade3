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

Функції для журналів

*/
using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ФункціїДляЖурналів
    {
        public static void ВідкритиСписокДокументів(Widget relative_to, string[] allowDocument, ТипПеріодуДляЖурналівДокументів periodWhere = 0)
        {
            VBox vBox = new VBox();

            foreach (string typeDoc in allowDocument)
            {
                LinkButton lb = new LinkButton(typeDoc, " " + typeDoc) { Halign = Align.Start };
                vBox.PackStart(lb, false, false, 0);

                lb.Clicked += (object? sender, EventArgs args) =>
                {
                    ФункціїДляЖурналів.ВідкритиЖурналВідповідноДоВидуДокументу(typeDoc, new UnigueID(), periodWhere);
                };
            }

            Popover PopoverSelect = new Popover(relative_to) { Position = PositionType.Bottom, BorderWidth = 2 };

            PopoverSelect.Add(vBox);
            PopoverSelect.ShowAll();
        }

        public static void ВідкритиЖурналВідповідноДоВидуДокументу(string typeDoc, UnigueID unigueID, ТипПеріодуДляЖурналівДокументів periodWhere = 0)
        {
            switch (typeDoc)
            {
                case "АктВиконанихРобіт":
                    {
                        АктВиконанихРобіт page = new АктВиконанихРобіт() { SelectPointerItem = new АктВиконанихРобіт_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Акт виконаних робіт", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ЗамовленняКлієнта":
                    {
                        ЗамовленняКлієнта page = new ЗамовленняКлієнта() { SelectPointerItem = new ЗамовленняКлієнта_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Замовлення клієнтів", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "РахунокФактура":
                    {
                        РахунокФактура page = new РахунокФактура() { SelectPointerItem = new РахунокФактура_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Рахунок фактура", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ЗамовленняПостачальнику":
                    {
                        ЗамовленняПостачальнику page = new ЗамовленняПостачальнику() { SelectPointerItem = new ЗамовленняПостачальнику_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Замовлення постачальнику", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "РеалізаціяТоварівТаПослуг":
                    {
                        РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг() { SelectPointerItem = new РеалізаціяТоварівТаПослуг_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Реалізація товарів та послуг", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ПоступленняТоварівТаПослуг":
                    {
                        ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг() { SelectPointerItem = new ПоступленняТоварівТаПослуг_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Поступлення товарів та послуг", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "РозхіднийКасовийОрдер":
                    {
                        РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер() { SelectPointerItem = new РозхіднийКасовийОрдер_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Розхідний касовий ордер", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ПрихіднийКасовийОрдер":
                    {
                        ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер() { SelectPointerItem = new ПрихіднийКасовийОрдер_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Прихідний касовий ордер", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ПереміщенняТоварів":
                    {
                        ПереміщенняТоварів page = new ПереміщенняТоварів() { SelectPointerItem = new ПереміщенняТоварів_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Переміщення товарів", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ПоверненняТоварівВідКлієнта":
                    {
                        ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта() { SelectPointerItem = new ПоверненняТоварівВідКлієнта_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Повернення товарів від клієнта", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ПоверненняТоварівПостачальнику":
                    {
                        ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику() { SelectPointerItem = new ПоверненняТоварівПостачальнику_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Повернення товарів постачальнику", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ВнутрішнєСпоживанняТоварів":
                    {
                        ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів() { SelectPointerItem = new ВнутрішнєСпоживанняТоварів_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Внутрішнє споживання товарів", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ПсуванняТоварів":
                    {
                        ПсуванняТоварів page = new ПсуванняТоварів() { SelectPointerItem = new ПсуванняТоварів_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Псування товарів", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ВведенняЗалишків":
                    {
                        ВведенняЗалишків page = new ВведенняЗалишків() { SelectPointerItem = new ВведенняЗалишків_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Введення залишків", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ВстановленняЦінНоменклатури":
                    {
                        ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури() { SelectPointerItem = new ВстановленняЦінНоменклатури_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Встановлення цін номенклатури", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "РозміщенняТоварівНаСкладі":
                    {
                        РозміщенняТоварівНаСкладі page = new РозміщенняТоварівНаСкладі() { SelectPointerItem = new РозміщенняТоварівНаСкладі_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Розміщення товарів на складі", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ЗбіркаТоварівНаСкладі":
                    {
                        ЗбіркаТоварівНаСкладі page = new ЗбіркаТоварівНаСкладі() { SelectPointerItem = new ЗбіркаТоварівНаСкладі_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Збірка товарів на складі", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                case "ПереміщенняТоварівНаСкладі":
                    {
                        ПереміщенняТоварівНаСкладі page = new ПереміщенняТоварівНаСкладі() { SelectPointerItem = new ПереміщенняТоварівНаСкладі_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage("Переміщення товарів на складі", () => { return page; }, true);

                        if (periodWhere != 0)
                        {
                            page.PeriodWhere = periodWhere;
                            page.SetValue();
                        }
                        else
                            page.LoadRecords();

                        break;
                    }
                default:
                    {
                        Message.Info(null, "Для даного типу документу не оприділений журнал документів");
                        break;
                    }
            }

        }
    }
}
