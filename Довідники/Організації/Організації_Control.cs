using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Організації_Control : DirectoryControl
    {
        public Організації_Control()
        {
            directoryPointer = new Організації_Pointer();
        }

        Організації_Pointer directoryPointer;
        public Організації_Pointer DirectoryPointer
        {
            get
            {
                return directoryPointer;
            }
            set
            {
                directoryPointer = value;

                if (directoryPointer != null)
                    Presentation = directoryPointer.GetPresentation();
                else
                    Presentation = "";
            }
        }

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            GeneralForm?.CreateNotebookPage("Вибір - Довідник: Організації", () =>
            {
                Організації page = new Організації
                {
                    GeneralForm = GeneralForm
                };

                page.DirectoryPointerItem = DirectoryPointer;
                
                page.CallBack_OnSelectPointer = (Організації_Pointer selectPointer) =>
                {
                    DirectoryPointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }
    }
}