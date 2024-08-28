/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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
using InterfaceGtk;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Файли_Елемент : ДовідникЕлемент
    {
        public Файли_Objest Файли_Objest { get; set; } = new Файли_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };

        Label НазваФайлу = new Label() { Selectable = true };
        Label Розмір = new Label() { Selectable = true };
        Label ДатаСтворення = new Label() { Selectable = true };


        //Шлях до вибраного файлу
        string SelectFileName = "";

        Label labelFileName = new Label() { Wrap = true, Selectable = true };

        public Файли_Елемент() : base() { }

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
            if (string.IsNullOrEmpty(Файли_Objest.НазваФайлу) || Файли_Objest.БінарніДані.Length == 0)
                return;

            string fullPath = "";

            FileChooserDialog fc = new FileChooserDialog("Виберіть каталог для збереження файлу: " + Файли_Objest.НазваФайлу, Program.GeneralForm,
                FileChooserAction.SelectFolder, "Закрити", ResponseType.Cancel, "Вибрати", ResponseType.Accept);

            if (fc.Run() == (int)ResponseType.Accept)
            {
                if (!string.IsNullOrEmpty(fc.CurrentFolder))
                    fullPath = System.IO.Path.Combine(fc.CurrentFolder, Файли_Objest.НазваФайлу);
            }

            fc.Dispose();
            fc.Destroy();

            if (!string.IsNullOrEmpty(fullPath))
            {
                if (File.Exists(fullPath) && Message.Request(Program.GeneralForm, "Файл '" + Файли_Objest.НазваФайлу + "' уже існує. Перезаписати?") == ResponseType.No)
                    return;

                try
                {
                    File.WriteAllBytes(fullPath, Файли_Objest.БінарніДані);
                    Message.Info(Program.GeneralForm, "Файл збережений!\n\nШлях: " + fullPath);
                }
                catch (Exception ex)
                {
                    Message.Error(Program.GeneralForm, "Помилка збереження файлу! \n" + ex.Message);
                }
            }
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            if (IsNew)
                await Файли_Objest.New();

            Код.Text = Файли_Objest.Код;
            Назва.Text = Файли_Objest.Назва;

            НазваФайлу.Text = Файли_Objest.НазваФайлу;
            Розмір.Text = Файли_Objest.Розмір;
            ДатаСтворення.Text = Файли_Objest.ДатаСтворення.ToString();

            SelectFileName = "";
            labelFileName.Text = "";
        }

        protected override void GetValue()
        {
            Файли_Objest.Код = Код.Text;
            Файли_Objest.Назва = Назва.Text;

            if (!string.IsNullOrEmpty(SelectFileName) && File.Exists(SelectFileName))
            {
                try
                {
                    Файли_Objest.БінарніДані = File.ReadAllBytes(SelectFileName);
                }
                catch (Exception ex)
                {
                    Message.Error(Program.GeneralForm, "Помилка завантаження файлу! \n" + ex.Message);
                    return;
                }

                FileInfo fi = new FileInfo(SelectFileName);

                if (string.IsNullOrEmpty(Файли_Objest.Назва))
                    Файли_Objest.Назва = fi.Name;

                Файли_Objest.НазваФайлу = fi.Name;
                Файли_Objest.Розмір = Math.Round((decimal)(Файли_Objest.БінарніДані.Length / 1024)).ToString() + " KB";
                Файли_Objest.ДатаСтворення = DateTime.Now;
            }
        }

        #endregion

        protected override async ValueTask Save()
        {
            UnigueID = Файли_Objest.UnigueID;
            Caption = Файли_Objest.Назва;

            try
            {
                if (await Файли_Objest.Save())
                    //Перечитати
                    SetValue();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Файли_Objest.GetBasis(), Caption, ex);
            }
        }
    }
}