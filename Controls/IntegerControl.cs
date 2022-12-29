using Gtk;

namespace StorageAndTrade
{
    class IntegerControl : HBox
    {
        Entry entryInteger = new Entry();
        HBox hBoxInfoValid = new HBox() { WidthRequest = 16 };

        public IntegerControl() : base()
        {
            PackStart(hBoxInfoValid, false, false, 1);

            entryInteger.Changed += OnEntryIntegerChanged;
            PackStart(entryInteger, false, false, 2);
        }

        int mValue;
        public int Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                entryInteger.Text = mValue.ToString();
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

            int value;
            if (int.TryParse(entryInteger.Text, out value))
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

        void OnEntryIntegerChanged(object? sender, EventArgs args)
        {
            IsValidValue();
        }
    }
}