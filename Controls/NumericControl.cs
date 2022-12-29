using Gtk;

namespace StorageAndTrade
{
    class NumericControl : HBox
    {
        Entry entryNumeric = new Entry();
        HBox hBoxInfoValid = new HBox() { WidthRequest = 16 };

        public NumericControl() : base()
        {
            PackStart(hBoxInfoValid, false, false, 1);

            entryNumeric.Changed += OnEntryNumericChanged;
            PackStart(entryNumeric, false, false, 1);
        }

        decimal mValue;
        public decimal Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                entryNumeric.Text = mValue.ToString();
            }
        }

        void ClearHBoxInfoValid()
        {
            foreach (Widget item in hBoxInfoValid.Children)
                hBoxInfoValid.Remove(item);
        }

        public bool IsValidValue()
        {
            ClearHBoxInfoValid();

            decimal value;
            if (decimal.TryParse(entryNumeric.Text, out value))
            {
                mValue = value;

                hBoxInfoValid.Add(new Image("images/16/ok.png"));
                hBoxInfoValid.ShowAll();

                return true;
            }
            else
            {
                hBoxInfoValid.Add(new Image("images/16/error.png"));
                hBoxInfoValid.ShowAll();

                return false;
            }
        }

        void OnEntryNumericChanged(object? sender, EventArgs args)
        {
            IsValidValue();
        }
    }
}