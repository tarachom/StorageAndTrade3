
/*     
        ЗбереженіЗвіти_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ЗбереженіЗвіти_PointerControl : PointerControl
    {
        event EventHandler<ЗбереженіЗвіти_Pointer> PointerChanged;

        public ЗбереженіЗвіти_PointerControl()
        {
            pointer = new ЗбереженіЗвіти_Pointer();
            WidthPresentation = 300;
            Caption = $"{ЗбереженіЗвіти_Const.FULLNAME}:";
            PointerChanged += async (object? _, ЗбереженіЗвіти_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ЗбереженіЗвіти_Pointer pointer;
        public ЗбереженіЗвіти_Pointer Pointer
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
            ЗбереженіЗвіти_ШвидкийВибір page = new ЗбереженіЗвіти_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ЗбереженіЗвіти_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗбереженіЗвіти_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
