
/*
        Обробка_ПочатковеЗаповнення.cs
*/

using Gtk;
using InterfaceGtk;

using Константи = GeneratedCode.Константи;
using GeneratedCode.Довідники;
using Перелічення = GeneratedCode.Перелічення;

using System.Xml.XPath;

namespace StorageAndTrade
{
    class Обробка_ПочатковеЗаповнення : Обробка
    {
        #region Fields

        Button bFilling;
        Button bStop;
        CancellationTokenSource? CancellationToken { get; set; }

        #endregion

        public Обробка_ПочатковеЗаповнення()
        {
            bFilling = new Button("Заповнити");
            bFilling.Clicked += OnFilling;

            HBoxTop.PackStart(bFilling, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            HBoxTop.PackStart(bStop, false, false, 10);

            //Показувати при запуску
            {
                Switch onStart = new Switch() { Active = !Константи.ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const };
                onStart.ButtonReleaseEvent += (sender, args) => Константи.ПриЗапускуПрограми.ПрограмаЗаповненаПочатковимиДаними_Const = onStart.Active;

                Box vBox = new Box(Orientation.Vertical, 0);
                CreateField(vBox, "Показувати при запуску", onStart);

                HBoxTop.PackEnd(vBox, false, false, 10);
            }

            Button bClear = new Button("Очистити");
            bClear.Clicked += (sender, e) => Лог.ClearMessage();
            HBoxTop.PackEnd(bClear, false, false, 10);

            ShowAll();
        }

        async void OnFilling(object? sender, EventArgs args)
        {
            CancellationToken = new CancellationTokenSource();
            await Filling();
        }

        async ValueTask Filling()
        {
            ButtonSensitive(false);

            string initialFillingXmlFilePath = System.IO.Path.Combine(AppContext.BaseDirectory, "template/InitialFilling.xml");

            if (File.Exists(initialFillingXmlFilePath))
            {
                Лог.CreateMessage("Файл - " + initialFillingXmlFilePath, LogMessage.TypeMessage.Ok);

                XPathDocument xPathDoc = new XPathDocument(initialFillingXmlFilePath);
                XPathNavigator xPathDocNavigator = xPathDoc.CreateNavigator();
                XPathNavigator? rootNode = xPathDocNavigator.SelectSingleNode("/root");
                XPathNavigator? rootДовідники = rootNode?.SelectSingleNode("Довідники");

                //Валюти
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Валюти";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    Валюти_Select валюти_Select = new Валюти_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Валюти/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняТипові = currentNode?.GetAttribute("ЗначенняТипові", "") ?? "";
                            string Код_R030 = currentNode?.SelectSingleNode("Код")?.Value ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string Коротко = currentNode?.SelectSingleNode("Коротко")?.Value ?? "";
                            string ВиводитиКурсНаСтартову = currentNode?.SelectSingleNode("ВиводитиКурсНаСтартову")?.Value ?? "";

                            Валюти_Pointer валюти_Pointer = await валюти_Select.FindByField(Валюти_Const.Код_R030, Код_R030);
                            if (валюти_Pointer.IsEmpty())
                            {
                                Валюти_Objest валюти_Objest = new Валюти_Objest
                                {
                                    Назва = Назва,
                                    Код_R030 = Код_R030,
                                    КороткаНазва = Коротко,
                                    ВиводитиКурсНаСтартову = ВиводитиКурсНаСтартову == "1"
                                };
                                await валюти_Objest.New();
                                await валюти_Objest.Save();

                                if (ЗначенняТипові == "1")
                                    Константи.ЗначенняТипові.ОсновнаВалюта_Const = валюти_Objest.GetDirectoryPointer();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}, код {Код_R030}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва} з кодом {Код_R030}", LogMessage.TypeMessage.Info);
                        }
                }

                // Організації
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Організації";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    Організації_Select організації_Select = new Організації_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Організації/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняТипові = currentNode?.GetAttribute("ЗначенняТипові", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Організації_Pointer організації_Pointer = await організації_Select.FindByField(Організації_Const.Назва, Назва);
                            if (організації_Pointer.IsEmpty())
                            {
                                Організації_Objest організації_Objest = new Організації_Objest { Назва = Назва };
                                await організації_Objest.New();
                                await організації_Objest.Save();

                                if (ЗначенняТипові == "1")
                                    Константи.ЗначенняТипові.ОсновнаОрганізація_Const = організації_Objest.GetDirectoryPointer();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }

                // Каси
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Каси";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    Каси_Select вибірка = new Каси_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Каси/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняТипові = currentNode?.GetAttribute("ЗначенняТипові", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string Валюта = currentNode?.SelectSingleNode("Валюта")?.Value ?? "";

