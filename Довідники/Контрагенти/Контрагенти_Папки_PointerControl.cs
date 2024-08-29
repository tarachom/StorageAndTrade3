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
    class Контрагенти_Папки_PointerControl : PointerControl
    {
        event EventHandler<Контрагенти_Папки_Pointer> PointerChanged;

        public Контрагенти_Папки_PointerControl()
        {
            pointer = new Контрагенти_Папки_Pointer();
            WidthPresentation = 300;
            Caption = $"{Контрагенти_Папки_Const.FULLNAME}:";
            PointerChanged += async (object? _, Контрагенти_Папки_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        public UnigueID? OpenFolder { get; set; }

        Контрагенти_Папки_Pointer pointer;
        public Контрагенти_Папки_Pointer Pointer
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

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            BeforeClickOpenFunc?.Invoke();

            Контрагенти_Папки_Дерево_ШвидкийВибір page = new Контрагенти_Папки_Дерево_ШвидкийВибір
            {
                PopoverParent = popover,
                OpenFolder = OpenFolder,
                DirectoryPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Контрагенти_Папки_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            popover.Add(page);
            popover.ShowAll();

            page.LoadTree();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Контрагенти_Папки_Pointer();
        }
    }
}