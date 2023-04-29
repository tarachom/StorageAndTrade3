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
    class СкладськіПриміщення_PointerControl : PointerControl
    {
        public СкладськіПриміщення_PointerControl()
        {
            pointer = new СкладськіПриміщення_Pointer();
            WidthPresentation = 300;
            Caption = $"{СкладськіПриміщення_Const.FULLNAME}:";
        }

        СкладськіПриміщення_Pointer pointer;
        public СкладськіПриміщення_Pointer Pointer
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

        public Склади_Pointer СкладВласник = new Склади_Pointer();

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Popover PopoverSmallSelect = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            if (BeforeClickOpenFunc != null)
                BeforeClickOpenFunc.Invoke();

            СкладськіПриміщення_ШвидкийВибір page = new СкладськіПриміщення_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = Pointer };

            page.DirectoryPointerItem = Pointer;
            page.СкладВласник.Pointer = СкладВласник;
            page.CallBack_OnSelectPointer = (СкладськіПриміщення_Pointer selectPointer) =>
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
            Pointer = new СкладськіПриміщення_Pointer();

            if (AfterSelectFunc != null)
                AfterSelectFunc.Invoke();
        }
    }
}