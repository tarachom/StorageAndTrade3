
/*     
        ПакуванняОдиниціВиміру_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ПакуванняОдиниціВиміру_PointerControl : PointerControl
    {
        event EventHandler<ПакуванняОдиниціВиміру_Pointer> PointerChanged;

        public ПакуванняОдиниціВиміру_PointerControl()
        {
            pointer = new ПакуванняОдиниціВиміру_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПакуванняОдиниціВиміру_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПакуванняОдиниціВиміру_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПакуванняОдиниціВиміру_Pointer pointer;
        public ПакуванняОдиниціВиміру_Pointer Pointer
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
            ПакуванняОдиниціВиміру_ШвидкийВибір page = new ПакуванняОдиниціВиміру_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ПакуванняОдиниціВиміру_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПакуванняОдиниціВиміру_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
