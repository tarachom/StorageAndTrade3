
/*     
        ВидиНоменклатури_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class ВидиНоменклатури_PointerControl : PointerControl
    {
        event EventHandler<ВидиНоменклатури_Pointer> PointerChanged;

        public ВидиНоменклатури_PointerControl()
        {
            pointer = new ВидиНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = $"{ВидиНоменклатури_Const.FULLNAME}:";
            PointerChanged += async (object? _, ВидиНоменклатури_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ВидиНоменклатури_Pointer pointer;
        public ВидиНоменклатури_Pointer Pointer
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
            ВидиНоменклатури_ШвидкийВибір page = new ВидиНоменклатури_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ВидиНоменклатури_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВидиНоменклатури_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
