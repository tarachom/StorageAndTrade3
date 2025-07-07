<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="text" indent="yes" />

    <!-- Файл -->
    <xsl:param name="File" />

    <!-- Простори імен -->
    <xsl:param name="NameSpaceGeneratedCode" />
    <xsl:param name="NameSpace" />

    <xsl:template match="root">

        <xsl:choose>

            <xsl:when test="$File = 'Triggers'">
                <xsl:call-template name="TablePartTriggers" />
            </xsl:when>
            <xsl:when test="$File = 'TablePart'">
                <xsl:call-template name="TablePart" />
            </xsl:when>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="TablePartList" />
            </xsl:when>
            <xsl:when test="$File = 'Report'">
                <xsl:call-template name="TablePartReport" />
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
            <xsl:when test="Type = 'composite_text'">NameAndText</xsl:when>
            <xsl:when test="Type = 'enum'"><xsl:value-of select="substring-after(Pointer, '.')"/></xsl:when>
            <xsl:when test="Type = 'bytea'">byte[]</xsl:when>
            <xsl:when test="Type = 'uuid[]'">Guid[]</xsl:when>
        </xsl:choose>    
    </xsl:template>

    <!-- Для конструкторів. Значення поля по замовчуванню відповідно до типу -->
    <xsl:template name="DefaultFieldValue">
        <xsl:choose>
            <xsl:when test="Type = 'string'">""</xsl:when>
            <xsl:when test="Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'bytea' or Type = 'uuid[]'">[]</xsl:when>
            <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'enum'">0</xsl:when>
            <xsl:when test="Type = 'boolean'">false</xsl:when>
            <xsl:when test="Type = 'time'">DateTime.MinValue.TimeOfDay</xsl:when>
            <xsl:when test="Type = 'date' or Type = 'datetime'">DateTime.MinValue</xsl:when>
            <xsl:when test="Type = 'pointer'">new <xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer()</xsl:when>
            <xsl:when test="Type = 'any_pointer'">new Guid()</xsl:when>
            <xsl:when test="Type = 'composite_pointer'">new UuidAndText()</xsl:when>
            <xsl:when test="Type = 'composite_text'">new NameAndText()</xsl:when>
        </xsl:choose>
    </xsl:template>

<!--- 
//
// ============================ Triggers ============================
//
-->

    <xsl:template name="TablePartTriggers">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>

        <!-- Назви функцій -->
        <xsl:variable name="TriggerFunctions" select="TablePart/TriggerFunctions"/>

        <xsl:variable name="OwnerTypeName">
            <xsl:choose>
                <xsl:when test="TablePart/OwnerType = 'Directory'">Довідник</xsl:when>
                <xsl:when test="TablePart/OwnerType = 'Document'">Документ</xsl:when>
                <xsl:otherwise>[...]</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:variable name="OwnerTypeNameSpace">
            <xsl:choose>
                <xsl:when test="TablePart/OwnerType = 'Directory'">Довідники</xsl:when>
                <xsl:when test="TablePart/OwnerType = 'Document'">Документи</xsl:when>
                <xsl:otherwise>[...]</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:variable name="OwnerName" select="TablePart/OwnerName"/>

/*
        <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Triggers.cs
        Тригери табличної частини <xsl:value-of select="$TablePartName"/>
*/

using AccountingSoftware;

