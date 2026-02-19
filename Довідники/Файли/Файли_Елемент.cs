/*

        Файли_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk3;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class Файли_Елемент : ДовідникЕлемент
    {
        public Файли_Objest Елемент { get; set; } = new Файли_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };

        Label НазваФайлу = new Label() { Selectable = true };
        Label Розмір = new Label() { Selectable = true };
        Label ДатаСтворення = new Label() { Selectable = true };


        //Шлях до вибраного файлу
        string SelectFileName = "";

        Label labelFileName = new Label() { Wrap = true, Selectable = true };

        public Файли_Елемент()  
        {
            Element = Елемент;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //НазваФайлу
            CreateField(vBox, "Назва файлу:", НазваФайлу);

            //Розмір
            CreateField(vBox, "Розмір (KB):", Розмір);

            //ДатаСтворення
            CreateField(vBox, "Дата cтворення:", ДатаСтворення);
        }

        protected override void CreatePack2(Box vBox)
        {
            //Загрузити файл
            {
                Box hBox = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
                vBox.PackStart(hBox, false, false, 5);

                Button bSelectFile = new Button("Вибрати файл");
                bSelectFile.Clicked += SelectFile;
                hBox.PackStart(bSelectFile, false, false, 10);

                hBox.PackStart(labelFileName, false, false, 10);
            }

            //Вигрузити файл
            {
                Box hBox = new Box(Orientation.Horizontal, 0) { Halign = Align.Start };
                vBox.PackStart(hBox, false, false, 5);

                Button bSaveFile = new Button("Зберегти файл");
                bSaveFile.Clicked += SaveFile;
                hBox.PackStart(bSaveFile, false, false, 10);
            }
        }

        void SelectFile(object? sender, EventArgs args)
        {
            SelectFileName = "";

            FileChooserDialog fc = new FileChooserDialog("Виберіть файл для загрузки", Program.GeneralForm,
                FileChooserAction.Open, "Закрити", ResponseType.Cancel, "Вибрати", ResponseType.Accept)
            { Filter = new FileFilter() };

            fc.Filter.AddPattern("*.*");

            if (fc.Run() == (int)ResponseType.Accept)
                SelectFileName = fc.Filename;

            fc.Dispose();
            fc.Destroy();

            labelFileName.Text = SelectFileName;

            if (!string.IsNullOrEmpty(SelectFileName) && File.Exists(SelectFileName))
            {
                FileInfo fi = new FileInfo(SelectFileName);

                if (string.IsNullOrEmpty(Назва.Text))
                    Назва.Text = fi.Name;

                НазваФайлу.Text = fi.Name;
                Розмір.Text = Math.Round((decimal)(fi.Length / 1024)).ToString() + " KB";
                ДатаСтворення.Text = DateTime.Now.ToString();
            }
        }

        void SaveFile(object? sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(Елемент.НазваФайлу) || Елемент.БінарніДані.Length == 0)
                return;

            string fullPath = "";

            FileChooserDialog fc = new FileChooserDialog("Виберіть каталог для збереження файлу: " + Елемент.НазваФайлу, Program.GeneralForm,
                FileChooserAction.SelectFolder, "Закрити", ResponseType.Cancel, "Вибрати", ResponseType.Accept);

            if (fc.Run() == (int)ResponseType.Accept)
            {
                if (!string.IsNullOrEmpty(fc.CurrentFolder))
                    fullPath = System.IO.Path.Combine(fc.CurrentFolder, Елемент.НазваФайлу);
            }

            fc.Dispose();
            fc.Destroy();

            if (!string.IsNullOrEmpty(fullPath))
            {
                if (File.Exists(fullPath) && Message.Request(Program.GeneralForm, "Файл '" + Елемент.НазваФайлу + "' уже існує. Перезаписати?") == ResponseType.No)
                    return;

                try
                {
                    File.WriteAllBytes(fullPath, Елемент.БінарніДані);
                    Message.Info(Program.GeneralForm, "Файл збережений!\n\nШлях: " + fullPath);
                }
                catch (Exception ex)
                {
                    Message.Error(Program.GeneralForm, "Помилка збереження файлу! \n" + ex.Message);
                }
            }
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;

            НазваФайлу.Text = Елемент.НазваФайлу;
            Розмір.Text = Елемент.Розмір;
            ДатаСтворення.Text = Елемент.ДатаСтворення.ToString();

            SelectFileName = "";
            labelFileName.Text = "";
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;

            if (!string.IsNullOrEmpty(SelectFileName) && File.Exists(SelectFileName))
            {
                try
                {
                    Елемент.БінарніДані = File.ReadAllBytes(SelectFileName);
                }
                catch (Exception ex)
                {
                    Message.Error(Program.GeneralForm, "Помилка завантаження файлу! \n" + ex.Message);
                    return;
                }

                FileInfo fi = new FileInfo(SelectFileName);

                if (string.IsNullOrEmpty(Елемент.Назва))
                    Елемент.Назва = fi.Name;

                Елемент.НазваФайлу = fi.Name;
                Елемент.Розмір = Math.Round((decimal)(Елемент.БінарніДані.Length / 1024)).ToString() + " KB";
                Елемент.ДатаСтворення = DateTime.Now;
            }
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                if (await Елемент.Save())
                {
                    //Перечитати
                    SetValue();
                }
                return true;
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
                return false;
            }
        }
    }
}