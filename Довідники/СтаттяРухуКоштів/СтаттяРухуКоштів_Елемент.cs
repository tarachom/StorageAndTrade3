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

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class СтаттяРухуКоштів_Елемент : VBox
    {
        public СтаттяРухуКоштів? PageList { get; set; }
        public System.Action<СтаттяРухуКоштів_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public СтаттяРухуКоштів_Objest СтаттяРухуКоштів_Objest { get; set; } = new СтаттяРухуКоштів_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry КореспондуючийРахунок = new Entry() { WidthRequest = 200 };
        ComboBoxText ВидРухуКоштів = new ComboBoxText();
        TextView Опис = new TextView();

        #endregion

        public СтаттяРухуКоштів_Елемент() : base()
        {
            HBox hBox = new HBox();

            Button bSaveAndClose = new Button("Зберегти та закрити");
            bSaveAndClose.Clicked += (object? sender, EventArgs args) => { Save(true); };
            hBox.PackStart(bSaveAndClose, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += (object? sender, EventArgs args) => { Save(); };
            hBox.PackStart(bSave, false, false, 10);

            PackStart(hBox, false, false, 10);

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 500 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();

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

            foreach (ConfigurationEnumField field in Config.Kernel!.Conf.Enums["ВидиРухуКоштів"].Fields.Values)
                ВидРухуКоштів.Append(field.Name, field.Desc);

            hBoxVidRuhu.PackStart(new Label("Вид руху коштів:"), false, false, 5);
            hBoxVidRuhu.PackStart(ВидРухуКоштів, false, false, 5);

            hPaned.Pack1(vBox, false, false);

            //Опис
            HBox hBoxOpys = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOpys, false, false, 5);

            hBoxOpys.PackStart(new Label("Опис:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextViewOpys = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 200 };
            scrollTextViewOpys.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextViewOpys.Add(Опис);

            hBoxOpys.PackStart(scrollTextViewOpys, false, false, 5);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();



            hPaned.Pack2(vBox, false, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
            {
                СтаттяРухуКоштів_Objest.New();
                СтаттяРухуКоштів_Objest.ВидРухуКоштів = Перелічення.ВидиРухуКоштів.ОплатаОборотнихАктивів;
            }

            Код.Text = СтаттяРухуКоштів_Objest.Код;
            Назва.Text = СтаттяРухуКоштів_Objest.Назва;
            КореспондуючийРахунок.Text = СтаттяРухуКоштів_Objest.КореспондуючийРахунок;
            ВидРухуКоштів.ActiveId = СтаттяРухуКоштів_Objest.ВидРухуКоштів.ToString();
            Опис.Buffer.Text = СтаттяРухуКоштів_Objest.Опис;
        }

        void GetValue()
        {
            СтаттяРухуКоштів_Objest.Код = Код.Text;
            СтаттяРухуКоштів_Objest.Назва = Назва.Text;
            СтаттяРухуКоштів_Objest.КореспондуючийРахунок = КореспондуючийРахунок.Text;
            СтаттяРухуКоштів_Objest.ВидРухуКоштів = Enum.Parse<Перелічення.ВидиРухуКоштів>(ВидРухуКоштів.ActiveId);
            СтаттяРухуКоштів_Objest.Опис = Опис.Buffer.Text;
        }

        #endregion

        void Save(bool closePage = false)
        {
            GetValue();

            bool isSave = false;

            try
            {
                isSave = СтаттяРухуКоштів_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис",
                    СтаттяРухуКоштів_Objest.UnigueID.UGuid, "Довідники", СтаттяРухуКоштів_Objest.Назва, ex.Message);

                ФункціїДляПовідомлень.ВідкритиТермінал();
            }

            if (!isSave)
            {
                Message.Info(Program.GeneralForm, "Не вдалось записати");
                return;
            }

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Стаття руху коштів: {СтаттяРухуКоштів_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(СтаттяРухуКоштів_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.SelectPointerItem = СтаттяРухуКоштів_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}