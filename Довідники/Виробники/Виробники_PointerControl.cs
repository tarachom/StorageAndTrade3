
/*     
        Виробники_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Виробники_PointerControl : PointerControl
    {
        event EventHandler<Виробники_Pointer> PointerChanged;

        public Виробники_PointerControl()
        {
            pointer = new Виробники_Pointer();
            WidthPresentation = 300;
            Caption = $"{Виробники_Const.FULLNAME}:";
            PointerChanged += async (object? _, Виробники_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Виробники_Pointer pointer;
        public Виробники_Pointer Pointer
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
            Виробники_ШвидкийВибір page = new Виробники_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Виробники_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Виробники_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
