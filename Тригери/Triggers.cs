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
 
Модуль функцій зворотнього виклику.

1. Перед записом
2. Після запису
3. Перед видаленням
 
*/

using AccountingSoftware;
using StorageAndTrade;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;


namespace StorageAndTrade_1_0.Документи
{
    class ЗамовленняПостачальнику_Triggers
    {
        public static async ValueTask New(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗамовленняПостачальнику_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗамовленняПостачальнику_Objest ДокументОбєкт, ЗамовленняПостачальнику_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗамовленняПостачальнику_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗамовленняПостачальнику_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗамовленняПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПоступленняТоварівТаПослуг_Triggers
    {
        public static async ValueTask New(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоступленняТоварівТаПослуг_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт, ПоступленняТоварівТаПослуг_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПоступленняТоварівТаПослуг_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт, bool label)
        {
            // Помітка на видалення всіх партій
            if (label == true)
            {
                ПартіяТоварівКомпозит_Select партіяТоварівКомпозит_Select = new ПартіяТоварівКомпозит_Select();
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.ПоступленняТоварівТаПослуг, Comparison.EQ, ДокументОбєкт.UnigueID.UGuid));
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.DELETION_LABEL, Comparison.NOT, true));
                await партіяТоварівКомпозит_Select.Select();

