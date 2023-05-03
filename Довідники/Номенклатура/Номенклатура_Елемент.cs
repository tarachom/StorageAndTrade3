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

using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class Номенклатура_Елемент : ДовідникЕлемент
    {
        public Номенклатура_Objest Номенклатура_Objest { get; set; } = new Номенклатура_Objest();
        public Номенклатура_Папки_Pointer РодичДляНового { get; set; } = new Номенклатура_Папки_Pointer();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 100 };
        public Entry Назва { get; private set; } = new Entry() { WidthRequest = 500 };
        TextView НазваПовна = new TextView();
        TextView Опис = new TextView();
        ComboBoxText ТипНоменклатури = new ComboBoxText();
        Entry Артикул = new Entry() { WidthRequest = 500 };
        Виробники_PointerControl Виробник = new Виробники_PointerControl() { WidthPresentation = 300 };
        Номенклатура_Папки_PointerControl Родич = new Номенклатура_Папки_PointerControl() { Caption = "Папка:", WidthPresentation = 420 };
        ВидиНоменклатури_PointerControl ВидНоменклатури = new ВидиНоменклатури_PointerControl() { Caption = "Вид:", WidthPresentation = 300 };
        ПакуванняОдиниціВиміру_PointerControl ОдиницяВиміру = new ПакуванняОдиниціВиміру_PointerControl() { WidthPresentation = 300 };
        Файли_PointerControl ОсновнаКартинкаФайл = new Файли_PointerControl() { Caption = "Основна картинка:", WidthPresentation = 300 };
        Номенклатура_ТабличнаЧастина_Файли Файли = new Номенклатура_ТабличнаЧастина_Файли();

        #endregion

        public Номенклатура_Елемент() : base() { }

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

            //НазваПовна
            HBox hBoxDesc = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDesc, false, false, 5);

            hBoxDesc.PackStart(new Label("Повна назва:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextView = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 60 };
            scrollTextView.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextView.Add(НазваПовна);

            hBoxDesc.PackStart(scrollTextView, false, false, 5);

            //Родич
            HBox hBoxParent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxParent, false, false, 5);

            hBoxParent.PackStart(Родич, false, false, 5);

            //Артикул
            HBox hBoxArtykul = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxArtykul, false, false, 5);

            hBoxArtykul.PackStart(new Label("Артикул:"), false, false, 5);
            hBoxArtykul.PackStart(Артикул, false, false, 5);

            //Тип
            HBox hBoxType = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxType, false, false, 5);

            foreach (var field in ПсевдонімиПерелічення.ТипиНоменклатури_List())
                ТипНоменклатури.Append(field.Value.ToString(), field.Name);

            hBoxType.PackStart(new Label("Тип:"), false, false, 5);
            hBoxType.PackStart(ТипНоменклатури, false, false, 5);

            //ОдиницяВиміру
            HBox hBoxOdynyca = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOdynyca, false, false, 5);

            hBoxOdynyca.PackStart(ОдиницяВиміру, false, false, 5);

            //ВидНоменклатури
            HBox hBoxVidNumeklatury = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVidNumeklatury, false, false, 5);

            hBoxVidNumeklatury.PackStart(ВидНоменклатури, false, false, 5);

            //Виробник
            HBox hBoxVirobnyk = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVirobnyk, false, false, 5);

            hBoxVirobnyk.PackStart(Виробник, false, false, 5);

            //Опис
            HBox hBoxOpys = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOpys, false, false, 5);

            hBoxOpys.PackStart(new Label("Опис:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextViewOpys = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 500, HeightRequest = 200 };
            scrollTextViewOpys.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextViewOpys.Add(Опис);

            hBoxOpys.PackStart(scrollTextViewOpys, false, false, 5);

            //ОсновнаКартинкаФайл
            HBox hBoxDefPicture = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxDefPicture, false, false, 5);

            hBoxDefPicture.PackStart(ОсновнаКартинкаФайл, false, false, 5);

            //Файли Заголовок
            HBox hBoxFilesInfo = new HBox();

            hBoxFilesInfo.PackStart(new Label("Файли:"), false, false, 5);
            vBox.PackStart(hBoxFilesInfo, false, false, 5);

            //Файли
            HBox hBoxFiles = new HBox();
            vBox.PackStart(hBoxFiles, false, false, 5);

            hBoxFiles.PackStart(Файли, true, true, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                Номенклатура_Objest.New();
                Номенклатура_Objest.Папка = РодичДляНового;
                Номенклатура_Objest.ТипНоменклатури = ТипиНоменклатури.Товар;
                Номенклатура_Objest.ОдиницяВиміру = ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const;
                Номенклатура_Objest.ВидНоменклатури = ЗначенняЗаЗамовчуванням.ОсновнийВидНоменклатури_Const;
            }

            Код.Text = Номенклатура_Objest.Код;
            Назва.Text = Номенклатура_Objest.Назва;
            Артикул.Text = Номенклатура_Objest.Артикул;
            ТипНоменклатури.ActiveId = Номенклатура_Objest.ТипНоменклатури.ToString();
            ВидНоменклатури.Pointer = Номенклатура_Objest.ВидНоменклатури;
            Родич.Pointer = Номенклатура_Objest.Папка;
            НазваПовна.Buffer.Text = Номенклатура_Objest.НазваПовна;
            Опис.Buffer.Text = Номенклатура_Objest.Опис;
            Виробник.Pointer = Номенклатура_Objest.Виробник;
            ОдиницяВиміру.Pointer = Номенклатура_Objest.ОдиницяВиміру;
            ОсновнаКартинкаФайл.Pointer = Номенклатура_Objest.ОсновнаКартинкаФайл;

            if (ТипНоменклатури.Active == -1)
                ТипНоменклатури.ActiveId = ТипиНоменклатури.Товар.ToString();

            Файли.Номенклатура_Objest = Номенклатура_Objest;
            Файли.LoadRecords();
        }

        protected override void GetValue()
        {
            UnigueID = Номенклатура_Objest.UnigueID;
            Caption = Назва.Text;

            Номенклатура_Objest.Код = Код.Text;
            Номенклатура_Objest.Назва = Назва.Text;
            Номенклатура_Objest.Артикул = Артикул.Text;
            Номенклатура_Objest.ТипНоменклатури = Enum.Parse<ТипиНоменклатури>(ТипНоменклатури.ActiveId);
            Номенклатура_Objest.ВидНоменклатури = ВидНоменклатури.Pointer;
            Номенклатура_Objest.Папка = Родич.Pointer;
            Номенклатура_Objest.НазваПовна = НазваПовна.Buffer.Text;
            Номенклатура_Objest.Опис = Опис.Buffer.Text;
            Номенклатура_Objest.Виробник = Виробник.Pointer;
            Номенклатура_Objest.ОдиницяВиміру = ОдиницяВиміру.Pointer;
            Номенклатура_Objest.ОсновнаКартинкаФайл = ОсновнаКартинкаФайл.Pointer;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                Номенклатура_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            Файли.SaveRecords();
        }
    }
}