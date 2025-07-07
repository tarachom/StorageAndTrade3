/*

Динамічне відкриття журналів

*/

using Gtk;
using InterfaceGtk3;
using GeneratedCode;

namespace StorageAndTrade
{
    class ФункціїДляДинамічногоВідкриття : InterfaceGtk3.ФункціїДляДинамічногоВідкриття
    {
        public ФункціїДляДинамічногоВідкриття() : base(Config.NameSpageProgram, Config.NameSpageCodeGeneration) { }

        protected override void CreateNotebookPage(string tabName, Func<Widget>? pageWidget)
        {
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, tabName, pageWidget);
        }
    }
}