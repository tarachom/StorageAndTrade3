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
        public ФункціїДляДинамічногоВідкриття() : base(Config.NamespaceProgram, Config.NamespaceCodeGeneration) { }

        protected override void CreateNotebookPage(string tabName, Func<Widget>? pageWidget)
        {
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, tabName, pageWidget);
        }
    }
}