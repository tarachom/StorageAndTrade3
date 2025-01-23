
/*     
        ВидиЦінПостачальників_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class ВидиЦінПостачальників_PointerControl : PointerControl
    {
        event EventHandler<ВидиЦінПостачальників_Pointer> PointerChanged;

        public ВидиЦінПостачальників_PointerControl()
        {
            pointer = new ВидиЦінПостачальників_Pointer();
            WidthPresentation = 300;
            Caption = $"{ВидиЦінПостачальників_Const.FULLNAME}:";
            PointerChanged += async (object? _, ВидиЦінПостачальників_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ВидиЦінПостачальників_Pointer pointer;
        public ВидиЦінПостачальників_Pointer Pointer
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
            ВидиЦінПостачальників_ШвидкийВибір page = new ВидиЦінПостачальників_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ВидиЦінПостачальників_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВидиЦінПостачальників_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
