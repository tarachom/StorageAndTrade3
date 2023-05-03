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

namespace <xsl:value-of select="Configuration/NameSpace"/>.Довідники.ТабличніСписки
{
    <xsl:for-each select="Configuration/Directories/Directory">
      <xsl:variable name="DirectoryName" select="Name"/>
    #region DIRECTORY "<xsl:value-of select="$DirectoryName"/>"
    
      <xsl:for-each select="TabularLists/TabularList">
        <xsl:variable name="TabularListName" select="Name"/>
    public class <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        string ID = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID
            /* */ <xsl:for-each select="Fields/Field">
              <xsl:text>, </xsl:text>
              <xsl:value-of select="Name"/>
            </xsl:for-each> };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */
            <xsl:for-each select="Fields/Field">
              <xsl:text>, typeof(string)</xsl:text> /* <xsl:value-of select="Name"/> */
            </xsl:for-each>);

        public static void AddColumns(TreeView treeView)
        {
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            /* */
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
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static List&lt;Where&gt; Where { get; set; } = new List&lt;Where&gt;();

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = FirstPath = null;

            Довідники.<xsl:value-of select="$DirectoryName"/>_Select <xsl:value-of select="$DirectoryName"/>_Select = new Довідники.<xsl:value-of select="$DirectoryName"/>_Select();
            <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/
                    <xsl:for-each select="Fields/Field[Type != 'pointer']">
                        <xsl:text>, </xsl:text>
                        <xsl:text>Довідники.</xsl:text>
                        <xsl:value-of select="$DirectoryName"/>
                        <xsl:text>_Const.</xsl:text>
                        <xsl:value-of select="Name"/> /* <xsl:value-of select="position()"/> */
                    </xsl:for-each>
                });

            /* Where */
            <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Where = Where;

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

            /* SELECT */
            <xsl:value-of select="$DirectoryName"/>_Select.Select();
            while (<xsl:value-of select="$DirectoryName"/>_Select.MoveNext())
            {
                Довідники.<xsl:value-of select="$DirectoryName"/>_Pointer? cur = <xsl:value-of select="$DirectoryName"/>_Select.Current;

                if (cur != null)
                {
                    <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        <xsl:variable name="CountPointer" select="count(Fields/Field[Type = 'pointer'])"/>
                        <xsl:variable name="CountNotPointer" select="count(Fields/Field[Type != 'pointer'])"/>
                        <xsl:for-each select="Fields/Field[Type = 'pointer']">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:variable name="CountAlias" select="count(FieldAndAlias)"/>
                          <xsl:for-each select="FieldAndAlias">
                            <xsl:if test="position() &gt; 1"> + " " + </xsl:if>
                            <xsl:text>cur.Fields?[</xsl:text>"<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/><xsl:text>"]?.ToString()</xsl:text>
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
                              <xsl:text>(cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]! != DBNull.Value ? cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]! : 0)) )</xsl:text>
                            </xsl:when>
                            <xsl:when test="Type = 'boolean'">
                              <xsl:text>(cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]! != DBNull.Value ? (bool)cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]! : false) ? "Так" : ""</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text>cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DirectoryName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]?.ToString() ?? ""</xsl:text>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:if test="position() != $CountNotPointer">,</xsl:if> /**/
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
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpace"/>.Документи.ТабличніСписки
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

        public static void ДодатиВідбірПоПеріоду(List&lt;Where&gt; Where, string fieldWhere, Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            switch (типПеріоду)
            {
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуРоку:
                {
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, 1, 1)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.Квартал:
                {
                    DateTime ДатаТриМісцяНазад = DateTime.Now.AddMonths(-3);
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(ДатаТриМісцяНазад.Year, ДатаТриМісцяНазад.Month, 1)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗМинулогоМісяця:
                {
                    DateTime ДатаМісцьНазад = DateTime.Now.AddMonths(-1);
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(ДатаМісцьНазад.Year, ДатаМісцьНазад.Month, 1)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.Місяць:
                {
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, DateTime.Now.AddMonths(-1)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуМісяця:
                {
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ЗПочаткуТижня:
                {
                    DateTime СімДнівНазад = DateTime.Now.AddDays(-7);
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(СімДнівНазад.Year, СімДнівНазад.Month, СімДнівНазад.Day)));
                    break;
                }
                case Перелічення.ТипПеріодуДляЖурналівДокументів.ПоточнийДень:
                {
                    Where.Add(new Where(fieldWhere, Comparison.QT_EQ, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)));
                    break;
                }
            }
        }
    }

    <xsl:for-each select="Configuration/Documents/Document">
      <xsl:variable name="DocumentName" select="Name"/>
    #region DOCUMENT "<xsl:value-of select="$DocumentName"/>"
    
      <xsl:for-each select="TabularLists/TabularList">
        <xsl:variable name="TabularListName" select="Name"/>
    public class <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/>
    {
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Spend /*Проведений документ*/
            /* */ <xsl:for-each select="Fields/Field">
              <xsl:text>, </xsl:text>
              <xsl:value-of select="Name"/>
            </xsl:for-each> };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(bool) /* Spend Проведений документ*/
            <xsl:for-each select="Fields/Field">
              <xsl:text>, typeof(string)</xsl:text> /* <xsl:value-of select="Name"/> */
            </xsl:for-each>);

        public static void AddColumns(TreeView treeView)
        {
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

        public static List&lt;Where&gt; Where { get; set; } = new List&lt;Where&gt;();

        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            Інтерфейс.ДодатиВідбірПоПеріоду(Where, Документи.<xsl:value-of select="$DocumentName"/>_Const.ДатаДок, типПеріоду);
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            SelectPath = FirstPath = null;

            Документи.<xsl:value-of select="$DocumentName"/>_Select <xsl:value-of select="$DocumentName"/>_Select = new Документи.<xsl:value-of select="$DocumentName"/>_Select();
            <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Field.AddRange(
                new string[]
                { "deletion_label" /*Помітка на видалення*/,
                  "spend" /*Проведений документ*/
                    <xsl:for-each select="Fields/Field[Type != 'pointer']">
                        <xsl:text>, </xsl:text>
                        <xsl:text>Документи.</xsl:text>
                        <xsl:value-of select="$DocumentName"/>
                        <xsl:text>_Const.</xsl:text>
                        <xsl:value-of select="Name"/> /* <xsl:value-of select="position()"/> */
                    </xsl:for-each>
                });

            /* Where */
            <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Where = Where;

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
            <xsl:value-of select="$DocumentName"/>_Select.Select();
            while (<xsl:value-of select="$DocumentName"/>_Select.MoveNext())
            {
                Документи.<xsl:value-of select="$DocumentName"/>_Pointer? cur = <xsl:value-of select="$DocumentName"/>_Select.Current;

                if (cur != null)
                {
                    <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)cur.Fields?["spend"]!, /*Проведений документ*/
                        DeletionLabel = (bool)cur.Fields?["deletion_label"]!, /*Помітка на видалення*/
                        <xsl:variable name="CountPointer" select="count(Fields/Field[Type = 'pointer'])"/>
                        <xsl:variable name="CountNotPointer" select="count(Fields/Field[Type != 'pointer'])"/>
                        <xsl:for-each select="Fields/Field[Type = 'pointer']">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:variable name="CountAlias" select="count(FieldAndAlias)"/>
                          <xsl:for-each select="FieldAndAlias">
                            <xsl:if test="position() &gt; 1"> + " " + </xsl:if>
                            <xsl:text>cur.Fields?[</xsl:text>"<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/><xsl:text>"]?.ToString()</xsl:text>
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
                              <xsl:text>(cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]! != DBNull.Value ? cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]! : 0)) )</xsl:text>
                            </xsl:when>
                            <xsl:when test="Type = 'boolean'">
                              <xsl:text>(cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]! != DBNull.Value ? (bool)cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]! : false) ? "Так" : ""</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text>cur.Fields?[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>]?.ToString() ?? ""</xsl:text>
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
        string Image 
        {
            get
            {
                return AppContext.BaseDirectory + "images/" + (DeletionLabel ? "doc_delete.png" : "doc.png");
            }
        }

        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        // Масив для запису стрічки в Store
        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Type, Spend /*Проведений документ*/
            /* */ <xsl:for-each select="Fields/Field">
              <xsl:text>, </xsl:text>
              <xsl:value-of select="Name"/>
            </xsl:for-each> };
        }

        // Джерело даних для списку
        public static ListStore Store = new ListStore(
          typeof(Gdk.Pixbuf) /* Image */, 
          typeof(string) /* ID */, 
          typeof(string) /* Type */, 
          typeof(bool) /* Spend Проведений документ*/
            <xsl:for-each select="Fields/Field">
              <xsl:text>, typeof(string)</xsl:text> /* <xsl:value-of select="Name"/> */
            </xsl:for-each>);

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
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

        // Словник з відборами, ключ це Тип документу
        public static Dictionary&lt;string, List&lt;Where&gt;&gt; Where { get; set; } = new Dictionary&lt;string, List&lt;Where&gt;&gt;();

        // Добавляє відбір по періоду в словник відборів
        public static void ДодатиВідбірПоПеріоду(Перелічення.ТипПеріодуДляЖурналівДокументів типПеріоду)
        {
            Where.Clear();
            <xsl:for-each select="$AllowDocument/Item">
              <xsl:variable name="AllowName" select="Name"/>
              <xsl:variable name="DocField" select="../../TabularLists/TabularList[Name = $AllowName]/Fields/Field[WherePeriod = '1']/DocField" />
              <xsl:if test="normalize-space($DocField) != ''">
            {
                List&lt;Where&gt; where = new List&lt;Where&gt;();
                Where.Add("<xsl:value-of select="$AllowName"/>", where);
                Інтерфейс.ДодатиВідбірПоПеріоду(where, <xsl:value-of select="$AllowName"/>_Const.<xsl:value-of select="$DocField"/>, типПеріоду);
            }
              </xsl:if>
            </xsl:for-each>
        }

        // Список документів які входять в журнал
        public static Dictionary&lt;string, string&gt; AllowDocument()
        {
            Dictionary&lt;string, string&gt; allowDoc = new Dictionary&lt;string, string&gt;();
            <xsl:for-each select="$AllowDocument/Item">
                <xsl:text>allowDoc.Add("</xsl:text>
                <xsl:value-of select="Name"/>", "<xsl:value-of select="normalize-space(FullName)"/>");
            </xsl:for-each>
            return allowDoc;
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static void LoadRecords()
        {
            Store.Clear();
            List&lt;string&gt; allQuery = new List&lt;string&gt;();
            Dictionary&lt;string, object&gt; paramQuery = new Dictionary&lt;string, object&gt;();

            <xsl:for-each select="TabularLists/TabularList">
              <xsl:variable name="DocumentName" select="Name"/>
              <xsl:variable name="Table" select="Table"/>
              
              <xsl:if test="count($AllowDocument/Item[Name = $DocumentName]) = 1">
              {
                  Query query = new Query(Документи.<xsl:value-of select="$DocumentName"/>_Const.TABLE);

                  // Встановлення відбору для даного типу документу
                  if (Where.ContainsKey("<xsl:value-of select="$DocumentName"/>") &amp;&amp; Where["<xsl:value-of select="$DocumentName"/>"].Count != 0) {
                      query.Where = Where["<xsl:value-of select="$DocumentName"/>"];
                      foreach(Where field in query.Where)
                          paramQuery.Add(field.Alias, field.Value);
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

            string[] columnsName;
            List&lt;Dictionary&lt;string, object&gt;&gt; listRow;

            Config.Kernel!.DataBase.SelectRequest(unionAllQuery, paramQuery, out columnsName, out listRow);

            foreach (Dictionary&lt;string, object&gt; row in listRow)
            {
                Журнали_<xsl:value-of select="$JournalName"/> record = new Журнали_<xsl:value-of select="$JournalName"/>();
                record.ID = row["uid"]?.ToString() ?? "";
                record.Type = row["type"]?.ToString() ?? "";
                record.DeletionLabel = (bool)row["deletion_label"];
                record.Spend = (bool)row["spend"];
                <xsl:for-each select="Fields/Field">
                    record.<xsl:value-of select="Name"/> = row["<xsl:value-of select="Name"/>"] != DBNull.Value ? (row["<xsl:value-of select="Name"/>"]?.ToString() ?? "") : "";
                </xsl:for-each>

                TreeIter CurrentIter = Store.AppendValues(record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
        }
    }
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpace"/>.РегістриВідомостей.ТабличніСписки
{
    <xsl:for-each select="Configuration/RegistersInformation/RegisterInformation">
      <xsl:variable name="RegisterName" select="Name"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    
      <xsl:for-each select="TabularLists/TabularList">
        <xsl:variable name="TabularListName" select="Name"/>
    public class <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/>
    {
        string Image = AppContext.BaseDirectory + "images/doc.png";
        string ID = "";
        string Період = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] { new Gdk.Pixbuf(Image), ID, Період
            /* */ <xsl:for-each select="Fields/Field">
              <xsl:text>, </xsl:text>
              <xsl:value-of select="Name"/>
            </xsl:for-each> };
        }

        public static ListStore Store = new ListStore(typeof(Gdk.Pixbuf) /* Image */, typeof(string) /* ID */, typeof(string) /* Період */
            <xsl:for-each select="Fields/Field">
              <xsl:text>, typeof(string)</xsl:text> /* <xsl:value-of select="Name"/> */
            </xsl:for-each>);

        public static void AddColumns(TreeView treeView)
        {
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

        public static List&lt;Where&gt; Where { get; set; } = new List&lt;Where&gt;();

        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static void LoadRecords()
        {
            Store.Clear();
            FirstPath = null;

            РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_RecordsSet <xsl:value-of select="$RegisterName"/>_RecordsSet = new РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_RecordsSet();

            /* Where */
            <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Where = Where;

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
            <xsl:value-of select="$RegisterName"/>_RecordsSet.Read();
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

                if (FirstPath == null)
                    FirstPath = CurrentPath;
            }
        }
    }
	    </xsl:for-each>
    #endregion
    </xsl:for-each>
}

  </xsl:template>
</xsl:stylesheet>