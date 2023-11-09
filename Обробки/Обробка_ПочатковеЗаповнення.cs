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

using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using Перелічення = StorageAndTrade_1_0.Перелічення;

using System.Xml.XPath;

namespace StorageAndTrade
{
    class Обробка_ПочатковеЗаповнення : VBox
    {
        #region Fields

        Button bFilling;
        Button bStop;
        Switch visibleOnStart;
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

        public Обробка_ПочатковеЗаповнення() : base()
        {
            //Кнопки
            HBox hBoxTop = new HBox();
            PackStart(hBoxTop, false, false, 10);

            bFilling = new Button("Заповнити");
            bFilling.Clicked += OnFilling;

            hBoxTop.PackStart(bFilling, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            hBoxTop.PackStart(bStop, false, false, 10);

            //Показувати при запуску -->
            VBox vBoxSwitch = new VBox();

            HBox hBoxSwitch = new HBox();
            vBoxSwitch.PackStart(hBoxSwitch, false, false, 0);

            visibleOnStart = new Switch() { HeightRequest = 20, Active = !Константи.ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const };
            visibleOnStart.ButtonReleaseEvent += (object? sender, ButtonReleaseEventArgs args) => { Константи.ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const = visibleOnStart.Active; };

            hBoxSwitch.PackStart(visibleOnStart, false, false, 0);
            hBoxSwitch.PackStart(new Label("Показувати при запуску"), false, false, 10);

            hBoxTop.PackEnd(vBoxSwitch, false, false, 10);
            //<-- Показувати при запуску

            scrollMessage = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollMessage.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollMessage.Add(vBoxMessage = new VBox());

            PackStart(scrollMessage, true, true, 0);

            ShowAll();
        }

        void OnFilling(object? sender, EventArgs args)
        {
            ClearMessage();

            Program.ListCancellationToken.Add(CancellationTokenThread = new CancellationTokenSource());

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
                if (!CancellationTokenThread!.IsCancellationRequested)
                {
                    string name = "Валюти";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Валюти_Select валюти_Select = new Валюти_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Валюти/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняЗаЗамовчуванням = currentNode?.GetAttribute("ЗначенняЗаЗамовчуванням", "") ?? "";
                            string Код_R030 = currentNode?.SelectSingleNode("Код")?.Value ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string Коротко = currentNode?.SelectSingleNode("Коротко")?.Value ?? "";
                            string ВиводитиКурсНаСтартову = currentNode?.SelectSingleNode("ВиводитиКурсНаСтартову")?.Value ?? "";

                            Валюти_Pointer валюти_Pointer = валюти_Select.FindByField(Валюти_Const.Код_R030, Код_R030);
                            if (валюти_Pointer.IsEmpty())
                            {
                                Валюти_Objest валюти_Objest = new Валюти_Objest
                                {
                                    Назва = Назва,
                                    Код_R030 = Код_R030,
                                    КороткаНазва = Коротко,
                                    ВиводитиКурсНаСтартову = ВиводитиКурсНаСтартову == "1"
                                };
                                валюти_Objest.New();
                                валюти_Objest.Save();

                                if (ЗначенняЗаЗамовчуванням == "1")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнаВалюта_Const = валюти_Objest.GetDirectoryPointer();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}, код {Код_R030}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва} з кодом {Код_R030}");
                        }
                }

