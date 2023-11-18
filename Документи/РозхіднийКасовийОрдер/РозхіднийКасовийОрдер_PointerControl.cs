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
    class РозхіднийКасовийОрдер_PointerControl : PointerControl
    {
        event EventHandler<РозхіднийКасовийОрдер_Pointer>? PointerChanged;

        public РозхіднийКасовийОрдер_PointerControl()
        {
            PointerChanged += OnPointerChanged;

            pointer = new РозхіднийКасовийОрдер_Pointer();
            WidthPresentation = 300;
            Caption = $"{РозхіднийКасовийОрдер_Const.FULLNAME}:";
        }

        РозхіднийКасовийОрдер_Pointer pointer;
        public РозхіднийКасовийОрдер_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;
                PointerChanged?.Invoke(this, pointer);
            }
        }

        protected async void OnPointerChanged(object? sender, РозхіднийКасовийОрдер_Pointer pointer)
        {
            Presentation = pointer != null ? await pointer.GetPresentation() : "";
        }

        //Відбір по періоду в журналі
        public bool UseWherePeriod { get; set; } = false;

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new РозхіднийКасовийОрдер_Pointer(selectPointer);
                }
            };

            Program.GeneralForm?.CreateNotebookPage($"Вибір - {РозхіднийКасовийОрдер_Const.FULLNAME}", () => { return page; }, true);

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РозхіднийКасовийОрдер_Pointer();
        }
    }
}