/*

        СтаттяРухуКоштів_Елемент.cs
        Елемент

*/

using Gtk;
using InterfaceGtk;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class СтаттяРухуКоштів_Елемент : ДовідникЕлемент
    {
        public СтаттяРухуКоштів_Objest Елемент { get; set; } = new СтаттяРухуКоштів_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry КореспондуючийРахунок = new Entry() { WidthRequest = 200 };
        ComboBoxText ВидРухуКоштів = new ComboBoxText();
        TextView Опис = new TextView();

        #endregion

        public СтаттяРухуКоштів_Елемент() 
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

            //КореспондуючийРахунок
            CreateField(vBox, "Кореспондентський рахунок:", КореспондуючийРахунок);

            //ВидРухуКоштів
            foreach (var field in ПсевдонімиПерелічення.ВидиРухуКоштів_List())
                ВидРухуКоштів.Append(field.Value.ToString(), field.Name);

            CreateField(vBox, "Вид руху коштів:", ВидРухуКоштів);

            //Опис
            CreateFieldView(vBox, "Опис:", Опис, 500, 200);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Елемент.ВидРухуКоштів = ВидиРухуКоштів.ОплатаОборотнихАктивів;

            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            КореспондуючийРахунок.Text = Елемент.КореспондуючийРахунок;
            ВидРухуКоштів.ActiveId = Елемент.ВидРухуКоштів.ToString();
            Опис.Buffer.Text = Елемент.Опис;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.КореспондуючийРахунок = КореспондуючийРахунок.Text;
            Елемент.ВидРухуКоштів = Enum.Parse<ВидиРухуКоштів>(ВидРухуКоштів.ActiveId);
            Елемент.Опис = Опис.Buffer.Text;
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