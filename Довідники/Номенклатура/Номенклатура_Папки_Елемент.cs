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

namespace StorageAndTrade
{
    class Номенклатура_Папки_Елемент : ДовідникЕлемент
    {
        public Номенклатура_Папки_Objest Номенклатура_Папки_Objest { get; set; } = new Номенклатура_Папки_Objest();
        public Номенклатура_Папки_Pointer РодичДляНового { get; set; } = new Номенклатура_Папки_Pointer();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Номенклатура_Папки_PointerControl Родич = new Номенклатура_Папки_PointerControl() { Caption = "Родич:" };

        public Номенклатура_Папки_Елемент() : base() 
        {
            Номенклатура_Папки_Objest.UnigueIDChanged += UnigueIDChanged;
            Номенклатура_Папки_Objest.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //Родич
            CreateField(vBox, null, Родич);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Номенклатура_Папки_Objest.Родич = РодичДляНового;
            else
                Родич.OpenFolder = Номенклатура_Папки_Objest.UnigueID;

            Код.Text = Номенклатура_Папки_Objest.Код;
            Назва.Text = Номенклатура_Папки_Objest.Назва;
            Родич.Pointer = Номенклатура_Папки_Objest.Родич;
        }

        protected override void GetValue()
        {
            Номенклатура_Папки_Objest.Код = Код.Text;
            Номенклатура_Папки_Objest.Назва = Назва.Text;
            Номенклатура_Папки_Objest.Родич = Родич.Pointer;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await Номенклатура_Папки_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Номенклатура_Папки_Objest.GetBasis(), Caption, ex);
            }
        }
    }
}