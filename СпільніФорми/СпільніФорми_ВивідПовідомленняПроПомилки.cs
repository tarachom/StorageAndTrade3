/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

/*
 
Повідомлення про помилки

*/

using Gtk;
using AccountingSoftware;

namespace StorageAndTrade
{
    class СпільніФорми_ВивідПовідомленняПроПомилки : InterfaceGtk.СпільніФорми_ВивідПовідомленняПроПомилки
    {
        protected override async ValueTask<SelectRequest_Record> ПрочитатиПовідомленняПроПомилки()
        {
            return await new ФункціїДляПовідомлень().ПрочитатиПовідомленняПроПомилки();
        }

        protected override async ValueTask ОчиститиВсіПовідомлення()
        {
            await new ФункціїДляПовідомлень().ОчиститиВсіПовідомлення();
        }

        protected override Widget СтворитиВибір(UuidAndText uuidAndText)
        {
            return new CompositePointerControl { Pointer = uuidAndText, Caption = "" };
        }
    }

    class СпільніФорми_ВивідПовідомленняПроПомилки_ШвидкийВивід : InterfaceGtk.СпільніФорми_ВивідПовідомленняПроПомилки_ШвидкийВивід
    {
        protected override async ValueTask<SelectRequest_Record> ПрочитатиПовідомленняПроПомилки(UnigueID? ВідбірПоОбєкту = null, int? limit = null)
        {
            return await new ФункціїДляПовідомлень().ПрочитатиПовідомленняПроПомилки(ВідбірПоОбєкту, limit);
        }
    }
}