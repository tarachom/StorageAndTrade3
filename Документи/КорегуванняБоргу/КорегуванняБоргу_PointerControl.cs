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

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class КорегуванняБоргу_PointerControl : PointerControl
    {
        public КорегуванняБоргу_PointerControl()
        {
            pointer = new КорегуванняБоргу_Pointer();
            WidthPresentation = 300;
            Caption = $"{КорегуванняБоргу_Const.FULLNAME}:";
        }

        КорегуванняБоргу_Pointer pointer;
        public КорегуванняБоргу_Pointer Pointer
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

        //Відбір по періоду в журналі
        public bool UseWherePeriod { get; set; } = false;

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            КорегуванняБоргу page = new КорегуванняБоргу
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new КорегуванняБоргу_Pointer(selectPointer);
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"Вибір - {КорегуванняБоргу_Const.FULLNAME}", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new КорегуванняБоргу_Pointer();
        }
    }
}