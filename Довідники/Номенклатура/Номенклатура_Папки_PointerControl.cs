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
    class Номенклатура_Папки_PointerControl : PointerControl
    {
        public Номенклатура_Папки_PointerControl()
        {
            pointer = new Номенклатура_Папки_Pointer();
            WidthPresentation = 300;
            Caption = "Родич:";
        }

        public string UidOpenFolder { get; set; } = "";

        Номенклатура_Папки_Pointer pointer;
        public Номенклатура_Папки_Pointer Pointer
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

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Номенклатура_Папки_Дерево page = new Номенклатура_Папки_Дерево(true);

            page.DirectoryPointerItem = Pointer;
            page.UidOpenFolder = UidOpenFolder;
            page.CallBack_OnSelectPointer = (Номенклатура_Папки_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Номенклатура Папки", () => { return page; });

            page.LoadTree();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Номенклатура_Папки_Pointer();
        }
    }
}