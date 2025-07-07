/*     
        Контрагенти_Папки_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class Контрагенти_Папки_PointerControl : PointerControl
    {
        event EventHandler<Контрагенти_Папки_Pointer> PointerChanged;

        public Контрагенти_Папки_PointerControl()
        {
            pointer = new Контрагенти_Папки_Pointer();
            WidthPresentation = 300;
            Caption = $"{Контрагенти_Папки_Const.FULLNAME}:";
            PointerChanged += async (object? _, Контрагенти_Папки_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Контрагенти_Папки_Pointer pointer;
        public Контрагенти_Папки_Pointer Pointer
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
            Контрагенти_Папки_ШвидкийВибір page = new Контрагенти_Папки_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Контрагенти_Папки_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Контрагенти_Папки_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
