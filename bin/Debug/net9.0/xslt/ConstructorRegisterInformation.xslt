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
            <xsl:when test="$File = 'Element'">
                <xsl:call-template name="RegisterInformationElement" />
            </xsl:when>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="RegisterInformationList" />
            </xsl:when>
            <xsl:when test="$File = 'Report'">
                <xsl:call-template name="RegisterInformationReport" />
            </xsl:when>
        </xsl:choose>

    </xsl:template>

<!--- 
//
// ============================ Елемент ============================
//
-->

    <!-- Елемент -->
    <xsl:template name="RegisterInformationElement">
        <xsl:variable name="RegisterInformationName" select="RegisterInformation/Name"/>
        <!--<xsl:variable name="Fields" select="RegisterInformation/Fields/Field"/>-->
        <xsl:variable name="FieldsTL" select="RegisterInformation/ElementFields/Field"/>

/*
        <xsl:value-of select="$RegisterInformationName"/>_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриВідомостей;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$RegisterInformationName"/>_Елемент : РегістриВідомостейЕлемент
    {
        public <xsl:value-of select="$RegisterInformationName"/>_Objest Елемент { get; init; } = new <xsl:value-of select="$RegisterInformationName"/>_Objest();
        DateTimeControl Період = new DateTimeControl();

        #region Fields
        <xsl:for-each select="$FieldsTL">
            <xsl:variable name="Size">
                <xsl:choose>
                    <xsl:when test="Size != '0'"><xsl:value-of select="Size"/></xsl:when>
                    <xsl:otherwise>500</xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <xsl:choose>
                <xsl:when test="Type = 'string'">
                    <xsl:choose>
                        <xsl:when test="Multiline = '1'">
                    <xsl:text>TextView </xsl:text><xsl:value-of select="Name"/> = new TextView() { WrapMode = WrapMode.Word };
                        </xsl:when>
                        <xsl:otherwise>
                    <xsl:text>Entry </xsl:text><xsl:value-of select="Name"/> = new Entry() { WidthRequest = <xsl:value-of select="$Size"/> };
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:when>
                <xsl:when test="Type = 'integer'">
                    <xsl:text>IntegerControl </xsl:text><xsl:value-of select="Name"/> = new IntegerControl();
                </xsl:when>
                <xsl:when test="Type = 'numeric'">
                    <xsl:text>NumericControl </xsl:text><xsl:value-of select="Name"/> = new NumericControl();
                </xsl:when>
                <xsl:when test="Type = 'boolean'">
                    <xsl:text>CheckButton </xsl:text><xsl:value-of select="Name"/> = new CheckButton("<xsl:value-of select="Name"/>");
                </xsl:when>
                <xsl:when test="Type = 'date' or Type = 'datetime'">
                    <xsl:text>DateTimeControl </xsl:text><xsl:value-of select="Name"/> = new DateTimeControl()<xsl:if test="Type = 'date'">{ OnlyDate = true }</xsl:if>;
                </xsl:when>
                <xsl:when test="Type = 'time'">
                    <xsl:text>TimeControl </xsl:text><xsl:value-of select="Name"/> = new TimeControl();
                </xsl:when>
                <xsl:when test="Type = 'composite_pointer'">
                    <xsl:text>CompositePointerControl </xsl:text><xsl:value-of select="Name"/> = new CompositePointerControl() { BoundConfType = "Довідники.<xsl:value-of select="$RegisterInformationName"/>.<xsl:value-of select="Name"/>" };
                </xsl:when>
                <xsl:when test="Type = 'composite_text'">
                    <xsl:text>//NameAndText </xsl:text><xsl:value-of select="Name"/> = new NameAndText();
                </xsl:when>
                <xsl:when test="Type = 'pointer'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    <xsl:value-of select="$namePointer"/>_PointerControl <xsl:value-of select="Name"/> = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="Caption"/>", WidthPresentation = <xsl:value-of select="$Size"/> };
                </xsl:when>
                <xsl:when test="Type = 'enum'">
                    <xsl:text>ComboBoxText </xsl:text><xsl:value-of select="Name"/> = new ComboBoxText();
                </xsl:when>
                <xsl:when test="Type = 'any_pointer'">
                    <xsl:text>//Guid </xsl:text><xsl:value-of select="Name"/> = new Guid();
                </xsl:when>
                <xsl:when test="Type = 'bytea'">
                    <xsl:text>//byte[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'string[]'">
                    <xsl:text>//string[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'integer[]'">
                    <xsl:text>//int[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'numeric[]'">
                    <xsl:text>//decimal[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'uuid[]'">
                    <xsl:text>//Guid[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
            </xsl:choose>
        </xsl:for-each>

        #endregion

        public <xsl:value-of select="$RegisterInformationName"/>_Елемент() : base() 
        { 
            //Елемент.UnigueIDChanged += UnigueIDChanged;
            //Елемент.CaptionChanged += CaptionChanged;

            <xsl:for-each select="$FieldsTL[Type = 'enum']">
                foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_List())
                    <xsl:value-of select="Name"/>.Append(field.Value.ToString(), field.Name);
            </xsl:for-each>
        }

        protected override void CreatePack1(Box vBox)
        {
                //Період
                CreateField(vBox, "Період:", Період);
            <xsl:for-each select="$FieldsTL">
                //<xsl:value-of select="Name"/>
                <xsl:choose>
                    <xsl:when test="Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:choose>
                            <xsl:when test="Type = 'string' and Multiline = '1'">
                            <xsl:variable name="Size">
                                <xsl:choose>
                                    <xsl:when test="Size != '0'"><xsl:value-of select="Size"/></xsl:when>
                                    <xsl:otherwise>500</xsl:otherwise>
                                </xsl:choose>
                            </xsl:variable>
                            <xsl:variable name="Height">
                                <xsl:choose>
                                    <xsl:when test="Height != '0'"><xsl:value-of select="Height"/></xsl:when>
                                    <xsl:otherwise>200</xsl:otherwise>
                                </xsl:choose>
                            </xsl:variable>
                CreateFieldView(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>, <xsl:value-of select="$Size"/>, <xsl:value-of select="$Height"/>);
                            </xsl:when>
                            <xsl:otherwise>
                CreateField(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>);
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer' or Type = 'boolean'">
                CreateField(vBox, null, <xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                CreateField(vBox, null, <xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                CreateField(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>);
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        protected override void CreatePack2(Box vBox)
        {
            
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {            
            if (IsNew)
                Елемент.New();

            Період.Value = Елемент.Period;
            <xsl:for-each select="$FieldsTL">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:choose>
                            <xsl:when test="Multiline = '1'">
                                <xsl:value-of select="Name"/>.Buffer.Text = Елемент.<xsl:value-of select="Name"/>;
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="Name"/>.Text = Елемент.<xsl:value-of select="Name"/>;
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:value-of select="Name"/>.Value = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        <xsl:value-of select="Name"/>.Active = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:value-of select="Name"/>.Pointer = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:value-of select="Name"/>.Pointer = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:value-of select="Name"/>.ActiveId = Елемент.ToString();
                        <xsl:text>//if (</xsl:text><xsl:value-of select="Name"/>.Active == -1) <xsl:value-of select="Name"/>.Active = 0;
                    </xsl:when>
                    <xsl:when test="Type = 'any_pointer' or Type = 'composite_text' or Type = 'bytea' or Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'uuid[]'">
                        <xsl:text>//</xsl:text><xsl:value-of select="Name"/> = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        protected override void GetValue()
        {
            Елемент.Period = Період.Value;
            <xsl:for-each select="$FieldsTL">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:choose>
                            <xsl:when test="Multiline = '1'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Buffer.Text;
                            </xsl:when>
                            <xsl:otherwise>
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Text;
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Active;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = ПсевдонімиПерелічення.<xsl:value-of select="$namePointer"/>_FindByName(<xsl:value-of select="Name"/>.ActiveId);
                    </xsl:when>
                    <xsl:when test="Type = 'any_pointer' or Type = 'composite_text' or Type = 'bytea' or Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'uuid[]'">
                        <xsl:text>//Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        #endregion

        protected override async ValueTask&lt;bool&gt; Save()
        {
            bool isSaved = false;
            try
            {
                await Елемент.Save();
                isSaved = true;
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(new UuidAndText(UnigueID.UGuid), Caption, ex);
            }
            return isSaved;
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Список ============================
//
-->

    <!-- Список -->
    <xsl:template name="RegisterInformationList">
        <xsl:variable name="RegisterInformationName" select="RegisterInformation/Name"/>
        <xsl:variable name="TabularList" select="RegisterInformation/TabularList"/>

/*     
        <xsl:value-of select="$RegisterInformationName"/>.cs
        Список

        Табличний список - <xsl:value-of select="$TabularList"/>
*/

