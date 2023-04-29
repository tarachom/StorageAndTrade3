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
 

*/

using AccountingSoftware;

using Довідники = StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Довідники;

using Перелічення = StorageAndTrade_1_0.Перелічення;
using Константи = StorageAndTrade_1_0.Константи;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    /// <summary>
    /// Спільні функції для довідників 
    /// </summary>
    class ФункціїДляДовідників
    {
        /*
        !!! Функція перенесена із звіту і потребує доробки
        */
        public static void ВідкритиДовідникВідповідноДоВиду(string typeDir, UnigueID unigueID)
        {
            switch (typeDir)
            {
                case "Організація":
                case "Організація_Назва":
                    {
                        Організації page = new Організації() { SelectPointerItem = new Організації_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{Організації_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                case "Номенклатура":
                case "Номенклатура_Назва":
                    {
                        Номенклатура page = new Номенклатура() { SelectPointerItem = new Номенклатура_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{Номенклатура_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadTree();

                        break;
                    }
                case "ХарактеристикаНоменклатури_Назва":
                    {
                        ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури() { SelectPointerItem = new ХарактеристикиНоменклатури_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{ХарактеристикиНоменклатури_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                case "Серія_Номер":
                    {
                        СеріїНоменклатури page = new СеріїНоменклатури() { SelectPointerItem = new СеріїНоменклатури_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{СеріїНоменклатури_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                case "Контрагент_Назва":
                    {
                        Контрагенти page = new Контрагенти() { SelectPointerItem = new Контрагенти_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{Контрагенти_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadTree();

                        break;
                    }
                case "Договір_Назва":
                    {
                        ДоговориКонтрагентів page = new ДоговориКонтрагентів() { SelectPointerItem = new ДоговориКонтрагентів_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{ДоговориКонтрагентів_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                case "Валюта_Назва":
                    {
                        Валюти page = new Валюти() { SelectPointerItem = new Валюти_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{Валюти_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                case "Каса_Назва":
                    {
                        Каси page = new Каси() { SelectPointerItem = new Каси_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{Каси_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                case "Склад_Назва":
                    {
                        Склади page = new Склади() { SelectPointerItem = new Склади_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{Склади_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadTree();

                        break;
                    }
                case "ПартіяТоварівКомпозит_Назва":
                    {
                        ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит() { SelectPointerItem = new ПартіяТоварівКомпозит_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{ПартіяТоварівКомпозит_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                /* -> два документи в довідниках */
                case "ЗамовленняКлієнта_Назва":
                    {
                        ЗамовленняКлієнта page = new ЗамовленняКлієнта() { SelectPointerItem = new ЗамовленняКлієнта_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняКлієнта_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                case "ЗамовленняПостачальнику_Назва":
                    {
                        ЗамовленняПостачальнику page = new ЗамовленняПостачальнику() { SelectPointerItem = new ЗамовленняПостачальнику_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняПостачальнику_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                /* <- два документи в довідниках */
                case "Пакування_Назва":
                    {
                        ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру() { SelectPointerItem = new ПакуванняОдиниціВиміру_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{ПакуванняОдиниціВиміру_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
                case "Комірка_Назва":
                    {
                        СкладськіКомірки page = new СкладськіКомірки() { SelectPointerItem = new СкладськіКомірки_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{СкладськіКомірки_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadTree();

                        break;
                    }
                case "Приміщення_Назва":
                    {
                        СкладськіПриміщення page = new СкладськіПриміщення() { SelectPointerItem = new СкладськіПриміщення_Pointer(unigueID) };
                        Program.GeneralForm?.CreateNotebookPage($"{СкладськіПриміщення_Const.FULLNAME}", () => { return page; }, true);
                        page.LoadRecords();

                        break;
                    }
            }
        }

        /// <summary>
        /// Функція створює договори для контрагента
        /// </summary>
        /// <param name="Контрагент">Контрагент</param>
        public static void СтворитиДоговориКонтрагентаЗаЗамовчуванням(Довідники.Контрагенти_Pointer Контрагент)
        {
            if (Контрагент.IsEmpty())
                return;

            Довідники.ДоговориКонтрагентів_Objest НовийДоговір = new Довідники.ДоговориКонтрагентів_Objest();
            НовийДоговір.Назва = "Основний договір";
            НовийДоговір.Контрагент = Контрагент;
            НовийДоговір.Статус = Перелічення.СтатусиДоговорівКонтрагентів.Діє;
            НовийДоговір.Дата = DateTime.Now;

            Довідники.ДоговориКонтрагентів_Select ВибіркаДоговорівКонтрагента = new Довідники.ДоговориКонтрагентів_Select();

            //Відбір по контрагенту
            ВибіркаДоговорівКонтрагента.QuerySelect.Where.Add(
                new Where(Довідники.ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, Контрагент.UnigueID.UGuid));

            //Відбір по типу договору
            ВибіркаДоговорівКонтрагента.QuerySelect.Where.Add(
                new Where(Comparison.AND, Довідники.ДоговориКонтрагентів_Const.ТипДоговору, Comparison.EQ, (int)Перелічення.ТипДоговорів.ЗПокупцями));

            if (!ВибіркаДоговорівКонтрагента.Select())
            {
                НовийДоговір.New();
                НовийДоговір.Код = (++Константи.НумераціяДовідників.Контрагенти_Const).ToString("D6");
                НовийДоговір.ТипДоговору = Перелічення.ТипДоговорів.ЗПокупцями;
                НовийДоговір.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта;
                НовийДоговір.Save();
            }

            //Відбір по типу договору
            ВибіркаДоговорівКонтрагента.QuerySelect.Where[1].Value = (int)Перелічення.ТипДоговорів.ЗПостачальниками;

            if (!ВибіркаДоговорівКонтрагента.Select())
            {
                НовийДоговір.New();
                НовийДоговір.Код = (++Константи.НумераціяДовідників.Контрагенти_Const).ToString("D6");
                НовийДоговір.ТипДоговору = Перелічення.ТипДоговорів.ЗПостачальниками;
                НовийДоговір.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ОплатаПостачальнику;
                НовийДоговір.Save();
            }
        }

        /// <summary>
        /// Функція повертає вказівник на серійний номер, або створює новий
        /// </summary>
        /// <returns>Вказівник на елемент довідника СеріїНоменклатури</returns>
        public static Довідники.СеріїНоменклатури_Pointer ОтриматиВказівникНаСеріюНоменклатури(string СерійнийНомер)
        {
            СерійнийНомер = СерійнийНомер.Trim();

            Довідники.СеріїНоменклатури_Select серіїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            серіїНоменклатури_Select.QuerySelect.Where.Add(new Where(Довідники.СеріїНоменклатури_Const.Номер, Comparison.EQ, СерійнийНомер));

            if (серіїНоменклатури_Select.SelectSingle())
                return серіїНоменклатури_Select.Current!;
            else
            {
                Довідники.СеріїНоменклатури_Objest серіїНоменклатури_Objest = new Довідники.СеріїНоменклатури_Objest();
                серіїНоменклатури_Objest.New();
                серіїНоменклатури_Objest.Номер = СерійнийНомер;
                серіїНоменклатури_Objest.ДатаСтворення = DateTime.Now;
                серіїНоменклатури_Objest.Save();

                return серіїНоменклатури_Objest.GetDirectoryPointer();
            }
        }

        /// <summary>
        /// Створення нового запису довідника Файли
        /// </summary>
        /// <param name="PathToFile">Шлях до файлу</param>
        /// <returns></returns>
        public static Довідники.Файли_Pointer ЗавантажитиФайл(string PathToFile)
        {
            FileInfo fileInfo = new FileInfo(PathToFile);

            Довідники.Файли_Objest файли_Objest = new Довідники.Файли_Objest();
            файли_Objest.New();
            файли_Objest.Код = (++Константи.НумераціяДовідників.Файли_Const).ToString("D6");
            файли_Objest.НазваФайлу = fileInfo.Name;
            файли_Objest.Назва = Path.GetFileNameWithoutExtension(PathToFile);
            файли_Objest.Розмір = Math.Round((decimal)(fileInfo.Length / 1024)).ToString() + " KB";
            файли_Objest.ДатаСтворення = DateTime.Now;
            файли_Objest.БінарніДані = File.ReadAllBytes(PathToFile);
            файли_Objest.Save();

            return файли_Objest.GetDirectoryPointer();
        }


    }
}
