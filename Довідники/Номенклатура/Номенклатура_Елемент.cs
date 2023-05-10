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
        Номенклатура_Папки_PointerControl Родич = new Номенклатура_Папки_PointerControl() { Caption = "Родич:", WidthPresentation = 420 };
        ВидиНоменклатури_PointerControl ВидНоменклатури = new ВидиНоменклатури_PointerControl() { Caption = "Вид:", WidthPresentation = 300 };
        ПакуванняОдиниціВиміру_PointerControl ОдиницяВиміру = new ПакуванняОдиниціВиміру_PointerControl() { Caption = "Пакування:", WidthPresentation = 300 };
        Файли_PointerControl ОсновнаКартинкаФайл = new Файли_PointerControl() { Caption = "Основна картинка:", WidthPresentation = 300 };

        Номенклатура_ТабличнаЧастина_Файли Файли = new Номенклатура_ТабличнаЧастина_Файли();

        //Попередній перегляд картинки
        ScrolledWindow scrollImageView = new ScrolledWindow() { ShadowType = ShadowType.In };

        #endregion

        public Номенклатура_Елемент() : base()
        {
            ОсновнаКартинкаФайл.AfterSelectFunc = () =>
            {
                foreach (Widget item in scrollImageView.Children)
                    scrollImageView.Remove(item);

                if (!ОсновнаКартинкаФайл.Pointer.IsEmpty())
                {
                    Файли_Objest? Файл = ОсновнаКартинкаФайл.Pointer.GetDirectoryObject();
                    if (Файл != null)
                    {
                        try
                        {
                            scrollImageView.Add(new Image(new Gdk.Pixbuf(Файл.БінарніДані)) { Halign = Align.Start, Valign = Align.Start });
                        }
                        catch { }
                    }
                }

                scrollImageView.ShowAll();
            };
        }

        protected override void CreatePack1(VBox vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //НазваПовна
            CreateFieldView(vBox, "Повна назва:", НазваПовна, 500, 60);

            //Родич
            CreateField(vBox, null, Родич);

            //Артикул
            CreateField(vBox, "Артикул:", Артикул);

            //Тип
            foreach (var field in ПсевдонімиПерелічення.ТипиНоменклатури_List())
                ТипНоменклатури.Append(field.Value.ToString(), field.Name);

            CreateField(vBox, "Тип:", ТипНоменклатури);

            //ОдиницяВиміру
            CreateField(vBox, null, ОдиницяВиміру);

            //ВидНоменклатури
            CreateField(vBox, null, ВидНоменклатури);

            //Виробник
            CreateField(vBox, null, Виробник);

            //Опис
            CreateFieldView(vBox, "Опис:", Опис, 500, 200);

            //ОсновнаКартинкаФайл
            CreateField(vBox, null, ОсновнаКартинкаФайл);

            //Файли
            CreateTablePart(vBox, "Файли:", Файли);
        }

        protected override void CreatePack2(VBox vBox)
        {
            //Картинка
            {
                HBox hBox = new HBox() { Halign = Halign };
                vBox.PackStart(hBox, true, true, 5);

                scrollImageView.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                hBox.PackStart(scrollImageView, true, true, 5);
            }
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

            if (ОсновнаКартинкаФайл.AfterSelectFunc != null)
                ОсновнаКартинкаФайл.AfterSelectFunc.Invoke();
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