#region Info

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

#endregion

using Gtk;

using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;

using System.Xml.XPath;

namespace StorageAndTrade
{
    class PageInitialFilling : VBox
    {
        #region Fields

        Button bClose;
        Button bFilling;
        Button bStop;
        Switch visibleOnSart;
        ScrolledWindow scrollMessage;
        VBox vBoxMessage;
        CancellationTokenSource? CancellationTokenThread { get; set; }

        enum TypeMessage
        {
            Ok,
            Error,
            Info,
            None
        }

        #endregion

        public PageInitialFilling() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            bFilling = new Button("Заповнити");
            bFilling.Clicked += OnFilling;

            hBoxBotton.PackStart(bFilling, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            hBoxBotton.PackStart(bStop, false, false, 10);

            //Показувати при запуску -->
            VBox vBoxSwitch = new VBox();

            HBox hBoxSwitch = new HBox();
            vBoxSwitch.PackStart(hBoxSwitch, false, false, 0);

            visibleOnSart = new Switch() { HeightRequest = 20, Active = !Константи.ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const };
            visibleOnSart.ButtonReleaseEvent += (object? sender, ButtonReleaseEventArgs args) => { Константи.ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const = visibleOnSart.Active; };

            hBoxSwitch.PackStart(visibleOnSart, false, false, 0);
            hBoxSwitch.PackStart(new Label("Показувати при запуску"), false, false, 10);

            hBoxBotton.PackEnd(vBoxSwitch, false, false, 10);
            //<-- Показувати при запуску

            PackStart(hBoxBotton, false, false, 10);

            scrollMessage = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollMessage.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollMessage.Add(vBoxMessage = new VBox());

            PackStart(scrollMessage, true, true, 0);

            ShowAll();
        }

        void OnFilling(object? sender, EventArgs args)
        {
            ClearMessage();

            CancellationTokenThread = new CancellationTokenSource();
            Thread thread = new Thread(new ThreadStart(Filling));
            thread.Start();
        }

        void Filling()
        {
            ButtonSensitive(false);

            string initialFillingXmlFilePath = System.IO.Path.Combine(AppContext.BaseDirectory, "template/InitialFilling.xml");

            if (File.Exists(initialFillingXmlFilePath))
            {
                CreateMessage(TypeMessage.Ok, "Файл - " + initialFillingXmlFilePath);

                XPathDocument xPathDoc = new XPathDocument(initialFillingXmlFilePath);
                XPathNavigator xPathDocNavigator = xPathDoc.CreateNavigator();
                XPathNavigator? rootNode = xPathDocNavigator.SelectSingleNode("/root");
                XPathNavigator? rootДовідники = rootNode?.SelectSingleNode("Довідники");

                //Валюти
                {
                    string name = "Валюти";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Валюти_Select валюти_Select = new Валюти_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Валюти/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string Код_R030 = currentNode?.SelectSingleNode("Код")?.Value ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string Коротко = currentNode?.SelectSingleNode("Коротко")?.Value ?? "";

                            Валюти_Pointer валюти_Pointer = валюти_Select.FindByField(Валюти_Const.Код_R030, Код_R030);
                            if (валюти_Pointer.IsEmpty())
                            {
                                Валюти_Objest валюти_Objest = new Валюти_Objest();
                                валюти_Objest.New();
                                валюти_Objest.Код = (++Константи.НумераціяДовідників.Валюти_Const).ToString("D6");
                                валюти_Objest.Назва = Назва;
                                валюти_Objest.Код_R030 = Код_R030;
                                валюти_Objest.КороткаНазва = Коротко;
                                валюти_Objest.Save();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}, код {Код_R030}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва} з кодом {Код_R030}");
                        }
                }

