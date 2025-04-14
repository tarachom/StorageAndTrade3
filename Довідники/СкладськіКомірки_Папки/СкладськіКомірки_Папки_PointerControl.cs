
/*     
        СкладськіКомірки_Папки_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class СкладськіКомірки_Папки_PointerControl : PointerControl
    {
        event EventHandler<СкладськіКомірки_Папки_Pointer> PointerChanged;

        public СкладськіКомірки_Папки_PointerControl()
        {
            pointer = new СкладськіКомірки_Папки_Pointer();
            WidthPresentation = 300;
            Caption = $"{СкладськіКомірки_Папки_Const.FULLNAME}:";
            PointerChanged += async (object? _, СкладськіКомірки_Папки_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        СкладськіКомірки_Папки_Pointer pointer;
        public СкладськіКомірки_Папки_Pointer Pointer
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
            СкладськіКомірки_Папки_ШвидкийВибір page = new СкладськіКомірки_Папки_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new СкладськіКомірки_Папки_Pointer(selectPointer);
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
            Pointer = new СкладськіКомірки_Папки_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
