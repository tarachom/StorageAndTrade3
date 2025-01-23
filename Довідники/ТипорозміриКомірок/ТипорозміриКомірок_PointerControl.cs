
/*     
        ТипорозміриКомірок_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class ТипорозміриКомірок_PointerControl : PointerControl
    {
        event EventHandler<ТипорозміриКомірок_Pointer> PointerChanged;

        public ТипорозміриКомірок_PointerControl()
        {
            pointer = new ТипорозміриКомірок_Pointer();
            WidthPresentation = 300;
            Caption = $"{ТипорозміриКомірок_Const.FULLNAME}:";
            PointerChanged += async (object? _, ТипорозміриКомірок_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ТипорозміриКомірок_Pointer pointer;
        public ТипорозміриКомірок_Pointer Pointer
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
            ТипорозміриКомірок_ШвидкийВибір page = new ТипорозміриКомірок_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ТипорозміриКомірок_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ТипорозміриКомірок_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
