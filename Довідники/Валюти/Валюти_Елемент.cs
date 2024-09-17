/*
        Валюти_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Валюти_Елемент : ДовідникЕлемент
    {
        public Валюти_Objest Елемент { get; set; } = new Валюти_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry КороткаНазва = new Entry() { WidthRequest = 500 };
        Entry Код_R030 = new Entry() { WidthRequest = 100 };
        CheckButton ВиводитиКурсНаСтартову = new CheckButton("Виводити курс на стартову");

        public Валюти_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            //Код
            CreateField(vBox, "Код:", Код);

            //Назва
            CreateField(vBox, "Назва:", Назва);

            //КороткаНазва
            CreateField(vBox, "Коротка назва:", КороткаНазва);

            //Код_R030
            CreateField(vBox, "Код R030:", Код_R030);

            //ВиводитиКурсНаСтартову
            CreateField(vBox, null, ВиводитиКурсНаСтартову);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            КороткаНазва.Text = Елемент.КороткаНазва;
            Код_R030.Text = Елемент.Код_R030;
            ВиводитиКурсНаСтартову.Active = Елемент.ВиводитиКурсНаСтартову;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.КороткаНазва = КороткаНазва.Text;
            Елемент.Код_R030 = Код_R030.Text;
            Елемент.ВиводитиКурсНаСтартову = ВиводитиКурсНаСтартову.Active;
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                return await Елемент.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
                return false;
            }
        }
    }
}