                // Організації
                if (!CancellationTokenThread!.IsCancellationRequested)
                {
                    string name = "Організації";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Організації_Select організації_Select = new Організації_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Організації/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняЗаЗамовчуванням = currentNode?.GetAttribute("ЗначенняЗаЗамовчуванням", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Організації_Pointer організації_Pointer = організації_Select.FindByField(Організації_Const.Назва, Назва);
                            if (організації_Pointer.IsEmpty())
                            {
                                Організації_Objest організації_Objest = new Організації_Objest { Назва = Назва };
                                організації_Objest.New();
                                організації_Objest.Save();

                                if (ЗначенняЗаЗамовчуванням == "1")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнаОрганізація_Const = організації_Objest.GetDirectoryPointer();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // Каси
                if (!CancellationTokenThread!.IsCancellationRequested)
                {
                    string name = "Каси";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Каси_Select вибірка = new Каси_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Каси/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняЗаЗамовчуванням = currentNode?.GetAttribute("ЗначенняЗаЗамовчуванням", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string Валюта = currentNode?.SelectSingleNode("Валюта")?.Value ?? "";

                            Каси_Pointer вказівник = вибірка.FindByField(Каси_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                Каси_Objest обєкт = new Каси_Objest
                                {
                                    Назва = Назва,
                                    Валюта = new Валюти_Select().FindByField(Валюти_Const.Назва, Валюта)
                                };
                                обєкт.New();
                                обєкт.Save();

                                if (ЗначенняЗаЗамовчуванням == "1")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнаКаса_Const = обєкт.GetDirectoryPointer();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // ПакуванняОдиниціВиміру
                if (!CancellationTokenThread!.IsCancellationRequested)
                {
                    string name = "Пакування";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    ПакуванняОдиниціВиміру_Select вибірка = new ПакуванняОдиниціВиміру_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("ПакуванняОдиниціВиміру/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняЗаЗамовчуванням = currentNode?.GetAttribute("ЗначенняЗаЗамовчуванням", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string НазваПовна = currentNode?.SelectSingleNode("НазваПовна")?.Value ?? "";

                            ПакуванняОдиниціВиміру_Pointer вказівник = вибірка.FindByField(ПакуванняОдиниціВиміру_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                ПакуванняОдиниціВиміру_Objest обєкт = new ПакуванняОдиниціВиміру_Objest
                                {
                                    Назва = Назва,
                                    НазваПовна = НазваПовна,
                                    КількістьУпаковок = 1
                                };
                                обєкт.New();
                                обєкт.Save();

                                if (ЗначенняЗаЗамовчуванням == "1")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнаОдиницяПакування_Const = обєкт.GetDirectoryPointer();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // ВидиНоменклатури
                if (!CancellationTokenThread!.IsCancellationRequested)
                {
                    string name = "Види номенклатури";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    ВидиНоменклатури_Select вибірка = new ВидиНоменклатури_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("ВидиНоменклатури/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняЗаЗамовчуванням = currentNode?.GetAttribute("ЗначенняЗаЗамовчуванням", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string ТипНоменклатуриТекст = currentNode?.SelectSingleNode("ТипНоменклатури")?.Value ?? "";

                            Перелічення.ТипиНоменклатури ТипНоменклатури;
                            if (!Enum.TryParse<Перелічення.ТипиНоменклатури>(ТипНоменклатуриТекст, true, out ТипНоменклатури))
                                ТипНоменклатури = Перелічення.ТипиНоменклатури.Товар;

                            ВидиНоменклатури_Pointer вказівник = вибірка.FindByField(ВидиНоменклатури_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                ВидиНоменклатури_Objest обєкт = new ВидиНоменклатури_Objest
                                {
                                    Назва = Назва,
                                    ТипНоменклатури = ТипНоменклатури
                                };
                                обєкт.New();
                                обєкт.Save();

                                if (ЗначенняЗаЗамовчуванням == "1")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидНоменклатури_Const = обєкт.GetDirectoryPointer();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // ВидиЦін
                if (!CancellationTokenThread!.IsCancellationRequested)
                {
                    string name = "Види цін";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    ВидиЦін_Select вибірка = new ВидиЦін_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("ВидиЦін/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняЗаЗамовчуванням = currentNode?.GetAttribute("ЗначенняЗаЗамовчуванням", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string Валюта = currentNode?.SelectSingleNode("Валюта")?.Value ?? "";

                            ВидиЦін_Pointer вказівник = вибірка.FindByField(ВидиЦін_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                ВидиЦін_Objest обєкт = new ВидиЦін_Objest
                                {
                                    Назва = Назва,
                                    Валюта = new Валюти_Select().FindByField(Валюти_Const.Назва, Валюта)
                                };
                                обєкт.New();
                                обєкт.Save();

                                if (ЗначенняЗаЗамовчуванням == "1")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіни_Const = обєкт.GetDirectoryPointer();

                                if (ЗначенняЗаЗамовчуванням == "2")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнийВидЦіниЗакупівлі_Const = обєкт.GetDirectoryPointer();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // Склади
                if (!CancellationTokenThread!.IsCancellationRequested)
                {
                    string name = "Склади";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Склади_Select склади_Select = new Склади_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Склади/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняЗаЗамовчуванням = currentNode?.GetAttribute("ЗначенняЗаЗамовчуванням", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Склади_Pointer склади_Pointer = склади_Select.FindByField(Склади_Const.Назва, Назва);
                            if (склади_Pointer.IsEmpty())
                            {
                                Склади_Objest склади_Objest = new Склади_Objest { Назва = Назва };
                                склади_Objest.New();
                                склади_Objest.Save();

                                if (ЗначенняЗаЗамовчуванням == "1")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнийСклад_Const = склади_Objest.GetDirectoryPointer();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // Контрагенти
                if (!CancellationTokenThread!.IsCancellationRequested)
                {
                    string name = "Контрагенти";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Контрагенти_Select контрагенти_Select = new Контрагенти_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Контрагенти/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняЗаЗамовчуванням = currentNode?.GetAttribute("ЗначенняЗаЗамовчуванням", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Контрагенти_Pointer контрагенти_Pointer = контрагенти_Select.FindByField(Контрагенти_Const.Назва, Назва);
                            if (контрагенти_Pointer.IsEmpty())
                            {
                                Контрагенти_Objest контрагенти_Objest = new Контрагенти_Objest { Назва = Назва };
                                контрагенти_Objest.New();
                                контрагенти_Objest.Save();

                                if (ЗначенняЗаЗамовчуванням == "1")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнийПокупець_Const = контрагенти_Objest.GetDirectoryPointer();

                                if (ЗначенняЗаЗамовчуванням == "2")
                                    Константи.ЗначенняЗаЗамовчуванням.ОсновнийПостачальник_Const = контрагенти_Objest.GetDirectoryPointer();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // Контрагенти Папки
                if (!CancellationTokenThread!.IsCancellationRequested)
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
                                Контрагенти_Папки_Objest контрагенти_Папки_Objest = new Контрагенти_Папки_Objest { Назва = Назва };
                                контрагенти_Папки_Objest.New();
                                контрагенти_Папки_Objest.Save();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}и: {Назва}");
                        }
                }

                // Номенклатура Папки
                if (!CancellationTokenThread!.IsCancellationRequested)
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
                                Номенклатура_Папки_Objest номенклатура_Папки_Objest = new Номенклатура_Папки_Objest { Назва = Назва };
                                номенклатура_Папки_Objest.New();
                                номенклатура_Папки_Objest.Save();

                                CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника {name}: {Назва}");
                            }
                            else
                                CreateMessage(TypeMessage.Info, $"Знайдено елемент довідника {name}: {Назва}");
                        }
                }

                // Номенклатура
                if (!CancellationTokenThread!.IsCancellationRequested)
                {
                    string name = "Номенклатура";

                    CreateMessage(TypeMessage.None, $"\n{name}\n");

                    Номенклатура_Select вибірка = new Номенклатура_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Номенклатура/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string ВидНоменклатури = currentNode?.SelectSingleNode("ВидНоменклатури")?.Value ?? "";
                            string ТипНоменклатуриТекст = currentNode?.SelectSingleNode("ТипНоменклатури")?.Value ?? "";
                            string ПакуванняОдиниціВиміру = currentNode?.SelectSingleNode("ПакуванняОдиниціВиміру")?.Value ?? "";

                            Перелічення.ТипиНоменклатури ТипНоменклатури;
                            if (!Enum.TryParse<Перелічення.ТипиНоменклатури>(ТипНоменклатуриТекст, true, out ТипНоменклатури))
                                ТипНоменклатури = Перелічення.ТипиНоменклатури.Товар;

                            Номенклатура_Pointer вказівник = вибірка.FindByField(Номенклатура_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                Номенклатура_Objest обєкт = new Номенклатура_Objest
                                {
                                    Назва = Назва,
                                    ВидНоменклатури = new ВидиНоменклатури_Select().FindByField(ВидиНоменклатури_Const.Назва, ВидНоменклатури),
                                    ТипНоменклатури = ТипНоменклатури,
                                    ОдиницяВиміру = new ПакуванняОдиниціВиміру_Select().FindByField(ПакуванняОдиниціВиміру_Const.Назва, ПакуванняОдиниціВиміру)
                                };
                                обєкт.New();
                                обєкт.Save();

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

            Program.RemoveCancellationToken(CancellationTokenThread);

            ButtonSensitive(true);

            CreateMessage(TypeMessage.None, "\nГотово\n");

            Thread.Sleep(1000);
            CreateMessage(TypeMessage.None, "\n\n\n");
        }

        void ButtonSensitive(bool sensitive)
        {
            Gtk.Application.Invoke
            (
                delegate
                {
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
                                hBoxInfo.PackStart(new Image(AppContext.BaseDirectory + "images/16/ok.png"), false, false, 5);
                                break;
                            }
                        case TypeMessage.Error:
                            {
                                hBoxInfo.PackStart(new Image(AppContext.BaseDirectory + "images/16/error.png"), false, false, 5);
                                break;
                            }
                        case TypeMessage.Info:
                            {
                                hBoxInfo.PackStart(new Image(AppContext.BaseDirectory + "images/16/info.png"), false, false, 5);
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
            Program.RemoveCancellationToken(CancellationTokenThread);
        }

    }
}