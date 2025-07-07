
/*     
        ВидиЗапасів_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class ВидиЗапасів_PointerControl : PointerControl
    {
        event EventHandler<ВидиЗапасів_Pointer> PointerChanged;

        public ВидиЗапасів_PointerControl()
        {
            pointer = new ВидиЗапасів_Pointer();
            WidthPresentation = 300;
            Caption = $"{ВидиЗапасів_Const.FULLNAME}:";
            PointerChanged += async (object? _, ВидиЗапасів_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ВидиЗапасів_Pointer pointer;
        public ВидиЗапасів_Pointer Pointer
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
            ВидиЗапасів_ШвидкийВибір page = new ВидиЗапасів_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ВидиЗапасів_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВидиЗапасів_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
