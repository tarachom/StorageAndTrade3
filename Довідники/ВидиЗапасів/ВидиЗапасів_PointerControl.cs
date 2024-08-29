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
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ВидиЗапасів_PointerControl : PointerControl
    {
        event EventHandler<ВидиЗапасів_Pointer> PointerChanged;

        public ВидиЗапасів_PointerControl()
        {
            pointer = new ВидиЗапасів_Pointer();
            WidthPresentation = 300;
            Caption = $"{ВидиЗапасів_Const.FULLNAME}:";
            PointerChanged += async (object? _, ВидиЗапасів_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ВидиЗапасів_Pointer pointer;
        public ВидиЗапасів_Pointer Pointer
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
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            BeforeClickOpenFunc?.Invoke();

            ВидиЗапасів_ШвидкийВибір page = new ВидиЗапасів_ШвидкийВибір
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ВидиЗапасів_Pointer(selectPointer);

                    AfterSelectFunc?.Invoke();
                }
            };

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВидиЗапасів_Pointer();
        }
    }
}