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

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури_PointerControl : PointerControl
    {
        event EventHandler<ВстановленняЦінНоменклатури_Pointer> PointerChanged;

        public ВстановленняЦінНоменклатури_PointerControl()
        {
            pointer = new ВстановленняЦінНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = $"{ВстановленняЦінНоменклатури_Const.FULLNAME}:";
            PointerChanged += async (object? _, ВстановленняЦінНоменклатури_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
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
                PointerChanged?.Invoke(null, pointer);
            }
        }

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ВстановленняЦінНоменклатури_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ВстановленняЦінНоменклатури_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВстановленняЦінНоменклатури_Pointer();
        }
    }
}