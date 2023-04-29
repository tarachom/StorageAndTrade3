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

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class Basis_PointerControl : PointerControl
    {
        public Basis_PointerControl()
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
                    Presentation = Functions.GetBasisObjectPresentation(pointer, out PointerName, out Type);
                else
                    Presentation = PointerName = Type = "";
            }
        }

        public string PointerName = "";
        public string Type = "";

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            if (String.IsNullOrEmpty(PointerName))
            {
                Popover PopoverSmallSelect = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

                System.Action<string, string> CallBackAction = (string p, string t) =>
                {
                    PointerName = p;
                    Type = t;

                    PopoverSmallSelect.Hide();
                };

                PopoverSmallSelect.Add(ВибірТипуДаних(CallBackAction));
                PopoverSmallSelect.ShowAll();
            }

            if (PointerName == "Документи")
            {
                switch (Type)
                {
                    case "ПоступленняТоварівТаПослуг":
                        {
                            ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг(true);

                            page.DocumentPointerItem = new ПоступленняТоварівТаПослуг_Pointer(pointer.Uuid);
                            page.CallBack_OnSelectPointer = (ПоступленняТоварівТаПослуг_Pointer selectPointer) =>
                            {
                                Pointer = selectPointer.GetBasis();
                            };

                            Program.GeneralForm?.CreateNotebookPage($"Вибір - {ПоступленняТоварівТаПослуг_Const.FULLNAME}", () => { return page; }, true);
                            page.LoadRecords();

                            break;
                        }
                    default:
                        {
                            Message.Info(Program.GeneralForm, $"Для документу '{Type}' не оприділена сторінка вибору");
                            break;
                        }
                }
            }
            else if (PointerName == "Довідники")
            {
                switch (Type)
                {
                    case "Валюти":
                        {
                            Валюти page = new Валюти();
                            page.DirectoryPointerItem = new Валюти_Pointer(pointer.Uuid);
                            page.CallBack_OnSelectPointer = (Валюти_Pointer selectPointer) =>
                            {
                                Pointer = selectPointer.GetBasis();
                            };

                            Program.GeneralForm?.CreateNotebookPage("Вибір - Валюти", () => { return page; }, true);
                            page.LoadRecords();

                            break;
                        }
                    default:
                        {
                            Message.Info(Program.GeneralForm, $"Для довідника '{Type}' не оприділена сторінка вибору");
                            break;
                        }
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
                    row.Add(new Label(directories.Value.Name) { Halign = Align.Start });

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
                    row.Add(new Label(documents.Value.Name) { Halign = Align.Start });

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