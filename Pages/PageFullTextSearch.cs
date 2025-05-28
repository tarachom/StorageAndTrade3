/*

Повнотекстовий пошук

*/

using AccountingSoftware;
using GeneratedCode;

namespace StorageAndTrade
{
    class PageFullTextSearch : InterfaceGtk.PageFullTextSearch
    {
        public PageFullTextSearch() : base(Config.Kernel) { }

        protected override CompositePointerControl CreateCompositeControl(string caption, UuidAndText uuidAndText) =>
            new CompositePointerControl() { Caption = caption, Pointer = uuidAndText, ClearSensetive = false, TypeSelectSensetive = false };
    }
}