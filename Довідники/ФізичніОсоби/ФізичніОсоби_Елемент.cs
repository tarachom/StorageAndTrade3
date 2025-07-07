
/*
        ФізичніОсоби_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ФізичніОсоби_Елемент : ДовідникЕлемент
    {
        public ФізичніОсоби_Objest Елемент { get; set; } = new ФізичніОсоби_Objest();

        #region Fields
        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        DateTimeControl ДатаНародження = new DateTimeControl() { OnlyDate = true };
        ComboBoxText Стать = new ComboBoxText();
        Entry ІПН = new Entry() { WidthRequest = 200 };

        #endregion

        #region TabularParts

        // Таблична частина "Контакти" 
        ФізичніОсоби_ТабличнаЧастина_Контакти Контакти = new ФізичніОсоби_ТабличнаЧастина_Контакти() { HeightRequest = 300 };

        #endregion

        public ФізичніОсоби_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;


            foreach (var field in ПсевдонімиПерелічення.СтатьФізичноїОсоби_List())
                Стать.Append(field.Value.ToString(), field.Name);

        }

        protected override void CreatePack1(Box vBox)
        {

            // Код
            CreateField(vBox, "Код:", Код);

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // ДатаНародження
            CreateField(vBox, "Дата народження:", ДатаНародження);

            // Стать
            CreateField(vBox, "Стать:", Стать);

            // ІПН
            CreateField(vBox, "ІПН:", ІПН);
        }

        protected override void CreatePack2(Box vBox)
        {
            // Таблична частина "Контакти" 
            CreateTablePart(vBox, "Контакти:", Контакти);
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            ДатаНародження.Value = Елемент.ДатаНародження;
            Стать.ActiveId = Елемент.Стать.ToString(); if (Стать.Active == -1) Стать.Active = 0;
            ІПН.Text = Елемент.ІПН;

            // Таблична частина "Контакти"
            Контакти.ЕлементВласник = Елемент;
            await Контакти.LoadRecords();
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.ДатаНародження = ДатаНародження.Value;
            if (Стать.Active != -1) Елемент.Стать = Enum.Parse<СтатьФізичноїОсоби>(Стать.ActiveId);
            Елемент.ІПН = ІПН.Text;

        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSaved = false;
            try
            {
                if (await Елемент.Save())
                {
                    await Контакти.SaveRecords(); // Таблична частина "Контакти"

                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }
            return isSaved;
        }
    }
}
