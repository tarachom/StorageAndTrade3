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
 
Модуль функцій зворотнього виклику.

1. Перед записом
2. Після запису
3. Перед видаленням
 
*/

using System;
using System.Collections.Generic;

using AccountingSoftware;
using StorageAndTrade;
using Конфа = StorageAndTrade_1_0;

namespace StorageAndTrade_1_0.Константи
{

}

namespace StorageAndTrade_1_0.Довідники
{
    class Валюти_Triggers
    {
        public static void BeforeRecording(Валюти_Objest ДовідникОбєкт)
        {

        }

        public static void AfterRecording(Валюти_Objest ДовідникОбєкт)
        {
            
        }

        public static void BeforeDelete(Валюти_Objest ДовідникОбєкт)
        {
            //Очистити регістр КурсиВалют
			//при видаленні валюти

            string query = $@"
DELETE FROM 
	{РегістриВідомостей.КурсиВалют_Const.TABLE} AS КурсиВалют
WHERE
    КурсиВалют.{РегістриВідомостей.КурсиВалют_Const.Валюта} = @Валюта
";

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("Валюта", ДовідникОбєкт.UnigueID.UGuid);

            Конфа.Config.Kernel.DataBase.ExecuteSQL(query, paramQuery);
        }
    }

    class Контрагенти_Triggers
	{
		public static void BeforeRecording(Контрагенти_Objest ДовідникОбєкт)
		{

		}

		public static void AfterRecording(Контрагенти_Objest ДовідникОбєкт)
		{
			ФункціїДляДовідників.СтворитиДоговориКонтрагентаЗаЗамовчуванням(ДовідникОбєкт.GetDirectoryPointer());
		}

		public static void BeforeDelete(Контрагенти_Objest ДовідникОбєкт)
		{

		}
	}

    class Номенклатура_Triggers
    {
        public static void BeforeRecording(Номенклатура_Objest ДовідникОбєкт)
        {

        }
        public static void AfterRecording(Номенклатура_Objest ДовідникОбєкт)
        {

        }

        public static void BeforeDelete(Номенклатура_Objest ДовідникОбєкт)
        {
            //
            //Очистка штрих-кодів
            //

            
        }
    }

    class Номенклатура_Папки_Triggers
	{
		public static void BeforeRecording(Номенклатура_Папки_Objest ДовідникОбєкт)
		{

		}
		public static void AfterRecording(Номенклатура_Папки_Objest ДовідникОбєкт)
		{

		}

