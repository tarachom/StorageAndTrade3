
/*     
        Номенклатура_Папки_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Номенклатура_Папки_PointerControl : PointerControl
    {
        event EventHandler<Номенклатура_Папки_Pointer> PointerChanged;

        public Номенклатура_Папки_PointerControl()
        {
            pointer = new Номенклатура_Папки_Pointer();
            WidthPresentation = 300;
            Caption = $"{Номенклатура_Папки_Const.FULLNAME}:";
            PointerChanged += async (object? _, Номенклатура_Папки_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Номенклатура_Папки_Pointer pointer;
        public Номенклатура_Папки_Pointer Pointer
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
            Номенклатура_Папки_ШвидкийВибір page = new Номенклатура_Папки_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Номенклатура_Папки_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Номенклатура_Папки_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
