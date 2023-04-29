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
        public static void ВідкритиСписокДокументів(Widget relative_to, Dictionary<string, string> allowDocument, ТипПеріодуДляЖурналівДокументів periodWhere = 0)
        {
            VBox vBox = new VBox();

            foreach (KeyValuePair<string, string> typeDoc in allowDocument)
            {
                LinkButton lb = new LinkButton(typeDoc.Value, " " + typeDoc.Value) { Halign = Align.Start };
                vBox.PackStart(lb, false, false, 0);

                lb.Clicked += (object? sender, EventArgs args) =>
                {
                    ФункціїДляЖурналів.ВідкритиЖурналВідповідноДоВидуДокументу(typeDoc.Key, new UnigueID(), periodWhere);
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
                        Program.GeneralForm?.CreateNotebookPage($"{АктВиконанихРобіт_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняКлієнта_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{РахунокФактура_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняПостачальнику_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{РеалізаціяТоварівТаПослуг_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ПоступленняТоварівТаПослуг_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{РозхіднийКасовийОрдер_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ПрихіднийКасовийОрдер_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ПереміщенняТоварів_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ПоверненняТоварівВідКлієнта_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ПоверненняТоварівПостачальнику_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ПсуванняТоварів_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ВведенняЗалишків_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ВстановленняЦінНоменклатури_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{РозміщенняТоварівНаСкладі_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ЗбіркаТоварівНаСкладі_Const.FULLNAME}", () => { return page; }, true);

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
                        Program.GeneralForm?.CreateNotebookPage($"{ПереміщенняТоварівНаСкладі_Const.FULLNAME}", () => { return page; }, true);

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
