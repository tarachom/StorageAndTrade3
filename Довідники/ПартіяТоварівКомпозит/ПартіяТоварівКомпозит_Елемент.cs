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
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_Елемент : ДовідникЕлемент
    {
        public ПартіяТоварівКомпозит_Objest Елемент { get; set; } = new ПартіяТоварівКомпозит_Objest();

        Entry Назва = new Entry() { WidthRequest = 500 };
        ComboBoxText ТипДокументу = new ComboBoxText();
        ПоступленняТоварівТаПослуг_PointerControl ПоступленняТоварівТаПослуг = new ПоступленняТоварівТаПослуг_PointerControl();
        ВведенняЗалишків_PointerControl ВведенняЗалишків = new ВведенняЗалишків_PointerControl();

        public ПартіяТоварівКомпозит_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            foreach (var field in ПсевдонімиПерелічення.ТипДокументуПартіяТоварівКомпозит_List())
                ТипДокументу.Append(field.Value.ToString(), field.Name);

            ТипДокументу.ActiveId = ТипДокументуПартіяТоварівКомпозит.ПоступленняТоварівТаПослуг.ToString();
        }

        protected override void CreatePack1(Box vBox)
        {
            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Тип
            CreateField(vBox, "Тип документу:", ТипДокументу);

            //ПоступленняТоварівТаПослуг
            CreateField(vBox, null, ПоступленняТоварівТаПослуг);

            //ВведенняЗалишків
            CreateField(vBox, null, ВведенняЗалишків);
        }

        protected override void CreatePack2(Box vBox)
        {
            Box hBoxInfo = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
            vBox.PackStart(hBoxInfo, false, false, 5);

            hBoxInfo.PackStart(new Label("Редагувати дозволено тільки назву"), false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Назва.Text = Елемент.Назва;
            ТипДокументу.ActiveId = Елемент.ТипДокументу.ToString();
            ПоступленняТоварівТаПослуг.Pointer = Елемент.ПоступленняТоварівТаПослуг;
            ВведенняЗалишків.Pointer = Елемент.ВведенняЗалишків;
        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;

            /*
            Редагування заборонено, тільки назва

            Елемент.ТипДокументу = Enum.Parse<Перелічення.ТипДокументуПартіяТоварівКомпозит>(ТипДокументу.ActiveId);
            Елемент.ПоступленняТоварівТаПослуг = ПоступленняТоварівТаПослуг.Pointer;
            Елемент.ВведенняЗалишків = ВведенняЗалишків.Pointer;
            
            */
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await Елемент.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }            
        }
    }
}