                while (партіяТоварівКомпозит_Select.MoveNext())
                    if (партіяТоварівКомпозит_Select.Current != null)
                    {
                        ПартіяТоварівКомпозит_Objest? партіяТоварівКомпозит_Objest = await партіяТоварівКомпозит_Select.Current.GetDirectoryObject();
                        if (партіяТоварівКомпозит_Objest != null)
                            await партіяТоварівКомпозит_Objest.SetDeletionLabel();
                    }
            }
        }

        public static async ValueTask BeforeDelete(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ЗамовленняКлієнта_Triggers
    {
        public static async ValueTask New(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗамовленняКлієнта_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗамовленняКлієнта_Objest ДокументОбєкт, ЗамовленняКлієнта_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗамовленняКлієнта_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗамовленняКлієнта_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗамовленняКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РеалізаціяТоварівТаПослуг_Triggers
    {
        public static async ValueTask New(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РеалізаціяТоварівТаПослуг_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт, РеалізаціяТоварівТаПослуг_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РеалізаціяТоварівТаПослуг_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВстановленняЦінНоменклатури_Triggers
    {
        public static async ValueTask New(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВстановленняЦінНоменклатури_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВстановленняЦінНоменклатури_Objest ДокументОбєкт, ВстановленняЦінНоменклатури_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ВстановленняЦінНоменклатури_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВстановленняЦінНоменклатури_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПрихіднийКасовийОрдер_Triggers
    {
        public static async ValueTask New(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПрихіднийКасовийОрдер_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПрихіднийКасовийОрдер_Objest ДокументОбєкт, ПрихіднийКасовийОрдер_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПрихіднийКасовийОрдер_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПрихіднийКасовийОрдер_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РозхіднийКасовийОрдер_Triggers
    {
        public static async ValueTask New(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозхіднийКасовийОрдер_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РозхіднийКасовийОрдер_Objest ДокументОбєкт, РозхіднийКасовийОрдер_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РозхіднийКасовийОрдер_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РозхіднийКасовийОрдер_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПереміщенняТоварів_Triggers
    {
        public static async ValueTask New(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПереміщенняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПереміщенняТоварів_Objest ДокументОбєкт, ПереміщенняТоварів_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПереміщенняТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПереміщенняТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПереміщенняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПоверненняТоварівПостачальнику_Triggers
    {
        public static async ValueTask New(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоверненняТоварівПостачальнику_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт, ПоверненняТоварівПостачальнику_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПоверненняТоварівПостачальнику_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПоверненняТоварівВідКлієнта_Triggers
    {
        public static async ValueTask New(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПоверненняТоварівВідКлієнта_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт, ПоверненняТоварівВідКлієнта_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class АктВиконанихРобіт_Triggers
    {
        public static async ValueTask New(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.АктВиконанихРобіт_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(АктВиконанихРобіт_Objest ДокументОбєкт, АктВиконанихРобіт_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{АктВиконанихРобіт_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(АктВиконанихРобіт_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(АктВиконанихРобіт_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВведенняЗалишків_Triggers
    {
        public static async ValueTask New(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВведенняЗалишків_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВведенняЗалишків_Objest ДокументОбєкт, ВведенняЗалишків_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ВведенняЗалишків_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВведенняЗалишків_Objest ДокументОбєкт, bool label)
        {
            // Помітка на виделення всіх партій
            if (label == true)
            {
                ПартіяТоварівКомпозит_Select партіяТоварівКомпозит_Select = new ПартіяТоварівКомпозит_Select();
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.ВведенняЗалишків, Comparison.EQ, ДокументОбєкт.UnigueID.UGuid));
                партіяТоварівКомпозит_Select.QuerySelect.Where.Add(new Where(ПартіяТоварівКомпозит_Const.DELETION_LABEL, Comparison.NOT, true));
                await партіяТоварівКомпозит_Select.Select();

                while (партіяТоварівКомпозит_Select.MoveNext())
                    if (партіяТоварівКомпозит_Select.Current != null)
                    {
                        ПартіяТоварівКомпозит_Objest? партіяТоварівКомпозит_Objest = await партіяТоварівКомпозит_Select.Current.GetDirectoryObject();
                        if (партіяТоварівКомпозит_Objest != null)
                            await партіяТоварівКомпозит_Objest.SetDeletionLabel();
                    }
            }
        }

        public static async ValueTask BeforeDelete(ВведенняЗалишків_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПерерахунокТоварів_Triggers
    {
        public static async ValueTask New(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПерерахунокТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            //Відповідального можна отримати з Користувача
            var Користувач_Обєкт = await Program.Користувач.GetDirectoryObject();
            if (Користувач_Обєкт != null)
                ДокументОбєкт.Відповідальний = Користувач_Обєкт.ФізичнаОсоба;
        }

        public static async ValueTask Copying(ПерерахунокТоварів_Objest ДокументОбєкт, ПерерахунокТоварів_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПерерахунокТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПерерахунокТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПерерахунокТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПсуванняТоварів_Triggers
    {
        public static async ValueTask New(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПсуванняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПсуванняТоварів_Objest ДокументОбєкт, ПсуванняТоварів_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПсуванняТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПсуванняТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПсуванняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ВнутрішнєСпоживанняТоварів_Triggers
    {
        public static async ValueTask New(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ВнутрішнєСпоживанняТоварів_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт, ВнутрішнєСпоживанняТоварів_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РахунокФактура_Triggers
    {
        public static async ValueTask New(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РахунокФактура_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;
            ДокументОбєкт.Менеджер = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РахунокФактура_Objest ДокументОбєкт, РахунокФактура_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РахунокФактура_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РахунокФактура_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РахунокФактура_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РахунокФактура_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РахунокФактура_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РозміщенняТоварівНаСкладі_Triggers
    {
        public static async ValueTask New(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозміщенняТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт, РозміщенняТоварівНаСкладі_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РозміщенняТоварівНаСкладі_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РозміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ПереміщенняТоварівНаСкладі_Triggers
    {
        public static async ValueTask New(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ПереміщенняТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт, ПереміщенняТоварівНаСкладі_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ПереміщенняТоварівНаСкладі_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ПереміщенняТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class ЗбіркаТоварівНаСкладі_Triggers
    {
        public static async ValueTask New(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.ЗбіркаТоварівНаСкладі_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт, ЗбіркаТоварівНаСкладі_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{ЗбіркаТоварівНаСкладі_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(ЗбіркаТоварівНаСкладі_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class РозміщенняНоменклатуриПоКоміркам_Triggers
    {
        public static async ValueTask New(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.РозміщенняНоменклатуриПоКоміркам_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт, РозміщенняНоменклатуриПоКоміркам_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";

            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(РозміщенняНоменклатуриПоКоміркам_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

    class КорегуванняБоргу_Triggers
    {
        public static async ValueTask New(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            ДокументОбєкт.НомерДок = (++НумераціяДокументів.КорегуванняБоргу_Const).ToString("D8");
            ДокументОбєкт.ДатаДок = DateTime.Now;
            ДокументОбєкт.Автор = Program.Користувач;

            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(КорегуванняБоргу_Objest ДокументОбєкт, КорегуванняБоргу_Objest Основа)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{КорегуванняБоргу_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToShortDateString()}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(КорегуванняБоргу_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeDelete(КорегуванняБоргу_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }

}