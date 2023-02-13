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

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ТипорозміриКомірок_PointerControl : PointerControl
    {
        public ТипорозміриКомірок_PointerControl()
        {
            pointer = new ТипорозміриКомірок_Pointer();
            WidthPresentation = 300;
            Caption = "Типорозмір:";
        }

        ТипорозміриКомірок_Pointer pointer;
        public ТипорозміриКомірок_Pointer Pointer
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

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            ТипорозміриКомірок page = new ТипорозміриКомірок();

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ТипорозміриКомірок_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Типорозміри комірок", () => { return page; }, true);

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ТипорозміриКомірок_Pointer();
        }
    }
}