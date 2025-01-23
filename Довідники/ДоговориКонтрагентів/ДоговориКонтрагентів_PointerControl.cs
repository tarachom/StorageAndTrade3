
/*     
        ДоговориКонтрагентів_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    public class ДоговориКонтрагентів_PointerControl : PointerControl
    {
        event EventHandler<ДоговориКонтрагентів_Pointer> PointerChanged;

        public ДоговориКонтрагентів_PointerControl()
        {
            pointer = new ДоговориКонтрагентів_Pointer();
            WidthPresentation = 300;
            Caption = $"{ДоговориКонтрагентів_Const.FULLNAME}:";
            PointerChanged += async (object? _, ДоговориКонтрагентів_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ДоговориКонтрагентів_Pointer pointer;
        public ДоговориКонтрагентів_Pointer Pointer
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

        public Контрагенти_Pointer КонтрагентВласник { get; set; } = new Контрагенти_Pointer();

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            ДоговориКонтрагентів_ШвидкийВибір page = new ДоговориКонтрагентів_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ДоговориКонтрагентів_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            page.КонтрагентВласник.Pointer = КонтрагентВласник;

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ДоговориКонтрагентів_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
