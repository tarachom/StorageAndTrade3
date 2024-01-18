<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes" />

  <xsl:template name="License">
/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
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
  </xsl:template>

  <xsl:template match="/">
    <xsl:call-template name="License" />
/*
 *
 * Конфігурації "<xsl:value-of select="Configuration/Name"/>"
 * Автор <xsl:value-of select="Configuration/Author"/>
 * Дата конфігурації: <xsl:value-of select="Configuration/DateTimeSave"/>
 *
 *
 * Цей код згенерований в Конфігураторі 3. Шаблон Gtk.xslt
 *
 */

using Gtk;
using AccountingSoftware;

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Іконки
{
    public static class ДляТабличногоСписку
    {
        public static Gdk.Pixbuf Normal = new Gdk.Pixbuf($"{AppContext.BaseDirectory}images/doc.png");
        public static Gdk.Pixbuf Delete = new Gdk.Pixbuf($"{AppContext.BaseDirectory}images/doc_delete.png");
    }

    public static class ДляДерева
    {
        public static Gdk.Pixbuf Normal = new Gdk.Pixbuf($"{AppContext.BaseDirectory}images/folder.png");
        public static Gdk.Pixbuf Delete = new Gdk.Pixbuf($"{AppContext.BaseDirectory}images/folder_delete.png");
    }
}

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>
{
    public abstract class ТабличнийСписок
    {
        public static void ДодатиВідбір(TreeView treeView, Where where, bool clear_all_before_add = false)
        {
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", new List&lt;Where&gt;() { where });
            else
            {
                if (clear_all_before_add)
                    treeView.Data["Where"] = new List&lt;Where&gt;() { where };
                else
                {
                    object? value = treeView.Data["Where"];
                    if (value == null)
                        treeView.Data["Where"] = new List&lt;Where&gt;() { where };
                    else
                        ((List&lt;Where&gt;)value).Add(where);
                }
            }
        }

        public static void ДодатиВідбір(TreeView treeView, List&lt;Where&gt; where, bool clear_all_before_add = false)
        {
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", where);
            else
            {
                if (clear_all_before_add)
                    treeView.Data["Where"] = where;
                else
                {
                    object? value = treeView.Data["Where"];
                    if (value == null)
                        treeView.Data["Where"] = where;
                    else
                    {
                        var list = (List&lt;Where&gt;)value;
                        foreach (Where item in where)
                            list.Add(item);
                    }
                }
            }
        }

        public static void ОчиститиВідбір(TreeView treeView)
        {
            if (treeView.Data.ContainsKey("Where"))
                treeView.Data["Where"] = null;
        }
    }
}

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Довідники.ТабличніСписки
{
    <xsl:for-each select="Configuration/Directories/Directory">
      <xsl:variable name="DirectoryName" select="Name"/>
    #region DIRECTORY "<xsl:value-of select="$DirectoryName"/>"
    
      <!-- ТАБЛИЦЯ -->
      <xsl:for-each select="TabularLists/TabularList[IsTree = '0']">
        <xsl:variable name="TabularListName" select="Name"/>
    /* ТАБЛИЦЯ */
    public class <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        <xsl:for-each select="Fields/AdditionalField">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? Іконки.ДляТабличногоСписку.Delete : Іконки.ДляТабличногоСписку.Normal,
                ID,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ <xsl:value-of select="Name"/>,
                </xsl:for-each>

                <xsl:for-each select="Fields/AdditionalField">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ <xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string),  
                </xsl:for-each>
                
                <xsl:for-each select="Fields/AdditionalField">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string), 
                </xsl:for-each>
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            <xsl:for-each select="Fields/Field">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text>
              <xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text>
              <xsl:value-of select="position() + 1"/>
              <xsl:text>) { MinWidth = 20, Resizable = true, SortColumnId = </xsl:text>
              <xsl:value-of select="position() + 1"/>
              <xsl:if test="Size != '0'">
                <xsl:text>, FixedWidth = </xsl:text>
                <xsl:value-of select="Size"/>
              </xsl:if>
              <xsl:text> } )</xsl:text>; /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>

            /* Додаткові поля */
            <xsl:variable name="CountField" select="count(Fields/Field) + 1"/>
            <xsl:for-each select="Fields/AdditionalField">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text>
              <xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text>
              <xsl:value-of select="$CountField + position()"/>
              <xsl:text>) { MinWidth = 20, Resizable = true, SortColumnId = </xsl:text>
              <xsl:value-of select="$CountField + position()"/>
              <xsl:if test="Size != '0'">
                <xsl:text>, FixedWidth = </xsl:text>
                <xsl:value-of select="Size"/>
              </xsl:if>
              <xsl:text> } )</xsl:text>; /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.<xsl:value-of select="$DirectoryName"/>_Select <xsl:value-of select="$DirectoryName"/>_Select = new Довідники.<xsl:value-of select="$DirectoryName"/>_Select();
            <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                <xsl:for-each select="Fields/Field[Type != 'pointer']">
                    <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text>
                    <xsl:text>Довідники.</xsl:text>
                    <xsl:value-of select="$DirectoryName"/>
                    <xsl:text>_Const.</xsl:text>
                    <xsl:value-of select="Name"/>,
                </xsl:for-each>
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Where = (List&lt;Where&gt;)where;
            }

            <xsl:for-each select="Fields/Field[SortField = 'True' and Type != 'pointer']">
              /* ORDER */
              <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Order.Add(Довідники.<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>, SelectOrder.ASC);
            </xsl:for-each>

            <xsl:for-each select="Fields/Field[Type = 'pointer']">
                /* Join Table */
                <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Joins.Add(
                    new Join(<xsl:value-of select="Join/table"/>, Довідники.<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Join/field"/>, <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Table, "<xsl:value-of select="Join/alias"/>"));
                <xsl:for-each select="FieldAndAlias">
                  /* Field */
                  <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue&lt;string&gt;("<xsl:value-of select="table"/>." + <xsl:value-of select="field"/>, "<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/>"));
                  <xsl:if test="../SortField = 'True'">
                    /* ORDER */
                    <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Order.Add("<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/>", SelectOrder.ASC);
                  </xsl:if>
                </xsl:for-each>
            </xsl:for-each>

            <xsl:for-each select="Fields/AdditionalField">
                /* Additional Field */
                <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue&lt;string&gt;(@$"<xsl:value-of select="Value"/>", "<xsl:value-of select="Name"/>"));
            </xsl:for-each>

            /* SELECT */
            await <xsl:value-of select="$DirectoryName"/>_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (<xsl:value-of select="$DirectoryName"/>_Select.MoveNext())
            {
                Довідники.<xsl:value-of select="$DirectoryName"/>_Pointer? cur = <xsl:value-of select="$DirectoryName"/>_Select.Current;

                if (cur != null)
                {
                    Dictionary&lt;string, object&gt; Fields = cur.Fields!;
                    <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        <!--<xsl:variable name="CountPointer" select="count(Fields/Field[Type = 'pointer'])"/>
                        <xsl:variable name="CountNotPointer" select="count(Fields/Field[Type != 'pointer'])"/>-->
                        <xsl:for-each select="Fields/Field[Type = 'pointer']">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:variable name="CountAlias" select="count(FieldAndAlias)"/>
                          <xsl:for-each select="FieldAndAlias">
                            <xsl:if test="position() &gt; 1"> + " " + </xsl:if>
                            <xsl:text>Fields[</xsl:text>"<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/><xsl:text>"].ToString()</xsl:text>
                            <xsl:if test="$CountAlias = 1"> ?? ""</xsl:if>
                          </xsl:for-each>,
                        </xsl:for-each>
                        <xsl:for-each select="Fields/Field[Type != 'pointer']">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:choose>
                            <xsl:when test="Type = 'enum'">
                              <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                              <xsl:text>Перелічення.ПсевдонімиПерелічення.</xsl:text>
                              <xsl:value-of select="$namePointer"/>
                              <xsl:text>_Alias( </xsl:text>
                              <xsl:text>((</xsl:text>
                              <xsl:value-of select="Pointer"/>
                              <xsl:text>)</xsl:text>
                              <xsl:text>(Fields[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] != DBNull.Value ? Fields[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] : 0)) )</xsl:text>
                            </xsl:when>
                            <xsl:when test="Type = 'boolean'">
                              <xsl:text>(Fields[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] != DBNull.Value ? (bool)Fields[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] : false) ? "Так" : ""</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text>Fields[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>].ToString() ?? ""</xsl:text>
                            </xsl:otherwise>
                          </xsl:choose>,
                        </xsl:for-each>

                        <xsl:for-each select="Fields/AdditionalField">
                            <xsl:value-of select="Name"/>
                            <xsl:text> = </xsl:text>
                            <xsl:text>Fields["</xsl:text>
                            <xsl:value-of select="Name"/>
                            <xsl:text>"].ToString() ?? ""</xsl:text>,
                        </xsl:for-each>
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    </xsl:for-each>

      <!-- ДЕРЕВО -->
      <xsl:for-each select="TabularLists/TabularList[IsTree = '1']">
        <xsl:variable name="TabularListName" select="Name"/>
    /* ДЕРЕВО */
    public class <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
    {
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? Іконки.ДляДерева.Delete : Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer <xsl:if test="count(Fields/Field[Name = 'Власник']) = 1">, UnigueID? owner</xsl:if>)
        {
            RootPath = SelectPath = null;

            <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> rootRecord = new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            #region SQL

            string query = $@"
WITH RECURSIVE r AS (
    SELECT 
        uid, 
        {<xsl:value-of select="$DirectoryName"/>_Const.Назва}, 
        {<xsl:value-of select="$DirectoryName"/>_Const.Родич}, 
        1 AS level,
        deletion_label
    FROM {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}
    WHERE {<xsl:value-of select="$DirectoryName"/>_Const.Родич} = '{Guid.Empty}'";

        <!-- Якщо є поле Власник у табличному списку -->
        <xsl:if test="count(Fields/Field[Name = 'Власник']) = 1">
            if (owner != null &amp;&amp; !owner.IsEmpty()) query += $@"
        AND {<xsl:value-of select="$DirectoryName"/>_Const.Власник} = '{owner}'";
        </xsl:if>

            if (openFolder != null) query += $@"
        AND uid != '{openFolder}'";

            query += $@"
    UNION ALL
    SELECT 
        {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.uid, 
        {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.{<xsl:value-of select="$DirectoryName"/>_Const.Назва}, 
        {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.{<xsl:value-of select="$DirectoryName"/>_Const.Родич}, 
        r.level + 1 AS level,
        {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.deletion_label 
    FROM {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}
        JOIN r ON {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.{<xsl:value-of select="$DirectoryName"/>_Const.Родич} = r.uid";

        <!-- Якщо є поле Власник у табличному списку -->
        <xsl:if test="count(Fields/Field[Name = 'Власник']) = 1">
            if (owner != null &amp;&amp; !owner.IsEmpty()) query += $@"
        AND {<xsl:value-of select="$DirectoryName"/>_Const.Власник} = '{owner}'";
        </xsl:if>

            if (openFolder != null) query += $@"
    WHERE {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.uid != '{openFolder}'";

            query += $@"
)
SELECT 
    uid, 
    {<xsl:value-of select="$DirectoryName"/>_Const.Назва} AS Назва, 
    {<xsl:value-of select="$DirectoryName"/>_Const.Родич} AS Родич, 
    level,
    deletion_label
FROM r
ORDER BY level, {<xsl:value-of select="$DirectoryName"/>_Const.Назва} ASC
";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequestAsync(query);

            Dictionary&lt;string, TreeIter&gt; nodeDictionary = new Dictionary&lt;string, TreeIter&gt;();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

                    <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> record = new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      </xsl:for-each>
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Документи.ТабличніСписки
{
    public static class Інтерфейс
    {
        public static ComboBoxText СписокВідбірПоПеріоду()
        {
            ComboBoxText сomboBox = new ComboBoxText();

            if (Config.Kernel != null)
            {
                ConfigurationEnums ТипПеріодуДляЖурналівДокументів = Config.Kernel.Conf.Enums["ТипПеріодуДляЖурналівДокументів"];

                foreach (ConfigurationEnumField field in ТипПеріодуДляЖурналівДокументів.Fields.Values)
                    сomboBox.Append(field.Name, field.Desc);
            }

            /*сomboBox.Active = 0;*/

            return сomboBox;
        }

        public static Where? ВідбірПоПеріоду(string fieldWhere, Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            switch (типПеріоду)
            {
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
                    return new Where(fieldWhere, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, 1, 1));
                case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
                {
                    DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
                    return new Where(fieldWhere, Comparison.QT_EQ, new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1));
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
                {
                    DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
                    return new Where(fieldWhere, Comparison.QT_EQ, new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1));
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.Місяць:
                    return new Where(fieldWhere, Comparison.QT_EQ, DateTime.Now.AddMonths(-1));
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
                    return new Where(fieldWhere, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
                {
                    DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
                    return new Where(fieldWhere, Comparison.QT_EQ, new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day));
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
                    return new Where(fieldWhere, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                default: 
                    return null;
            }
        }
    }

    <xsl:for-each select="Configuration/Documents/Document">
      <xsl:variable name="DocumentName" select="Name"/>
    #region DOCUMENT "<xsl:value-of select="$DocumentName"/>"
    
      <xsl:for-each select="TabularLists/TabularList">
        <xsl:variable name="TabularListName" select="Name"/>
    public class <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/> : ТабличнийСписок
    {
        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? Іконки.ДляТабличногоСписку.Delete : Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ <xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string),  
                </xsl:for-each>
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            <xsl:for-each select="Fields/Field">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text>
              <xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text>
              <xsl:value-of select="position() + 2"/>
              <xsl:text>) { MinWidth = 20, Resizable = true</xsl:text>
              <xsl:if test="Size != '0'">
                <xsl:text>, FixedWidth = </xsl:text>
                <xsl:value-of select="Size"/>
              </xsl:if>
              <xsl:text> } )</xsl:text>; /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            ОчиститиВідбір(treeView);
            Where? where = Інтерфейс.ВідбірПоПеріоду(Документи.<xsl:value-of select="$DocumentName"/>_Const.ДатаДок, типПеріоду);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.<xsl:value-of select="$DocumentName"/>_Select <xsl:value-of select="$DocumentName"/>_Select = new Документи.<xsl:value-of select="$DocumentName"/>_Select();
            <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                <xsl:for-each select="Fields/Field[Type != 'pointer']">
                    <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text>
                    <xsl:text>Документи.</xsl:text>
                    <xsl:value-of select="$DocumentName"/>
                    <xsl:text>_Const.</xsl:text>
                    <xsl:value-of select="Name"/>,
                </xsl:for-each>
            ]);

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Where = (List&lt;Where&gt;)where;
            }

            <xsl:for-each select="Fields/Field[SortField = 'True' and Type != 'pointer']">
              /* ORDER */
              <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Order.Add(Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>, SelectOrder.ASC);
            </xsl:for-each>

            <xsl:for-each select="Fields/Field[Type = 'pointer']">
                /* Join Table */
                <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Joins.Add(
                    new Join(<xsl:value-of select="Join/table"/>, Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Join/field"/>, <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Table, "<xsl:value-of select="Join/alias"/>"));
                <xsl:for-each select="FieldAndAlias">
                  /* Field */
                  <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue&lt;string&gt;("<xsl:value-of select="table"/>." + <xsl:value-of select="field"/>, "<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/>"));
                  <xsl:if test="../SortField = 'True'">
                    /* ORDER */
                    <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Order.Add("<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/>", SelectOrder.ASC);
                  </xsl:if>
                </xsl:for-each>
            </xsl:for-each>

            /* SELECT */
            await <xsl:value-of select="$DocumentName"/>_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (<xsl:value-of select="$DocumentName"/>_Select.MoveNext())
            {
                Документи.<xsl:value-of select="$DocumentName"/>_Pointer? cur = <xsl:value-of select="$DocumentName"/>_Select.Current;

                if (cur != null)
                {
                    Dictionary&lt;string, object&gt; Fields = cur.Fields!;
                    <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        <xsl:variable name="CountPointer" select="count(Fields/Field[Type = 'pointer'])"/>
                        <xsl:variable name="CountNotPointer" select="count(Fields/Field[Type != 'pointer'])"/>
                        <xsl:for-each select="Fields/Field[Type = 'pointer']">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:variable name="CountAlias" select="count(FieldAndAlias)"/>
                          <xsl:for-each select="FieldAndAlias">
                            <xsl:if test="position() &gt; 1"> + " " + </xsl:if>
                            <xsl:text>Fields[</xsl:text>"<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/><xsl:text>"].ToString()</xsl:text>
                            <xsl:if test="$CountAlias = 1"> ?? ""</xsl:if>
                          </xsl:for-each>
                          <xsl:if test="$CountNotPointer != 0 or position() != $CountPointer">,</xsl:if> /**/
                        </xsl:for-each>
                        <xsl:for-each select="Fields/Field[Type != 'pointer']">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:choose>
                            <xsl:when test="Type = 'enum'">
                              <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                              <xsl:text>Перелічення.ПсевдонімиПерелічення.</xsl:text>
                              <xsl:value-of select="$namePointer"/>
                              <xsl:text>_Alias( </xsl:text>
                              <xsl:text>((</xsl:text>
                              <xsl:value-of select="Pointer"/>
                              <xsl:text>)</xsl:text>
                              <xsl:text>(Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] != DBNull.Value ? Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] : 0)) )</xsl:text>
                            </xsl:when>
                            <xsl:when test="Type = 'boolean'">
                              <xsl:text>(Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] != DBNull.Value ? (bool)Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] : false) ? "Так" : ""</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text>Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>].ToString() ?? ""</xsl:text>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:if test="position() != $CountNotPointer">,</xsl:if> /**/
                        </xsl:for-each>
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    if (FirstPath == null)
                        FirstPath = CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();

                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    </xsl:for-each>
    #endregion
    </xsl:for-each>

    //
    // Журнали
    //

    <xsl:for-each select="Configuration/Journals/Journal">
      <xsl:variable name="JournalName" select="Name"/>
      <xsl:variable name="AllowDocument" select="AllowDocument"/>
    #region JOURNAL "<xsl:value-of select="$JournalName"/>"
    
    public class Журнали_<xsl:value-of select="$JournalName"/>
    {
        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        // Масив для запису стрічки в Store
        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? Іконки.ДляТабличногоСписку.Delete : Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Type, 
                /*Проведений документ*/ Spend,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text>
                  <xsl:value-of select="Name"/>
                  <xsl:text>, </xsl:text>
                </xsl:for-each> 
            };
        }

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                typeof(Gdk.Pixbuf), /* Image */
                typeof(string), /* ID */
                typeof(string), /* Type */
                typeof(bool), /* Spend Проведений документ */
                <xsl:for-each select="Fields/Field">
                    <xsl:text>typeof(string), </xsl:text>/*<xsl:value-of select="Name"/>*/
                </xsl:for-each>
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("Type", new CellRendererText(), "text", 2) { Visible = false }); /*Type*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 3)); /*Проведений документ*/
            /* */
            <xsl:for-each select="Fields/Field">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text>
              <xsl:value-of select="normalize-space(Name)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text>
              <xsl:value-of select="position() + 3"/> <!-- УВАГА! Коефіціент зміщення нумерації колонок -->
              <xsl:text>) { MinWidth = 20, Resizable = true } )</xsl:text>; /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Dictionary&lt;string, List&lt;Where&gt;&gt; WhereDict = [];
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", WhereDict);
            else
                treeView.Data["Where"] = WhereDict;
            <xsl:for-each select="$AllowDocument/Item">
              <xsl:variable name="AllowName" select="Name"/>
              <xsl:variable name="DocField" select="../../TabularLists/TabularList[Name = $AllowName]/Fields/Field[WherePeriod = '1']/DocField" />
              <xsl:if test="normalize-space($DocField) != ''">
            {
                List&lt;Where&gt; whereList = [];
                WhereDict.Add("<xsl:value-of select="$AllowName"/>", whereList);
                Where? where = Інтерфейс.ВідбірПоПеріоду(Документи.<xsl:value-of select="$AllowName"/>_Const.<xsl:value-of select="$DocField"/>, типПеріоду);
                if (where != null) whereList.Add(where);
            }
              </xsl:if>
            </xsl:for-each>
        }

        public static void ОчиститиВідбір(TreeView treeView)
        {
            if (treeView.Data.ContainsKey("Where"))
                treeView.Data["Where"] = null;
        }

        // Список документів які входять в журнал
        public static Dictionary&lt;string, string&gt; AllowDocument()
        {
            return new Dictionary&lt;string, string&gt;()
            {
                <xsl:for-each select="$AllowDocument/Item">
                    <xsl:text>{"</xsl:text><xsl:value-of select="Name"/>", "<xsl:value-of select="normalize-space(FullName)"/>"},
                </xsl:for-each>
            };
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static async ValueTask LoadRecords(TreeView treeView) 
        {
            SelectPath = CurrentPath = null;

            List&lt;string&gt; allQuery = [];
            Dictionary&lt;string, object&gt; paramQuery = [];

          <xsl:if test="count(TabularLists/TabularList) != 0">
            <xsl:for-each select="TabularLists/TabularList">
              <xsl:variable name="DocumentName" select="Name"/>
              <xsl:variable name="Table" select="Table"/>
              
              <xsl:if test="count($AllowDocument/Item[Name = $DocumentName]) = 1">
              //Документ: <xsl:value-of select="$DocumentName"/>
              {
                  Query query = new Query(Документи.<xsl:value-of select="$DocumentName"/>_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (treeView.Data.ContainsKey("Where"))
                  {
                      var where = treeView.Data["Where"];
                      if (where != null)
                      {
                          var Where = (Dictionary&lt;string, List&lt;Where&gt;&gt;)where;
                          if (Where.ContainsKey("<xsl:value-of select="$DocumentName"/>") &amp;&amp; Where["<xsl:value-of select="$DocumentName"/>"].Count != 0) 
                          {
                              query.Where = Where["<xsl:value-of select="$DocumentName"/>"];
                              foreach(Where field in query.Where)
                                  paramQuery.Add(field.Alias, field.Value);
                          }
                      }
                  }

                  query.FieldAndAlias.Add(new NameValue&lt;string&gt;("'<xsl:value-of select="$DocumentName"/>'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  <xsl:for-each select="Fields/Field">
                      <!-- Приведення даних в запиті до певного типу -->
                      <xsl:variable name="SqlType">
                          <xsl:if test="normalize-space(SqlType) != ''">
                              <xsl:text> + "::</xsl:text>
                              <xsl:value-of select="normalize-space(SqlType)"/>
                              <xsl:text>"</xsl:text>
                          </xsl:if>
                      </xsl:variable>
                      <xsl:choose>
                        <xsl:when test="normalize-space(DocField) != ''">
                          <xsl:choose>
                            <xsl:when test="Type = 'pointer'">
                              /* Join Table */
                              query.Joins.Add(
                                  new Join(<xsl:value-of select="Join/table"/>, Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Join/field"/>, query.Table, "<xsl:value-of select="Join/alias"/>"));
                              <xsl:for-each select="FieldAndAlias">
                                /* Field */
                                query.FieldAndAlias.Add(
                                  new NameValue&lt;string&gt;("<xsl:value-of select="table"/>." + <xsl:value-of select="field"/><xsl:value-of select="$SqlType"/>, "<xsl:value-of select="../Name"/>"));
                              </xsl:for-each>
                            </xsl:when>
                            <xsl:otherwise>
                              query.FieldAndAlias.Add(
                                  new NameValue&lt;string&gt;(Документи.<xsl:value-of select="$DocumentName"/>_Const.TABLE + "." + Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="DocField"/><xsl:value-of select="$SqlType"/>, "<xsl:value-of select="Name"/>"));
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          /* Empty Field */
                          query.FieldAndAlias.Add(new NameValue&lt;string&gt;("''", "<xsl:value-of select="Name"/>"));
                        </xsl:otherwise>
                      </xsl:choose>
                  </xsl:for-each>

                  allQuery.Add(query.Construct());
              }
              </xsl:if>
            </xsl:for-each>

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            <xsl:if test="count(Fields/Field[SortField = '1']) != 0">
                <xsl:text>unionAllQuery += "\nORDER BY </xsl:text>
                <xsl:for-each select="Fields/Field[SortField = '1']">
                    <xsl:if test="position() != 1">
                        <xsl:text>, </xsl:text>
                    </xsl:if>
                    <xsl:value-of select="Name"/>
                </xsl:for-each>
                <xsl:text>";</xsl:text>
            </xsl:if>

            var recordResult = await Config.Kernel.DataBase.SelectRequestAsync(unionAllQuery, paramQuery);

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Dictionary&lt;string, object&gt; row in recordResult.ListRow)
            {
                Журнали_<xsl:value-of select="$JournalName"/> record = new Журнали_<xsl:value-of select="$JournalName"/>
                {
                    ID = row["uid"].ToString() ?? "",
                    Type = row["type"].ToString() ?? "",
                    DeletionLabel = (bool)row["deletion_label"],
                    Spend = (bool)row["spend"],
                    <xsl:for-each select="Fields/Field">
                        <xsl:value-of select="Name"/> = row["<xsl:value-of select="Name"/>"] != DBNull.Value ? (row["<xsl:value-of select="Name"/>"].ToString() ?? "") : "",
                    </xsl:for-each>
                };

                TreeIter CurrentIter = Store.AppendValues(record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
          </xsl:if>
        }
    }
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.РегістриВідомостей.ТабличніСписки
{
    <xsl:for-each select="Configuration/RegistersInformation/RegisterInformation">
      <xsl:variable name="RegisterName" select="Name"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    
      <xsl:for-each select="TabularLists/TabularList">
        <xsl:variable name="TabularListName" select="Name"/>
    public class <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/> : ТабличнийСписок
    {
        string ID = "";
        string Період = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] 
            { 
                Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Період,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each> 
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Період*/ typeof(string),
                <xsl:for-each select="Fields/Field">
                    <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string),
                </xsl:for-each>
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 2));
            /* */
            <xsl:for-each select="Fields/Field">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text>
              <xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text>
              <xsl:value-of select="position() + 2"/>
              <xsl:text>) { MinWidth = 20, Resizable = true, SortColumnId = </xsl:text>
              <xsl:value-of select="position() + 2"/>
              <xsl:if test="Size != '0'">
                <xsl:text>, FixedWidth = </xsl:text>
                <xsl:value-of select="Size"/>
              </xsl:if>
              <xsl:text> } )</xsl:text>; /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_RecordsSet <xsl:value-of select="$RegisterName"/>_RecordsSet = new РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_RecordsSet();

            /* Where */
            if (treeView.Data.ContainsKey("Where"))
            {
                var where = treeView.Data["Where"];
                if (where != null) <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Where = (List&lt;Where&gt;)where;
            }

            /* DEFAULT ORDER */
            <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Order.Add("period", SelectOrder.ASC);

            <xsl:for-each select="Fields/Field[SortField = 'True' and Type != 'pointer']">
              /* ORDER */
              <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Order.Add(РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_Const.<xsl:value-of select="Name"/>, SelectOrder.ASC);
            </xsl:for-each>

            <xsl:for-each select="Fields/Field[Type = 'pointer']">
                /* Join Table */
                <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Joins.Add(
                    new Join(<xsl:value-of select="Join/table"/>, РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_Const.<xsl:value-of select="Join/field"/>, <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Table, "<xsl:value-of select="Join/alias"/>"));
                <xsl:for-each select="FieldAndAlias">
                  /* Field */
                  <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.FieldAndAlias.Add(
                    new NameValue&lt;string&gt;("<xsl:value-of select="table"/>." + <xsl:value-of select="field"/>, "<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/>"));
                  <xsl:if test="../SortField = 'True'">
                    /* ORDER */
                    <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Order.Add("<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/>", SelectOrder.ASC);
                  </xsl:if>
                </xsl:for-each>
            </xsl:for-each>

            /* Read */
            await <xsl:value-of select="$RegisterName"/>_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (<xsl:value-of select="$RegisterName"/>_RecordsSet.Record record in <xsl:value-of select="$RegisterName"/>_RecordsSet.Records)
            {
                <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/>
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    <xsl:variable name="CountPointer" select="count(Fields/Field[Type = 'pointer'])"/>
                    <xsl:variable name="CountNotPointer" select="count(Fields/Field[Type != 'pointer'])"/>
                    <xsl:for-each select="Fields/Field[Type = 'pointer']">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:variable name="CountAlias" select="count(FieldAndAlias)"/>
                      <xsl:for-each select="FieldAndAlias">
                        <xsl:if test="position() &gt; 1"> + " " + </xsl:if>
                        <xsl:value-of select="$RegisterName"/>_RecordsSet.JoinValue[record.UID.ToString()]["<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/><xsl:text>"].ToString()</xsl:text>
                        <xsl:if test="$CountAlias = 1"> ?? ""</xsl:if>
                      </xsl:for-each>
                      <xsl:if test="$CountNotPointer != 0 or position() != $CountPointer">,</xsl:if> /**/
                    </xsl:for-each>
                    <xsl:for-each select="Fields/Field[Type != 'pointer']">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:choose>
                        <xsl:when test="Type = 'enum'">
                          <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                          <xsl:text>Перелічення.ПсевдонімиПерелічення.</xsl:text>
                          <xsl:value-of select="$namePointer"/>
                          <xsl:text>_Alias( </xsl:text>
                          <xsl:text>((</xsl:text>
                          <xsl:value-of select="Pointer"/>
                          <xsl:text>)</xsl:text>
                          <xsl:text>(record.</xsl:text>
                          <xsl:value-of select="Name"/>
                          <xsl:text> != DBNull.Value ? record.</xsl:text>
                          <xsl:value-of select="Name"/>
                          <xsl:text> : 0)) )</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text>record.</xsl:text>
                          <xsl:value-of select="Name"/>
                          <xsl:text>.ToString() ?? ""</xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>
                      <xsl:if test="position() != $CountNotPointer">,</xsl:if> /**/
                    </xsl:for-each>
                };

                TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (Record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
        }
    }
	    </xsl:for-each>
    #endregion
    </xsl:for-each>
}

  </xsl:template>
</xsl:stylesheet>