		public static void BeforeDelete(Номенклатура_Папки_Objest ДовідникОбєкт)
		{
			//
			//Елементи переносяться на верхній рівень
			//

			Номенклатура_Select номенклатура_Select = new Номенклатура_Select();
			номенклатура_Select.QuerySelect.Where.Add(new Where(Номенклатура_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			номенклатура_Select.Select();

			while (номенклатура_Select.MoveNext())
			{
				Номенклатура_Objest номенклатура_Objest = номенклатура_Select.Current.GetDirectoryObject();
				номенклатура_Objest.Папка = new Номенклатура_Папки_Pointer();
				номенклатура_Objest.Save();
			}

			//
			//Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
			//

			Номенклатура_Папки_Select номенклатура_Папки_Select = new Номенклатура_Папки_Select();
			номенклатура_Папки_Select.QuerySelect.Where.Add(new Where(Номенклатура_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			номенклатура_Папки_Select.Select();

			while (номенклатура_Папки_Select.MoveNext())
			{
				Номенклатура_Папки_Objest номенклатура_Папки_Objest = номенклатура_Папки_Select.Current.GetDirectoryObject();
				if (номенклатура_Папки_Objest != null)
					номенклатура_Папки_Objest.Delete();

			}
		}
	}

	class Контрагенти_Папки_Triggers
	{
		public static void BeforeRecording(Контрагенти_Папки_Objest ДовідникОбєкт)
		{

		}
		public static void AfterRecording(Контрагенти_Папки_Objest ДовідникОбєкт)
		{

		}

		public static void BeforeDelete(Контрагенти_Папки_Objest ДовідникОбєкт)
		{
			//
			//Елементи переносяться на верхній рівень
			//

			Контрагенти_Select контрагенти_Select = new Контрагенти_Select();
			контрагенти_Select.QuerySelect.Where.Add(new Where(Контрагенти_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			контрагенти_Select.Select();

			while (контрагенти_Select.MoveNext())
			{
				Контрагенти_Objest контрагенти_Objest = контрагенти_Select.Current.GetDirectoryObject();
				контрагенти_Objest.Папка = new Контрагенти_Папки_Pointer();
				контрагенти_Objest.Save();
			}

			//
			//Вкладені папки видаляються. Для кожної папки буде викликана функція BeforeDelete
			//

			Контрагенти_Папки_Select контрагенти_Папки_Select = new Контрагенти_Папки_Select();
			контрагенти_Папки_Select.QuerySelect.Where.Add(new Where(Контрагенти_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			контрагенти_Папки_Select.Select();

			while (контрагенти_Папки_Select.MoveNext())
			{
				Контрагенти_Папки_Objest контрагенти_Папки_Objest = контрагенти_Папки_Select.Current.GetDirectoryObject();
				if (контрагенти_Папки_Objest != null)
					контрагенти_Папки_Objest.Delete();

			}
		}
	}

	class ДоговориКонтрагентів_Triggers
	{
		public static void BeforeRecording(ДоговориКонтрагентів_Objest ДовідникОбєкт)
		{
			string НазваПереліченняЗКонфігурації =
				Конфа.Config.Kernel.Conf.Enums["ТипДоговорів"].Fields[ДовідникОбєкт.ТипДоговору.ToString()].Desc;

			ДовідникОбєкт.ТипДоговоруПредставлення = НазваПереліченняЗКонфігурації;
		}
		public static void AfterRecording(ДоговориКонтрагентів_Objest ДовідникОбєкт)
		{

		}

		public static void BeforeDelete(ДоговориКонтрагентів_Objest ДовідникОбєкт)
		{

		}
	}

	class Склади_Папки_Triggers
	{
		public static void BeforeRecording(Склади_Папки_Objest ДовідникОбєкт)
		{

		}
		public static void AfterRecording(Склади_Папки_Objest ДовідникОбєкт)
		{

		}

		public static void BeforeDelete(Склади_Папки_Objest ДовідникОбєкт)
		{
			//
			//Елементи переносяться на верхній рівень
			//

			Склади_Select склади_Select = new Склади_Select();
			склади_Select.QuerySelect.Where.Add(new Where(Склади_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			склади_Select.Select();

			while (склади_Select.MoveNext())
			{
				Склади_Objest склади_Objest = склади_Select.Current.GetDirectoryObject();
				склади_Objest.Папка = new Склади_Папки_Pointer();
				склади_Objest.Save();
			}

			//
			//Вкладені папки видяляються. Для кожної папки буде викликана функція BeforeDelete
			//

			Склади_Папки_Select cклади_Папки_Select = new Склади_Папки_Select();
			cклади_Папки_Select.QuerySelect.Where.Add(new Where(Склади_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
			cклади_Папки_Select.Select();

			while (cклади_Папки_Select.MoveNext())
			{
				Склади_Папки_Objest cклади_Папки_Objest = cклади_Папки_Select.Current.GetDirectoryObject();
				if (cклади_Папки_Objest != null)
					cклади_Папки_Objest.Delete();

			}
		}
	}

	class СеріїНоменклатури_Triggers
	{
		public static void BeforeRecording(СеріїНоменклатури_Objest ДовідникОбєкт)
		{
			СеріїНоменклатури_Select серіїНоменклатури_Select = new СеріїНоменклатури_Select();
			серіїНоменклатури_Select.QuerySelect.Where.Add(new Where(СеріїНоменклатури_Const.Номер, Comparison.EQ, ДовідникОбєкт.Номер));
			серіїНоменклатури_Select.QuerySelect.Where.Add(new Where(Comparison.AND, "uid", Comparison.NOT, ДовідникОбєкт.UnigueID.UGuid));

			if (серіїНоменклатури_Select.SelectSingle())
			{
				//MessageBox.Show($"Помилка: Серійний номер [ {ДовідникОбєкт.Номер} ] вже існує в базі даних.");

				ДовідникОбєкт.Коментар = $"Помилка: Серійний номер [ {ДовідникОбєкт.Номер} ] вже існує в базі даних. " + ДовідникОбєкт.Коментар;
				ДовідникОбєкт.Номер = Guid.NewGuid().ToString();
			}
		}

		public static void AfterRecording(СеріїНоменклатури_Objest ДовідникОбєкт)
		{

		}

		public static void BeforeDelete(СеріїНоменклатури_Objest ДовідникОбєкт)
		{

		}
	}

}

namespace StorageAndTrade_1_0.Документи
{
	class ЗамовленняКлієнта_Triggers
    {
		/// <summary>
		/// Перед записом
		/// </summary>
		/// <param name="ДокументОбєкт"></param>
		public static void BeforeRecording(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{

		}

		/// <summary>
		/// Після запису
		/// </summary>
		/// <param name="ДокументОбєкт"></param>
		public static void AfterRecording(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{
			
		}

		/// <summary>
		/// Перед видаленням
		/// </summary>
		/// <param name="ДокументОбєкт"></param>
        public static void BeforeDelete(ЗамовленняКлієнта_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}

	class РахунокФактура_Triggers
	{
		/// <summary>
		/// Перед записом
		/// </summary>
		/// <param name="ДокументОбєкт"></param>
		public static void BeforeRecording(РахунокФактура_Objest ДокументОбєкт)
		{

		}

		/// <summary>
		/// Після запису
		/// </summary>
		/// <param name="ДокументОбєкт"></param>
		public static void AfterRecording(РахунокФактура_Objest ДокументОбєкт)
		{

		}

		/// <summary>
		/// Перед видаленням
		/// </summary>
		/// <param name="ДокументОбєкт"></param>
		public static void BeforeDelete(РахунокФактура_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}

	class РеалізаціяТоварівТаПослуг_Triggers
    {
		public static void BeforeRecording(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{
			
        }

        public static void BeforeDelete(РеалізаціяТоварівТаПослуг_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}

	class АктВиконанихРобіт_Triggers
	{
		public static void BeforeRecording(АктВиконанихРобіт_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(АктВиконанихРобіт_Objest ДокументОбєкт)
		{

		}

		public static void BeforeDelete(АктВиконанихРобіт_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}

	class ПоступленняТоварівТаПослуг_Triggers
	{
		public static void BeforeRecording(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
			
		}

		public static void AfterRecording(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
			
        }

		public static void BeforeDelete(ПоступленняТоварівТаПослуг_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}

	}

	class ЗамовленняПостачальнику_Triggers
	{
		public static void BeforeRecording(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{
			
        }

		public static void BeforeDelete(ЗамовленняПостачальнику_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}

	class ПоверненняТоварівВідКлієнта_Triggers
    {
		public static void BeforeRecording(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
		{
			
        }

		public static void BeforeDelete(ПоверненняТоварівВідКлієнта_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}

	class ПоверненняТоварівПостачальнику_Triggers
	{
		public static void BeforeRecording(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
		{
			
		}

		public static void BeforeDelete(ПоверненняТоварівПостачальнику_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}

	class ПрихіднийКасовийОрдер_Triggers
	{
		public static void BeforeRecording(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			
		}

		public static void BeforeDelete(ПрихіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}

	}

	class РозхіднийКасовийОрдер_Triggers
	{
		public static void BeforeRecording(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			
		}

		public static void BeforeDelete(РозхіднийКасовийОрдер_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}

	}

	class ПереміщенняТоварів_Triggers
	{
		public static void BeforeRecording(ПереміщенняТоварів_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ПереміщенняТоварів_Objest ДокументОбєкт)
		{

		}

		public static void BeforeDelete(ПереміщенняТоварів_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}

	}

	class ВстановленняЦінНоменклатури_Triggers
	{
		public static void BeforeRecording(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
		{

		}

		public static void BeforeDelete(ВстановленняЦінНоменклатури_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}

	}

	class ВведенняЗалишків_Triggers
	{
		public static void BeforeRecording(ВведенняЗалишків_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ВведенняЗалишків_Objest ДокументОбєкт)
		{

		}

		public static void BeforeDelete(ВведенняЗалишків_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}

	class ВнутрішнєСпоживанняТоварів_Triggers
	{
		public static void BeforeRecording(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
		{

		}

		public static void BeforeDelete(ВнутрішнєСпоживанняТоварів_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}

	class ПсуванняТоварів_Triggers
	{
		public static void BeforeRecording(ПсуванняТоварів_Objest ДокументОбєкт)
		{

		}

		public static void AfterRecording(ПсуванняТоварів_Objest ДокументОбєкт)
		{

		}

		public static void BeforeDelete(ПсуванняТоварів_Objest ДокументОбєкт)
		{
			ДокументОбєкт.ClearSpendTheDocument();
		}
	}
}