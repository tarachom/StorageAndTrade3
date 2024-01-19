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

using Gtk;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class СтаттяРухуКоштів_Елемент : ДовідникЕлемент
    {
        public СтаттяРухуКоштів_Objest СтаттяРухуКоштів_Objest { get; set; } = new СтаттяРухуКоштів_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry КореспондуючийРахунок = new Entry() { WidthRequest = 200 };
        ComboBoxText ВидРухуКоштів = new ComboBoxText();
        TextView Опис = new TextView();

        #endregion

        public СтаттяРухуКоштів_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //КореспондуючийРахунок
            CreateField(vBox, "Кореспондентський рахунок:", КореспондуючийРахунок);

            //ВидРухуКоштів
            foreach (var field in ПсевдонімиПерелічення.ВидиРухуКоштів_List())
                ВидРухуКоштів.Append(field.Value.ToString(), field.Name);

            CreateField(vBox, "Вид руху коштів:", ВидРухуКоштів);

            //Опис
            CreateFieldView(vBox, "Опис:", Опис, 500, 200);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                await СтаттяРухуКоштів_Objest.New();
                СтаттяРухуКоштів_Objest.ВидРухуКоштів = ВидиРухуКоштів.ОплатаОборотнихАктивів;
            }

            Код.Text = СтаттяРухуКоштів_Objest.Код;
            Назва.Text = СтаттяРухуКоштів_Objest.Назва;
            КореспондуючийРахунок.Text = СтаттяРухуКоштів_Objest.КореспондуючийРахунок;
            ВидРухуКоштів.ActiveId = СтаттяРухуКоштів_Objest.ВидРухуКоштів.ToString();
            Опис.Buffer.Text = СтаттяРухуКоштів_Objest.Опис;
        }

        protected override void GetValue()
        {
            СтаттяРухуКоштів_Objest.Код = Код.Text;
            СтаттяРухуКоштів_Objest.Назва = Назва.Text;
            СтаттяРухуКоштів_Objest.КореспондуючийРахунок = КореспондуючийРахунок.Text;
            СтаттяРухуКоштів_Objest.ВидРухуКоштів = Enum.Parse<ВидиРухуКоштів>(ВидРухуКоштів.ActiveId);
            СтаттяРухуКоштів_Objest.Опис = Опис.Buffer.Text;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await СтаттяРухуКоштів_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = СтаттяРухуКоштів_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}