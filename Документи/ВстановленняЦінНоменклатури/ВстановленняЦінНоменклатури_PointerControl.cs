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

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури_PointerControl : PointerControl
    {
        public ВстановленняЦінНоменклатури_PointerControl()
        {
            pointer = new ВстановленняЦінНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = "Встановлення цін номенклатури:";
        }

        ВстановленняЦінНоменклатури_Pointer pointer;
        public ВстановленняЦінНоменклатури_Pointer Pointer
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

        //Відбір по періоду в журналі
        public bool UseWherePeriod { get; set; } = false;

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ВстановленняЦінНоменклатури_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Встановлення цін номенклатури", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВстановленняЦінНоменклатури_Pointer();
        }
    }
}