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

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    public class Контрагенти_PointerControl : PointerControl
    {
        event EventHandler<Контрагенти_Pointer>? PointerChanged;

        public Контрагенти_PointerControl()
        {
            PointerChanged += OnPointerChanged;

            pointer = new Контрагенти_Pointer();
            WidthPresentation = 300;
            Caption = $"{Контрагенти_Const.FULLNAME}:";
        }

        Контрагенти_Pointer pointer;
        public Контрагенти_Pointer Pointer
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

        protected async void OnPointerChanged(object? sender, Контрагенти_Pointer pointer)
        {
            Presentation = pointer != null ? await pointer.GetPresentation() : "";
        }

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover PopoverSmallSelect = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            if (BeforeClickOpenFunc != null)
                BeforeClickOpenFunc.Invoke();

            Контрагенти_ШвидкийВибір page = new Контрагенти_ШвидкийВибір
            {
                PopoverParent = PopoverSmallSelect,
                DirectoryPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Контрагенти_Pointer(selectPointer);

                    if (AfterSelectFunc != null)
                        AfterSelectFunc.Invoke();
                }
            };

            PopoverSmallSelect.Add(page);
            PopoverSmallSelect.ShowAll();

            await page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Контрагенти_Pointer();

            if (AfterSelectFunc != null)
                AfterSelectFunc.Invoke();
        }
    }
}