
/*     
        Блокнот_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Блокнот_PointerControl : PointerControl
    {
        event EventHandler<Блокнот_Pointer> PointerChanged;

        public Блокнот_PointerControl()
        {
            pointer = new Блокнот_Pointer();
            WidthPresentation = 300;
            Caption = $"{Блокнот_Const.FULLNAME}:";
            PointerChanged += async (object? _, Блокнот_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Блокнот_Pointer pointer;
        public Блокнот_Pointer Pointer
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
            Блокнот_ШвидкийВибір page = new Блокнот_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Блокнот_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Блокнот_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
