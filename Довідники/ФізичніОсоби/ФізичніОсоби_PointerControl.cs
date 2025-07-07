
/*     
        ФізичніОсоби_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class ФізичніОсоби_PointerControl : PointerControl
    {
        event EventHandler<ФізичніОсоби_Pointer> PointerChanged;

        public ФізичніОсоби_PointerControl()
        {
            pointer = new ФізичніОсоби_Pointer();
            WidthPresentation = 300;
            Caption = $"{ФізичніОсоби_Const.FULLNAME}:";
            PointerChanged += async (object? _, ФізичніОсоби_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ФізичніОсоби_Pointer pointer;
        public ФізичніОсоби_Pointer Pointer
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
            ФізичніОсоби_ШвидкийВибір page = new ФізичніОсоби_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ФізичніОсоби_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ФізичніОсоби_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
