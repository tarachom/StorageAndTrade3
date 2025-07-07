
/*     
        БанківськіРахункиОрганізацій_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class БанківськіРахункиОрганізацій_PointerControl : PointerControl
    {
        event EventHandler<БанківськіРахункиОрганізацій_Pointer> PointerChanged;

        public БанківськіРахункиОрганізацій_PointerControl()
        {
            pointer = new БанківськіРахункиОрганізацій_Pointer();
            WidthPresentation = 300;
            Caption = $"{БанківськіРахункиОрганізацій_Const.FULLNAME}:";
            PointerChanged += async (object? _, БанківськіРахункиОрганізацій_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        БанківськіРахункиОрганізацій_Pointer pointer;
        public БанківськіРахункиОрганізацій_Pointer Pointer
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
            БанківськіРахункиОрганізацій_ШвидкийВибір page = new БанківськіРахункиОрганізацій_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new БанківськіРахункиОрганізацій_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new БанківськіРахункиОрганізацій_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
