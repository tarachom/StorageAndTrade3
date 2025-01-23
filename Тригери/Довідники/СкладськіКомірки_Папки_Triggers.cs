
/*
    СкладськіКомірки_Папки_Triggers.cs
    Тригери для довідника СкладськіКомірки_Папки
*/

using GeneratedCode.Константи;
using AccountingSoftware;

namespace GeneratedCode.Довідники
{
    class СкладськіКомірки_Папки_Triggers
    {
        public static async ValueTask New(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            ДовідникОбєкт.Код = (++НумераціяДовідників.СкладськіКомірки_Папки_Const).ToString("D6");
            await ValueTask.FromResult(true);
        }

        public static async ValueTask Copying(СкладськіКомірки_Папки_Objest ДовідникОбєкт, СкладськіКомірки_Папки_Objest Основа)
        {
            ДовідникОбєкт.Назва += " - Копія";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask BeforeSave(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask AfterSave(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask SetDeletionLabel(СкладськіКомірки_Папки_Objest ДовідникОбєкт, bool label)
        {
            //Якщо встановлюється мітка на видалення
            if (label)
            {
                //Елементи помічаються на видалення
                {
                    СкладськіКомірки_Select select = new СкладськіКомірки_Select();
                    select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                    select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Const.DELETION_LABEL, Comparison.NOT, true));
                    await select.Select();

                    while (select.MoveNext())
                        if (select.Current != null)
                        {
                            СкладськіКомірки_Objest? Обєкт = await select.Current.GetDirectoryObject();
                            if (Обєкт != null)
                                await Обєкт.SetDeletionLabel();
                        }
                }

                //Вкладені папки помічаються на видалення
                {
                    СкладськіКомірки_Папки_Select select = new СкладськіКомірки_Папки_Select();
                    select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                    await select.Select();

                    while (select.MoveNext())
                        if (select.Current != null)
                        {
                            СкладськіКомірки_Папки_Objest? Обєкт = await select.Current.GetDirectoryObject();
                            if (Обєкт != null)
                                await Обєкт.SetDeletionLabel();
                        }
                }
            }
        }

        public static async ValueTask BeforeDelete(СкладськіКомірки_Папки_Objest ДовідникОбєкт)
        {
            //Елементи помічаються на видалення
            {
                СкладськіКомірки_Select select = new СкладськіКомірки_Select();
                select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Const.Папка, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Const.DELETION_LABEL, Comparison.NOT, true));
                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                    {
                        СкладськіКомірки_Objest? Обєкт = await select.Current.GetDirectoryObject();
                        if (Обєкт != null)
                        {
                            await Обєкт.SetDeletionLabel();

                            Обєкт.Папка = new СкладськіКомірки_Папки_Pointer();
                            await Обєкт.Save();
                        }
                    }
            }

            //Вкладені папки помічаються на видалення
            {
                СкладськіКомірки_Папки_Select select = new СкладськіКомірки_Папки_Select();
                select.QuerySelect.Where.Add(new Where(СкладськіКомірки_Папки_Const.Родич, Comparison.EQ, ДовідникОбєкт.UnigueID.UGuid));
                await select.Select();

                while (select.MoveNext())
                    if (select.Current != null)
                    {
                        СкладськіКомірки_Папки_Objest? Обєкт = await select.Current.GetDirectoryObject();
                        if (Обєкт != null)
                            await Обєкт.Delete();
                    }
            }
        }
    }
}
