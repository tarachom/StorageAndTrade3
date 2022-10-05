
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
 *
 * Конфігурації "Зберігання та Торгівля"
 * Автор Тарахомин Юрій Іванович, accounting.org.ua
 * Дата конфігурації: 30.09.2022 08:05:23
 *
 */

using System;
using System.Collections.Generic;
using AccountingSoftware;

namespace StorageAndTrade_1_0
{
    public static class Config
    {
        public static Kernel Kernel { get; set; }
        public static Kernel KernelBackgroundTask { get; set; }
        public static Kernel KernelParalelWork { get; set; }
		
        public static void ReadAllConstants()
        {
            Константи.ЗначенняЗаЗамовчуванням.ReadAll();
            Константи.Системні.ReadAll();
            Константи.ВіртуальніТаблиціРегістрів.ReadAll();
            Константи.НумераціяДокументів.ReadAll();
            Константи.НумераціяДовідників.ReadAll();
            Константи.ЖурналиДокументів.ReadAll();
            Константи.ПартіїТоварів.ReadAll();
            Константи.ЗавантаженняДанихІзСайтів.ReadAll();
            Константи.ПриЗапускуПрограми.ReadAll();
            
        }
    }
}

namespace StorageAndTrade_1_0.Константи
{
    
	#region CONSTANTS BLOCK "ЗначенняЗаЗамовчуванням"
    public static class ЗначенняЗаЗамовчуванням
    {
        public static void ReadAll()
        {
            
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            bool IsSelect = Config.Kernel.DataBase.SelectAllConstants("tab_constants",
                 new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_g4", "col_g5", "col_g9" }, fieldValue);
            
            if (IsSelect)
            {
                m_ОсновнаОрганізація_Const = new Довідники.Організації_Pointer(fieldValue["col_a1"]);
                m_ОснонийСклад_Const = new Довідники.Склади_Pointer(fieldValue["col_a2"]);
                m_ОсновнаВалюта_Const = new Довідники.Валюти_Pointer(fieldValue["col_a3"]);
                m_ОсновнийПостачальник_Const = new Довідники.Контрагенти_Pointer(fieldValue["col_a4"]);
                m_ОсновнийПокупець_Const = new Довідники.Контрагенти_Pointer(fieldValue["col_a5"]);
                m_ОсновнаКаса_Const = new Довідники.Каси_Pointer(fieldValue["col_a6"]);
                m_ОсновнаОдиницяПакування_Const = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_a7"]);
                m_ОсновнийПідрозділ_Const = new Довідники.СтруктураПідприємства_Pointer(fieldValue["col_g4"]);
                m_ОсновнийБанківськийРахунок_Const = new Довідники.БанківськіРахункиОрганізацій_Pointer(fieldValue["col_g5"]);
                m_ОсновнийВидЦіни_Const = new Довідники.ВидиЦін_Pointer(fieldValue["col_g9"]);
                
            }
			
        }
        
        
        static Довідники.Організації_Pointer m_ОсновнаОрганізація_Const = new Довідники.Організації_Pointer();
        public static Довідники.Організації_Pointer ОсновнаОрганізація_Const
        {
            get 
            {
                return m_ОсновнаОрганізація_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОсновнаОрганізація_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_a1", m_ОсновнаОрганізація_Const.UnigueID.UGuid);
            }
        }
        
        static Довідники.Склади_Pointer m_ОснонийСклад_Const = new Довідники.Склади_Pointer();
        public static Довідники.Склади_Pointer ОснонийСклад_Const
        {
            get 
            {
                return m_ОснонийСклад_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОснонийСклад_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_a2", m_ОснонийСклад_Const.UnigueID.UGuid);
            }
        }
        
        static Довідники.Валюти_Pointer m_ОсновнаВалюта_Const = new Довідники.Валюти_Pointer();
        public static Довідники.Валюти_Pointer ОсновнаВалюта_Const
        {
            get 
            {
                return m_ОсновнаВалюта_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОсновнаВалюта_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_a3", m_ОсновнаВалюта_Const.UnigueID.UGuid);
            }
        }
        
        static Довідники.Контрагенти_Pointer m_ОсновнийПостачальник_Const = new Довідники.Контрагенти_Pointer();
        public static Довідники.Контрагенти_Pointer ОсновнийПостачальник_Const
        {
            get 
            {
                return m_ОсновнийПостачальник_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОсновнийПостачальник_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_a4", m_ОсновнийПостачальник_Const.UnigueID.UGuid);
            }
        }
        
        static Довідники.Контрагенти_Pointer m_ОсновнийПокупець_Const = new Довідники.Контрагенти_Pointer();
        public static Довідники.Контрагенти_Pointer ОсновнийПокупець_Const
        {
            get 
            {
                return m_ОсновнийПокупець_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОсновнийПокупець_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_a5", m_ОсновнийПокупець_Const.UnigueID.UGuid);
            }
        }
        
        static Довідники.Каси_Pointer m_ОсновнаКаса_Const = new Довідники.Каси_Pointer();
        public static Довідники.Каси_Pointer ОсновнаКаса_Const
        {
            get 
            {
                return m_ОсновнаКаса_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОсновнаКаса_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_a6", m_ОсновнаКаса_Const.UnigueID.UGuid);
            }
        }
        
        static Довідники.ПакуванняОдиниціВиміру_Pointer m_ОсновнаОдиницяПакування_Const = new Довідники.ПакуванняОдиниціВиміру_Pointer();
        public static Довідники.ПакуванняОдиниціВиміру_Pointer ОсновнаОдиницяПакування_Const
        {
            get 
            {
                return m_ОсновнаОдиницяПакування_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОсновнаОдиницяПакування_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_a7", m_ОсновнаОдиницяПакування_Const.UnigueID.UGuid);
            }
        }
        
        static Довідники.СтруктураПідприємства_Pointer m_ОсновнийПідрозділ_Const = new Довідники.СтруктураПідприємства_Pointer();
        public static Довідники.СтруктураПідприємства_Pointer ОсновнийПідрозділ_Const
        {
            get 
            {
                return m_ОсновнийПідрозділ_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОсновнийПідрозділ_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_g4", m_ОсновнийПідрозділ_Const.UnigueID.UGuid);
            }
        }
        
        static Довідники.БанківськіРахункиОрганізацій_Pointer m_ОсновнийБанківськийРахунок_Const = new Довідники.БанківськіРахункиОрганізацій_Pointer();
        public static Довідники.БанківськіРахункиОрганізацій_Pointer ОсновнийБанківськийРахунок_Const
        {
            get 
            {
                return m_ОсновнийБанківськийРахунок_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОсновнийБанківськийРахунок_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_g5", m_ОсновнийБанківськийРахунок_Const.UnigueID.UGuid);
            }
        }
        
        static Довідники.ВидиЦін_Pointer m_ОсновнийВидЦіни_Const = new Довідники.ВидиЦін_Pointer();
        public static Довідники.ВидиЦін_Pointer ОсновнийВидЦіни_Const
        {
            get 
            {
                return m_ОсновнийВидЦіни_Const.GetNewDirectoryPointer();
            }
            set
            {
                m_ОсновнийВидЦіни_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_g9", m_ОсновнийВидЦіни_Const.UnigueID.UGuid);
            }
        }
             
    }
    #endregion
    
	#region CONSTANTS BLOCK "Системні"
    public static class Системні
    {
        public static void ReadAll()
        {
            
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            bool IsSelect = Config.Kernel.DataBase.SelectAllConstants("tab_constants",
                 new string[] { "col_a8", "col_a9", "col_g6", "col_g7", "col_h8" }, fieldValue);
            
            if (IsSelect)
            {
                m_ЖурналРеєстрації_Const = fieldValue["col_a8"].ToString();
                m_ФоновіЗадачі_Const = fieldValue["col_a9"].ToString();
                m_ЗупинитиФоновіЗадачі_Const = (fieldValue["col_g6"] != DBNull.Value) ? bool.Parse(fieldValue["col_g6"].ToString()) : false;
                m_ЗаблокованіОбєкти_Const = fieldValue["col_g7"].ToString();
                m_ПовідомленняТаПомилки_Const = fieldValue["col_h8"].ToString();
                
            }
			
        }
        
        
        static string m_ЖурналРеєстрації_Const = "";
        public static string ЖурналРеєстрації_Const
        {
            get 
            {
                return m_ЖурналРеєстрації_Const;
            }
            set
            {
                m_ЖурналРеєстрації_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_a8", m_ЖурналРеєстрації_Const);
            }
        }
        
        static string m_ФоновіЗадачі_Const = "";
        public static string ФоновіЗадачі_Const
        {
            get 
            {
                return m_ФоновіЗадачі_Const;
            }
            set
            {
                m_ФоновіЗадачі_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_a9", m_ФоновіЗадачі_Const);
            }
        }
        
        static bool m_ЗупинитиФоновіЗадачі_Const = false;
        public static bool ЗупинитиФоновіЗадачі_Const
        {
            get 
            {
                return m_ЗупинитиФоновіЗадачі_Const;
            }
            set
            {
                m_ЗупинитиФоновіЗадачі_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_g6", m_ЗупинитиФоновіЗадачі_Const);
            }
        }
        
        static string m_ЗаблокованіОбєкти_Const = "";
        public static string ЗаблокованіОбєкти_Const
        {
            get 
            {
                return m_ЗаблокованіОбєкти_Const;
            }
            set
            {
                m_ЗаблокованіОбєкти_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_g7", m_ЗаблокованіОбєкти_Const);
            }
        }
        
        static string m_ПовідомленняТаПомилки_Const = "";
        public static string ПовідомленняТаПомилки_Const
        {
            get 
            {
                return m_ПовідомленняТаПомилки_Const;
            }
            set
            {
                m_ПовідомленняТаПомилки_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_h8", m_ПовідомленняТаПомилки_Const);
            }
        }
        
        public class ЖурналРеєстрації_Журнал_TablePart : ConstantsTablePart
        {
            public ЖурналРеєстрації_Журнал_TablePart() : base(Config.Kernel, "tab_a69",
                 new string[] { "col_a7", "col_a8", "col_a1", "col_a2" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a69";
            
            public const string Дата = "col_a7";
            public const string Коментар = "col_a8";
            public const string Обєкт = "col_a1";
            public const string Користувач = "col_a2";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Дата = (fieldValue["col_a7"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a7"].ToString()) : DateTime.MinValue;
                    record.Коментар = fieldValue["col_a8"].ToString();
                    record.Обєкт = fieldValue["col_a1"].ToString();
                    record.Користувач = fieldValue["col_a2"].ToString();
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a7", record.Дата);
                    fieldValue.Add("col_a8", record.Коментар);
                    fieldValue.Add("col_a1", record.Обєкт);
                    fieldValue.Add("col_a2", record.Користувач);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Дата = DateTime.MinValue;
                    Коментар = "";
                    Обєкт = "";
                    Користувач = "";
                    
                }
                public DateTime Дата { get; set; }
                public string Коментар { get; set; }
                public string Обєкт { get; set; }
                public string Користувач { get; set; }
                
            }            
        }
          
        public class ФоновіЗадачі_Задачі_TablePart : ConstantsTablePart
        {
            public ФоновіЗадачі_Задачі_TablePart() : base(Config.Kernel, "tab_a67",
                 new string[] { "col_a2", "col_a1", "col_a5", "col_a3", "col_a4", "col_a6" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a67";
            
            public const string Назва = "col_a2";
            public const string Дата = "col_a1";
            public const string Ключ = "col_a5";
            public const string Виконано = "col_a3";
            public const string Заблоковано = "col_a4";
            public const string Результат = "col_a6";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Назва = fieldValue["col_a2"].ToString();
                    record.Дата = (fieldValue["col_a1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a1"].ToString()) : DateTime.MinValue;
                    record.Ключ = fieldValue["col_a5"].ToString();
                    record.Виконано = (fieldValue["col_a3"] != DBNull.Value) ? bool.Parse(fieldValue["col_a3"].ToString()) : false;
                    record.Заблоковано = (fieldValue["col_a4"] != DBNull.Value) ? bool.Parse(fieldValue["col_a4"].ToString()) : false;
                    record.Результат = fieldValue["col_a6"].ToString();
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a2", record.Назва);
                    fieldValue.Add("col_a1", record.Дата);
                    fieldValue.Add("col_a5", record.Ключ);
                    fieldValue.Add("col_a3", record.Виконано);
                    fieldValue.Add("col_a4", record.Заблоковано);
                    fieldValue.Add("col_a6", record.Результат);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Назва = "";
                    Дата = DateTime.MinValue;
                    Ключ = "";
                    Виконано = false;
                    Заблоковано = false;
                    Результат = "";
                    
                }
                public string Назва { get; set; }
                public DateTime Дата { get; set; }
                public string Ключ { get; set; }
                public bool Виконано { get; set; }
                public bool Заблоковано { get; set; }
                public string Результат { get; set; }
                
            }            
        }
          
        public class ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart : ConstantsTablePart
        {
            public ФоновіЗадачі_ОбчисленняВіртуальнихЗалишків_TablePart() : base(Config.Kernel, "tab_a77",
                 new string[] { "col_a2", "col_a9", "col_a1", "col_a3", "col_a8", "col_a5", "col_a6", "col_a7" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a77";
            
            public const string Дата = "col_a2";
            public const string Документ = "col_a9";
            public const string ТипДокументу = "col_a1";
            public const string ПеріодОбчислення = "col_a3";
            public const string ТипРухуПоРегістру = "col_a8";
            public const string Заблоковано = "col_a5";
            public const string Виконано = "col_a6";
            public const string Користувач = "col_a7";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Дата = (fieldValue["col_a2"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a2"].ToString()) : DateTime.MinValue;
                    record.Документ = fieldValue["col_a9"].ToString();
                    record.ТипДокументу = fieldValue["col_a1"].ToString();
                    record.ПеріодОбчислення = (fieldValue["col_a3"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a3"].ToString()) : DateTime.MinValue;
                    record.ТипРухуПоРегістру = fieldValue["col_a8"].ToString();
                    record.Заблоковано = (fieldValue["col_a5"] != DBNull.Value) ? bool.Parse(fieldValue["col_a5"].ToString()) : false;
                    record.Виконано = (fieldValue["col_a6"] != DBNull.Value) ? bool.Parse(fieldValue["col_a6"].ToString()) : false;
                    record.Користувач = fieldValue["col_a7"].ToString();
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a2", record.Дата);
                    fieldValue.Add("col_a9", record.Документ);
                    fieldValue.Add("col_a1", record.ТипДокументу);
                    fieldValue.Add("col_a3", record.ПеріодОбчислення);
                    fieldValue.Add("col_a8", record.ТипРухуПоРегістру);
                    fieldValue.Add("col_a5", record.Заблоковано);
                    fieldValue.Add("col_a6", record.Виконано);
                    fieldValue.Add("col_a7", record.Користувач);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Дата = DateTime.MinValue;
                    Документ = "";
                    ТипДокументу = "";
                    ПеріодОбчислення = DateTime.MinValue;
                    ТипРухуПоРегістру = "";
                    Заблоковано = false;
                    Виконано = false;
                    Користувач = "";
                    
                }
                public DateTime Дата { get; set; }
                public string Документ { get; set; }
                public string ТипДокументу { get; set; }
                public DateTime ПеріодОбчислення { get; set; }
                public string ТипРухуПоРегістру { get; set; }
                public bool Заблоковано { get; set; }
                public bool Виконано { get; set; }
                public string Користувач { get; set; }
                
            }            
        }
          
        public class ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart : ConstantsTablePart
        {
            public ФоновіЗадачі_АктуальністьВіртуальнихЗалишків_TablePart() : base(Config.Kernel, "tab_a96",
                 new string[] { "col_a1", "col_a2", "col_a3", "col_a4" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a96";
            
            public const string Регістр = "col_a1";
            public const string Місяць = "col_a2";
            public const string Актуально = "col_a3";
            public const string ДатаОстанньогоОбчислення = "col_a4";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Регістр = fieldValue["col_a1"].ToString();
                    record.Місяць = (fieldValue["col_a2"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a2"].ToString()) : DateTime.MinValue;
                    record.Актуально = (fieldValue["col_a3"] != DBNull.Value) ? bool.Parse(fieldValue["col_a3"].ToString()) : false;
                    record.ДатаОстанньогоОбчислення = (fieldValue["col_a4"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a4"].ToString()) : DateTime.MinValue;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a1", record.Регістр);
                    fieldValue.Add("col_a2", record.Місяць);
                    fieldValue.Add("col_a3", record.Актуально);
                    fieldValue.Add("col_a4", record.ДатаОстанньогоОбчислення);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Регістр = "";
                    Місяць = DateTime.MinValue;
                    Актуально = false;
                    ДатаОстанньогоОбчислення = DateTime.MinValue;
                    
                }
                public string Регістр { get; set; }
                public DateTime Місяць { get; set; }
                public bool Актуально { get; set; }
                public DateTime ДатаОстанньогоОбчислення { get; set; }
                
            }            
        }
          
        public class ЗаблокованіОбєкти_Документи_TablePart : ConstantsTablePart
        {
            public ЗаблокованіОбєкти_Документи_TablePart() : base(Config.Kernel, "tab_b03",
                 new string[] { "col_a1", "col_a2", "col_a3", "col_a4" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_b03";
            
            public const string Обєкт = "col_a1";
            public const string Користувач = "col_a2";
            public const string ДатаБлокування = "col_a3";
            public const string ДатаПідтвердженняБлокування = "col_a4";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Обєкт = (fieldValue["col_a1"] != DBNull.Value) ? Guid.Parse(fieldValue["col_a1"].ToString()) : Guid.Empty;
                    record.Користувач = fieldValue["col_a2"].ToString();
                    record.ДатаБлокування = (fieldValue["col_a3"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a3"].ToString()) : DateTime.MinValue;
                    record.ДатаПідтвердженняБлокування = (fieldValue["col_a4"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a4"].ToString()) : DateTime.MinValue;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a1", record.Обєкт);
                    fieldValue.Add("col_a2", record.Користувач);
                    fieldValue.Add("col_a3", record.ДатаБлокування);
                    fieldValue.Add("col_a4", record.ДатаПідтвердженняБлокування);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Обєкт = new Guid();
                    Користувач = "";
                    ДатаБлокування = DateTime.MinValue;
                    ДатаПідтвердженняБлокування = DateTime.MinValue;
                    
                }
                public Guid Обєкт { get; set; }
                public string Користувач { get; set; }
                public DateTime ДатаБлокування { get; set; }
                public DateTime ДатаПідтвердженняБлокування { get; set; }
                
            }            
        }
          
        public class ПовідомленняТаПомилки_Помилки_TablePart : ConstantsTablePart
        {
            public ПовідомленняТаПомилки_Помилки_TablePart() : base(Config.Kernel, "tab_b21",
                 new string[] { "col_a2", "col_a5", "col_a1", "col_a6", "col_a4", "col_a3" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_b21";
            
            public const string Дата = "col_a2";
            public const string НазваПроцесу = "col_a5";
            public const string Обєкт = "col_a1";
            public const string ТипОбєкту = "col_a6";
            public const string НазваОбєкту = "col_a4";
            public const string Повідомлення = "col_a3";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Дата = (fieldValue["col_a2"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a2"].ToString()) : DateTime.MinValue;
                    record.НазваПроцесу = fieldValue["col_a5"].ToString();
                    record.Обєкт = (fieldValue["col_a1"] != DBNull.Value) ? Guid.Parse(fieldValue["col_a1"].ToString()) : Guid.Empty;
                    record.ТипОбєкту = fieldValue["col_a6"].ToString();
                    record.НазваОбєкту = fieldValue["col_a4"].ToString();
                    record.Повідомлення = fieldValue["col_a3"].ToString();
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a2", record.Дата);
                    fieldValue.Add("col_a5", record.НазваПроцесу);
                    fieldValue.Add("col_a1", record.Обєкт);
                    fieldValue.Add("col_a6", record.ТипОбєкту);
                    fieldValue.Add("col_a4", record.НазваОбєкту);
                    fieldValue.Add("col_a3", record.Повідомлення);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Дата = DateTime.MinValue;
                    НазваПроцесу = "";
                    Обєкт = new Guid();
                    ТипОбєкту = "";
                    НазваОбєкту = "";
                    Повідомлення = "";
                    
                }
                public DateTime Дата { get; set; }
                public string НазваПроцесу { get; set; }
                public Guid Обєкт { get; set; }
                public string ТипОбєкту { get; set; }
                public string НазваОбєкту { get; set; }
                public string Повідомлення { get; set; }
                
            }            
        }
               
    }
    #endregion
    
	#region CONSTANTS BLOCK "ВіртуальніТаблиціРегістрів"
    public static class ВіртуальніТаблиціРегістрів
    {
        public static void ReadAll()
        {
            
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            bool IsSelect = Config.Kernel.DataBase.SelectAllConstants("tab_constants",
                 new string[] { "col_b1", "col_b3", "col_b4", "col_b2", "col_b5", "col_b6", "col_c3", "col_g3", "col_g8" }, fieldValue);
            
            if (IsSelect)
            {
                m_ЗамовленняКлієнтів_Const = fieldValue["col_b1"].ToString();
                m_ТовариНаСкладах_Const = fieldValue["col_b3"].ToString();
                m_РозрахункиЗКлієнтами_Const = fieldValue["col_b4"].ToString();
                m_РозрахункиЗПостачальниками_Const = fieldValue["col_b2"].ToString();
                m_ЗамовленняПостачальникам_Const = fieldValue["col_b5"].ToString();
                m_ВільніЗалишки_Const = fieldValue["col_b6"].ToString();
                m_РухКоштів_Const = fieldValue["col_c3"].ToString();
                m_Закупівлі_Const = fieldValue["col_g3"].ToString();
                m_ПартіїТоварів_Const = fieldValue["col_g8"].ToString();
                
            }
			
        }
        
        
        static string m_ЗамовленняКлієнтів_Const = "";
        public static string ЗамовленняКлієнтів_Const
        {
            get 
            {
                return m_ЗамовленняКлієнтів_Const;
            }
            set
            {
                m_ЗамовленняКлієнтів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_b1", m_ЗамовленняКлієнтів_Const);
            }
        }
        
        static string m_ТовариНаСкладах_Const = "";
        public static string ТовариНаСкладах_Const
        {
            get 
            {
                return m_ТовариНаСкладах_Const;
            }
            set
            {
                m_ТовариНаСкладах_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_b3", m_ТовариНаСкладах_Const);
            }
        }
        
        static string m_РозрахункиЗКлієнтами_Const = "";
        public static string РозрахункиЗКлієнтами_Const
        {
            get 
            {
                return m_РозрахункиЗКлієнтами_Const;
            }
            set
            {
                m_РозрахункиЗКлієнтами_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_b4", m_РозрахункиЗКлієнтами_Const);
            }
        }
        
        static string m_РозрахункиЗПостачальниками_Const = "";
        public static string РозрахункиЗПостачальниками_Const
        {
            get 
            {
                return m_РозрахункиЗПостачальниками_Const;
            }
            set
            {
                m_РозрахункиЗПостачальниками_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_b2", m_РозрахункиЗПостачальниками_Const);
            }
        }
        
        static string m_ЗамовленняПостачальникам_Const = "";
        public static string ЗамовленняПостачальникам_Const
        {
            get 
            {
                return m_ЗамовленняПостачальникам_Const;
            }
            set
            {
                m_ЗамовленняПостачальникам_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_b5", m_ЗамовленняПостачальникам_Const);
            }
        }
        
        static string m_ВільніЗалишки_Const = "";
        public static string ВільніЗалишки_Const
        {
            get 
            {
                return m_ВільніЗалишки_Const;
            }
            set
            {
                m_ВільніЗалишки_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_b6", m_ВільніЗалишки_Const);
            }
        }
        
        static string m_РухКоштів_Const = "";
        public static string РухКоштів_Const
        {
            get 
            {
                return m_РухКоштів_Const;
            }
            set
            {
                m_РухКоштів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_c3", m_РухКоштів_Const);
            }
        }
        
        static string m_Закупівлі_Const = "";
        public static string Закупівлі_Const
        {
            get 
            {
                return m_Закупівлі_Const;
            }
            set
            {
                m_Закупівлі_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_g3", m_Закупівлі_Const);
            }
        }
        
        static string m_ПартіїТоварів_Const = "";
        public static string ПартіїТоварів_Const
        {
            get 
            {
                return m_ПартіїТоварів_Const;
            }
            set
            {
                m_ПартіїТоварів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_g8", m_ПартіїТоварів_Const);
            }
        }
        
        public class ЗамовленняКлієнтів_Місяць_TablePart : ConstantsTablePart
        {
            public ЗамовленняКлієнтів_Місяць_TablePart() : base(Config.Kernel, "tab_a68",
                 new string[] { "col_a6", "col_a2", "col_a3", "col_a4", "col_a5", "col_a7" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a68";
            
            public const string Період = "col_a6";
            public const string Номенклатура = "col_a2";
            public const string ХарактеристикаНоменклатури = "col_a3";
            public const string Склад = "col_a4";
            public const string Замовлено = "col_a5";
            public const string Сума = "col_a7";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a6"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a6"].ToString()) : DateTime.MinValue;
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                    record.Замовлено = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    record.Сума = (fieldValue["col_a7"] != DBNull.Value) ? (decimal)fieldValue["col_a7"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a6", record.Період);
                    fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.Замовлено);
                    fieldValue.Add("col_a7", record.Сума);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Замовлено = 0;
                    Сума = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public decimal Замовлено { get; set; }
                public decimal Сума { get; set; }
                
            }            
        }
          
        public class ЗамовленняКлієнтів_День_TablePart : ConstantsTablePart
        {
            public ЗамовленняКлієнтів_День_TablePart() : base(Config.Kernel, "tab_a70",
                 new string[] { "col_b1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a70";
            
            public const string Період = "col_b1";
            public const string Номенклатура = "col_a2";
            public const string ХарактеристикаНоменклатури = "col_a3";
            public const string Склад = "col_a4";
            public const string Замовлено = "col_a5";
            public const string Сума = "col_a6";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_b1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_b1"].ToString()) : DateTime.MinValue;
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                    record.Замовлено = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    record.Сума = (fieldValue["col_a6"] != DBNull.Value) ? (decimal)fieldValue["col_a6"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_b1", record.Період);
                    fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.Замовлено);
                    fieldValue.Add("col_a6", record.Сума);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Замовлено = 0;
                    Сума = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public decimal Замовлено { get; set; }
                public decimal Сума { get; set; }
                
            }            
        }
          
        public class ТовариНаСкладах_День_TablePart : ConstantsTablePart
        {
            public ТовариНаСкладах_День_TablePart() : base(Config.Kernel, "tab_a74",
                 new string[] { "col_a7", "col_a8", "col_a9", "col_b1", "col_a1", "col_b2", "col_b3" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a74";
            
            public const string Період = "col_a7";
            public const string Номенклатура = "col_a8";
            public const string ХарактеристикаНоменклатури = "col_a9";
            public const string Склад = "col_b1";
            public const string Серія = "col_a1";
            public const string ВНаявності = "col_b2";
            public const string ДоВідвантаження = "col_b3";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a7"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a7"].ToString()) : DateTime.MinValue;
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a8"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a9"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_b1"]);
                    record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a1"]);
                    record.ВНаявності = (fieldValue["col_b2"] != DBNull.Value) ? (decimal)fieldValue["col_b2"] : 0;
                    record.ДоВідвантаження = (fieldValue["col_b3"] != DBNull.Value) ? (decimal)fieldValue["col_b3"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a7", record.Період);
                    fieldValue.Add("col_a8", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a9", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_b1", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a1", record.Серія.UnigueID.UGuid);
                    fieldValue.Add("col_b2", record.ВНаявності);
                    fieldValue.Add("col_b3", record.ДоВідвантаження);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Серія = new Довідники.СеріїНоменклатури_Pointer();
                    ВНаявності = 0;
                    ДоВідвантаження = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
                public decimal ВНаявності { get; set; }
                public decimal ДоВідвантаження { get; set; }
                
            }            
        }
          
        public class ТовариНаСкладах_Місяць_TablePart : ConstantsTablePart
        {
            public ТовариНаСкладах_Місяць_TablePart() : base(Config.Kernel, "tab_a73",
                 new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a7", "col_a5", "col_a6" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a73";
            
            public const string Період = "col_a1";
            public const string Номенклатура = "col_a2";
            public const string ХарактеристикаНоменклатури = "col_a3";
            public const string Склад = "col_a4";
            public const string Серія = "col_a7";
            public const string ВНаявності = "col_a5";
            public const string ДоВідвантаження = "col_a6";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a1"].ToString()) : DateTime.MinValue;
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                    record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a7"]);
                    record.ВНаявності = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    record.ДоВідвантаження = (fieldValue["col_a6"] != DBNull.Value) ? (decimal)fieldValue["col_a6"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a1", record.Період);
                    fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a7", record.Серія.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.ВНаявності);
                    fieldValue.Add("col_a6", record.ДоВідвантаження);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Серія = new Довідники.СеріїНоменклатури_Pointer();
                    ВНаявності = 0;
                    ДоВідвантаження = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
                public decimal ВНаявності { get; set; }
                public decimal ДоВідвантаження { get; set; }
                
            }            
        }
          
        public class ТовариНаСкладах_Підсумок_TablePart : ConstantsTablePart
        {
            public ТовариНаСкладах_Підсумок_TablePart() : base(Config.Kernel, "tab_b09",
                 new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_b09";
            
            public const string Номенклатура = "col_a1";
            public const string ХарактеристикаНоменклатури = "col_a2";
            public const string Серія = "col_a3";
            public const string Склад = "col_a4";
            public const string ВНаявності = "col_a5";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a1"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a2"]);
                    record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a3"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                    record.ВНаявності = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a1", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.Серія.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.ВНаявності);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Серія = new Довідники.СеріїНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    ВНаявності = 0;
                    
                }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public decimal ВНаявності { get; set; }
                
            }            
        }
          
        public class РозрахункиЗКлієнтами_Місяць_TablePart : ConstantsTablePart
        {
            public РозрахункиЗКлієнтами_Місяць_TablePart() : base(Config.Kernel, "tab_a75",
                 new string[] { "col_a6", "col_a1", "col_a2", "col_a3" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a75";
            
            public const string Період = "col_a6";
            public const string Валюта = "col_a1";
            public const string Контрагент = "col_a2";
            public const string Сума = "col_a3";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a6"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a6"].ToString()) : DateTime.MinValue;
                    record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a1"]);
                    record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_a2"]);
                    record.Сума = (fieldValue["col_a3"] != DBNull.Value) ? (decimal)fieldValue["col_a3"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a6", record.Період);
                    fieldValue.Add("col_a1", record.Валюта.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.Контрагент.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.Сума);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Валюта = new Довідники.Валюти_Pointer();
                    Контрагент = new Довідники.Контрагенти_Pointer();
                    Сума = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Валюти_Pointer Валюта { get; set; }
                public Довідники.Контрагенти_Pointer Контрагент { get; set; }
                public decimal Сума { get; set; }
                
            }            
        }
          
        public class РозрахункиЗКлієнтами_День_TablePart : ConstantsTablePart
        {
            public РозрахункиЗКлієнтами_День_TablePart() : base(Config.Kernel, "tab_a76",
                 new string[] { "col_b1", "col_a1", "col_a2", "col_a6" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a76";
            
            public const string Період = "col_b1";
            public const string Валюта = "col_a1";
            public const string Контрагент = "col_a2";
            public const string Сума = "col_a6";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_b1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_b1"].ToString()) : DateTime.MinValue;
                    record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a1"]);
                    record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_a2"]);
                    record.Сума = (fieldValue["col_a6"] != DBNull.Value) ? (decimal)fieldValue["col_a6"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_b1", record.Період);
                    fieldValue.Add("col_a1", record.Валюта.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.Контрагент.UnigueID.UGuid);
                    fieldValue.Add("col_a6", record.Сума);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Валюта = new Довідники.Валюти_Pointer();
                    Контрагент = new Довідники.Контрагенти_Pointer();
                    Сума = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Валюти_Pointer Валюта { get; set; }
                public Довідники.Контрагенти_Pointer Контрагент { get; set; }
                public decimal Сума { get; set; }
                
            }            
        }
          
        public class РозрахункиЗПостачальниками_Місяць_TablePart : ConstantsTablePart
        {
            public РозрахункиЗПостачальниками_Місяць_TablePart() : base(Config.Kernel, "tab_a63",
                 new string[] { "col_a6", "col_a1", "col_a2", "col_a3" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a63";
            
            public const string Період = "col_a6";
            public const string Валюта = "col_a1";
            public const string Контрагент = "col_a2";
            public const string Сума = "col_a3";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a6"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a6"].ToString()) : DateTime.MinValue;
                    record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a1"]);
                    record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_a2"]);
                    record.Сума = (fieldValue["col_a3"] != DBNull.Value) ? (decimal)fieldValue["col_a3"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a6", record.Період);
                    fieldValue.Add("col_a1", record.Валюта.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.Контрагент.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.Сума);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Валюта = new Довідники.Валюти_Pointer();
                    Контрагент = new Довідники.Контрагенти_Pointer();
                    Сума = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Валюти_Pointer Валюта { get; set; }
                public Довідники.Контрагенти_Pointer Контрагент { get; set; }
                public decimal Сума { get; set; }
                
            }            
        }
          
        public class РозрахункиЗПостачальниками_День_TablePart : ConstantsTablePart
        {
            public РозрахункиЗПостачальниками_День_TablePart() : base(Config.Kernel, "tab_a64",
                 new string[] { "col_b1", "col_a1", "col_a2", "col_a6" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a64";
            
            public const string Період = "col_b1";
            public const string Валюта = "col_a1";
            public const string Контрагент = "col_a2";
            public const string Сума = "col_a6";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_b1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_b1"].ToString()) : DateTime.MinValue;
                    record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a1"]);
                    record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_a2"]);
                    record.Сума = (fieldValue["col_a6"] != DBNull.Value) ? (decimal)fieldValue["col_a6"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_b1", record.Період);
                    fieldValue.Add("col_a1", record.Валюта.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.Контрагент.UnigueID.UGuid);
                    fieldValue.Add("col_a6", record.Сума);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Валюта = new Довідники.Валюти_Pointer();
                    Контрагент = new Довідники.Контрагенти_Pointer();
                    Сума = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Валюти_Pointer Валюта { get; set; }
                public Довідники.Контрагенти_Pointer Контрагент { get; set; }
                public decimal Сума { get; set; }
                
            }            
        }
          
        public class ЗамовленняПостачальникам_Місяць_TablePart : ConstantsTablePart
        {
            public ЗамовленняПостачальникам_Місяць_TablePart() : base(Config.Kernel, "tab_a65",
                 new string[] { "col_a6", "col_a2", "col_a3", "col_a4", "col_a5" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a65";
            
            public const string Період = "col_a6";
            public const string Номенклатура = "col_a2";
            public const string ХарактеристикаНоменклатури = "col_a3";
            public const string Склад = "col_a4";
            public const string Замовлено = "col_a5";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a6"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a6"].ToString()) : DateTime.MinValue;
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                    record.Замовлено = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a6", record.Період);
                    fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.Замовлено);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Замовлено = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public decimal Замовлено { get; set; }
                
            }            
        }
          
        public class ЗамовленняПостачальникам_День_TablePart : ConstantsTablePart
        {
            public ЗамовленняПостачальникам_День_TablePart() : base(Config.Kernel, "tab_a66",
                 new string[] { "col_b1", "col_a2", "col_a3", "col_a4", "col_a5" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a66";
            
            public const string Період = "col_b1";
            public const string Номенклатура = "col_a2";
            public const string ХарактеристикаНоменклатури = "col_a3";
            public const string Склад = "col_a4";
            public const string Замовлено = "col_a5";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_b1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_b1"].ToString()) : DateTime.MinValue;
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                    record.Замовлено = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_b1", record.Період);
                    fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.Замовлено);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Замовлено = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public decimal Замовлено { get; set; }
                
            }            
        }
          
        public class ВільніЗалишки_Місяць_TablePart : ConstantsTablePart
        {
            public ВільніЗалишки_Місяць_TablePart() : base(Config.Kernel, "tab_a71",
                 new string[] { "col_a6", "col_a2", "col_a3", "col_a4", "col_a5", "col_a7", "col_a1" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a71";
            
            public const string Період = "col_a6";
            public const string Номенклатура = "col_a2";
            public const string ХарактеристикаНоменклатури = "col_a3";
            public const string Склад = "col_a4";
            public const string ВНаявності = "col_a5";
            public const string ВРезервіЗіСкладу = "col_a7";
            public const string ВРезервіПідЗамовлення = "col_a1";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a6"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a6"].ToString()) : DateTime.MinValue;
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                    record.ВНаявності = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    record.ВРезервіЗіСкладу = (fieldValue["col_a7"] != DBNull.Value) ? (decimal)fieldValue["col_a7"] : 0;
                    record.ВРезервіПідЗамовлення = (fieldValue["col_a1"] != DBNull.Value) ? (decimal)fieldValue["col_a1"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a6", record.Період);
                    fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.ВНаявності);
                    fieldValue.Add("col_a7", record.ВРезервіЗіСкладу);
                    fieldValue.Add("col_a1", record.ВРезервіПідЗамовлення);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    ВНаявності = 0;
                    ВРезервіЗіСкладу = 0;
                    ВРезервіПідЗамовлення = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public decimal ВНаявності { get; set; }
                public decimal ВРезервіЗіСкладу { get; set; }
                public decimal ВРезервіПідЗамовлення { get; set; }
                
            }            
        }
          
        public class ВільніЗалишки_День_TablePart : ConstantsTablePart
        {
            public ВільніЗалишки_День_TablePart() : base(Config.Kernel, "tab_a72",
                 new string[] { "col_b1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a1" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a72";
            
            public const string Період = "col_b1";
            public const string Номенклатура = "col_a2";
            public const string ХарактеристикаНоменклатури = "col_a3";
            public const string Склад = "col_a4";
            public const string ВНаявності = "col_a5";
            public const string ВРезервіЗіСкладу = "col_a6";
            public const string ВРезервіПідЗамовлення = "col_a1";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_b1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_b1"].ToString()) : DateTime.MinValue;
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                    record.ВНаявності = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    record.ВРезервіЗіСкладу = (fieldValue["col_a6"] != DBNull.Value) ? (decimal)fieldValue["col_a6"] : 0;
                    record.ВРезервіПідЗамовлення = (fieldValue["col_a1"] != DBNull.Value) ? (decimal)fieldValue["col_a1"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_b1", record.Період);
                    fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.ВНаявності);
                    fieldValue.Add("col_a6", record.ВРезервіЗіСкладу);
                    fieldValue.Add("col_a1", record.ВРезервіПідЗамовлення);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    ВНаявності = 0;
                    ВРезервіЗіСкладу = 0;
                    ВРезервіПідЗамовлення = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public decimal ВНаявності { get; set; }
                public decimal ВРезервіЗіСкладу { get; set; }
                public decimal ВРезервіПідЗамовлення { get; set; }
                
            }            
        }
          
        public class РухКоштів_Місяць_TablePart : ConstantsTablePart
        {
            public РухКоштів_Місяць_TablePart() : base(Config.Kernel, "tab_a98",
                 new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a98";
            
            public const string Період = "col_a1";
            public const string Організація = "col_a2";
            public const string Каса = "col_a3";
            public const string Валюта = "col_a4";
            public const string Сума = "col_a5";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a1"].ToString()) : DateTime.MinValue;
                    record.Організація = new Довідники.Організації_Pointer(fieldValue["col_a2"]);
                    record.Каса = new Довідники.Каси_Pointer(fieldValue["col_a3"]);
                    record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a4"]);
                    record.Сума = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a1", record.Період);
                    fieldValue.Add("col_a2", record.Організація.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.Каса.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Валюта.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.Сума);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Організація = new Довідники.Організації_Pointer();
                    Каса = new Довідники.Каси_Pointer();
                    Валюта = new Довідники.Валюти_Pointer();
                    Сума = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Організації_Pointer Організація { get; set; }
                public Довідники.Каси_Pointer Каса { get; set; }
                public Довідники.Валюти_Pointer Валюта { get; set; }
                public decimal Сума { get; set; }
                
            }            
        }
          
        public class РухКоштів_День_TablePart : ConstantsTablePart
        {
            public РухКоштів_День_TablePart() : base(Config.Kernel, "tab_a99",
                 new string[] { "col_a6", "col_a7", "col_a8", "col_a9", "col_b1" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a99";
            
            public const string Період = "col_a6";
            public const string Організація = "col_a7";
            public const string Каса = "col_a8";
            public const string Валюта = "col_a9";
            public const string Сума = "col_b1";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a6"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a6"].ToString()) : DateTime.MinValue;
                    record.Організація = new Довідники.Організації_Pointer(fieldValue["col_a7"]);
                    record.Каса = new Довідники.Каси_Pointer(fieldValue["col_a8"]);
                    record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a9"]);
                    record.Сума = (fieldValue["col_b1"] != DBNull.Value) ? (decimal)fieldValue["col_b1"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a6", record.Період);
                    fieldValue.Add("col_a7", record.Організація.UnigueID.UGuid);
                    fieldValue.Add("col_a8", record.Каса.UnigueID.UGuid);
                    fieldValue.Add("col_a9", record.Валюта.UnigueID.UGuid);
                    fieldValue.Add("col_b1", record.Сума);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Організація = new Довідники.Організації_Pointer();
                    Каса = new Довідники.Каси_Pointer();
                    Валюта = new Довідники.Валюти_Pointer();
                    Сума = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Організації_Pointer Організація { get; set; }
                public Довідники.Каси_Pointer Каса { get; set; }
                public Довідники.Валюти_Pointer Валюта { get; set; }
                public decimal Сума { get; set; }
                
            }            
        }
          
        public class Закупівлі_Місяць_TablePart : ConstantsTablePart
        {
            public Закупівлі_Місяць_TablePart() : base(Config.Kernel, "tab_a97",
                 new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_a97";
            
            public const string Період = "col_a1";
            public const string Організація = "col_a2";
            public const string Склад = "col_a3";
            public const string Контрагент = "col_a4";
            public const string Договір = "col_a5";
            public const string Кількість = "col_a6";
            public const string Сума = "col_a7";
            public const string Вартість = "col_a8";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a1"].ToString()) : DateTime.MinValue;
                    record.Організація = new Довідники.Організації_Pointer(fieldValue["col_a2"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a3"]);
                    record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_a4"]);
                    record.Договір = new Довідники.ДоговориКонтрагентів_Pointer(fieldValue["col_a5"]);
                    record.Кількість = (fieldValue["col_a6"] != DBNull.Value) ? (decimal)fieldValue["col_a6"] : 0;
                    record.Сума = (fieldValue["col_a7"] != DBNull.Value) ? (decimal)fieldValue["col_a7"] : 0;
                    record.Вартість = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a1", record.Період);
                    fieldValue.Add("col_a2", record.Організація.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Контрагент.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.Договір.UnigueID.UGuid);
                    fieldValue.Add("col_a6", record.Кількість);
                    fieldValue.Add("col_a7", record.Сума);
                    fieldValue.Add("col_a8", record.Вартість);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Організація = new Довідники.Організації_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Контрагент = new Довідники.Контрагенти_Pointer();
                    Договір = new Довідники.ДоговориКонтрагентів_Pointer();
                    Кількість = 0;
                    Сума = 0;
                    Вартість = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Організації_Pointer Організація { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public Довідники.Контрагенти_Pointer Контрагент { get; set; }
                public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
                public decimal Кількість { get; set; }
                public decimal Сума { get; set; }
                public decimal Вартість { get; set; }
                
            }            
        }
          
        public class Закупівлі_День_TablePart : ConstantsTablePart
        {
            public Закупівлі_День_TablePart() : base(Config.Kernel, "tab_b01",
                 new string[] { "col_a6", "col_a7", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a8" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_b01";
            
            public const string Період = "col_a6";
            public const string Організація = "col_a7";
            public const string Склад = "col_a1";
            public const string Контрагент = "col_a2";
            public const string Договір = "col_a3";
            public const string Кількість = "col_a4";
            public const string Сума = "col_a5";
            public const string Вартість = "col_a8";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a6"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a6"].ToString()) : DateTime.MinValue;
                    record.Організація = new Довідники.Організації_Pointer(fieldValue["col_a7"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a1"]);
                    record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_a2"]);
                    record.Договір = new Довідники.ДоговориКонтрагентів_Pointer(fieldValue["col_a3"]);
                    record.Кількість = (fieldValue["col_a4"] != DBNull.Value) ? (decimal)fieldValue["col_a4"] : 0;
                    record.Сума = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                    record.Вартість = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a6", record.Період);
                    fieldValue.Add("col_a7", record.Організація.UnigueID.UGuid);
                    fieldValue.Add("col_a1", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.Контрагент.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.Договір.UnigueID.UGuid);
                    fieldValue.Add("col_a4", record.Кількість);
                    fieldValue.Add("col_a5", record.Сума);
                    fieldValue.Add("col_a8", record.Вартість);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Організація = new Довідники.Організації_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Контрагент = new Довідники.Контрагенти_Pointer();
                    Договір = new Довідники.ДоговориКонтрагентів_Pointer();
                    Кількість = 0;
                    Сума = 0;
                    Вартість = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Організації_Pointer Організація { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public Довідники.Контрагенти_Pointer Контрагент { get; set; }
                public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
                public decimal Кількість { get; set; }
                public decimal Сума { get; set; }
                public decimal Вартість { get; set; }
                
            }            
        }
          
        public class ПартіїТоварів_День_TablePart : ConstantsTablePart
        {
            public ПартіїТоварів_День_TablePart() : base(Config.Kernel, "tab_b04",
                 new string[] { "col_a7", "col_a1", "col_b1", "col_a5", "col_a6", "col_a8", "col_a2", "col_a3", "col_a4" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_b04";
            
            public const string Період = "col_a7";
            public const string Організація = "col_a1";
            public const string ПартіяТоварівКомпозит = "col_b1";
            public const string Номенклатура = "col_a5";
            public const string ХарактеристикаНоменклатури = "col_a6";
            public const string Серія = "col_a8";
            public const string Склад = "col_a2";
            public const string Кількість = "col_a3";
            public const string Собівартість = "col_a4";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a7"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a7"].ToString()) : DateTime.MinValue;
                    record.Організація = new Довідники.Організації_Pointer(fieldValue["col_a1"]);
                    record.ПартіяТоварівКомпозит = new Довідники.ПартіяТоварівКомпозит_Pointer(fieldValue["col_b1"]);
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a5"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a6"]);
                    record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a8"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a2"]);
                    record.Кількість = (fieldValue["col_a3"] != DBNull.Value) ? (decimal)fieldValue["col_a3"] : 0;
                    record.Собівартість = (fieldValue["col_a4"] != DBNull.Value) ? (decimal)fieldValue["col_a4"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a7", record.Період);
                    fieldValue.Add("col_a1", record.Організація.UnigueID.UGuid);
                    fieldValue.Add("col_b1", record.ПартіяТоварівКомпозит.UnigueID.UGuid);
                    fieldValue.Add("col_a5", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_a6", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a8", record.Серія.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.Кількість);
                    fieldValue.Add("col_a4", record.Собівартість);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Організація = new Довідники.Організації_Pointer();
                    ПартіяТоварівКомпозит = new Довідники.ПартіяТоварівКомпозит_Pointer();
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Серія = new Довідники.СеріїНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Кількість = 0;
                    Собівартість = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Організації_Pointer Організація { get; set; }
                public Довідники.ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public decimal Кількість { get; set; }
                public decimal Собівартість { get; set; }
                
            }            
        }
          
        public class ПартіїТоварів_Місяць_TablePart : ConstantsTablePart
        {
            public ПартіїТоварів_Місяць_TablePart() : base(Config.Kernel, "tab_b05",
                 new string[] { "col_a8", "col_a9", "col_a3", "col_b4", "col_b5", "col_a1", "col_a2", "col_b2", "col_b3" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_b05";
            
            public const string Період = "col_a8";
            public const string Організація = "col_a9";
            public const string ПартіяТоварівКомпозит = "col_a3";
            public const string Номенклатура = "col_b4";
            public const string ХарактеристикаНоменклатури = "col_b5";
            public const string Серія = "col_a1";
            public const string Склад = "col_a2";
            public const string Кількість = "col_b2";
            public const string Собівартість = "col_b3";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Період = (fieldValue["col_a8"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a8"].ToString()) : DateTime.MinValue;
                    record.Організація = new Довідники.Організації_Pointer(fieldValue["col_a9"]);
                    record.ПартіяТоварівКомпозит = new Довідники.ПартіяТоварівКомпозит_Pointer(fieldValue["col_a3"]);
                    record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_b4"]);
                    record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_b5"]);
                    record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a1"]);
                    record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a2"]);
                    record.Кількість = (fieldValue["col_b2"] != DBNull.Value) ? (decimal)fieldValue["col_b2"] : 0;
                    record.Собівартість = (fieldValue["col_b3"] != DBNull.Value) ? (decimal)fieldValue["col_b3"] : 0;
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a8", record.Період);
                    fieldValue.Add("col_a9", record.Організація.UnigueID.UGuid);
                    fieldValue.Add("col_a3", record.ПартіяТоварівКомпозит.UnigueID.UGuid);
                    fieldValue.Add("col_b4", record.Номенклатура.UnigueID.UGuid);
                    fieldValue.Add("col_b5", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                    fieldValue.Add("col_a1", record.Серія.UnigueID.UGuid);
                    fieldValue.Add("col_a2", record.Склад.UnigueID.UGuid);
                    fieldValue.Add("col_b2", record.Кількість);
                    fieldValue.Add("col_b3", record.Собівартість);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Період = DateTime.MinValue;
                    Організація = new Довідники.Організації_Pointer();
                    ПартіяТоварівКомпозит = new Довідники.ПартіяТоварівКомпозит_Pointer();
                    Номенклатура = new Довідники.Номенклатура_Pointer();
                    ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                    Серія = new Довідники.СеріїНоменклатури_Pointer();
                    Склад = new Довідники.Склади_Pointer();
                    Кількість = 0;
                    Собівартість = 0;
                    
                }
                public DateTime Період { get; set; }
                public Довідники.Організації_Pointer Організація { get; set; }
                public Довідники.ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит { get; set; }
                public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
                public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
                public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
                public Довідники.Склади_Pointer Склад { get; set; }
                public decimal Кількість { get; set; }
                public decimal Собівартість { get; set; }
                
            }            
        }
               
    }
    #endregion
    
	#region CONSTANTS BLOCK "НумераціяДокументів"
    public static class НумераціяДокументів
    {
        public static void ReadAll()
        {
            
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            bool IsSelect = Config.Kernel.DataBase.SelectAllConstants("tab_constants",
                 new string[] { "col_b7", "col_b9", "col_c1", "col_c2", "col_c4", "col_c5", "col_c6", "col_c7", "col_c8", "col_c9", "col_f6", "col_f7", "col_f8", "col_f9", "col_g1", "col_g2", "col_h1", "col_h2" }, fieldValue);
            
            if (IsSelect)
            {
                m_ПоступленняТоварівТаПослуг_Const = (fieldValue["col_b7"] != DBNull.Value) ? (int)fieldValue["col_b7"] : 0;
                m_ЗамовленняПостачальнику_Const = (fieldValue["col_b9"] != DBNull.Value) ? (int)fieldValue["col_b9"] : 0;
                m_ЗамовленняКлієнта_Const = (fieldValue["col_c1"] != DBNull.Value) ? (int)fieldValue["col_c1"] : 0;
                m_РеалізаціяТоварівТаПослуг_Const = (fieldValue["col_c2"] != DBNull.Value) ? (int)fieldValue["col_c2"] : 0;
                m_ВстановленняЦінНоменклатури_Const = (fieldValue["col_c4"] != DBNull.Value) ? (int)fieldValue["col_c4"] : 0;
                m_ПрихіднийКасовийОрдер_Const = (fieldValue["col_c5"] != DBNull.Value) ? (int)fieldValue["col_c5"] : 0;
                m_РозхіднийКасовийОрдер_Const = (fieldValue["col_c6"] != DBNull.Value) ? (int)fieldValue["col_c6"] : 0;
                m_ПереміщенняТоварів_Const = (fieldValue["col_c7"] != DBNull.Value) ? (int)fieldValue["col_c7"] : 0;
                m_ПоверненняТоварівПостачальнику_Const = (fieldValue["col_c8"] != DBNull.Value) ? (int)fieldValue["col_c8"] : 0;
                m_ПоверненняТоварівВідКлієнта_Const = (fieldValue["col_c9"] != DBNull.Value) ? (int)fieldValue["col_c9"] : 0;
                m_АктВиконанихРобіт_Const = (fieldValue["col_f6"] != DBNull.Value) ? (int)fieldValue["col_f6"] : 0;
                m_ВведенняЗалишків_Const = (fieldValue["col_f7"] != DBNull.Value) ? (int)fieldValue["col_f7"] : 0;
                m_НадлишкиТоварів_Const = (fieldValue["col_f8"] != DBNull.Value) ? (int)fieldValue["col_f8"] : 0;
                m_ПересортицяТоварів_Const = (fieldValue["col_f9"] != DBNull.Value) ? (int)fieldValue["col_f9"] : 0;
                m_ПерерахунокТоварів_Const = (fieldValue["col_g1"] != DBNull.Value) ? (int)fieldValue["col_g1"] : 0;
                m_ПсуванняТоварів_Const = (fieldValue["col_g2"] != DBNull.Value) ? (int)fieldValue["col_g2"] : 0;
                m_ВнутрішнєСпоживанняТоварів_Const = (fieldValue["col_h1"] != DBNull.Value) ? (int)fieldValue["col_h1"] : 0;
                m_РахунокФактура_Const = (fieldValue["col_h2"] != DBNull.Value) ? (int)fieldValue["col_h2"] : 0;
                
            }
			
        }
        
        
        static int m_ПоступленняТоварівТаПослуг_Const = 0;
        public static int ПоступленняТоварівТаПослуг_Const
        {
            get 
            {
                return m_ПоступленняТоварівТаПослуг_Const;
            }
            set
            {
                m_ПоступленняТоварівТаПослуг_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_b7", m_ПоступленняТоварівТаПослуг_Const);
            }
        }
        
        static int m_ЗамовленняПостачальнику_Const = 0;
        public static int ЗамовленняПостачальнику_Const
        {
            get 
            {
                return m_ЗамовленняПостачальнику_Const;
            }
            set
            {
                m_ЗамовленняПостачальнику_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_b9", m_ЗамовленняПостачальнику_Const);
            }
        }
        
        static int m_ЗамовленняКлієнта_Const = 0;
        public static int ЗамовленняКлієнта_Const
        {
            get 
            {
                return m_ЗамовленняКлієнта_Const;
            }
            set
            {
                m_ЗамовленняКлієнта_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_c1", m_ЗамовленняКлієнта_Const);
            }
        }
        
        static int m_РеалізаціяТоварівТаПослуг_Const = 0;
        public static int РеалізаціяТоварівТаПослуг_Const
        {
            get 
            {
                return m_РеалізаціяТоварівТаПослуг_Const;
            }
            set
            {
                m_РеалізаціяТоварівТаПослуг_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_c2", m_РеалізаціяТоварівТаПослуг_Const);
            }
        }
        
        static int m_ВстановленняЦінНоменклатури_Const = 0;
        public static int ВстановленняЦінНоменклатури_Const
        {
            get 
            {
                return m_ВстановленняЦінНоменклатури_Const;
            }
            set
            {
                m_ВстановленняЦінНоменклатури_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_c4", m_ВстановленняЦінНоменклатури_Const);
            }
        }
        
        static int m_ПрихіднийКасовийОрдер_Const = 0;
        public static int ПрихіднийКасовийОрдер_Const
        {
            get 
            {
                return m_ПрихіднийКасовийОрдер_Const;
            }
            set
            {
                m_ПрихіднийКасовийОрдер_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_c5", m_ПрихіднийКасовийОрдер_Const);
            }
        }
        
        static int m_РозхіднийКасовийОрдер_Const = 0;
        public static int РозхіднийКасовийОрдер_Const
        {
            get 
            {
                return m_РозхіднийКасовийОрдер_Const;
            }
            set
            {
                m_РозхіднийКасовийОрдер_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_c6", m_РозхіднийКасовийОрдер_Const);
            }
        }
        
        static int m_ПереміщенняТоварів_Const = 0;
        public static int ПереміщенняТоварів_Const
        {
            get 
            {
                return m_ПереміщенняТоварів_Const;
            }
            set
            {
                m_ПереміщенняТоварів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_c7", m_ПереміщенняТоварів_Const);
            }
        }
        
        static int m_ПоверненняТоварівПостачальнику_Const = 0;
        public static int ПоверненняТоварівПостачальнику_Const
        {
            get 
            {
                return m_ПоверненняТоварівПостачальнику_Const;
            }
            set
            {
                m_ПоверненняТоварівПостачальнику_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_c8", m_ПоверненняТоварівПостачальнику_Const);
            }
        }
        
        static int m_ПоверненняТоварівВідКлієнта_Const = 0;
        public static int ПоверненняТоварівВідКлієнта_Const
        {
            get 
            {
                return m_ПоверненняТоварівВідКлієнта_Const;
            }
            set
            {
                m_ПоверненняТоварівВідКлієнта_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_c9", m_ПоверненняТоварівВідКлієнта_Const);
            }
        }
        
        static int m_АктВиконанихРобіт_Const = 0;
        public static int АктВиконанихРобіт_Const
        {
            get 
            {
                return m_АктВиконанихРобіт_Const;
            }
            set
            {
                m_АктВиконанихРобіт_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_f6", m_АктВиконанихРобіт_Const);
            }
        }
        
        static int m_ВведенняЗалишків_Const = 0;
        public static int ВведенняЗалишків_Const
        {
            get 
            {
                return m_ВведенняЗалишків_Const;
            }
            set
            {
                m_ВведенняЗалишків_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_f7", m_ВведенняЗалишків_Const);
            }
        }
        
        static int m_НадлишкиТоварів_Const = 0;
        public static int НадлишкиТоварів_Const
        {
            get 
            {
                return m_НадлишкиТоварів_Const;
            }
            set
            {
                m_НадлишкиТоварів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_f8", m_НадлишкиТоварів_Const);
            }
        }
        
        static int m_ПересортицяТоварів_Const = 0;
        public static int ПересортицяТоварів_Const
        {
            get 
            {
                return m_ПересортицяТоварів_Const;
            }
            set
            {
                m_ПересортицяТоварів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_f9", m_ПересортицяТоварів_Const);
            }
        }
        
        static int m_ПерерахунокТоварів_Const = 0;
        public static int ПерерахунокТоварів_Const
        {
            get 
            {
                return m_ПерерахунокТоварів_Const;
            }
            set
            {
                m_ПерерахунокТоварів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_g1", m_ПерерахунокТоварів_Const);
            }
        }
        
        static int m_ПсуванняТоварів_Const = 0;
        public static int ПсуванняТоварів_Const
        {
            get 
            {
                return m_ПсуванняТоварів_Const;
            }
            set
            {
                m_ПсуванняТоварів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_g2", m_ПсуванняТоварів_Const);
            }
        }
        
        static int m_ВнутрішнєСпоживанняТоварів_Const = 0;
        public static int ВнутрішнєСпоживанняТоварів_Const
        {
            get 
            {
                return m_ВнутрішнєСпоживанняТоварів_Const;
            }
            set
            {
                m_ВнутрішнєСпоживанняТоварів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_h1", m_ВнутрішнєСпоживанняТоварів_Const);
            }
        }
        
        static int m_РахунокФактура_Const = 0;
        public static int РахунокФактура_Const
        {
            get 
            {
                return m_РахунокФактура_Const;
            }
            set
            {
                m_РахунокФактура_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_h2", m_РахунокФактура_Const);
            }
        }
             
    }
    #endregion
    
	#region CONSTANTS BLOCK "НумераціяДовідників"
    public static class НумераціяДовідників
    {
        public static void ReadAll()
        {
            
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            bool IsSelect = Config.Kernel.DataBase.SelectAllConstants("tab_constants",
                 new string[] { "col_b8", "col_d1", "col_d2", "col_d3", "col_d4", "col_d5", "col_d6", "col_d7", "col_d8", "col_d9", "col_e1", "col_e2", "col_e3", "col_e4", "col_e5", "col_e6", "col_e7", "col_e8", "col_e9", "col_f1", "col_f2", "col_f3", "col_f4", "col_f5" }, fieldValue);
            
            if (IsSelect)
            {
                m_Номенклатура_Const = (fieldValue["col_b8"] != DBNull.Value) ? (int)fieldValue["col_b8"] : 0;
                m_Номенклатура_Папки_Const = (fieldValue["col_d1"] != DBNull.Value) ? (int)fieldValue["col_d1"] : 0;
                m_Склади_Const = (fieldValue["col_d2"] != DBNull.Value) ? (int)fieldValue["col_d2"] : 0;
                m_Склади_Папки_Const = (fieldValue["col_d3"] != DBNull.Value) ? (int)fieldValue["col_d3"] : 0;
                m_Контрагенти_Const = (fieldValue["col_d4"] != DBNull.Value) ? (int)fieldValue["col_d4"] : 0;
                m_Контрагенти_Папки_Const = (fieldValue["col_d5"] != DBNull.Value) ? (int)fieldValue["col_d5"] : 0;
                m_ХарактеристикиНоменклатури_Const = (fieldValue["col_d6"] != DBNull.Value) ? (int)fieldValue["col_d6"] : 0;
                m_Валюти_Const = (fieldValue["col_d7"] != DBNull.Value) ? (int)fieldValue["col_d7"] : 0;
                m_Організації_Const = (fieldValue["col_d8"] != DBNull.Value) ? (int)fieldValue["col_d8"] : 0;
                m_Виробники_Const = (fieldValue["col_d9"] != DBNull.Value) ? (int)fieldValue["col_d9"] : 0;
                m_ВидиНоменклатури_Const = (fieldValue["col_e1"] != DBNull.Value) ? (int)fieldValue["col_e1"] : 0;
                m_ПакуванняОдиниціВиміру_Const = (fieldValue["col_e2"] != DBNull.Value) ? (int)fieldValue["col_e2"] : 0;
                m_ВидиЦін_Const = (fieldValue["col_e3"] != DBNull.Value) ? (int)fieldValue["col_e3"] : 0;
                m_ВидиЦінПостачальників_Const = (fieldValue["col_e4"] != DBNull.Value) ? (int)fieldValue["col_e4"] : 0;
                m_Користувачі_Const = (fieldValue["col_e5"] != DBNull.Value) ? (int)fieldValue["col_e5"] : 0;
                m_ФізичніОсоби_Const = (fieldValue["col_e6"] != DBNull.Value) ? (int)fieldValue["col_e6"] : 0;
                m_СтруктураПідприємства_Const = (fieldValue["col_e7"] != DBNull.Value) ? (int)fieldValue["col_e7"] : 0;
                m_КраїниСвіту_Const = (fieldValue["col_e8"] != DBNull.Value) ? (int)fieldValue["col_e8"] : 0;
                m_Файли_Const = (fieldValue["col_e9"] != DBNull.Value) ? (int)fieldValue["col_e9"] : 0;
                m_Каси_Const = (fieldValue["col_f1"] != DBNull.Value) ? (int)fieldValue["col_f1"] : 0;
                m_БанківськіРахункиОрганізацій_Const = (fieldValue["col_f2"] != DBNull.Value) ? (int)fieldValue["col_f2"] : 0;
                m_ДоговориКонтрагентів_Const = (fieldValue["col_f3"] != DBNull.Value) ? (int)fieldValue["col_f3"] : 0;
                m_БанківськіРахункиКонтрагентів_Const = (fieldValue["col_f4"] != DBNull.Value) ? (int)fieldValue["col_f4"] : 0;
                m_СтаттяРухуКоштів_Const = (fieldValue["col_f5"] != DBNull.Value) ? (int)fieldValue["col_f5"] : 0;
                
            }
			
        }
        
        
        static int m_Номенклатура_Const = 0;
        public static int Номенклатура_Const
        {
            get 
            {
                return m_Номенклатура_Const;
            }
            set
            {
                m_Номенклатура_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_b8", m_Номенклатура_Const);
            }
        }
        
        static int m_Номенклатура_Папки_Const = 0;
        public static int Номенклатура_Папки_Const
        {
            get 
            {
                return m_Номенклатура_Папки_Const;
            }
            set
            {
                m_Номенклатура_Папки_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_d1", m_Номенклатура_Папки_Const);
            }
        }
        
        static int m_Склади_Const = 0;
        public static int Склади_Const
        {
            get 
            {
                return m_Склади_Const;
            }
            set
            {
                m_Склади_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_d2", m_Склади_Const);
            }
        }
        
        static int m_Склади_Папки_Const = 0;
        public static int Склади_Папки_Const
        {
            get 
            {
                return m_Склади_Папки_Const;
            }
            set
            {
                m_Склади_Папки_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_d3", m_Склади_Папки_Const);
            }
        }
        
        static int m_Контрагенти_Const = 0;
        public static int Контрагенти_Const
        {
            get 
            {
                return m_Контрагенти_Const;
            }
            set
            {
                m_Контрагенти_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_d4", m_Контрагенти_Const);
            }
        }
        
        static int m_Контрагенти_Папки_Const = 0;
        public static int Контрагенти_Папки_Const
        {
            get 
            {
                return m_Контрагенти_Папки_Const;
            }
            set
            {
                m_Контрагенти_Папки_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_d5", m_Контрагенти_Папки_Const);
            }
        }
        
        static int m_ХарактеристикиНоменклатури_Const = 0;
        public static int ХарактеристикиНоменклатури_Const
        {
            get 
            {
                return m_ХарактеристикиНоменклатури_Const;
            }
            set
            {
                m_ХарактеристикиНоменклатури_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_d6", m_ХарактеристикиНоменклатури_Const);
            }
        }
        
        static int m_Валюти_Const = 0;
        public static int Валюти_Const
        {
            get 
            {
                return m_Валюти_Const;
            }
            set
            {
                m_Валюти_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_d7", m_Валюти_Const);
            }
        }
        
        static int m_Організації_Const = 0;
        public static int Організації_Const
        {
            get 
            {
                return m_Організації_Const;
            }
            set
            {
                m_Організації_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_d8", m_Організації_Const);
            }
        }
        
        static int m_Виробники_Const = 0;
        public static int Виробники_Const
        {
            get 
            {
                return m_Виробники_Const;
            }
            set
            {
                m_Виробники_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_d9", m_Виробники_Const);
            }
        }
        
        static int m_ВидиНоменклатури_Const = 0;
        public static int ВидиНоменклатури_Const
        {
            get 
            {
                return m_ВидиНоменклатури_Const;
            }
            set
            {
                m_ВидиНоменклатури_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_e1", m_ВидиНоменклатури_Const);
            }
        }
        
        static int m_ПакуванняОдиниціВиміру_Const = 0;
        public static int ПакуванняОдиниціВиміру_Const
        {
            get 
            {
                return m_ПакуванняОдиниціВиміру_Const;
            }
            set
            {
                m_ПакуванняОдиниціВиміру_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_e2", m_ПакуванняОдиниціВиміру_Const);
            }
        }
        
        static int m_ВидиЦін_Const = 0;
        public static int ВидиЦін_Const
        {
            get 
            {
                return m_ВидиЦін_Const;
            }
            set
            {
                m_ВидиЦін_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_e3", m_ВидиЦін_Const);
            }
        }
        
        static int m_ВидиЦінПостачальників_Const = 0;
        public static int ВидиЦінПостачальників_Const
        {
            get 
            {
                return m_ВидиЦінПостачальників_Const;
            }
            set
            {
                m_ВидиЦінПостачальників_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_e4", m_ВидиЦінПостачальників_Const);
            }
        }
        
        static int m_Користувачі_Const = 0;
        public static int Користувачі_Const
        {
            get 
            {
                return m_Користувачі_Const;
            }
            set
            {
                m_Користувачі_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_e5", m_Користувачі_Const);
            }
        }
        
        static int m_ФізичніОсоби_Const = 0;
        public static int ФізичніОсоби_Const
        {
            get 
            {
                return m_ФізичніОсоби_Const;
            }
            set
            {
                m_ФізичніОсоби_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_e6", m_ФізичніОсоби_Const);
            }
        }
        
        static int m_СтруктураПідприємства_Const = 0;
        public static int СтруктураПідприємства_Const
        {
            get 
            {
                return m_СтруктураПідприємства_Const;
            }
            set
            {
                m_СтруктураПідприємства_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_e7", m_СтруктураПідприємства_Const);
            }
        }
        
        static int m_КраїниСвіту_Const = 0;
        public static int КраїниСвіту_Const
        {
            get 
            {
                return m_КраїниСвіту_Const;
            }
            set
            {
                m_КраїниСвіту_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_e8", m_КраїниСвіту_Const);
            }
        }
        
        static int m_Файли_Const = 0;
        public static int Файли_Const
        {
            get 
            {
                return m_Файли_Const;
            }
            set
            {
                m_Файли_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_e9", m_Файли_Const);
            }
        }
        
        static int m_Каси_Const = 0;
        public static int Каси_Const
        {
            get 
            {
                return m_Каси_Const;
            }
            set
            {
                m_Каси_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_f1", m_Каси_Const);
            }
        }
        
        static int m_БанківськіРахункиОрганізацій_Const = 0;
        public static int БанківськіРахункиОрганізацій_Const
        {
            get 
            {
                return m_БанківськіРахункиОрганізацій_Const;
            }
            set
            {
                m_БанківськіРахункиОрганізацій_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_f2", m_БанківськіРахункиОрганізацій_Const);
            }
        }
        
        static int m_ДоговориКонтрагентів_Const = 0;
        public static int ДоговориКонтрагентів_Const
        {
            get 
            {
                return m_ДоговориКонтрагентів_Const;
            }
            set
            {
                m_ДоговориКонтрагентів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_f3", m_ДоговориКонтрагентів_Const);
            }
        }
        
        static int m_БанківськіРахункиКонтрагентів_Const = 0;
        public static int БанківськіРахункиКонтрагентів_Const
        {
            get 
            {
                return m_БанківськіРахункиКонтрагентів_Const;
            }
            set
            {
                m_БанківськіРахункиКонтрагентів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_f4", m_БанківськіРахункиКонтрагентів_Const);
            }
        }
        
        static int m_СтаттяРухуКоштів_Const = 0;
        public static int СтаттяРухуКоштів_Const
        {
            get 
            {
                return m_СтаттяРухуКоштів_Const;
            }
            set
            {
                m_СтаттяРухуКоштів_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_f5", m_СтаттяРухуКоштів_Const);
            }
        }
             
    }
    #endregion
    
	#region CONSTANTS BLOCK "ЖурналиДокументів"
    public static class ЖурналиДокументів
    {
        public static void ReadAll()
        {
            
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            bool IsSelect = Config.Kernel.DataBase.SelectAllConstants("tab_constants",
                 new string[] { "col_h3" }, fieldValue);
            
            if (IsSelect)
            {
                m_ОсновнийТипПеріоду_Const = (fieldValue["col_h3"] != DBNull.Value) ? (Перелічення.ТипПеріодуДляЖурналівДокументів)fieldValue["col_h3"] : 0;
                
            }
			
        }
        
        
        static Перелічення.ТипПеріодуДляЖурналівДокументів m_ОсновнийТипПеріоду_Const = 0;
        public static Перелічення.ТипПеріодуДляЖурналівДокументів ОсновнийТипПеріоду_Const
        {
            get 
            {
                return m_ОсновнийТипПеріоду_Const;
            }
            set
            {
                m_ОсновнийТипПеріоду_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_h3", (int)m_ОсновнийТипПеріоду_Const);
            }
        }
             
    }
    #endregion
    
	#region CONSTANTS BLOCK "ПартіїТоварів"
    public static class ПартіїТоварів
    {
        public static void ReadAll()
        {
            
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            bool IsSelect = Config.Kernel.DataBase.SelectAllConstants("tab_constants",
                 new string[] { "col_h4" }, fieldValue);
            
            if (IsSelect)
            {
                m_МетодСписанняПартій_Const = (fieldValue["col_h4"] != DBNull.Value) ? (Перелічення.МетодиСписанняПартій)fieldValue["col_h4"] : 0;
                
            }
			
        }
        
        
        static Перелічення.МетодиСписанняПартій m_МетодСписанняПартій_Const = 0;
        public static Перелічення.МетодиСписанняПартій МетодСписанняПартій_Const
        {
            get 
            {
                return m_МетодСписанняПартій_Const;
            }
            set
            {
                m_МетодСписанняПартій_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_h4", (int)m_МетодСписанняПартій_Const);
            }
        }
             
    }
    #endregion
    
	#region CONSTANTS BLOCK "ЗавантаженняДанихІзСайтів"
    public static class ЗавантаженняДанихІзСайтів
    {
        public static void ReadAll()
        {
            
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            bool IsSelect = Config.Kernel.DataBase.SelectAllConstants("tab_constants",
                 new string[] { "col_h5" }, fieldValue);
            
            if (IsSelect)
            {
                m_ЗавантаженняКурсівВалют_Const = fieldValue["col_h5"].ToString();
                
            }
			
        }
        
        
        static string m_ЗавантаженняКурсівВалют_Const = "";
        public static string ЗавантаженняКурсівВалют_Const
        {
            get 
            {
                return m_ЗавантаженняКурсівВалют_Const;
            }
            set
            {
                m_ЗавантаженняКурсівВалют_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_h5", m_ЗавантаженняКурсівВалют_Const);
            }
        }
        
        public class ЗавантаженняКурсівВалют_Історія_TablePart : ConstantsTablePart
        {
            public ЗавантаженняКурсівВалют_Історія_TablePart() : base(Config.Kernel, "tab_b22",
                 new string[] { "col_a1", "col_a2", "col_a3", "col_a4" }) 
            {
                Records = new List<Record>();
            }
            
            public const string TABLE = "tab_b22";
            
            public const string Дата = "col_a1";
            public const string Стан = "col_a2";
            public const string Посилання = "col_a3";
            public const string Повідомлення = "col_a4";
            public List<Record> Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    record.Дата = (fieldValue["col_a1"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a1"].ToString()) : DateTime.MinValue;
                    record.Стан = fieldValue["col_a2"].ToString();
                    record.Посилання = fieldValue["col_a3"].ToString();
                    record.Повідомлення = fieldValue["col_a4"].ToString();
                    
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                    fieldValue.Add("col_a1", record.Дата);
                    fieldValue.Add("col_a2", record.Стан);
                    fieldValue.Add("col_a3", record.Посилання);
                    fieldValue.Add("col_a4", record.Повідомлення);
                    
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                public Record()
                {
                    Дата = DateTime.MinValue;
                    Стан = "";
                    Посилання = "";
                    Повідомлення = "";
                    
                }
                public DateTime Дата { get; set; }
                public string Стан { get; set; }
                public string Посилання { get; set; }
                public string Повідомлення { get; set; }
                
            }            
        }
               
    }
    #endregion
    
	#region CONSTANTS BLOCK "ПриЗапускуПрограми"
    public static class ПриЗапускуПрограми
    {
        public static void ReadAll()
        {
            
            Dictionary<string, object> fieldValue = new Dictionary<string, object>();
            bool IsSelect = Config.Kernel.DataBase.SelectAllConstants("tab_constants",
                 new string[] { "col_h7", "col_h6" }, fieldValue);
            
            if (IsSelect)
            {
                m_ПрограмаЗаповненаПочатковимиДаними_Const = (fieldValue["col_h7"] != DBNull.Value) ? bool.Parse(fieldValue["col_h7"].ToString()) : false;
                m_ВідкриватиРобочийСтіл_Const = (fieldValue["col_h6"] != DBNull.Value) ? bool.Parse(fieldValue["col_h6"].ToString()) : false;
                
            }
			
        }
        
        
        static bool m_ПрограмаЗаповненаПочатковимиДаними_Const = false;
        public static bool ПрограмаЗаповненаПочатковимиДаними_Const
        {
            get 
            {
                return m_ПрограмаЗаповненаПочатковимиДаними_Const;
            }
            set
            {
                m_ПрограмаЗаповненаПочатковимиДаними_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_h7", m_ПрограмаЗаповненаПочатковимиДаними_Const);
            }
        }
        
        static bool m_ВідкриватиРобочийСтіл_Const = false;
        public static bool ВідкриватиРобочийСтіл_Const
        {
            get 
            {
                return m_ВідкриватиРобочийСтіл_Const;
            }
            set
            {
                m_ВідкриватиРобочийСтіл_Const = value;
                Config.Kernel.DataBase.SaveConstants("tab_constants", "col_h6", m_ВідкриватиРобочийСтіл_Const);
            }
        }
             
    }
    #endregion
    
}

namespace StorageAndTrade_1_0.Довідники
{
    
    #region DIRECTORY "Організації"
    
    public static class Організації_Const
    {
        public const string TABLE = "tab_a01";
        
        public const string Назва = "col_a1";
        public const string Код = "col_a2";
        public const string НазваПовна = "col_a3";
        public const string НазваСкорочена = "col_a4";
        public const string ДатаРеєстрації = "col_a5";
        public const string КраїнаРеєстрації = "col_a6";
        public const string СвідоцтвоСеріяНомер = "col_a7";
        public const string СвідоцтвоДатаВидачі = "col_a8";
    }
	
    
    public class Організації_Objest : DirectoryObject
    {
        public Організації_Objest() : base(Config.Kernel, "tab_a01",
             new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            НазваСкорочена = "";
            ДатаРеєстрації = DateTime.MinValue;
            КраїнаРеєстрації = "";
            СвідоцтвоСеріяНомер = "";
            СвідоцтвоДатаВидачі = "";
            
            //Табличні частини
            Контакти_TablePart = new Організації_Контакти_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_a1"].ToString();
                Код = base.FieldValue["col_a2"].ToString();
                НазваПовна = base.FieldValue["col_a3"].ToString();
                НазваСкорочена = base.FieldValue["col_a4"].ToString();
                ДатаРеєстрації = (base.FieldValue["col_a5"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a5"].ToString()) : DateTime.MinValue;
                КраїнаРеєстрації = base.FieldValue["col_a6"].ToString();
                СвідоцтвоСеріяНомер = base.FieldValue["col_a7"].ToString();
                СвідоцтвоДатаВидачі = base.FieldValue["col_a8"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_a1"] = Назва;
            base.FieldValue["col_a2"] = Код;
            base.FieldValue["col_a3"] = НазваПовна;
            base.FieldValue["col_a4"] = НазваСкорочена;
            base.FieldValue["col_a5"] = ДатаРеєстрації;
            base.FieldValue["col_a6"] = КраїнаРеєстрації;
            base.FieldValue["col_a7"] = СвідоцтвоСеріяНомер;
            base.FieldValue["col_a8"] = СвідоцтвоДатаВидачі;
            
            BaseSave();
			
        }
		
        public Організації_Objest Copy()
        {
            Організації_Objest copy = new Організації_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.НазваПовна = НазваПовна;
			copy.НазваСкорочена = НазваСкорочена;
			copy.ДатаРеєстрації = ДатаРеєстрації;
			copy.КраїнаРеєстрації = КраїнаРеєстрації;
			copy.СвідоцтвоСеріяНомер = СвідоцтвоСеріяНомер;
			copy.СвідоцтвоДатаВидачі = СвідоцтвоДатаВидачі;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] { "tab_a02" });
        }
        
        public Організації_Pointer GetDirectoryPointer()
        {
            Організації_Pointer directoryPointer = new Організації_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        public string НазваСкорочена { get; set; }
        public DateTime ДатаРеєстрації { get; set; }
        public string КраїнаРеєстрації { get; set; }
        public string СвідоцтвоСеріяНомер { get; set; }
        public string СвідоцтвоДатаВидачі { get; set; }
        
        //Табличні частини
        public Організації_Контакти_TablePart Контакти_TablePart { get; set; }
        
    }
    
    
    public class Організації_Pointer : DirectoryPointer
    {
        public Організації_Pointer(object uid = null) : base(Config.Kernel, "tab_a01")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Організації_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a01")
        {
            base.Init(uid, fields);
        }
        
        public Організації_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Організації_Objest ОрганізаціїObjestItem = new Організації_Objest();
            return ОрганізаціїObjestItem.Read(base.UnigueID) ? ОрганізаціїObjestItem : null;
        }
		
        public Організації_Pointer GetNewDirectoryPointer()
        {
            return new Організації_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_a1" }
			);
        }
		
        public Організації_Pointer GetEmptyPointer()
        {
            return new Організації_Pointer();
        }
    }
    
    
    public class Організації_Select : DirectorySelect
    {
        public Організації_Select() : base(Config.Kernel, "tab_a01") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Організації_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Організації_Pointer Current { get; private set; }
        
        public Організації_Pointer FindByField(string name, object value)
        {
            Організації_Pointer itemPointer = new Організації_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Організації_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Організації_Pointer> directoryPointerList = new List<Організації_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Організації_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    public class Організації_Контакти_TablePart : DirectoryTablePart
    {
        public Організації_Контакти_TablePart(Організації_Objest owner) : base(Config.Kernel, "tab_a02",
             new string[] { "col_a9", "col_a4", "col_a5", "col_a1", "col_a6", "col_a2", "col_a3" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string Тип = "col_a9";
        public const string Телефон = "col_a4";
        public const string ЕлектроннаПошта = "col_a5";
        public const string Країна = "col_a1";
        public const string Область = "col_a6";
        public const string Район = "col_a2";
        public const string Місто = "col_a3";

        public Організації_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_a9"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_a9"] : 0;
                record.Телефон = fieldValue["col_a4"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_a5"].ToString();
                record.Країна = fieldValue["col_a1"].ToString();
                record.Область = fieldValue["col_a6"].ToString();
                record.Район = fieldValue["col_a2"].ToString();
                record.Місто = fieldValue["col_a3"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a9", (int)record.Тип);
                fieldValue.Add("col_a4", record.Телефон);
                fieldValue.Add("col_a5", record.ЕлектроннаПошта);
                fieldValue.Add("col_a1", record.Країна);
                fieldValue.Add("col_a6", record.Область);
                fieldValue.Add("col_a2", record.Район);
                fieldValue.Add("col_a3", record.Місто);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Телефон = "";
                ЕлектроннаПошта = "";
                Країна = "";
                Область = "";
                Район = "";
                Місто = "";
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Телефон { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Країна { get; set; }
            public string Область { get; set; }
            public string Район { get; set; }
            public string Місто { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "Номенклатура"
    
    public static class Номенклатура_Const
    {
        public const string TABLE = "tab_a03";
        
        public const string Назва = "col_b1";
        public const string Код = "col_b2";
        public const string НазваПовна = "col_b4";
        public const string Опис = "col_a1";
        public const string Артикул = "col_b3";
        public const string ТипНоменклатури = "col_b5";
        public const string Виробник = "col_a2";
        public const string ВидНоменклатури = "col_a3";
        public const string ОдиницяВиміру = "col_a4";
        public const string Папка = "col_a5";
        public const string ОсновнаКартинкаФайл = "col_a7";
    }
	
    
    public class Номенклатура_Objest : DirectoryObject
    {
        public Номенклатура_Objest() : base(Config.Kernel, "tab_a03",
             new string[] { "col_b1", "col_b2", "col_b4", "col_a1", "col_b3", "col_b5", "col_a2", "col_a3", "col_a4", "col_a5", "col_a7" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            Опис = "";
            Артикул = "";
            ТипНоменклатури = 0;
            Виробник = new Довідники.Виробники_Pointer();
            ВидНоменклатури = new Довідники.ВидиНоменклатури_Pointer();
            ОдиницяВиміру = new Довідники.ПакуванняОдиниціВиміру_Pointer();
            Папка = new Довідники.Номенклатура_Папки_Pointer();
            ОсновнаКартинкаФайл = new Довідники.Файли_Pointer();
            
            //Табличні частини
            Файли_TablePart = new Номенклатура_Файли_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_b1"].ToString();
                Код = base.FieldValue["col_b2"].ToString();
                НазваПовна = base.FieldValue["col_b4"].ToString();
                Опис = base.FieldValue["col_a1"].ToString();
                Артикул = base.FieldValue["col_b3"].ToString();
                ТипНоменклатури = (base.FieldValue["col_b5"] != DBNull.Value) ? (Перелічення.ТипиНоменклатури)base.FieldValue["col_b5"] : 0;
                Виробник = new Довідники.Виробники_Pointer(base.FieldValue["col_a2"]);
                ВидНоменклатури = new Довідники.ВидиНоменклатури_Pointer(base.FieldValue["col_a3"]);
                ОдиницяВиміру = new Довідники.ПакуванняОдиниціВиміру_Pointer(base.FieldValue["col_a4"]);
                Папка = new Довідники.Номенклатура_Папки_Pointer(base.FieldValue["col_a5"]);
                ОсновнаКартинкаФайл = new Довідники.Файли_Pointer(base.FieldValue["col_a7"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    Номенклатура_Triggers.BeforeRecording(this);
			base.FieldValue["col_b1"] = Назва;
            base.FieldValue["col_b2"] = Код;
            base.FieldValue["col_b4"] = НазваПовна;
            base.FieldValue["col_a1"] = Опис;
            base.FieldValue["col_b3"] = Артикул;
            base.FieldValue["col_b5"] = (int)ТипНоменклатури;
            base.FieldValue["col_a2"] = Виробник.UnigueID.UGuid;
            base.FieldValue["col_a3"] = ВидНоменклатури.UnigueID.UGuid;
            base.FieldValue["col_a4"] = ОдиницяВиміру.UnigueID.UGuid;
            base.FieldValue["col_a5"] = Папка.UnigueID.UGuid;
            base.FieldValue["col_a7"] = ОсновнаКартинкаФайл.UnigueID.UGuid;
            
            BaseSave();
			Номенклатура_Triggers.AfterRecording(this);
        }
		
        public Номенклатура_Objest Copy()
        {
            Номенклатура_Objest copy = new Номенклатура_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.НазваПовна = НазваПовна;
			copy.Опис = Опис;
			copy.Артикул = Артикул;
			copy.ТипНоменклатури = ТипНоменклатури;
			copy.Виробник = Виробник;
			copy.ВидНоменклатури = ВидНоменклатури;
			copy.ОдиницяВиміру = ОдиницяВиміру;
			copy.Папка = Папка;
			copy.ОсновнаКартинкаФайл = ОсновнаКартинкаФайл;
			
			return copy;
        }

        public void Delete()
        {
            Номенклатура_Triggers.BeforeDelete(this);
			base.BaseDelete(new string[] { "tab_b19" });
        }
        
        public Номенклатура_Pointer GetDirectoryPointer()
        {
            Номенклатура_Pointer directoryPointer = new Номенклатура_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        public string Опис { get; set; }
        public string Артикул { get; set; }
        public Перелічення.ТипиНоменклатури ТипНоменклатури { get; set; }
        public Довідники.Виробники_Pointer Виробник { get; set; }
        public Довідники.ВидиНоменклатури_Pointer ВидНоменклатури { get; set; }
        public Довідники.ПакуванняОдиниціВиміру_Pointer ОдиницяВиміру { get; set; }
        public Довідники.Номенклатура_Папки_Pointer Папка { get; set; }
        public Довідники.Файли_Pointer ОсновнаКартинкаФайл { get; set; }
        
        //Табличні частини
        public Номенклатура_Файли_TablePart Файли_TablePart { get; set; }
        
    }
    
    
    public class Номенклатура_Pointer : DirectoryPointer
    {
        public Номенклатура_Pointer(object uid = null) : base(Config.Kernel, "tab_a03")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Номенклатура_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a03")
        {
            base.Init(uid, fields);
        }
        
        public Номенклатура_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Номенклатура_Objest НоменклатураObjestItem = new Номенклатура_Objest();
            return НоменклатураObjestItem.Read(base.UnigueID) ? НоменклатураObjestItem : null;
        }
		
        public Номенклатура_Pointer GetNewDirectoryPointer()
        {
            return new Номенклатура_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_b1" }
			);
        }
		
        public Номенклатура_Pointer GetEmptyPointer()
        {
            return new Номенклатура_Pointer();
        }
    }
    
    
    public class Номенклатура_Select : DirectorySelect
    {
        public Номенклатура_Select() : base(Config.Kernel, "tab_a03") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Номенклатура_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Номенклатура_Pointer Current { get; private set; }
        
        public Номенклатура_Pointer FindByField(string name, object value)
        {
            Номенклатура_Pointer itemPointer = new Номенклатура_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Номенклатура_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Номенклатура_Pointer> directoryPointerList = new List<Номенклатура_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Номенклатура_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    public class Номенклатура_Файли_TablePart : DirectoryTablePart
    {
        public Номенклатура_Файли_TablePart(Номенклатура_Objest owner) : base(Config.Kernel, "tab_b19",
             new string[] { "col_a1", "col_a3" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string Файл = "col_a1";
        public const string Основний = "col_a3";

        public Номенклатура_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Файл = new Довідники.Файли_Pointer(fieldValue["col_a1"]);
                record.Основний = (fieldValue["col_a3"] != DBNull.Value) ? bool.Parse(fieldValue["col_a3"].ToString()) : false;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.Файл.UnigueID.UGuid);
                fieldValue.Add("col_a3", record.Основний);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Файл = new Довідники.Файли_Pointer();
                Основний = false;
                
            }
            public Довідники.Файли_Pointer Файл { get; set; }
            public bool Основний { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "Виробники"
    
    public static class Виробники_Const
    {
        public const string TABLE = "tab_a04";
        
        public const string Назва = "col_b6";
        public const string Код = "col_b7";
    }
	
    
    public class Виробники_Objest : DirectoryObject
    {
        public Виробники_Objest() : base(Config.Kernel, "tab_a04",
             new string[] { "col_b6", "col_b7" }) 
        {
            Назва = "";
            Код = "";
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_b6"].ToString();
                Код = base.FieldValue["col_b7"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_b6"] = Назва;
            base.FieldValue["col_b7"] = Код;
            
            BaseSave();
			
        }
		
        public Виробники_Objest Copy()
        {
            Виробники_Objest copy = new Виробники_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public Виробники_Pointer GetDirectoryPointer()
        {
            Виробники_Pointer directoryPointer = new Виробники_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        
    }
    
    
    public class Виробники_Pointer : DirectoryPointer
    {
        public Виробники_Pointer(object uid = null) : base(Config.Kernel, "tab_a04")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Виробники_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a04")
        {
            base.Init(uid, fields);
        }
        
        public Виробники_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Виробники_Objest ВиробникиObjestItem = new Виробники_Objest();
            return ВиробникиObjestItem.Read(base.UnigueID) ? ВиробникиObjestItem : null;
        }
		
        public Виробники_Pointer GetNewDirectoryPointer()
        {
            return new Виробники_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_b6" }
			);
        }
		
        public Виробники_Pointer GetEmptyPointer()
        {
            return new Виробники_Pointer();
        }
    }
    
    
    public class Виробники_Select : DirectorySelect
    {
        public Виробники_Select() : base(Config.Kernel, "tab_a04") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Виробники_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Виробники_Pointer Current { get; private set; }
        
        public Виробники_Pointer FindByField(string name, object value)
        {
            Виробники_Pointer itemPointer = new Виробники_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Виробники_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Виробники_Pointer> directoryPointerList = new List<Виробники_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Виробники_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ВидиНоменклатури"
    
    public static class ВидиНоменклатури_Const
    {
        public const string TABLE = "tab_a05";
        
        public const string Назва = "col_b8";
        public const string Код = "col_b9";
        public const string Опис = "col_a2";
        public const string ТипНоменклатури = "col_a1";
        public const string ОдиницяВиміру = "col_a4";
    }
	
    
    public class ВидиНоменклатури_Objest : DirectoryObject
    {
        public ВидиНоменклатури_Objest() : base(Config.Kernel, "tab_a05",
             new string[] { "col_b8", "col_b9", "col_a2", "col_a1", "col_a4" }) 
        {
            Назва = "";
            Код = "";
            Опис = "";
            ТипНоменклатури = 0;
            ОдиницяВиміру = new Довідники.ПакуванняОдиниціВиміру_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_b8"].ToString();
                Код = base.FieldValue["col_b9"].ToString();
                Опис = base.FieldValue["col_a2"].ToString();
                ТипНоменклатури = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.ТипиНоменклатури)base.FieldValue["col_a1"] : 0;
                ОдиницяВиміру = new Довідники.ПакуванняОдиниціВиміру_Pointer(base.FieldValue["col_a4"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_b8"] = Назва;
            base.FieldValue["col_b9"] = Код;
            base.FieldValue["col_a2"] = Опис;
            base.FieldValue["col_a1"] = (int)ТипНоменклатури;
            base.FieldValue["col_a4"] = ОдиницяВиміру.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public ВидиНоменклатури_Objest Copy()
        {
            ВидиНоменклатури_Objest copy = new ВидиНоменклатури_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.Опис = Опис;
			copy.ТипНоменклатури = ТипНоменклатури;
			copy.ОдиницяВиміру = ОдиницяВиміру;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public ВидиНоменклатури_Pointer GetDirectoryPointer()
        {
            ВидиНоменклатури_Pointer directoryPointer = new ВидиНоменклатури_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string Опис { get; set; }
        public Перелічення.ТипиНоменклатури ТипНоменклатури { get; set; }
        public Довідники.ПакуванняОдиниціВиміру_Pointer ОдиницяВиміру { get; set; }
        
    }
    
    
    public class ВидиНоменклатури_Pointer : DirectoryPointer
    {
        public ВидиНоменклатури_Pointer(object uid = null) : base(Config.Kernel, "tab_a05")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВидиНоменклатури_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a05")
        {
            base.Init(uid, fields);
        }
        
        public ВидиНоменклатури_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ВидиНоменклатури_Objest ВидиНоменклатуриObjestItem = new ВидиНоменклатури_Objest();
            return ВидиНоменклатуриObjestItem.Read(base.UnigueID) ? ВидиНоменклатуриObjestItem : null;
        }
		
        public ВидиНоменклатури_Pointer GetNewDirectoryPointer()
        {
            return new ВидиНоменклатури_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_b8" }
			);
        }
		
        public ВидиНоменклатури_Pointer GetEmptyPointer()
        {
            return new ВидиНоменклатури_Pointer();
        }
    }
    
    
    public class ВидиНоменклатури_Select : DirectorySelect
    {
        public ВидиНоменклатури_Select() : base(Config.Kernel, "tab_a05") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВидиНоменклатури_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ВидиНоменклатури_Pointer Current { get; private set; }
        
        public ВидиНоменклатури_Pointer FindByField(string name, object value)
        {
            ВидиНоменклатури_Pointer itemPointer = new ВидиНоменклатури_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ВидиНоменклатури_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ВидиНоменклатури_Pointer> directoryPointerList = new List<ВидиНоменклатури_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ВидиНоменклатури_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ПакуванняОдиниціВиміру"
    
    public static class ПакуванняОдиниціВиміру_Const
    {
        public const string TABLE = "tab_a06";
        
        public const string Назва = "col_c1";
        public const string Код = "col_c2";
        public const string НазваПовна = "col_c3";
        public const string КількістьУпаковок = "col_c4";
    }
	
    
    public class ПакуванняОдиниціВиміру_Objest : DirectoryObject
    {
        public ПакуванняОдиниціВиміру_Objest() : base(Config.Kernel, "tab_a06",
             new string[] { "col_c1", "col_c2", "col_c3", "col_c4" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            КількістьУпаковок = 0;
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_c1"].ToString();
                Код = base.FieldValue["col_c2"].ToString();
                НазваПовна = base.FieldValue["col_c3"].ToString();
                КількістьУпаковок = (base.FieldValue["col_c4"] != DBNull.Value) ? (int)base.FieldValue["col_c4"] : 0;
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_c1"] = Назва;
            base.FieldValue["col_c2"] = Код;
            base.FieldValue["col_c3"] = НазваПовна;
            base.FieldValue["col_c4"] = КількістьУпаковок;
            
            BaseSave();
			
        }
		
        public ПакуванняОдиниціВиміру_Objest Copy()
        {
            ПакуванняОдиниціВиміру_Objest copy = new ПакуванняОдиниціВиміру_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.НазваПовна = НазваПовна;
			copy.КількістьУпаковок = КількістьУпаковок;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public ПакуванняОдиниціВиміру_Pointer GetDirectoryPointer()
        {
            ПакуванняОдиниціВиміру_Pointer directoryPointer = new ПакуванняОдиниціВиміру_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        public int КількістьУпаковок { get; set; }
        
    }
    
    
    public class ПакуванняОдиниціВиміру_Pointer : DirectoryPointer
    {
        public ПакуванняОдиниціВиміру_Pointer(object uid = null) : base(Config.Kernel, "tab_a06")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПакуванняОдиниціВиміру_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a06")
        {
            base.Init(uid, fields);
        }
        
        public ПакуванняОдиниціВиміру_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ПакуванняОдиниціВиміру_Objest ПакуванняОдиниціВиміруObjestItem = new ПакуванняОдиниціВиміру_Objest();
            return ПакуванняОдиниціВиміруObjestItem.Read(base.UnigueID) ? ПакуванняОдиниціВиміруObjestItem : null;
        }
		
        public ПакуванняОдиниціВиміру_Pointer GetNewDirectoryPointer()
        {
            return new ПакуванняОдиниціВиміру_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_c1" }
			);
        }
		
        public ПакуванняОдиниціВиміру_Pointer GetEmptyPointer()
        {
            return new ПакуванняОдиниціВиміру_Pointer();
        }
    }
    
    
    public class ПакуванняОдиниціВиміру_Select : DirectorySelect
    {
        public ПакуванняОдиниціВиміру_Select() : base(Config.Kernel, "tab_a06") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПакуванняОдиниціВиміру_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ПакуванняОдиниціВиміру_Pointer Current { get; private set; }
        
        public ПакуванняОдиниціВиміру_Pointer FindByField(string name, object value)
        {
            ПакуванняОдиниціВиміру_Pointer itemPointer = new ПакуванняОдиниціВиміру_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ПакуванняОдиниціВиміру_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ПакуванняОдиниціВиміру_Pointer> directoryPointerList = new List<ПакуванняОдиниціВиміру_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ПакуванняОдиниціВиміру_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Валюти"
    
    public static class Валюти_Const
    {
        public const string TABLE = "tab_a07";
        
        public const string Назва = "col_c5";
        public const string КороткаНазва = "col_a2";
        public const string Код = "col_c6";
        public const string Код_R030 = "col_a1";
    }
	
    
    public class Валюти_Objest : DirectoryObject
    {
        public Валюти_Objest() : base(Config.Kernel, "tab_a07",
             new string[] { "col_c5", "col_a2", "col_c6", "col_a1" }) 
        {
            Назва = "";
            КороткаНазва = "";
            Код = "";
            Код_R030 = "";
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_c5"].ToString();
                КороткаНазва = base.FieldValue["col_a2"].ToString();
                Код = base.FieldValue["col_c6"].ToString();
                Код_R030 = base.FieldValue["col_a1"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    Валюти_Triggers.BeforeRecording(this);
			base.FieldValue["col_c5"] = Назва;
            base.FieldValue["col_a2"] = КороткаНазва;
            base.FieldValue["col_c6"] = Код;
            base.FieldValue["col_a1"] = Код_R030;
            
            BaseSave();
			Валюти_Triggers.AfterRecording(this);
        }
		
        public Валюти_Objest Copy()
        {
            Валюти_Objest copy = new Валюти_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.КороткаНазва = КороткаНазва;
			copy.Код = Код;
			copy.Код_R030 = Код_R030;
			
			return copy;
        }

        public void Delete()
        {
            Валюти_Triggers.BeforeDelete(this);
			base.BaseDelete(new string[] {  });
        }
        
        public Валюти_Pointer GetDirectoryPointer()
        {
            Валюти_Pointer directoryPointer = new Валюти_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string КороткаНазва { get; set; }
        public string Код { get; set; }
        public string Код_R030 { get; set; }
        
    }
    
    
    public class Валюти_Pointer : DirectoryPointer
    {
        public Валюти_Pointer(object uid = null) : base(Config.Kernel, "tab_a07")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Валюти_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a07")
        {
            base.Init(uid, fields);
        }
        
        public Валюти_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Валюти_Objest ВалютиObjestItem = new Валюти_Objest();
            return ВалютиObjestItem.Read(base.UnigueID) ? ВалютиObjestItem : null;
        }
		
        public Валюти_Pointer GetNewDirectoryPointer()
        {
            return new Валюти_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_c5" }
			);
        }
		
        public Валюти_Pointer GetEmptyPointer()
        {
            return new Валюти_Pointer();
        }
    }
    
    
    public class Валюти_Select : DirectorySelect
    {
        public Валюти_Select() : base(Config.Kernel, "tab_a07") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Валюти_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Валюти_Pointer Current { get; private set; }
        
        public Валюти_Pointer FindByField(string name, object value)
        {
            Валюти_Pointer itemPointer = new Валюти_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Валюти_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Валюти_Pointer> directoryPointerList = new List<Валюти_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Валюти_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Контрагенти"
    
    public static class Контрагенти_Const
    {
        public const string TABLE = "tab_a08";
        
        public const string Назва = "col_c7";
        public const string Код = "col_c8";
        public const string НазваПовна = "col_c9";
        public const string РеєстраційнийНомер = "col_d1";
        public const string Папка = "col_a1";
        public const string Опис = "col_a2";
    }
	
    
    public class Контрагенти_Objest : DirectoryObject
    {
        public Контрагенти_Objest() : base(Config.Kernel, "tab_a08",
             new string[] { "col_c7", "col_c8", "col_c9", "col_d1", "col_a1", "col_a2" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            РеєстраційнийНомер = "";
            Папка = new Довідники.Контрагенти_Папки_Pointer();
            Опис = "";
            
            //Табличні частини
            Контакти_TablePart = new Контрагенти_Контакти_TablePart(this);
            Файли_TablePart = new Контрагенти_Файли_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_c7"].ToString();
                Код = base.FieldValue["col_c8"].ToString();
                НазваПовна = base.FieldValue["col_c9"].ToString();
                РеєстраційнийНомер = base.FieldValue["col_d1"].ToString();
                Папка = new Довідники.Контрагенти_Папки_Pointer(base.FieldValue["col_a1"]);
                Опис = base.FieldValue["col_a2"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    Контрагенти_Triggers.BeforeRecording(this);
			base.FieldValue["col_c7"] = Назва;
            base.FieldValue["col_c8"] = Код;
            base.FieldValue["col_c9"] = НазваПовна;
            base.FieldValue["col_d1"] = РеєстраційнийНомер;
            base.FieldValue["col_a1"] = Папка.UnigueID.UGuid;
            base.FieldValue["col_a2"] = Опис;
            
            BaseSave();
			Контрагенти_Triggers.AfterRecording(this);
        }
		
        public Контрагенти_Objest Copy()
        {
            Контрагенти_Objest copy = new Контрагенти_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.НазваПовна = НазваПовна;
			copy.РеєстраційнийНомер = РеєстраційнийНомер;
			copy.Папка = Папка;
			copy.Опис = Опис;
			
			return copy;
        }

        public void Delete()
        {
            Контрагенти_Triggers.BeforeDelete(this);
			base.BaseDelete(new string[] { "tab_a09", "tab_b20" });
        }
        
        public Контрагенти_Pointer GetDirectoryPointer()
        {
            Контрагенти_Pointer directoryPointer = new Контрагенти_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        public string РеєстраційнийНомер { get; set; }
        public Довідники.Контрагенти_Папки_Pointer Папка { get; set; }
        public string Опис { get; set; }
        
        //Табличні частини
        public Контрагенти_Контакти_TablePart Контакти_TablePart { get; set; }
        public Контрагенти_Файли_TablePart Файли_TablePart { get; set; }
        
    }
    
    
    public class Контрагенти_Pointer : DirectoryPointer
    {
        public Контрагенти_Pointer(object uid = null) : base(Config.Kernel, "tab_a08")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Контрагенти_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a08")
        {
            base.Init(uid, fields);
        }
        
        public Контрагенти_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Контрагенти_Objest КонтрагентиObjestItem = new Контрагенти_Objest();
            return КонтрагентиObjestItem.Read(base.UnigueID) ? КонтрагентиObjestItem : null;
        }
		
        public Контрагенти_Pointer GetNewDirectoryPointer()
        {
            return new Контрагенти_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_c7" }
			);
        }
		
        public Контрагенти_Pointer GetEmptyPointer()
        {
            return new Контрагенти_Pointer();
        }
    }
    
    
    public class Контрагенти_Select : DirectorySelect
    {
        public Контрагенти_Select() : base(Config.Kernel, "tab_a08") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Контрагенти_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Контрагенти_Pointer Current { get; private set; }
        
        public Контрагенти_Pointer FindByField(string name, object value)
        {
            Контрагенти_Pointer itemPointer = new Контрагенти_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Контрагенти_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Контрагенти_Pointer> directoryPointerList = new List<Контрагенти_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Контрагенти_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    public class Контрагенти_Контакти_TablePart : DirectoryTablePart
    {
        public Контрагенти_Контакти_TablePart(Контрагенти_Objest owner) : base(Config.Kernel, "tab_a09",
             new string[] { "col_d2", "col_d8", "col_d7", "col_d3", "col_d5", "col_d4", "col_d6" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string Тип = "col_d2";
        public const string Телефон = "col_d8";
        public const string ЕлектроннаПошта = "col_d7";
        public const string Країна = "col_d3";
        public const string Область = "col_d5";
        public const string Район = "col_d4";
        public const string Місто = "col_d6";

        public Контрагенти_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_d2"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_d2"] : 0;
                record.Телефон = fieldValue["col_d8"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_d7"].ToString();
                record.Країна = fieldValue["col_d3"].ToString();
                record.Область = fieldValue["col_d5"].ToString();
                record.Район = fieldValue["col_d4"].ToString();
                record.Місто = fieldValue["col_d6"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_d2", (int)record.Тип);
                fieldValue.Add("col_d8", record.Телефон);
                fieldValue.Add("col_d7", record.ЕлектроннаПошта);
                fieldValue.Add("col_d3", record.Країна);
                fieldValue.Add("col_d5", record.Область);
                fieldValue.Add("col_d4", record.Район);
                fieldValue.Add("col_d6", record.Місто);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Телефон = "";
                ЕлектроннаПошта = "";
                Країна = "";
                Область = "";
                Район = "";
                Місто = "";
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Телефон { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Країна { get; set; }
            public string Область { get; set; }
            public string Район { get; set; }
            public string Місто { get; set; }
            
        }
    }
      
    public class Контрагенти_Файли_TablePart : DirectoryTablePart
    {
        public Контрагенти_Файли_TablePart(Контрагенти_Objest owner) : base(Config.Kernel, "tab_b20",
             new string[] { "col_a1" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string Файл = "col_a1";

        public Контрагенти_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Файл = new Довідники.Файли_Pointer(fieldValue["col_a1"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.Файл.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Файл = new Довідники.Файли_Pointer();
                
            }
            public Довідники.Файли_Pointer Файл { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "Склади"
    
    public static class Склади_Const
    {
        public const string TABLE = "tab_a10";
        
        public const string Назва = "col_d9";
        public const string Код = "col_e1";
        public const string ТипСкладу = "col_a1";
        public const string Відповідальний = "col_a2";
        public const string ВидЦін = "col_a3";
        public const string Підрозділ = "col_a4";
        public const string Папка = "col_a5";
    }
	
    
    public class Склади_Objest : DirectoryObject
    {
        public Склади_Objest() : base(Config.Kernel, "tab_a10",
             new string[] { "col_d9", "col_e1", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5" }) 
        {
            Назва = "";
            Код = "";
            ТипСкладу = 0;
            Відповідальний = new Довідники.ФізичніОсоби_Pointer();
            ВидЦін = new Довідники.ВидиЦін_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Папка = new Довідники.Склади_Папки_Pointer();
            
            //Табличні частини
            Контакти_TablePart = new Склади_Контакти_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_d9"].ToString();
                Код = base.FieldValue["col_e1"].ToString();
                ТипСкладу = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.ТипиСкладів)base.FieldValue["col_a1"] : 0;
                Відповідальний = new Довідники.ФізичніОсоби_Pointer(base.FieldValue["col_a2"]);
                ВидЦін = new Довідники.ВидиЦін_Pointer(base.FieldValue["col_a3"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a4"]);
                Папка = new Довідники.Склади_Папки_Pointer(base.FieldValue["col_a5"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_d9"] = Назва;
            base.FieldValue["col_e1"] = Код;
            base.FieldValue["col_a1"] = (int)ТипСкладу;
            base.FieldValue["col_a2"] = Відповідальний.UnigueID.UGuid;
            base.FieldValue["col_a3"] = ВидЦін.UnigueID.UGuid;
            base.FieldValue["col_a4"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_a5"] = Папка.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public Склади_Objest Copy()
        {
            Склади_Objest copy = new Склади_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.ТипСкладу = ТипСкладу;
			copy.Відповідальний = Відповідальний;
			copy.ВидЦін = ВидЦін;
			copy.Підрозділ = Підрозділ;
			copy.Папка = Папка;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] { "tab_a11" });
        }
        
        public Склади_Pointer GetDirectoryPointer()
        {
            Склади_Pointer directoryPointer = new Склади_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Перелічення.ТипиСкладів ТипСкладу { get; set; }
        public Довідники.ФізичніОсоби_Pointer Відповідальний { get; set; }
        public Довідники.ВидиЦін_Pointer ВидЦін { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Папки_Pointer Папка { get; set; }
        
        //Табличні частини
        public Склади_Контакти_TablePart Контакти_TablePart { get; set; }
        
    }
    
    
    public class Склади_Pointer : DirectoryPointer
    {
        public Склади_Pointer(object uid = null) : base(Config.Kernel, "tab_a10")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Склади_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a10")
        {
            base.Init(uid, fields);
        }
        
        public Склади_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Склади_Objest СкладиObjestItem = new Склади_Objest();
            return СкладиObjestItem.Read(base.UnigueID) ? СкладиObjestItem : null;
        }
		
        public Склади_Pointer GetNewDirectoryPointer()
        {
            return new Склади_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_d9" }
			);
        }
		
        public Склади_Pointer GetEmptyPointer()
        {
            return new Склади_Pointer();
        }
    }
    
    
    public class Склади_Select : DirectorySelect
    {
        public Склади_Select() : base(Config.Kernel, "tab_a10") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Склади_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Склади_Pointer Current { get; private set; }
        
        public Склади_Pointer FindByField(string name, object value)
        {
            Склади_Pointer itemPointer = new Склади_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Склади_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Склади_Pointer> directoryPointerList = new List<Склади_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Склади_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    public class Склади_Контакти_TablePart : DirectoryTablePart
    {
        public Склади_Контакти_TablePart(Склади_Objest owner) : base(Config.Kernel, "tab_a11",
             new string[] { "col_e2", "col_e8", "col_e7", "col_e3", "col_e5", "col_e4", "col_e6" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string Тип = "col_e2";
        public const string Телефон = "col_e8";
        public const string ЕлектроннаПошта = "col_e7";
        public const string Країна = "col_e3";
        public const string Область = "col_e5";
        public const string Район = "col_e4";
        public const string Місто = "col_e6";

        public Склади_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_e2"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_e2"] : 0;
                record.Телефон = fieldValue["col_e8"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_e7"].ToString();
                record.Країна = fieldValue["col_e3"].ToString();
                record.Область = fieldValue["col_e5"].ToString();
                record.Район = fieldValue["col_e4"].ToString();
                record.Місто = fieldValue["col_e6"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_e2", (int)record.Тип);
                fieldValue.Add("col_e8", record.Телефон);
                fieldValue.Add("col_e7", record.ЕлектроннаПошта);
                fieldValue.Add("col_e3", record.Країна);
                fieldValue.Add("col_e5", record.Область);
                fieldValue.Add("col_e4", record.Район);
                fieldValue.Add("col_e6", record.Місто);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Телефон = "";
                ЕлектроннаПошта = "";
                Країна = "";
                Область = "";
                Район = "";
                Місто = "";
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Телефон { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Країна { get; set; }
            public string Область { get; set; }
            public string Район { get; set; }
            public string Місто { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "ВидиЦін"
    
    public static class ВидиЦін_Const
    {
        public const string TABLE = "tab_a12";
        
        public const string Назва = "col_e9";
        public const string Код = "col_f1";
        public const string Валюта = "col_f2";
    }
	
    
    public class ВидиЦін_Objest : DirectoryObject
    {
        public ВидиЦін_Objest() : base(Config.Kernel, "tab_a12",
             new string[] { "col_e9", "col_f1", "col_f2" }) 
        {
            Назва = "";
            Код = "";
            Валюта = new Довідники.Валюти_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_e9"].ToString();
                Код = base.FieldValue["col_f1"].ToString();
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_f2"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_e9"] = Назва;
            base.FieldValue["col_f1"] = Код;
            base.FieldValue["col_f2"] = Валюта.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public ВидиЦін_Objest Copy()
        {
            ВидиЦін_Objest copy = new ВидиЦін_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.Валюта = Валюта;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public ВидиЦін_Pointer GetDirectoryPointer()
        {
            ВидиЦін_Pointer directoryPointer = new ВидиЦін_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        
    }
    
    
    public class ВидиЦін_Pointer : DirectoryPointer
    {
        public ВидиЦін_Pointer(object uid = null) : base(Config.Kernel, "tab_a12")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВидиЦін_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a12")
        {
            base.Init(uid, fields);
        }
        
        public ВидиЦін_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ВидиЦін_Objest ВидиЦінObjestItem = new ВидиЦін_Objest();
            return ВидиЦінObjestItem.Read(base.UnigueID) ? ВидиЦінObjestItem : null;
        }
		
        public ВидиЦін_Pointer GetNewDirectoryPointer()
        {
            return new ВидиЦін_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_e9" }
			);
        }
		
        public ВидиЦін_Pointer GetEmptyPointer()
        {
            return new ВидиЦін_Pointer();
        }
    }
    
    
    public class ВидиЦін_Select : DirectorySelect
    {
        public ВидиЦін_Select() : base(Config.Kernel, "tab_a12") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВидиЦін_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ВидиЦін_Pointer Current { get; private set; }
        
        public ВидиЦін_Pointer FindByField(string name, object value)
        {
            ВидиЦін_Pointer itemPointer = new ВидиЦін_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ВидиЦін_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ВидиЦін_Pointer> directoryPointerList = new List<ВидиЦін_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ВидиЦін_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ВидиЦінПостачальників"
    
    public static class ВидиЦінПостачальників_Const
    {
        public const string TABLE = "tab_a13";
        
        public const string Назва = "col_f3";
        public const string Код = "col_f4";
        public const string Валюта = "col_f5";
    }
	
    
    public class ВидиЦінПостачальників_Objest : DirectoryObject
    {
        public ВидиЦінПостачальників_Objest() : base(Config.Kernel, "tab_a13",
             new string[] { "col_f3", "col_f4", "col_f5" }) 
        {
            Назва = "";
            Код = "";
            Валюта = new Довідники.Валюти_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_f3"].ToString();
                Код = base.FieldValue["col_f4"].ToString();
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_f5"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_f3"] = Назва;
            base.FieldValue["col_f4"] = Код;
            base.FieldValue["col_f5"] = Валюта.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public ВидиЦінПостачальників_Objest Copy()
        {
            ВидиЦінПостачальників_Objest copy = new ВидиЦінПостачальників_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.Валюта = Валюта;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public ВидиЦінПостачальників_Pointer GetDirectoryPointer()
        {
            ВидиЦінПостачальників_Pointer directoryPointer = new ВидиЦінПостачальників_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        
    }
    
    
    public class ВидиЦінПостачальників_Pointer : DirectoryPointer
    {
        public ВидиЦінПостачальників_Pointer(object uid = null) : base(Config.Kernel, "tab_a13")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВидиЦінПостачальників_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a13")
        {
            base.Init(uid, fields);
        }
        
        public ВидиЦінПостачальників_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ВидиЦінПостачальників_Objest ВидиЦінПостачальниківObjestItem = new ВидиЦінПостачальників_Objest();
            return ВидиЦінПостачальниківObjestItem.Read(base.UnigueID) ? ВидиЦінПостачальниківObjestItem : null;
        }
		
        public ВидиЦінПостачальників_Pointer GetNewDirectoryPointer()
        {
            return new ВидиЦінПостачальників_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_f3" }
			);
        }
		
        public ВидиЦінПостачальників_Pointer GetEmptyPointer()
        {
            return new ВидиЦінПостачальників_Pointer();
        }
    }
    
    
    public class ВидиЦінПостачальників_Select : DirectorySelect
    {
        public ВидиЦінПостачальників_Select() : base(Config.Kernel, "tab_a13") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВидиЦінПостачальників_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ВидиЦінПостачальників_Pointer Current { get; private set; }
        
        public ВидиЦінПостачальників_Pointer FindByField(string name, object value)
        {
            ВидиЦінПостачальників_Pointer itemPointer = new ВидиЦінПостачальників_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ВидиЦінПостачальників_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ВидиЦінПостачальників_Pointer> directoryPointerList = new List<ВидиЦінПостачальників_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ВидиЦінПостачальників_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Користувачі"
    
    public static class Користувачі_Const
    {
        public const string TABLE = "tab_a14";
        
        public const string Назва = "col_f6";
        public const string Код = "col_f7";
        public const string ФізичнаОсоба = "col_a1";
        public const string Коментар = "col_g6";
    }
	
    
    public class Користувачі_Objest : DirectoryObject
    {
        public Користувачі_Objest() : base(Config.Kernel, "tab_a14",
             new string[] { "col_f6", "col_f7", "col_a1", "col_g6" }) 
        {
            Назва = "";
            Код = "";
            ФізичнаОсоба = new Довідники.ФізичніОсоби_Pointer();
            Коментар = "";
            
            //Табличні частини
            Контакти_TablePart = new Користувачі_Контакти_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_f6"].ToString();
                Код = base.FieldValue["col_f7"].ToString();
                ФізичнаОсоба = new Довідники.ФізичніОсоби_Pointer(base.FieldValue["col_a1"]);
                Коментар = base.FieldValue["col_g6"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_f6"] = Назва;
            base.FieldValue["col_f7"] = Код;
            base.FieldValue["col_a1"] = ФізичнаОсоба.UnigueID.UGuid;
            base.FieldValue["col_g6"] = Коментар;
            
            BaseSave();
			
        }
		
        public Користувачі_Objest Copy()
        {
            Користувачі_Objest copy = new Користувачі_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.ФізичнаОсоба = ФізичнаОсоба;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] { "tab_a15" });
        }
        
        public Користувачі_Pointer GetDirectoryPointer()
        {
            Користувачі_Pointer directoryPointer = new Користувачі_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.ФізичніОсоби_Pointer ФізичнаОсоба { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public Користувачі_Контакти_TablePart Контакти_TablePart { get; set; }
        
    }
    
    
    public class Користувачі_Pointer : DirectoryPointer
    {
        public Користувачі_Pointer(object uid = null) : base(Config.Kernel, "tab_a14")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Користувачі_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a14")
        {
            base.Init(uid, fields);
        }
        
        public Користувачі_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Користувачі_Objest КористувачіObjestItem = new Користувачі_Objest();
            return КористувачіObjestItem.Read(base.UnigueID) ? КористувачіObjestItem : null;
        }
		
        public Користувачі_Pointer GetNewDirectoryPointer()
        {
            return new Користувачі_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_f6" }
			);
        }
		
        public Користувачі_Pointer GetEmptyPointer()
        {
            return new Користувачі_Pointer();
        }
    }
    
    
    public class Користувачі_Select : DirectorySelect
    {
        public Користувачі_Select() : base(Config.Kernel, "tab_a14") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Користувачі_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Користувачі_Pointer Current { get; private set; }
        
        public Користувачі_Pointer FindByField(string name, object value)
        {
            Користувачі_Pointer itemPointer = new Користувачі_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Користувачі_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Користувачі_Pointer> directoryPointerList = new List<Користувачі_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Користувачі_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    public class Користувачі_Контакти_TablePart : DirectoryTablePart
    {
        public Користувачі_Контакти_TablePart(Користувачі_Objest owner) : base(Config.Kernel, "tab_a15",
             new string[] { "col_f8", "col_g5", "col_g4", "col_f9", "col_g1", "col_g2", "col_g3" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string Тип = "col_f8";
        public const string Телефон = "col_g5";
        public const string ЕлектроннаПошта = "col_g4";
        public const string Країна = "col_f9";
        public const string Область = "col_g1";
        public const string Район = "col_g2";
        public const string Місто = "col_g3";

        public Користувачі_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_f8"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_f8"] : 0;
                record.Телефон = fieldValue["col_g5"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_g4"].ToString();
                record.Країна = fieldValue["col_f9"].ToString();
                record.Область = fieldValue["col_g1"].ToString();
                record.Район = fieldValue["col_g2"].ToString();
                record.Місто = fieldValue["col_g3"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_f8", (int)record.Тип);
                fieldValue.Add("col_g5", record.Телефон);
                fieldValue.Add("col_g4", record.ЕлектроннаПошта);
                fieldValue.Add("col_f9", record.Країна);
                fieldValue.Add("col_g1", record.Область);
                fieldValue.Add("col_g2", record.Район);
                fieldValue.Add("col_g3", record.Місто);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Телефон = "";
                ЕлектроннаПошта = "";
                Країна = "";
                Область = "";
                Район = "";
                Місто = "";
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Телефон { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Країна { get; set; }
            public string Область { get; set; }
            public string Район { get; set; }
            public string Місто { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "ФізичніОсоби"
    
    public static class ФізичніОсоби_Const
    {
        public const string TABLE = "tab_a16";
        
        public const string Назва = "col_g7";
        public const string Код = "col_g8";
        public const string ДатаНародження = "col_g9";
        public const string Стать = "col_a1";
        public const string ІПН = "col_a2";
    }
	
    
    public class ФізичніОсоби_Objest : DirectoryObject
    {
        public ФізичніОсоби_Objest() : base(Config.Kernel, "tab_a16",
             new string[] { "col_g7", "col_g8", "col_g9", "col_a1", "col_a2" }) 
        {
            Назва = "";
            Код = "";
            ДатаНародження = DateTime.MinValue;
            Стать = 0;
            ІПН = "";
            
            //Табличні частини
            Контакти_TablePart = new ФізичніОсоби_Контакти_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_g7"].ToString();
                Код = base.FieldValue["col_g8"].ToString();
                ДатаНародження = (base.FieldValue["col_g9"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_g9"].ToString()) : DateTime.MinValue;
                Стать = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.СтатьФізичноїОсоби)base.FieldValue["col_a1"] : 0;
                ІПН = base.FieldValue["col_a2"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_g7"] = Назва;
            base.FieldValue["col_g8"] = Код;
            base.FieldValue["col_g9"] = ДатаНародження;
            base.FieldValue["col_a1"] = (int)Стать;
            base.FieldValue["col_a2"] = ІПН;
            
            BaseSave();
			
        }
		
        public ФізичніОсоби_Objest Copy()
        {
            ФізичніОсоби_Objest copy = new ФізичніОсоби_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.ДатаНародження = ДатаНародження;
			copy.Стать = Стать;
			copy.ІПН = ІПН;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] { "tab_a17" });
        }
        
        public ФізичніОсоби_Pointer GetDirectoryPointer()
        {
            ФізичніОсоби_Pointer directoryPointer = new ФізичніОсоби_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public DateTime ДатаНародження { get; set; }
        public Перелічення.СтатьФізичноїОсоби Стать { get; set; }
        public string ІПН { get; set; }
        
        //Табличні частини
        public ФізичніОсоби_Контакти_TablePart Контакти_TablePart { get; set; }
        
    }
    
    
    public class ФізичніОсоби_Pointer : DirectoryPointer
    {
        public ФізичніОсоби_Pointer(object uid = null) : base(Config.Kernel, "tab_a16")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ФізичніОсоби_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a16")
        {
            base.Init(uid, fields);
        }
        
        public ФізичніОсоби_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ФізичніОсоби_Objest ФізичніОсобиObjestItem = new ФізичніОсоби_Objest();
            return ФізичніОсобиObjestItem.Read(base.UnigueID) ? ФізичніОсобиObjestItem : null;
        }
		
        public ФізичніОсоби_Pointer GetNewDirectoryPointer()
        {
            return new ФізичніОсоби_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_g7" }
			);
        }
		
        public ФізичніОсоби_Pointer GetEmptyPointer()
        {
            return new ФізичніОсоби_Pointer();
        }
    }
    
    
    public class ФізичніОсоби_Select : DirectorySelect
    {
        public ФізичніОсоби_Select() : base(Config.Kernel, "tab_a16") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ФізичніОсоби_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ФізичніОсоби_Pointer Current { get; private set; }
        
        public ФізичніОсоби_Pointer FindByField(string name, object value)
        {
            ФізичніОсоби_Pointer itemPointer = new ФізичніОсоби_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ФізичніОсоби_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ФізичніОсоби_Pointer> directoryPointerList = new List<ФізичніОсоби_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ФізичніОсоби_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    public class ФізичніОсоби_Контакти_TablePart : DirectoryTablePart
    {
        public ФізичніОсоби_Контакти_TablePart(ФізичніОсоби_Objest owner) : base(Config.Kernel, "tab_a17",
             new string[] { "col_h1", "col_h7", "col_h6", "col_h2", "col_h3", "col_h4", "col_h5" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string Тип = "col_h1";
        public const string Телефон = "col_h7";
        public const string ЕлектроннаПошта = "col_h6";
        public const string Країна = "col_h2";
        public const string Область = "col_h3";
        public const string Район = "col_h4";
        public const string Місто = "col_h5";

        public ФізичніОсоби_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Тип = (fieldValue["col_h1"] != DBNull.Value) ? (Перелічення.ТипиКонтактноїІнформації)fieldValue["col_h1"] : 0;
                record.Телефон = fieldValue["col_h7"].ToString();
                record.ЕлектроннаПошта = fieldValue["col_h6"].ToString();
                record.Країна = fieldValue["col_h2"].ToString();
                record.Область = fieldValue["col_h3"].ToString();
                record.Район = fieldValue["col_h4"].ToString();
                record.Місто = fieldValue["col_h5"].ToString();
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_h1", (int)record.Тип);
                fieldValue.Add("col_h7", record.Телефон);
                fieldValue.Add("col_h6", record.ЕлектроннаПошта);
                fieldValue.Add("col_h2", record.Країна);
                fieldValue.Add("col_h3", record.Область);
                fieldValue.Add("col_h4", record.Район);
                fieldValue.Add("col_h5", record.Місто);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                Тип = 0;
                Телефон = "";
                ЕлектроннаПошта = "";
                Країна = "";
                Область = "";
                Район = "";
                Місто = "";
                
            }
            public Перелічення.ТипиКонтактноїІнформації Тип { get; set; }
            public string Телефон { get; set; }
            public string ЕлектроннаПошта { get; set; }
            public string Країна { get; set; }
            public string Область { get; set; }
            public string Район { get; set; }
            public string Місто { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "СтруктураПідприємства"
    
    public static class СтруктураПідприємства_Const
    {
        public const string TABLE = "tab_a18";
        
        public const string Назва = "col_h8";
        public const string Код = "col_h9";
        public const string Керівник = "col_i1";
    }
	
    
    public class СтруктураПідприємства_Objest : DirectoryObject
    {
        public СтруктураПідприємства_Objest() : base(Config.Kernel, "tab_a18",
             new string[] { "col_h8", "col_h9", "col_i1" }) 
        {
            Назва = "";
            Код = "";
            Керівник = new Довідники.ФізичніОсоби_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_h8"].ToString();
                Код = base.FieldValue["col_h9"].ToString();
                Керівник = new Довідники.ФізичніОсоби_Pointer(base.FieldValue["col_i1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_h8"] = Назва;
            base.FieldValue["col_h9"] = Код;
            base.FieldValue["col_i1"] = Керівник.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public СтруктураПідприємства_Objest Copy()
        {
            СтруктураПідприємства_Objest copy = new СтруктураПідприємства_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.Керівник = Керівник;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public СтруктураПідприємства_Pointer GetDirectoryPointer()
        {
            СтруктураПідприємства_Pointer directoryPointer = new СтруктураПідприємства_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.ФізичніОсоби_Pointer Керівник { get; set; }
        
    }
    
    
    public class СтруктураПідприємства_Pointer : DirectoryPointer
    {
        public СтруктураПідприємства_Pointer(object uid = null) : base(Config.Kernel, "tab_a18")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public СтруктураПідприємства_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a18")
        {
            base.Init(uid, fields);
        }
        
        public СтруктураПідприємства_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            СтруктураПідприємства_Objest СтруктураПідприємстваObjestItem = new СтруктураПідприємства_Objest();
            return СтруктураПідприємстваObjestItem.Read(base.UnigueID) ? СтруктураПідприємстваObjestItem : null;
        }
		
        public СтруктураПідприємства_Pointer GetNewDirectoryPointer()
        {
            return new СтруктураПідприємства_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_h8" }
			);
        }
		
        public СтруктураПідприємства_Pointer GetEmptyPointer()
        {
            return new СтруктураПідприємства_Pointer();
        }
    }
    
    
    public class СтруктураПідприємства_Select : DirectorySelect
    {
        public СтруктураПідприємства_Select() : base(Config.Kernel, "tab_a18") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new СтруктураПідприємства_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public СтруктураПідприємства_Pointer Current { get; private set; }
        
        public СтруктураПідприємства_Pointer FindByField(string name, object value)
        {
            СтруктураПідприємства_Pointer itemPointer = new СтруктураПідприємства_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<СтруктураПідприємства_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<СтруктураПідприємства_Pointer> directoryPointerList = new List<СтруктураПідприємства_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new СтруктураПідприємства_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "КраїниСвіту"
    
    public static class КраїниСвіту_Const
    {
        public const string TABLE = "tab_a19";
        
        public const string Назва = "col_i2";
        public const string Код = "col_i3";
        public const string НазваПовна = "col_i4";
    }
	
    
    public class КраїниСвіту_Objest : DirectoryObject
    {
        public КраїниСвіту_Objest() : base(Config.Kernel, "tab_a19",
             new string[] { "col_i2", "col_i3", "col_i4" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_i2"].ToString();
                Код = base.FieldValue["col_i3"].ToString();
                НазваПовна = base.FieldValue["col_i4"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_i2"] = Назва;
            base.FieldValue["col_i3"] = Код;
            base.FieldValue["col_i4"] = НазваПовна;
            
            BaseSave();
			
        }
		
        public КраїниСвіту_Objest Copy()
        {
            КраїниСвіту_Objest copy = new КраїниСвіту_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.НазваПовна = НазваПовна;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public КраїниСвіту_Pointer GetDirectoryPointer()
        {
            КраїниСвіту_Pointer directoryPointer = new КраїниСвіту_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        
    }
    
    
    public class КраїниСвіту_Pointer : DirectoryPointer
    {
        public КраїниСвіту_Pointer(object uid = null) : base(Config.Kernel, "tab_a19")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public КраїниСвіту_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a19")
        {
            base.Init(uid, fields);
        }
        
        public КраїниСвіту_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            КраїниСвіту_Objest КраїниСвітуObjestItem = new КраїниСвіту_Objest();
            return КраїниСвітуObjestItem.Read(base.UnigueID) ? КраїниСвітуObjestItem : null;
        }
		
        public КраїниСвіту_Pointer GetNewDirectoryPointer()
        {
            return new КраїниСвіту_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_i2" }
			);
        }
		
        public КраїниСвіту_Pointer GetEmptyPointer()
        {
            return new КраїниСвіту_Pointer();
        }
    }
    
    
    public class КраїниСвіту_Select : DirectorySelect
    {
        public КраїниСвіту_Select() : base(Config.Kernel, "tab_a19") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new КраїниСвіту_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public КраїниСвіту_Pointer Current { get; private set; }
        
        public КраїниСвіту_Pointer FindByField(string name, object value)
        {
            КраїниСвіту_Pointer itemPointer = new КраїниСвіту_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<КраїниСвіту_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<КраїниСвіту_Pointer> directoryPointerList = new List<КраїниСвіту_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new КраїниСвіту_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Файли"
    
    public static class Файли_Const
    {
        public const string TABLE = "tab_a20";
        
        public const string Код = "col_i6";
        public const string Назва = "col_i5";
        public const string НазваФайлу = "col_a2";
        public const string БінарніДані = "col_a1";
        public const string Розмір = "col_a3";
        public const string ДатаСтворення = "col_a4";
    }
	
    
    public class Файли_Objest : DirectoryObject
    {
        public Файли_Objest() : base(Config.Kernel, "tab_a20",
             new string[] { "col_i6", "col_i5", "col_a2", "col_a1", "col_a3", "col_a4" }) 
        {
            Код = "";
            Назва = "";
            НазваФайлу = "";
            БінарніДані = new byte[] { };
            Розмір = "";
            ДатаСтворення = DateTime.MinValue;
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Код = base.FieldValue["col_i6"].ToString();
                Назва = base.FieldValue["col_i5"].ToString();
                НазваФайлу = base.FieldValue["col_a2"].ToString();
                БінарніДані = (base.FieldValue["col_a1"] != DBNull.Value) ? (byte[])base.FieldValue["col_a1"] : new byte[] { };
                Розмір = base.FieldValue["col_a3"].ToString();
                ДатаСтворення = (base.FieldValue["col_a4"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a4"].ToString()) : DateTime.MinValue;
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_i6"] = Код;
            base.FieldValue["col_i5"] = Назва;
            base.FieldValue["col_a2"] = НазваФайлу;
            base.FieldValue["col_a1"] = БінарніДані;
            base.FieldValue["col_a3"] = Розмір;
            base.FieldValue["col_a4"] = ДатаСтворення;
            
            BaseSave();
			
        }
		
        public Файли_Objest Copy()
        {
            Файли_Objest copy = new Файли_Objest();
			copy.New();
            copy.Код = Код;
			copy.Назва = Назва;
			copy.НазваФайлу = НазваФайлу;
			copy.БінарніДані = БінарніДані;
			copy.Розмір = Розмір;
			copy.ДатаСтворення = ДатаСтворення;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public Файли_Pointer GetDirectoryPointer()
        {
            Файли_Pointer directoryPointer = new Файли_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Код { get; set; }
        public string Назва { get; set; }
        public string НазваФайлу { get; set; }
        public byte[] БінарніДані { get; set; }
        public string Розмір { get; set; }
        public DateTime ДатаСтворення { get; set; }
        
    }
    
    
    public class Файли_Pointer : DirectoryPointer
    {
        public Файли_Pointer(object uid = null) : base(Config.Kernel, "tab_a20")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Файли_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a20")
        {
            base.Init(uid, fields);
        }
        
        public Файли_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Файли_Objest ФайлиObjestItem = new Файли_Objest();
            return ФайлиObjestItem.Read(base.UnigueID) ? ФайлиObjestItem : null;
        }
		
        public Файли_Pointer GetNewDirectoryPointer()
        {
            return new Файли_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_i5" }
			);
        }
		
        public Файли_Pointer GetEmptyPointer()
        {
            return new Файли_Pointer();
        }
    }
    
    
    public class Файли_Select : DirectorySelect
    {
        public Файли_Select() : base(Config.Kernel, "tab_a20") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Файли_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Файли_Pointer Current { get; private set; }
        
        public Файли_Pointer FindByField(string name, object value)
        {
            Файли_Pointer itemPointer = new Файли_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Файли_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Файли_Pointer> directoryPointerList = new List<Файли_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Файли_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ХарактеристикиНоменклатури"
    
    public static class ХарактеристикиНоменклатури_Const
    {
        public const string TABLE = "tab_a21";
        
        public const string Назва = "col_i7";
        public const string Код = "col_i8";
        public const string НазваПовна = "col_i9";
        public const string Номенклатура = "col_a1";
    }
	
    
    public class ХарактеристикиНоменклатури_Objest : DirectoryObject
    {
        public ХарактеристикиНоменклатури_Objest() : base(Config.Kernel, "tab_a21",
             new string[] { "col_i7", "col_i8", "col_i9", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            НазваПовна = "";
            Номенклатура = new Довідники.Номенклатура_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_i7"].ToString();
                Код = base.FieldValue["col_i8"].ToString();
                НазваПовна = base.FieldValue["col_i9"].ToString();
                Номенклатура = new Довідники.Номенклатура_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_i7"] = Назва;
            base.FieldValue["col_i8"] = Код;
            base.FieldValue["col_i9"] = НазваПовна;
            base.FieldValue["col_a1"] = Номенклатура.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public ХарактеристикиНоменклатури_Objest Copy()
        {
            ХарактеристикиНоменклатури_Objest copy = new ХарактеристикиНоменклатури_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.НазваПовна = НазваПовна;
			copy.Номенклатура = Номенклатура;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public ХарактеристикиНоменклатури_Pointer GetDirectoryPointer()
        {
            ХарактеристикиНоменклатури_Pointer directoryPointer = new ХарактеристикиНоменклатури_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НазваПовна { get; set; }
        public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
        
    }
    
    
    public class ХарактеристикиНоменклатури_Pointer : DirectoryPointer
    {
        public ХарактеристикиНоменклатури_Pointer(object uid = null) : base(Config.Kernel, "tab_a21")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ХарактеристикиНоменклатури_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a21")
        {
            base.Init(uid, fields);
        }
        
        public ХарактеристикиНоменклатури_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатуриObjestItem = new ХарактеристикиНоменклатури_Objest();
            return ХарактеристикиНоменклатуриObjestItem.Read(base.UnigueID) ? ХарактеристикиНоменклатуриObjestItem : null;
        }
		
        public ХарактеристикиНоменклатури_Pointer GetNewDirectoryPointer()
        {
            return new ХарактеристикиНоменклатури_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_i7" }
			);
        }
		
        public ХарактеристикиНоменклатури_Pointer GetEmptyPointer()
        {
            return new ХарактеристикиНоменклатури_Pointer();
        }
    }
    
    
    public class ХарактеристикиНоменклатури_Select : DirectorySelect
    {
        public ХарактеристикиНоменклатури_Select() : base(Config.Kernel, "tab_a21") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ХарактеристикиНоменклатури_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ХарактеристикиНоменклатури_Pointer Current { get; private set; }
        
        public ХарактеристикиНоменклатури_Pointer FindByField(string name, object value)
        {
            ХарактеристикиНоменклатури_Pointer itemPointer = new ХарактеристикиНоменклатури_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ХарактеристикиНоменклатури_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ХарактеристикиНоменклатури_Pointer> directoryPointerList = new List<ХарактеристикиНоменклатури_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ХарактеристикиНоменклатури_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Номенклатура_Папки"
    
    public static class Номенклатура_Папки_Const
    {
        public const string TABLE = "tab_a22";
        
        public const string Назва = "col_j1";
        public const string Код = "col_j2";
        public const string Родич = "col_j3";
    }
	
    
    public class Номенклатура_Папки_Objest : DirectoryObject
    {
        public Номенклатура_Папки_Objest() : base(Config.Kernel, "tab_a22",
             new string[] { "col_j1", "col_j2", "col_j3" }) 
        {
            Назва = "";
            Код = "";
            Родич = new Довідники.Номенклатура_Папки_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_j1"].ToString();
                Код = base.FieldValue["col_j2"].ToString();
                Родич = new Довідники.Номенклатура_Папки_Pointer(base.FieldValue["col_j3"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_j1"] = Назва;
            base.FieldValue["col_j2"] = Код;
            base.FieldValue["col_j3"] = Родич.UnigueID.UGuid;
            
            BaseSave();
			Номенклатура_Папки_Triggers.AfterRecording(this);
        }
		
        public Номенклатура_Папки_Objest Copy()
        {
            Номенклатура_Папки_Objest copy = new Номенклатура_Папки_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.Родич = Родич;
			
			return copy;
        }

        public void Delete()
        {
            Номенклатура_Папки_Triggers.BeforeDelete(this);
			base.BaseDelete(new string[] {  });
        }
        
        public Номенклатура_Папки_Pointer GetDirectoryPointer()
        {
            Номенклатура_Папки_Pointer directoryPointer = new Номенклатура_Папки_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Номенклатура_Папки_Pointer Родич { get; set; }
        
    }
    
    
    public class Номенклатура_Папки_Pointer : DirectoryPointer
    {
        public Номенклатура_Папки_Pointer(object uid = null) : base(Config.Kernel, "tab_a22")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Номенклатура_Папки_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a22")
        {
            base.Init(uid, fields);
        }
        
        public Номенклатура_Папки_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Номенклатура_Папки_Objest Номенклатура_ПапкиObjestItem = new Номенклатура_Папки_Objest();
            return Номенклатура_ПапкиObjestItem.Read(base.UnigueID) ? Номенклатура_ПапкиObjestItem : null;
        }
		
        public Номенклатура_Папки_Pointer GetNewDirectoryPointer()
        {
            return new Номенклатура_Папки_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_j1" }
			);
        }
		
        public Номенклатура_Папки_Pointer GetEmptyPointer()
        {
            return new Номенклатура_Папки_Pointer();
        }
    }
    
    
    public class Номенклатура_Папки_Select : DirectorySelect
    {
        public Номенклатура_Папки_Select() : base(Config.Kernel, "tab_a22") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Номенклатура_Папки_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Номенклатура_Папки_Pointer Current { get; private set; }
        
        public Номенклатура_Папки_Pointer FindByField(string name, object value)
        {
            Номенклатура_Папки_Pointer itemPointer = new Номенклатура_Папки_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Номенклатура_Папки_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Номенклатура_Папки_Pointer> directoryPointerList = new List<Номенклатура_Папки_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Номенклатура_Папки_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Контрагенти_Папки"
    
    public static class Контрагенти_Папки_Const
    {
        public const string TABLE = "tab_a23";
        
        public const string Назва = "col_j4";
        public const string Код = "col_j5";
        public const string Родич = "col_j6";
    }
	
    
    public class Контрагенти_Папки_Objest : DirectoryObject
    {
        public Контрагенти_Папки_Objest() : base(Config.Kernel, "tab_a23",
             new string[] { "col_j4", "col_j5", "col_j6" }) 
        {
            Назва = "";
            Код = "";
            Родич = new Довідники.Контрагенти_Папки_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_j4"].ToString();
                Код = base.FieldValue["col_j5"].ToString();
                Родич = new Довідники.Контрагенти_Папки_Pointer(base.FieldValue["col_j6"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_j4"] = Назва;
            base.FieldValue["col_j5"] = Код;
            base.FieldValue["col_j6"] = Родич.UnigueID.UGuid;
            
            BaseSave();
			Контрагенти_Папки_Triggers.AfterRecording(this);
        }
		
        public Контрагенти_Папки_Objest Copy()
        {
            Контрагенти_Папки_Objest copy = new Контрагенти_Папки_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.Родич = Родич;
			
			return copy;
        }

        public void Delete()
        {
            Контрагенти_Папки_Triggers.BeforeDelete(this);
			base.BaseDelete(new string[] {  });
        }
        
        public Контрагенти_Папки_Pointer GetDirectoryPointer()
        {
            Контрагенти_Папки_Pointer directoryPointer = new Контрагенти_Папки_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Контрагенти_Папки_Pointer Родич { get; set; }
        
    }
    
    
    public class Контрагенти_Папки_Pointer : DirectoryPointer
    {
        public Контрагенти_Папки_Pointer(object uid = null) : base(Config.Kernel, "tab_a23")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Контрагенти_Папки_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a23")
        {
            base.Init(uid, fields);
        }
        
        public Контрагенти_Папки_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Контрагенти_Папки_Objest Контрагенти_ПапкиObjestItem = new Контрагенти_Папки_Objest();
            return Контрагенти_ПапкиObjestItem.Read(base.UnigueID) ? Контрагенти_ПапкиObjestItem : null;
        }
		
        public Контрагенти_Папки_Pointer GetNewDirectoryPointer()
        {
            return new Контрагенти_Папки_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_j4" }
			);
        }
		
        public Контрагенти_Папки_Pointer GetEmptyPointer()
        {
            return new Контрагенти_Папки_Pointer();
        }
    }
    
    
    public class Контрагенти_Папки_Select : DirectorySelect
    {
        public Контрагенти_Папки_Select() : base(Config.Kernel, "tab_a23") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Контрагенти_Папки_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Контрагенти_Папки_Pointer Current { get; private set; }
        
        public Контрагенти_Папки_Pointer FindByField(string name, object value)
        {
            Контрагенти_Папки_Pointer itemPointer = new Контрагенти_Папки_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Контрагенти_Папки_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Контрагенти_Папки_Pointer> directoryPointerList = new List<Контрагенти_Папки_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Контрагенти_Папки_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Склади_Папки"
    
    public static class Склади_Папки_Const
    {
        public const string TABLE = "tab_a24";
        
        public const string Назва = "col_j7";
        public const string Код = "col_j8";
        public const string Родич = "col_a1";
    }
	
    
    public class Склади_Папки_Objest : DirectoryObject
    {
        public Склади_Папки_Objest() : base(Config.Kernel, "tab_a24",
             new string[] { "col_j7", "col_j8", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            Родич = new Довідники.Склади_Папки_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_j7"].ToString();
                Код = base.FieldValue["col_j8"].ToString();
                Родич = new Довідники.Склади_Папки_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_j7"] = Назва;
            base.FieldValue["col_j8"] = Код;
            base.FieldValue["col_a1"] = Родич.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public Склади_Папки_Objest Copy()
        {
            Склади_Папки_Objest copy = new Склади_Папки_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.Родич = Родич;
			
			return copy;
        }

        public void Delete()
        {
            Склади_Папки_Triggers.BeforeDelete(this);
			base.BaseDelete(new string[] {  });
        }
        
        public Склади_Папки_Pointer GetDirectoryPointer()
        {
            Склади_Папки_Pointer directoryPointer = new Склади_Папки_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Склади_Папки_Pointer Родич { get; set; }
        
    }
    
    
    public class Склади_Папки_Pointer : DirectoryPointer
    {
        public Склади_Папки_Pointer(object uid = null) : base(Config.Kernel, "tab_a24")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Склади_Папки_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a24")
        {
            base.Init(uid, fields);
        }
        
        public Склади_Папки_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Склади_Папки_Objest Склади_ПапкиObjestItem = new Склади_Папки_Objest();
            return Склади_ПапкиObjestItem.Read(base.UnigueID) ? Склади_ПапкиObjestItem : null;
        }
		
        public Склади_Папки_Pointer GetNewDirectoryPointer()
        {
            return new Склади_Папки_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_j7" }
			);
        }
		
        public Склади_Папки_Pointer GetEmptyPointer()
        {
            return new Склади_Папки_Pointer();
        }
    }
    
    
    public class Склади_Папки_Select : DirectorySelect
    {
        public Склади_Папки_Select() : base(Config.Kernel, "tab_a24") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Склади_Папки_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Склади_Папки_Pointer Current { get; private set; }
        
        public Склади_Папки_Pointer FindByField(string name, object value)
        {
            Склади_Папки_Pointer itemPointer = new Склади_Папки_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Склади_Папки_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Склади_Папки_Pointer> directoryPointerList = new List<Склади_Папки_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Склади_Папки_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "Каси"
    
    public static class Каси_Const
    {
        public const string TABLE = "tab_a26";
        
        public const string Назва = "col_k8";
        public const string Код = "col_k9";
        public const string Валюта = "col_a2";
        public const string Підрозділ = "col_a1";
    }
	
    
    public class Каси_Objest : DirectoryObject
    {
        public Каси_Objest() : base(Config.Kernel, "tab_a26",
             new string[] { "col_k8", "col_k9", "col_a2", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            Валюта = new Довідники.Валюти_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_k8"].ToString();
                Код = base.FieldValue["col_k9"].ToString();
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_a2"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_k8"] = Назва;
            base.FieldValue["col_k9"] = Код;
            base.FieldValue["col_a2"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_a1"] = Підрозділ.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public Каси_Objest Copy()
        {
            Каси_Objest copy = new Каси_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.Валюта = Валюта;
			copy.Підрозділ = Підрозділ;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public Каси_Pointer GetDirectoryPointer()
        {
            Каси_Pointer directoryPointer = new Каси_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        
    }
    
    
    public class Каси_Pointer : DirectoryPointer
    {
        public Каси_Pointer(object uid = null) : base(Config.Kernel, "tab_a26")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public Каси_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a26")
        {
            base.Init(uid, fields);
        }
        
        public Каси_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            Каси_Objest КасиObjestItem = new Каси_Objest();
            return КасиObjestItem.Read(base.UnigueID) ? КасиObjestItem : null;
        }
		
        public Каси_Pointer GetNewDirectoryPointer()
        {
            return new Каси_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_k8" }
			);
        }
		
        public Каси_Pointer GetEmptyPointer()
        {
            return new Каси_Pointer();
        }
    }
    
    
    public class Каси_Select : DirectorySelect
    {
        public Каси_Select() : base(Config.Kernel, "tab_a26") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new Каси_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public Каси_Pointer Current { get; private set; }
        
        public Каси_Pointer FindByField(string name, object value)
        {
            Каси_Pointer itemPointer = new Каси_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<Каси_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<Каси_Pointer> directoryPointerList = new List<Каси_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new Каси_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "БанківськіРахункиОрганізацій"
    
    public static class БанківськіРахункиОрганізацій_Const
    {
        public const string TABLE = "tab_a27";
        
        public const string Назва = "col_l1";
        public const string Код = "col_l2";
        public const string Валюта = "col_l3";
        public const string Банк = "col_l4";
        public const string Підрозділ = "col_l5";
        public const string НазваБанку = "col_l6";
        public const string НомерРахунку = "col_l7";
        public const string АдресаБанку = "col_l8";
        public const string МістоБанку = "col_l9";
        public const string КореспонденськийРахунокБанку = "col_n1";
        public const string ТелефониБанку = "col_n2";
        public const string Закритий = "col_n3";
        public const string Організація = "col_a1";
    }
	
    
    public class БанківськіРахункиОрганізацій_Objest : DirectoryObject
    {
        public БанківськіРахункиОрганізацій_Objest() : base(Config.Kernel, "tab_a27",
             new string[] { "col_l1", "col_l2", "col_l3", "col_l4", "col_l5", "col_l6", "col_l7", "col_l8", "col_l9", "col_n1", "col_n2", "col_n3", "col_a1" }) 
        {
            Назва = "";
            Код = "";
            Валюта = new Довідники.Валюти_Pointer();
            Банк = "";
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            НазваБанку = "";
            НомерРахунку = "";
            АдресаБанку = "";
            МістоБанку = "";
            КореспонденськийРахунокБанку = "";
            ТелефониБанку = "";
            Закритий = false;
            Організація = new Довідники.Організації_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_l1"].ToString();
                Код = base.FieldValue["col_l2"].ToString();
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_l3"]);
                Банк = base.FieldValue["col_l4"].ToString();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_l5"]);
                НазваБанку = base.FieldValue["col_l6"].ToString();
                НомерРахунку = base.FieldValue["col_l7"].ToString();
                АдресаБанку = base.FieldValue["col_l8"].ToString();
                МістоБанку = base.FieldValue["col_l9"].ToString();
                КореспонденськийРахунокБанку = base.FieldValue["col_n1"].ToString();
                ТелефониБанку = base.FieldValue["col_n2"].ToString();
                Закритий = (base.FieldValue["col_n3"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_n3"].ToString()) : false;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_l1"] = Назва;
            base.FieldValue["col_l2"] = Код;
            base.FieldValue["col_l3"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_l4"] = Банк;
            base.FieldValue["col_l5"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_l6"] = НазваБанку;
            base.FieldValue["col_l7"] = НомерРахунку;
            base.FieldValue["col_l8"] = АдресаБанку;
            base.FieldValue["col_l9"] = МістоБанку;
            base.FieldValue["col_n1"] = КореспонденськийРахунокБанку;
            base.FieldValue["col_n2"] = ТелефониБанку;
            base.FieldValue["col_n3"] = Закритий;
            base.FieldValue["col_a1"] = Організація.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public БанківськіРахункиОрганізацій_Objest Copy()
        {
            БанківськіРахункиОрганізацій_Objest copy = new БанківськіРахункиОрганізацій_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.Валюта = Валюта;
			copy.Банк = Банк;
			copy.Підрозділ = Підрозділ;
			copy.НазваБанку = НазваБанку;
			copy.НомерРахунку = НомерРахунку;
			copy.АдресаБанку = АдресаБанку;
			copy.МістоБанку = МістоБанку;
			copy.КореспонденськийРахунокБанку = КореспонденськийРахунокБанку;
			copy.ТелефониБанку = ТелефониБанку;
			copy.Закритий = Закритий;
			copy.Організація = Організація;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public БанківськіРахункиОрганізацій_Pointer GetDirectoryPointer()
        {
            БанківськіРахункиОрганізацій_Pointer directoryPointer = new БанківськіРахункиОрганізацій_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public string Банк { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public string НазваБанку { get; set; }
        public string НомерРахунку { get; set; }
        public string АдресаБанку { get; set; }
        public string МістоБанку { get; set; }
        public string КореспонденськийРахунокБанку { get; set; }
        public string ТелефониБанку { get; set; }
        public bool Закритий { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        
    }
    
    
    public class БанківськіРахункиОрганізацій_Pointer : DirectoryPointer
    {
        public БанківськіРахункиОрганізацій_Pointer(object uid = null) : base(Config.Kernel, "tab_a27")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public БанківськіРахункиОрганізацій_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a27")
        {
            base.Init(uid, fields);
        }
        
        public БанківськіРахункиОрганізацій_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізаційObjestItem = new БанківськіРахункиОрганізацій_Objest();
            return БанківськіРахункиОрганізаційObjestItem.Read(base.UnigueID) ? БанківськіРахункиОрганізаційObjestItem : null;
        }
		
        public БанківськіРахункиОрганізацій_Pointer GetNewDirectoryPointer()
        {
            return new БанківськіРахункиОрганізацій_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_l1" }
			);
        }
		
        public БанківськіРахункиОрганізацій_Pointer GetEmptyPointer()
        {
            return new БанківськіРахункиОрганізацій_Pointer();
        }
    }
    
    
    public class БанківськіРахункиОрганізацій_Select : DirectorySelect
    {
        public БанківськіРахункиОрганізацій_Select() : base(Config.Kernel, "tab_a27") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new БанківськіРахункиОрганізацій_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public БанківськіРахункиОрганізацій_Pointer Current { get; private set; }
        
        public БанківськіРахункиОрганізацій_Pointer FindByField(string name, object value)
        {
            БанківськіРахункиОрганізацій_Pointer itemPointer = new БанківськіРахункиОрганізацій_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<БанківськіРахункиОрганізацій_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<БанківськіРахункиОрганізацій_Pointer> directoryPointerList = new List<БанківськіРахункиОрганізацій_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new БанківськіРахункиОрганізацій_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ДоговориКонтрагентів"
    
    public static class ДоговориКонтрагентів_Const
    {
        public const string TABLE = "tab_a28";
        
        public const string Назва = "col_n4";
        public const string Код = "col_n5";
        public const string БанківськийРахунок = "col_n6";
        public const string БанківськийРахунокКонтрагента = "col_a1";
        public const string ВалютаВзаєморозрахунків = "col_a2";
        public const string ДатаПочаткуДії = "col_a4";
        public const string ДатаЗакінченняДії = "col_a5";
        public const string Організація = "col_a6";
        public const string Контрагент = "col_a7";
        public const string Дата = "col_a8";
        public const string Номер = "col_a9";
        public const string Підрозділ = "col_b1";
        public const string Узгоджений = "col_b2";
        public const string Статус = "col_b3";
        public const string ГосподарськаОперація = "col_b4";
        public const string ТипДоговору = "col_b5";
        public const string ТипДоговоруПредставлення = "col_b8";
        public const string ДопустимаСумаЗаборгованості = "col_b6";
        public const string Сума = "col_b7";
        public const string Коментар = "col_a3";
    }
	
    
    public class ДоговориКонтрагентів_Objest : DirectoryObject
    {
        public ДоговориКонтрагентів_Objest() : base(Config.Kernel, "tab_a28",
             new string[] { "col_n4", "col_n5", "col_n6", "col_a1", "col_a2", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_a9", "col_b1", "col_b2", "col_b3", "col_b4", "col_b5", "col_b8", "col_b6", "col_b7", "col_a3" }) 
        {
            Назва = "";
            Код = "";
            БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
            ВалютаВзаєморозрахунків = new Довідники.Валюти_Pointer();
            ДатаПочаткуДії = DateTime.MinValue;
            ДатаЗакінченняДії = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            Дата = DateTime.MinValue;
            Номер = "";
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Узгоджений = false;
            Статус = 0;
            ГосподарськаОперація = 0;
            ТипДоговору = 0;
            ТипДоговоруПредставлення = "";
            ДопустимаСумаЗаборгованості = 0;
            Сума = 0;
            Коментар = "";
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_n4"].ToString();
                Код = base.FieldValue["col_n5"].ToString();
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_n6"]);
                БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer(base.FieldValue["col_a1"]);
                ВалютаВзаєморозрахунків = new Довідники.Валюти_Pointer(base.FieldValue["col_a2"]);
                ДатаПочаткуДії = (base.FieldValue["col_a4"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a4"].ToString()) : DateTime.MinValue;
                ДатаЗакінченняДії = (base.FieldValue["col_a5"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a5"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_a6"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_a7"]);
                Дата = (base.FieldValue["col_a8"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a8"].ToString()) : DateTime.MinValue;
                Номер = base.FieldValue["col_a9"].ToString();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_b1"]);
                Узгоджений = (base.FieldValue["col_b2"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_b2"].ToString()) : false;
                Статус = (base.FieldValue["col_b3"] != DBNull.Value) ? (Перелічення.СтатусиДоговорівКонтрагентів)base.FieldValue["col_b3"] : 0;
                ГосподарськаОперація = (base.FieldValue["col_b4"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_b4"] : 0;
                ТипДоговору = (base.FieldValue["col_b5"] != DBNull.Value) ? (Перелічення.ТипДоговорів)base.FieldValue["col_b5"] : 0;
                ТипДоговоруПредставлення = base.FieldValue["col_b8"].ToString();
                ДопустимаСумаЗаборгованості = (base.FieldValue["col_b6"] != DBNull.Value) ? (decimal)base.FieldValue["col_b6"] : 0;
                Сума = (base.FieldValue["col_b7"] != DBNull.Value) ? (decimal)base.FieldValue["col_b7"] : 0;
                Коментар = base.FieldValue["col_a3"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    ДоговориКонтрагентів_Triggers.BeforeRecording(this);
			base.FieldValue["col_n4"] = Назва;
            base.FieldValue["col_n5"] = Код;
            base.FieldValue["col_n6"] = БанківськийРахунок.UnigueID.UGuid;
            base.FieldValue["col_a1"] = БанківськийРахунокКонтрагента.UnigueID.UGuid;
            base.FieldValue["col_a2"] = ВалютаВзаєморозрахунків.UnigueID.UGuid;
            base.FieldValue["col_a4"] = ДатаПочаткуДії;
            base.FieldValue["col_a5"] = ДатаЗакінченняДії;
            base.FieldValue["col_a6"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_a7"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_a8"] = Дата;
            base.FieldValue["col_a9"] = Номер;
            base.FieldValue["col_b1"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_b2"] = Узгоджений;
            base.FieldValue["col_b3"] = (int)Статус;
            base.FieldValue["col_b4"] = (int)ГосподарськаОперація;
            base.FieldValue["col_b5"] = (int)ТипДоговору;
            base.FieldValue["col_b8"] = ТипДоговоруПредставлення;
            base.FieldValue["col_b6"] = ДопустимаСумаЗаборгованості;
            base.FieldValue["col_b7"] = Сума;
            base.FieldValue["col_a3"] = Коментар;
            
            BaseSave();
			ДоговориКонтрагентів_Triggers.AfterRecording(this);
        }
		
        public ДоговориКонтрагентів_Objest Copy()
        {
            ДоговориКонтрагентів_Objest copy = new ДоговориКонтрагентів_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.БанківськийРахунок = БанківськийРахунок;
			copy.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента;
			copy.ВалютаВзаєморозрахунків = ВалютаВзаєморозрахунків;
			copy.ДатаПочаткуДії = ДатаПочаткуДії;
			copy.ДатаЗакінченняДії = ДатаЗакінченняДії;
			copy.Організація = Організація;
			copy.Контрагент = Контрагент;
			copy.Дата = Дата;
			copy.Номер = Номер;
			copy.Підрозділ = Підрозділ;
			copy.Узгоджений = Узгоджений;
			copy.Статус = Статус;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.ТипДоговору = ТипДоговору;
			copy.ТипДоговоруПредставлення = ТипДоговоруПредставлення;
			copy.ДопустимаСумаЗаборгованості = ДопустимаСумаЗаборгованості;
			copy.Сума = Сума;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
            ДоговориКонтрагентів_Triggers.BeforeDelete(this);
			base.BaseDelete(new string[] {  });
        }
        
        public ДоговориКонтрагентів_Pointer GetDirectoryPointer()
        {
            ДоговориКонтрагентів_Pointer directoryPointer = new ДоговориКонтрагентів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
        public Довідники.БанківськіРахункиКонтрагентів_Pointer БанківськийРахунокКонтрагента { get; set; }
        public Довідники.Валюти_Pointer ВалютаВзаєморозрахунків { get; set; }
        public DateTime ДатаПочаткуДії { get; set; }
        public DateTime ДатаЗакінченняДії { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public DateTime Дата { get; set; }
        public string Номер { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public bool Узгоджений { get; set; }
        public Перелічення.СтатусиДоговорівКонтрагентів Статус { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Перелічення.ТипДоговорів ТипДоговору { get; set; }
        public string ТипДоговоруПредставлення { get; set; }
        public decimal ДопустимаСумаЗаборгованості { get; set; }
        public decimal Сума { get; set; }
        public string Коментар { get; set; }
        
    }
    
    
    public class ДоговориКонтрагентів_Pointer : DirectoryPointer
    {
        public ДоговориКонтрагентів_Pointer(object uid = null) : base(Config.Kernel, "tab_a28")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ДоговориКонтрагентів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a28")
        {
            base.Init(uid, fields);
        }
        
        public ДоговориКонтрагентів_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ДоговориКонтрагентів_Objest ДоговориКонтрагентівObjestItem = new ДоговориКонтрагентів_Objest();
            return ДоговориКонтрагентівObjestItem.Read(base.UnigueID) ? ДоговориКонтрагентівObjestItem : null;
        }
		
        public ДоговориКонтрагентів_Pointer GetNewDirectoryPointer()
        {
            return new ДоговориКонтрагентів_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_n4", "col_b8" }
			);
        }
		
        public ДоговориКонтрагентів_Pointer GetEmptyPointer()
        {
            return new ДоговориКонтрагентів_Pointer();
        }
    }
    
    
    public class ДоговориКонтрагентів_Select : DirectorySelect
    {
        public ДоговориКонтрагентів_Select() : base(Config.Kernel, "tab_a28") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ДоговориКонтрагентів_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ДоговориКонтрагентів_Pointer Current { get; private set; }
        
        public ДоговориКонтрагентів_Pointer FindByField(string name, object value)
        {
            ДоговориКонтрагентів_Pointer itemPointer = new ДоговориКонтрагентів_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ДоговориКонтрагентів_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ДоговориКонтрагентів_Pointer> directoryPointerList = new List<ДоговориКонтрагентів_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ДоговориКонтрагентів_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "БанківськіРахункиКонтрагентів"
    
    public static class БанківськіРахункиКонтрагентів_Const
    {
        public const string TABLE = "tab_a29";
        
        public const string Назва = "col_n7";
        public const string Код = "col_n8";
        public const string НомерРахунку = "col_n9";
        public const string Банк = "col_m1";
        public const string БанкДляРозрахунків = "col_m2";
        public const string ТекстКореспондента = "col_m3";
        public const string ТекстПризначення = "col_m4";
        public const string Валюта = "col_m5";
        public const string НазваБанку = "col_m6";
        public const string КорРахунокБанку = "col_m7";
        public const string МістоБанку = "col_m8";
        public const string АдресаБанку = "col_m9";
        public const string ТелефониБанку = "col_o1";
        public const string Закрито = "col_o2";
        public const string Контрагент = "col_o3";
    }
	
    
    public class БанківськіРахункиКонтрагентів_Objest : DirectoryObject
    {
        public БанківськіРахункиКонтрагентів_Objest() : base(Config.Kernel, "tab_a29",
             new string[] { "col_n7", "col_n8", "col_n9", "col_m1", "col_m2", "col_m3", "col_m4", "col_m5", "col_m6", "col_m7", "col_m8", "col_m9", "col_o1", "col_o2", "col_o3" }) 
        {
            Назва = "";
            Код = "";
            НомерРахунку = "";
            Банк = "";
            БанкДляРозрахунків = "";
            ТекстКореспондента = "";
            ТекстПризначення = "";
            Валюта = new Довідники.Валюти_Pointer();
            НазваБанку = "";
            КорРахунокБанку = "";
            МістоБанку = "";
            АдресаБанку = "";
            ТелефониБанку = "";
            Закрито = false;
            Контрагент = new Довідники.Контрагенти_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_n7"].ToString();
                Код = base.FieldValue["col_n8"].ToString();
                НомерРахунку = base.FieldValue["col_n9"].ToString();
                Банк = base.FieldValue["col_m1"].ToString();
                БанкДляРозрахунків = base.FieldValue["col_m2"].ToString();
                ТекстКореспондента = base.FieldValue["col_m3"].ToString();
                ТекстПризначення = base.FieldValue["col_m4"].ToString();
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_m5"]);
                НазваБанку = base.FieldValue["col_m6"].ToString();
                КорРахунокБанку = base.FieldValue["col_m7"].ToString();
                МістоБанку = base.FieldValue["col_m8"].ToString();
                АдресаБанку = base.FieldValue["col_m9"].ToString();
                ТелефониБанку = base.FieldValue["col_o1"].ToString();
                Закрито = (base.FieldValue["col_o2"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_o2"].ToString()) : false;
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_o3"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_n7"] = Назва;
            base.FieldValue["col_n8"] = Код;
            base.FieldValue["col_n9"] = НомерРахунку;
            base.FieldValue["col_m1"] = Банк;
            base.FieldValue["col_m2"] = БанкДляРозрахунків;
            base.FieldValue["col_m3"] = ТекстКореспондента;
            base.FieldValue["col_m4"] = ТекстПризначення;
            base.FieldValue["col_m5"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_m6"] = НазваБанку;
            base.FieldValue["col_m7"] = КорРахунокБанку;
            base.FieldValue["col_m8"] = МістоБанку;
            base.FieldValue["col_m9"] = АдресаБанку;
            base.FieldValue["col_o1"] = ТелефониБанку;
            base.FieldValue["col_o2"] = Закрито;
            base.FieldValue["col_o3"] = Контрагент.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public БанківськіРахункиКонтрагентів_Objest Copy()
        {
            БанківськіРахункиКонтрагентів_Objest copy = new БанківськіРахункиКонтрагентів_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.НомерРахунку = НомерРахунку;
			copy.Банк = Банк;
			copy.БанкДляРозрахунків = БанкДляРозрахунків;
			copy.ТекстКореспондента = ТекстКореспондента;
			copy.ТекстПризначення = ТекстПризначення;
			copy.Валюта = Валюта;
			copy.НазваБанку = НазваБанку;
			copy.КорРахунокБанку = КорРахунокБанку;
			copy.МістоБанку = МістоБанку;
			copy.АдресаБанку = АдресаБанку;
			copy.ТелефониБанку = ТелефониБанку;
			copy.Закрито = Закрито;
			copy.Контрагент = Контрагент;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public БанківськіРахункиКонтрагентів_Pointer GetDirectoryPointer()
        {
            БанківськіРахункиКонтрагентів_Pointer directoryPointer = new БанківськіРахункиКонтрагентів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string НомерРахунку { get; set; }
        public string Банк { get; set; }
        public string БанкДляРозрахунків { get; set; }
        public string ТекстКореспондента { get; set; }
        public string ТекстПризначення { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public string НазваБанку { get; set; }
        public string КорРахунокБанку { get; set; }
        public string МістоБанку { get; set; }
        public string АдресаБанку { get; set; }
        public string ТелефониБанку { get; set; }
        public bool Закрито { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        
    }
    
    
    public class БанківськіРахункиКонтрагентів_Pointer : DirectoryPointer
    {
        public БанківськіРахункиКонтрагентів_Pointer(object uid = null) : base(Config.Kernel, "tab_a29")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public БанківськіРахункиКонтрагентів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a29")
        {
            base.Init(uid, fields);
        }
        
        public БанківськіРахункиКонтрагентів_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            БанківськіРахункиКонтрагентів_Objest БанківськіРахункиКонтрагентівObjestItem = new БанківськіРахункиКонтрагентів_Objest();
            return БанківськіРахункиКонтрагентівObjestItem.Read(base.UnigueID) ? БанківськіРахункиКонтрагентівObjestItem : null;
        }
		
        public БанківськіРахункиКонтрагентів_Pointer GetNewDirectoryPointer()
        {
            return new БанківськіРахункиКонтрагентів_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_n7" }
			);
        }
		
        public БанківськіРахункиКонтрагентів_Pointer GetEmptyPointer()
        {
            return new БанківськіРахункиКонтрагентів_Pointer();
        }
    }
    
    
    public class БанківськіРахункиКонтрагентів_Select : DirectorySelect
    {
        public БанківськіРахункиКонтрагентів_Select() : base(Config.Kernel, "tab_a29") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new БанківськіРахункиКонтрагентів_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public БанківськіРахункиКонтрагентів_Pointer Current { get; private set; }
        
        public БанківськіРахункиКонтрагентів_Pointer FindByField(string name, object value)
        {
            БанківськіРахункиКонтрагентів_Pointer itemPointer = new БанківськіРахункиКонтрагентів_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<БанківськіРахункиКонтрагентів_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<БанківськіРахункиКонтрагентів_Pointer> directoryPointerList = new List<БанківськіРахункиКонтрагентів_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new БанківськіРахункиКонтрагентів_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "СтаттяРухуКоштів"
    
    public static class СтаттяРухуКоштів_Const
    {
        public const string TABLE = "tab_a45";
        
        public const string Назва = "col_i7";
        public const string Код = "col_i8";
        public const string КореспондуючийРахунок = "col_i9";
        public const string ВидРухуКоштів = "col_j2";
        public const string Опис = "col_j1";
    }
	
    
    public class СтаттяРухуКоштів_Objest : DirectoryObject
    {
        public СтаттяРухуКоштів_Objest() : base(Config.Kernel, "tab_a45",
             new string[] { "col_i7", "col_i8", "col_i9", "col_j2", "col_j1" }) 
        {
            Назва = "";
            Код = "";
            КореспондуючийРахунок = "";
            ВидРухуКоштів = 0;
            Опис = "";
            
            //Табличні частини
            ГосподарськіОперації_TablePart = new СтаттяРухуКоштів_ГосподарськіОперації_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_i7"].ToString();
                Код = base.FieldValue["col_i8"].ToString();
                КореспондуючийРахунок = base.FieldValue["col_i9"].ToString();
                ВидРухуКоштів = (base.FieldValue["col_j2"] != DBNull.Value) ? (Перелічення.ВидиРухуКоштів)base.FieldValue["col_j2"] : 0;
                Опис = base.FieldValue["col_j1"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_i7"] = Назва;
            base.FieldValue["col_i8"] = Код;
            base.FieldValue["col_i9"] = КореспондуючийРахунок;
            base.FieldValue["col_j2"] = (int)ВидРухуКоштів;
            base.FieldValue["col_j1"] = Опис;
            
            BaseSave();
			
        }
		
        public СтаттяРухуКоштів_Objest Copy()
        {
            СтаттяРухуКоштів_Objest copy = new СтаттяРухуКоштів_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Код = Код;
			copy.КореспондуючийРахунок = КореспондуючийРахунок;
			copy.ВидРухуКоштів = ВидРухуКоштів;
			copy.Опис = Опис;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] { "tab_a46" });
        }
        
        public СтаттяРухуКоштів_Pointer GetDirectoryPointer()
        {
            СтаттяРухуКоштів_Pointer directoryPointer = new СтаттяРухуКоштів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string Код { get; set; }
        public string КореспондуючийРахунок { get; set; }
        public Перелічення.ВидиРухуКоштів ВидРухуКоштів { get; set; }
        public string Опис { get; set; }
        
        //Табличні частини
        public СтаттяРухуКоштів_ГосподарськіОперації_TablePart ГосподарськіОперації_TablePart { get; set; }
        
    }
    
    
    public class СтаттяРухуКоштів_Pointer : DirectoryPointer
    {
        public СтаттяРухуКоштів_Pointer(object uid = null) : base(Config.Kernel, "tab_a45")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public СтаттяРухуКоштів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a45")
        {
            base.Init(uid, fields);
        }
        
        public СтаттяРухуКоштів_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            СтаттяРухуКоштів_Objest СтаттяРухуКоштівObjestItem = new СтаттяРухуКоштів_Objest();
            return СтаттяРухуКоштівObjestItem.Read(base.UnigueID) ? СтаттяРухуКоштівObjestItem : null;
        }
		
        public СтаттяРухуКоштів_Pointer GetNewDirectoryPointer()
        {
            return new СтаттяРухуКоштів_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_i7" }
			);
        }
		
        public СтаттяРухуКоштів_Pointer GetEmptyPointer()
        {
            return new СтаттяРухуКоштів_Pointer();
        }
    }
    
    
    public class СтаттяРухуКоштів_Select : DirectorySelect
    {
        public СтаттяРухуКоштів_Select() : base(Config.Kernel, "tab_a45") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new СтаттяРухуКоштів_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public СтаттяРухуКоштів_Pointer Current { get; private set; }
        
        public СтаттяРухуКоштів_Pointer FindByField(string name, object value)
        {
            СтаттяРухуКоштів_Pointer itemPointer = new СтаттяРухуКоштів_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<СтаттяРухуКоштів_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<СтаттяРухуКоштів_Pointer> directoryPointerList = new List<СтаттяРухуКоштів_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new СтаттяРухуКоштів_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
    public class СтаттяРухуКоштів_ГосподарськіОперації_TablePart : DirectoryTablePart
    {
        public СтаттяРухуКоштів_ГосподарськіОперації_TablePart(СтаттяРухуКоштів_Objest owner) : base(Config.Kernel, "tab_a46",
             new string[] { "col_j3" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string ГосподарськаОперація = "col_j3";

        public СтаттяРухуКоштів_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.ГосподарськаОперація = (fieldValue["col_j3"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)fieldValue["col_j3"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_j3", (int)record.ГосподарськаОперація);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : DirectoryTablePartRecord
        {
            public Record()
            {
                ГосподарськаОперація = 0;
                
            }
            public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
            
        }
    }
      
   
    #endregion
    
    #region DIRECTORY "СеріїНоменклатури"
    
    public static class СеріїНоменклатури_Const
    {
        public const string TABLE = "tab_b02";
        
        public const string Номер = "col_a3";
        public const string Коментар = "col_a1";
        public const string ДатаСтворення = "col_a2";
    }
	
    
    public class СеріїНоменклатури_Objest : DirectoryObject
    {
        public СеріїНоменклатури_Objest() : base(Config.Kernel, "tab_b02",
             new string[] { "col_a3", "col_a1", "col_a2" }) 
        {
            Номер = "";
            Коментар = "";
            ДатаСтворення = DateTime.MinValue;
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Номер = base.FieldValue["col_a3"].ToString();
                Коментар = base.FieldValue["col_a1"].ToString();
                ДатаСтворення = (base.FieldValue["col_a2"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a2"].ToString()) : DateTime.MinValue;
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    СеріїНоменклатури_Triggers.BeforeRecording(this);
			base.FieldValue["col_a3"] = Номер;
            base.FieldValue["col_a1"] = Коментар;
            base.FieldValue["col_a2"] = ДатаСтворення;
            
            BaseSave();
			СеріїНоменклатури_Triggers.AfterRecording(this);
        }
		
        public СеріїНоменклатури_Objest Copy()
        {
            СеріїНоменклатури_Objest copy = new СеріїНоменклатури_Objest();
			copy.New();
            copy.Номер = Номер;
			copy.Коментар = Коментар;
			copy.ДатаСтворення = ДатаСтворення;
			
			return copy;
        }

        public void Delete()
        {
            СеріїНоменклатури_Triggers.BeforeDelete(this);
			base.BaseDelete(new string[] {  });
        }
        
        public СеріїНоменклатури_Pointer GetDirectoryPointer()
        {
            СеріїНоменклатури_Pointer directoryPointer = new СеріїНоменклатури_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Номер { get; set; }
        public string Коментар { get; set; }
        public DateTime ДатаСтворення { get; set; }
        
    }
    
    
    public class СеріїНоменклатури_Pointer : DirectoryPointer
    {
        public СеріїНоменклатури_Pointer(object uid = null) : base(Config.Kernel, "tab_b02")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public СеріїНоменклатури_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_b02")
        {
            base.Init(uid, fields);
        }
        
        public СеріїНоменклатури_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            СеріїНоменклатури_Objest СеріїНоменклатуриObjestItem = new СеріїНоменклатури_Objest();
            return СеріїНоменклатуриObjestItem.Read(base.UnigueID) ? СеріїНоменклатуриObjestItem : null;
        }
		
        public СеріїНоменклатури_Pointer GetNewDirectoryPointer()
        {
            return new СеріїНоменклатури_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] { "col_a3" }
			);
        }
		
        public СеріїНоменклатури_Pointer GetEmptyPointer()
        {
            return new СеріїНоменклатури_Pointer();
        }
    }
    
    
    public class СеріїНоменклатури_Select : DirectorySelect
    {
        public СеріїНоменклатури_Select() : base(Config.Kernel, "tab_b02") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new СеріїНоменклатури_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public СеріїНоменклатури_Pointer Current { get; private set; }
        
        public СеріїНоменклатури_Pointer FindByField(string name, object value)
        {
            СеріїНоменклатури_Pointer itemPointer = new СеріїНоменклатури_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<СеріїНоменклатури_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<СеріїНоменклатури_Pointer> directoryPointerList = new List<СеріїНоменклатури_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new СеріїНоменклатури_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ПартіяТоварівКомпозит"
    
    public static class ПартіяТоварівКомпозит_Const
    {
        public const string TABLE = "tab_b06";
        
        public const string Назва = "col_a1";
        public const string Дата = "col_a2";
        public const string ТипДокументу = "col_a3";
        public const string ДокументКлюч = "col_a6";
        public const string ПоступленняТоварівТаПослуг = "col_a4";
        public const string ВведенняЗалишків = "col_a5";
    }
	
    
    public class ПартіяТоварівКомпозит_Objest : DirectoryObject
    {
        public ПартіяТоварівКомпозит_Objest() : base(Config.Kernel, "tab_b06",
             new string[] { "col_a1", "col_a2", "col_a3", "col_a6", "col_a4", "col_a5" }) 
        {
            Назва = "";
            Дата = DateTime.MinValue;
            ТипДокументу = 0;
            ДокументКлюч = new Guid();
            ПоступленняТоварівТаПослуг = new Документи.ПоступленняТоварівТаПослуг_Pointer();
            ВведенняЗалишків = new Документи.ВведенняЗалишків_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_a1"].ToString();
                Дата = (base.FieldValue["col_a2"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a2"].ToString()) : DateTime.MinValue;
                ТипДокументу = (base.FieldValue["col_a3"] != DBNull.Value) ? (Перелічення.ТипДокументуПартіяТоварівКомпозит)base.FieldValue["col_a3"] : 0;
                ДокументКлюч = (base.FieldValue["col_a6"] != DBNull.Value) ? Guid.Parse(base.FieldValue["col_a6"].ToString()) : Guid.Empty;
                ПоступленняТоварівТаПослуг = new Документи.ПоступленняТоварівТаПослуг_Pointer(base.FieldValue["col_a4"]);
                ВведенняЗалишків = new Документи.ВведенняЗалишків_Pointer(base.FieldValue["col_a5"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_a1"] = Назва;
            base.FieldValue["col_a2"] = Дата;
            base.FieldValue["col_a3"] = (int)ТипДокументу;
            base.FieldValue["col_a6"] = ДокументКлюч;
            base.FieldValue["col_a4"] = ПоступленняТоварівТаПослуг.UnigueID.UGuid;
            base.FieldValue["col_a5"] = ВведенняЗалишків.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public ПартіяТоварівКомпозит_Objest Copy()
        {
            ПартіяТоварівКомпозит_Objest copy = new ПартіяТоварівКомпозит_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Дата = Дата;
			copy.ТипДокументу = ТипДокументу;
			copy.ДокументКлюч = ДокументКлюч;
			copy.ПоступленняТоварівТаПослуг = ПоступленняТоварівТаПослуг;
			copy.ВведенняЗалишків = ВведенняЗалишків;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public ПартіяТоварівКомпозит_Pointer GetDirectoryPointer()
        {
            ПартіяТоварівКомпозит_Pointer directoryPointer = new ПартіяТоварівКомпозит_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public DateTime Дата { get; set; }
        public Перелічення.ТипДокументуПартіяТоварівКомпозит ТипДокументу { get; set; }
        public Guid ДокументКлюч { get; set; }
        public Документи.ПоступленняТоварівТаПослуг_Pointer ПоступленняТоварівТаПослуг { get; set; }
        public Документи.ВведенняЗалишків_Pointer ВведенняЗалишків { get; set; }
        
    }
    
    
    public class ПартіяТоварівКомпозит_Pointer : DirectoryPointer
    {
        public ПартіяТоварівКомпозит_Pointer(object uid = null) : base(Config.Kernel, "tab_b06")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПартіяТоварівКомпозит_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_b06")
        {
            base.Init(uid, fields);
        }
        
        public ПартіяТоварівКомпозит_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ПартіяТоварівКомпозит_Objest ПартіяТоварівКомпозитObjestItem = new ПартіяТоварівКомпозит_Objest();
            return ПартіяТоварівКомпозитObjestItem.Read(base.UnigueID) ? ПартіяТоварівКомпозитObjestItem : null;
        }
		
        public ПартіяТоварівКомпозит_Pointer GetNewDirectoryPointer()
        {
            return new ПартіяТоварівКомпозит_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] {  }
			);
        }
		
        public ПартіяТоварівКомпозит_Pointer GetEmptyPointer()
        {
            return new ПартіяТоварівКомпозит_Pointer();
        }
    }
    
    
    public class ПартіяТоварівКомпозит_Select : DirectorySelect
    {
        public ПартіяТоварівКомпозит_Select() : base(Config.Kernel, "tab_b06") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПартіяТоварівКомпозит_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ПартіяТоварівКомпозит_Pointer Current { get; private set; }
        
        public ПартіяТоварівКомпозит_Pointer FindByField(string name, object value)
        {
            ПартіяТоварівКомпозит_Pointer itemPointer = new ПартіяТоварівКомпозит_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ПартіяТоварівКомпозит_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ПартіяТоварівКомпозит_Pointer> directoryPointerList = new List<ПартіяТоварівКомпозит_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ПартіяТоварівКомпозит_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ВидиЗапасів"
    
    public static class ВидиЗапасів_Const
    {
        public const string TABLE = "tab_b13";
        
        public const string Назва = "col_a5";
        public const string Організація = "col_b3";
        public const string ТипЗапасів = "col_a7";
        public const string Валюта = "col_a9";
        public const string Контрагент = "col_b1";
        public const string Договір = "col_b2";
    }
	
    
    public class ВидиЗапасів_Objest : DirectoryObject
    {
        public ВидиЗапасів_Objest() : base(Config.Kernel, "tab_b13",
             new string[] { "col_a5", "col_b3", "col_a7", "col_a9", "col_b1", "col_b2" }) 
        {
            Назва = "";
            Організація = new Довідники.Організації_Pointer();
            ТипЗапасів = 0;
            Валюта = new Довідники.Валюти_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_a5"].ToString();
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_b3"]);
                ТипЗапасів = (base.FieldValue["col_a7"] != DBNull.Value) ? (Перелічення.ТипЗапасів)base.FieldValue["col_a7"] : 0;
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_a9"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_b1"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_b2"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_a5"] = Назва;
            base.FieldValue["col_b3"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_a7"] = (int)ТипЗапасів;
            base.FieldValue["col_a9"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_b1"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_b2"] = Договір.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public ВидиЗапасів_Objest Copy()
        {
            ВидиЗапасів_Objest copy = new ВидиЗапасів_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Організація = Організація;
			copy.ТипЗапасів = ТипЗапасів;
			copy.Валюта = Валюта;
			copy.Контрагент = Контрагент;
			copy.Договір = Договір;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public ВидиЗапасів_Pointer GetDirectoryPointer()
        {
            ВидиЗапасів_Pointer directoryPointer = new ВидиЗапасів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Перелічення.ТипЗапасів ТипЗапасів { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        
    }
    
    
    public class ВидиЗапасів_Pointer : DirectoryPointer
    {
        public ВидиЗапасів_Pointer(object uid = null) : base(Config.Kernel, "tab_b13")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВидиЗапасів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_b13")
        {
            base.Init(uid, fields);
        }
        
        public ВидиЗапасів_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ВидиЗапасів_Objest ВидиЗапасівObjestItem = new ВидиЗапасів_Objest();
            return ВидиЗапасівObjestItem.Read(base.UnigueID) ? ВидиЗапасівObjestItem : null;
        }
		
        public ВидиЗапасів_Pointer GetNewDirectoryPointer()
        {
            return new ВидиЗапасів_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] {  }
			);
        }
		
        public ВидиЗапасів_Pointer GetEmptyPointer()
        {
            return new ВидиЗапасів_Pointer();
        }
    }
    
    
    public class ВидиЗапасів_Select : DirectorySelect
    {
        public ВидиЗапасів_Select() : base(Config.Kernel, "tab_b13") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВидиЗапасів_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ВидиЗапасів_Pointer Current { get; private set; }
        
        public ВидиЗапасів_Pointer FindByField(string name, object value)
        {
            ВидиЗапасів_Pointer itemPointer = new ВидиЗапасів_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ВидиЗапасів_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ВидиЗапасів_Pointer> directoryPointerList = new List<ВидиЗапасів_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ВидиЗапасів_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "ПродажіДокументКомпозит"
    
    public static class ПродажіДокументКомпозит_Const
    {
        public const string TABLE = "tab_b14";
        
        public const string Назва = "col_b4";
        public const string ТипДокументу = "col_b6";
        public const string ДокументКлюч = "col_b7";
        public const string ЗамовленняКлієнта = "col_b8";
        public const string АктВиконанихРобіт = "col_b9";
        public const string РеалізаціяТоварівТаПослуг = "col_c1";
    }
	
    
    public class ПродажіДокументКомпозит_Objest : DirectoryObject
    {
        public ПродажіДокументКомпозит_Objest() : base(Config.Kernel, "tab_b14",
             new string[] { "col_b4", "col_b6", "col_b7", "col_b8", "col_b9", "col_c1" }) 
        {
            Назва = "";
            ТипДокументу = 0;
            ДокументКлюч = new Guid();
            ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer();
            АктВиконанихРобіт = new Документи.АктВиконанихРобіт_Pointer();
            РеалізаціяТоварівТаПослуг = new Документи.РеалізаціяТоварівТаПослуг_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_b4"].ToString();
                ТипДокументу = (base.FieldValue["col_b6"] != DBNull.Value) ? (Перелічення.ТипДокументуПродажіДокументКомпозит)base.FieldValue["col_b6"] : 0;
                ДокументКлюч = (base.FieldValue["col_b7"] != DBNull.Value) ? Guid.Parse(base.FieldValue["col_b7"].ToString()) : Guid.Empty;
                ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer(base.FieldValue["col_b8"]);
                АктВиконанихРобіт = new Документи.АктВиконанихРобіт_Pointer(base.FieldValue["col_b9"]);
                РеалізаціяТоварівТаПослуг = new Документи.РеалізаціяТоварівТаПослуг_Pointer(base.FieldValue["col_c1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_b4"] = Назва;
            base.FieldValue["col_b6"] = (int)ТипДокументу;
            base.FieldValue["col_b7"] = ДокументКлюч;
            base.FieldValue["col_b8"] = ЗамовленняКлієнта.UnigueID.UGuid;
            base.FieldValue["col_b9"] = АктВиконанихРобіт.UnigueID.UGuid;
            base.FieldValue["col_c1"] = РеалізаціяТоварівТаПослуг.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public ПродажіДокументКомпозит_Objest Copy()
        {
            ПродажіДокументКомпозит_Objest copy = new ПродажіДокументКомпозит_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.ТипДокументу = ТипДокументу;
			copy.ДокументКлюч = ДокументКлюч;
			copy.ЗамовленняКлієнта = ЗамовленняКлієнта;
			copy.АктВиконанихРобіт = АктВиконанихРобіт;
			copy.РеалізаціяТоварівТаПослуг = РеалізаціяТоварівТаПослуг;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public ПродажіДокументКомпозит_Pointer GetDirectoryPointer()
        {
            ПродажіДокументКомпозит_Pointer directoryPointer = new ПродажіДокументКомпозит_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public Перелічення.ТипДокументуПродажіДокументКомпозит ТипДокументу { get; set; }
        public Guid ДокументКлюч { get; set; }
        public Документи.ЗамовленняКлієнта_Pointer ЗамовленняКлієнта { get; set; }
        public Документи.АктВиконанихРобіт_Pointer АктВиконанихРобіт { get; set; }
        public Документи.РеалізаціяТоварівТаПослуг_Pointer РеалізаціяТоварівТаПослуг { get; set; }
        
    }
    
    
    public class ПродажіДокументКомпозит_Pointer : DirectoryPointer
    {
        public ПродажіДокументКомпозит_Pointer(object uid = null) : base(Config.Kernel, "tab_b14")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПродажіДокументКомпозит_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_b14")
        {
            base.Init(uid, fields);
        }
        
        public ПродажіДокументКомпозит_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            ПродажіДокументКомпозит_Objest ПродажіДокументКомпозитObjestItem = new ПродажіДокументКомпозит_Objest();
            return ПродажіДокументКомпозитObjestItem.Read(base.UnigueID) ? ПродажіДокументКомпозитObjestItem : null;
        }
		
        public ПродажіДокументКомпозит_Pointer GetNewDirectoryPointer()
        {
            return new ПродажіДокументКомпозит_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] {  }
			);
        }
		
        public ПродажіДокументКомпозит_Pointer GetEmptyPointer()
        {
            return new ПродажіДокументКомпозит_Pointer();
        }
    }
    
    
    public class ПродажіДокументКомпозит_Select : DirectorySelect
    {
        public ПродажіДокументКомпозит_Select() : base(Config.Kernel, "tab_b14") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПродажіДокументКомпозит_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public ПродажіДокументКомпозит_Pointer Current { get; private set; }
        
        public ПродажіДокументКомпозит_Pointer FindByField(string name, object value)
        {
            ПродажіДокументКомпозит_Pointer itemPointer = new ПродажіДокументКомпозит_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<ПродажіДокументКомпозит_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<ПродажіДокументКомпозит_Pointer> directoryPointerList = new List<ПродажіДокументКомпозит_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new ПродажіДокументКомпозит_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "АналітикаНоменклатуриКомпозит"
    
    public static class АналітикаНоменклатуриКомпозит_Const
    {
        public const string TABLE = "tab_b15";
        
        public const string Назва = "col_a1";
        public const string Номенклатура = "col_a4";
        public const string ХарактеристикаНоменклатури = "col_a5";
        public const string Серія = "col_a6";
        public const string Склад = "col_a7";
    }
	
    
    public class АналітикаНоменклатуриКомпозит_Objest : DirectoryObject
    {
        public АналітикаНоменклатуриКомпозит_Objest() : base(Config.Kernel, "tab_b15",
             new string[] { "col_a1", "col_a4", "col_a5", "col_a6", "col_a7" }) 
        {
            Назва = "";
            Номенклатура = new Довідники.Номенклатура_Pointer();
            ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
            Серія = new Довідники.СеріїНоменклатури_Pointer();
            Склад = new Довідники.Склади_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_a1"].ToString();
                Номенклатура = new Довідники.Номенклатура_Pointer(base.FieldValue["col_a4"]);
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(base.FieldValue["col_a5"]);
                Серія = new Довідники.СеріїНоменклатури_Pointer(base.FieldValue["col_a6"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_a7"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_a1"] = Назва;
            base.FieldValue["col_a4"] = Номенклатура.UnigueID.UGuid;
            base.FieldValue["col_a5"] = ХарактеристикаНоменклатури.UnigueID.UGuid;
            base.FieldValue["col_a6"] = Серія.UnigueID.UGuid;
            base.FieldValue["col_a7"] = Склад.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public АналітикаНоменклатуриКомпозит_Objest Copy()
        {
            АналітикаНоменклатуриКомпозит_Objest copy = new АналітикаНоменклатуриКомпозит_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Номенклатура = Номенклатура;
			copy.ХарактеристикаНоменклатури = ХарактеристикаНоменклатури;
			copy.Серія = Серія;
			copy.Склад = Склад;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public АналітикаНоменклатуриКомпозит_Pointer GetDirectoryPointer()
        {
            АналітикаНоменклатуриКомпозит_Pointer directoryPointer = new АналітикаНоменклатуриКомпозит_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
        public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
        public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        
    }
    
    
    public class АналітикаНоменклатуриКомпозит_Pointer : DirectoryPointer
    {
        public АналітикаНоменклатуриКомпозит_Pointer(object uid = null) : base(Config.Kernel, "tab_b15")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public АналітикаНоменклатуриКомпозит_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_b15")
        {
            base.Init(uid, fields);
        }
        
        public АналітикаНоменклатуриКомпозит_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            АналітикаНоменклатуриКомпозит_Objest АналітикаНоменклатуриКомпозитObjestItem = new АналітикаНоменклатуриКомпозит_Objest();
            return АналітикаНоменклатуриКомпозитObjestItem.Read(base.UnigueID) ? АналітикаНоменклатуриКомпозитObjestItem : null;
        }
		
        public АналітикаНоменклатуриКомпозит_Pointer GetNewDirectoryPointer()
        {
            return new АналітикаНоменклатуриКомпозит_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] {  }
			);
        }
		
        public АналітикаНоменклатуриКомпозит_Pointer GetEmptyPointer()
        {
            return new АналітикаНоменклатуриКомпозит_Pointer();
        }
    }
    
    
    public class АналітикаНоменклатуриКомпозит_Select : DirectorySelect
    {
        public АналітикаНоменклатуриКомпозит_Select() : base(Config.Kernel, "tab_b15") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new АналітикаНоменклатуриКомпозит_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public АналітикаНоменклатуриКомпозит_Pointer Current { get; private set; }
        
        public АналітикаНоменклатуриКомпозит_Pointer FindByField(string name, object value)
        {
            АналітикаНоменклатуриКомпозит_Pointer itemPointer = new АналітикаНоменклатуриКомпозит_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<АналітикаНоменклатуриКомпозит_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<АналітикаНоменклатуриКомпозит_Pointer> directoryPointerList = new List<АналітикаНоменклатуриКомпозит_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new АналітикаНоменклатуриКомпозит_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "АналітикаКонтрагентівКомпозит"
    
    public static class АналітикаКонтрагентівКомпозит_Const
    {
        public const string TABLE = "tab_b16";
        
        public const string Назва = "col_a8";
        public const string Організація = "col_b3";
        public const string Контрагент = "col_b1";
        public const string Договір = "col_b2";
    }
	
    
    public class АналітикаКонтрагентівКомпозит_Objest : DirectoryObject
    {
        public АналітикаКонтрагентівКомпозит_Objest() : base(Config.Kernel, "tab_b16",
             new string[] { "col_a8", "col_b3", "col_b1", "col_b2" }) 
        {
            Назва = "";
            Організація = new Довідники.Організації_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_a8"].ToString();
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_b3"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_b1"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_b2"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_a8"] = Назва;
            base.FieldValue["col_b3"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_b1"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_b2"] = Договір.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public АналітикаКонтрагентівКомпозит_Objest Copy()
        {
            АналітикаКонтрагентівКомпозит_Objest copy = new АналітикаКонтрагентівКомпозит_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Організація = Організація;
			copy.Контрагент = Контрагент;
			copy.Договір = Договір;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public АналітикаКонтрагентівКомпозит_Pointer GetDirectoryPointer()
        {
            АналітикаКонтрагентівКомпозит_Pointer directoryPointer = new АналітикаКонтрагентівКомпозит_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        
    }
    
    
    public class АналітикаКонтрагентівКомпозит_Pointer : DirectoryPointer
    {
        public АналітикаКонтрагентівКомпозит_Pointer(object uid = null) : base(Config.Kernel, "tab_b16")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public АналітикаКонтрагентівКомпозит_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_b16")
        {
            base.Init(uid, fields);
        }
        
        public АналітикаКонтрагентівКомпозит_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            АналітикаКонтрагентівКомпозит_Objest АналітикаКонтрагентівКомпозитObjestItem = new АналітикаКонтрагентівКомпозит_Objest();
            return АналітикаКонтрагентівКомпозитObjestItem.Read(base.UnigueID) ? АналітикаКонтрагентівКомпозитObjestItem : null;
        }
		
        public АналітикаКонтрагентівКомпозит_Pointer GetNewDirectoryPointer()
        {
            return new АналітикаКонтрагентівКомпозит_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] {  }
			);
        }
		
        public АналітикаКонтрагентівКомпозит_Pointer GetEmptyPointer()
        {
            return new АналітикаКонтрагентівКомпозит_Pointer();
        }
    }
    
    
    public class АналітикаКонтрагентівКомпозит_Select : DirectorySelect
    {
        public АналітикаКонтрагентівКомпозит_Select() : base(Config.Kernel, "tab_b16") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new АналітикаКонтрагентівКомпозит_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public АналітикаКонтрагентівКомпозит_Pointer Current { get; private set; }
        
        public АналітикаКонтрагентівКомпозит_Pointer FindByField(string name, object value)
        {
            АналітикаКонтрагентівКомпозит_Pointer itemPointer = new АналітикаКонтрагентівКомпозит_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<АналітикаКонтрагентівКомпозит_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<АналітикаКонтрагентівКомпозит_Pointer> directoryPointerList = new List<АналітикаКонтрагентівКомпозит_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new АналітикаКонтрагентівКомпозит_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
    #region DIRECTORY "АналітикаПартійКомпозит"
    
    public static class АналітикаПартійКомпозит_Const
    {
        public const string TABLE = "tab_b18";
        
        public const string Назва = "col_b4";
        public const string Контрагент = "col_b6";
    }
	
    
    public class АналітикаПартійКомпозит_Objest : DirectoryObject
    {
        public АналітикаПартійКомпозит_Objest() : base(Config.Kernel, "tab_b18",
             new string[] { "col_b4", "col_b6" }) 
        {
            Назва = "";
            Контрагент = new Довідники.Контрагенти_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["col_b4"].ToString();
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_b6"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
		    base.FieldValue["col_b4"] = Назва;
            base.FieldValue["col_b6"] = Контрагент.UnigueID.UGuid;
            
            BaseSave();
			
        }
		
        public АналітикаПартійКомпозит_Objest Copy()
        {
            АналітикаПартійКомпозит_Objest copy = new АналітикаПартійКомпозит_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.Контрагент = Контрагент;
			
			return copy;
        }

        public void Delete()
        {
            
			base.BaseDelete(new string[] {  });
        }
        
        public АналітикаПартійКомпозит_Pointer GetDirectoryPointer()
        {
            АналітикаПартійКомпозит_Pointer directoryPointer = new АналітикаПартійКомпозит_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        
    }
    
    
    public class АналітикаПартійКомпозит_Pointer : DirectoryPointer
    {
        public АналітикаПартійКомпозит_Pointer(object uid = null) : base(Config.Kernel, "tab_b18")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public АналітикаПартійКомпозит_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_b18")
        {
            base.Init(uid, fields);
        }
        
        public АналітикаПартійКомпозит_Objest GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            АналітикаПартійКомпозит_Objest АналітикаПартійКомпозитObjestItem = new АналітикаПартійКомпозит_Objest();
            return АналітикаПартійКомпозитObjestItem.Read(base.UnigueID) ? АналітикаПартійКомпозитObjestItem : null;
        }
		
        public АналітикаПартійКомпозит_Pointer GetNewDirectoryPointer()
        {
            return new АналітикаПартійКомпозит_Pointer(base.UnigueID);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
			    new string[] {  }
			);
        }
		
        public АналітикаПартійКомпозит_Pointer GetEmptyPointer()
        {
            return new АналітикаПартійКомпозит_Pointer();
        }
    }
    
    
    public class АналітикаПартійКомпозит_Select : DirectorySelect
    {
        public АналітикаПартійКомпозит_Select() : base(Config.Kernel, "tab_b18") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new АналітикаПартійКомпозит_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public АналітикаПартійКомпозит_Pointer Current { get; private set; }
        
        public АналітикаПартійКомпозит_Pointer FindByField(string name, object value)
        {
            АналітикаПартійКомпозит_Pointer itemPointer = new АналітикаПартійКомпозит_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List<АналітикаПартійКомпозит_Pointer> FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List<АналітикаПартійКомпозит_Pointer> directoryPointerList = new List<АналітикаПартійКомпозит_Pointer>();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new АналітикаПартійКомпозит_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      
   
    #endregion
    
}

namespace StorageAndTrade_1_0.Перелічення
{
    
    #region ENUM "ТипиКонтактноїІнформації"
    
    public enum ТипиКонтактноїІнформації
    {
         Адрес = 1,
         Телефон = 2,
         ЕлектроннаПошта = 3,
         Сайт = 4,
         Skype = 5,
         Інше = 6
    }
    #endregion
    
    #region ENUM "ТипиНоменклатури"
    
    public enum ТипиНоменклатури
    {
         Товар = 1,
         Послуга = 2,
         Робота = 3,
         Тара = 4,
         Набір = 5
    }
    #endregion
    
    #region ENUM "ТипиСкладів"
    
    public enum ТипиСкладів
    {
         Гуртовий = 1,
         Роздрібний = 2
    }
    #endregion
    
    #region ENUM "СтатьФізичноїОсоби"
    
    public enum СтатьФізичноїОсоби
    {
         Чоловік = 1,
         Жінка = 2
    }
    #endregion
    
    #region ENUM "СтатусиДоговорівКонтрагентів"
    
    public enum СтатусиДоговорівКонтрагентів
    {
         НеУзгоджений = 1,
         Діє = 2,
         Закритий = 3
    }
    #endregion
    
    #region ENUM "ГосподарськіОперації"
    
    public enum ГосподарськіОперації
    {
         ЗамовленняВПостачальника = 1,
         ОплатаПостачальнику = 2,
         ОприбуткуванняТоварів = 3,
         ПереміщенняТоварів = 4,
         НадходженняПослуг = 5,
         ІншеНадходженняТоварів = 6,
         ІншіДоходи = 7,
         ІншіВитрати = 8,
         РеалізаціяКлієнту = 9,
         СписанняТоварів = 10,
         ПоступленняОплатиВідКлієнта = 11,
         ПоступленняКоштівЗІншоїКаси = 12,
         ПоступленняКоштівЗБанку = 13,
         ПоверненняКоштівВідПостачальника = 14,
         ПоверненняКоштівПостачальнику = 15,
         ЗдачаКоштівВБанк = 16,
         ПоверненняОплатиКлієнту = 17,
         ВидачаКоштівВІншуКасу = 18,
         ЗакупівляВПостачальника = 19,
         ПлануванняПоЗамовленнямПостачальнику = 20,
         ПлануванняПоЗамовленнямКлієнта = 21,
         ПоверненняТоварівВідКлієнта = 22,
         ПоверненняТоварівПостачальнику = 23,
         ВведенняЗалишків = 24,
         ВнутрішнєСпоживанняТоварів = 25
    }
    #endregion
    
    #region ENUM "ТипДоговорів"
    
    public enum ТипДоговорів
    {
         ЗПокупцями = 1,
         ЗПостачальниками = 2
    }
    #endregion
    
    #region ENUM "СпособиДоставки"
    
    public enum СпособиДоставки
    {
         Самовивіз = 1,
         ДоКлієнта = 2,
         СиламиПеревізника = 3,
         НашимиСиламиЗАдресиВідправника = 4,
         ПорученняЕкспедитору = 5
    }
    #endregion
    
    #region ENUM "ФормаОплати"
    
    public enum ФормаОплати
    {
         Готівка = 1,
         Безготівка = 2,
         Взаєморозрахунок = 3
    }
    #endregion
    
    #region ENUM "СтатусиЗамовленьКлієнтів"
    
    public enum СтатусиЗамовленьКлієнтів
    {
         НеУзгоджений = 1,
         ДоЗабезпечення = 2,
         ДоВідгрузки = 3,
         Закритий = 4
    }
    #endregion
    
    #region ENUM "СтатусиРеалізаціїТоварівТаПослуг"
    
    public enum СтатусиРеалізаціїТоварівТаПослуг
    {
         ДоОплати = 1,
         ВДорозі = 2,
         Відгружено = 3
    }
    #endregion
    
    #region ENUM "ВидиРухуКоштів"
    
    public enum ВидиРухуКоштів
    {
         ОплатаПраці = 1,
         ПодатокНаПрибуток = 2,
         ОплатаОборотнихАктивів = 3
    }
    #endregion
    
    #region ENUM "СтатусиПереміщенняТоварів"
    
    public enum СтатусиПереміщенняТоварів
    {
         Відгружено = 1,
         Принято = 2
    }
    #endregion
    
    #region ENUM "СтатусиЗамовленьПостачальникам"
    
    public enum СтатусиЗамовленьПостачальникам
    {
         НеУзгоджений = 1,
         Узгоджений = 2,
         Підтверджений = 3,
         Закритий = 4
    }
    #endregion
    
    #region ENUM "ТипДокументуПартіяТоварівКомпозит"
    
    public enum ТипДокументуПартіяТоварівКомпозит
    {
         ПоступленняТоварівТаПослуг = 1,
         ВведенняЗалишків = 2
    }
    #endregion
    
    #region ENUM "ТипЗапасів"
    
    public enum ТипЗапасів
    {
         Товар = 1,
         Послуга = 2
    }
    #endregion
    
    #region ENUM "ТипДокументуПродажіДокументКомпозит"
    
    public enum ТипДокументуПродажіДокументКомпозит
    {
         ЗамовленняКлієнта = 5,
         АктВиконанихРобіт = 6,
         РеалізаціяТоварівТаПослуг = 7
    }
    #endregion
    
    #region ENUM "ТипПеріодуДляЖурналівДокументів"
    
    public enum ТипПеріодуДляЖурналівДокументів
    {
         ВесьПеріод = 1,
         ЗПочаткуРоку = 2,
         Квартал = 6,
         ЗМинулогоМісяця = 7,
         ЗПочаткуМісяця = 3,
         ЗПочаткуТижня = 4,
         ПоточнийДень = 5
    }
    #endregion
    
    #region ENUM "МетодиСписанняПартій"
    
    public enum МетодиСписанняПартій
    {
         FIFO = 1,
         LIFO = 2
    }
    #endregion
    
}

namespace StorageAndTrade_1_0.Документи
{
    
    #region DOCUMENT "ЗамовленняПостачальнику"
    
    public static class ЗамовленняПостачальнику_Const
    {
        public const string TABLE = "tab_a25";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Контрагент = "col_k2";
        public const string Організація = "col_k3";
        public const string Склад = "col_k4";
        public const string Валюта = "col_k5";
        public const string СумаДокументу = "col_k6";
        public const string Каса = "col_k7";
        public const string БанківськийРахунок = "col_a1";
        public const string Підрозділ = "col_a3";
        public const string Договір = "col_a4";
        public const string Автор = "col_a5";
        public const string ДатаПоступлення = "col_a7";
        public const string АдресаДоставкиДляПостачальника = "col_a8";
        public const string ПовернутиТару = "col_a9";
        public const string СпосібДоставки = "col_b1";
        public const string ЧасДоставкиЗ = "col_b2";
        public const string ЧасДоставкиДо = "col_b3";
        public const string АдресаДоставки = "col_b4";
        public const string ГосподарськаОперація = "col_a6";
        public const string Статус = "col_b5";
        public const string ФормаОплати = "col_b6";
        public const string Менеджер = "col_b7";
        public const string Основа = "col_b8";
        public const string Коментар = "col_a2";
    }
	
    
    public class ЗамовленняПостачальнику_Objest : DocumentObject
    {
        public ЗамовленняПостачальнику_Objest() : base(Config.Kernel, "tab_a25", "ЗамовленняПостачальнику",
             new string[] { "docname", "docnomer", "docdate", "col_k2", "col_k3", "col_k4", "col_k5", "col_k6", "col_k7", "col_a1", "col_a3", "col_a4", "col_a5", "col_a7", "col_a8", "col_a9", "col_b1", "col_b2", "col_b3", "col_b4", "col_a6", "col_b5", "col_b6", "col_b7", "col_b8", "col_a2" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Контрагент = new Довідники.Контрагенти_Pointer();
            Організація = new Довідники.Організації_Pointer();
            Склад = new Довідники.Склади_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            СумаДокументу = 0;
            Каса = new Довідники.Каси_Pointer();
            БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            Автор = new Довідники.Користувачі_Pointer();
            ДатаПоступлення = DateTime.MinValue;
            АдресаДоставкиДляПостачальника = "";
            ПовернутиТару = false;
            СпосібДоставки = 0;
            ЧасДоставкиЗ = DateTime.MinValue.TimeOfDay;
            ЧасДоставкиДо = DateTime.MinValue.TimeOfDay;
            АдресаДоставки = "";
            ГосподарськаОперація = 0;
            Статус = 0;
            ФормаОплати = 0;
            Менеджер = new Довідники.Користувачі_Pointer();
            Основа = new UuidAndText();
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ЗамовленняПостачальнику_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_k2"]);
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_k3"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_k4"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_k5"]);
                СумаДокументу = (base.FieldValue["col_k6"] != DBNull.Value) ? (decimal)base.FieldValue["col_k6"] : 0;
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_k7"]);
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_a1"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a3"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_a4"]);
                Автор = new Довідники.Користувачі_Pointer(base.FieldValue["col_a5"]);
                ДатаПоступлення = (base.FieldValue["col_a7"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a7"].ToString()) : DateTime.MinValue;
                АдресаДоставкиДляПостачальника = base.FieldValue["col_a8"].ToString();
                ПовернутиТару = (base.FieldValue["col_a9"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_a9"].ToString()) : false;
                СпосібДоставки = (base.FieldValue["col_b1"] != DBNull.Value) ? (Перелічення.СпособиДоставки)base.FieldValue["col_b1"] : 0;
                ЧасДоставкиЗ = (base.FieldValue["col_b2"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_b2"].ToString()) : DateTime.MinValue.TimeOfDay;
                ЧасДоставкиДо = (base.FieldValue["col_b3"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_b3"].ToString()) : DateTime.MinValue.TimeOfDay;
                АдресаДоставки = base.FieldValue["col_b4"].ToString();
                ГосподарськаОперація = (base.FieldValue["col_a6"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_a6"] : 0;
                Статус = (base.FieldValue["col_b5"] != DBNull.Value) ? (Перелічення.СтатусиЗамовленьПостачальникам)base.FieldValue["col_b5"] : 0;
                ФормаОплати = (base.FieldValue["col_b6"] != DBNull.Value) ? (Перелічення.ФормаОплати)base.FieldValue["col_b6"] : 0;
                Менеджер = new Довідники.Користувачі_Pointer(base.FieldValue["col_b7"]);
                Основа = (base.FieldValue["col_b8"] != DBNull.Value) ? (UuidAndText)base.FieldValue["col_b8"] : new UuidAndText();
                Коментар = base.FieldValue["col_a2"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ЗамовленняПостачальнику_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_k2"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_k3"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_k4"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_k5"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_k6"] = СумаДокументу;
            base.FieldValue["col_k7"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_a1"] = БанківськийРахунок.UnigueID.UGuid;
            base.FieldValue["col_a3"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_a4"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_a5"] = Автор.UnigueID.UGuid;
            base.FieldValue["col_a7"] = ДатаПоступлення;
            base.FieldValue["col_a8"] = АдресаДоставкиДляПостачальника;
            base.FieldValue["col_a9"] = ПовернутиТару;
            base.FieldValue["col_b1"] = (int)СпосібДоставки;
            base.FieldValue["col_b2"] = ЧасДоставкиЗ;
            base.FieldValue["col_b3"] = ЧасДоставкиДо;
            base.FieldValue["col_b4"] = АдресаДоставки;
            base.FieldValue["col_a6"] = (int)ГосподарськаОперація;
            base.FieldValue["col_b5"] = (int)Статус;
            base.FieldValue["col_b6"] = (int)ФормаОплати;
            base.FieldValue["col_b7"] = Менеджер.UnigueID.UGuid;
            base.FieldValue["col_b8"] = Основа;
            base.FieldValue["col_a2"] = Коментар;
            
            BaseSave();
			ЗамовленняПостачальнику_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ЗамовленняПостачальнику_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ЗамовленняПостачальнику_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ЗамовленняПостачальнику_Objest Copy()
        {
            ЗамовленняПостачальнику_Objest copy = new ЗамовленняПостачальнику_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Контрагент = Контрагент;
			copy.Організація = Організація;
			copy.Склад = Склад;
			copy.Валюта = Валюта;
			copy.СумаДокументу = СумаДокументу;
			copy.Каса = Каса;
			copy.БанківськийРахунок = БанківськийРахунок;
			copy.Підрозділ = Підрозділ;
			copy.Договір = Договір;
			copy.Автор = Автор;
			copy.ДатаПоступлення = ДатаПоступлення;
			copy.АдресаДоставкиДляПостачальника = АдресаДоставкиДляПостачальника;
			copy.ПовернутиТару = ПовернутиТару;
			copy.СпосібДоставки = СпосібДоставки;
			copy.ЧасДоставкиЗ = ЧасДоставкиЗ;
			copy.ЧасДоставкиДо = ЧасДоставкиДо;
			copy.АдресаДоставки = АдресаДоставки;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Статус = Статус;
			copy.ФормаОплати = ФормаОплати;
			copy.Менеджер = Менеджер;
			copy.Основа = Основа;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ЗамовленняПостачальнику_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a30" });
        }
        
        public ЗамовленняПостачальнику_Pointer GetDocumentPointer()
        {
            ЗамовленняПостачальнику_Pointer directoryPointer = new ЗамовленняПостачальнику_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public decimal СумаДокументу { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Довідники.Користувачі_Pointer Автор { get; set; }
        public DateTime ДатаПоступлення { get; set; }
        public string АдресаДоставкиДляПостачальника { get; set; }
        public bool ПовернутиТару { get; set; }
        public Перелічення.СпособиДоставки СпосібДоставки { get; set; }
        public TimeSpan ЧасДоставкиЗ { get; set; }
        public TimeSpan ЧасДоставкиДо { get; set; }
        public string АдресаДоставки { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Перелічення.СтатусиЗамовленьПостачальникам Статус { get; set; }
        public Перелічення.ФормаОплати ФормаОплати { get; set; }
        public Довідники.Користувачі_Pointer Менеджер { get; set; }
        public UuidAndText Основа { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ЗамовленняПостачальнику_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ЗамовленняПостачальнику_Pointer : DocumentPointer
    {
        public ЗамовленняПостачальнику_Pointer(object uid = null) : base(Config.Kernel, "tab_a25", "ЗамовленняПостачальнику")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ЗамовленняПостачальнику_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a25", "ЗамовленняПостачальнику")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ЗамовленняПостачальнику_Pointer GetNewDocumentPointer()
        {
            return new ЗамовленняПостачальнику_Pointer(base.UnigueID);
        }
		
        public ЗамовленняПостачальнику_Pointer GetEmptyPointer()
        {
            return new ЗамовленняПостачальнику_Pointer();
        }
		
        public ЗамовленняПостачальнику_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ЗамовленняПостачальнику_Objest ЗамовленняПостачальникуObjestItem = new ЗамовленняПостачальнику_Objest();
            ЗамовленняПостачальникуObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ЗамовленняПостачальникуObjestItem.Товари_TablePart.Read();
			}
			
            return ЗамовленняПостачальникуObjestItem;
        }
    }
    
    
    public class ЗамовленняПостачальнику_Select : DocumentSelect
    {		
        public ЗамовленняПостачальнику_Select() : base(Config.Kernel, "tab_a25") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ЗамовленняПостачальнику_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ЗамовленняПостачальнику_Pointer Current { get; private set; }
    }
    
      
    public class ЗамовленняПостачальнику_Товари_TablePart : DocumentTablePart
    {
        public ЗамовленняПостачальнику_Товари_TablePart(ЗамовленняПостачальнику_Objest owner) : base(Config.Kernel, "tab_a30",
             new string[] { "col_b2", "col_o4", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_a9", "col_b1" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_b2";
        public const string Номенклатура = "col_o4";
        public const string ХарактеристикаНоменклатури = "col_a1";
        public const string Пакування = "col_a2";
        public const string КількістьУпаковок = "col_a3";
        public const string Кількість = "col_a4";
        public const string ДатаПоступлення = "col_a5";
        public const string Ціна = "col_a6";
        public const string Сума = "col_a7";
        public const string Скидка = "col_a8";
        public const string Склад = "col_a9";
        public const string Підрозділ = "col_b1";

        public ЗамовленняПостачальнику_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_b2"] != DBNull.Value) ? (int)fieldValue["col_b2"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_o4"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_a2"]);
                record.КількістьУпаковок = (fieldValue["col_a3"] != DBNull.Value) ? (int)fieldValue["col_a3"] : 0;
                record.Кількість = (fieldValue["col_a4"] != DBNull.Value) ? (decimal)fieldValue["col_a4"] : 0;
                record.ДатаПоступлення = (fieldValue["col_a5"] != DBNull.Value) ? DateTime.Parse(fieldValue["col_a5"].ToString()) : DateTime.MinValue;
                record.Ціна = (fieldValue["col_a6"] != DBNull.Value) ? (decimal)fieldValue["col_a6"] : 0;
                record.Сума = (fieldValue["col_a7"] != DBNull.Value) ? (decimal)fieldValue["col_a7"] : 0;
                record.Скидка = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a9"]);
                record.Підрозділ = new Довідники.СтруктураПідприємства_Pointer(fieldValue["col_b1"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_b2", record.НомерРядка);
                fieldValue.Add("col_o4", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_a1", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a2", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_a3", record.КількістьУпаковок);
                fieldValue.Add("col_a4", record.Кількість);
                fieldValue.Add("col_a5", record.ДатаПоступлення);
                fieldValue.Add("col_a6", record.Ціна);
                fieldValue.Add("col_a7", record.Сума);
                fieldValue.Add("col_a8", record.Скидка);
                fieldValue.Add("col_a9", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_b1", record.Підрозділ.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                ДатаПоступлення = DateTime.MinValue;
                Ціна = 0;
                Сума = 0;
                Скидка = 0;
                Склад = new Довідники.Склади_Pointer();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public DateTime ДатаПоступлення { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public decimal Скидка { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ПоступленняТоварівТаПослуг"
    
    public static class ПоступленняТоварівТаПослуг_Const
    {
        public const string TABLE = "tab_a32";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Валюта = "col_a3";
        public const string ГосподарськаОперація = "col_a4";
        public const string Підрозділ = "col_a5";
        public const string Склад = "col_a6";
        public const string Контрагент = "col_a7";
        public const string СумаДокументу = "col_a8";
        public const string ЗамовленняПостачальнику = "col_a9";
        public const string ДатаОплати = "col_b2";
        public const string ФормаОплати = "col_b3";
        public const string Узгоджений = "col_b4";
        public const string БанківськийрахунокОрганізації = "col_b5";
        public const string НомерВхідногоДокументу = "col_b6";
        public const string ДатаВхідногоДокументу = "col_b7";
        public const string БанківськийрахунокКонтрагента = "col_b8";
        public const string Договір = "col_b9";
        public const string Автор = "col_c1";
        public const string ВернутиТару = "col_c2";
        public const string ДатаПоверненняТари = "col_c3";
        public const string СпосібДоставки = "col_c4";
        public const string Організація = "col_c5";
        public const string Курс = "col_c6";
        public const string Кратність = "col_c7";
        public const string ЧасДоставкиЗ = "col_c8";
        public const string ЧасДоставкиДо = "col_c9";
        public const string Менеджер = "col_d1";
        public const string СтаттяРухуКоштів = "col_d2";
        public const string Каса = "col_d3";
        public const string Основа = "col_a1";
        public const string Коментар = "col_b1";
    }
	
    
    public class ПоступленняТоварівТаПослуг_Objest : DocumentObject
    {
        public ПоступленняТоварівТаПослуг_Objest() : base(Config.Kernel, "tab_a32", "ПоступленняТоварівТаПослуг",
             new string[] { "docname", "docnomer", "docdate", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_a9", "col_b2", "col_b3", "col_b4", "col_b5", "col_b6", "col_b7", "col_b8", "col_b9", "col_c1", "col_c2", "col_c3", "col_c4", "col_c5", "col_c6", "col_c7", "col_c8", "col_c9", "col_d1", "col_d2", "col_d3", "col_a1", "col_b1" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Валюта = new Довідники.Валюти_Pointer();
            ГосподарськаОперація = 0;
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Склад = new Довідники.Склади_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            СумаДокументу = 0;
            ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer();
            ДатаОплати = DateTime.MinValue;
            ФормаОплати = 0;
            Узгоджений = false;
            БанківськийрахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            НомерВхідногоДокументу = "";
            ДатаВхідногоДокументу = DateTime.MinValue;
            БанківськийрахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            Автор = new Довідники.Користувачі_Pointer();
            ВернутиТару = false;
            ДатаПоверненняТари = DateTime.MinValue;
            СпосібДоставки = 0;
            Організація = new Довідники.Організації_Pointer();
            Курс = 0;
            Кратність = 0;
            ЧасДоставкиЗ = DateTime.MinValue.TimeOfDay;
            ЧасДоставкиДо = DateTime.MinValue.TimeOfDay;
            Менеджер = new Довідники.Користувачі_Pointer();
            СтаттяРухуКоштів = new Довідники.СтаттяРухуКоштів_Pointer();
            Каса = new Довідники.Каси_Pointer();
            Основа = new UuidAndText();
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ПоступленняТоварівТаПослуг_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_a3"]);
                ГосподарськаОперація = (base.FieldValue["col_a4"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_a4"] : 0;
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a5"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_a6"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_a7"]);
                СумаДокументу = (base.FieldValue["col_a8"] != DBNull.Value) ? (decimal)base.FieldValue["col_a8"] : 0;
                ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer(base.FieldValue["col_a9"]);
                ДатаОплати = (base.FieldValue["col_b2"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_b2"].ToString()) : DateTime.MinValue;
                ФормаОплати = (base.FieldValue["col_b3"] != DBNull.Value) ? (Перелічення.ФормаОплати)base.FieldValue["col_b3"] : 0;
                Узгоджений = (base.FieldValue["col_b4"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_b4"].ToString()) : false;
                БанківськийрахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_b5"]);
                НомерВхідногоДокументу = base.FieldValue["col_b6"].ToString();
                ДатаВхідногоДокументу = (base.FieldValue["col_b7"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_b7"].ToString()) : DateTime.MinValue;
                БанківськийрахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer(base.FieldValue["col_b8"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_b9"]);
                Автор = new Довідники.Користувачі_Pointer(base.FieldValue["col_c1"]);
                ВернутиТару = (base.FieldValue["col_c2"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_c2"].ToString()) : false;
                ДатаПоверненняТари = (base.FieldValue["col_c3"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_c3"].ToString()) : DateTime.MinValue;
                СпосібДоставки = (base.FieldValue["col_c4"] != DBNull.Value) ? (Перелічення.СпособиДоставки)base.FieldValue["col_c4"] : 0;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_c5"]);
                Курс = (base.FieldValue["col_c6"] != DBNull.Value) ? (decimal)base.FieldValue["col_c6"] : 0;
                Кратність = (base.FieldValue["col_c7"] != DBNull.Value) ? (int)base.FieldValue["col_c7"] : 0;
                ЧасДоставкиЗ = (base.FieldValue["col_c8"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c8"].ToString()) : DateTime.MinValue.TimeOfDay;
                ЧасДоставкиДо = (base.FieldValue["col_c9"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c9"].ToString()) : DateTime.MinValue.TimeOfDay;
                Менеджер = new Довідники.Користувачі_Pointer(base.FieldValue["col_d1"]);
                СтаттяРухуКоштів = new Довідники.СтаттяРухуКоштів_Pointer(base.FieldValue["col_d2"]);
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_d3"]);
                Основа = (base.FieldValue["col_a1"] != DBNull.Value) ? (UuidAndText)base.FieldValue["col_a1"] : new UuidAndText();
                Коментар = base.FieldValue["col_b1"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ПоступленняТоварівТаПослуг_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_a3"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_a4"] = (int)ГосподарськаОперація;
            base.FieldValue["col_a5"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_a6"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_a7"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_a8"] = СумаДокументу;
            base.FieldValue["col_a9"] = ЗамовленняПостачальнику.UnigueID.UGuid;
            base.FieldValue["col_b2"] = ДатаОплати;
            base.FieldValue["col_b3"] = (int)ФормаОплати;
            base.FieldValue["col_b4"] = Узгоджений;
            base.FieldValue["col_b5"] = БанківськийрахунокОрганізації.UnigueID.UGuid;
            base.FieldValue["col_b6"] = НомерВхідногоДокументу;
            base.FieldValue["col_b7"] = ДатаВхідногоДокументу;
            base.FieldValue["col_b8"] = БанківськийрахунокКонтрагента.UnigueID.UGuid;
            base.FieldValue["col_b9"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_c1"] = Автор.UnigueID.UGuid;
            base.FieldValue["col_c2"] = ВернутиТару;
            base.FieldValue["col_c3"] = ДатаПоверненняТари;
            base.FieldValue["col_c4"] = (int)СпосібДоставки;
            base.FieldValue["col_c5"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_c6"] = Курс;
            base.FieldValue["col_c7"] = Кратність;
            base.FieldValue["col_c8"] = ЧасДоставкиЗ;
            base.FieldValue["col_c9"] = ЧасДоставкиДо;
            base.FieldValue["col_d1"] = Менеджер.UnigueID.UGuid;
            base.FieldValue["col_d2"] = СтаттяРухуКоштів.UnigueID.UGuid;
            base.FieldValue["col_d3"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_a1"] = Основа;
            base.FieldValue["col_b1"] = Коментар;
            
            BaseSave();
			ПоступленняТоварівТаПослуг_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ПоступленняТоварівТаПослуг_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ПоступленняТоварівТаПослуг_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ПоступленняТоварівТаПослуг_Objest Copy()
        {
            ПоступленняТоварівТаПослуг_Objest copy = new ПоступленняТоварівТаПослуг_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Валюта = Валюта;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Підрозділ = Підрозділ;
			copy.Склад = Склад;
			copy.Контрагент = Контрагент;
			copy.СумаДокументу = СумаДокументу;
			copy.ЗамовленняПостачальнику = ЗамовленняПостачальнику;
			copy.ДатаОплати = ДатаОплати;
			copy.ФормаОплати = ФормаОплати;
			copy.Узгоджений = Узгоджений;
			copy.БанківськийрахунокОрганізації = БанківськийрахунокОрганізації;
			copy.НомерВхідногоДокументу = НомерВхідногоДокументу;
			copy.ДатаВхідногоДокументу = ДатаВхідногоДокументу;
			copy.БанківськийрахунокКонтрагента = БанківськийрахунокКонтрагента;
			copy.Договір = Договір;
			copy.Автор = Автор;
			copy.ВернутиТару = ВернутиТару;
			copy.ДатаПоверненняТари = ДатаПоверненняТари;
			copy.СпосібДоставки = СпосібДоставки;
			copy.Організація = Організація;
			copy.Курс = Курс;
			copy.Кратність = Кратність;
			copy.ЧасДоставкиЗ = ЧасДоставкиЗ;
			copy.ЧасДоставкиДо = ЧасДоставкиДо;
			copy.Менеджер = Менеджер;
			copy.СтаттяРухуКоштів = СтаттяРухуКоштів;
			copy.Каса = Каса;
			copy.Основа = Основа;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ПоступленняТоварівТаПослуг_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a33" });
        }
        
        public ПоступленняТоварівТаПослуг_Pointer GetDocumentPointer()
        {
            ПоступленняТоварівТаПослуг_Pointer directoryPointer = new ПоступленняТоварівТаПослуг_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public decimal СумаДокументу { get; set; }
        public Документи.ЗамовленняПостачальнику_Pointer ЗамовленняПостачальнику { get; set; }
        public DateTime ДатаОплати { get; set; }
        public Перелічення.ФормаОплати ФормаОплати { get; set; }
        public bool Узгоджений { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийрахунокОрганізації { get; set; }
        public string НомерВхідногоДокументу { get; set; }
        public DateTime ДатаВхідногоДокументу { get; set; }
        public Довідники.БанківськіРахункиКонтрагентів_Pointer БанківськийрахунокКонтрагента { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Довідники.Користувачі_Pointer Автор { get; set; }
        public bool ВернутиТару { get; set; }
        public DateTime ДатаПоверненняТари { get; set; }
        public Перелічення.СпособиДоставки СпосібДоставки { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public decimal Курс { get; set; }
        public int Кратність { get; set; }
        public TimeSpan ЧасДоставкиЗ { get; set; }
        public TimeSpan ЧасДоставкиДо { get; set; }
        public Довідники.Користувачі_Pointer Менеджер { get; set; }
        public Довідники.СтаттяРухуКоштів_Pointer СтаттяРухуКоштів { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public UuidAndText Основа { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ПоступленняТоварівТаПослуг_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ПоступленняТоварівТаПослуг_Pointer : DocumentPointer
    {
        public ПоступленняТоварівТаПослуг_Pointer(object uid = null) : base(Config.Kernel, "tab_a32", "ПоступленняТоварівТаПослуг")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПоступленняТоварівТаПослуг_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a32", "ПоступленняТоварівТаПослуг")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ПоступленняТоварівТаПослуг_Pointer GetNewDocumentPointer()
        {
            return new ПоступленняТоварівТаПослуг_Pointer(base.UnigueID);
        }
		
        public ПоступленняТоварівТаПослуг_Pointer GetEmptyPointer()
        {
            return new ПоступленняТоварівТаПослуг_Pointer();
        }
		
        public ПоступленняТоварівТаПослуг_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослугObjestItem = new ПоступленняТоварівТаПослуг_Objest();
            ПоступленняТоварівТаПослугObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ПоступленняТоварівТаПослугObjestItem.Товари_TablePart.Read();
			}
			
            return ПоступленняТоварівТаПослугObjestItem;
        }
    }
    
    
    public class ПоступленняТоварівТаПослуг_Select : DocumentSelect
    {		
        public ПоступленняТоварівТаПослуг_Select() : base(Config.Kernel, "tab_a32") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПоступленняТоварівТаПослуг_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ПоступленняТоварівТаПослуг_Pointer Current { get; private set; }
    }
    
      
    public class ПоступленняТоварівТаПослуг_Товари_TablePart : DocumentTablePart
    {
        public ПоступленняТоварівТаПослуг_Товари_TablePart(ПоступленняТоварівТаПослуг_Objest owner) : base(Config.Kernel, "tab_a33",
             new string[] { "col_b3", "col_a9", "col_b1", "col_b4", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_b2" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_b3";
        public const string Номенклатура = "col_a9";
        public const string ХарактеристикаНоменклатури = "col_b1";
        public const string Серія = "col_b4";
        public const string Пакування = "col_a1";
        public const string КількістьУпаковок = "col_a2";
        public const string Кількість = "col_a3";
        public const string Ціна = "col_a4";
        public const string Сума = "col_a5";
        public const string Склад = "col_a6";
        public const string ЗамовленняПостачальнику = "col_a7";
        public const string Скидка = "col_a8";
        public const string Підрозділ = "col_b2";

        public ПоступленняТоварівТаПослуг_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_b3"] != DBNull.Value) ? (int)fieldValue["col_b3"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a9"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_b1"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_b4"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_a1"]);
                record.КількістьУпаковок = (fieldValue["col_a2"] != DBNull.Value) ? (int)fieldValue["col_a2"] : 0;
                record.Кількість = (fieldValue["col_a3"] != DBNull.Value) ? (decimal)fieldValue["col_a3"] : 0;
                record.Ціна = (fieldValue["col_a4"] != DBNull.Value) ? (decimal)fieldValue["col_a4"] : 0;
                record.Сума = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a6"]);
                record.ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer(fieldValue["col_a7"]);
                record.Скидка = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                record.Підрозділ = new Довідники.СтруктураПідприємства_Pointer(fieldValue["col_b2"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_b3", record.НомерРядка);
                fieldValue.Add("col_a9", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_b1", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_b4", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_a1", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_a2", record.КількістьУпаковок);
                fieldValue.Add("col_a3", record.Кількість);
                fieldValue.Add("col_a4", record.Ціна);
                fieldValue.Add("col_a5", record.Сума);
                fieldValue.Add("col_a6", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_a7", record.ЗамовленняПостачальнику.UnigueID.UGuid);
                fieldValue.Add("col_a8", record.Скидка);
                fieldValue.Add("col_b2", record.Підрозділ.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                Ціна = 0;
                Сума = 0;
                Склад = new Довідники.Склади_Pointer();
                ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer();
                Скидка = 0;
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public Документи.ЗамовленняПостачальнику_Pointer ЗамовленняПостачальнику { get; set; }
            public decimal Скидка { get; set; }
            public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ЗамовленняКлієнта"
    
    public static class ЗамовленняКлієнта_Const
    {
        public const string TABLE = "tab_a34";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Контрагент = "col_b4";
        public const string Організація = "col_b5";
        public const string Валюта = "col_b6";
        public const string СумаДокументу = "col_b7";
        public const string Склад = "col_b8";
        public const string Статус = "col_a1";
        public const string Узгоджений = "col_a2";
        public const string ФормаОплати = "col_a3";
        public const string БанківськийРахунок = "col_a4";
        public const string БанківськийРахунокКонтрагента = "col_a5";
        public const string Каса = "col_a6";
        public const string СумаАвансуДоЗабезпечення = "col_a7";
        public const string СумаПередоплатиДоВідгрузки = "col_a8";
        public const string ДатаВідгрузки = "col_b1";
        public const string АдресаДоставки = "col_a9";
        public const string ГосподарськаОперація = "col_b9";
        public const string Договір = "col_c2";
        public const string Підрозділ = "col_c3";
        public const string Автор = "col_c4";
        public const string СпосібДоставки = "col_c5";
        public const string ЧасДоставкиЗ = "col_c6";
        public const string ЧасДоставкиДо = "col_c7";
        public const string ПовернутиТару = "col_c8";
        public const string ДатаПоверненняТари = "col_c9";
        public const string Коментар = "col_c1";
    }
	
    
    public class ЗамовленняКлієнта_Objest : DocumentObject
    {
        public ЗамовленняКлієнта_Objest() : base(Config.Kernel, "tab_a34", "ЗамовленняКлієнта",
             new string[] { "docname", "docnomer", "docdate", "col_b4", "col_b5", "col_b6", "col_b7", "col_b8", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_b1", "col_a9", "col_b9", "col_c2", "col_c3", "col_c4", "col_c5", "col_c6", "col_c7", "col_c8", "col_c9", "col_c1" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Контрагент = new Довідники.Контрагенти_Pointer();
            Організація = new Довідники.Організації_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            СумаДокументу = 0;
            Склад = new Довідники.Склади_Pointer();
            Статус = 0;
            Узгоджений = false;
            ФормаОплати = 0;
            БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
            Каса = new Довідники.Каси_Pointer();
            СумаАвансуДоЗабезпечення = 0;
            СумаПередоплатиДоВідгрузки = 0;
            ДатаВідгрузки = DateTime.MinValue;
            АдресаДоставки = "";
            ГосподарськаОперація = 0;
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Автор = new Довідники.Користувачі_Pointer();
            СпосібДоставки = 0;
            ЧасДоставкиЗ = DateTime.MinValue.TimeOfDay;
            ЧасДоставкиДо = DateTime.MinValue.TimeOfDay;
            ПовернутиТару = false;
            ДатаПоверненняТари = DateTime.MinValue;
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ЗамовленняКлієнта_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_b4"]);
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_b5"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_b6"]);
                СумаДокументу = (base.FieldValue["col_b7"] != DBNull.Value) ? (decimal)base.FieldValue["col_b7"] : 0;
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_b8"]);
                Статус = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.СтатусиЗамовленьКлієнтів)base.FieldValue["col_a1"] : 0;
                Узгоджений = (base.FieldValue["col_a2"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_a2"].ToString()) : false;
                ФормаОплати = (base.FieldValue["col_a3"] != DBNull.Value) ? (Перелічення.ФормаОплати)base.FieldValue["col_a3"] : 0;
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_a4"]);
                БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer(base.FieldValue["col_a5"]);
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_a6"]);
                СумаАвансуДоЗабезпечення = (base.FieldValue["col_a7"] != DBNull.Value) ? (decimal)base.FieldValue["col_a7"] : 0;
                СумаПередоплатиДоВідгрузки = (base.FieldValue["col_a8"] != DBNull.Value) ? (decimal)base.FieldValue["col_a8"] : 0;
                ДатаВідгрузки = (base.FieldValue["col_b1"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_b1"].ToString()) : DateTime.MinValue;
                АдресаДоставки = base.FieldValue["col_a9"].ToString();
                ГосподарськаОперація = (base.FieldValue["col_b9"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_b9"] : 0;
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_c2"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_c3"]);
                Автор = new Довідники.Користувачі_Pointer(base.FieldValue["col_c4"]);
                СпосібДоставки = (base.FieldValue["col_c5"] != DBNull.Value) ? (Перелічення.СпособиДоставки)base.FieldValue["col_c5"] : 0;
                ЧасДоставкиЗ = (base.FieldValue["col_c6"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c6"].ToString()) : DateTime.MinValue.TimeOfDay;
                ЧасДоставкиДо = (base.FieldValue["col_c7"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c7"].ToString()) : DateTime.MinValue.TimeOfDay;
                ПовернутиТару = (base.FieldValue["col_c8"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_c8"].ToString()) : false;
                ДатаПоверненняТари = (base.FieldValue["col_c9"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_c9"].ToString()) : DateTime.MinValue;
                Коментар = base.FieldValue["col_c1"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ЗамовленняКлієнта_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_b4"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_b5"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_b6"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_b7"] = СумаДокументу;
            base.FieldValue["col_b8"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_a1"] = (int)Статус;
            base.FieldValue["col_a2"] = Узгоджений;
            base.FieldValue["col_a3"] = (int)ФормаОплати;
            base.FieldValue["col_a4"] = БанківськийРахунок.UnigueID.UGuid;
            base.FieldValue["col_a5"] = БанківськийРахунокКонтрагента.UnigueID.UGuid;
            base.FieldValue["col_a6"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_a7"] = СумаАвансуДоЗабезпечення;
            base.FieldValue["col_a8"] = СумаПередоплатиДоВідгрузки;
            base.FieldValue["col_b1"] = ДатаВідгрузки;
            base.FieldValue["col_a9"] = АдресаДоставки;
            base.FieldValue["col_b9"] = (int)ГосподарськаОперація;
            base.FieldValue["col_c2"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_c3"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_c4"] = Автор.UnigueID.UGuid;
            base.FieldValue["col_c5"] = (int)СпосібДоставки;
            base.FieldValue["col_c6"] = ЧасДоставкиЗ;
            base.FieldValue["col_c7"] = ЧасДоставкиДо;
            base.FieldValue["col_c8"] = ПовернутиТару;
            base.FieldValue["col_c9"] = ДатаПоверненняТари;
            base.FieldValue["col_c1"] = Коментар;
            
            BaseSave();
			ЗамовленняКлієнта_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ЗамовленняКлієнта_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ЗамовленняКлієнта_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ЗамовленняКлієнта_Objest Copy()
        {
            ЗамовленняКлієнта_Objest copy = new ЗамовленняКлієнта_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Контрагент = Контрагент;
			copy.Організація = Організація;
			copy.Валюта = Валюта;
			copy.СумаДокументу = СумаДокументу;
			copy.Склад = Склад;
			copy.Статус = Статус;
			copy.Узгоджений = Узгоджений;
			copy.ФормаОплати = ФормаОплати;
			copy.БанківськийРахунок = БанківськийРахунок;
			copy.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента;
			copy.Каса = Каса;
			copy.СумаАвансуДоЗабезпечення = СумаАвансуДоЗабезпечення;
			copy.СумаПередоплатиДоВідгрузки = СумаПередоплатиДоВідгрузки;
			copy.ДатаВідгрузки = ДатаВідгрузки;
			copy.АдресаДоставки = АдресаДоставки;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Договір = Договір;
			copy.Підрозділ = Підрозділ;
			copy.Автор = Автор;
			copy.СпосібДоставки = СпосібДоставки;
			copy.ЧасДоставкиЗ = ЧасДоставкиЗ;
			copy.ЧасДоставкиДо = ЧасДоставкиДо;
			copy.ПовернутиТару = ПовернутиТару;
			copy.ДатаПоверненняТари = ДатаПоверненняТари;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ЗамовленняКлієнта_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a35" });
        }
        
        public ЗамовленняКлієнта_Pointer GetDocumentPointer()
        {
            ЗамовленняКлієнта_Pointer directoryPointer = new ЗамовленняКлієнта_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public decimal СумаДокументу { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Перелічення.СтатусиЗамовленьКлієнтів Статус { get; set; }
        public bool Узгоджений { get; set; }
        public Перелічення.ФормаОплати ФормаОплати { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
        public Довідники.БанківськіРахункиКонтрагентів_Pointer БанківськийРахунокКонтрагента { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public decimal СумаАвансуДоЗабезпечення { get; set; }
        public decimal СумаПередоплатиДоВідгрузки { get; set; }
        public DateTime ДатаВідгрузки { get; set; }
        public string АдресаДоставки { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Користувачі_Pointer Автор { get; set; }
        public Перелічення.СпособиДоставки СпосібДоставки { get; set; }
        public TimeSpan ЧасДоставкиЗ { get; set; }
        public TimeSpan ЧасДоставкиДо { get; set; }
        public bool ПовернутиТару { get; set; }
        public DateTime ДатаПоверненняТари { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ЗамовленняКлієнта_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ЗамовленняКлієнта_Pointer : DocumentPointer
    {
        public ЗамовленняКлієнта_Pointer(object uid = null) : base(Config.Kernel, "tab_a34", "ЗамовленняКлієнта")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ЗамовленняКлієнта_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a34", "ЗамовленняКлієнта")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ЗамовленняКлієнта_Pointer GetNewDocumentPointer()
        {
            return new ЗамовленняКлієнта_Pointer(base.UnigueID);
        }
		
        public ЗамовленняКлієнта_Pointer GetEmptyPointer()
        {
            return new ЗамовленняКлієнта_Pointer();
        }
		
        public ЗамовленняКлієнта_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ЗамовленняКлієнта_Objest ЗамовленняКлієнтаObjestItem = new ЗамовленняКлієнта_Objest();
            ЗамовленняКлієнтаObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ЗамовленняКлієнтаObjestItem.Товари_TablePart.Read();
			}
			
            return ЗамовленняКлієнтаObjestItem;
        }
    }
    
    
    public class ЗамовленняКлієнта_Select : DocumentSelect
    {		
        public ЗамовленняКлієнта_Select() : base(Config.Kernel, "tab_a34") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ЗамовленняКлієнта_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ЗамовленняКлієнта_Pointer Current { get; private set; }
    }
    
      
    public class ЗамовленняКлієнта_Товари_TablePart : DocumentTablePart
    {
        public ЗамовленняКлієнта_Товари_TablePart(ЗамовленняКлієнта_Objest owner) : base(Config.Kernel, "tab_a35",
             new string[] { "col_a2", "col_b9", "col_c1", "col_c2", "col_c3", "col_c4", "col_c5", "col_c6", "col_c7", "col_c8", "col_a1" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a2";
        public const string Номенклатура = "col_b9";
        public const string ХарактеристикаНоменклатури = "col_c1";
        public const string Пакування = "col_c2";
        public const string КількістьУпаковок = "col_c3";
        public const string Кількість = "col_c4";
        public const string ВидЦіни = "col_c5";
        public const string Ціна = "col_c6";
        public const string Сума = "col_c7";
        public const string Скидка = "col_c8";
        public const string Склад = "col_a1";

        public ЗамовленняКлієнта_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a2"] != DBNull.Value) ? (int)fieldValue["col_a2"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_b9"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_c1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_c2"]);
                record.КількістьУпаковок = (fieldValue["col_c3"] != DBNull.Value) ? (int)fieldValue["col_c3"] : 0;
                record.Кількість = (fieldValue["col_c4"] != DBNull.Value) ? (decimal)fieldValue["col_c4"] : 0;
                record.ВидЦіни = new Довідники.ВидиЦін_Pointer(fieldValue["col_c5"]);
                record.Ціна = (fieldValue["col_c6"] != DBNull.Value) ? (decimal)fieldValue["col_c6"] : 0;
                record.Сума = (fieldValue["col_c7"] != DBNull.Value) ? (decimal)fieldValue["col_c7"] : 0;
                record.Скидка = (fieldValue["col_c8"] != DBNull.Value) ? (decimal)fieldValue["col_c8"] : 0;
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a1"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a2", record.НомерРядка);
                fieldValue.Add("col_b9", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_c1", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_c2", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_c3", record.КількістьУпаковок);
                fieldValue.Add("col_c4", record.Кількість);
                fieldValue.Add("col_c5", record.ВидЦіни.UnigueID.UGuid);
                fieldValue.Add("col_c6", record.Ціна);
                fieldValue.Add("col_c7", record.Сума);
                fieldValue.Add("col_c8", record.Скидка);
                fieldValue.Add("col_a1", record.Склад.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                ВидЦіни = new Довідники.ВидиЦін_Pointer();
                Ціна = 0;
                Сума = 0;
                Скидка = 0;
                Склад = new Довідники.Склади_Pointer();
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public decimal Скидка { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "РеалізаціяТоварівТаПослуг"
    
    public static class РеалізаціяТоварівТаПослуг_Const
    {
        public const string TABLE = "tab_a36";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_a1";
        public const string Валюта = "col_a2";
        public const string БанківськийРахунокОрганізації = "col_a3";
        public const string БанківськийРахунокКонтрагента = "col_a4";
        public const string ДатаОплати = "col_a5";
        public const string ЗамовленняКлієнта = "col_a6";
        public const string Контрагент = "col_a7";
        public const string СумаДокументу = "col_a8";
        public const string Підрозділ = "col_a9";
        public const string Склад = "col_b1";
        public const string ФормаОплати = "col_b3";
        public const string ГосподарськаОперація = "col_b4";
        public const string Каса = "col_b5";
        public const string Договір = "col_b6";
        public const string Основа = "col_b7";
        public const string Статус = "col_b8";
        public const string Автор = "col_b9";
        public const string СумаПередоплати = "col_c1";
        public const string СумаПередоплатиЗаТару = "col_c2";
        public const string СпосібДоставки = "col_c3";
        public const string ЧасДоставкиЗ = "col_c4";
        public const string ЧасДоставкиДо = "col_c5";
        public const string ПовернутиТару = "col_c6";
        public const string ДатаПоверненняТари = "col_c7";
        public const string Курс = "col_c8";
        public const string Кратність = "col_d2";
        public const string Коментар = "col_b2";
    }
	
    
    public class РеалізаціяТоварівТаПослуг_Objest : DocumentObject
    {
        public РеалізаціяТоварівТаПослуг_Objest() : base(Config.Kernel, "tab_a36", "РеалізаціяТоварівТаПослуг",
             new string[] { "docname", "docnomer", "docdate", "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6", "col_a7", "col_a8", "col_a9", "col_b1", "col_b3", "col_b4", "col_b5", "col_b6", "col_b7", "col_b8", "col_b9", "col_c1", "col_c2", "col_c3", "col_c4", "col_c5", "col_c6", "col_c7", "col_c8", "col_d2", "col_b2" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            БанківськийРахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
            ДатаОплати = DateTime.MinValue;
            ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            СумаДокументу = 0;
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Склад = new Довідники.Склади_Pointer();
            ФормаОплати = 0;
            ГосподарськаОперація = 0;
            Каса = new Довідники.Каси_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            Основа = new UuidAndText();
            Статус = 0;
            Автор = new Довідники.Користувачі_Pointer();
            СумаПередоплати = 0;
            СумаПередоплатиЗаТару = 0;
            СпосібДоставки = 0;
            ЧасДоставкиЗ = DateTime.MinValue.TimeOfDay;
            ЧасДоставкиДо = DateTime.MinValue.TimeOfDay;
            ПовернутиТару = false;
            ДатаПоверненняТари = DateTime.MinValue;
            Курс = 0;
            Кратність = 0;
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new РеалізаціяТоварівТаПослуг_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_a1"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_a2"]);
                БанківськийРахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_a3"]);
                БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer(base.FieldValue["col_a4"]);
                ДатаОплати = (base.FieldValue["col_a5"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_a5"].ToString()) : DateTime.MinValue;
                ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer(base.FieldValue["col_a6"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_a7"]);
                СумаДокументу = (base.FieldValue["col_a8"] != DBNull.Value) ? (decimal)base.FieldValue["col_a8"] : 0;
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a9"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_b1"]);
                ФормаОплати = (base.FieldValue["col_b3"] != DBNull.Value) ? (Перелічення.ФормаОплати)base.FieldValue["col_b3"] : 0;
                ГосподарськаОперація = (base.FieldValue["col_b4"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_b4"] : 0;
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_b5"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_b6"]);
                Основа = (base.FieldValue["col_b7"] != DBNull.Value) ? (UuidAndText)base.FieldValue["col_b7"] : new UuidAndText();
                Статус = (base.FieldValue["col_b8"] != DBNull.Value) ? (Перелічення.СтатусиРеалізаціїТоварівТаПослуг)base.FieldValue["col_b8"] : 0;
                Автор = new Довідники.Користувачі_Pointer(base.FieldValue["col_b9"]);
                СумаПередоплати = (base.FieldValue["col_c1"] != DBNull.Value) ? (decimal)base.FieldValue["col_c1"] : 0;
                СумаПередоплатиЗаТару = (base.FieldValue["col_c2"] != DBNull.Value) ? (decimal)base.FieldValue["col_c2"] : 0;
                СпосібДоставки = (base.FieldValue["col_c3"] != DBNull.Value) ? (Перелічення.СпособиДоставки)base.FieldValue["col_c3"] : 0;
                ЧасДоставкиЗ = (base.FieldValue["col_c4"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c4"].ToString()) : DateTime.MinValue.TimeOfDay;
                ЧасДоставкиДо = (base.FieldValue["col_c5"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_c5"].ToString()) : DateTime.MinValue.TimeOfDay;
                ПовернутиТару = (base.FieldValue["col_c6"] != DBNull.Value) ? bool.Parse(base.FieldValue["col_c6"].ToString()) : false;
                ДатаПоверненняТари = (base.FieldValue["col_c7"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["col_c7"].ToString()) : DateTime.MinValue;
                Курс = (base.FieldValue["col_c8"] != DBNull.Value) ? (decimal)base.FieldValue["col_c8"] : 0;
                Кратність = (base.FieldValue["col_d2"] != DBNull.Value) ? (int)base.FieldValue["col_d2"] : 0;
                Коментар = base.FieldValue["col_b2"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            РеалізаціяТоварівТаПослуг_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_a1"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_a2"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_a3"] = БанківськийРахунокОрганізації.UnigueID.UGuid;
            base.FieldValue["col_a4"] = БанківськийРахунокКонтрагента.UnigueID.UGuid;
            base.FieldValue["col_a5"] = ДатаОплати;
            base.FieldValue["col_a6"] = ЗамовленняКлієнта.UnigueID.UGuid;
            base.FieldValue["col_a7"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_a8"] = СумаДокументу;
            base.FieldValue["col_a9"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_b1"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_b3"] = (int)ФормаОплати;
            base.FieldValue["col_b4"] = (int)ГосподарськаОперація;
            base.FieldValue["col_b5"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_b6"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_b7"] = Основа;
            base.FieldValue["col_b8"] = (int)Статус;
            base.FieldValue["col_b9"] = Автор.UnigueID.UGuid;
            base.FieldValue["col_c1"] = СумаПередоплати;
            base.FieldValue["col_c2"] = СумаПередоплатиЗаТару;
            base.FieldValue["col_c3"] = (int)СпосібДоставки;
            base.FieldValue["col_c4"] = ЧасДоставкиЗ;
            base.FieldValue["col_c5"] = ЧасДоставкиДо;
            base.FieldValue["col_c6"] = ПовернутиТару;
            base.FieldValue["col_c7"] = ДатаПоверненняТари;
            base.FieldValue["col_c8"] = Курс;
            base.FieldValue["col_d2"] = Кратність;
            base.FieldValue["col_b2"] = Коментар;
            
            BaseSave();
			РеалізаціяТоварівТаПослуг_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = РеалізаціяТоварівТаПослуг_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            РеалізаціяТоварівТаПослуг_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public РеалізаціяТоварівТаПослуг_Objest Copy()
        {
            РеалізаціяТоварівТаПослуг_Objest copy = new РеалізаціяТоварівТаПослуг_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Валюта = Валюта;
			copy.БанківськийРахунокОрганізації = БанківськийРахунокОрганізації;
			copy.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента;
			copy.ДатаОплати = ДатаОплати;
			copy.ЗамовленняКлієнта = ЗамовленняКлієнта;
			copy.Контрагент = Контрагент;
			copy.СумаДокументу = СумаДокументу;
			copy.Підрозділ = Підрозділ;
			copy.Склад = Склад;
			copy.ФормаОплати = ФормаОплати;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Каса = Каса;
			copy.Договір = Договір;
			copy.Основа = Основа;
			copy.Статус = Статус;
			copy.Автор = Автор;
			copy.СумаПередоплати = СумаПередоплати;
			copy.СумаПередоплатиЗаТару = СумаПередоплатиЗаТару;
			copy.СпосібДоставки = СпосібДоставки;
			copy.ЧасДоставкиЗ = ЧасДоставкиЗ;
			copy.ЧасДоставкиДо = ЧасДоставкиДо;
			copy.ПовернутиТару = ПовернутиТару;
			copy.ДатаПоверненняТари = ДатаПоверненняТари;
			copy.Курс = Курс;
			copy.Кратність = Кратність;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    РеалізаціяТоварівТаПослуг_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a37" });
        }
        
        public РеалізаціяТоварівТаПослуг_Pointer GetDocumentPointer()
        {
            РеалізаціяТоварівТаПослуг_Pointer directoryPointer = new РеалізаціяТоварівТаПослуг_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунокОрганізації { get; set; }
        public Довідники.БанківськіРахункиКонтрагентів_Pointer БанківськийРахунокКонтрагента { get; set; }
        public DateTime ДатаОплати { get; set; }
        public Документи.ЗамовленняКлієнта_Pointer ЗамовленняКлієнта { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public decimal СумаДокументу { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Перелічення.ФормаОплати ФормаОплати { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public UuidAndText Основа { get; set; }
        public Перелічення.СтатусиРеалізаціїТоварівТаПослуг Статус { get; set; }
        public Довідники.Користувачі_Pointer Автор { get; set; }
        public decimal СумаПередоплати { get; set; }
        public decimal СумаПередоплатиЗаТару { get; set; }
        public Перелічення.СпособиДоставки СпосібДоставки { get; set; }
        public TimeSpan ЧасДоставкиЗ { get; set; }
        public TimeSpan ЧасДоставкиДо { get; set; }
        public bool ПовернутиТару { get; set; }
        public DateTime ДатаПоверненняТари { get; set; }
        public decimal Курс { get; set; }
        public int Кратність { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public РеалізаціяТоварівТаПослуг_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class РеалізаціяТоварівТаПослуг_Pointer : DocumentPointer
    {
        public РеалізаціяТоварівТаПослуг_Pointer(object uid = null) : base(Config.Kernel, "tab_a36", "РеалізаціяТоварівТаПослуг")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public РеалізаціяТоварівТаПослуг_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a36", "РеалізаціяТоварівТаПослуг")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public РеалізаціяТоварівТаПослуг_Pointer GetNewDocumentPointer()
        {
            return new РеалізаціяТоварівТаПослуг_Pointer(base.UnigueID);
        }
		
        public РеалізаціяТоварівТаПослуг_Pointer GetEmptyPointer()
        {
            return new РеалізаціяТоварівТаПослуг_Pointer();
        }
		
        public РеалізаціяТоварівТаПослуг_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослугObjestItem = new РеалізаціяТоварівТаПослуг_Objest();
            РеалізаціяТоварівТаПослугObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				РеалізаціяТоварівТаПослугObjestItem.Товари_TablePart.Read();
			}
			
            return РеалізаціяТоварівТаПослугObjestItem;
        }
    }
    
    
    public class РеалізаціяТоварівТаПослуг_Select : DocumentSelect
    {		
        public РеалізаціяТоварівТаПослуг_Select() : base(Config.Kernel, "tab_a36") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new РеалізаціяТоварівТаПослуг_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public РеалізаціяТоварівТаПослуг_Pointer Current { get; private set; }
    }
    
      
    public class РеалізаціяТоварівТаПослуг_Товари_TablePart : DocumentTablePart
    {
        public РеалізаціяТоварівТаПослуг_Товари_TablePart(РеалізаціяТоварівТаПослуг_Objest owner) : base(Config.Kernel, "tab_a37",
             new string[] { "col_a1", "col_d2", "col_d3", "col_a2", "col_d4", "col_d5", "col_d6", "col_d7", "col_d8", "col_d9", "col_e1", "col_e2", "col_a3", "col_e3" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string Номенклатура = "col_d2";
        public const string ХарактеристикаНоменклатури = "col_d3";
        public const string Серія = "col_a2";
        public const string Пакування = "col_d4";
        public const string КількістьУпаковок = "col_d5";
        public const string Кількість = "col_d6";
        public const string ВидЦіни = "col_d7";
        public const string Ціна = "col_d8";
        public const string Сума = "col_d9";
        public const string Склад = "col_e1";
        public const string ЗамовленняКлієнта = "col_e2";
        public const string РахунокФактура = "col_a3";
        public const string Скидка = "col_e3";

        public РеалізаціяТоварівТаПослуг_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_d2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_d3"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a2"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_d4"]);
                record.КількістьУпаковок = (fieldValue["col_d5"] != DBNull.Value) ? (int)fieldValue["col_d5"] : 0;
                record.Кількість = (fieldValue["col_d6"] != DBNull.Value) ? (decimal)fieldValue["col_d6"] : 0;
                record.ВидЦіни = new Довідники.ВидиЦін_Pointer(fieldValue["col_d7"]);
                record.Ціна = (fieldValue["col_d8"] != DBNull.Value) ? (decimal)fieldValue["col_d8"] : 0;
                record.Сума = (fieldValue["col_d9"] != DBNull.Value) ? (decimal)fieldValue["col_d9"] : 0;
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_e1"]);
                record.ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer(fieldValue["col_e2"]);
                record.РахунокФактура = new Документи.РахунокФактура_Pointer(fieldValue["col_a3"]);
                record.Скидка = (fieldValue["col_e3"] != DBNull.Value) ? (decimal)fieldValue["col_e3"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_d2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_d3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a2", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_d4", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_d5", record.КількістьУпаковок);
                fieldValue.Add("col_d6", record.Кількість);
                fieldValue.Add("col_d7", record.ВидЦіни.UnigueID.UGuid);
                fieldValue.Add("col_d8", record.Ціна);
                fieldValue.Add("col_d9", record.Сума);
                fieldValue.Add("col_e1", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_e2", record.ЗамовленняКлієнта.UnigueID.UGuid);
                fieldValue.Add("col_a3", record.РахунокФактура.UnigueID.UGuid);
                fieldValue.Add("col_e3", record.Скидка);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                ВидЦіни = new Довідники.ВидиЦін_Pointer();
                Ціна = 0;
                Сума = 0;
                Склад = new Довідники.Склади_Pointer();
                ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer();
                РахунокФактура = new Документи.РахунокФактура_Pointer();
                Скидка = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public Документи.ЗамовленняКлієнта_Pointer ЗамовленняКлієнта { get; set; }
            public Документи.РахунокФактура_Pointer РахунокФактура { get; set; }
            public decimal Скидка { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ВстановленняЦінНоменклатури"
    
    public static class ВстановленняЦінНоменклатури_Const
    {
        public const string TABLE = "tab_a42";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_a2";
        public const string Валюта = "col_a1";
        public const string ВидЦіни = "col_a3";
        public const string Коментар = "col_g9";
    }
	
    
    public class ВстановленняЦінНоменклатури_Objest : DocumentObject
    {
        public ВстановленняЦінНоменклатури_Objest() : base(Config.Kernel, "tab_a42", "ВстановленняЦінНоменклатури",
             new string[] { "docname", "docnomer", "docdate", "col_a2", "col_a1", "col_a3", "col_g9" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            ВидЦіни = new Довідники.ВидиЦін_Pointer();
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ВстановленняЦінНоменклатури_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_a2"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_a1"]);
                ВидЦіни = new Довідники.ВидиЦін_Pointer(base.FieldValue["col_a3"]);
                Коментар = base.FieldValue["col_g9"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ВстановленняЦінНоменклатури_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_a2"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_a1"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_a3"] = ВидЦіни.UnigueID.UGuid;
            base.FieldValue["col_g9"] = Коментар;
            
            BaseSave();
			ВстановленняЦінНоменклатури_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ВстановленняЦінНоменклатури_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ВстановленняЦінНоменклатури_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ВстановленняЦінНоменклатури_Objest Copy()
        {
            ВстановленняЦінНоменклатури_Objest copy = new ВстановленняЦінНоменклатури_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Валюта = Валюта;
			copy.ВидЦіни = ВидЦіни;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ВстановленняЦінНоменклатури_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a43" });
        }
        
        public ВстановленняЦінНоменклатури_Pointer GetDocumentPointer()
        {
            ВстановленняЦінНоменклатури_Pointer directoryPointer = new ВстановленняЦінНоменклатури_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ВстановленняЦінНоменклатури_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ВстановленняЦінНоменклатури_Pointer : DocumentPointer
    {
        public ВстановленняЦінНоменклатури_Pointer(object uid = null) : base(Config.Kernel, "tab_a42", "ВстановленняЦінНоменклатури")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВстановленняЦінНоменклатури_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a42", "ВстановленняЦінНоменклатури")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ВстановленняЦінНоменклатури_Pointer GetNewDocumentPointer()
        {
            return new ВстановленняЦінНоменклатури_Pointer(base.UnigueID);
        }
		
        public ВстановленняЦінНоменклатури_Pointer GetEmptyPointer()
        {
            return new ВстановленняЦінНоменклатури_Pointer();
        }
		
        public ВстановленняЦінНоменклатури_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ВстановленняЦінНоменклатури_Objest ВстановленняЦінНоменклатуриObjestItem = new ВстановленняЦінНоменклатури_Objest();
            ВстановленняЦінНоменклатуриObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ВстановленняЦінНоменклатуриObjestItem.Товари_TablePart.Read();
			}
			
            return ВстановленняЦінНоменклатуриObjestItem;
        }
    }
    
    
    public class ВстановленняЦінНоменклатури_Select : DocumentSelect
    {		
        public ВстановленняЦінНоменклатури_Select() : base(Config.Kernel, "tab_a42") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВстановленняЦінНоменклатури_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ВстановленняЦінНоменклатури_Pointer Current { get; private set; }
    }
    
      
    public class ВстановленняЦінНоменклатури_Товари_TablePart : DocumentTablePart
    {
        public ВстановленняЦінНоменклатури_Товари_TablePart(ВстановленняЦінНоменклатури_Objest owner) : base(Config.Kernel, "tab_a43",
             new string[] { "col_a1", "col_h1", "col_h2", "col_h3", "col_h4", "col_h5" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string Номенклатура = "col_h1";
        public const string ХарактеристикаНоменклатури = "col_h2";
        public const string Пакування = "col_h3";
        public const string ВидЦіни = "col_h4";
        public const string Ціна = "col_h5";

        public ВстановленняЦінНоменклатури_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_h1"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_h2"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_h3"]);
                record.ВидЦіни = new Довідники.ВидиЦін_Pointer(fieldValue["col_h4"]);
                record.Ціна = (fieldValue["col_h5"] != DBNull.Value) ? (decimal)fieldValue["col_h5"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_h1", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_h2", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_h3", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_h4", record.ВидЦіни.UnigueID.UGuid);
                fieldValue.Add("col_h5", record.Ціна);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                ВидЦіни = new Довідники.ВидиЦін_Pointer();
                Ціна = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
            public decimal Ціна { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ПрихіднийКасовийОрдер"
    
    public static class ПрихіднийКасовийОрдер_Const
    {
        public const string TABLE = "tab_a44";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_h8";
        public const string Каса = "col_h9";
        public const string СумаДокументу = "col_i1";
        public const string ГосподарськаОперація = "col_i2";
        public const string Основа = "col_i3";
        public const string Контрагент = "col_i4";
        public const string Договір = "col_a6";
        public const string БанківськийРахунок = "col_i5";
        public const string Валюта = "col_i6";
        public const string СтаттяРухуКоштів = "col_a1";
        public const string КасаВідправник = "col_a2";
        public const string Курс = "col_a4";
        public const string Коментар = "col_a3";
    }
	
    
    public class ПрихіднийКасовийОрдер_Objest : DocumentObject
    {
        public ПрихіднийКасовийОрдер_Objest() : base(Config.Kernel, "tab_a44", "ПрихіднийКасовийОрдер",
             new string[] { "docname", "docnomer", "docdate", "col_h8", "col_h9", "col_i1", "col_i2", "col_i3", "col_i4", "col_a6", "col_i5", "col_i6", "col_a1", "col_a2", "col_a4", "col_a3" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Каса = new Довідники.Каси_Pointer();
            СумаДокументу = 0;
            ГосподарськаОперація = 0;
            Основа = new UuidAndText();
            Контрагент = new Довідники.Контрагенти_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            СтаттяРухуКоштів = new Довідники.СтаттяРухуКоштів_Pointer();
            КасаВідправник = new Довідники.Каси_Pointer();
            Курс = 0;
            Коментар = "";
            
            //Табличні частини
            РозшифруванняПлатежу_TablePart = new ПрихіднийКасовийОрдер_РозшифруванняПлатежу_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_h8"]);
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_h9"]);
                СумаДокументу = (base.FieldValue["col_i1"] != DBNull.Value) ? (decimal)base.FieldValue["col_i1"] : 0;
                ГосподарськаОперація = (base.FieldValue["col_i2"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_i2"] : 0;
                Основа = (base.FieldValue["col_i3"] != DBNull.Value) ? (UuidAndText)base.FieldValue["col_i3"] : new UuidAndText();
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_i4"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_a6"]);
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_i5"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_i6"]);
                СтаттяРухуКоштів = new Довідники.СтаттяРухуКоштів_Pointer(base.FieldValue["col_a1"]);
                КасаВідправник = new Довідники.Каси_Pointer(base.FieldValue["col_a2"]);
                Курс = (base.FieldValue["col_a4"] != DBNull.Value) ? (decimal)base.FieldValue["col_a4"] : 0;
                Коментар = base.FieldValue["col_a3"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ПрихіднийКасовийОрдер_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_h8"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_h9"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_i1"] = СумаДокументу;
            base.FieldValue["col_i2"] = (int)ГосподарськаОперація;
            base.FieldValue["col_i3"] = Основа;
            base.FieldValue["col_i4"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_a6"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_i5"] = БанківськийРахунок.UnigueID.UGuid;
            base.FieldValue["col_i6"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_a1"] = СтаттяРухуКоштів.UnigueID.UGuid;
            base.FieldValue["col_a2"] = КасаВідправник.UnigueID.UGuid;
            base.FieldValue["col_a4"] = Курс;
            base.FieldValue["col_a3"] = Коментар;
            
            BaseSave();
			ПрихіднийКасовийОрдер_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ПрихіднийКасовийОрдер_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ПрихіднийКасовийОрдер_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ПрихіднийКасовийОрдер_Objest Copy()
        {
            ПрихіднийКасовийОрдер_Objest copy = new ПрихіднийКасовийОрдер_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Каса = Каса;
			copy.СумаДокументу = СумаДокументу;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Основа = Основа;
			copy.Контрагент = Контрагент;
			copy.Договір = Договір;
			copy.БанківськийРахунок = БанківськийРахунок;
			copy.Валюта = Валюта;
			copy.СтаттяРухуКоштів = СтаттяРухуКоштів;
			copy.КасаВідправник = КасаВідправник;
			copy.Курс = Курс;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ПрихіднийКасовийОрдер_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a47" });
        }
        
        public ПрихіднийКасовийОрдер_Pointer GetDocumentPointer()
        {
            ПрихіднийКасовийОрдер_Pointer directoryPointer = new ПрихіднийКасовийОрдер_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public decimal СумаДокументу { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public UuidAndText Основа { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Довідники.СтаттяРухуКоштів_Pointer СтаттяРухуКоштів { get; set; }
        public Довідники.Каси_Pointer КасаВідправник { get; set; }
        public decimal Курс { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ПрихіднийКасовийОрдер_РозшифруванняПлатежу_TablePart РозшифруванняПлатежу_TablePart { get; set; }
        
    }
    
    
    public class ПрихіднийКасовийОрдер_Pointer : DocumentPointer
    {
        public ПрихіднийКасовийОрдер_Pointer(object uid = null) : base(Config.Kernel, "tab_a44", "ПрихіднийКасовийОрдер")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПрихіднийКасовийОрдер_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a44", "ПрихіднийКасовийОрдер")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ПрихіднийКасовийОрдер_Pointer GetNewDocumentPointer()
        {
            return new ПрихіднийКасовийОрдер_Pointer(base.UnigueID);
        }
		
        public ПрихіднийКасовийОрдер_Pointer GetEmptyPointer()
        {
            return new ПрихіднийКасовийОрдер_Pointer();
        }
		
        public ПрихіднийКасовийОрдер_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ПрихіднийКасовийОрдер_Objest ПрихіднийКасовийОрдерObjestItem = new ПрихіднийКасовийОрдер_Objest();
            ПрихіднийКасовийОрдерObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ПрихіднийКасовийОрдерObjestItem.РозшифруванняПлатежу_TablePart.Read();
			}
			
            return ПрихіднийКасовийОрдерObjestItem;
        }
    }
    
    
    public class ПрихіднийКасовийОрдер_Select : DocumentSelect
    {		
        public ПрихіднийКасовийОрдер_Select() : base(Config.Kernel, "tab_a44") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПрихіднийКасовийОрдер_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ПрихіднийКасовийОрдер_Pointer Current { get; private set; }
    }
    
      
    public class ПрихіднийКасовийОрдер_РозшифруванняПлатежу_TablePart : DocumentTablePart
    {
        public ПрихіднийКасовийОрдер_РозшифруванняПлатежу_TablePart(ПрихіднийКасовийОрдер_Objest owner) : base(Config.Kernel, "tab_a47",
             new string[] { "col_a1", "col_j4", "col_j5", "col_j6", "col_j7", "col_j8" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string Замовлення = "col_j4";
        public const string Сума = "col_j5";
        public const string Підрозділ = "col_j6";
        public const string ВалютаВзаєморозрахунків = "col_j7";
        public const string Організація = "col_j8";

        public ПрихіднийКасовийОрдер_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.Замовлення = fieldValue["col_j4"].ToString();
                record.Сума = (fieldValue["col_j5"] != DBNull.Value) ? (decimal)fieldValue["col_j5"] : 0;
                record.Підрозділ = new Довідники.СтруктураПідприємства_Pointer(fieldValue["col_j6"]);
                record.ВалютаВзаєморозрахунків = new Довідники.Валюти_Pointer(fieldValue["col_j7"]);
                record.Організація = new Довідники.Організації_Pointer(fieldValue["col_j8"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_j4", record.Замовлення);
                fieldValue.Add("col_j5", record.Сума);
                fieldValue.Add("col_j6", record.Підрозділ.UnigueID.UGuid);
                fieldValue.Add("col_j7", record.ВалютаВзаєморозрахунків.UnigueID.UGuid);
                fieldValue.Add("col_j8", record.Організація.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Замовлення = "";
                Сума = 0;
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
                ВалютаВзаєморозрахунків = new Довідники.Валюти_Pointer();
                Організація = new Довідники.Організації_Pointer();
                
            }
            public int НомерРядка { get; set; }
            public string Замовлення { get; set; }
            public decimal Сума { get; set; }
            public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
            public Довідники.Валюти_Pointer ВалютаВзаєморозрахунків { get; set; }
            public Довідники.Організації_Pointer Організація { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "РозхіднийКасовийОрдер"
    
    public static class РозхіднийКасовийОрдер_Const
    {
        public const string TABLE = "tab_a48";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_k2";
        public const string Каса = "col_k3";
        public const string СумаДокументу = "col_a3";
        public const string СумаДокументуПоКурсу = "col_a5";
        public const string ГосподарськаОперація = "col_k5";
        public const string ОрганізаціяОтримувач = "col_k4";
        public const string Контрагент = "col_k7";
        public const string Договір = "col_a4";
        public const string БанківськийРахунок = "col_k8";
        public const string Валюта = "col_k9";
        public const string СтаттяРухуКоштів = "col_l2";
        public const string КасаОтримувач = "col_k6";
        public const string Курс = "col_a2";
        public const string Основа = "col_a1";
        public const string Коментар = "col_l1";
    }
	
    
    public class РозхіднийКасовийОрдер_Objest : DocumentObject
    {
        public РозхіднийКасовийОрдер_Objest() : base(Config.Kernel, "tab_a48", "РозхіднийКасовийОрдер",
             new string[] { "docname", "docnomer", "docdate", "col_k2", "col_k3", "col_a3", "col_a5", "col_k5", "col_k4", "col_k7", "col_a4", "col_k8", "col_k9", "col_l2", "col_k6", "col_a2", "col_a1", "col_l1" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Каса = new Довідники.Каси_Pointer();
            СумаДокументу = 0;
            СумаДокументуПоКурсу = 0;
            ГосподарськаОперація = 0;
            ОрганізаціяОтримувач = new Довідники.Організації_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            СтаттяРухуКоштів = new Довідники.СтаттяРухуКоштів_Pointer();
            КасаОтримувач = new Довідники.Каси_Pointer();
            Курс = 0;
            Основа = new UuidAndText();
            Коментар = "";
            
            //Табличні частини
            РозшифруванняПлатежу_TablePart = new РозхіднийКасовийОрдер_РозшифруванняПлатежу_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_k2"]);
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_k3"]);
                СумаДокументу = (base.FieldValue["col_a3"] != DBNull.Value) ? (decimal)base.FieldValue["col_a3"] : 0;
                СумаДокументуПоКурсу = (base.FieldValue["col_a5"] != DBNull.Value) ? (decimal)base.FieldValue["col_a5"] : 0;
                ГосподарськаОперація = (base.FieldValue["col_k5"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_k5"] : 0;
                ОрганізаціяОтримувач = new Довідники.Організації_Pointer(base.FieldValue["col_k4"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_k7"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_a4"]);
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_k8"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_k9"]);
                СтаттяРухуКоштів = new Довідники.СтаттяРухуКоштів_Pointer(base.FieldValue["col_l2"]);
                КасаОтримувач = new Довідники.Каси_Pointer(base.FieldValue["col_k6"]);
                Курс = (base.FieldValue["col_a2"] != DBNull.Value) ? (decimal)base.FieldValue["col_a2"] : 0;
                Основа = (base.FieldValue["col_a1"] != DBNull.Value) ? (UuidAndText)base.FieldValue["col_a1"] : new UuidAndText();
                Коментар = base.FieldValue["col_l1"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            РозхіднийКасовийОрдер_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_k2"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_k3"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_a3"] = СумаДокументу;
            base.FieldValue["col_a5"] = СумаДокументуПоКурсу;
            base.FieldValue["col_k5"] = (int)ГосподарськаОперація;
            base.FieldValue["col_k4"] = ОрганізаціяОтримувач.UnigueID.UGuid;
            base.FieldValue["col_k7"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_a4"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_k8"] = БанківськийРахунок.UnigueID.UGuid;
            base.FieldValue["col_k9"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_l2"] = СтаттяРухуКоштів.UnigueID.UGuid;
            base.FieldValue["col_k6"] = КасаОтримувач.UnigueID.UGuid;
            base.FieldValue["col_a2"] = Курс;
            base.FieldValue["col_a1"] = Основа;
            base.FieldValue["col_l1"] = Коментар;
            
            BaseSave();
			РозхіднийКасовийОрдер_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = РозхіднийКасовийОрдер_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            РозхіднийКасовийОрдер_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public РозхіднийКасовийОрдер_Objest Copy()
        {
            РозхіднийКасовийОрдер_Objest copy = new РозхіднийКасовийОрдер_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Каса = Каса;
			copy.СумаДокументу = СумаДокументу;
			copy.СумаДокументуПоКурсу = СумаДокументуПоКурсу;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.ОрганізаціяОтримувач = ОрганізаціяОтримувач;
			copy.Контрагент = Контрагент;
			copy.Договір = Договір;
			copy.БанківськийРахунок = БанківськийРахунок;
			copy.Валюта = Валюта;
			copy.СтаттяРухуКоштів = СтаттяРухуКоштів;
			copy.КасаОтримувач = КасаОтримувач;
			copy.Курс = Курс;
			copy.Основа = Основа;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    РозхіднийКасовийОрдер_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a49" });
        }
        
        public РозхіднийКасовийОрдер_Pointer GetDocumentPointer()
        {
            РозхіднийКасовийОрдер_Pointer directoryPointer = new РозхіднийКасовийОрдер_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public decimal СумаДокументу { get; set; }
        public decimal СумаДокументуПоКурсу { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Довідники.Організації_Pointer ОрганізаціяОтримувач { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Довідники.СтаттяРухуКоштів_Pointer СтаттяРухуКоштів { get; set; }
        public Довідники.Каси_Pointer КасаОтримувач { get; set; }
        public decimal Курс { get; set; }
        public UuidAndText Основа { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public РозхіднийКасовийОрдер_РозшифруванняПлатежу_TablePart РозшифруванняПлатежу_TablePart { get; set; }
        
    }
    
    
    public class РозхіднийКасовийОрдер_Pointer : DocumentPointer
    {
        public РозхіднийКасовийОрдер_Pointer(object uid = null) : base(Config.Kernel, "tab_a48", "РозхіднийКасовийОрдер")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public РозхіднийКасовийОрдер_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a48", "РозхіднийКасовийОрдер")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public РозхіднийКасовийОрдер_Pointer GetNewDocumentPointer()
        {
            return new РозхіднийКасовийОрдер_Pointer(base.UnigueID);
        }
		
        public РозхіднийКасовийОрдер_Pointer GetEmptyPointer()
        {
            return new РозхіднийКасовийОрдер_Pointer();
        }
		
        public РозхіднийКасовийОрдер_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            РозхіднийКасовийОрдер_Objest РозхіднийКасовийОрдерObjestItem = new РозхіднийКасовийОрдер_Objest();
            РозхіднийКасовийОрдерObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				РозхіднийКасовийОрдерObjestItem.РозшифруванняПлатежу_TablePart.Read();
			}
			
            return РозхіднийКасовийОрдерObjestItem;
        }
    }
    
    
    public class РозхіднийКасовийОрдер_Select : DocumentSelect
    {		
        public РозхіднийКасовийОрдер_Select() : base(Config.Kernel, "tab_a48") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new РозхіднийКасовийОрдер_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public РозхіднийКасовийОрдер_Pointer Current { get; private set; }
    }
    
      
    public class РозхіднийКасовийОрдер_РозшифруванняПлатежу_TablePart : DocumentTablePart
    {
        public РозхіднийКасовийОрдер_РозшифруванняПлатежу_TablePart(РозхіднийКасовийОрдер_Objest owner) : base(Config.Kernel, "tab_a49",
             new string[] { "col_a1", "col_l4", "col_l5", "col_l6", "col_l7", "col_l8", "col_l9" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string Замовлення = "col_l4";
        public const string Сума = "col_l5";
        public const string ВалютаВзаєморозрахунків = "col_l6";
        public const string Підрозділ = "col_l7";
        public const string Коментар = "col_l8";
        public const string Організація = "col_l9";

        public РозхіднийКасовийОрдер_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.Замовлення = fieldValue["col_l4"].ToString();
                record.Сума = (fieldValue["col_l5"] != DBNull.Value) ? (decimal)fieldValue["col_l5"] : 0;
                record.ВалютаВзаєморозрахунків = new Довідники.Валюти_Pointer(fieldValue["col_l6"]);
                record.Підрозділ = new Довідники.СтруктураПідприємства_Pointer(fieldValue["col_l7"]);
                record.Коментар = fieldValue["col_l8"].ToString();
                record.Організація = new Довідники.Організації_Pointer(fieldValue["col_l9"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_l4", record.Замовлення);
                fieldValue.Add("col_l5", record.Сума);
                fieldValue.Add("col_l6", record.ВалютаВзаєморозрахунків.UnigueID.UGuid);
                fieldValue.Add("col_l7", record.Підрозділ.UnigueID.UGuid);
                fieldValue.Add("col_l8", record.Коментар);
                fieldValue.Add("col_l9", record.Організація.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Замовлення = "";
                Сума = 0;
                ВалютаВзаєморозрахунків = new Довідники.Валюти_Pointer();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
                Коментар = "";
                Організація = new Довідники.Організації_Pointer();
                
            }
            public int НомерРядка { get; set; }
            public string Замовлення { get; set; }
            public decimal Сума { get; set; }
            public Довідники.Валюти_Pointer ВалютаВзаєморозрахунків { get; set; }
            public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
            public string Коментар { get; set; }
            public Довідники.Організації_Pointer Організація { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ПереміщенняТоварів"
    
    public static class ПереміщенняТоварів_Const
    {
        public const string TABLE = "tab_a31";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_a3";
        public const string БанківськийРахунокОрганізації = "col_a4";
        public const string ВидЦіни = "col_a5";
        public const string ОрганізаціяОтримувач = "col_a7";
        public const string Відповідальний = "col_a8";
        public const string Підрозділ = "col_a9";
        public const string СкладВідправник = "col_b1";
        public const string СкладОтримувач = "col_b2";
        public const string Статус = "col_b3";
        public const string ГосподарськаОперація = "col_b4";
        public const string СпосібДоставки = "col_b5";
        public const string АдресДоставки = "col_b6";
        public const string ЧасДоставкиЗ = "col_b7";
        public const string ЧасДоставкиДо = "col_b8";
        public const string Коментар = "col_a6";
    }
	
    
    public class ПереміщенняТоварів_Objest : DocumentObject
    {
        public ПереміщенняТоварів_Objest() : base(Config.Kernel, "tab_a31", "ПереміщенняТоварів",
             new string[] { "docname", "docnomer", "docdate", "col_a3", "col_a4", "col_a5", "col_a7", "col_a8", "col_a9", "col_b1", "col_b2", "col_b3", "col_b4", "col_b5", "col_b6", "col_b7", "col_b8", "col_a6" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            БанківськийРахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            ВидЦіни = new Довідники.ВидиЦін_Pointer();
            ОрганізаціяОтримувач = new Довідники.Організації_Pointer();
            Відповідальний = new Довідники.Користувачі_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            СкладВідправник = new Довідники.Склади_Pointer();
            СкладОтримувач = new Довідники.Склади_Pointer();
            Статус = 0;
            ГосподарськаОперація = 0;
            СпосібДоставки = 0;
            АдресДоставки = "";
            ЧасДоставкиЗ = DateTime.MinValue.TimeOfDay;
            ЧасДоставкиДо = DateTime.MinValue.TimeOfDay;
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ПереміщенняТоварів_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_a3"]);
                БанківськийРахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_a4"]);
                ВидЦіни = new Довідники.ВидиЦін_Pointer(base.FieldValue["col_a5"]);
                ОрганізаціяОтримувач = new Довідники.Організації_Pointer(base.FieldValue["col_a7"]);
                Відповідальний = new Довідники.Користувачі_Pointer(base.FieldValue["col_a8"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a9"]);
                СкладВідправник = new Довідники.Склади_Pointer(base.FieldValue["col_b1"]);
                СкладОтримувач = new Довідники.Склади_Pointer(base.FieldValue["col_b2"]);
                Статус = (base.FieldValue["col_b3"] != DBNull.Value) ? (Перелічення.СтатусиПереміщенняТоварів)base.FieldValue["col_b3"] : 0;
                ГосподарськаОперація = (base.FieldValue["col_b4"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_b4"] : 0;
                СпосібДоставки = (base.FieldValue["col_b5"] != DBNull.Value) ? (Перелічення.СпособиДоставки)base.FieldValue["col_b5"] : 0;
                АдресДоставки = base.FieldValue["col_b6"].ToString();
                ЧасДоставкиЗ = (base.FieldValue["col_b7"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_b7"].ToString()) : DateTime.MinValue.TimeOfDay;
                ЧасДоставкиДо = (base.FieldValue["col_b8"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_b8"].ToString()) : DateTime.MinValue.TimeOfDay;
                Коментар = base.FieldValue["col_a6"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ПереміщенняТоварів_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_a3"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_a4"] = БанківськийРахунокОрганізації.UnigueID.UGuid;
            base.FieldValue["col_a5"] = ВидЦіни.UnigueID.UGuid;
            base.FieldValue["col_a7"] = ОрганізаціяОтримувач.UnigueID.UGuid;
            base.FieldValue["col_a8"] = Відповідальний.UnigueID.UGuid;
            base.FieldValue["col_a9"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_b1"] = СкладВідправник.UnigueID.UGuid;
            base.FieldValue["col_b2"] = СкладОтримувач.UnigueID.UGuid;
            base.FieldValue["col_b3"] = (int)Статус;
            base.FieldValue["col_b4"] = (int)ГосподарськаОперація;
            base.FieldValue["col_b5"] = (int)СпосібДоставки;
            base.FieldValue["col_b6"] = АдресДоставки;
            base.FieldValue["col_b7"] = ЧасДоставкиЗ;
            base.FieldValue["col_b8"] = ЧасДоставкиДо;
            base.FieldValue["col_a6"] = Коментар;
            
            BaseSave();
			ПереміщенняТоварів_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ПереміщенняТоварів_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ПереміщенняТоварів_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ПереміщенняТоварів_Objest Copy()
        {
            ПереміщенняТоварів_Objest copy = new ПереміщенняТоварів_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.БанківськийРахунокОрганізації = БанківськийРахунокОрганізації;
			copy.ВидЦіни = ВидЦіни;
			copy.ОрганізаціяОтримувач = ОрганізаціяОтримувач;
			copy.Відповідальний = Відповідальний;
			copy.Підрозділ = Підрозділ;
			copy.СкладВідправник = СкладВідправник;
			copy.СкладОтримувач = СкладОтримувач;
			copy.Статус = Статус;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.СпосібДоставки = СпосібДоставки;
			copy.АдресДоставки = АдресДоставки;
			copy.ЧасДоставкиЗ = ЧасДоставкиЗ;
			copy.ЧасДоставкиДо = ЧасДоставкиДо;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ПереміщенняТоварів_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a50" });
        }
        
        public ПереміщенняТоварів_Pointer GetDocumentPointer()
        {
            ПереміщенняТоварів_Pointer directoryPointer = new ПереміщенняТоварів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунокОрганізації { get; set; }
        public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
        public Довідники.Організації_Pointer ОрганізаціяОтримувач { get; set; }
        public Довідники.Користувачі_Pointer Відповідальний { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Pointer СкладВідправник { get; set; }
        public Довідники.Склади_Pointer СкладОтримувач { get; set; }
        public Перелічення.СтатусиПереміщенняТоварів Статус { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Перелічення.СпособиДоставки СпосібДоставки { get; set; }
        public string АдресДоставки { get; set; }
        public TimeSpan ЧасДоставкиЗ { get; set; }
        public TimeSpan ЧасДоставкиДо { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ПереміщенняТоварів_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ПереміщенняТоварів_Pointer : DocumentPointer
    {
        public ПереміщенняТоварів_Pointer(object uid = null) : base(Config.Kernel, "tab_a31", "ПереміщенняТоварів")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПереміщенняТоварів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a31", "ПереміщенняТоварів")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ПереміщенняТоварів_Pointer GetNewDocumentPointer()
        {
            return new ПереміщенняТоварів_Pointer(base.UnigueID);
        }
		
        public ПереміщенняТоварів_Pointer GetEmptyPointer()
        {
            return new ПереміщенняТоварів_Pointer();
        }
		
        public ПереміщенняТоварів_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ПереміщенняТоварів_Objest ПереміщенняТоварівObjestItem = new ПереміщенняТоварів_Objest();
            ПереміщенняТоварівObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ПереміщенняТоварівObjestItem.Товари_TablePart.Read();
			}
			
            return ПереміщенняТоварівObjestItem;
        }
    }
    
    
    public class ПереміщенняТоварів_Select : DocumentSelect
    {		
        public ПереміщенняТоварів_Select() : base(Config.Kernel, "tab_a31") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПереміщенняТоварів_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ПереміщенняТоварів_Pointer Current { get; private set; }
    }
    
      
    public class ПереміщенняТоварів_Товари_TablePart : DocumentTablePart
    {
        public ПереміщенняТоварів_Товари_TablePart(ПереміщенняТоварів_Objest owner) : base(Config.Kernel, "tab_a50",
             new string[] { "col_b8", "col_b3", "col_b4", "col_a1", "col_b5", "col_b6", "col_b7" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_b8";
        public const string Номенклатура = "col_b3";
        public const string ХарактеристикаНоменклатури = "col_b4";
        public const string Серія = "col_a1";
        public const string Пакування = "col_b5";
        public const string КількістьУпаковок = "col_b6";
        public const string Кількість = "col_b7";

        public ПереміщенняТоварів_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_b8"] != DBNull.Value) ? (int)fieldValue["col_b8"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_b3"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_b4"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_b5"]);
                record.КількістьУпаковок = (fieldValue["col_b6"] != DBNull.Value) ? (int)fieldValue["col_b6"] : 0;
                record.Кількість = (fieldValue["col_b7"] != DBNull.Value) ? (decimal)fieldValue["col_b7"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_b8", record.НомерРядка);
                fieldValue.Add("col_b3", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_b4", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a1", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_b5", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_b6", record.КількістьУпаковок);
                fieldValue.Add("col_b7", record.Кількість);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ПоверненняТоварівПостачальнику"
    
    public static class ПоверненняТоварівПостачальнику_Const
    {
        public const string TABLE = "tab_a51";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_c2";
        public const string Контрагент = "col_c3";
        public const string Підрозділ = "col_c4";
        public const string Валюта = "col_c5";
        public const string Склад = "col_c6";
        public const string СумаДокументу = "col_c7";
        public const string ГосподарськаОперація = "col_c9";
        public const string БанківськийРахунокОрганізації = "col_d1";
        public const string БанківськийРахунокКонтрагента = "col_d2";
        public const string Договір = "col_d3";
        public const string СпосібДоставки = "col_d4";
        public const string АдресДоставки = "col_d5";
        public const string ЧасДоставкиЗ = "col_d6";
        public const string ЧасДоставкиДо = "col_d7";
        public const string Каса = "col_a1";
        public const string Основа = "col_a2";
        public const string Коментар = "col_c8";
    }
	
    
    public class ПоверненняТоварівПостачальнику_Objest : DocumentObject
    {
        public ПоверненняТоварівПостачальнику_Objest() : base(Config.Kernel, "tab_a51", "ПоверненняТоварівПостачальнику",
             new string[] { "docname", "docnomer", "docdate", "col_c2", "col_c3", "col_c4", "col_c5", "col_c6", "col_c7", "col_c9", "col_d1", "col_d2", "col_d3", "col_d4", "col_d5", "col_d6", "col_d7", "col_a1", "col_a2", "col_c8" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            Склад = new Довідники.Склади_Pointer();
            СумаДокументу = 0;
            ГосподарськаОперація = 0;
            БанківськийРахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            СпосібДоставки = 0;
            АдресДоставки = "";
            ЧасДоставкиЗ = DateTime.MinValue.TimeOfDay;
            ЧасДоставкиДо = DateTime.MinValue.TimeOfDay;
            Каса = new Довідники.Каси_Pointer();
            Основа = new UuidAndText();
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ПоверненняТоварівПостачальнику_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_c2"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_c3"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_c4"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_c5"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_c6"]);
                СумаДокументу = (base.FieldValue["col_c7"] != DBNull.Value) ? (decimal)base.FieldValue["col_c7"] : 0;
                ГосподарськаОперація = (base.FieldValue["col_c9"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_c9"] : 0;
                БанківськийРахунокОрганізації = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_d1"]);
                БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer(base.FieldValue["col_d2"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_d3"]);
                СпосібДоставки = (base.FieldValue["col_d4"] != DBNull.Value) ? (Перелічення.СпособиДоставки)base.FieldValue["col_d4"] : 0;
                АдресДоставки = base.FieldValue["col_d5"].ToString();
                ЧасДоставкиЗ = (base.FieldValue["col_d6"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_d6"].ToString()) : DateTime.MinValue.TimeOfDay;
                ЧасДоставкиДо = (base.FieldValue["col_d7"] != DBNull.Value) ? TimeSpan.Parse(base.FieldValue["col_d7"].ToString()) : DateTime.MinValue.TimeOfDay;
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_a1"]);
                Основа = (base.FieldValue["col_a2"] != DBNull.Value) ? (UuidAndText)base.FieldValue["col_a2"] : new UuidAndText();
                Коментар = base.FieldValue["col_c8"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ПоверненняТоварівПостачальнику_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_c2"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_c3"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_c4"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_c5"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_c6"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_c7"] = СумаДокументу;
            base.FieldValue["col_c9"] = (int)ГосподарськаОперація;
            base.FieldValue["col_d1"] = БанківськийРахунокОрганізації.UnigueID.UGuid;
            base.FieldValue["col_d2"] = БанківськийРахунокКонтрагента.UnigueID.UGuid;
            base.FieldValue["col_d3"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_d4"] = (int)СпосібДоставки;
            base.FieldValue["col_d5"] = АдресДоставки;
            base.FieldValue["col_d6"] = ЧасДоставкиЗ;
            base.FieldValue["col_d7"] = ЧасДоставкиДо;
            base.FieldValue["col_a1"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_a2"] = Основа;
            base.FieldValue["col_c8"] = Коментар;
            
            BaseSave();
			ПоверненняТоварівПостачальнику_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ПоверненняТоварівПостачальнику_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ПоверненняТоварівПостачальнику_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ПоверненняТоварівПостачальнику_Objest Copy()
        {
            ПоверненняТоварівПостачальнику_Objest copy = new ПоверненняТоварівПостачальнику_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Контрагент = Контрагент;
			copy.Підрозділ = Підрозділ;
			copy.Валюта = Валюта;
			copy.Склад = Склад;
			copy.СумаДокументу = СумаДокументу;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.БанківськийРахунокОрганізації = БанківськийРахунокОрганізації;
			copy.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента;
			copy.Договір = Договір;
			copy.СпосібДоставки = СпосібДоставки;
			copy.АдресДоставки = АдресДоставки;
			copy.ЧасДоставкиЗ = ЧасДоставкиЗ;
			copy.ЧасДоставкиДо = ЧасДоставкиДо;
			copy.Каса = Каса;
			copy.Основа = Основа;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ПоверненняТоварівПостачальнику_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a52" });
        }
        
        public ПоверненняТоварівПостачальнику_Pointer GetDocumentPointer()
        {
            ПоверненняТоварівПостачальнику_Pointer directoryPointer = new ПоверненняТоварівПостачальнику_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public decimal СумаДокументу { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунокОрганізації { get; set; }
        public Довідники.БанківськіРахункиКонтрагентів_Pointer БанківськийРахунокКонтрагента { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Перелічення.СпособиДоставки СпосібДоставки { get; set; }
        public string АдресДоставки { get; set; }
        public TimeSpan ЧасДоставкиЗ { get; set; }
        public TimeSpan ЧасДоставкиДо { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public UuidAndText Основа { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ПоверненняТоварівПостачальнику_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ПоверненняТоварівПостачальнику_Pointer : DocumentPointer
    {
        public ПоверненняТоварівПостачальнику_Pointer(object uid = null) : base(Config.Kernel, "tab_a51", "ПоверненняТоварівПостачальнику")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПоверненняТоварівПостачальнику_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a51", "ПоверненняТоварівПостачальнику")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ПоверненняТоварівПостачальнику_Pointer GetNewDocumentPointer()
        {
            return new ПоверненняТоварівПостачальнику_Pointer(base.UnigueID);
        }
		
        public ПоверненняТоварівПостачальнику_Pointer GetEmptyPointer()
        {
            return new ПоверненняТоварівПостачальнику_Pointer();
        }
		
        public ПоверненняТоварівПостачальнику_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ПоверненняТоварівПостачальнику_Objest ПоверненняТоварівПостачальникуObjestItem = new ПоверненняТоварівПостачальнику_Objest();
            ПоверненняТоварівПостачальникуObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ПоверненняТоварівПостачальникуObjestItem.Товари_TablePart.Read();
			}
			
            return ПоверненняТоварівПостачальникуObjestItem;
        }
    }
    
    
    public class ПоверненняТоварівПостачальнику_Select : DocumentSelect
    {		
        public ПоверненняТоварівПостачальнику_Select() : base(Config.Kernel, "tab_a51") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПоверненняТоварівПостачальнику_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ПоверненняТоварівПостачальнику_Pointer Current { get; private set; }
    }
    
      
    public class ПоверненняТоварівПостачальнику_Товари_TablePart : DocumentTablePart
    {
        public ПоверненняТоварівПостачальнику_Товари_TablePart(ПоверненняТоварівПостачальнику_Objest owner) : base(Config.Kernel, "tab_a52",
             new string[] { "col_a1", "col_d8", "col_d9", "col_a3", "col_e1", "col_e2", "col_e3", "col_e4", "col_e5", "col_a2" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string Номенклатура = "col_d8";
        public const string ХарактеристикаНоменклатури = "col_d9";
        public const string Серія = "col_a3";
        public const string Пакування = "col_e1";
        public const string КількістьУпаковок = "col_e2";
        public const string Кількість = "col_e3";
        public const string Ціна = "col_e4";
        public const string Сума = "col_e5";
        public const string ДокументПоступлення = "col_a2";

        public ПоверненняТоварівПостачальнику_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_d8"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_d9"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a3"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_e1"]);
                record.КількістьУпаковок = (fieldValue["col_e2"] != DBNull.Value) ? (int)fieldValue["col_e2"] : 0;
                record.Кількість = (fieldValue["col_e3"] != DBNull.Value) ? (decimal)fieldValue["col_e3"] : 0;
                record.Ціна = (fieldValue["col_e4"] != DBNull.Value) ? (decimal)fieldValue["col_e4"] : 0;
                record.Сума = (fieldValue["col_e5"] != DBNull.Value) ? (decimal)fieldValue["col_e5"] : 0;
                record.ДокументПоступлення = new Документи.ПоступленняТоварівТаПослуг_Pointer(fieldValue["col_a2"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_d8", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_d9", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a3", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_e1", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_e2", record.КількістьУпаковок);
                fieldValue.Add("col_e3", record.Кількість);
                fieldValue.Add("col_e4", record.Ціна);
                fieldValue.Add("col_e5", record.Сума);
                fieldValue.Add("col_a2", record.ДокументПоступлення.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                Ціна = 0;
                Сума = 0;
                ДокументПоступлення = new Документи.ПоступленняТоварівТаПослуг_Pointer();
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public Документи.ПоступленняТоварівТаПослуг_Pointer ДокументПоступлення { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ПоверненняТоварівВідКлієнта"
    
    public static class ПоверненняТоварівВідКлієнта_Const
    {
        public const string TABLE = "tab_a53";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_e8";
        public const string Валюта = "col_e9";
        public const string СумаДокументу = "col_f1";
        public const string Склад = "col_f2";
        public const string Підрозділ = "col_f3";
        public const string Менеджер = "col_f5";
        public const string ДокументПродажу = "col_f7";
        public const string ГосподарськаОперація = "col_f8";
        public const string Договір = "col_f9";
        public const string ПричинаПовернення = "col_g1";
        public const string Контрагент = "col_g2";
        public const string Каса = "col_a1";
        public const string Основа = "col_a2";
        public const string Коментар = "col_f6";
    }
	
    
    public class ПоверненняТоварівВідКлієнта_Objest : DocumentObject
    {
        public ПоверненняТоварівВідКлієнта_Objest() : base(Config.Kernel, "tab_a53", "ПоверненняТоварівВідКлієнта",
             new string[] { "docname", "docnomer", "docdate", "col_e8", "col_e9", "col_f1", "col_f2", "col_f3", "col_f5", "col_f7", "col_f8", "col_f9", "col_g1", "col_g2", "col_a1", "col_a2", "col_f6" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            СумаДокументу = 0;
            Склад = new Довідники.Склади_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Менеджер = new Довідники.Користувачі_Pointer();
            ДокументПродажу = new Документи.РеалізаціяТоварівТаПослуг_Pointer();
            ГосподарськаОперація = 0;
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            ПричинаПовернення = "";
            Контрагент = new Довідники.Контрагенти_Pointer();
            Каса = new Довідники.Каси_Pointer();
            Основа = new UuidAndText();
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ПоверненняТоварівВідКлієнта_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_e8"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_e9"]);
                СумаДокументу = (base.FieldValue["col_f1"] != DBNull.Value) ? (decimal)base.FieldValue["col_f1"] : 0;
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_f2"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_f3"]);
                Менеджер = new Довідники.Користувачі_Pointer(base.FieldValue["col_f5"]);
                ДокументПродажу = new Документи.РеалізаціяТоварівТаПослуг_Pointer(base.FieldValue["col_f7"]);
                ГосподарськаОперація = (base.FieldValue["col_f8"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_f8"] : 0;
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_f9"]);
                ПричинаПовернення = base.FieldValue["col_g1"].ToString();
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_g2"]);
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_a1"]);
                Основа = (base.FieldValue["col_a2"] != DBNull.Value) ? (UuidAndText)base.FieldValue["col_a2"] : new UuidAndText();
                Коментар = base.FieldValue["col_f6"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ПоверненняТоварівВідКлієнта_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_e8"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_e9"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_f1"] = СумаДокументу;
            base.FieldValue["col_f2"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_f3"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_f5"] = Менеджер.UnigueID.UGuid;
            base.FieldValue["col_f7"] = ДокументПродажу.UnigueID.UGuid;
            base.FieldValue["col_f8"] = (int)ГосподарськаОперація;
            base.FieldValue["col_f9"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_g1"] = ПричинаПовернення;
            base.FieldValue["col_g2"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_a1"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_a2"] = Основа;
            base.FieldValue["col_f6"] = Коментар;
            
            BaseSave();
			ПоверненняТоварівВідКлієнта_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ПоверненняТоварівВідКлієнта_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ПоверненняТоварівВідКлієнта_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ПоверненняТоварівВідКлієнта_Objest Copy()
        {
            ПоверненняТоварівВідКлієнта_Objest copy = new ПоверненняТоварівВідКлієнта_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Валюта = Валюта;
			copy.СумаДокументу = СумаДокументу;
			copy.Склад = Склад;
			copy.Підрозділ = Підрозділ;
			copy.Менеджер = Менеджер;
			copy.ДокументПродажу = ДокументПродажу;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Договір = Договір;
			copy.ПричинаПовернення = ПричинаПовернення;
			copy.Контрагент = Контрагент;
			copy.Каса = Каса;
			copy.Основа = Основа;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ПоверненняТоварівВідКлієнта_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a54" });
        }
        
        public ПоверненняТоварівВідКлієнта_Pointer GetDocumentPointer()
        {
            ПоверненняТоварівВідКлієнта_Pointer directoryPointer = new ПоверненняТоварівВідКлієнта_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public decimal СумаДокументу { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Користувачі_Pointer Менеджер { get; set; }
        public Документи.РеалізаціяТоварівТаПослуг_Pointer ДокументПродажу { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public string ПричинаПовернення { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public UuidAndText Основа { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ПоверненняТоварівВідКлієнта_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ПоверненняТоварівВідКлієнта_Pointer : DocumentPointer
    {
        public ПоверненняТоварівВідКлієнта_Pointer(object uid = null) : base(Config.Kernel, "tab_a53", "ПоверненняТоварівВідКлієнта")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПоверненняТоварівВідКлієнта_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a53", "ПоверненняТоварівВідКлієнта")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ПоверненняТоварівВідКлієнта_Pointer GetNewDocumentPointer()
        {
            return new ПоверненняТоварівВідКлієнта_Pointer(base.UnigueID);
        }
		
        public ПоверненняТоварівВідКлієнта_Pointer GetEmptyPointer()
        {
            return new ПоверненняТоварівВідКлієнта_Pointer();
        }
		
        public ПоверненняТоварівВідКлієнта_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнтаObjestItem = new ПоверненняТоварівВідКлієнта_Objest();
            ПоверненняТоварівВідКлієнтаObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ПоверненняТоварівВідКлієнтаObjestItem.Товари_TablePart.Read();
			}
			
            return ПоверненняТоварівВідКлієнтаObjestItem;
        }
    }
    
    
    public class ПоверненняТоварівВідКлієнта_Select : DocumentSelect
    {		
        public ПоверненняТоварівВідКлієнта_Select() : base(Config.Kernel, "tab_a53") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПоверненняТоварівВідКлієнта_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ПоверненняТоварівВідКлієнта_Pointer Current { get; private set; }
    }
    
      
    public class ПоверненняТоварівВідКлієнта_Товари_TablePart : DocumentTablePart
    {
        public ПоверненняТоварівВідКлієнта_Товари_TablePart(ПоверненняТоварівВідКлієнта_Objest owner) : base(Config.Kernel, "tab_a54",
             new string[] { "col_h2", "col_g3", "col_g4", "col_a1", "col_g5", "col_g6", "col_g7", "col_g8", "col_g9", "col_h1", "col_a2" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_h2";
        public const string Номенклатура = "col_g3";
        public const string ХарактеристикаНоменклатури = "col_g4";
        public const string Серія = "col_a1";
        public const string Пакування = "col_g5";
        public const string КількістьУпаковок = "col_g6";
        public const string Кількість = "col_g7";
        public const string Ціна = "col_g8";
        public const string Сума = "col_g9";
        public const string Штрихкод = "col_h1";
        public const string ДокументРеалізації = "col_a2";

        public ПоверненняТоварівВідКлієнта_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_h2"] != DBNull.Value) ? (int)fieldValue["col_h2"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_g3"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_g4"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_g5"]);
                record.КількістьУпаковок = (fieldValue["col_g6"] != DBNull.Value) ? (int)fieldValue["col_g6"] : 0;
                record.Кількість = (fieldValue["col_g7"] != DBNull.Value) ? (decimal)fieldValue["col_g7"] : 0;
                record.Ціна = (fieldValue["col_g8"] != DBNull.Value) ? (decimal)fieldValue["col_g8"] : 0;
                record.Сума = (fieldValue["col_g9"] != DBNull.Value) ? (decimal)fieldValue["col_g9"] : 0;
                record.Штрихкод = fieldValue["col_h1"].ToString();
                record.ДокументРеалізації = new Документи.РеалізаціяТоварівТаПослуг_Pointer(fieldValue["col_a2"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_h2", record.НомерРядка);
                fieldValue.Add("col_g3", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_g4", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a1", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_g5", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_g6", record.КількістьУпаковок);
                fieldValue.Add("col_g7", record.Кількість);
                fieldValue.Add("col_g8", record.Ціна);
                fieldValue.Add("col_g9", record.Сума);
                fieldValue.Add("col_h1", record.Штрихкод);
                fieldValue.Add("col_a2", record.ДокументРеалізації.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                Ціна = 0;
                Сума = 0;
                Штрихкод = "";
                ДокументРеалізації = new Документи.РеалізаціяТоварівТаПослуг_Pointer();
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public string Штрихкод { get; set; }
            public Документи.РеалізаціяТоварівТаПослуг_Pointer ДокументРеалізації { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "АктВиконанихРобіт"
    
    public static class АктВиконанихРобіт_Const
    {
        public const string TABLE = "tab_a81";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Валюта = "col_b2";
        public const string ЗамовленняКлієнта = "col_b3";
        public const string Каса = "col_b4";
        public const string Контрагент = "col_b5";
        public const string Організація = "col_b6";
        public const string Підрозділ = "col_a5";
        public const string СумаДокументу = "col_a2";
        public const string ФормаОплати = "col_a3";
        public const string Договір = "col_a4";
        public const string ГосподарськаОперація = "col_a6";
        public const string Коментар = "col_a1";
    }
	
    
    public class АктВиконанихРобіт_Objest : DocumentObject
    {
        public АктВиконанихРобіт_Objest() : base(Config.Kernel, "tab_a81", "АктВиконанихРобіт",
             new string[] { "docname", "docnomer", "docdate", "col_b2", "col_b3", "col_b4", "col_b5", "col_b6", "col_a5", "col_a2", "col_a3", "col_a4", "col_a6", "col_a1" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Валюта = new Довідники.Валюти_Pointer();
            ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer();
            Каса = new Довідники.Каси_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            Організація = new Довідники.Організації_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            СумаДокументу = 0;
            ФормаОплати = 0;
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            ГосподарськаОперація = 0;
            Коментар = "";
            
            //Табличні частини
            Послуги_TablePart = new АктВиконанихРобіт_Послуги_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_b2"]);
                ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer(base.FieldValue["col_b3"]);
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_b4"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_b5"]);
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_b6"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a5"]);
                СумаДокументу = (base.FieldValue["col_a2"] != DBNull.Value) ? (decimal)base.FieldValue["col_a2"] : 0;
                ФормаОплати = (base.FieldValue["col_a3"] != DBNull.Value) ? (Перелічення.ФормаОплати)base.FieldValue["col_a3"] : 0;
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_a4"]);
                ГосподарськаОперація = (base.FieldValue["col_a6"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_a6"] : 0;
                Коментар = base.FieldValue["col_a1"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            АктВиконанихРобіт_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_b2"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_b3"] = ЗамовленняКлієнта.UnigueID.UGuid;
            base.FieldValue["col_b4"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_b5"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_b6"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_a5"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_a2"] = СумаДокументу;
            base.FieldValue["col_a3"] = (int)ФормаОплати;
            base.FieldValue["col_a4"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_a6"] = (int)ГосподарськаОперація;
            base.FieldValue["col_a1"] = Коментар;
            
            BaseSave();
			АктВиконанихРобіт_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = АктВиконанихРобіт_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            АктВиконанихРобіт_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public АктВиконанихРобіт_Objest Copy()
        {
            АктВиконанихРобіт_Objest copy = new АктВиконанихРобіт_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Валюта = Валюта;
			copy.ЗамовленняКлієнта = ЗамовленняКлієнта;
			copy.Каса = Каса;
			copy.Контрагент = Контрагент;
			copy.Організація = Організація;
			copy.Підрозділ = Підрозділ;
			copy.СумаДокументу = СумаДокументу;
			copy.ФормаОплати = ФормаОплати;
			copy.Договір = Договір;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    АктВиконанихРобіт_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a82" });
        }
        
        public АктВиконанихРобіт_Pointer GetDocumentPointer()
        {
            АктВиконанихРобіт_Pointer directoryPointer = new АктВиконанихРобіт_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Документи.ЗамовленняКлієнта_Pointer ЗамовленняКлієнта { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public decimal СумаДокументу { get; set; }
        public Перелічення.ФормаОплати ФормаОплати { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public АктВиконанихРобіт_Послуги_TablePart Послуги_TablePart { get; set; }
        
    }
    
    
    public class АктВиконанихРобіт_Pointer : DocumentPointer
    {
        public АктВиконанихРобіт_Pointer(object uid = null) : base(Config.Kernel, "tab_a81", "АктВиконанихРобіт")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public АктВиконанихРобіт_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a81", "АктВиконанихРобіт")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public АктВиконанихРобіт_Pointer GetNewDocumentPointer()
        {
            return new АктВиконанихРобіт_Pointer(base.UnigueID);
        }
		
        public АктВиконанихРобіт_Pointer GetEmptyPointer()
        {
            return new АктВиконанихРобіт_Pointer();
        }
		
        public АктВиконанихРобіт_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            АктВиконанихРобіт_Objest АктВиконанихРобітObjestItem = new АктВиконанихРобіт_Objest();
            АктВиконанихРобітObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				АктВиконанихРобітObjestItem.Послуги_TablePart.Read();
			}
			
            return АктВиконанихРобітObjestItem;
        }
    }
    
    
    public class АктВиконанихРобіт_Select : DocumentSelect
    {		
        public АктВиконанихРобіт_Select() : base(Config.Kernel, "tab_a81") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new АктВиконанихРобіт_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public АктВиконанихРобіт_Pointer Current { get; private set; }
    }
    
      
    public class АктВиконанихРобіт_Послуги_TablePart : DocumentTablePart
    {
        public АктВиконанихРобіт_Послуги_TablePart(АктВиконанихРобіт_Objest owner) : base(Config.Kernel, "tab_a82",
             new string[] { "col_c4", "col_b8", "col_b9", "col_c1", "col_c3", "col_c2" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_c4";
        public const string Номенклатура = "col_b8";
        public const string ХарактеристикаНоменклатури = "col_b9";
        public const string Кількість = "col_c1";
        public const string Ціна = "col_c3";
        public const string Сума = "col_c2";

        public АктВиконанихРобіт_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_c4"] != DBNull.Value) ? (int)fieldValue["col_c4"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_b8"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_b9"]);
                record.Кількість = (fieldValue["col_c1"] != DBNull.Value) ? (decimal)fieldValue["col_c1"] : 0;
                record.Ціна = (fieldValue["col_c3"] != DBNull.Value) ? (decimal)fieldValue["col_c3"] : 0;
                record.Сума = (fieldValue["col_c2"] != DBNull.Value) ? (decimal)fieldValue["col_c2"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_c4", record.НомерРядка);
                fieldValue.Add("col_b8", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_b9", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_c1", record.Кількість);
                fieldValue.Add("col_c3", record.Ціна);
                fieldValue.Add("col_c2", record.Сума);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Кількість = 0;
                Ціна = 0;
                Сума = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ВведенняЗалишків"
    
    public static class ВведенняЗалишків_Const
    {
        public const string TABLE = "tab_a83";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_c8";
        public const string Підрозділ = "col_d5";
        public const string Склад = "col_c9";
        public const string Валюта = "col_d2";
        public const string Контрагент = "col_d3";
        public const string Договір = "col_d4";
        public const string ГосподарськаОперація = "col_a1";
        public const string Коментар = "col_d1";
        public const string ПартіяТоварівКомпозит = "col_a2";
    }
	
    
    public class ВведенняЗалишків_Objest : DocumentObject
    {
        public ВведенняЗалишків_Objest() : base(Config.Kernel, "tab_a83", "ВведенняЗалишків",
             new string[] { "docname", "docnomer", "docdate", "col_c8", "col_d5", "col_c9", "col_d2", "col_d3", "col_d4", "col_a1", "col_d1", "col_a2" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Склад = new Довідники.Склади_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            Контрагент = new Довідники.Контрагенти_Pointer();
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            ГосподарськаОперація = 0;
            Коментар = "";
            ПартіяТоварівКомпозит = new Довідники.ПартіяТоварівКомпозит_Pointer();
            
            //Табличні частини
            Товари_TablePart = new ВведенняЗалишків_Товари_TablePart(this);
            Каси_TablePart = new ВведенняЗалишків_Каси_TablePart(this);
            БанківськіРахунки_TablePart = new ВведенняЗалишків_БанківськіРахунки_TablePart(this);
            РозрахункиЗКонтрагентами_TablePart = new ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_c8"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_d5"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_c9"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_d2"]);
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_d3"]);
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_d4"]);
                ГосподарськаОперація = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_a1"] : 0;
                Коментар = base.FieldValue["col_d1"].ToString();
                ПартіяТоварівКомпозит = new Довідники.ПартіяТоварівКомпозит_Pointer(base.FieldValue["col_a2"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ВведенняЗалишків_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_c8"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_d5"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_c9"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_d2"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_d3"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_d4"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_a1"] = (int)ГосподарськаОперація;
            base.FieldValue["col_d1"] = Коментар;
            base.FieldValue["col_a2"] = ПартіяТоварівКомпозит.UnigueID.UGuid;
            
            BaseSave();
			ВведенняЗалишків_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ВведенняЗалишків_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ВведенняЗалишків_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ВведенняЗалишків_Objest Copy()
        {
            ВведенняЗалишків_Objest copy = new ВведенняЗалишків_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Підрозділ = Підрозділ;
			copy.Склад = Склад;
			copy.Валюта = Валюта;
			copy.Контрагент = Контрагент;
			copy.Договір = Договір;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Коментар = Коментар;
			copy.ПартіяТоварівКомпозит = ПартіяТоварівКомпозит;
			
			return copy;
        }

        public void Delete()
        {
		    ВведенняЗалишків_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a84", "tab_a85", "tab_a86", "tab_a87" });
        }
        
        public ВведенняЗалишків_Pointer GetDocumentPointer()
        {
            ВведенняЗалишків_Pointer directoryPointer = new ВведенняЗалишків_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public string Коментар { get; set; }
        public Довідники.ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит { get; set; }
        
        //Табличні частини
        public ВведенняЗалишків_Товари_TablePart Товари_TablePart { get; set; }
        public ВведенняЗалишків_Каси_TablePart Каси_TablePart { get; set; }
        public ВведенняЗалишків_БанківськіРахунки_TablePart БанківськіРахунки_TablePart { get; set; }
        public ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart РозрахункиЗКонтрагентами_TablePart { get; set; }
        
    }
    
    
    public class ВведенняЗалишків_Pointer : DocumentPointer
    {
        public ВведенняЗалишків_Pointer(object uid = null) : base(Config.Kernel, "tab_a83", "ВведенняЗалишків")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВведенняЗалишків_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a83", "ВведенняЗалишків")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ВведенняЗалишків_Pointer GetNewDocumentPointer()
        {
            return new ВведенняЗалишків_Pointer(base.UnigueID);
        }
		
        public ВведенняЗалишків_Pointer GetEmptyPointer()
        {
            return new ВведенняЗалишків_Pointer();
        }
		
        public ВведенняЗалишків_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ВведенняЗалишків_Objest ВведенняЗалишківObjestItem = new ВведенняЗалишків_Objest();
            ВведенняЗалишківObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ВведенняЗалишківObjestItem.Товари_TablePart.Read();ВведенняЗалишківObjestItem.Каси_TablePart.Read();ВведенняЗалишківObjestItem.БанківськіРахунки_TablePart.Read();ВведенняЗалишківObjestItem.РозрахункиЗКонтрагентами_TablePart.Read();
			}
			
            return ВведенняЗалишківObjestItem;
        }
    }
    
    
    public class ВведенняЗалишків_Select : DocumentSelect
    {		
        public ВведенняЗалишків_Select() : base(Config.Kernel, "tab_a83") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВведенняЗалишків_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ВведенняЗалишків_Pointer Current { get; private set; }
    }
    
      
    public class ВведенняЗалишків_Товари_TablePart : DocumentTablePart
    {
        public ВведенняЗалишків_Товари_TablePart(ВведенняЗалишків_Objest owner) : base(Config.Kernel, "tab_a84",
             new string[] { "col_e4", "col_d6", "col_d7", "col_a1", "col_d8", "col_d9", "col_e1", "col_e2", "col_e3" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_e4";
        public const string Номенклатура = "col_d6";
        public const string ХарактеристикаНоменклатури = "col_d7";
        public const string Серія = "col_a1";
        public const string Пакування = "col_d8";
        public const string КількістьУпаковок = "col_d9";
        public const string Кількість = "col_e1";
        public const string Ціна = "col_e2";
        public const string Сума = "col_e3";

        public ВведенняЗалишків_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_e4"] != DBNull.Value) ? (int)fieldValue["col_e4"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_d6"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_d7"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_d8"]);
                record.КількістьУпаковок = (fieldValue["col_d9"] != DBNull.Value) ? (int)fieldValue["col_d9"] : 0;
                record.Кількість = (fieldValue["col_e1"] != DBNull.Value) ? (decimal)fieldValue["col_e1"] : 0;
                record.Ціна = (fieldValue["col_e2"] != DBNull.Value) ? (decimal)fieldValue["col_e2"] : 0;
                record.Сума = (fieldValue["col_e3"] != DBNull.Value) ? (decimal)fieldValue["col_e3"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_e4", record.НомерРядка);
                fieldValue.Add("col_d6", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_d7", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a1", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_d8", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_d9", record.КількістьУпаковок);
                fieldValue.Add("col_e1", record.Кількість);
                fieldValue.Add("col_e2", record.Ціна);
                fieldValue.Add("col_e3", record.Сума);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                Ціна = 0;
                Сума = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
      
    public class ВведенняЗалишків_Каси_TablePart : DocumentTablePart
    {
        public ВведенняЗалишків_Каси_TablePart(ВведенняЗалишків_Objest owner) : base(Config.Kernel, "tab_a85",
             new string[] { "col_a1", "col_e5", "col_e6" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string Каса = "col_e5";
        public const string Сума = "col_e6";

        public ВведенняЗалишків_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.Каса = new Довідники.Каси_Pointer(fieldValue["col_e5"]);
                record.Сума = (fieldValue["col_e6"] != DBNull.Value) ? (decimal)fieldValue["col_e6"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_e5", record.Каса.UnigueID.UGuid);
                fieldValue.Add("col_e6", record.Сума);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Каса = new Довідники.Каси_Pointer();
                Сума = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Каси_Pointer Каса { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
      
    public class ВведенняЗалишків_БанківськіРахунки_TablePart : DocumentTablePart
    {
        public ВведенняЗалишків_БанківськіРахунки_TablePart(ВведенняЗалишків_Objest owner) : base(Config.Kernel, "tab_a86",
             new string[] { "col_a1", "col_e7", "col_e8" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string БанківськийРахунок = "col_e7";
        public const string Сума = "col_e8";

        public ВведенняЗалишків_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(fieldValue["col_e7"]);
                record.Сума = (fieldValue["col_e8"] != DBNull.Value) ? (decimal)fieldValue["col_e8"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_e7", record.БанківськийРахунок.UnigueID.UGuid);
                fieldValue.Add("col_e8", record.Сума);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
                Сума = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
      
    public class ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart : DocumentTablePart
    {
        public ВведенняЗалишків_РозрахункиЗКонтрагентами_TablePart(ВведенняЗалишків_Objest owner) : base(Config.Kernel, "tab_a87",
             new string[] { "col_a1", "col_e9", "col_f1", "col_f2" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string Контрагент = "col_e9";
        public const string Валюта = "col_f1";
        public const string Сума = "col_f2";

        public ВведенняЗалишків_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_e9"]);
                record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_f1"]);
                record.Сума = (fieldValue["col_f2"] != DBNull.Value) ? (decimal)fieldValue["col_f2"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_e9", record.Контрагент.UnigueID.UGuid);
                fieldValue.Add("col_f1", record.Валюта.UnigueID.UGuid);
                fieldValue.Add("col_f2", record.Сума);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Контрагент = new Довідники.Контрагенти_Pointer();
                Валюта = new Довідники.Валюти_Pointer();
                Сума = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Контрагенти_Pointer Контрагент { get; set; }
            public Довідники.Валюти_Pointer Валюта { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "НадлишкиТоварів"
    
    public static class НадлишкиТоварів_Const
    {
        public const string TABLE = "tab_a88";
        
        public const string Організація = "col_f6";
        public const string Підрозділ = "col_f7";
        public const string Склад = "col_f8";
        public const string ВидЦіни = "col_f9";
        public const string Коментар = "col_g1";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Назва = "docname";
    }
	
    
    public class НадлишкиТоварів_Objest : DocumentObject
    {
        public НадлишкиТоварів_Objest() : base(Config.Kernel, "tab_a88", "НадлишкиТоварів",
             new string[] { "col_f6", "col_f7", "col_f8", "col_f9", "col_g1", "docnomer", "docdate", "docname" }) 
        {
            Організація = new Довідники.Організації_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Склад = new Довідники.Склади_Pointer();
            ВидЦіни = new Довідники.ВидиЦін_Pointer();
            Коментар = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Назва = "";
            
            //Табличні частини
            Товари_TablePart = new НадлишкиТоварів_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_f6"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_f7"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_f8"]);
                ВидЦіни = new Довідники.ВидиЦін_Pointer(base.FieldValue["col_f9"]);
                Коментар = base.FieldValue["col_g1"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Назва = base.FieldValue["docname"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_f6"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_f7"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_f8"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_f9"] = ВидЦіни.UnigueID.UGuid;
            base.FieldValue["col_g1"] = Коментар;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["docname"] = Назва;
            
            BaseSave();
			
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            BaseSpend(false, DateTime.MinValue);
		    return false;
		}

		public void ClearSpendTheDocument()
		{
            BaseSpend(false, DateTime.MinValue);
		}

		public НадлишкиТоварів_Objest Copy()
        {
            НадлишкиТоварів_Objest copy = new НадлишкиТоварів_Objest();
			copy.New();
            copy.Організація = Організація;
			copy.Підрозділ = Підрозділ;
			copy.Склад = Склад;
			copy.ВидЦіни = ВидЦіни;
			copy.Коментар = Коментар;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Назва = Назва;
			
			return copy;
        }

        public void Delete()
        {
		    
            base.BaseDelete(new string[] { "tab_a89" });
        }
        
        public НадлишкиТоварів_Pointer GetDocumentPointer()
        {
            НадлишкиТоварів_Pointer directoryPointer = new НадлишкиТоварів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
        public string Коментар { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public string Назва { get; set; }
        
        //Табличні частини
        public НадлишкиТоварів_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class НадлишкиТоварів_Pointer : DocumentPointer
    {
        public НадлишкиТоварів_Pointer(object uid = null) : base(Config.Kernel, "tab_a88", "НадлишкиТоварів")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public НадлишкиТоварів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a88", "НадлишкиТоварів")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public НадлишкиТоварів_Pointer GetNewDocumentPointer()
        {
            return new НадлишкиТоварів_Pointer(base.UnigueID);
        }
		
        public НадлишкиТоварів_Pointer GetEmptyPointer()
        {
            return new НадлишкиТоварів_Pointer();
        }
		
        public НадлишкиТоварів_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            НадлишкиТоварів_Objest НадлишкиТоварівObjestItem = new НадлишкиТоварів_Objest();
            НадлишкиТоварівObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				НадлишкиТоварівObjestItem.Товари_TablePart.Read();
			}
			
            return НадлишкиТоварівObjestItem;
        }
    }
    
    
    public class НадлишкиТоварів_Select : DocumentSelect
    {		
        public НадлишкиТоварів_Select() : base(Config.Kernel, "tab_a88") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new НадлишкиТоварів_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public НадлишкиТоварів_Pointer Current { get; private set; }
    }
    
      
    public class НадлишкиТоварів_Товари_TablePart : DocumentTablePart
    {
        public НадлишкиТоварів_Товари_TablePart(НадлишкиТоварів_Objest owner) : base(Config.Kernel, "tab_a89",
             new string[] { "col_g2", "col_g3", "col_g4", "col_g5", "col_g6" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string Номенклатура = "col_g2";
        public const string ХарактеристикаНоменклатури = "col_g3";
        public const string Кількість = "col_g4";
        public const string Ціна = "col_g5";
        public const string Сума = "col_g6";

        public НадлишкиТоварів_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_g2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_g3"]);
                record.Кількість = (fieldValue["col_g4"] != DBNull.Value) ? (decimal)fieldValue["col_g4"] : 0;
                record.Ціна = (fieldValue["col_g5"] != DBNull.Value) ? (decimal)fieldValue["col_g5"] : 0;
                record.Сума = (fieldValue["col_g6"] != DBNull.Value) ? (decimal)fieldValue["col_g6"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_g2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_g3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_g4", record.Кількість);
                fieldValue.Add("col_g5", record.Ціна);
                fieldValue.Add("col_g6", record.Сума);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Кількість = 0;
                Ціна = 0;
                Сума = 0;
                
            }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ПересортицяТоварів"
    
    public static class ПересортицяТоварів_Const
    {
        public const string TABLE = "tab_a90";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_h2";
        public const string Підрозділ = "col_h3";
        public const string Склад = "col_h4";
        public const string ВидЦіни = "col_h5";
        public const string Коментар = "col_h6";
    }
	
    
    public class ПересортицяТоварів_Objest : DocumentObject
    {
        public ПересортицяТоварів_Objest() : base(Config.Kernel, "tab_a90", "ПересортицяТоварів",
             new string[] { "docname", "docnomer", "docdate", "col_h2", "col_h3", "col_h4", "col_h5", "col_h6" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Склад = new Довідники.Склади_Pointer();
            ВидЦіни = new Довідники.ВидиЦін_Pointer();
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ПересортицяТоварів_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_h2"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_h3"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_h4"]);
                ВидЦіни = new Довідники.ВидиЦін_Pointer(base.FieldValue["col_h5"]);
                Коментар = base.FieldValue["col_h6"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_h2"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_h3"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_h4"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_h5"] = ВидЦіни.UnigueID.UGuid;
            base.FieldValue["col_h6"] = Коментар;
            
            BaseSave();
			
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            BaseSpend(false, DateTime.MinValue);
		    return false;
		}

		public void ClearSpendTheDocument()
		{
            BaseSpend(false, DateTime.MinValue);
		}

		public ПересортицяТоварів_Objest Copy()
        {
            ПересортицяТоварів_Objest copy = new ПересортицяТоварів_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Підрозділ = Підрозділ;
			copy.Склад = Склад;
			copy.ВидЦіни = ВидЦіни;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    
            base.BaseDelete(new string[] { "tab_a91" });
        }
        
        public ПересортицяТоварів_Pointer GetDocumentPointer()
        {
            ПересортицяТоварів_Pointer directoryPointer = new ПересортицяТоварів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ПересортицяТоварів_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ПересортицяТоварів_Pointer : DocumentPointer
    {
        public ПересортицяТоварів_Pointer(object uid = null) : base(Config.Kernel, "tab_a90", "ПересортицяТоварів")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПересортицяТоварів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a90", "ПересортицяТоварів")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ПересортицяТоварів_Pointer GetNewDocumentPointer()
        {
            return new ПересортицяТоварів_Pointer(base.UnigueID);
        }
		
        public ПересортицяТоварів_Pointer GetEmptyPointer()
        {
            return new ПересортицяТоварів_Pointer();
        }
		
        public ПересортицяТоварів_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ПересортицяТоварів_Objest ПересортицяТоварівObjestItem = new ПересортицяТоварів_Objest();
            ПересортицяТоварівObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ПересортицяТоварівObjestItem.Товари_TablePart.Read();
			}
			
            return ПересортицяТоварівObjestItem;
        }
    }
    
    
    public class ПересортицяТоварів_Select : DocumentSelect
    {		
        public ПересортицяТоварів_Select() : base(Config.Kernel, "tab_a90") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПересортицяТоварів_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ПересортицяТоварів_Pointer Current { get; private set; }
    }
    
      
    public class ПересортицяТоварів_Товари_TablePart : DocumentTablePart
    {
        public ПересортицяТоварів_Товари_TablePart(ПересортицяТоварів_Objest owner) : base(Config.Kernel, "tab_a91",
             new string[] { "col_a1", "col_h7", "col_h8", "col_i1", "col_h9" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string Номенклатура = "col_h7";
        public const string ХарактеристикаНоменклатури = "col_h8";
        public const string Кількість = "col_i1";
        public const string Ціна = "col_h9";

        public ПересортицяТоварів_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_h7"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_h8"]);
                record.Кількість = (fieldValue["col_i1"] != DBNull.Value) ? (decimal)fieldValue["col_i1"] : 0;
                record.Ціна = (fieldValue["col_h9"] != DBNull.Value) ? (decimal)fieldValue["col_h9"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_h7", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_h8", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_i1", record.Кількість);
                fieldValue.Add("col_h9", record.Ціна);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Кількість = 0;
                Ціна = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ПерерахунокТоварів"
    
    public static class ПерерахунокТоварів_Const
    {
        public const string TABLE = "tab_a92";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Склад = "col_i6";
        public const string Відповідальний = "col_i7";
        public const string Коментар = "col_i5";
    }
	
    
    public class ПерерахунокТоварів_Objest : DocumentObject
    {
        public ПерерахунокТоварів_Objest() : base(Config.Kernel, "tab_a92", "ПерерахунокТоварів",
             new string[] { "docname", "docnomer", "docdate", "col_i6", "col_i7", "col_i5" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Склад = new Довідники.Склади_Pointer();
            Відповідальний = new Довідники.ФізичніОсоби_Pointer();
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ПерерахунокТоварів_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_i6"]);
                Відповідальний = new Довідники.ФізичніОсоби_Pointer(base.FieldValue["col_i7"]);
                Коментар = base.FieldValue["col_i5"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_i6"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_i7"] = Відповідальний.UnigueID.UGuid;
            base.FieldValue["col_i5"] = Коментар;
            
            BaseSave();
			
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            BaseSpend(false, DateTime.MinValue);
		    return false;
		}

		public void ClearSpendTheDocument()
		{
            BaseSpend(false, DateTime.MinValue);
		}

		public ПерерахунокТоварів_Objest Copy()
        {
            ПерерахунокТоварів_Objest copy = new ПерерахунокТоварів_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Склад = Склад;
			copy.Відповідальний = Відповідальний;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    
            base.BaseDelete(new string[] { "tab_a93" });
        }
        
        public ПерерахунокТоварів_Pointer GetDocumentPointer()
        {
            ПерерахунокТоварів_Pointer directoryPointer = new ПерерахунокТоварів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.ФізичніОсоби_Pointer Відповідальний { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ПерерахунокТоварів_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ПерерахунокТоварів_Pointer : DocumentPointer
    {
        public ПерерахунокТоварів_Pointer(object uid = null) : base(Config.Kernel, "tab_a92", "ПерерахунокТоварів")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПерерахунокТоварів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a92", "ПерерахунокТоварів")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ПерерахунокТоварів_Pointer GetNewDocumentPointer()
        {
            return new ПерерахунокТоварів_Pointer(base.UnigueID);
        }
		
        public ПерерахунокТоварів_Pointer GetEmptyPointer()
        {
            return new ПерерахунокТоварів_Pointer();
        }
		
        public ПерерахунокТоварів_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ПерерахунокТоварів_Objest ПерерахунокТоварівObjestItem = new ПерерахунокТоварів_Objest();
            ПерерахунокТоварівObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ПерерахунокТоварівObjestItem.Товари_TablePart.Read();
			}
			
            return ПерерахунокТоварівObjestItem;
        }
    }
    
    
    public class ПерерахунокТоварів_Select : DocumentSelect
    {		
        public ПерерахунокТоварів_Select() : base(Config.Kernel, "tab_a92") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПерерахунокТоварів_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ПерерахунокТоварів_Pointer Current { get; private set; }
    }
    
      
    public class ПерерахунокТоварів_Товари_TablePart : DocumentTablePart
    {
        public ПерерахунокТоварів_Товари_TablePart(ПерерахунокТоварів_Objest owner) : base(Config.Kernel, "tab_a93",
             new string[] { "col_i8", "col_i9", "col_j1", "col_j3", "col_j4", "col_j5", "col_j6" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string Кількість = "col_i8";
        public const string КількістьФакт = "col_i9";
        public const string КількістьУпаковок = "col_j1";
        public const string КількістьУпаковокФакт = "col_j3";
        public const string Номенклатура = "col_j4";
        public const string Пакування = "col_j5";
        public const string ХарактеристикаНоменклатури = "col_j6";

        public ПерерахунокТоварів_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.Кількість = (fieldValue["col_i8"] != DBNull.Value) ? (decimal)fieldValue["col_i8"] : 0;
                record.КількістьФакт = (fieldValue["col_i9"] != DBNull.Value) ? (decimal)fieldValue["col_i9"] : 0;
                record.КількістьУпаковок = (fieldValue["col_j1"] != DBNull.Value) ? (int)fieldValue["col_j1"] : 0;
                record.КількістьУпаковокФакт = (fieldValue["col_j3"] != DBNull.Value) ? (int)fieldValue["col_j3"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_j4"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_j5"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_j6"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_i8", record.Кількість);
                fieldValue.Add("col_i9", record.КількістьФакт);
                fieldValue.Add("col_j1", record.КількістьУпаковок);
                fieldValue.Add("col_j3", record.КількістьУпаковокФакт);
                fieldValue.Add("col_j4", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_j5", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_j6", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                Кількість = 0;
                КількістьФакт = 0;
                КількістьУпаковок = 0;
                КількістьУпаковокФакт = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                
            }
            public decimal Кількість { get; set; }
            public decimal КількістьФакт { get; set; }
            public int КількістьУпаковок { get; set; }
            public int КількістьУпаковокФакт { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ПсуванняТоварів"
    
    public static class ПсуванняТоварів_Const
    {
        public const string TABLE = "tab_a94";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_a6";
        public const string Підрозділ = "col_a7";
        public const string Склад = "col_a4";
        public const string Причина = "col_a8";
        public const string ВидЦіни = "col_b1";
        public const string СумаДокументу = "col_a1";
        public const string Коментар = "col_a5";
    }
	
    
    public class ПсуванняТоварів_Objest : DocumentObject
    {
        public ПсуванняТоварів_Objest() : base(Config.Kernel, "tab_a94", "ПсуванняТоварів",
             new string[] { "docname", "docnomer", "docdate", "col_a6", "col_a7", "col_a4", "col_a8", "col_b1", "col_a1", "col_a5" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Склад = new Довідники.Склади_Pointer();
            Причина = "";
            ВидЦіни = new Довідники.ВидиЦін_Pointer();
            СумаДокументу = 0;
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ПсуванняТоварів_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_a6"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a7"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_a4"]);
                Причина = base.FieldValue["col_a8"].ToString();
                ВидЦіни = new Довідники.ВидиЦін_Pointer(base.FieldValue["col_b1"]);
                СумаДокументу = (base.FieldValue["col_a1"] != DBNull.Value) ? (decimal)base.FieldValue["col_a1"] : 0;
                Коментар = base.FieldValue["col_a5"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ПсуванняТоварів_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_a6"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_a7"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_a4"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_a8"] = Причина;
            base.FieldValue["col_b1"] = ВидЦіни.UnigueID.UGuid;
            base.FieldValue["col_a1"] = СумаДокументу;
            base.FieldValue["col_a5"] = Коментар;
            
            BaseSave();
			ПсуванняТоварів_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ПсуванняТоварів_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ПсуванняТоварів_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ПсуванняТоварів_Objest Copy()
        {
            ПсуванняТоварів_Objest copy = new ПсуванняТоварів_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Підрозділ = Підрозділ;
			copy.Склад = Склад;
			copy.Причина = Причина;
			copy.ВидЦіни = ВидЦіни;
			copy.СумаДокументу = СумаДокументу;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ПсуванняТоварів_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_a95" });
        }
        
        public ПсуванняТоварів_Pointer GetDocumentPointer()
        {
            ПсуванняТоварів_Pointer directoryPointer = new ПсуванняТоварів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public string Причина { get; set; }
        public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
        public decimal СумаДокументу { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ПсуванняТоварів_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ПсуванняТоварів_Pointer : DocumentPointer
    {
        public ПсуванняТоварів_Pointer(object uid = null) : base(Config.Kernel, "tab_a94", "ПсуванняТоварів")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ПсуванняТоварів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_a94", "ПсуванняТоварів")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ПсуванняТоварів_Pointer GetNewDocumentPointer()
        {
            return new ПсуванняТоварів_Pointer(base.UnigueID);
        }
		
        public ПсуванняТоварів_Pointer GetEmptyPointer()
        {
            return new ПсуванняТоварів_Pointer();
        }
		
        public ПсуванняТоварів_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ПсуванняТоварів_Objest ПсуванняТоварівObjestItem = new ПсуванняТоварів_Objest();
            ПсуванняТоварівObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ПсуванняТоварівObjestItem.Товари_TablePart.Read();
			}
			
            return ПсуванняТоварівObjestItem;
        }
    }
    
    
    public class ПсуванняТоварів_Select : DocumentSelect
    {		
        public ПсуванняТоварів_Select() : base(Config.Kernel, "tab_a94") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ПсуванняТоварів_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ПсуванняТоварів_Pointer Current { get; private set; }
    }
    
      
    public class ПсуванняТоварів_Товари_TablePart : DocumentTablePart
    {
        public ПсуванняТоварів_Товари_TablePart(ПсуванняТоварів_Objest owner) : base(Config.Kernel, "tab_a95",
             new string[] { "col_a3", "col_b2", "col_b3", "col_a1", "col_a4", "col_a5", "col_b4", "col_b5", "col_a2" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a3";
        public const string Номенклатура = "col_b2";
        public const string ХарактеристикаНоменклатури = "col_b3";
        public const string Серія = "col_a1";
        public const string Пакування = "col_a4";
        public const string КількістьУпаковок = "col_a5";
        public const string Кількість = "col_b4";
        public const string Ціна = "col_b5";
        public const string Сума = "col_a2";

        public ПсуванняТоварів_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a3"] != DBNull.Value) ? (int)fieldValue["col_a3"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_b2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_b3"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_a4"]);
                record.КількістьУпаковок = (fieldValue["col_a5"] != DBNull.Value) ? (int)fieldValue["col_a5"] : 0;
                record.Кількість = (fieldValue["col_b4"] != DBNull.Value) ? (decimal)fieldValue["col_b4"] : 0;
                record.Ціна = (fieldValue["col_b5"] != DBNull.Value) ? (decimal)fieldValue["col_b5"] : 0;
                record.Сума = (fieldValue["col_a2"] != DBNull.Value) ? (decimal)fieldValue["col_a2"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a3", record.НомерРядка);
                fieldValue.Add("col_b2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_b3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a1", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_a4", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_a5", record.КількістьУпаковок);
                fieldValue.Add("col_b4", record.Кількість);
                fieldValue.Add("col_b5", record.Ціна);
                fieldValue.Add("col_a2", record.Сума);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                Ціна = 0;
                Сума = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "ВнутрішнєСпоживанняТоварів"
    
    public static class ВнутрішнєСпоживанняТоварів_Const
    {
        public const string TABLE = "tab_b07";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Організація = "col_a1";
        public const string Склад = "col_b1";
        public const string Валюта = "col_a2";
        public const string Підрозділ = "col_a9";
        public const string СумаДокументу = "col_a8";
        public const string Основа = "col_b7";
        public const string Автор = "col_b9";
        public const string ГосподарськаОперація = "col_b4";
        public const string Коментар = "col_b2";
    }
	
    
    public class ВнутрішнєСпоживанняТоварів_Objest : DocumentObject
    {
        public ВнутрішнєСпоживанняТоварів_Objest() : base(Config.Kernel, "tab_b07", "ВнутрішнєСпоживанняТоварів",
             new string[] { "docname", "docnomer", "docdate", "col_a1", "col_b1", "col_a2", "col_a9", "col_a8", "col_b7", "col_b9", "col_b4", "col_b2" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Організація = new Довідники.Організації_Pointer();
            Склад = new Довідники.Склади_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            СумаДокументу = 0;
            Основа = "";
            Автор = new Довідники.Користувачі_Pointer();
            ГосподарськаОперація = 0;
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new ВнутрішнєСпоживанняТоварів_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_a1"]);
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_b1"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_a2"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_a9"]);
                СумаДокументу = (base.FieldValue["col_a8"] != DBNull.Value) ? (decimal)base.FieldValue["col_a8"] : 0;
                Основа = base.FieldValue["col_b7"].ToString();
                Автор = new Довідники.Користувачі_Pointer(base.FieldValue["col_b9"]);
                ГосподарськаОперація = (base.FieldValue["col_b4"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_b4"] : 0;
                Коментар = base.FieldValue["col_b2"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            ВнутрішнєСпоживанняТоварів_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_a1"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_b1"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_a2"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_a9"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_a8"] = СумаДокументу;
            base.FieldValue["col_b7"] = Основа;
            base.FieldValue["col_b9"] = Автор.UnigueID.UGuid;
            base.FieldValue["col_b4"] = (int)ГосподарськаОперація;
            base.FieldValue["col_b2"] = Коментар;
            
            BaseSave();
			ВнутрішнєСпоживанняТоварів_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = ВнутрішнєСпоживанняТоварів_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            ВнутрішнєСпоживанняТоварів_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public ВнутрішнєСпоживанняТоварів_Objest Copy()
        {
            ВнутрішнєСпоживанняТоварів_Objest copy = new ВнутрішнєСпоживанняТоварів_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Організація = Організація;
			copy.Склад = Склад;
			copy.Валюта = Валюта;
			copy.Підрозділ = Підрозділ;
			copy.СумаДокументу = СумаДокументу;
			copy.Основа = Основа;
			copy.Автор = Автор;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    ВнутрішнєСпоживанняТоварів_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_b08" });
        }
        
        public ВнутрішнєСпоживанняТоварів_Pointer GetDocumentPointer()
        {
            ВнутрішнєСпоживанняТоварів_Pointer directoryPointer = new ВнутрішнєСпоживанняТоварів_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public decimal СумаДокументу { get; set; }
        public string Основа { get; set; }
        public Довідники.Користувачі_Pointer Автор { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public ВнутрішнєСпоживанняТоварів_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class ВнутрішнєСпоживанняТоварів_Pointer : DocumentPointer
    {
        public ВнутрішнєСпоживанняТоварів_Pointer(object uid = null) : base(Config.Kernel, "tab_b07", "ВнутрішнєСпоживанняТоварів")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public ВнутрішнєСпоживанняТоварів_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_b07", "ВнутрішнєСпоживанняТоварів")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public ВнутрішнєСпоживанняТоварів_Pointer GetNewDocumentPointer()
        {
            return new ВнутрішнєСпоживанняТоварів_Pointer(base.UnigueID);
        }
		
        public ВнутрішнєСпоживанняТоварів_Pointer GetEmptyPointer()
        {
            return new ВнутрішнєСпоживанняТоварів_Pointer();
        }
		
        public ВнутрішнєСпоживанняТоварів_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            ВнутрішнєСпоживанняТоварів_Objest ВнутрішнєСпоживанняТоварівObjestItem = new ВнутрішнєСпоживанняТоварів_Objest();
            ВнутрішнєСпоживанняТоварівObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				ВнутрішнєСпоживанняТоварівObjestItem.Товари_TablePart.Read();
			}
			
            return ВнутрішнєСпоживанняТоварівObjestItem;
        }
    }
    
    
    public class ВнутрішнєСпоживанняТоварів_Select : DocumentSelect
    {		
        public ВнутрішнєСпоживанняТоварів_Select() : base(Config.Kernel, "tab_b07") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new ВнутрішнєСпоживанняТоварів_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public ВнутрішнєСпоживанняТоварів_Pointer Current { get; private set; }
    }
    
      
    public class ВнутрішнєСпоживанняТоварів_Товари_TablePart : DocumentTablePart
    {
        public ВнутрішнєСпоживанняТоварів_Товари_TablePart(ВнутрішнєСпоживанняТоварів_Objest owner) : base(Config.Kernel, "tab_b08",
             new string[] { "col_a1", "col_d2", "col_d3", "col_a2", "col_d4", "col_d5", "col_d6", "col_d8", "col_d9" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a1";
        public const string Номенклатура = "col_d2";
        public const string ХарактеристикаНоменклатури = "col_d3";
        public const string Серія = "col_a2";
        public const string Пакування = "col_d4";
        public const string КількістьУпаковок = "col_d5";
        public const string Кількість = "col_d6";
        public const string Ціна = "col_d8";
        public const string Сума = "col_d9";

        public ВнутрішнєСпоживанняТоварів_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a1"] != DBNull.Value) ? (int)fieldValue["col_a1"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_d2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_d3"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a2"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_d4"]);
                record.КількістьУпаковок = (fieldValue["col_d5"] != DBNull.Value) ? (int)fieldValue["col_d5"] : 0;
                record.Кількість = (fieldValue["col_d6"] != DBNull.Value) ? (decimal)fieldValue["col_d6"] : 0;
                record.Ціна = (fieldValue["col_d8"] != DBNull.Value) ? (decimal)fieldValue["col_d8"] : 0;
                record.Сума = (fieldValue["col_d9"] != DBNull.Value) ? (decimal)fieldValue["col_d9"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a1", record.НомерРядка);
                fieldValue.Add("col_d2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_d3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a2", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_d4", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_d5", record.КількістьУпаковок);
                fieldValue.Add("col_d6", record.Кількість);
                fieldValue.Add("col_d8", record.Ціна);
                fieldValue.Add("col_d9", record.Сума);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                Ціна = 0;
                Сума = 0;
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
      
    
    #endregion
    
    #region DOCUMENT "РахунокФактура"
    
    public static class РахунокФактура_Const
    {
        public const string TABLE = "tab_b10";
        
        public const string Назва = "docname";
        public const string НомерДок = "docnomer";
        public const string ДатаДок = "docdate";
        public const string Контрагент = "col_b4";
        public const string Організація = "col_b5";
        public const string Валюта = "col_b6";
        public const string СумаДокументу = "col_b7";
        public const string Склад = "col_b8";
        public const string Статус = "col_a1";
        public const string ФормаОплати = "col_a3";
        public const string БанківськийРахунок = "col_a4";
        public const string БанківськийРахунокКонтрагента = "col_a5";
        public const string Каса = "col_a6";
        public const string ГосподарськаОперація = "col_b9";
        public const string Договір = "col_c2";
        public const string Підрозділ = "col_c3";
        public const string Автор = "col_c4";
        public const string Коментар = "col_c1";
    }
	
    
    public class РахунокФактура_Objest : DocumentObject
    {
        public РахунокФактура_Objest() : base(Config.Kernel, "tab_b10", "РахунокФактура",
             new string[] { "docname", "docnomer", "docdate", "col_b4", "col_b5", "col_b6", "col_b7", "col_b8", "col_a1", "col_a3", "col_a4", "col_a5", "col_a6", "col_b9", "col_c2", "col_c3", "col_c4", "col_c1" }) 
        {
            Назва = "";
            НомерДок = "";
            ДатаДок = DateTime.MinValue;
            Контрагент = new Довідники.Контрагенти_Pointer();
            Організація = new Довідники.Організації_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            СумаДокументу = 0;
            Склад = new Довідники.Склади_Pointer();
            Статус = 0;
            ФормаОплати = 0;
            БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer();
            БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer();
            Каса = new Довідники.Каси_Pointer();
            ГосподарськаОперація = 0;
            Договір = new Довідники.ДоговориКонтрагентів_Pointer();
            Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
            Автор = new Довідники.Користувачі_Pointer();
            Коментар = "";
            
            //Табличні частини
            Товари_TablePart = new РахунокФактура_Товари_TablePart(this);
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Назва = base.FieldValue["docname"].ToString();
                НомерДок = base.FieldValue["docnomer"].ToString();
                ДатаДок = (base.FieldValue["docdate"] != DBNull.Value) ? DateTime.Parse(base.FieldValue["docdate"].ToString()) : DateTime.MinValue;
                Контрагент = new Довідники.Контрагенти_Pointer(base.FieldValue["col_b4"]);
                Організація = new Довідники.Організації_Pointer(base.FieldValue["col_b5"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_b6"]);
                СумаДокументу = (base.FieldValue["col_b7"] != DBNull.Value) ? (decimal)base.FieldValue["col_b7"] : 0;
                Склад = new Довідники.Склади_Pointer(base.FieldValue["col_b8"]);
                Статус = (base.FieldValue["col_a1"] != DBNull.Value) ? (Перелічення.СтатусиЗамовленьКлієнтів)base.FieldValue["col_a1"] : 0;
                ФормаОплати = (base.FieldValue["col_a3"] != DBNull.Value) ? (Перелічення.ФормаОплати)base.FieldValue["col_a3"] : 0;
                БанківськийРахунок = new Довідники.БанківськіРахункиОрганізацій_Pointer(base.FieldValue["col_a4"]);
                БанківськийРахунокКонтрагента = new Довідники.БанківськіРахункиКонтрагентів_Pointer(base.FieldValue["col_a5"]);
                Каса = new Довідники.Каси_Pointer(base.FieldValue["col_a6"]);
                ГосподарськаОперація = (base.FieldValue["col_b9"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)base.FieldValue["col_b9"] : 0;
                Договір = new Довідники.ДоговориКонтрагентів_Pointer(base.FieldValue["col_c2"]);
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer(base.FieldValue["col_c3"]);
                Автор = new Довідники.Користувачі_Pointer(base.FieldValue["col_c4"]);
                Коментар = base.FieldValue["col_c1"].ToString();
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            РахунокФактура_Triggers.BeforeRecording(this);
			base.FieldValue["docname"] = Назва;
            base.FieldValue["docnomer"] = НомерДок;
            base.FieldValue["docdate"] = ДатаДок;
            base.FieldValue["col_b4"] = Контрагент.UnigueID.UGuid;
            base.FieldValue["col_b5"] = Організація.UnigueID.UGuid;
            base.FieldValue["col_b6"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_b7"] = СумаДокументу;
            base.FieldValue["col_b8"] = Склад.UnigueID.UGuid;
            base.FieldValue["col_a1"] = (int)Статус;
            base.FieldValue["col_a3"] = (int)ФормаОплати;
            base.FieldValue["col_a4"] = БанківськийРахунок.UnigueID.UGuid;
            base.FieldValue["col_a5"] = БанківськийРахунокКонтрагента.UnigueID.UGuid;
            base.FieldValue["col_a6"] = Каса.UnigueID.UGuid;
            base.FieldValue["col_b9"] = (int)ГосподарськаОперація;
            base.FieldValue["col_c2"] = Договір.UnigueID.UGuid;
            base.FieldValue["col_c3"] = Підрозділ.UnigueID.UGuid;
            base.FieldValue["col_c4"] = Автор.UnigueID.UGuid;
            base.FieldValue["col_c1"] = Коментар;
            
            BaseSave();
			РахунокФактура_Triggers.AfterRecording(this);
		}

		public bool SpendTheDocument(DateTime spendDate)
		{
            bool rezult = РахунокФактура_SpendTheDocument.Spend(this);
		    BaseSpend(rezult, spendDate);
		    return rezult;
		}

		public void ClearSpendTheDocument()
		{
            РахунокФактура_SpendTheDocument.ClearSpend(this);
			BaseSpend(false, DateTime.MinValue);
		}

		public РахунокФактура_Objest Copy()
        {
            РахунокФактура_Objest copy = new РахунокФактура_Objest();
			copy.New();
            copy.Назва = Назва;
			copy.НомерДок = НомерДок;
			copy.ДатаДок = ДатаДок;
			copy.Контрагент = Контрагент;
			copy.Організація = Організація;
			copy.Валюта = Валюта;
			copy.СумаДокументу = СумаДокументу;
			copy.Склад = Склад;
			copy.Статус = Статус;
			copy.ФормаОплати = ФормаОплати;
			copy.БанківськийРахунок = БанківськийРахунок;
			copy.БанківськийРахунокКонтрагента = БанківськийРахунокКонтрагента;
			copy.Каса = Каса;
			copy.ГосподарськаОперація = ГосподарськаОперація;
			copy.Договір = Договір;
			copy.Підрозділ = Підрозділ;
			copy.Автор = Автор;
			copy.Коментар = Коментар;
			
			return copy;
        }

        public void Delete()
        {
		    РахунокФактура_Triggers.BeforeDelete(this);
            base.BaseDelete(new string[] { "tab_b11" });
        }
        
        public РахунокФактура_Pointer GetDocumentPointer()
        {
            РахунокФактура_Pointer directoryPointer = new РахунокФактура_Pointer(UnigueID.UGuid);
            return directoryPointer;
        }
        
        public string Назва { get; set; }
        public string НомерДок { get; set; }
        public DateTime ДатаДок { get; set; }
        public Довідники.Контрагенти_Pointer Контрагент { get; set; }
        public Довідники.Організації_Pointer Організація { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public decimal СумаДокументу { get; set; }
        public Довідники.Склади_Pointer Склад { get; set; }
        public Перелічення.СтатусиЗамовленьКлієнтів Статус { get; set; }
        public Перелічення.ФормаОплати ФормаОплати { get; set; }
        public Довідники.БанківськіРахункиОрганізацій_Pointer БанківськийРахунок { get; set; }
        public Довідники.БанківськіРахункиКонтрагентів_Pointer БанківськийРахунокКонтрагента { get; set; }
        public Довідники.Каси_Pointer Каса { get; set; }
        public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
        public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
        public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
        public Довідники.Користувачі_Pointer Автор { get; set; }
        public string Коментар { get; set; }
        
        //Табличні частини
        public РахунокФактура_Товари_TablePart Товари_TablePart { get; set; }
        
    }
    
    
    public class РахунокФактура_Pointer : DocumentPointer
    {
        public РахунокФактура_Pointer(object uid = null) : base(Config.Kernel, "tab_b10", "РахунокФактура")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public РахунокФактура_Pointer(UnigueID uid, Dictionary<string, object> fields = null) : base(Config.Kernel, "tab_b10", "РахунокФактура")
        {
            base.Init(uid, fields);
        }
		
		public string GetPresentation()
        {
		    return base.BasePresentation(
				new string[] { "docname" }
			);
        }
		
        public РахунокФактура_Pointer GetNewDocumentPointer()
        {
            return new РахунокФактура_Pointer(base.UnigueID);
        }
		
        public РахунокФактура_Pointer GetEmptyPointer()
        {
            return new РахунокФактура_Pointer();
        }
		
        public РахунокФактура_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            РахунокФактура_Objest РахунокФактураObjestItem = new РахунокФактура_Objest();
            РахунокФактураObjestItem.Read(base.UnigueID);
			
			if (readAllTablePart)
			{   
				РахунокФактураObjestItem.Товари_TablePart.Read();
			}
			
            return РахунокФактураObjestItem;
        }
    }
    
    
    public class РахунокФактура_Select : DocumentSelect
    {		
        public РахунокФактура_Select() : base(Config.Kernel, "tab_b10") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new РахунокФактура_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public РахунокФактура_Pointer Current { get; private set; }
    }
    
      
    public class РахунокФактура_Товари_TablePart : DocumentTablePart
    {
        public РахунокФактура_Товари_TablePart(РахунокФактура_Objest owner) : base(Config.Kernel, "tab_b11",
             new string[] { "col_a2", "col_b9", "col_c1", "col_c2", "col_c3", "col_c4", "col_c5", "col_c6", "col_c7", "col_c8", "col_a1" }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List<Record>();
        }
        
        public const string НомерРядка = "col_a2";
        public const string Номенклатура = "col_b9";
        public const string ХарактеристикаНоменклатури = "col_c1";
        public const string Пакування = "col_c2";
        public const string КількістьУпаковок = "col_c3";
        public const string Кількість = "col_c4";
        public const string ВидЦіни = "col_c5";
        public const string Ціна = "col_c6";
        public const string Сума = "col_c7";
        public const string Скидка = "col_c8";
        public const string Склад = "col_a1";

        public РахунокФактура_Objest Owner { get; private set; }
        
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                record.НомерРядка = (fieldValue["col_a2"] != DBNull.Value) ? (int)fieldValue["col_a2"] : 0;
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_b9"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_c1"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_c2"]);
                record.КількістьУпаковок = (fieldValue["col_c3"] != DBNull.Value) ? (int)fieldValue["col_c3"] : 0;
                record.Кількість = (fieldValue["col_c4"] != DBNull.Value) ? (decimal)fieldValue["col_c4"] : 0;
                record.ВидЦіни = new Довідники.ВидиЦін_Pointer(fieldValue["col_c5"]);
                record.Ціна = (fieldValue["col_c6"] != DBNull.Value) ? (decimal)fieldValue["col_c6"] : 0;
                record.Сума = (fieldValue["col_c7"] != DBNull.Value) ? (decimal)fieldValue["col_c7"] : 0;
                record.Скидка = (fieldValue["col_c8"] != DBNull.Value) ? (decimal)fieldValue["col_c8"] : 0;
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a1"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();

                fieldValue.Add("col_a2", record.НомерРядка);
                fieldValue.Add("col_b9", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_c1", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_c2", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_c3", record.КількістьУпаковок);
                fieldValue.Add("col_c4", record.Кількість);
                fieldValue.Add("col_c5", record.ВидЦіни.UnigueID.UGuid);
                fieldValue.Add("col_c6", record.Ціна);
                fieldValue.Add("col_c7", record.Сума);
                fieldValue.Add("col_c8", record.Скидка);
                fieldValue.Add("col_a1", record.Склад.UnigueID.UGuid);
                
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseBeginTransaction();
            base.BaseDelete(Owner.UnigueID);
            base.BaseCommitTransaction();
        }

        public List<Record> Copy()
        {
            List<Record> copyRecords = new List<Record>();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DocumentTablePartRecord
        {
            public Record()
            {
                НомерРядка = 0;
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                КількістьУпаковок = 0;
                Кількість = 0;
                ВидЦіни = new Довідники.ВидиЦін_Pointer();
                Ціна = 0;
                Сума = 0;
                Скидка = 0;
                Склад = new Довідники.Склади_Pointer();
                
            }
            public int НомерРядка { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public int КількістьУпаковок { get; set; }
            public decimal Кількість { get; set; }
            public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
            public decimal Ціна { get; set; }
            public decimal Сума { get; set; }
            public decimal Скидка { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            
        }
    }
      
    
    #endregion
    
}

namespace StorageAndTrade_1_0.Журнали
{
    #region Journal
    public class Journal_Select: JournalSelect
    {
        public Journal_Select() : base(Config.Kernel,
             new string[] { "tab_a25", "tab_a32", "tab_a34", "tab_a36", "tab_a42", "tab_a44", "tab_a48", "tab_a31", "tab_a51", "tab_a53", "tab_a81", "tab_a83", "tab_a88", "tab_a90", "tab_a92", "tab_a94", "tab_b07", "tab_b10"},
			 new string[] { "ЗамовленняПостачальнику", "ПоступленняТоварівТаПослуг", "ЗамовленняКлієнта", "РеалізаціяТоварівТаПослуг", "ВстановленняЦінНоменклатури", "ПрихіднийКасовийОрдер", "РозхіднийКасовийОрдер", "ПереміщенняТоварів", "ПоверненняТоварівПостачальнику", "ПоверненняТоварівВідКлієнта", "АктВиконанихРобіт", "ВведенняЗалишків", "НадлишкиТоварів", "ПересортицяТоварів", "ПерерахунокТоварів", "ПсуванняТоварів", "ВнутрішнєСпоживанняТоварів", "РахунокФактура"}) { }

        public DocumentObject GetDocumentObject(bool readAllTablePart = true)
        {
            if (Current == null)
                return null;

            switch (Current.TypeDocument)
            {
			    case "ЗамовленняПостачальнику": return new Документи.ЗамовленняПостачальнику_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ПоступленняТоварівТаПослуг": return new Документи.ПоступленняТоварівТаПослуг_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ЗамовленняКлієнта": return new Документи.ЗамовленняКлієнта_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "РеалізаціяТоварівТаПослуг": return new Документи.РеалізаціяТоварівТаПослуг_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ВстановленняЦінНоменклатури": return new Документи.ВстановленняЦінНоменклатури_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ПрихіднийКасовийОрдер": return new Документи.ПрихіднийКасовийОрдер_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "РозхіднийКасовийОрдер": return new Документи.РозхіднийКасовийОрдер_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ПереміщенняТоварів": return new Документи.ПереміщенняТоварів_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ПоверненняТоварівПостачальнику": return new Документи.ПоверненняТоварівПостачальнику_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ПоверненняТоварівВідКлієнта": return new Документи.ПоверненняТоварівВідКлієнта_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "АктВиконанихРобіт": return new Документи.АктВиконанихРобіт_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ВведенняЗалишків": return new Документи.ВведенняЗалишків_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "НадлишкиТоварів": return new Документи.НадлишкиТоварів_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ПересортицяТоварів": return new Документи.ПересортицяТоварів_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ПерерахунокТоварів": return new Документи.ПерерахунокТоварів_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ПсуванняТоварів": return new Документи.ПсуванняТоварів_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "ВнутрішнєСпоживанняТоварів": return new Документи.ВнутрішнєСпоживанняТоварів_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				case "РахунокФактура": return new Документи.РахунокФактура_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
				
            }
			
			return null;
        }
    }
    #endregion

}

namespace StorageAndTrade_1_0.РегістриВідомостей
{
    
    #region REGISTER "ЦіниНоменклатури"
    
    public static class ЦіниНоменклатури_Const
    {
        public const string TABLE = "tab_a40";
        
        public const string Номенклатура = "col_f5";
        public const string ХарактеристикаНоменклатури = "col_f6";
        public const string ВидЦіни = "col_f7";
        public const string Ціна = "col_f8";
        public const string Пакування = "col_f9";
        public const string Валюта = "col_g2";
    }
	
    
    public class ЦіниНоменклатури_RecordsSet : RegisterInformationRecordsSet
    {
        public ЦіниНоменклатури_RecordsSet() : base(Config.Kernel, "tab_a40",
             new string[] { "col_f5", "col_f6", "col_f7", "col_f8", "col_f9", "col_g2" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead();
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Owner = (Guid)fieldValue["owner"];
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_f5"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_f6"]);
                record.ВидЦіни = new Довідники.ВидиЦін_Pointer(fieldValue["col_f7"]);
                record.Ціна = (fieldValue["col_f8"] != DBNull.Value) ? (decimal)fieldValue["col_f8"] : 0;
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_f9"]);
                record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_g2"]);
                
                Records.Add(record);
            }
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_f5", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_f6", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_f7", record.ВидЦіни.UnigueID.UGuid);
                fieldValue.Add("col_f8", record.Ціна);
                fieldValue.Add("col_f9", record.Пакування.UnigueID.UGuid);
                fieldValue.Add("col_g2", record.Валюта.UnigueID.UGuid);
                
                base.BaseSave(record.UID, period, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }
        
        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }

        
        public class Record : RegisterInformationRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                ВидЦіни = new Довідники.ВидиЦін_Pointer();
                Ціна = 0;
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                Валюта = new Довідники.Валюти_Pointer();
                
            }
        
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
            public decimal Ціна { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            public Довідники.Валюти_Pointer Валюта { get; set; }
            
        }
    }
	
    
    public class ЦіниНоменклатури_Objest : RegisterInformationObject
    {
		public ЦіниНоменклатури_Objest() : base(Config.Kernel, "tab_a40",
             new string[] { "col_f5", "col_f6", "col_f7", "col_f8", "col_f9", "col_g2" }) 
        {
            Номенклатура = new Довідники.Номенклатура_Pointer();
            ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
            ВидЦіни = new Довідники.ВидиЦін_Pointer();
            Ціна = 0;
            Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
            Валюта = new Довідники.Валюти_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Номенклатура = new Довідники.Номенклатура_Pointer(base.FieldValue["col_f5"]);
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(base.FieldValue["col_f6"]);
                ВидЦіни = new Довідники.ВидиЦін_Pointer(base.FieldValue["col_f7"]);
                Ціна = (base.FieldValue["col_f8"] != DBNull.Value) ? (decimal)base.FieldValue["col_f8"] : 0;
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(base.FieldValue["col_f9"]);
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_g2"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_f5"] = Номенклатура.UnigueID.UGuid;
            base.FieldValue["col_f6"] = ХарактеристикаНоменклатури.UnigueID.UGuid;
            base.FieldValue["col_f7"] = ВидЦіни.UnigueID.UGuid;
            base.FieldValue["col_f8"] = Ціна;
            base.FieldValue["col_f9"] = Пакування.UnigueID.UGuid;
            base.FieldValue["col_g2"] = Валюта.UnigueID.UGuid;
            
            BaseSave();
        }

        public ЦіниНоменклатури_Objest Copy()
        {
            ЦіниНоменклатури_Objest copy = new ЦіниНоменклатури_Objest();
			copy.New();
            copy.Номенклатура = Номенклатура;
			copy.ХарактеристикаНоменклатури = ХарактеристикаНоменклатури;
			copy.ВидЦіни = ВидЦіни;
			copy.Ціна = Ціна;
			copy.Пакування = Пакування;
			copy.Валюта = Валюта;
			
			return copy;
        }

        public void Delete()
        {
			base.BaseDelete();
        }
                
        public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
        public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
        public Довідники.ВидиЦін_Pointer ВидЦіни { get; set; }
        public decimal Ціна { get; set; }
        public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
        public Довідники.Валюти_Pointer Валюта { get; set; }
        
    }
	
    #endregion
  
    #region REGISTER "КурсиВалют"
    
    public static class КурсиВалют_Const
    {
        public const string TABLE = "tab_a59";
        
        public const string Валюта = "col_a1";
        public const string Курс = "col_a2";
        public const string Кратність = "col_a3";
    }
	
    
    public class КурсиВалют_RecordsSet : RegisterInformationRecordsSet
    {
        public КурсиВалют_RecordsSet() : base(Config.Kernel, "tab_a59",
             new string[] { "col_a1", "col_a2", "col_a3" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead();
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Owner = (Guid)fieldValue["owner"];
                record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a1"]);
                record.Курс = (fieldValue["col_a2"] != DBNull.Value) ? (decimal)fieldValue["col_a2"] : 0;
                record.Кратність = (fieldValue["col_a3"] != DBNull.Value) ? (int)fieldValue["col_a3"] : 0;
                
                Records.Add(record);
            }
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a1", record.Валюта.UnigueID.UGuid);
                fieldValue.Add("col_a2", record.Курс);
                fieldValue.Add("col_a3", record.Кратність);
                
                base.BaseSave(record.UID, period, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }
        
        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }

        
        public class Record : RegisterInformationRecord
        {
            public Record()
            {
                Валюта = new Довідники.Валюти_Pointer();
                Курс = 0;
                Кратність = 0;
                
            }
        
            public Довідники.Валюти_Pointer Валюта { get; set; }
            public decimal Курс { get; set; }
            public int Кратність { get; set; }
            
        }
    }
	
    
    public class КурсиВалют_Objest : RegisterInformationObject
    {
		public КурсиВалют_Objest() : base(Config.Kernel, "tab_a59",
             new string[] { "col_a1", "col_a2", "col_a3" }) 
        {
            Валюта = new Довідники.Валюти_Pointer();
            Курс = 0;
            Кратність = 0;
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Валюта = new Довідники.Валюти_Pointer(base.FieldValue["col_a1"]);
                Курс = (base.FieldValue["col_a2"] != DBNull.Value) ? (decimal)base.FieldValue["col_a2"] : 0;
                Кратність = (base.FieldValue["col_a3"] != DBNull.Value) ? (int)base.FieldValue["col_a3"] : 0;
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_a1"] = Валюта.UnigueID.UGuid;
            base.FieldValue["col_a2"] = Курс;
            base.FieldValue["col_a3"] = Кратність;
            
            BaseSave();
        }

        public КурсиВалют_Objest Copy()
        {
            КурсиВалют_Objest copy = new КурсиВалют_Objest();
			copy.New();
            copy.Валюта = Валюта;
			copy.Курс = Курс;
			copy.Кратність = Кратність;
			
			return copy;
        }

        public void Delete()
        {
			base.BaseDelete();
        }
                
        public Довідники.Валюти_Pointer Валюта { get; set; }
        public decimal Курс { get; set; }
        public int Кратність { get; set; }
        
    }
	
    #endregion
  
    #region REGISTER "ШтрихкодиНоменклатури"
    
    public static class ШтрихкодиНоменклатури_Const
    {
        public const string TABLE = "tab_b17";
        
        public const string Штрихкод = "col_a1";
        public const string Номенклатура = "col_a2";
        public const string ХарактеристикаНоменклатури = "col_a3";
        public const string Пакування = "col_a4";
    }
	
    
    public class ШтрихкодиНоменклатури_RecordsSet : RegisterInformationRecordsSet
    {
        public ШтрихкодиНоменклатури_RecordsSet() : base(Config.Kernel, "tab_b17",
             new string[] { "col_a1", "col_a2", "col_a3", "col_a4" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead();
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Owner = (Guid)fieldValue["owner"];
                record.Штрихкод = fieldValue["col_a1"].ToString();
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                record.Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(fieldValue["col_a4"]);
                
                Records.Add(record);
            }
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a1", record.Штрихкод);
                fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a4", record.Пакування.UnigueID.UGuid);
                
                base.BaseSave(record.UID, period, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }
        
        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }

        
        public class Record : RegisterInformationRecord
        {
            public Record()
            {
                Штрихкод = "";
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
                
            }
        
            public string Штрихкод { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
            
        }
    }
	
    
    public class ШтрихкодиНоменклатури_Objest : RegisterInformationObject
    {
		public ШтрихкодиНоменклатури_Objest() : base(Config.Kernel, "tab_b17",
             new string[] { "col_a1", "col_a2", "col_a3", "col_a4" }) 
        {
            Штрихкод = "";
            Номенклатура = new Довідники.Номенклатура_Pointer();
            ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
            Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Штрихкод = base.FieldValue["col_a1"].ToString();
                Номенклатура = new Довідники.Номенклатура_Pointer(base.FieldValue["col_a2"]);
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(base.FieldValue["col_a3"]);
                Пакування = new Довідники.ПакуванняОдиниціВиміру_Pointer(base.FieldValue["col_a4"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_a1"] = Штрихкод;
            base.FieldValue["col_a2"] = Номенклатура.UnigueID.UGuid;
            base.FieldValue["col_a3"] = ХарактеристикаНоменклатури.UnigueID.UGuid;
            base.FieldValue["col_a4"] = Пакування.UnigueID.UGuid;
            
            BaseSave();
        }

        public ШтрихкодиНоменклатури_Objest Copy()
        {
            ШтрихкодиНоменклатури_Objest copy = new ШтрихкодиНоменклатури_Objest();
			copy.New();
            copy.Штрихкод = Штрихкод;
			copy.Номенклатура = Номенклатура;
			copy.ХарактеристикаНоменклатури = ХарактеристикаНоменклатури;
			copy.Пакування = Пакування;
			
			return copy;
        }

        public void Delete()
        {
			base.BaseDelete();
        }
                
        public string Штрихкод { get; set; }
        public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
        public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
        public Довідники.ПакуванняОдиниціВиміру_Pointer Пакування { get; set; }
        
    }
	
    #endregion
  
    #region REGISTER "ФайлиДокументів"
    
    public static class ФайлиДокументів_Const
    {
        public const string TABLE = "tab_b23";
        
        public const string Файл = "col_a1";
    }
	
    
    public class ФайлиДокументів_RecordsSet : RegisterInformationRecordsSet
    {
        public ФайлиДокументів_RecordsSet() : base(Config.Kernel, "tab_b23",
             new string[] { "col_a1" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead();
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Owner = (Guid)fieldValue["owner"];
                record.Файл = new Довідники.Файли_Pointer(fieldValue["col_a1"]);
                
                Records.Add(record);
            }
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a1", record.Файл.UnigueID.UGuid);
                
                base.BaseSave(record.UID, period, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }
        
        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }

        
        public class Record : RegisterInformationRecord
        {
            public Record()
            {
                Файл = new Довідники.Файли_Pointer();
                
            }
        
            public Довідники.Файли_Pointer Файл { get; set; }
            
        }
    }
	
    
    public class ФайлиДокументів_Objest : RegisterInformationObject
    {
		public ФайлиДокументів_Objest() : base(Config.Kernel, "tab_b23",
             new string[] { "col_a1" }) 
        {
            Файл = new Довідники.Файли_Pointer();
            
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                Файл = new Довідники.Файли_Pointer(base.FieldValue["col_a1"]);
                
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            base.FieldValue["col_a1"] = Файл.UnigueID.UGuid;
            
            BaseSave();
        }

        public ФайлиДокументів_Objest Copy()
        {
            ФайлиДокументів_Objest copy = new ФайлиДокументів_Objest();
			copy.New();
            copy.Файл = Файл;
			
			return copy;
        }

        public void Delete()
        {
			base.BaseDelete();
        }
                
        public Довідники.Файли_Pointer Файл { get; set; }
        
    }
	
    #endregion
  
}

namespace StorageAndTrade_1_0.РегістриНакопичення
{
    
    #region REGISTER "ТовариНаСкладах"
    
    public static class ТовариНаСкладах_Const
    {
        public const string TABLE = "tab_a38";
		public static readonly string[] AllowDocumentSpendTable = new string[] { "tab_a32", "tab_a36", "tab_a31", "tab_a51", "tab_a53", "tab_a83", "tab_a94", "tab_b07" };
		public static readonly string[] AllowDocumentSpendType = new string[] { "ПоступленняТоварівТаПослуг", "РеалізаціяТоварівТаПослуг", "ПереміщенняТоварів", "ПоверненняТоварівПостачальнику", "ПоверненняТоварівВідКлієнта", "ВведенняЗалишків", "ПсуванняТоварів", "ВнутрішнєСпоживанняТоварів" };
        
        public const string Номенклатура = "col_e4";
        public const string ХарактеристикаНоменклатури = "col_e5";
        public const string Склад = "col_e6";
        public const string Серія = "col_a1";
        public const string ВНаявності = "col_e7";
        public const string ДоВідвантаження = "col_e8";
    }
	
    
    public class ТовариНаСкладах_RecordsSet : RegisterAccumulationRecordsSet
    {
        public ТовариНаСкладах_RecordsSet() : base(Config.Kernel, "tab_a38",
             new string[] { "col_e4", "col_e5", "col_e6", "col_a1", "col_e7", "col_e8" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_e4"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_e5"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_e6"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a1"]);
                record.ВНаявності = (fieldValue["col_e7"] != DBNull.Value) ? (decimal)fieldValue["col_e7"] : 0;
                record.ДоВідвантаження = (fieldValue["col_e8"] != DBNull.Value) ? (decimal)fieldValue["col_e8"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_e4", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_e5", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_e6", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_a1", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_e7", record.ВНаявності);
                fieldValue.Add("col_e8", record.ДоВідвантаження);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Склад = new Довідники.Склади_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                ВНаявності = 0;
                ДоВідвантаження = 0;
                
            }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public decimal ВНаявності { get; set; }
            public decimal ДоВідвантаження { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "ТовариОрганізацій"
    
    public static class ТовариОрганізацій_Const
    {
        public const string TABLE = "tab_a39";
		public static readonly string[] AllowDocumentSpendTable = new string[] {  };
		public static readonly string[] AllowDocumentSpendType = new string[] {  };
        
        public const string Організація = "col_e9";
        public const string Номенклатура = "col_f2";
        public const string ХарактеристикаНоменклатури = "col_f3";
        public const string Кількість = "col_f1";
    }
	
    
    public class ТовариОрганізацій_RecordsSet : RegisterAccumulationRecordsSet
    {
        public ТовариОрганізацій_RecordsSet() : base(Config.Kernel, "tab_a39",
             new string[] { "col_e9", "col_f2", "col_f3", "col_f1" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Організація = new Довідники.Організації_Pointer(fieldValue["col_e9"]);
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_f2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_f3"]);
                record.Кількість = (fieldValue["col_f1"] != DBNull.Value) ? (decimal)fieldValue["col_f1"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_e9", record.Організація.UnigueID.UGuid);
                fieldValue.Add("col_f2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_f3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_f1", record.Кількість);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Організація = new Довідники.Організації_Pointer();
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Кількість = 0;
                
            }
            public Довідники.Організації_Pointer Організація { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public decimal Кількість { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "РухТоварів"
    
    public static class РухТоварів_Const
    {
        public const string TABLE = "tab_a41";
		public static readonly string[] AllowDocumentSpendTable = new string[] {  };
		public static readonly string[] AllowDocumentSpendType = new string[] {  };
        
        public const string Номенклатура = "col_g3";
        public const string ХарактеристикаНоменклатури = "col_g4";
        public const string Склад = "col_g5";
        public const string Кількість = "col_g6";
    }
	
    
    public class РухТоварів_RecordsSet : RegisterAccumulationRecordsSet
    {
        public РухТоварів_RecordsSet() : base(Config.Kernel, "tab_a41",
             new string[] { "col_g3", "col_g4", "col_g5", "col_g6" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_g3"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_g4"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_g5"]);
                record.Кількість = (fieldValue["col_g6"] != DBNull.Value) ? (decimal)fieldValue["col_g6"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_g3", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_g4", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_g5", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_g6", record.Кількість);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Склад = new Довідники.Склади_Pointer();
                Кількість = 0;
                
            }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public decimal Кількість { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "ЗамовленняКлієнтів"
    
    public static class ЗамовленняКлієнтів_Const
    {
        public const string TABLE = "tab_a55";
		public static readonly string[] AllowDocumentSpendTable = new string[] { "tab_a34", "tab_a36" };
		public static readonly string[] AllowDocumentSpendType = new string[] { "ЗамовленняКлієнта", "РеалізаціяТоварівТаПослуг" };
        
        public const string ЗамовленняКлієнта = "col_a1";
        public const string Номенклатура = "col_a2";
        public const string ХарактеристикаНоменклатури = "col_a3";
        public const string Склад = "col_a4";
        public const string Замовлено = "col_a5";
        public const string Сума = "col_a6";
    }
	
    
    public class ЗамовленняКлієнтів_RecordsSet : RegisterAccumulationRecordsSet
    {
        public ЗамовленняКлієнтів_RecordsSet() : base(Config.Kernel, "tab_a55",
             new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5", "col_a6" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer(fieldValue["col_a1"]);
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                record.Замовлено = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                record.Сума = (fieldValue["col_a6"] != DBNull.Value) ? (decimal)fieldValue["col_a6"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a1", record.ЗамовленняКлієнта.UnigueID.UGuid);
                fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_a5", record.Замовлено);
                fieldValue.Add("col_a6", record.Сума);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                ЗамовленняКлієнта = new Документи.ЗамовленняКлієнта_Pointer();
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Склад = new Довідники.Склади_Pointer();
                Замовлено = 0;
                Сума = 0;
                
            }
            public Документи.ЗамовленняКлієнта_Pointer ЗамовленняКлієнта { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public decimal Замовлено { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "РозрахункиЗКлієнтами"
    
    public static class РозрахункиЗКлієнтами_Const
    {
        public const string TABLE = "tab_a56";
		public static readonly string[] AllowDocumentSpendTable = new string[] { "tab_a36", "tab_a44", "tab_a48", "tab_a53", "tab_a81" };
		public static readonly string[] AllowDocumentSpendType = new string[] { "РеалізаціяТоварівТаПослуг", "ПрихіднийКасовийОрдер", "РозхіднийКасовийОрдер", "ПоверненняТоварівВідКлієнта", "АктВиконанихРобіт" };
        
        public const string Валюта = "col_a2";
        public const string Контрагент = "col_a5";
        public const string Сума = "col_a4";
    }
	
    
    public class РозрахункиЗКлієнтами_RecordsSet : RegisterAccumulationRecordsSet
    {
        public РозрахункиЗКлієнтами_RecordsSet() : base(Config.Kernel, "tab_a56",
             new string[] { "col_a2", "col_a5", "col_a4" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a2"]);
                record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_a5"]);
                record.Сума = (fieldValue["col_a4"] != DBNull.Value) ? (decimal)fieldValue["col_a4"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a2", record.Валюта.UnigueID.UGuid);
                fieldValue.Add("col_a5", record.Контрагент.UnigueID.UGuid);
                fieldValue.Add("col_a4", record.Сума);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Валюта = new Довідники.Валюти_Pointer();
                Контрагент = new Довідники.Контрагенти_Pointer();
                Сума = 0;
                
            }
            public Довідники.Валюти_Pointer Валюта { get; set; }
            public Довідники.Контрагенти_Pointer Контрагент { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "Закупівлі"
    
    public static class Закупівлі_Const
    {
        public const string TABLE = "tab_a57";
		public static readonly string[] AllowDocumentSpendTable = new string[] {  };
		public static readonly string[] AllowDocumentSpendType = new string[] {  };
        
        public const string Організація = "col_a5";
        public const string Склад = "col_a6";
        public const string Контрагент = "col_a7";
        public const string Договір = "col_b3";
        public const string Кількість = "col_a8";
        public const string Сума = "col_b1";
        public const string Вартість = "col_b2";
    }
	
    
    public class Закупівлі_RecordsSet : RegisterAccumulationRecordsSet
    {
        public Закупівлі_RecordsSet() : base(Config.Kernel, "tab_a57",
             new string[] { "col_a5", "col_a6", "col_a7", "col_b3", "col_a8", "col_b1", "col_b2" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Організація = new Довідники.Організації_Pointer(fieldValue["col_a5"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a6"]);
                record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_a7"]);
                record.Договір = new Довідники.ДоговориКонтрагентів_Pointer(fieldValue["col_b3"]);
                record.Кількість = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                record.Сума = (fieldValue["col_b1"] != DBNull.Value) ? (decimal)fieldValue["col_b1"] : 0;
                record.Вартість = (fieldValue["col_b2"] != DBNull.Value) ? (decimal)fieldValue["col_b2"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a5", record.Організація.UnigueID.UGuid);
                fieldValue.Add("col_a6", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_a7", record.Контрагент.UnigueID.UGuid);
                fieldValue.Add("col_b3", record.Договір.UnigueID.UGuid);
                fieldValue.Add("col_a8", record.Кількість);
                fieldValue.Add("col_b1", record.Сума);
                fieldValue.Add("col_b2", record.Вартість);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Організація = new Довідники.Організації_Pointer();
                Склад = new Довідники.Склади_Pointer();
                Контрагент = new Довідники.Контрагенти_Pointer();
                Договір = new Довідники.ДоговориКонтрагентів_Pointer();
                Кількість = 0;
                Сума = 0;
                Вартість = 0;
                
            }
            public Довідники.Організації_Pointer Організація { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public Довідники.Контрагенти_Pointer Контрагент { get; set; }
            public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
            public decimal Кількість { get; set; }
            public decimal Сума { get; set; }
            public decimal Вартість { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "ВільніЗалишки"
    
    public static class ВільніЗалишки_Const
    {
        public const string TABLE = "tab_a58";
		public static readonly string[] AllowDocumentSpendTable = new string[] { "tab_a32", "tab_a34", "tab_a36", "tab_a31", "tab_a51", "tab_a53", "tab_a83", "tab_a94", "tab_b07", "tab_b10" };
		public static readonly string[] AllowDocumentSpendType = new string[] { "ПоступленняТоварівТаПослуг", "ЗамовленняКлієнта", "РеалізаціяТоварівТаПослуг", "ПереміщенняТоварів", "ПоверненняТоварівПостачальнику", "ПоверненняТоварівВідКлієнта", "ВведенняЗалишків", "ПсуванняТоварів", "ВнутрішнєСпоживанняТоварів", "РахунокФактура" };
        
        public const string Номенклатура = "col_a5";
        public const string ХарактеристикаНоменклатури = "col_a6";
        public const string Склад = "col_a7";
        public const string ВНаявності = "col_a8";
        public const string ВРезервіЗіСкладу = "col_b1";
        public const string ВРезервіПідЗамовлення = "col_b2";
        public const string ДокументРезерву = "col_a1";
    }
	
    
    public class ВільніЗалишки_RecordsSet : RegisterAccumulationRecordsSet
    {
        public ВільніЗалишки_RecordsSet() : base(Config.Kernel, "tab_a58",
             new string[] { "col_a5", "col_a6", "col_a7", "col_a8", "col_b1", "col_b2", "col_a1" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a5"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a6"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a7"]);
                record.ВНаявності = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                record.ВРезервіЗіСкладу = (fieldValue["col_b1"] != DBNull.Value) ? (decimal)fieldValue["col_b1"] : 0;
                record.ВРезервіПідЗамовлення = (fieldValue["col_b2"] != DBNull.Value) ? (decimal)fieldValue["col_b2"] : 0;
                record.ДокументРезерву = (fieldValue["col_a1"] != DBNull.Value) ? Guid.Parse(fieldValue["col_a1"].ToString()) : Guid.Empty;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a5", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_a6", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a7", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_a8", record.ВНаявності);
                fieldValue.Add("col_b1", record.ВРезервіЗіСкладу);
                fieldValue.Add("col_b2", record.ВРезервіПідЗамовлення);
                fieldValue.Add("col_a1", record.ДокументРезерву);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Склад = new Довідники.Склади_Pointer();
                ВНаявності = 0;
                ВРезервіЗіСкладу = 0;
                ВРезервіПідЗамовлення = 0;
                ДокументРезерву = new Guid();
                
            }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public decimal ВНаявності { get; set; }
            public decimal ВРезервіЗіСкладу { get; set; }
            public decimal ВРезервіПідЗамовлення { get; set; }
            public Guid ДокументРезерву { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "ЗамовленняПостачальникам"
    
    public static class ЗамовленняПостачальникам_Const
    {
        public const string TABLE = "tab_a60";
		public static readonly string[] AllowDocumentSpendTable = new string[] { "tab_a25", "tab_a32" };
		public static readonly string[] AllowDocumentSpendType = new string[] { "ЗамовленняПостачальнику", "ПоступленняТоварівТаПослуг" };
        
        public const string ЗамовленняПостачальнику = "col_a1";
        public const string Номенклатура = "col_a2";
        public const string ХарактеристикаНоменклатури = "col_a3";
        public const string Склад = "col_a4";
        public const string Замовлено = "col_a5";
    }
	
    
    public class ЗамовленняПостачальникам_RecordsSet : RegisterAccumulationRecordsSet
    {
        public ЗамовленняПостачальникам_RecordsSet() : base(Config.Kernel, "tab_a60",
             new string[] { "col_a1", "col_a2", "col_a3", "col_a4", "col_a5" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer(fieldValue["col_a1"]);
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a3"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a4"]);
                record.Замовлено = (fieldValue["col_a5"] != DBNull.Value) ? (decimal)fieldValue["col_a5"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a1", record.ЗамовленняПостачальнику.UnigueID.UGuid);
                fieldValue.Add("col_a2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_a3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a4", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_a5", record.Замовлено);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                ЗамовленняПостачальнику = new Документи.ЗамовленняПостачальнику_Pointer();
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Склад = new Довідники.Склади_Pointer();
                Замовлено = 0;
                
            }
            public Документи.ЗамовленняПостачальнику_Pointer ЗамовленняПостачальнику { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public decimal Замовлено { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "РозрахункиЗПостачальниками"
    
    public static class РозрахункиЗПостачальниками_Const
    {
        public const string TABLE = "tab_a61";
		public static readonly string[] AllowDocumentSpendTable = new string[] { "tab_a32", "tab_a44", "tab_a48", "tab_a51" };
		public static readonly string[] AllowDocumentSpendType = new string[] { "ПоступленняТоварівТаПослуг", "ПрихіднийКасовийОрдер", "РозхіднийКасовийОрдер", "ПоверненняТоварівПостачальнику" };
        
        public const string Контрагент = "col_a6";
        public const string Валюта = "col_a7";
        public const string Сума = "col_a8";
    }
	
    
    public class РозрахункиЗПостачальниками_RecordsSet : RegisterAccumulationRecordsSet
    {
        public РозрахункиЗПостачальниками_RecordsSet() : base(Config.Kernel, "tab_a61",
             new string[] { "col_a6", "col_a7", "col_a8" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Контрагент = new Довідники.Контрагенти_Pointer(fieldValue["col_a6"]);
                record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a7"]);
                record.Сума = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a6", record.Контрагент.UnigueID.UGuid);
                fieldValue.Add("col_a7", record.Валюта.UnigueID.UGuid);
                fieldValue.Add("col_a8", record.Сума);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Контрагент = new Довідники.Контрагенти_Pointer();
                Валюта = new Довідники.Валюти_Pointer();
                Сума = 0;
                
            }
            public Довідники.Контрагенти_Pointer Контрагент { get; set; }
            public Довідники.Валюти_Pointer Валюта { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "ТовариДоПоступлення"
    
    public static class ТовариДоПоступлення_Const
    {
        public const string TABLE = "tab_a62";
		public static readonly string[] AllowDocumentSpendTable = new string[] {  };
		public static readonly string[] AllowDocumentSpendType = new string[] {  };
        
        public const string Номенклатура = "col_b2";
        public const string ХарактеристикаНоменклатури = "col_b3";
        public const string Склад = "col_b4";
        public const string ДоПоступлення = "col_b5";
    }
	
    
    public class ТовариДоПоступлення_RecordsSet : RegisterAccumulationRecordsSet
    {
        public ТовариДоПоступлення_RecordsSet() : base(Config.Kernel, "tab_a62",
             new string[] { "col_b2", "col_b3", "col_b4", "col_b5" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_b2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_b3"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_b4"]);
                record.ДоПоступлення = (fieldValue["col_b5"] != DBNull.Value) ? (decimal)fieldValue["col_b5"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_b2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_b3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_b4", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_b5", record.ДоПоступлення);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Склад = new Довідники.Склади_Pointer();
                ДоПоступлення = 0;
                
            }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public decimal ДоПоступлення { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "РухКоштів"
    
    public static class РухКоштів_Const
    {
        public const string TABLE = "tab_a78";
		public static readonly string[] AllowDocumentSpendTable = new string[] { "tab_a44", "tab_a48", "tab_a83" };
		public static readonly string[] AllowDocumentSpendType = new string[] { "ПрихіднийКасовийОрдер", "РозхіднийКасовийОрдер", "ВведенняЗалишків" };
        
        public const string Організація = "col_a1";
        public const string Каса = "col_a2";
        public const string Валюта = "col_a3";
        public const string Сума = "col_a4";
    }
	
    
    public class РухКоштів_RecordsSet : RegisterAccumulationRecordsSet
    {
        public РухКоштів_RecordsSet() : base(Config.Kernel, "tab_a78",
             new string[] { "col_a1", "col_a2", "col_a3", "col_a4" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Організація = new Довідники.Організації_Pointer(fieldValue["col_a1"]);
                record.Каса = new Довідники.Каси_Pointer(fieldValue["col_a2"]);
                record.Валюта = new Довідники.Валюти_Pointer(fieldValue["col_a3"]);
                record.Сума = (fieldValue["col_a4"] != DBNull.Value) ? (decimal)fieldValue["col_a4"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a1", record.Організація.UnigueID.UGuid);
                fieldValue.Add("col_a2", record.Каса.UnigueID.UGuid);
                fieldValue.Add("col_a3", record.Валюта.UnigueID.UGuid);
                fieldValue.Add("col_a4", record.Сума);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Організація = new Довідники.Організації_Pointer();
                Каса = new Довідники.Каси_Pointer();
                Валюта = new Довідники.Валюти_Pointer();
                Сума = 0;
                
            }
            public Довідники.Організації_Pointer Організація { get; set; }
            public Довідники.Каси_Pointer Каса { get; set; }
            public Довідники.Валюти_Pointer Валюта { get; set; }
            public decimal Сума { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "ПартіїТоварів"
    
    public static class ПартіїТоварів_Const
    {
        public const string TABLE = "tab_a79";
		public static readonly string[] AllowDocumentSpendTable = new string[] { "tab_a32", "tab_a36", "tab_a31", "tab_a51", "tab_a53", "tab_a83", "tab_a94", "tab_b07" };
		public static readonly string[] AllowDocumentSpendType = new string[] { "ПоступленняТоварівТаПослуг", "РеалізаціяТоварівТаПослуг", "ПереміщенняТоварів", "ПоверненняТоварівПостачальнику", "ПоверненняТоварівВідКлієнта", "ВведенняЗалишків", "ПсуванняТоварів", "ВнутрішнєСпоживанняТоварів" };
        
        public const string Організація = "col_a1";
        public const string ПартіяТоварівКомпозит = "col_a9";
        public const string Номенклатура = "col_a5";
        public const string ХарактеристикаНоменклатури = "col_a7";
        public const string Серія = "col_a8";
        public const string Склад = "col_a2";
        public const string Кількість = "col_a3";
        public const string Собівартість = "col_a4";
        public const string СписанаСобівартість = "col_b5";
    }
	
    
    public class ПартіїТоварів_RecordsSet : RegisterAccumulationRecordsSet
    {
        public ПартіїТоварів_RecordsSet() : base(Config.Kernel, "tab_a79",
             new string[] { "col_a1", "col_a9", "col_a5", "col_a7", "col_a8", "col_a2", "col_a3", "col_a4", "col_b5" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Організація = new Довідники.Організації_Pointer(fieldValue["col_a1"]);
                record.ПартіяТоварівКомпозит = new Довідники.ПартіяТоварівКомпозит_Pointer(fieldValue["col_a9"]);
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_a5"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_a7"]);
                record.Серія = new Довідники.СеріїНоменклатури_Pointer(fieldValue["col_a8"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a2"]);
                record.Кількість = (fieldValue["col_a3"] != DBNull.Value) ? (decimal)fieldValue["col_a3"] : 0;
                record.Собівартість = (fieldValue["col_a4"] != DBNull.Value) ? (decimal)fieldValue["col_a4"] : 0;
                record.СписанаСобівартість = (fieldValue["col_b5"] != DBNull.Value) ? (decimal)fieldValue["col_b5"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a1", record.Організація.UnigueID.UGuid);
                fieldValue.Add("col_a9", record.ПартіяТоварівКомпозит.UnigueID.UGuid);
                fieldValue.Add("col_a5", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_a7", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_a8", record.Серія.UnigueID.UGuid);
                fieldValue.Add("col_a2", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_a3", record.Кількість);
                fieldValue.Add("col_a4", record.Собівартість);
                fieldValue.Add("col_b5", record.СписанаСобівартість);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Організація = new Довідники.Організації_Pointer();
                ПартіяТоварівКомпозит = new Довідники.ПартіяТоварівКомпозит_Pointer();
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Серія = new Довідники.СеріїНоменклатури_Pointer();
                Склад = new Довідники.Склади_Pointer();
                Кількість = 0;
                Собівартість = 0;
                СписанаСобівартість = 0;
                
            }
            public Довідники.Організації_Pointer Організація { get; set; }
            public Довідники.ПартіяТоварівКомпозит_Pointer ПартіяТоварівКомпозит { get; set; }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.СеріїНоменклатури_Pointer Серія { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public decimal Кількість { get; set; }
            public decimal Собівартість { get; set; }
            public decimal СписанаСобівартість { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "ТовариДоВідвантаження"
    
    public static class ТовариДоВідвантаження_Const
    {
        public const string TABLE = "tab_a80";
		public static readonly string[] AllowDocumentSpendTable = new string[] {  };
		public static readonly string[] AllowDocumentSpendType = new string[] {  };
        
        public const string Номенклатура = "col_b2";
        public const string ХарактеристикаНоменклатури = "col_b3";
        public const string Склад = "col_b4";
        public const string ВРезерві = "col_b5";
        public const string ДоВідвантаження = "col_a2";
    }
	
    
    public class ТовариДоВідвантаження_RecordsSet : RegisterAccumulationRecordsSet
    {
        public ТовариДоВідвантаження_RecordsSet() : base(Config.Kernel, "tab_a80",
             new string[] { "col_b2", "col_b3", "col_b4", "col_b5", "col_a2" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.Номенклатура = new Довідники.Номенклатура_Pointer(fieldValue["col_b2"]);
                record.ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer(fieldValue["col_b3"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_b4"]);
                record.ВРезерві = (fieldValue["col_b5"] != DBNull.Value) ? (decimal)fieldValue["col_b5"] : 0;
                record.ДоВідвантаження = (fieldValue["col_a2"] != DBNull.Value) ? (decimal)fieldValue["col_a2"] : 0;
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_b2", record.Номенклатура.UnigueID.UGuid);
                fieldValue.Add("col_b3", record.ХарактеристикаНоменклатури.UnigueID.UGuid);
                fieldValue.Add("col_b4", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_b5", record.ВРезерві);
                fieldValue.Add("col_a2", record.ДоВідвантаження);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                Номенклатура = new Довідники.Номенклатура_Pointer();
                ХарактеристикаНоменклатури = new Довідники.ХарактеристикиНоменклатури_Pointer();
                Склад = new Довідники.Склади_Pointer();
                ВРезерві = 0;
                ДоВідвантаження = 0;
                
            }
            public Довідники.Номенклатура_Pointer Номенклатура { get; set; }
            public Довідники.ХарактеристикиНоменклатури_Pointer ХарактеристикаНоменклатури { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public decimal ВРезерві { get; set; }
            public decimal ДоВідвантаження { get; set; }
            
        }
    }
    
    #endregion
  
    #region REGISTER "ДоходиТаСобівартістьПродажів"
    
    public static class ДоходиТаСобівартістьПродажів_Const
    {
        public const string TABLE = "tab_b12";
		public static readonly string[] AllowDocumentSpendTable = new string[] {  };
		public static readonly string[] AllowDocumentSpendType = new string[] {  };
        
        public const string ДокументКомпозит = "col_a1";
        public const string ТипЗапасів = "col_a2";
        public const string ВидЗапасів = "col_a4";
        public const string Склад = "col_a5";
        public const string Договір = "col_a6";
        public const string ГосподарськаОперація = "col_a7";
        public const string АналітикаНоменклатуриКомпозит = "col_a9";
        public const string АналітикаКонтрагентівКомпозит = "col_b4";
        public const string Підрозділ = "col_b6";
        public const string Кількість = "col_a8";
        public const string СумаДоходу = "col_b2";
        public const string Собівартість = "col_b3";
        public const string ВалютаДокументу = "col_b5";
    }
	
    
    public class ДоходиТаСобівартістьПродажів_RecordsSet : RegisterAccumulationRecordsSet
    {
        public ДоходиТаСобівартістьПродажів_RecordsSet() : base(Config.Kernel, "tab_b12",
             new string[] { "col_a1", "col_a2", "col_a4", "col_a5", "col_a6", "col_a7", "col_a9", "col_b4", "col_b6", "col_a8", "col_b2", "col_b3", "col_b5" }) 
        {
            Records = new List<Record>();
        }
		
        public List<Record> Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary<string, object> fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
				record.Period = DateTime.Parse(fieldValue["period"].ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                record.ДокументКомпозит = new Довідники.ПродажіДокументКомпозит_Pointer(fieldValue["col_a1"]);
                record.ТипЗапасів = (fieldValue["col_a2"] != DBNull.Value) ? (Перелічення.ТипЗапасів)fieldValue["col_a2"] : 0;
                record.ВидЗапасів = new Довідники.ВидиЗапасів_Pointer(fieldValue["col_a4"]);
                record.Склад = new Довідники.Склади_Pointer(fieldValue["col_a5"]);
                record.Договір = new Довідники.ДоговориКонтрагентів_Pointer(fieldValue["col_a6"]);
                record.ГосподарськаОперація = (fieldValue["col_a7"] != DBNull.Value) ? (Перелічення.ГосподарськіОперації)fieldValue["col_a7"] : 0;
                record.АналітикаНоменклатуриКомпозит = new Довідники.АналітикаНоменклатуриКомпозит_Pointer(fieldValue["col_a9"]);
                record.АналітикаКонтрагентівКомпозит = new Довідники.АналітикаКонтрагентівКомпозит_Pointer(fieldValue["col_b4"]);
                record.Підрозділ = new Довідники.СтруктураПідприємства_Pointer(fieldValue["col_b6"]);
                record.Кількість = (fieldValue["col_a8"] != DBNull.Value) ? (decimal)fieldValue["col_a8"] : 0;
                record.СумаДоходу = (fieldValue["col_b2"] != DBNull.Value) ? (decimal)fieldValue["col_b2"] : 0;
                record.Собівартість = (fieldValue["col_b3"] != DBNull.Value) ? (decimal)fieldValue["col_b3"] : 0;
                record.ВалютаДокументу = new Довідники.Валюти_Pointer(fieldValue["col_b5"]);
                
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary<string, object> fieldValue = new Dictionary<string, object>();
                fieldValue.Add("col_a1", record.ДокументКомпозит.UnigueID.UGuid);
                fieldValue.Add("col_a2", (int)record.ТипЗапасів);
                fieldValue.Add("col_a4", record.ВидЗапасів.UnigueID.UGuid);
                fieldValue.Add("col_a5", record.Склад.UnigueID.UGuid);
                fieldValue.Add("col_a6", record.Договір.UnigueID.UGuid);
                fieldValue.Add("col_a7", (int)record.ГосподарськаОперація);
                fieldValue.Add("col_a9", record.АналітикаНоменклатуриКомпозит.UnigueID.UGuid);
                fieldValue.Add("col_b4", record.АналітикаКонтрагентівКомпозит.UnigueID.UGuid);
                fieldValue.Add("col_b6", record.Підрозділ.UnigueID.UGuid);
                fieldValue.Add("col_a8", record.Кількість);
                fieldValue.Add("col_b2", record.СумаДоходу);
                fieldValue.Add("col_b3", record.Собівартість);
                fieldValue.Add("col_b5", record.ВалютаДокументу.UnigueID.UGuid);
                
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            base.BaseCommitTransaction();
        }
        
        
        public class Record : RegisterAccumulationRecord
        {
            public Record()
            {
                ДокументКомпозит = new Довідники.ПродажіДокументКомпозит_Pointer();
                ТипЗапасів = 0;
                ВидЗапасів = new Довідники.ВидиЗапасів_Pointer();
                Склад = new Довідники.Склади_Pointer();
                Договір = new Довідники.ДоговориКонтрагентів_Pointer();
                ГосподарськаОперація = 0;
                АналітикаНоменклатуриКомпозит = new Довідники.АналітикаНоменклатуриКомпозит_Pointer();
                АналітикаКонтрагентівКомпозит = new Довідники.АналітикаКонтрагентівКомпозит_Pointer();
                Підрозділ = new Довідники.СтруктураПідприємства_Pointer();
                Кількість = 0;
                СумаДоходу = 0;
                Собівартість = 0;
                ВалютаДокументу = new Довідники.Валюти_Pointer();
                
            }
            public Довідники.ПродажіДокументКомпозит_Pointer ДокументКомпозит { get; set; }
            public Перелічення.ТипЗапасів ТипЗапасів { get; set; }
            public Довідники.ВидиЗапасів_Pointer ВидЗапасів { get; set; }
            public Довідники.Склади_Pointer Склад { get; set; }
            public Довідники.ДоговориКонтрагентів_Pointer Договір { get; set; }
            public Перелічення.ГосподарськіОперації ГосподарськаОперація { get; set; }
            public Довідники.АналітикаНоменклатуриКомпозит_Pointer АналітикаНоменклатуриКомпозит { get; set; }
            public Довідники.АналітикаКонтрагентівКомпозит_Pointer АналітикаКонтрагентівКомпозит { get; set; }
            public Довідники.СтруктураПідприємства_Pointer Підрозділ { get; set; }
            public decimal Кількість { get; set; }
            public decimal СумаДоходу { get; set; }
            public decimal Собівартість { get; set; }
            public Довідники.Валюти_Pointer ВалютаДокументу { get; set; }
            
        }
    }
    
    #endregion
  
}
  