using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриВідомостей;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриВідомостей.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>.РегістриВідомостей
{
    public class <xsl:value-of select="$RegisterInformationName"/> : РегістриВідомостейЖурнал
    {
        public <xsl:value-of select="$RegisterInformationName"/>() : base()
        {
            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

             //period
            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            <xsl:value-of select="$RegisterInformationName"/>_Елемент page = new <xsl:value-of select="$RegisterInformationName"/>_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () =&gt; page);
            page.SetValue();
        }
        protected override async ValueTask Delete(UnigueID unigueID)
        {
            <xsl:value-of select="$RegisterInformationName"/>_Objest Обєкт = new <xsl:value-of select="$RegisterInformationName"/>_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask&lt;UnigueID?&gt; Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$RegisterInformationName"/>_Objest Обєкт = new <xsl:value-of select="$RegisterInformationName"/>_Objest();
            if (await Обєкт.Read(unigueID))
            {
                <xsl:value-of select="$RegisterInformationName"/>_Objest Новий = Обєкт.Copy();
                await Новий.Save();
                return Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "РегістриВідомостей.<xsl:value-of select="$RegisterInformationName"/>";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
    </xsl:template>

<!--- 
//
// ============================ Звіт ============================
//
-->

    <!-- Список -->
    <xsl:template name="RegisterInformationReport">
        <xsl:variable name="RegisterInformationName" select="RegisterInformation/Name"/>
        <xsl:variable name="FieldsTL" select="RegisterInformation/ElementFields/Field"/>

/*
        <xsl:value-of select="$RegisterInformationName"/>_Звіт.cs
        Звіт
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриВідомостей;

namespace <xsl:value-of select="$NameSpace"/>
{
    public static class <xsl:value-of select="$RegisterInformationName"/>_Звіт
    {
        public static async ValueTask Сформувати()
        {
            <xsl:variable name="CountFieldsTL" select="count($FieldsTL)"/>
            string query = $@"
SELECT
    <xsl:value-of select="$RegisterInformationName"/>.period,
    <xsl:value-of select="$RegisterInformationName"/>.owner,
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
            <xsl:value-of select="$RegisterInformationName"/>.{<xsl:value-of select="$RegisterInformationName"/>_Const.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>,
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
            <xsl:value-of select="$RegisterInformationName"/>.{<xsl:value-of select="$RegisterInformationName"/>_Const.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>
        </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="position() != $CountFieldsTL">,
    </xsl:if>
    </xsl:for-each>
FROM
    {<xsl:value-of select="$RegisterInformationName"/>_Const.TABLE} AS <xsl:value-of select="$RegisterInformationName"/>
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
        <xsl:value-of select="$RegisterInformationName"/>.{<xsl:value-of select="$RegisterInformationName"/>_Const.<xsl:value-of select="Name"/>}
    </xsl:for-each>
";
            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "<xsl:value-of select="$RegisterInformationName"/>_Звіт",
                Caption = "<xsl:value-of select="$RegisterInformationName"/>",
                Query = query,
                GetInfo = () =&gt; ValueTask.FromResult("")
            };

            Звіт.ColumnSettings.Add("period", new("Період"));
            Звіт.ColumnSettings.Add("owner", new("owner"));
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