
/*
        Обробка_ЗавантаженняБанків.cs
*/

using Gtk;
using InterfaceGtk;

using System.Text;

using AccountingSoftware;
using Константи = GeneratedCode.Константи;
using GeneratedCode.Довідники;

using System.Xml.XPath;

namespace StorageAndTrade
{
    class Обробка_ЗавантаженняБанків : Обробка
    {
        #region Fields

        Button bDownload;
        Button bStop;
        ScrolledWindow scrollMessage;
        Box vBoxMessage;
        CancellationTokenSource? CancellationTokenThread { get; set; }

        enum TypeMessage
        {
            Ok,
            Error,
            Info,
            None
        }

        #endregion

        public Обробка_ЗавантаженняБанків() 
        {
            bDownload = new Button("Завантаження");
            bDownload.Clicked += OnDownload;

            HBoxTop.PackStart(bDownload, false, false, 10);

            bStop = new Button("Зупинити") { Sensitive = false };
            bStop.Clicked += OnStopClick;

            HBoxTop.PackStart(bStop, false, false, 10);

            scrollMessage = new ScrolledWindow() { ShadowType = ShadowType.In };
            scrollMessage.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollMessage.Add(vBoxMessage = new Box(Orientation.Vertical, 0));

            PackStart(scrollMessage, true, true, 0);

            ShowAll();
        }

        /// <summary>
        /// Функція яка викликається після завершення завантаження у фоновому режимі
        /// </summary>
        public System.Action? CallBack_EndBackgroundWork { get; set; }

        public async void OnDownload(object? sender, EventArgs args)
        {
            CancellationTokenThread = new CancellationTokenSource();
            await DownloadExCurr();
        }