namespace <xsl:value-of select="$NameSpaceGeneratedCode"/>.<xsl:value-of select="$OwnerTypeNameSpace"/>
{
    static class <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Triggers
    {
        public static async ValueTask <xsl:value-of select="$TriggerFunctions/BeforeSave"/>(<xsl:value-of select="$OwnerName"/>_Objest <xsl:value-of select="$OwnerTypeName"/>Обєкт, <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart ТабличнаЧастина)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/AfterSave"/>(<xsl:value-of select="$OwnerName"/>_Objest <xsl:value-of select="$OwnerTypeName"/>Обєкт, <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart ТабличнаЧастина)
        {
            await ValueTask.FromResult(true);
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Таблична Частина ============================
//
-->

    <!-- Таблична Частина -->
    <xsl:template name="TablePart">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>
        <xsl:variable name="IncludeIconColumn" select="TablePart/IncludeIconColumn"/>
        <xsl:variable name="FieldsTL" select="TablePart/ElementFields/Field"/>

        <xsl:variable name="OwnerExist" select="TablePart/OwnerExist"/>
        <xsl:variable name="OwnerType" select="TablePart/OwnerType"/>
        <xsl:variable name="OwnerTypeName">
            <xsl:choose>
                <xsl:when test="TablePart/OwnerType = 'Directory'">Довідник</xsl:when>
                <xsl:when test="TablePart/OwnerType = 'Document'">Документ</xsl:when>
                <xsl:when test="TablePart/OwnerType = 'Constants'">Константа</xsl:when>
                <xsl:otherwise>[...]</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:variable name="OwnerName" select="TablePart/OwnerName"/>
        <xsl:variable name="OwnerBlockName">
            <xsl:if test="TablePart/OwnerType = 'Constants' and normalize-space(TablePart/OwnerBlockName) != ''">
                <xsl:value-of select="concat(normalize-space(TablePart/OwnerBlockName), '.')"/>
            </xsl:if>
        </xsl:variable>
/*
        <xsl:value-of select="$OwnerName"/>_ТабличнаЧастина_<xsl:value-of select="$TablePartName"/>.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Перелічення;
<xsl:if test="$IncludeIconColumn = '1'">using InterfaceGtk3.Іконки;</xsl:if>

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$OwnerName"/>_ТабличнаЧастина_<xsl:value-of select="$TablePartName"/> : <xsl:value-of select="$OwnerTypeName"/>ТабличнаЧастина
    {
        <xsl:choose>
            <xsl:when test="$OwnerType = 'Constants'">
        <xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart Таблиця { get; set; } = new();
            </xsl:when>
            <xsl:otherwise>
        public <xsl:value-of select="$OwnerName"/>_Objest? ЕлементВласник { get; set; }
            </xsl:otherwise>
        </xsl:choose>
        
        #region Записи

        enum Columns
        {
            <xsl:if test="$IncludeIconColumn = '1'">Image,</xsl:if>
            <xsl:for-each select="$FieldsTL">
                <xsl:value-of select="Name"/>,
            </xsl:for-each>
        }

        ListStore Store = new ListStore([
            <xsl:if test="$IncludeIconColumn = '1'">typeof(Gdk.Pixbuf),</xsl:if>
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
                    <xsl:if test="$IncludeIconColumn = '1'">ДляТабличногоСписку.Normal,</xsl:if>
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
            <xsl:if test="$IncludeIconColumn = '1'">TreeViewGrid.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", (int)Columns.Image));</xsl:if>
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
                        <xsl:choose>
                            <xsl:when test="AutomaticNumbering = '1'">
                TreeViewColumn column = new TreeViewColumn("<xsl:value-of select="Caption"/>", new CellRendererText() { Xalign = 1 }, "text", (int)Columns.<xsl:value-of select="Name"/>) { Alignment = 1, MinWidth = 30 };
                            </xsl:when>
                            <xsl:otherwise>
                CellRendererText cellNumber = new CellRendererText() { Editable = true, Xalign = 1 };
                cellNumber.Edited += EditCell;
                cellNumber.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("<xsl:value-of select="Caption"/>", cellNumber, "text", (int)Columns.<xsl:value-of select="Name"/>) { Resizable = true, Alignment = 1, MinWidth = 100 };
                column.SetCellDataFunc(cellNumber, new TreeCellDataFunc(NumericCellDataFunc));
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'date' or Type = 'datetime' or Type = 'time'">
                CellRendererText cellDateTime = new CellRendererText() { Editable = true };
                cellDateTime.Edited += EditCell;
                cellDateTime.EditingStarted += EditingStarted;
                TreeViewColumn column = new TreeViewColumn("<xsl:value-of select="Caption"/>", cellDateTime, "text", (int)Columns.<xsl:value-of select="Name"/>) { Resizable = true, MinWidth = 100 };
                column.SetCellDataFunc(cellDateTime, new TreeCellDataFunc(DateTimeCellDataFunc));
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                CellRendererToggle cellToggle = new CellRendererToggle();
                cellToggle.Toggled += EditCell;
                TreeViewColumn column = new TreeViewColumn("<xsl:value-of select="Caption"/>", cellToggle, "active", (int)Columns.<xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:otherwise>
                CellRendererText cellText = new CellRendererText() { Editable = true };
                cellText.Edited += EditCell;
                cellText.EditingStarted += EditingStarted;
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
            <xsl:choose>
                <xsl:when test="$OwnerType = 'Constants'">
            Таблиця.QuerySelect.Clear();
                <xsl:for-each select="$FieldsTL[AutomaticNumbering = '1']">
            Таблиця.QuerySelect.Order.Add(<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>, SelectOrder.ASC);
                </xsl:for-each>
            await Таблиця.Read();
                </xsl:when>
                <xsl:otherwise>
            if (ЕлементВласник != null) <!-- відкриття if -->
            {
                <xsl:variable name="OrderFields">
                    <xsl:for-each select="$FieldsTL[AutomaticNumbering = '1']">
                        <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>
                        <xsl:text>,</xsl:text>
                    </xsl:for-each>
                </xsl:variable>
                ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.FillJoin([<xsl:value-of select="$OrderFields"/>]);
                await ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.Read();
                </xsl:otherwise>
            </xsl:choose>

            Записи.Clear();
            Store.Clear();

            <xsl:variable name="InRecords">
                <xsl:choose>
                    <xsl:when test="$OwnerType = 'Constants'">Таблиця</xsl:when>
                    <xsl:otherwise>ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart</xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            foreach (<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.Record record in <xsl:value-of select="$InRecords"/>.Records)
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
            <xsl:if test="$OwnerType != 'Constants'">}</xsl:if><!-- закриття if -->
            SelectRowActivated();
        }

        public override async ValueTask SaveRecords()
        {
            <xsl:if test="$OwnerType != 'Constants'"><!-- відкриття if -->
            if (ЕлементВласник != null)
            {
            </xsl:if>

            <xsl:variable name="Records">
                <xsl:choose>
                    <xsl:when test="$OwnerType = 'Constants'">Таблиця</xsl:when>
                    <xsl:otherwise>ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart</xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <xsl:value-of select="$Records"/>.Records.Clear();
            foreach (Запис запис in Записи)
            {
                <xsl:value-of select="$Records"/>.Records.Add(new <xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.Record()
                {
                    UID = запис.ID,
                    <xsl:for-each select="$FieldsTL">
                    <xsl:value-of select="Name"/> = запис.<xsl:value-of select="Name"/>,
                    </xsl:for-each>
                });
            }
            await <xsl:value-of select="$Records"/>.Save(true);
            await LoadRecords();
            <xsl:if test="$OwnerType != 'Constants'">}</xsl:if><!-- закриття if -->
        }

        /*public string КлючовіСловаДляПошуку()
        {
            <xsl:variable name="FieldsForKeyWords" select="$FieldsTL[Type = 'string' or Type = 'pointer']" />
            string keyWords = "";
            foreach (Запис запис in Записи)<xsl:if test="count($FieldsForKeyWords) != 0">
                keyWords += $"\n<xsl:for-each select="$FieldsForKeyWords">
                    <xsl:choose>
                        <xsl:when test="Type = 'pointer'"> {запис.<xsl:value-of select="Name"/>.Назва}</xsl:when>
                        <xsl:when test="Type = 'string'"> {запис.<xsl:value-of select="Name"/>}</xsl:when>
                    </xsl:choose>
                </xsl:for-each>";
            </xsl:if>
            return keyWords;
        }*/

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
                        }<xsl:if test="MultipleSelect = '1'">,
                        CallBack_OnMultipleSelectPointer = async (UnigueID[] selectPointers) =&gt;
                        {
                            foreach (var selectPointer in selectPointers)
                            {
                                (Запис запис, TreeIter iter) = НовийЗапис();

                                запис.<xsl:value-of select="Name"/> = new <xsl:value-of select="$namePointer"/>_Pointer(selectPointer);
                                await Запис.ПісляЗміни_<xsl:value-of select="Name"/>(запис);
                                Store.SetValues(iter, запис.ToArray());
                            }
                        }</xsl:if>
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

