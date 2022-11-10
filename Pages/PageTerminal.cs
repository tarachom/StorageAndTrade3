using Gtk;

namespace StorageAndTrade
{
    class PageTerminal : VBox
    {
        VBox vBoxMessage = new VBox();

        public PageTerminal() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            Button bClear = new Button("Очистити");
            bClear.Clicked += OnClear;

            hBoxBotton.PackStart(bClear, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.Add(vBoxMessage);

            PackStart(scroll, true, true, 0);

            ShowAll();
        }

        public void LoadRecords()
        {
            foreach (Widget Child in vBoxMessage.Children)
                vBoxMessage.Remove(Child);

            List<Dictionary<string, object>> listRow = ФункціїДляПовідомлень.ПрочитатиПовідомленняПроПомилку();

            foreach (Dictionary<string, object> row in listRow)
                CreateMessage(row);
        }

        void CreateMessage(Dictionary<string, object> row)
        {
            HBox hBox = new HBox();
            vBoxMessage.PackStart(hBox, false, false, 10);

            hBox.PackStart(new Image("images/error.png"), false, false, 25);

            VBox vBoxInfo = new VBox();
            hBox.PackStart(vBoxInfo, false, false, 10);

            //Дата
            HBox hBoxDate = new HBox();
            hBoxDate.PackStart(new Label(row["Дата"].ToString()), false, false, 5);
            vBoxInfo.PackStart(hBoxDate, false, false, 5);

            //НазваОбєкту
            HBox hBoxObject = new HBox();
            hBoxObject.PackStart(new Label(row["НазваОбєкту"].ToString()), false, false, 5);
            vBoxInfo.PackStart(hBoxObject, false, false, 5);

            //НазваПроцесу
            HBox hBoxProces = new HBox();
            hBoxProces.PackStart(new Label(row["НазваПроцесу"].ToString()), false, false, 5);
            vBoxInfo.PackStart(hBoxProces, false, false, 5);

            //Повідомлення
            HBox hBoxMessage = new HBox();
            hBoxMessage.PackStart(new Label(" -> " + row["Повідомлення"].ToString()), false, false, 5);
            vBoxInfo.PackStart(hBoxMessage, false, false, 5);

            vBoxMessage.PackStart(new Separator(Orientation.Horizontal), false, false, 5);
        }

        void OnClear(object? sender, EventArgs args)
        {
            ФункціїДляПовідомлень.ОчиститиПовідомлення();
            LoadRecords();
        }

    }
}