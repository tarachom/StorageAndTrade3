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

        protected override void CreatePack1()
        {
            VBox vBox = new VBox();
            HPanedTop.Pack1(vBox, false, false);

            //Код
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //КореспондуючийРахунок
            HBox hBoxKorRahunok = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKorRahunok, false, false, 5);

            hBoxKorRahunok.PackStart(new Label("Кореспондентський рахунок:"), false, false, 5);
            hBoxKorRahunok.PackStart(КореспондуючийРахунок, false, false, 5);

            //ВидРухуКоштів
            HBox hBoxVidRuhu = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVidRuhu, false, false, 5);

            foreach (var field in ПсевдонімиПерелічення.ВидиРухуКоштів_List())
                ВидРухуКоштів.Append(field.Value.ToString(), field.Name);

            hBoxVidRuhu.PackStart(new Label("Вид руху коштів:"), false, false, 5);
            hBoxVidRuhu.PackStart(ВидРухуКоштів, false, false, 5);

            //Опис
            HBox hBoxOpys = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOpys, false, false, 5);

            hBoxOpys.PackStart(new Label("Опис:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextViewOpys = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 200 };
            scrollTextViewOpys.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextViewOpys.Add(Опис);

            hBoxOpys.PackStart(scrollTextViewOpys, false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                СтаттяРухуКоштів_Objest.New();
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
            UnigueID = СтаттяРухуКоштів_Objest.UnigueID;
            Caption = Назва.Text;

            СтаттяРухуКоштів_Objest.Код = Код.Text;
            СтаттяРухуКоштів_Objest.Назва = Назва.Text;
            СтаттяРухуКоштів_Objest.КореспондуючийРахунок = КореспондуючийРахунок.Text;
            СтаттяРухуКоштів_Objest.ВидРухуКоштів = Enum.Parse<ВидиРухуКоштів>(ВидРухуКоштів.ActiveId);
            СтаттяРухуКоштів_Objest.Опис = Опис.Buffer.Text;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                СтаттяРухуКоштів_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}