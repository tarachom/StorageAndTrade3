<?xml version="1.0" encoding="utf-8"?>
<!--
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
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="text" indent="yes" />

    <!-- Файл -->
    <xsl:param name="File" />

    <!-- Простори імен -->
    <xsl:param name="NameSpaceGenerationCode" />
    <xsl:param name="NameSpace" />

    <xsl:template match="root">

        <xsl:choose>

            <xsl:when test="$File = 'TablePart'">
                <xsl:call-template name="TablePart" />
            </xsl:when>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="TablePartList" />
            </xsl:when>
            
        </xsl:choose>

    </xsl:template>

    <!-- Для задання типу поля -->
    <xsl:template name="FieldType">
        <xsl:choose>
            <xsl:when test="Type = 'string'">string</xsl:when>
            <xsl:when test="Type = 'string[]'">string[]</xsl:when>
            <xsl:when test="Type = 'integer'">int</xsl:when>
            <xsl:when test="Type = 'integer[]'">int[]</xsl:when>
            <xsl:when test="Type = 'numeric'">decimal</xsl:when>
            <xsl:when test="Type = 'numeric[]'">decimal[]</xsl:when>
            <xsl:when test="Type = 'boolean'">bool</xsl:when>
            <xsl:when test="Type = 'time'">TimeSpan</xsl:when>
            <xsl:when test="Type = 'date' or Type = 'datetime'">DateTime</xsl:when>
            <xsl:when test="Type = 'pointer'"><xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer</xsl:when>
            <xsl:when test="Type = 'any_pointer'">Guid</xsl:when>
            <xsl:when test="Type = 'composite_pointer'">UuidAndText</xsl:when>
            <xsl:when test="Type = 'enum'"><xsl:value-of select="substring-after(Pointer, '.')"/></xsl:when>
            <xsl:when test="Type = 'bytea'">byte[]</xsl:when>
        </xsl:choose>    
    </xsl:template>

    <!-- Для конструкторів. Значення поля по замовчуванню відповідно до типу -->
    <xsl:template name="DefaultFieldValue">
        <xsl:choose>
            <xsl:when test="Type = 'string'">""</xsl:when>
            <xsl:when test="Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'bytea'">[]</xsl:when>
            <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'enum'">0</xsl:when>
            <xsl:when test="Type = 'boolean'">false</xsl:when>
            <xsl:when test="Type = 'time'">DateTime.MinValue.TimeOfDay</xsl:when>
            <xsl:when test="Type = 'date' or Type = 'datetime'">DateTime.MinValue</xsl:when>
            <xsl:when test="Type = 'pointer'">new <xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer()</xsl:when>
            <xsl:when test="Type = 'any_pointer'">new Guid()</xsl:when>
            <xsl:when test="Type = 'composite_pointer'">new UuidAndText()</xsl:when>
        </xsl:choose>
    </xsl:template>

<!--- 
//
// ============================ Таблична Частина ============================
//
-->

    <!-- Таблична Частина -->
    <xsl:template name="TablePart">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>
        <xsl:variable name="FieldsTL" select="TablePart/ElementFields/Field"/>

        <xsl:variable name="OwnerExist" select="TablePart/OwnerExist"/>
        <xsl:variable name="OwnerName" select="TablePart/OwnerName"/>
        <xsl:variable name="OwnerType">
            <xsl:choose>
                <xsl:when test="TablePart/OwnerType = 'Directory'">Довідник</xsl:when>
                <xsl:when test="TablePart/OwnerType = 'Document'">Документ</xsl:when>
                <xsl:otherwise>[...]</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
