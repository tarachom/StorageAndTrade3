
/*     
        Склади_Папки_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Склади_Папки_PointerControl : PointerControl
    {
        event EventHandler<Склади_Папки_Pointer> PointerChanged;

        public Склади_Папки_PointerControl()
        {
            pointer = new Склади_Папки_Pointer();
            WidthPresentation = 300;
            Caption = $"{Склади_Папки_Const.FULLNAME}:";
            PointerChanged += async (object? _, Склади_Папки_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Склади_Папки_Pointer pointer;
        public Склади_Папки_Pointer Pointer
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
            Склади_Папки_ШвидкийВибір page = new Склади_Папки_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Склади_Папки_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Склади_Папки_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
