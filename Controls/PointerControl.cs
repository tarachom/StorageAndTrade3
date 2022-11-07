using Gtk;

namespace StorageAndTrade
{
    abstract class PointerControl : HBox
    {
        Label labelCaption = new Label();
        Entry entryText = new Entry();

        public PointerControl() : base()
        {
            PackStart(labelCaption, false, false, 5);
            PackStart(entryText, false, false, 0);

            Button bOpen = new Button(new Image("find.png"));
            bOpen.Clicked += OpenSelect;

            Button bClear = new Button();
            bClear.Clicked += OnClear;

            PackStart(bOpen, false, false, 2);
            PackStart(bClear, false, false, 2);
        }

        protected virtual void OpenSelect(object? sender, EventArgs args) { }
        protected virtual void OnClear(object? sender, EventArgs args) { }

        /// <summary>
        /// Функція яка викликається перед відкриттям вибору
        /// </summary>
        public System.Action? BeforeClickOpenFunc { get; set; }

        /// <summary>
        /// Функція яка викликається після вибору.
        /// </summary>
        public System.Action? AfterSelectFunc { get; set; }

        public string Caption
        {
            get
            {
                return labelCaption.Text;
            }
            set
            {
                labelCaption.Text = value;
            }
        }

        public int WidthPresentation
        {
            get
            {
                return entryText.WidthRequest;
            }
            set
            {
                entryText.WidthRequest = value;
            }
        }

        public System.Action? Select { get; set; }

        protected string Presentation
        {
            get
            {
                return entryText.Text;
            }
            set
            {
                entryText.Text = value;
            }
        }
    }
}