        async ValueTask DownloadExCurr()
        {
            ButtonSensitive(false);
            ClearMessage();

            bool isOK = false;

            string link = Константи.ЗавантаженняДанихІзСайтів.ЗавантаженняСпискуБанків_Const;

            if (string.IsNullOrEmpty(link))
            {
                //За замовчуванням
                link = "https://bank.gov.ua/NBU_BankInfo/get_data_branch_glbank";
            }

            CreateMessage(TypeMessage.Info, $"Завантаження ХМЛ файлу");
            CreateMessage(TypeMessage.None, " --> " + link);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1251");

            XPathDocument xPathDoc;
            XPathNavigator? xPathDocNavigator = null;
            try
            {
                xPathDoc = new XPathDocument(link);
                xPathDocNavigator = xPathDoc.CreateNavigator();

                isOK = true;

                CreateMessage(TypeMessage.Ok, "OK");
            }
            catch (Exception ex)
            {
                CreateMessage(TypeMessage.Ok, "Помилка завантаження або аналізу ХМЛ файлу: " + ex.Message);
            }

            if (isOK)
            {
                Банки_Select банки_Select = new Банки_Select();

                //Помітка всіх елементів на видалення
                банки_Select.QuerySelect.Where.Add(new Where(Банки_Const.DELETION_LABEL, Comparison.NOT, true));
                if (await банки_Select.Select())
                    while (банки_Select.MoveNext())
                        if (банки_Select.Current != null) await банки_Select.Current.SetDeletionLabel();

                банки_Select.QuerySelect.Where.Clear();

                XPathNodeIterator? Записи = xPathDocNavigator?.Select("/BANKBRANCH/ROW");
                while (Записи!.MoveNext())
                {
                    if (CancellationTokenThread!.IsCancellationRequested)
                        break;

                    XPathNavigator? current = Записи?.Current;
                    if (current == null)
                        break;

                    string КодМФО = current.SelectSingleNode("GLMFO")?.Value ?? "";

                    if (string.IsNullOrEmpty(КодМФО))
                    {
                        CreateMessage(TypeMessage.Error, "Відсутній КодМФО");
                        break;
                    }

                    string НазваГоловноїУстановиАнг = current.SelectSingleNode("NAME_E")?.Value ?? "";
                    string КодЄДРПОУ = current.SelectSingleNode("KOD_EDRPOU")?.Value ?? "";
                    string Назва = current.SelectSingleNode("SHORTNAME")?.Value ?? "";
                    string ПовнаНазва = current.SelectSingleNode("FULLNAME")?.Value ?? "";
                    string УнікальнийКодБанку = current.SelectSingleNode("NKB")?.Value ?? "";
                    string КодОбластіОпераційноїДіяльності = current.SelectSingleNode("KU")?.Value ?? "";
                    string НазваОбластіОпераційноїДіяльності = current.SelectSingleNode("N_OBL")?.Value ?? "";
                    string КодОбластіЗгідноСтатуту = current.SelectSingleNode("OBL_UR")?.Value ?? "";
                    string НазваОбластіЗгідноСтатуту = current.SelectSingleNode("N_OBL_UR")?.Value ?? "";
                    string ПоштовийІндекс = current.SelectSingleNode("P_IND")?.Value ?? "";
                    string ТипНаселеногоПункту = current.SelectSingleNode("TNP")?.Value ?? "";
                    string НазваНаселеногоПункту = current.SelectSingleNode("NP")?.Value ?? "";
                    string Адреса = current.SelectSingleNode("ADRESS")?.Value ?? "";
                    string КодТелефонногоЗвязку = current.SelectSingleNode("KODT")?.Value ?? "";
                    string НомерТелефону = current.SelectSingleNode("TELEFON")?.Value ?? "";
                    string ЧисловийКодСтануУстанови = current.SelectSingleNode("KSTAN")?.Value ?? "";
                    string НазваСтануУстанови = current.SelectSingleNode("N_STAN")?.Value ?? "";
                    string ДатаЗміниСтану = current.SelectSingleNode("D_STAN")?.Value ?? "";
                    string ДатаВідкриттяУстанови = current.SelectSingleNode("D_OPEN")?.Value ?? "";
                    string ДатаЗакриттяУстанови = current.SelectSingleNode("D_CLOSE")?.Value ?? "";
                    string КодНБУ = current.SelectSingleNode("IDNBU")?.Value ?? "";
                    string НомерЛіцензії = current.SelectSingleNode("NUM_LIC")?.Value ?? "";
                    string ДатаЛіцензії = current.SelectSingleNode("DT_GRAND_LIC")?.Value ?? "";
                    string КодСтатусу = current.SelectSingleNode("PR_LIC")?.Value ?? "";
                    string Статус = current.SelectSingleNode("N_PR_LIC")?.Value ?? "";
                    string ДатаЗапису = current.SelectSingleNode("DT_LIC")?.Value ?? "";

                    Банки_Pointer банки_Pointer = await банки_Select.FindByField(Банки_Const.КодМФО, КодМФО);
                    Банки_Objest? банки_Objest;

                    if (банки_Pointer.IsEmpty())
                    {
                        банки_Objest = new Банки_Objest();
                        await банки_Objest.New();
                        банки_Objest.КодМФО = КодМФО;

                        CreateMessage(TypeMessage.Ok, $"Додано новий елемент довідника Банки: {Назва}");
                    }
                    else
                    {
                        банки_Objest = await банки_Pointer.GetDirectoryObject();
                        CreateMessage(TypeMessage.Info, $"Знайдено банк {Назва} з кодом МФО: {КодМФО}");
                    }

                    if (банки_Objest != null)
                    {
                        банки_Objest.НазваГоловноїУстановиАнг = НазваГоловноїУстановиАнг;
                        банки_Objest.КодЄДРПОУ = КодЄДРПОУ;
                        банки_Objest.Назва = Назва;
                        банки_Objest.ПовнаНазва = ПовнаНазва;
                        банки_Objest.УнікальнийКодБанку = УнікальнийКодБанку;
                        банки_Objest.КодОбластіОпераційноїДіяльності = КодОбластіОпераційноїДіяльності;
                        банки_Objest.НазваОбластіОпераційноїДіяльності = НазваОбластіОпераційноїДіяльності;
                        банки_Objest.КодОбластіЗгідноСтатуту = КодОбластіЗгідноСтатуту;
                        банки_Objest.НазваОбластіЗгідноСтатуту = НазваОбластіЗгідноСтатуту;
                        банки_Objest.ПоштовийІндекс = ПоштовийІндекс;
                        банки_Objest.ТипНаселеногоПункту = ТипНаселеногоПункту;
                        банки_Objest.НазваНаселеногоПункту = НазваНаселеногоПункту;
                        банки_Objest.Адреса = Адреса;
                        банки_Objest.КодТелефонногоЗвязку = КодТелефонногоЗвязку;
                        банки_Objest.НомерТелефону = НомерТелефону;
                        банки_Objest.ЧисловийКодСтануУстанови = ЧисловийКодСтануУстанови;
                        банки_Objest.НазваСтануУстанови = НазваСтануУстанови;
                        банки_Objest.ДатаЗміниСтану = ДатаЗміниСтану;
                        банки_Objest.ДатаВідкриттяУстанови = ДатаВідкриттяУстанови;
                        банки_Objest.ДатаЗакриттяУстанови = ДатаЗакриттяУстанови;
                        банки_Objest.КодНБУ = КодНБУ;
                        банки_Objest.НомерЛіцензії = НомерЛіцензії;
                        банки_Objest.ДатаЛіцензії = ДатаЛіцензії;
                        банки_Objest.КодСтатусу = КодСтатусу;
                        банки_Objest.Статус = Статус;
                        банки_Objest.ДатаЗапису = ДатаЗапису;

                        await банки_Objest.Save();

                        //Зняти помітку на видалення
                        if (банки_Objest.DeletionLabel)
                            await банки_Objest.SetDeletionLabel(false);
                    }
                }
            }

            ButtonSensitive(true);

            CreateMessage(TypeMessage.None, "\n\n\nГотово!\n\n\n");
            await Task.Delay(1000);
            CreateMessage(TypeMessage.None, "\n\n\n\n");
        }

        void ButtonSensitive(bool sensitive)
        {
            bDownload.Sensitive = sensitive;
            bStop.Sensitive = !sensitive;
        }

        void CreateMessage(TypeMessage typeMsg, string message)
        {
            Box hBoxInfo = new Box(Orientation.Horizontal, 0);
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