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
    class СкладськіКомірки_Папки_PointerControl : PointerControl
    {
        public СкладськіКомірки_Папки_PointerControl()
        {
            pointer = new СкладськіКомірки_Папки_Pointer();
            WidthPresentation = 300;
            Caption = $"{СкладськіКомірки_Папки_Const.FULLNAME}:";
        }

        public string UidOpenFolder { get; set; } = "";

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

                if (pointer != null)
                    Presentation = pointer.GetPresentation();
                else
                    Presentation = "";
            }
        }

        public СкладськіПриміщення_Pointer СкладПриміщенняВласник { get; set; } = new СкладськіПриміщення_Pointer();

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            if (BeforeClickOpenFunc != null)
                BeforeClickOpenFunc.Invoke();
                
            СкладськіКомірки_Папки_Дерево page = new СкладськіКомірки_Папки_Дерево();

            page.DirectoryPointerItem = Pointer;
            page.UidOpenFolder = UidOpenFolder;
            page.СкладПриміщенняВласник = СкладПриміщенняВласник;
            page.CallBack_OnSelectPointer = (СкладськіКомірки_Папки_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage($"Вибір - {СкладськіКомірки_Папки_Const.FULLNAME}", () => { return page; }, true);

            page.LoadTree();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СкладськіКомірки_Папки_Pointer();
        }
    }
}