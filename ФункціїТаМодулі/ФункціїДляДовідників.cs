/*

Спільні функції для довідників 

*/

using AccountingSoftware;

using Довідники = StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

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
        public static async ValueTask СтворитиДоговориКонтрагентаЗаЗамовчуванням(Довідники.Контрагенти_Pointer Контрагент)
        {
            if (Контрагент.IsEmpty())
                return;

            Довідники.ДоговориКонтрагентів_Objest НовийДоговір = new Довідники.ДоговориКонтрагентів_Objest()
            {
                Назва = "Основний договір",
                Контрагент = Контрагент,
                Статус = Перелічення.СтатусиДоговорівКонтрагентів.Діє,
                Дата = DateTime.Now
            };

            Довідники.ДоговориКонтрагентів_Select ВибіркаДоговорівКонтрагента = new Довідники.ДоговориКонтрагентів_Select();

            //Відбір по контрагенту
            ВибіркаДоговорівКонтрагента.QuerySelect.Where.Add(
                new Where(Довідники.ДоговориКонтрагентів_Const.Контрагент, Comparison.EQ, Контрагент.UnigueID.UGuid));

            //Відбір по типу договору
            ВибіркаДоговорівКонтрагента.QuerySelect.Where.Add(
                new Where(Довідники.ДоговориКонтрагентів_Const.ТипДоговору, Comparison.EQ, (int)Перелічення.ТипДоговорів.ЗПокупцями));

            if (!await ВибіркаДоговорівКонтрагента.Select())
            {
                await НовийДоговір.New();
                НовийДоговір.ТипДоговору = Перелічення.ТипДоговорів.ЗПокупцями;
                НовийДоговір.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ПоступленняОплатиВідКлієнта;
                await НовийДоговір.Save();
            }

            //Відбір по типу договору
            ВибіркаДоговорівКонтрагента.QuerySelect.Where[1].Value = (int)Перелічення.ТипДоговорів.ЗПостачальниками;

            if (!await ВибіркаДоговорівКонтрагента.Select())
            {
                await НовийДоговір.New();
                НовийДоговір.ТипДоговору = Перелічення.ТипДоговорів.ЗПостачальниками;
                НовийДоговір.ГосподарськаОперація = Перелічення.ГосподарськіОперації.ОплатаПостачальнику;
                await НовийДоговір.Save();
            }
        }

        /// <summary>
        /// Функція повертає вказівник на серійний номер, або створює новий
        /// </summary>
        /// <returns>Вказівник на елемент довідника СеріїНоменклатури</returns>
        public static async Task<Довідники.СеріїНоменклатури_Pointer> ОтриматиВказівникНаСеріюНоменклатури(string СерійнийНомер)
        {
            СерійнийНомер = СерійнийНомер.Trim();

            Довідники.СеріїНоменклатури_Select серіїНоменклатури_Select = new Довідники.СеріїНоменклатури_Select();
            серіїНоменклатури_Select.QuerySelect.Where.Add(new Where(Довідники.СеріїНоменклатури_Const.Номер, Comparison.EQ, СерійнийНомер));

            if (await серіїНоменклатури_Select.SelectSingle())
                return серіїНоменклатури_Select.Current!;
            else
            {
                Довідники.СеріїНоменклатури_Objest серіїНоменклатури_Objest = new Довідники.СеріїНоменклатури_Objest();
                await серіїНоменклатури_Objest.New();
                серіїНоменклатури_Objest.Номер = СерійнийНомер;
                серіїНоменклатури_Objest.ДатаСтворення = DateTime.Now;
                await серіїНоменклатури_Objest.Save();

                return серіїНоменклатури_Objest.GetDirectoryPointer();
            }
        }

        /// <summary>
        /// Створення нового запису довідника Файли
        /// </summary>
        /// <param name="PathToFile">Шлях до файлу</param>
        /// <returns></returns>
        public static async Task<Довідники.Файли_Pointer> ЗавантажитиФайл(string PathToFile)
        {
            FileInfo fileInfo = new FileInfo(PathToFile);

            Довідники.Файли_Objest файли_Objest = new Довідники.Файли_Objest();
            await файли_Objest.New();
            файли_Objest.НазваФайлу = fileInfo.Name;
            файли_Objest.Назва = Path.GetFileNameWithoutExtension(PathToFile);
            файли_Objest.Розмір = Math.Round((decimal)(fileInfo.Length / 1024)).ToString() + " KB";
            файли_Objest.ДатаСтворення = DateTime.Now;
            файли_Objest.БінарніДані = File.ReadAllBytes(PathToFile);
            await файли_Objest.Save();

            return файли_Objest.GetDirectoryPointer();
        }
    }
}
