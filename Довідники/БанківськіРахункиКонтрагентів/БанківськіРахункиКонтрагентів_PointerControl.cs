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
    class БанківськіРахункиКонтрагентів_PointerControl : PointerControl
    {
        public БанківськіРахункиКонтрагентів_PointerControl()
        {
            pointer = new БанківськіРахункиКонтрагентів_Pointer();
            WidthPresentation = 300;
            Caption = $"{БанківськіРахункиКонтрагентів_Const.FULLNAME}:";
        }

        БанківськіРахункиКонтрагентів_Pointer pointer;
        public БанківськіРахункиКонтрагентів_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;
                Presentation = pointer != null ? Task.Run(async () => { return await pointer.GetPresentation(); }).Result : "";
            }
        }

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover PopoverSmallSelect = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            BeforeClickOpenFunc?.Invoke();

            БанківськіРахункиКонтрагентів_ШвидкийВибір page = new БанківськіРахункиКонтрагентів_ШвидкийВибір() { PopoverParent = PopoverSmallSelect, DirectoryPointerItem = Pointer.UnigueID };
            page.CallBack_OnSelectPointer = (UnigueID selectPointer) =>
            {
                Pointer = new БанківськіРахункиКонтрагентів_Pointer(selectPointer);

                AfterSelectFunc?.Invoke();
            };

            PopoverSmallSelect.Add(page);
            PopoverSmallSelect.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new БанківськіРахункиКонтрагентів_Pointer();
        }
    }
}