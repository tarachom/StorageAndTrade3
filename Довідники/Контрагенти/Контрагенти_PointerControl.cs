
/*     
        Контрагенти_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    public class Контрагенти_PointerControl : PointerControl
    {
        event EventHandler<Контрагенти_Pointer> PointerChanged;

        public Контрагенти_PointerControl()
        {
            pointer = new Контрагенти_Pointer();
            WidthPresentation = 300;
            Caption = $"{Контрагенти_Const.FULLNAME}:";
            PointerChanged += async (object? _, Контрагенти_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Контрагенти_Pointer pointer;
        public Контрагенти_Pointer Pointer
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
            Контрагенти_ШвидкийВибір page = new Контрагенти_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Контрагенти_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Контрагенти_Pointer();
            AfterSelectFunc?.Invoke();
        }

        public async ValueTask ПривязкаДоДоговору(ДоговориКонтрагентів_PointerControl Договір)
        {
            if (Договір.Pointer.IsEmpty())
            {
                ДоговориКонтрагентів_Pointer? договірКонтрагента = await ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Pointer, ТипДоговорів.ЗПостачальниками);
                if (договірКонтрагента != null) Договір.Pointer = договірКонтрагента;
            }
            else
            {
                if (Pointer.IsEmpty())
                    Договір.Pointer = new ДоговориКонтрагентів_Pointer();
                else
                {
                    //Перевірити чи змінився контрагент
                    ДоговориКонтрагентів_Objest? договориКонтрагентів_Objest = await Договір.Pointer.GetDirectoryObject();
                    if (договориКонтрагентів_Objest != null)
                        if (договориКонтрагентів_Objest.Контрагент != Pointer)
                        {
                            Договір.Pointer = new ДоговориКонтрагентів_Pointer();
                            AfterSelectFunc?.Invoke();
                        };
                }
            }
        }
    }
}
