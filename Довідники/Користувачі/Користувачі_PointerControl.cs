
/*     
        Користувачі_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class Користувачі_PointerControl : PointerControl
    {
        event EventHandler<Користувачі_Pointer> PointerChanged;

        public Користувачі_PointerControl()
        {
            pointer = new Користувачі_Pointer();
            WidthPresentation = 300;
            Caption = $"{Користувачі_Const.FULLNAME}:";
            PointerChanged += async (object? _, Користувачі_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Користувачі_Pointer pointer;
        public Користувачі_Pointer Pointer
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

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            Користувачі_ШвидкийВибір page = new Користувачі_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Користувачі_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Користувачі_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
