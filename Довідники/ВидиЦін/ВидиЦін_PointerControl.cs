
/*     
        ВидиЦін_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ВидиЦін_PointerControl : PointerControl
    {
        event EventHandler<ВидиЦін_Pointer> PointerChanged;

        public ВидиЦін_PointerControl()
        {
            pointer = new ВидиЦін_Pointer();
            WidthPresentation = 300;
            Caption = $"{ВидиЦін_Const.FULLNAME}:";
            PointerChanged += async (object? _, ВидиЦін_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ВидиЦін_Pointer pointer;
        public ВидиЦін_Pointer Pointer
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
            ВидиЦін_ШвидкийВибір page = new ВидиЦін_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ВидиЦін_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВидиЦін_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
