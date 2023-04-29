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

namespace StorageAndTrade
{
    class ДоговориКонтрагентів_PointerControl : PointerControl
    {
        public ДоговориКонтрагентів_PointerControl()
        {
            pointer = new ДоговориКонтрагентів_Pointer();
            WidthPresentation = 300;
            Caption = $"{ДоговориКонтрагентів_Const.FULLNAME}:";
        }

        ДоговориКонтрагентів_Pointer pointer;
        public ДоговориКонтрагентів_Pointer Pointer
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

        public Контрагенти_Pointer КонтрагентВласник { get; set; } = new Контрагенти_Pointer();

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Popover PopoverSmallSelect = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            if (BeforeClickOpenFunc != null)
                BeforeClickOpenFunc.Invoke();

            ДоговориКонтрагентів_ШвидкийВибір page = new ДоговориКонтрагентів_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = Pointer };

            page.DirectoryPointerItem = Pointer;
            page.КонтрагентВласник.Pointer = КонтрагентВласник;
            page.CallBack_OnSelectPointer = (ДоговориКонтрагентів_Pointer selectPointer) =>
            {
                Pointer = selectPointer;

                if (AfterSelectFunc != null)
                    AfterSelectFunc.Invoke();
            };

            PopoverSmallSelect.Add(page);
            PopoverSmallSelect.ShowAll();

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ДоговориКонтрагентів_Pointer();

            if (AfterSelectFunc != null)
                AfterSelectFunc.Invoke();
        }
    }
}