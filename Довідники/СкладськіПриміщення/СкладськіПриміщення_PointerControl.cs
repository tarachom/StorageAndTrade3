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
    public class СкладськіПриміщення_PointerControl : PointerControl
    {
        event EventHandler<СкладськіПриміщення_Pointer> PointerChanged;

        public СкладськіПриміщення_PointerControl()
        {
            pointer = new СкладськіПриміщення_Pointer();
            WidthPresentation = 300;
            Caption = $"{СкладськіПриміщення_Const.FULLNAME}:";
            PointerChanged += async (object? _, СкладськіПриміщення_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
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
                PointerChanged?.Invoke(null, pointer);
            }
        }

        public Склади_Pointer СкладВласник = new Склади_Pointer();

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            BeforeClickOpenFunc?.Invoke();

            СкладськіПриміщення_ШвидкийВибір page = new СкладськіПриміщення_ШвидкийВибір
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new СкладськіПриміщення_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            page.СкладВласник.Pointer = СкладВласник;

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СкладськіПриміщення_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}