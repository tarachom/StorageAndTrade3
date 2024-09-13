/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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
using InterfaceGtk;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class Номенклатура_Елемент : ДовідникЕлемент
    {
        public Номенклатура_Objest Елемент { get; set; } = new Номенклатура_Objest();
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

        Номенклатура_ТабличнаЧастина_Файли Файли = new Номенклатура_ТабличнаЧастина_Файли() { HeightRequest = 300 };

        //Попередній перегляд картинки
        ScrolledWindow scrollImageView = new ScrolledWindow() { ShadowType = ShadowType.In };

        #endregion

        public Номенклатура_Елемент() 
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            ОсновнаКартинкаФайл.AfterSelectFunc = async () =>
            {
                foreach (Widget item in scrollImageView.Children)
                    scrollImageView.Remove(item);

                if (!ОсновнаКартинкаФайл.Pointer.IsEmpty())
                {
                    Файли_Objest? Файл = await ОсновнаКартинкаФайл.Pointer.GetDirectoryObject();
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

        protected override void CreatePack1(Box vBox)
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

        protected override void CreatePack2(Box vBox)
        {
            //Картинка
            {
                Box hBox = new Box(Orientation.Horizontal, 0) { Halign = Halign };
                vBox.PackStart(hBox, true, true, 5);

                scrollImageView.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                hBox.PackStart(scrollImageView, true, true, 5);
            }
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
            {
                Елемент.Папка = РодичДляНового;
                Елемент.ТипНоменклатури = ТипиНоменклатури.Товар;
                Елемент.ОдиницяВиміру = ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const;
                Елемент.ВидНоменклатури = ЗначенняЗаЗамовчуванням.ОсновнийВидНоменклатури_Const;
            }

            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Артикул.Text = Елемент.Артикул;
            ТипНоменклатури.ActiveId = Елемент.ТипНоменклатури.ToString();
            ВидНоменклатури.Pointer = Елемент.ВидНоменклатури;
            Родич.Pointer = Елемент.Папка;
            НазваПовна.Buffer.Text = Елемент.НазваПовна;
            Опис.Buffer.Text = Елемент.Опис;
            Виробник.Pointer = Елемент.Виробник;
            ОдиницяВиміру.Pointer = Елемент.ОдиницяВиміру;
            ОсновнаКартинкаФайл.Pointer = Елемент.ОсновнаКартинкаФайл;

            if (ТипНоменклатури.Active == -1)
                ТипНоменклатури.ActiveId = ТипиНоменклатури.Товар.ToString();

            Файли.ЕлементВласник = Елемент;
            await Файли.LoadRecords();

            ОсновнаКартинкаФайл.AfterSelectFunc?.Invoke();
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Артикул = Артикул.Text;
            Елемент.ТипНоменклатури = Enum.Parse<ТипиНоменклатури>(ТипНоменклатури.ActiveId);
            Елемент.ВидНоменклатури = ВидНоменклатури.Pointer;
            Елемент.Папка = Родич.Pointer;
            Елемент.НазваПовна = НазваПовна.Buffer.Text;
            Елемент.Опис = Опис.Buffer.Text;
            Елемент.Виробник = Виробник.Pointer;
            Елемент.ОдиницяВиміру = ОдиницяВиміру.Pointer;
            Елемент.ОсновнаКартинкаФайл = ОсновнаКартинкаФайл.Pointer;
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                if (await Елемент.Save())
                    await Файли.SaveRecords();
                return true;
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
                return false;
            }
        }
    }
}