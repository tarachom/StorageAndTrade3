
/*     
        Номенклатура_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    public class Номенклатура_PointerControl : PointerControl
    {
        event EventHandler<Номенклатура_Pointer> PointerChanged;

        public Номенклатура_PointerControl()
        {
            pointer = new Номенклатура_Pointer();
            WidthPresentation = 300;
            Caption = $"{Номенклатура_Const.FULLNAME}:";
            PointerChanged += async (object? _, Номенклатура_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Номенклатура_Pointer pointer;
        public Номенклатура_Pointer Pointer
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
            Номенклатура_ШвидкийВибір page = new Номенклатура_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Номенклатура_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Номенклатура_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
