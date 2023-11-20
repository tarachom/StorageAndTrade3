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
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class КурсиВалют_Елемент : РегістриЕлемент
    {
        public КурсиВалют_Objest КурсиВалют_Objest { get; set; } = new КурсиВалют_Objest();

        public Валюти_Pointer ВалютаДляНового { get; set; } = new Валюти_Pointer();

        DateTimeControl ДатаКурсу = new DateTimeControl();
        Валюти_PointerControl Валюта = new Валюти_PointerControl();
        NumericControl Курс = new NumericControl();
        IntegerControl Кратність = new IntegerControl();

        public КурсиВалют_Елемент() : base() { }

        protected override void CreatePack1(VBox vBox)
        {
            //ДатаКурсу
            CreateField(vBox, "Дата:", ДатаКурсу);

            //Валюта
            CreateField(vBox, null, Валюта);

            //Курс
            CreateField(vBox, "Курс:", Курс);

            //Кратність
            CreateField(vBox, "Кратність:", Кратність);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                КурсиВалют_Objest.New();

                КурсиВалют_Objest.Валюта = ВалютаДляНового;
                КурсиВалют_Objest.Кратність = 1;
            }

            ДатаКурсу.Value = КурсиВалют_Objest.Period;
            Валюта.Pointer = КурсиВалют_Objest.Валюта;
            Курс.Value = КурсиВалют_Objest.Курс;
            Кратність.Value = КурсиВалют_Objest.Кратність;
        }

        protected override void GetValue()
        {
            UnigueID = КурсиВалют_Objest.UnigueID;
            Caption = ДатаКурсу.Value.ToString();

            КурсиВалют_Objest.Period = ДатаКурсу.Value;
            КурсиВалют_Objest.Валюта = Валюта.Pointer;
            КурсиВалют_Objest.Курс = Курс.Value;
            КурсиВалют_Objest.Кратність = Кратність.Value;
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await КурсиВалют_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = КурсиВалют_Objest.UnigueID;
        }
    }
}