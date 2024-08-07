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

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_PointerControl : PointerControl
    {
        public ПартіяТоварівКомпозит_PointerControl()
        {
            pointer = new ПартіяТоварівКомпозит_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПартіяТоварівКомпозит_Const.FULLNAME}:";
        }

        ПартіяТоварівКомпозит_Pointer pointer;
        public ПартіяТоварівКомпозит_Pointer Pointer
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
            ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит
            {
                DirectoryPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ПартіяТоварівКомпозит_Pointer(selectPointer);
                }
            };

            Program.GeneralForm?.CreateNotebookPage($"Вибір - {ПартіяТоварівКомпозит_Const.FULLNAME}", () => { return page; });

            await page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПартіяТоварівКомпозит_Pointer();
        }
    }
}