                // Організації
                {
                    string name = "Організації";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Організації_Select організації_Select = new Організації_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Організації/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Організації_Pointer організації_Pointer = організації_Select.FindByField(Організації_Const.Назва, Назва);
                            if (організації_Pointer.IsEmpty())
                            {
                                Організації_Objest організації_Objest = new Організації_Objest();
                                організації_Objest.New();
                                організації_Objest.Код = (++Константи.НумераціяДовідників.Організації_Const).ToString("D6");
                                організації_Objest.Назва = Назва;
                                організації_Objest.Save();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // Склади
                {
                    string name = "Склади";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Склади_Select склади_Select = new Склади_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Склади/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Склади_Pointer склади_Pointer = склади_Select.FindByField(Склади_Const.Назва, Назва);
                            if (склади_Pointer.IsEmpty())
                            {
                                Склади_Objest склади_Objest = new Склади_Objest();
                                склади_Objest.New();
                                склади_Objest.Код = (++Константи.НумераціяДовідників.Склади_Const).ToString("D6");
                                склади_Objest.Назва = Назва;
                                склади_Objest.Save();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // Контрагенти
                {
                    string name = "Контрагенти";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Контрагенти_Select контрагенти_Select = new Контрагенти_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Контрагенти/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Контрагенти_Pointer контрагенти_Pointer = контрагенти_Select.FindByField(Контрагенти_Const.Назва, Назва);
                            if (контрагенти_Pointer.IsEmpty())
                            {
                                Контрагенти_Objest контрагенти_Objest = new Контрагенти_Objest();
                                контрагенти_Objest.New();
                                контрагенти_Objest.Код = (++Константи.НумераціяДовідників.Контрагенти_Const).ToString("D6");
                                контрагенти_Objest.Назва = Назва;
                                контрагенти_Objest.Save();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // Контрагенти Папки
                {
                    string name = "Контрагенти папки";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Контрагенти_Папки_Select контрагенти_Папки_Select = new Контрагенти_Папки_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Контрагенти_Папки/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Контрагенти_Папки_Pointer контрагенти_Папки_Pointer = контрагенти_Папки_Select.FindByField(Контрагенти_Папки_Const.Назва, Назва);
                            if (контрагенти_Папки_Pointer.IsEmpty())
                            {
                                Контрагенти_Папки_Objest контрагенти_Папки_Objest = new Контрагенти_Папки_Objest();
                                контрагенти_Папки_Objest.New();
                                контрагенти_Папки_Objest.Код = (++Константи.НумераціяДовідників.Контрагенти_Папки_Const).ToString("D6");
                                контрагенти_Папки_Objest.Назва = Назва;
                                контрагенти_Папки_Objest.Save();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}и: {Назва}");
                        }
                }

                // Номенклатура Папки
                {
                    string name = "Номенклатура папки";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Номенклатура_Папки_Select номенклатура_Папки_Select = new Номенклатура_Папки_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Номенклатура_Папки/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Номенклатура_Папки_Pointer номенклатура_Папки_Pointer = номенклатура_Папки_Select.FindByField(Номенклатура_Папки_Const.Назва, Назва);
                            if (номенклатура_Папки_Pointer.IsEmpty())
                            {
                                Номенклатура_Папки_Objest номенклатура_Папки_Objest = new Номенклатура_Папки_Objest();
                                номенклатура_Папки_Objest.New();
                                номенклатура_Папки_Objest.Код = (++Константи.НумераціяДовідників.Номенклатура_Папки_Const).ToString("D6");
                                номенклатура_Папки_Objest.Назва = Назва;
                                номенклатура_Папки_Objest.Save();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }
            }
            else
            {
                CreateMessage(TypeMessage.Error, $"Не знайдений файл {initialFillingXmlFilePath}");
                CreateMessage(TypeMessage.None, "Початкове заповнення перервано!");
            }

            ButtonSensitive(true);
        }

        void ButtonSensitive(bool sensitive)
        {
            Gtk.Application.Invoke
            (
                delegate
                {
                    bClose.Sensitive = sensitive;
                    bFilling.Sensitive = sensitive;
                    bStop.Sensitive = !sensitive;
                }
            );
        }

        void CreateMessage(TypeMessage typeMsg, string message)
        {
            Gtk.Application.Invoke
            (
                delegate
                {
                    HBox hBoxInfo = new HBox();
                    vBoxMessage.PackStart(hBoxInfo, false, false, 2);

                    switch (typeMsg)
                    {
                        case TypeMessage.Ok:
                            {
                                hBoxInfo.PackStart(new Image("images/16/ok.png"), false, false, 5);
                                break;
                            }
                        case TypeMessage.Error:
                            {
                                hBoxInfo.PackStart(new Image("images/16/error.png"), false, false, 5);
                                break;
                            }
                        case TypeMessage.Info:
                            {
                                hBoxInfo.PackStart(new Image("images/16/info.png"), false, false, 5);
                                break;
                            }
                        case TypeMessage.None:
                            {
                                hBoxInfo.PackStart(new Label(""), false, false, 5);
                                break;
                            }
                    }

                    hBoxInfo.PackStart(new Label(message) { Wrap = true }, false, false, 0);
                    hBoxInfo.ShowAll();

                    scrollMessage.Vadjustment.Value = scrollMessage.Vadjustment.Upper;
                }
            );
        }

        void ClearMessage()
        {
            foreach (Widget Child in vBoxMessage.Children)
                vBoxMessage.Remove(Child);
        }

        void OnStopClick(object? sender, EventArgs args)
        {
            CancellationTokenThread?.Cancel();
        }

    }
}