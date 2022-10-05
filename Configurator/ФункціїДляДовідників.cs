/*
Copyright (C) 2019-2022 TARAKHOMYN YURIY IVANOVYCH
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

using System;
using System.IO;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;
using Довідники = StorageAndTrade_1_0.Довідники;
using Документи = StorageAndTrade_1_0.Документи;
using Перелічення = StorageAndTrade_1_0.Перелічення;
using Константи = StorageAndTrade_1_0.Константи;


namespace StorageAndTrade
{
    /// <summary>
    /// Спільні функції для довідників
    /// </summary>
    class ФункціїДляДовідників
    {
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
                return серіїНоменклатури_Select.Current;
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
