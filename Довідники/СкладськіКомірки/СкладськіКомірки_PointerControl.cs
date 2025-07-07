
/*     
        СкладськіКомірки_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class СкладськіКомірки_PointerControl : PointerControl
    {
        event EventHandler<СкладськіКомірки_Pointer> PointerChanged;

        public СкладськіКомірки_PointerControl()
        {
            pointer = new СкладськіКомірки_Pointer();
            WidthPresentation = 300;
            Caption = $"{СкладськіКомірки_Const.FULLNAME}:";
            PointerChanged += async (object? _, СкладськіКомірки_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        СкладськіКомірки_Pointer pointer;
        public СкладськіКомірки_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;
                PointerChanged?.Invoke(null, pointer);
            }
        }


        public СкладськіПриміщення_Pointer Власник { get; set; } = new СкладськіПриміщення_Pointer();


        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            СкладськіКомірки_ШвидкийВибір page = new СкладськіКомірки_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new СкладськіКомірки_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            page.Власник.Pointer = Власник;

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СкладськіКомірки_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
