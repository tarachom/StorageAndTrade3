/*

        Каси_PointerControl.cs
        PointerControl

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Каси_PointerControl : PointerControl
    {
        event EventHandler<Каси_Pointer> PointerChanged;

        public Каси_PointerControl()
        {
            pointer = new Каси_Pointer();
            WidthPresentation = 300;
            Caption = $"{Каси_Const.FULLNAME}:";
            PointerChanged += async (object? _, Каси_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Каси_Pointer pointer;
        public Каси_Pointer Pointer
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

            Каси_ШвидкийВибір page = new Каси_ШвидкийВибір
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Каси_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Каси_Pointer();
        }
    }
}