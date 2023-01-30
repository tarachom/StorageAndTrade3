#region Info

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

#endregion

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ХарактеристикиНоменклатури_PointerControl : PointerControl
    {
        public ХарактеристикиНоменклатури_PointerControl()
        {
            pointer = new ХарактеристикиНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = "Характеристика:";
        }

        ХарактеристикиНоменклатури_Pointer pointer;
        public ХарактеристикиНоменклатури_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;

                if (pointer != null)
                    Presentation = pointer.GetPresentation();
                else
                    Presentation = "";
            }
        }

        public Номенклатура_Pointer НоменклатураВласник { get; set; } = new Номенклатура_Pointer();

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            if (BeforeClickOpenFunc != null)
                BeforeClickOpenFunc.Invoke();

            ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури(true);

            page.DirectoryPointerItem = Pointer;
            page.НоменклатураВласник.Pointer = НоменклатураВласник;
            page.CallBack_OnSelectPointer = (ХарактеристикиНоменклатури_Pointer selectPointer) =>
            {
                Pointer = selectPointer;

                if (AfterSelectFunc != null)
                    AfterSelectFunc.Invoke();
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Характеристики", () => { return page; }, true);

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ХарактеристикиНоменклатури_Pointer();

            if (AfterSelectFunc != null)
                AfterSelectFunc.Invoke();
        }
    }
}