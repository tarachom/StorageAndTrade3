
/*     
        ПартіяТоварівКомпозит_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_PointerControl : PointerControl
    {
        event EventHandler<ПартіяТоварівКомпозит_Pointer> PointerChanged;

        public ПартіяТоварівКомпозит_PointerControl()
        {
            pointer = new ПартіяТоварівКомпозит_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПартіяТоварівКомпозит_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПартіяТоварівКомпозит_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПартіяТоварівКомпозит_Pointer pointer;
        public ПартіяТоварівКомпозит_Pointer Pointer
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
            ПартіяТоварівКомпозит_ШвидкийВибір page = new ПартіяТоварівКомпозит_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ПартіяТоварівКомпозит_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПартіяТоварівКомпозит_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
