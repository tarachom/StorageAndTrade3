
/*     
        БанківськіРахункиКонтрагентів_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class БанківськіРахункиКонтрагентів_PointerControl : PointerControl
    {
        event EventHandler<БанківськіРахункиКонтрагентів_Pointer> PointerChanged;

        public БанківськіРахункиКонтрагентів_PointerControl()
        {
            pointer = new БанківськіРахункиКонтрагентів_Pointer();
            WidthPresentation = 300;
            Caption = $"{БанківськіРахункиКонтрагентів_Const.FULLNAME}:";
            PointerChanged += async (object? _, БанківськіРахункиКонтрагентів_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        БанківськіРахункиКонтрагентів_Pointer pointer;
        public БанківськіРахункиКонтрагентів_Pointer Pointer
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
            БанківськіРахункиКонтрагентів_ШвидкийВибір page = new БанківськіРахункиКонтрагентів_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new БанківськіРахункиКонтрагентів_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new БанківськіРахункиКонтрагентів_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
