using Gtk;

using System.Reflection;
using AccountingSoftware;
using Константи = StorageAndTrade_1_0.Константи;
using Журнали = StorageAndTrade_1_0.Журнали;

namespace StorageAndTrade
{
    class PageService : VBox
    {
        Button bClose;
        Button bSpendTheDocument;
        Button bCalculationBalances;
        Button bStop;
        ScrolledWindow scrollMessage;
        VBox vBoxMessage = new VBox();

        CancellationTokenSource? CancellationTokenThread { get; set; }

        enum TypeMessage
        {
            Ok,
            Error,
            Info,
            None
        }

        public PageService() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            bSpendTheDocument = new Button("Перепровести документи");
            bSpendTheDocument.Clicked += OnSpendTheDocument;

            hBoxBotton.PackStart(bSpendTheDocument, false, false, 10);

            bCalculationBalances = new Button("Перерахувати залишки");
            bCalculationBalances.Clicked += OnCalculationBalances;

            hBoxBotton.PackStart(bCalculationBalances, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            hBoxBotton.PackStart(bStop, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            scrollMessage = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollMessage.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollMessage.Add(vBoxMessage);

            PackStart(scrollMessage, true, true, 0);

            ShowAll();
        }

        void ButtonSensitive(bool sensitive)
        {
            Gtk.Application.Invoke
            (
                delegate
                {
                    bClose.Sensitive = sensitive;
                    bSpendTheDocument.Sensitive = sensitive;
                    bCalculationBalances.Sensitive = sensitive;
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

        #region SpendTheDocument

        void OnSpendTheDocument(object? sender, EventArgs args)
        {
            ClearMessage();

            CancellationTokenThread = new CancellationTokenSource();
            Thread thread = new Thread(new ThreadStart(SpendTheDocument));
            thread.Start();
        }

        void SpendTheDocument()
        {
            ButtonSensitive(false);

            Константи.Системні.ЗупинитиФоновіЗадачі_Const = true;

            ФункціїДляПовідомлень.ОчиститиПовідомлення();

            Журнали.Journal_Select journalSelect = new Журнали.Journal_Select();
            journalSelect.Select(DateTime.Parse("01.01.2000 00:00:00"), DateTime.Now);

            while (journalSelect.MoveNext())
            {
                if (CancellationTokenThread!.IsCancellationRequested)
                    break;

                if (journalSelect.Current.Spend)
                {
                    DocumentObject? doc = journalSelect.GetDocumentObject(true);

                    if (doc != null)
                    {
                        if (doc.GetType().GetMember("SpendTheDocument").Length == 1)
                        {
                            try
                            {
                                object? obj = doc.GetType().InvokeMember("SpendTheDocument",
                                     BindingFlags.InvokeMethod, null, doc, new object[] { journalSelect.Current.SpendDate });

                                bool rezult = obj != null ? (bool)obj : false;

                                if (!rezult)
                                {
                                    List<Dictionary<string, object>> listRow = ФункціїДляПовідомлень.ПрочитатиПовідомленняПроПомилку();
                                    string msg = "";

                                    foreach (Dictionary<string, object> row in listRow)
                                        msg += row["Повідомлення"].ToString();

                                    CreateMessage(TypeMessage.Error, msg);

                                    //Очистка проводок документу
                                    doc.GetType().InvokeMember("ClearSpendTheDocument", BindingFlags.InvokeMethod, null, doc, new object[] { });

                                    break;
                                }
                                else
                                    CreateMessage(TypeMessage.Ok, journalSelect.Current.TypeDocument + " " + journalSelect.Current.SpendDate);
                            }
                            catch (Exception ex)
                            {
                                CreateMessage(TypeMessage.Error, ex.Message);

                                //Очистка проводок документу
                                doc.GetType().InvokeMember("ClearSpendTheDocument", BindingFlags.InvokeMethod, null, doc, new object[] { });
                            }
                        }
                    }
                }
            }

            CreateMessage(TypeMessage.None, "Готово!");

            CalculationBalancesFunc();

            Константи.Системні.ЗупинитиФоновіЗадачі_Const = false;

            ButtonSensitive(true);
        }

        #endregion

        #region CalculationBalances

        void OnCalculationBalances(object? sender, EventArgs args)
        {
            ClearMessage();

            CancellationTokenThread = new CancellationTokenSource();
            Thread thread = new Thread(new ThreadStart(CalculationBalances));
            thread.Start();
        }

        void CalculationBalances()
        {
            ButtonSensitive(false);

            Константи.Системні.ЗупинитиФоновіЗадачі_Const = true;
            CalculationBalancesFunc();
            Константи.Системні.ЗупинитиФоновіЗадачі_Const = false;

            ButtonSensitive(true);
        }

        void CalculationBalancesFunc()
        {
            //Видалити всі задачі
            Service.CalculationBalances.ClearAllTask();

            if (!CancellationTokenThread!.IsCancellationRequested)
            {
                CreateMessage(TypeMessage.Info, "Перерахунок залишків");

                CreateMessage(TypeMessage.Info, "Обчислення залишків з групуванням по днях");
                foreach (string registerAccumulation in Service.CalculationBalances.СписокДоступнихВіртуальнихРегістрів)
                {
                    if (CancellationTokenThread.IsCancellationRequested)
                        break;

                    CreateMessage(TypeMessage.Ok, " --> регістер: " + registerAccumulation);
                    Service.CalculationBalances.ОбчисленняВіртуальнихЗалишківПоВсіхДнях(registerAccumulation);
                }
            }

            if (!CancellationTokenThread.IsCancellationRequested)
            {
                CreateMessage(TypeMessage.Info, "Обновлення актуальності:");
                foreach (string registerAccumulation in Service.CalculationBalances.СписокДоступнихВіртуальнихРегістрів)
                {
                    if (CancellationTokenThread.IsCancellationRequested)
                        break;

                    CreateMessage(TypeMessage.Ok, " --> регістер: " + registerAccumulation);
                    Service.CalculationBalances.СкинутиЗначенняАктуальностіВіртуальнихЗалишківПоВсіхМісяцях(registerAccumulation);
                }
            }

            if (!CancellationTokenThread.IsCancellationRequested)
            {
                CreateMessage(TypeMessage.Info, "Обчислення залишків з групуванням по місяцях");
                Service.CalculationBalances.ОбчисленняВіртуальнихЗалишківПоМісяцях();
            }

            if (!CancellationTokenThread.IsCancellationRequested)
                CreateMessage(TypeMessage.None, "Готово!");
        }

        #endregion

        void OnStopClick(object? sender, EventArgs args)
        {
            CancellationTokenThread?.Cancel();
        }
    }
}