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

using AccountingSoftware;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РозміщенняНоменклатуриПоКоміркам_PointerControl : PointerControl
    {
        public РозміщенняНоменклатуриПоКоміркам_PointerControl()
        {
            pointer = new РозміщенняНоменклатуриПоКоміркам_Pointer();
            WidthPresentation = 300;
            Caption = $"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}";
        }

        РозміщенняНоменклатуриПоКоміркам_Pointer pointer;
        public РозміщенняНоменклатуриПоКоміркам_Pointer Pointer
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
            РозміщенняНоменклатуриПоКоміркам page = new РозміщенняНоменклатуриПоКоміркам
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new РозміщенняНоменклатуриПоКоміркам_Pointer(selectPointer);
                }
            };

            Program.GeneralForm?.CreateNotebookPage($"Вибір - {РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}", () => { return page; }, true);

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РозміщенняНоменклатуриПоКоміркам_Pointer();
        }
    }
}