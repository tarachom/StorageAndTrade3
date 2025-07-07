
/*
 
        СпільніФорми_ВивідПовідомленняПроПомилки.cs
        Повідомлення про помилки

*/

using AccountingSoftware;
using GeneratedCode;

namespace StorageAndTrade
{
    class СпільніФорми_ВивідПовідомленняПроПомилки : InterfaceGtk3.СпільніФорми_ВивідПовідомленняПроПомилки
    {
        public СпільніФорми_ВивідПовідомленняПроПомилки() : base(Config.Kernel) { }

        protected override CompositePointerControl CreateCompositeControl(string caption, UuidAndText uuidAndText) =>
            new CompositePointerControl() { Caption = caption, Pointer = uuidAndText, ClearSensetive = false, TypeSelectSensetive = false };
    }
}