                            Каси_Pointer вказівник = await вибірка.FindByField(Каси_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                Каси_Objest обєкт = new Каси_Objest
                                {
                                    Назва = Назва,
                                    Валюта = await new Валюти_Select().FindByField(Валюти_Const.Назва, Валюта)
                                };

                                await обєкт.New();
                                await обєкт.Save();

                                if (ЗначенняТипові == "1")
                                    Константи.ЗначенняТипові.ОсновнаКаса_Const = обєкт.GetDirectoryPointer();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }

                // ПакуванняОдиниціВиміру
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Пакування";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    ПакуванняОдиниціВиміру_Select вибірка = new ПакуванняОдиниціВиміру_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("ПакуванняОдиниціВиміру/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняТипові = currentNode?.GetAttribute("ЗначенняТипові", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string НазваПовна = currentNode?.SelectSingleNode("НазваПовна")?.Value ?? "";

                            ПакуванняОдиниціВиміру_Pointer вказівник = await вибірка.FindByField(ПакуванняОдиниціВиміру_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                ПакуванняОдиниціВиміру_Objest обєкт = new ПакуванняОдиниціВиміру_Objest
                                {
                                    Назва = Назва,
                                    НазваПовна = НазваПовна,
                                    КількістьУпаковок = 1
                                };
                                await обєкт.New();
                                await обєкт.Save();

                                if (ЗначенняТипові == "1")
                                    Константи.ЗначенняТипові.ОсновнаОдиницяПакування_Const = обєкт.GetDirectoryPointer();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }

                // ВидиНоменклатури
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Види номенклатури";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    ВидиНоменклатури_Select вибірка = new ВидиНоменклатури_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("ВидиНоменклатури/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняТипові = currentNode?.GetAttribute("ЗначенняТипові", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string ТипНоменклатуриТекст = currentNode?.SelectSingleNode("ТипНоменклатури")?.Value ?? "";

                            Перелічення.ТипиНоменклатури ТипНоменклатури;
                            if (!Enum.TryParse<Перелічення.ТипиНоменклатури>(ТипНоменклатуриТекст, true, out ТипНоменклатури))
                                ТипНоменклатури = Перелічення.ТипиНоменклатури.Товар;

                            ВидиНоменклатури_Pointer вказівник = await вибірка.FindByField(ВидиНоменклатури_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                ВидиНоменклатури_Objest обєкт = new ВидиНоменклатури_Objest
                                {
                                    Назва = Назва,
                                    ТипНоменклатури = ТипНоменклатури
                                };
                                await обєкт.New();
                                await обєкт.Save();

                                if (ЗначенняТипові == "1")
                                    Константи.ЗначенняТипові.ОсновнийВидНоменклатури_Const = обєкт.GetDirectoryPointer();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }

                // ВидиЦін
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Види цін";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    ВидиЦін_Select вибірка = new ВидиЦін_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("ВидиЦін/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняТипові = currentNode?.GetAttribute("ЗначенняТипові", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";
                            string Валюта = currentNode?.SelectSingleNode("Валюта")?.Value ?? "";

                            ВидиЦін_Pointer вказівник = await вибірка.FindByField(ВидиЦін_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                ВидиЦін_Objest обєкт = new ВидиЦін_Objest
                                {
                                    Назва = Назва,
                                    Валюта = await new Валюти_Select().FindByField(Валюти_Const.Назва, Валюта)
                                };
                                await обєкт.New();
                                await обєкт.Save();

                                if (ЗначенняТипові == "1")
                                    Константи.ЗначенняТипові.ОсновнийВидЦіни_Const = обєкт.GetDirectoryPointer();

                                if (ЗначенняТипові == "2")
                                    Константи.ЗначенняТипові.ОсновнийВидЦіниЗакупівлі_Const = обєкт.GetDirectoryPointer();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }

                // Склади
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Склади";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    Склади_Select склади_Select = new Склади_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Склади/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняТипові = currentNode?.GetAttribute("ЗначенняТипові", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Склади_Pointer склади_Pointer = await склади_Select.FindByField(Склади_Const.Назва, Назва);
                            if (склади_Pointer.IsEmpty())
                            {
                                Склади_Objest склади_Objest = new Склади_Objest { Назва = Назва };
                                await склади_Objest.New();
                                await склади_Objest.Save();

                                if (ЗначенняТипові == "1")
                                    Константи.ЗначенняТипові.ОсновнийСклад_Const = склади_Objest.GetDirectoryPointer();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }

                // Контрагенти
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Контрагенти";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    Контрагенти_Select контрагенти_Select = new Контрагенти_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Контрагенти/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string ЗначенняТипові = currentNode?.GetAttribute("ЗначенняТипові", "") ?? "";
                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Контрагенти_Pointer контрагенти_Pointer = await контрагенти_Select.FindByField(Контрагенти_Const.Назва, Назва);
                            if (контрагенти_Pointer.IsEmpty())
                            {
                                Контрагенти_Objest контрагенти_Objest = new Контрагенти_Objest { Назва = Назва };
                                await контрагенти_Objest.New();
                                await контрагенти_Objest.Save();

                                if (ЗначенняТипові == "1")
                                    Константи.ЗначенняТипові.ОсновнийПокупець_Const = контрагенти_Objest.GetDirectoryPointer();

                                if (ЗначенняТипові == "2")
                                    Константи.ЗначенняТипові.ОсновнийПостачальник_Const = контрагенти_Objest.GetDirectoryPointer();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }

                // Контрагенти Папки
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Контрагенти папки";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    Контрагенти_Папки_Select контрагенти_Папки_Select = new Контрагенти_Папки_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Контрагенти_Папки/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Контрагенти_Папки_Pointer контрагенти_Папки_Pointer = await контрагенти_Папки_Select.FindByField(Контрагенти_Папки_Const.Назва, Назва);
                            if (контрагенти_Папки_Pointer.IsEmpty())
                            {
                                Контрагенти_Папки_Objest контрагенти_Папки_Objest = new Контрагенти_Папки_Objest { Назва = Назва };
                                await контрагенти_Папки_Objest.New();
                                await контрагенти_Папки_Objest.Save();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}и: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }

                // Номенклатура Папки
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Номенклатура папки";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

                    Номенклатура_Папки_Select номенклатура_Папки_Select = new Номенклатура_Папки_Select();

                    XPathNodeIterator? ДовідникЗаписи = rootДовідники?.Select("Номенклатура_Папки/Запис");

                    if (ДовідникЗаписи != null)
                        while (ДовідникЗаписи.MoveNext())
                        {
                            XPathNavigator? currentNode = ДовідникЗаписи.Current;

                            string Назва = currentNode?.SelectSingleNode("Назва")?.Value ?? "";

                            Номенклатура_Папки_Pointer номенклатура_Папки_Pointer = await номенклатура_Папки_Select.FindByField(Номенклатура_Папки_Const.Назва, Назва);
                            if (номенклатура_Папки_Pointer.IsEmpty())
                            {
                                Номенклатура_Папки_Objest номенклатура_Папки_Objest = new Номенклатура_Папки_Objest { Назва = Назва };
                                await номенклатура_Папки_Objest.New();
                                await номенклатура_Папки_Objest.Save();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }

                // Номенклатура
                if (!CancellationToken!.IsCancellationRequested)
                {
                    string name = "Номенклатура";
                    Лог.CreateMessage($"<b>{name}</b>", LogMessage.TypeMessage.None);

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

                            if (!Enum.TryParse<Перелічення.ТипиНоменклатури>(ТипНоменклатуриТекст, true, out Перелічення.ТипиНоменклатури ТипНоменклатури))
                                ТипНоменклатури = Перелічення.ТипиНоменклатури.Товар;

                            Номенклатура_Pointer вказівник = await вибірка.FindByField(Номенклатура_Const.Назва, Назва);
                            if (вказівник.IsEmpty())
                            {
                                Номенклатура_Objest обєкт = new Номенклатура_Objest
                                {
                                    Назва = Назва,
                                    ВидНоменклатури = await new ВидиНоменклатури_Select().FindByField(ВидиНоменклатури_Const.Назва, ВидНоменклатури),
                                    ТипНоменклатури = ТипНоменклатури,
                                    ОдиницяВиміру = await new ПакуванняОдиниціВиміру_Select().FindByField(ПакуванняОдиниціВиміру_Const.Назва, ПакуванняОдиниціВиміру)
                                };
                                await обєкт.New();
                                await обєкт.Save();

                                Лог.CreateMessage($"Додано новий елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Ok);
                            }
                            else
                                Лог.CreateMessage($"Знайдено елемент довідника {name}: {Назва}", LogMessage.TypeMessage.Info);
                        }
                }
            }
            else
            {
                Лог.CreateMessage($"Не знайдений файл {initialFillingXmlFilePath}", LogMessage.TypeMessage.Error);
                Лог.CreateMessage("Початкове заповнення перервано!", LogMessage.TypeMessage.None);
            }

            ButtonSensitive(true);

            Лог.CreateMessage("Готово", LogMessage.TypeMessage.None);

            await Task.Delay(1000);
            Лог.CreateEmptyMsg();
        }

        void ButtonSensitive(bool sensitive)
        {
            bFilling.Sensitive = sensitive;
            bStop.Sensitive = !sensitive;
        }

        void OnStopClick(object? sender, EventArgs args)
        {
            CancellationToken?.Cancel();
        }

    }
}