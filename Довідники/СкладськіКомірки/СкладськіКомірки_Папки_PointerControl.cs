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
    class СкладськіКомірки_Папки_PointerControl : PointerControl
    {
        public СкладськіКомірки_Папки_PointerControl()
        {
            pointer = new СкладськіКомірки_Папки_Pointer();
            WidthPresentation = 300;
            Caption = $"{СкладськіКомірки_Папки_Const.FULLNAME}:";
        }

        public UnigueID? OpenFolder { get; set; }

        СкладськіКомірки_Папки_Pointer pointer;
        public СкладськіКомірки_Папки_Pointer Pointer
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

        public СкладськіПриміщення_Pointer СкладПриміщенняВласник { get; set; } = new СкладськіПриміщення_Pointer();

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Popover PopoverSmallSelect = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            BeforeClickOpenFunc?.Invoke();

            СкладськіКомірки_Папки_Дерево_ШвидкийВибір page = new СкладськіКомірки_Папки_Дерево_ШвидкийВибір
            {
                PopoverParent = PopoverSmallSelect,
                OpenFolder = OpenFolder,
                DirectoryPointerItem = Pointer.UnigueID,
                СкладПриміщенняВласник = СкладПриміщенняВласник,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new СкладськіКомірки_Папки_Pointer(selectPointer);

                    AfterSelectFunc?.Invoke();
                }
            };

            PopoverSmallSelect.Add(page);
            PopoverSmallSelect.ShowAll();

            page.LoadTree();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СкладськіКомірки_Папки_Pointer();
        }
    }
}