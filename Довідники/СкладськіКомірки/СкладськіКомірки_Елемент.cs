
/*
        СкладськіКомірки_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class СкладськіКомірки_Елемент : ДовідникЕлемент
    {
        public СкладськіКомірки_Objest Елемент { get; init; } = new СкладськіКомірки_Objest();

        public СкладськіПриміщення_Pointer ВласникДляНового = new СкладськіПриміщення_Pointer();

        #region Fields
        Entry Назва = new Entry() { WidthRequest = 500 };
        СкладськіПриміщення_PointerControl Приміщення = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення", WidthPresentation = 500 };
        СкладськіКомірки_Папки_PointerControl Папка = new СкладськіКомірки_Папки_PointerControl() { Caption = "Папка", WidthPresentation = 500 };
        Entry Лінія = new Entry() { WidthRequest = 500 };
        Entry Позиція = new Entry() { WidthRequest = 500 };
        Entry Стелаж = new Entry() { WidthRequest = 500 };
        Entry Ярус = new Entry() { WidthRequest = 500 };
        ComboBoxText ТипСкладськоїКомірки = new ComboBoxText();
        ТипорозміриКомірок_PointerControl Типорозмір = new ТипорозміриКомірок_PointerControl() { Caption = "Типорозмір", WidthPresentation = 500 };

        #endregion

        #region TabularParts

        #endregion

        public СкладськіКомірки_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;


            foreach (var field in ПсевдонімиПерелічення.ТипиСкладськихКомірок_List())
                ТипСкладськоїКомірки.Append(field.Value.ToString(), field.Name);

        }

        protected override void CreatePack1(Box vBox)
        {

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Приміщення
            CreateField(vBox, null, Приміщення);

            // Папка
            CreateField(vBox, null, Папка);

            // Лінія
            CreateField(vBox, "Лінія:", Лінія);

            // Позиція
            CreateField(vBox, "Позиція:", Позиція);

            // Стелаж
            CreateField(vBox, "Стелаж:", Стелаж);

            // Ярус
            CreateField(vBox, "Ярус:", Ярус);

            // ТипСкладськоїКомірки
            CreateField(vBox, "Тип:", ТипСкладськоїКомірки);

            // Типорозмір
            CreateField(vBox, null, Типорозмір);

        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {

            if (IsNew)
                Елемент.Приміщення = ВласникДляНового;
            Назва.Text = Елемент.Назва;
            Приміщення.Pointer = Елемент.Приміщення;
            Папка.Pointer = Елемент.Папка;
            Лінія.Text = Елемент.Лінія;
            Позиція.Text = Елемент.Позиція;
            Стелаж.Text = Елемент.Стелаж;
            Ярус.Text = Елемент.Ярус;
            ТипСкладськоїКомірки.ActiveId = Елемент.ТипСкладськоїКомірки.ToString(); if (ТипСкладськоїКомірки.Active == -1) ТипСкладськоїКомірки.Active = 0;
            Типорозмір.Pointer = Елемент.Типорозмір;

        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;
            Елемент.Приміщення = Приміщення.Pointer;
            Елемент.Папка = Папка.Pointer;
            Елемент.Лінія = Лінія.Text;
            Елемент.Позиція = Позиція.Text;
            Елемент.Стелаж = Стелаж.Text;
            Елемент.Ярус = Ярус.Text;
            if (ТипСкладськоїКомірки.Active != -1) Елемент.ТипСкладськоїКомірки = Enum.Parse<ТипиСкладськихКомірок>(ТипСкладськоїКомірки.ActiveId);
            Елемент.Типорозмір = Типорозмір.Pointer;

        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSaved = false;
            try
            {
                if (await Елемент.Save())
                {
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
