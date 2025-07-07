
/*     
        КраїниСвіту_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class КраїниСвіту_PointerControl : PointerControl
    {
        event EventHandler<КраїниСвіту_Pointer> PointerChanged;

        public КраїниСвіту_PointerControl()
        {
            pointer = new КраїниСвіту_Pointer();
            WidthPresentation = 300;
            Caption = $"{КраїниСвіту_Const.FULLNAME}:";
            PointerChanged += async (object? _, КраїниСвіту_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        КраїниСвіту_Pointer pointer;
        public КраїниСвіту_Pointer Pointer
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
            КраїниСвіту_ШвидкийВибір page = new КраїниСвіту_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new КраїниСвіту_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new КраїниСвіту_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
