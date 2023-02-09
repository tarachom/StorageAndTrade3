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

namespace StorageAndTrade
{
    abstract class PointerControl : HBox
    {
        Label labelCaption = new Label();
        Entry entryText = new Entry();

        public PointerControl() : base()
        {
            PackStart(labelCaption, false, false, 5);
            PackStart(entryText, false, false, 0);

            Button bOpen = new Button(new Image("images/find.png"));
            bOpen.Clicked += OpenSelect;

            Button bClear = new Button(new Image("images/clean.png"));
            bClear.Clicked += OnClear;

            PackStart(bOpen, false, false, 2);
            PackStart(bClear, false, false, 2);
        }

        protected virtual void OpenSelect(object? sender, EventArgs args) { }
        protected virtual void OnClear(object? sender, EventArgs args) { }

        /// <summary>
        /// Функція яка викликається перед відкриттям вибору
        /// </summary>
        public System.Action? BeforeClickOpenFunc { get; set; }

        /// <summary>
        /// Функція яка викликається після вибору.
        /// </summary>
        public System.Action? AfterSelectFunc { get; set; }

        public string Caption
        {
            get
            {
                return labelCaption.Text;
            }
            set
            {
                labelCaption.Text = value;
            }
        }

        public int WidthPresentation
        {
            get
            {
                return entryText.WidthRequest;
            }
            set
            {
                entryText.WidthRequest = value;
            }
        }

        public System.Action? Select { get; set; }

        protected string Presentation
        {
            get
            {
                return entryText.Text;
            }
            set
            {
                entryText.Text = value;
            }
        }
    }
}