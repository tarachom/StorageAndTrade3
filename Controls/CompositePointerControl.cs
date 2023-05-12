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

using System.Reflection;

using AccountingSoftware;
using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class CompositePointerControl : PointerControl
    {
        public CompositePointerControl()
        {
            pointer = new UuidAndText();
            WidthPresentation = 300;
            Caption = "Основа:";
        }

        UuidAndText pointer;
        public UuidAndText Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;

                if (pointer != null)
                    Presentation = Functions.CompositePointerPresentation(pointer, out PointerName, out TypeCaption);
                else
                    Presentation = PointerName = TypeCaption = "";
            }
        }

        /// <summary>
        /// Документи або Довідники
        /// </summary>
        public string PointerName = "";

        /// <summary>
        /// Назва обєкту як в конфігурації
        /// </summary>
        public string TypeCaption = "";

        /// <summary>
        /// Функція формує назву
        /// </summary>
        string GetBasisName()
        {
            return !String.IsNullOrEmpty(PointerName) ? PointerName + "." + TypeCaption : "";
        }

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(PointerName))
            {
                //
                // Вибір типу
                //

                Popover PopoverSmallSelect = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

                System.Action<string, string> CallBackAction = (string p, string t) =>
                {
                    PointerName = p;
                    TypeCaption = t;

                    PopoverSmallSelect.Hide();
                };

                PopoverSmallSelect.Add(ВибірТипуДаних(CallBackAction));
                PopoverSmallSelect.ShowAll();
            }
            else if (PointerName == "Документи" || PointerName == "Довідники")
            {
                Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();

                //Простір імен програми
                string NameSpacePage = "StorageAndTrade";

                //Простір імен конфігурації
                string NameSpaceConfig = $"StorageAndTrade_1_0.{PointerName}";

                object? listPage;

                try
                {
                    listPage = ExecutingAssembly.CreateInstance($"{NameSpacePage}.{TypeCaption}");
                }
                catch (Exception ex)
                {
                    Message.Error(Program.GeneralForm, ex.Message);
                    return;
                }

                if (listPage != null)
                {
                    //Елемент який потрібно виділити в списку
                    listPage.GetType().GetProperty("SelectPointerItem")?.SetValue(listPage, pointer.UnigueID());

                    //Елемент для вибору
                    if (PointerName == "Документи")
                        listPage.GetType().GetProperty("DocumentPointerItem")?.SetValue(listPage, pointer.UnigueID());
                    else
                        listPage.GetType().GetProperty("DirectoryPointerItem")?.SetValue(listPage, pointer.UnigueID());

                    //Функція зворотнього виклику при виборі
                    listPage.GetType().GetProperty("CallBack_OnSelectPointer")?.SetValue(listPage, (UnigueID selectPointer) =>
                    {
                        Pointer = new UuidAndText(selectPointer.UGuid, GetBasisName());
                    });

                    //Заголовок журналу з константи конфігурації
                    string listName = "Список";
                    {
                        Type? documentConst = Type.GetType($"{NameSpaceConfig}.{TypeCaption}_Const");
                        if (documentConst != null)
                            listName = documentConst.GetField("FULLNAME")?.GetValue(null)?.ToString() ?? listName;
                    }

                    Program.GeneralForm?.CreateNotebookPage(listName, () => { return (Widget)listPage; }, true);

                    if (PointerName == "Документи")
                        listPage.GetType().InvokeMember("SetValue", BindingFlags.InvokeMethod, null, listPage, null);
                    else
                        listPage.GetType().InvokeMember("LoadRecords", BindingFlags.InvokeMethod, null, listPage, null);
                }
            }
        }

        VBox ВибірТипуДаних(System.Action<string, string> CallBackAction)
        {
            VBox vBoxContainer = new VBox();

            HBox hBoxCaption = new HBox();
            hBoxCaption.PackStart(new Label("<b>Вибір типу даних</b>") { UseMarkup = true, Halign = Align.Center }, true, false, 0);
            vBoxContainer.PackStart(hBoxCaption, false, false, 0);

            HBox hBox = new HBox();
            vBoxContainer.PackStart(hBox, false, false, 0);

            //Довідники
            {
                VBox vBox = new VBox();
                hBox.PackStart(vBox, false, false, 2);

                vBox.PackStart(new Label("Довідники"), false, false, 2);

                ListBox listBox = new ListBox();
                listBox.ButtonPressEvent += (object? sender, ButtonPressEventArgs args) =>
                {
                    if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
                        if (listBox.SelectedRows.Length != 0)
                            CallBackAction.Invoke("Довідники", listBox.SelectedRows[0].Name);
                };

                ScrolledWindow scrollList = new ScrolledWindow() { WidthRequest = 300, HeightRequest = 400, ShadowType = ShadowType.In };
                scrollList.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                scrollList.Add(listBox);

                vBox.PackStart(scrollList, false, false, 2);

                foreach (KeyValuePair<string, ConfigurationDirectories> directories in Config.Kernel!.Conf.Directories)
                {
                    ListBoxRow row = new ListBoxRow() { Name = directories.Key };
                    row.Add(new Label(directories.Value.FullName) { Halign = Align.Start });

                    listBox.Add(row);
                }
            }

            //Документи
            {
                VBox vBox = new VBox();
                hBox.PackStart(vBox, false, false, 2);

                vBox.PackStart(new Label("Документи"), false, false, 2);

                ListBox listBox = new ListBox();
                listBox.ButtonPressEvent += (object? sender, ButtonPressEventArgs args) =>
                {
                    if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
                        if (listBox.SelectedRows.Length != 0)
                            CallBackAction.Invoke("Документи", listBox.SelectedRows[0].Name);
                };

                ScrolledWindow scrollList = new ScrolledWindow() { WidthRequest = 300, HeightRequest = 400, ShadowType = ShadowType.In };
                scrollList.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                scrollList.Add(listBox);

                vBox.PackStart(scrollList, false, false, 2);

                foreach (KeyValuePair<string, ConfigurationDocuments> documents in Config.Kernel!.Conf.Documents)
                {
                    ListBoxRow row = new ListBoxRow() { Name = documents.Key };
                    row.Add(new Label(documents.Value.FullName) { Halign = Align.Start });

                    listBox.Add(row);
                }
            }

            vBoxContainer.ShowAll();

            return vBoxContainer;
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new UuidAndText();
        }
    }
}