        <xsl:variable name="FieldsTL_Select" select="$FieldsTL[Type = 'string' or 
            (Type = 'integer' and AutomaticNumbering != '1') or 
            Type = 'numeric' or Type = 'enum' or Type = 'date' or Type = 'datetime' or Type = 'time']" />

        <xsl:if test="count($FieldsTL_Select) != 0">
        protected override void ChangeCell(TreeIter iter, int rowNumber, int colNumber, string newText)
        {
            Запис запис = Записи[rowNumber];
            switch ((Columns)colNumber)
            {
                <xsl:for-each select="$FieldsTL_Select">
                <xsl:text>case Columns.</xsl:text><xsl:value-of select="Name"/>: { <xsl:choose>
                    <xsl:when test="Type = 'string'">запис.<xsl:value-of select="Name"/> = newText;</xsl:when>
                    <xsl:when test="Type = 'integer'">var (check, value) = Validate.IsInt(newText); if (check) запис.<xsl:value-of select="Name"/> = value;</xsl:when>
                    <xsl:when test="Type = 'numeric'">var (check, value) = Validate.IsDecimal(newText); if (check) запис.<xsl:value-of select="Name"/> = value;</xsl:when>
                    <xsl:when test="Type = 'enum'">запис.<xsl:value-of select="Name"/> = ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_FindByName(newText);</xsl:when>
                    <xsl:when test="Type = 'date' or Type = 'datetime'">var (check, value) = Validate.IsDateTime(newText); if (check) запис.<xsl:value-of select="Name"/> = value;</xsl:when>
                    <xsl:when test="Type = 'time'">var (check, value) = Validate.IsTime(newText); if (check) запис.<xsl:value-of select="Name"/> = value;</xsl:when>
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

        <xsl:variable name="FieldsTL_Select_NumFunc" select="$FieldsTL[(Type = 'integer' and AutomaticNumbering != '1') or Type = 'numeric']" />

        <xsl:if test="count($FieldsTL_Select_NumFunc) != 0">
        void NumericCellDataFunc(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            if (GetColIndex(column, out int colNumber))
            {
                int rowNumber = int.Parse(Store.GetPath(iter).ToString());
                if (rowNumber &gt;= 0 &amp;&amp; rowNumber &lt; Записи.Count)
                {
                    Запис запис = Записи[rowNumber];

                    CellRendererText cellText = (CellRendererText)cell;
                    cellText.Foreground = "green";

                    switch ((Columns)colNumber)
                    {
                        <xsl:for-each select="$FieldsTL_Select_NumFunc">
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
        }
        </xsl:if>

        <xsl:if test="count($FieldsTL[Type = 'date' or Type = 'datetime' or Type = 'time']) != 0">
        void DateTimeCellDataFunc(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            if (GetColIndex(column, out int colNumber))
            {
                int rowNumber = int.Parse(Store.GetPath(iter).ToString());
                if (rowNumber &gt;= 0 &amp;&amp; rowNumber &lt; Записи.Count)
                {
                    Запис запис = Записи[rowNumber];

                    CellRendererText cellText = (CellRendererText)cell;
                    cellText.Foreground = "green";

                    switch ((Columns)colNumber)
                    {
                        <xsl:for-each select="$FieldsTL[Type = 'date' or Type = 'datetime' or Type = 'time']">
                        case Columns.<xsl:value-of select="Name"/>:
                        {
                            cellText.Text = запис.<xsl:value-of select="Name"/>.ToString(<xsl:choose>
                                    <xsl:when test="Type = 'date'">"dd.MM.yyyy"</xsl:when>
                                    <xsl:when test="Type = 'datetime'">"dd.MM.yyyy HH:mm:ss"</xsl:when>
                                    <xsl:when test="Type = 'time'">@"hh\:mm\:ss"</xsl:when>
                                </xsl:choose>);
                            break;
                        }
                        </xsl:for-each>
                        default: break;
                    }
                }
            }
        }
        </xsl:if>

        #endregion

        #region ToolBar

        (Запис запис, TreeIter iter) НовийЗапис()
        {
            Запис запис = new Запис();
            Записи.Add(запис);

            TreeIter iter = Store.AppendValues(запис.ToArray());
            TreeViewGrid.SetCursor(Store.GetPath(iter), TreeViewGrid.Columns[0], false);

            return (запис, iter);
        }

        protected override void AddRecord()
        {
            НовийЗапис();
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
        Таблична Частина Список
*/



    </xsl:template>

<!--- 
//
// ============================ Звіт ============================
//
-->

    <!-- Список -->
    <xsl:template name="TablePartReport">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>
        <xsl:variable name="FieldsTL" select="TablePart/ElementFields/Field"/>

        <xsl:variable name="OwnerName" select="TablePart/OwnerName"/>
        <xsl:variable name="OwnerBlockName">
            <xsl:if test="TablePart/OwnerType = 'Constants' and normalize-space(TablePart/OwnerBlockName) != ''">
                <xsl:value-of select="concat(normalize-space(TablePart/OwnerBlockName), '.')"/>
            </xsl:if>
        </xsl:variable>
/*
        <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Звіт.cs
        Звіт
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Константи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриНакопичення;

namespace <xsl:value-of select="$NameSpace"/>
{
    public static class <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Звіт
    {
        public static async ValueTask Сформувати()
        {
            <xsl:variable name="CountFieldsTL" select="count(TablePart/ElementFields/Field)"/>
            string query = $@"
SELECT
    <xsl:for-each select="$FieldsTL">
    <xsl:choose>
        <xsl:when test="Type = 'pointer'">
            <xsl:variable name="name" select="Name" />
            <xsl:variable name="nameGroup" select="substring-before(Pointer, '.')" />
            <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
            <xsl:variable name="index">
                <xsl:for-each select="$FieldsTL[Type = 'pointer' and $namePointer = substring-after(Pointer, '.')]">
                    <xsl:if test="$name = Name and position() &gt; 1"><xsl:value-of select="position()"/></xsl:if>
                </xsl:for-each>
            </xsl:variable>
            <xsl:value-of select="$TablePartName"/>.{<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>,
            <xsl:choose>
                <xsl:when test="PresetntationFields/@Count = 1">
                    <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/>.{<xsl:value-of select="$namePointer"/>_Const.<xsl:value-of select="PresetntationFields/Field"/><xsl:text>}</xsl:text>
                </xsl:when>
                <xsl:when test="PresetntationFields/@Count &gt; 1">
                    <xsl:text>concat_ws (', '</xsl:text>
                    <xsl:for-each select="PresetntationFields/Field">
                        <xsl:value-of select="concat(', ', $nameGroup, '_', $namePointer, $index, '.{', $namePointer, '_Const.', text(), '}')"/>
                    </xsl:for-each>
                    <xsl:text>)</xsl:text>
                </xsl:when>
                <xsl:otherwise>'#'</xsl:otherwise>
            </xsl:choose> AS <xsl:value-of select="concat(Name, '_Назва')"/>
        </xsl:when>
        <xsl:otherwise>
            <xsl:value-of select="$TablePartName"/>.{<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>
        </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="position() != $CountFieldsTL">,
    </xsl:if>
    </xsl:for-each>
FROM
    {<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.TABLE} AS <xsl:value-of select="$TablePartName"/>
    <xsl:for-each select="$FieldsTL[Type = 'pointer']">
        <xsl:variable name="name" select="Name" />
        <xsl:variable name="nameGroup" select="substring-before(Pointer, '.')" />
        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
        <xsl:variable name="index">
            <xsl:for-each select="$FieldsTL[Type = 'pointer' and $namePointer = substring-after(Pointer, '.')]">
                <xsl:if test="$name = Name and position() &gt; 1"><xsl:value-of select="position()"/></xsl:if>
            </xsl:for-each>
        </xsl:variable>
    LEFT JOIN {<xsl:value-of select="$namePointer"/>_Const.TABLE} AS <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/> ON <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/>.uid = 
        <xsl:value-of select="$TablePartName"/>.{<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>}
    </xsl:for-each>
";
            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "<xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Звіт",
                Caption = "<xsl:value-of select="$TablePartName"/>",
                Query = query,
                GetInfo = () =&gt; ValueTask.FromResult("")
            };

            <xsl:for-each select="$FieldsTL">
                <xsl:choose>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>_Назва", new("<xsl:value-of select="Caption"/>", "<xsl:value-of select="Name"/>", <xsl:value-of select="$namePointer"/>_Const.POINTER));
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric'">
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>", new("<xsl:value-of select="Caption"/>", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>", new("<xsl:value-of select="Caption"/>"));
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:for-each>
            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
    </xsl:template>

</xsl:stylesheet>