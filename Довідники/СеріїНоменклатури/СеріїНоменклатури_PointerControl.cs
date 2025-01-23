
/*     
        СеріїНоменклатури_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class СеріїНоменклатури_PointerControl : PointerControl
    {
        event EventHandler<СеріїНоменклатури_Pointer> PointerChanged;

        public СеріїНоменклатури_PointerControl()
        {
            pointer = new СеріїНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = $"{СеріїНоменклатури_Const.FULLNAME}:";
            PointerChanged += async (object? _, СеріїНоменклатури_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        СеріїНоменклатури_Pointer pointer;
        public СеріїНоменклатури_Pointer Pointer
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
            СеріїНоменклатури_ШвидкийВибір page = new СеріїНоменклатури_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new СеріїНоменклатури_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СеріїНоменклатури_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