/*
        <xsl:value-of select="$OwnerName"/>_ТабличнаЧастина_<xsl:value-of select="$TablePartName"/>.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Перелічення;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$OwnerName"/>_ТабличнаЧастина_<xsl:value-of select="$TablePartName"/> : <xsl:value-of select="$OwnerType"/>ТабличнаЧастина
    {
        public <xsl:value-of select="$OwnerName"/>_Objest? ЕлементВласник { get; set; }
        
        #region Записи

        enum Columns
        {
        <xsl:for-each select="$FieldsTL">
            <xsl:value-of select="Name"/>,
        </xsl:for-each>
        }

        ListStore Store = new ListStore([
        <xsl:for-each select="$FieldsTL">
            typeof(<xsl:choose>
                <xsl:when test="Type = 'string'">string</xsl:when>
                <xsl:when test="Type = 'integer'">int</xsl:when>
                <xsl:when test="Type = 'numeric'">float</xsl:when>
                <xsl:when test="Type = 'boolean'">bool</xsl:when>
                <xsl:otherwise>string</xsl:otherwise>
            </xsl:choose>), //<xsl:value-of select="Name"/>
        </xsl:for-each>
        ]);

        List&lt;Запис&gt; Записи = [];

        private class Запис
        {
            public Guid ID { get; set; } = Guid.Empty;
            <xsl:for-each select="$FieldsTL">
                <xsl:text>public </xsl:text>
                <xsl:call-template name="FieldType" />
                <xsl:text> </xsl:text>
                <xsl:value-of select="Name"/>
                <xsl:text> { get; set; } = </xsl:text>
                <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>

            public object[] ToArray()
            {
                return
                [
                    <xsl:for-each select="$FieldsTL">
                        <xsl:choose>
                            <xsl:when test="Type = 'numeric'">(float)</xsl:when>
                            <xsl:when test="Type = 'enum'">ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_Alias(</xsl:when>
                        </xsl:choose>
                        <xsl:value-of select="Name"/>
                        <xsl:choose>
                            <xsl:when test="Type = 'pointer'">.Назва</xsl:when>
                            <xsl:when test="Type = 'enum'">)</xsl:when>
                            <xsl:when test="Type = 'date' or Type = 'datetime' or Type = 'time'">.ToString()</xsl:when>
                        </xsl:choose>,
                    </xsl:for-each>
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    <xsl:for-each select="$FieldsTL">
                        <xsl:value-of select="Name"/>
                        <xsl:text> = запис.</xsl:text>
                        <xsl:value-of select="Name"/>
                        <xsl:choose>
                            <xsl:when test="Type = 'pointer'">.Copy()</xsl:when>
                        </xsl:choose>,
                    </xsl:for-each>
                };
            }
            <xsl:for-each select="$FieldsTL">
                <xsl:choose>
                    <xsl:when test="Type = 'pointer'">
            public static async ValueTask ПісляЗміни_<xsl:value-of select="Name"/>(Запис запис)
            {
                await запис.<xsl:value-of select="Name"/>.GetPresentation();
            }
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        #endregion

        public <xsl:value-of select="$OwnerName"/>_ТабличнаЧастина_<xsl:value-of select="$TablePartName"/>()
        {
            TreeViewGrid.Model = Store;
            AddColumn();
        }

        void AddColumn()
        {
            <xsl:for-each select="$FieldsTL">
            //<xsl:value-of select="Name"/>
            {
                <xsl:choose>
                    <xsl:when test="Type = 'pointer'">
                TreeViewColumn column = new TreeViewColumn("<xsl:value-of select="Caption"/>", new CellRendererText(), "text", (int)Columns.<xsl:value-of select="Name"/>) { Resizable = true, MinWidth = 200 };
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                ListStore store = new ListStore(typeof(string), typeof(string));

                foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_List())
                    store.AppendValues(field.Value.ToString(), field.Name);

                CellRendererCombo cellCombo = new CellRendererCombo() { Editable = true, Model = store, TextColumn = 1 };
                cellCombo.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("<xsl:value-of select="Caption"/>", cellCombo, "text", (int)Columns.<xsl:value-of select="Name"/>) { Resizable = true, MinWidth = 100 };
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric'">
                CellRendererText cellNumber = new CellRendererText() { Editable = true };
                cellNumber.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("<xsl:value-of select="Caption"/>", cellNumber, "text", (int)Columns.<xsl:value-of select="Name"/>) { Resizable = true, MinWidth = 100 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                CellRendererToggle cellToggle = new CellRendererToggle() { };
                cellToggle.Toggled += EditCell;
                TreeViewColumn column = new TreeViewColumn("<xsl:value-of select="Caption"/>", cellToggle, "active", (int)Columns.<xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:otherwise>
                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                TreeViewColumn column = new TreeViewColumn("<xsl:value-of select="Caption"/>", cellText, "text", (int)Columns.<xsl:value-of select="Name"/>) { Resizable = true, MinWidth = 100 };
                    </xsl:otherwise>
                </xsl:choose>
                SetColIndex(column, Columns.<xsl:value-of select="Name"/>);
                TreeViewGrid.AppendColumn(column);
            }
            </xsl:for-each>
            //Колонка пустишка для заповнення вільного простору
            TreeViewGrid.AppendColumn(new TreeViewColumn());            
        }
        #region Load and Save

        public override async ValueTask LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (ЕлементВласник != null)
            {
                ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.FillJoin([]);
                await ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.Read();
                foreach (<xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.Record record in ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.Records)
                {
                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        <xsl:for-each select="$FieldsTL">
                        <xsl:value-of select="Name"/> = record.<xsl:value-of select="Name"/>,
                        </xsl:for-each>
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public override async ValueTask SaveRecords()
        {
            if (ЕлементВласник != null)
            {
                ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.Records.Clear();
                foreach (Запис запис in Записи)
                {
                    ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.Records.Add(new <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.Record()
                    {
                        UID = запис.ID,
                        <xsl:for-each select="$FieldsTL">
                        <xsl:value-of select="Name"/> = запис.<xsl:value-of select="Name"/>,
                        </xsl:for-each>
                    });
                }
                await ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.Save(true);
                await LoadRecords();
            }
        }

        #endregion

        #region Func

        <xsl:if test="count($FieldsTL[Type = 'pointer']) != 0">
        protected override ФормаЖурнал? OpenSelect(TreeIter iter, int rowNumber, int colNumber)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                <xsl:for-each select="$FieldsTL[Type = 'pointer']">
                <xsl:variable name="nameGroup" select="substring-before(Pointer, '.')" />
                <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                <xsl:text>case Columns.</xsl:text><xsl:value-of select="Name"/>:
                {
                    <xsl:value-of select="$namePointer"/><xsl:if test="$nameGroup = 'Довідники'">_ШвидкийВибір</xsl:if> page = new()
                    {
                        <xsl:choose>
                            <xsl:when test="$nameGroup = 'Довідники'">DirectoryPointerItem</xsl:when>
                            <xsl:when test="$nameGroup = 'Документи'">DocumentPointerItem</xsl:when>
                        </xsl:choose> = запис.<xsl:value-of select="Name"/>.UnigueID,
                        CallBack_OnSelectPointer = async (UnigueID selectPointer) =&gt;
                        {
                            запис.<xsl:value-of select="Name"/> = new <xsl:value-of select="$namePointer"/>_Pointer(selectPointer);
                            await Запис.ПісляЗміни_<xsl:value-of select="Name"/>(запис);
                            Store.SetValues(iter, запис.ToArray());
                        }
                    };
                    return page;
                }
                </xsl:for-each>
                default: return null;
            }
        }

        protected override void ClearCell(TreeIter iter, int rowNumber, int colNumber)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                <xsl:for-each select="$FieldsTL[Type = 'pointer']">
                <xsl:text>case Columns.</xsl:text><xsl:value-of select="Name"/>: { запис.<xsl:value-of select="Name"/>.Clear(); break; }
                </xsl:for-each>
                default: break;             
            }
            Store.SetValues(iter, запис.ToArray());
        }
        </xsl:if>

        <xsl:if test="count($FieldsTL[Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'enum']) != 0">
        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, string newText)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                <xsl:for-each select="$FieldsTL[Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'enum']">
                <xsl:text>case Columns.</xsl:text><xsl:value-of select="Name"/>: { <xsl:choose>
                    <xsl:when test="Type = 'string'">запис.<xsl:value-of select="Name"/> = newText;</xsl:when>
                    <xsl:when test="Type = 'integer'">var (check, value) = Validate.IsInt(newText); if (check) запис.<xsl:value-of select="Name"/> = value;</xsl:when>
                    <xsl:when test="Type = 'numeric'">var (check, value) = Validate.IsDecimal(newText); if (check) запис.<xsl:value-of select="Name"/> = value;</xsl:when>
                    <xsl:when test="Type = 'enum'">запис.<xsl:value-of select="Name"/> = ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_FindByName(newText) ?? 0;</xsl:when>
                </xsl:choose> break; }
                </xsl:for-each>
                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }
        </xsl:if>

        <xsl:if test="count($FieldsTL[Type = 'boolean']) != 0">
        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, bool newValue)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                <xsl:for-each select="$FieldsTL[Type = 'boolean']">
                <xsl:text>case Columns.</xsl:text><xsl:value-of select="Name"/>: { запис.<xsl:value-of select="Name"/> = newValue; break; }
                </xsl:for-each>
                default: break;
            }
            Store.SetValues(iter, запис.ToArray());
        }
        </xsl:if>

        <xsl:if test="count($FieldsTL[Type = 'integer' or Type = 'numeric']) != 0">
        void NumericCellDataFunc(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            if (GetColIndex(column, out int colNumber))
            {
                int rowNumber = int.Parse(Store.GetPath(iter).ToString());
                Запис запис = Записи[rowNumber];

                CellRendererText cellText = (CellRendererText)cell;
                cellText.Foreground = "green";

                switch ((Columns)colNumber)
                {
                    <xsl:for-each select="$FieldsTL[Type = 'integer' or Type = 'numeric']">
                    case Columns.<xsl:value-of select="Name"/>:
                    {
                        cellText.Text = запис.<xsl:value-of select="Name"/>.ToString();
                        break;
                    }
                    </xsl:for-each>
                    default: break;
                }
            }
        }
        </xsl:if>

        #endregion

        #region ToolBar

        protected override void AddRecord()
        {
            Запис запис = new Запис();
            Записи.Add(запис);

            TreeIter iter = Store.AppendValues(запис.ToArray());
            TreeViewGrid.SetCursor(Store.GetPath(iter), TreeViewGrid.Columns[0], false);
        }

        protected override void CopyRecord(int rowNumber)
        {
            Запис запис = Записи[rowNumber];
            Запис записНовий = Запис.Clone(запис);
            Записи.Add(записНовий);

            TreeIter iter = Store.AppendValues(записНовий.ToArray());
            TreeViewGrid.SetCursor(Store.GetPath(iter), TreeViewGrid.Columns[0], false);
        }

        protected override void DeleteRecord(TreeIter iter, int rowNumber)
        {
            Запис запис = Записи[rowNumber];
            Записи.Remove(запис);
            Store.Remove(ref iter);
        }

        #endregion
    }
}
    </xsl:template>

<!--- 
//
// ============================ Список ============================
//
-->

    <!-- Список -->
    <xsl:template name="TablePartList">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>
        <!--<xsl:variable name="Fields" select="TablePart/Fields/Field"/>-->
/*
        _ТабличнаЧастина_<xsl:value-of select="$TablePartName"/>.cs
        Список
*/
    </xsl:template>

</xsl:stylesheet>