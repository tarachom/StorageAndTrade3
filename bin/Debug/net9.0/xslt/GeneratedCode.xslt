<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes" />
  
  <!-- Для задання типу поля -->
  <xsl:template name="FieldType">
    <xsl:choose>
      <xsl:when test="Type = 'string'">
        <xsl:text>string</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]'">
        <xsl:text>string[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer'">
        <xsl:text>int</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer[]'">
        <xsl:text>int[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric'">
        <xsl:text>decimal</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric[]'">
        <xsl:text>decimal[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text>bool</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'time'">
        <xsl:text>TimeSpan</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'date' or Type = 'datetime'">
        <xsl:text>DateTime</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'pointer'">
        <xsl:value-of select="Pointer"/>
        <xsl:text>_Pointer</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'any_pointer'">
        <xsl:text>Guid</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'composite_pointer'">
        <xsl:text>UuidAndText</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'enum'">
        <xsl:value-of select="Pointer"/>
      </xsl:when>
      <xsl:when test="Type = 'bytea'">
        <xsl:text>byte[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'uuid[]'">
        <xsl:text>Guid[]</xsl:text>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  
  <!-- Для конструкторів. Значення поля по замовчуванню відповідно до типу -->
  <xsl:template name="DefaultFieldValue">
    <xsl:choose>
      <xsl:when test="Type = 'string'">
        <xsl:text>""</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]'">
        <xsl:text>[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer[]'">
        <xsl:text>[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric[]'">
        <xsl:text>[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text>false</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'time'">
        <xsl:text>DateTime.MinValue.TimeOfDay</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'date' or Type = 'datetime'">
        <xsl:text>DateTime.MinValue</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'pointer'">
        <xsl:text>new </xsl:text>
        <xsl:value-of select="Pointer"/>
        <xsl:text>_Pointer()</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'any_pointer'">
        <xsl:text>new Guid()</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'composite_pointer'">
        <xsl:text>new UuidAndText()</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'enum'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'bytea'">
        <xsl:text>[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'uuid[]'">
        <xsl:text>[]</xsl:text>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <!-- Для параметрів функцій. Значення параметра по замовчуванню відповідно до типу -->
  <xsl:template name="DefaultParamValue">
    <xsl:choose>
      <xsl:when test="Type = 'string'">
        <xsl:text>""</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer[]'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric[]'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text>false</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'time'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'date' or Type = 'datetime'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'pointer'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'any_pointer'">
        <xsl:text>Guid.Empty</xsl:text>
        </xsl:when>
      <xsl:when test="Type = 'composite_pointer'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'enum'">
        <xsl:text>0</xsl:text>
      </xsl:when>
	    <xsl:when test="Type = 'bytea'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'uuid[]'">
        <xsl:text>null</xsl:text>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  
  <!-- Для присвоєння значення полям. Для масиву полів-->
  <xsl:template name="ReadFieldValue">
     <xsl:param name="BaseFieldContainer" />
     
     <xsl:choose>
        <xsl:when test="Type = 'string'">
          <xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"].ToString() ?? ""</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'string[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(string[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : []</xsl:text>
        </xsl:when>
       <xsl:when test="Type = 'integer'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(int)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'integer[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(int[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : []</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'numeric'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(decimal)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'numeric[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(decimal[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : []</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'boolean'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(bool)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : false</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'time'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>TimeSpan.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]?.ToString() ?? DateTime.MinValue.TimeOfDay.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue.TimeOfDay</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'date' or Type = 'datetime'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>DateTime.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"].ToString() ?? DateTime.MinValue.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'pointer'">
          <xsl:text>new </xsl:text><xsl:value-of select="Pointer"/>
          <xsl:text>_Pointer(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"])</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'any_pointer'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(Guid)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : Guid.Empty</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'composite_pointer'">
		    <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(UuidAndText)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : new UuidAndText()</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'enum'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(</xsl:text><xsl:value-of select="Pointer"/><xsl:text>)</xsl:text>
          <xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
		    <xsl:when test="Type = 'bytea'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(byte[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : []</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'uuid[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(Guid[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : []</xsl:text>
        </xsl:when>
     </xsl:choose>
  </xsl:template>

  <!-- Для присвоєння значення полям 2. Для одного значення без використання масиву полів-->
  <xsl:template name="ReadFieldValue2">
     <xsl:param name="BaseFieldContainer" />
     
     <xsl:choose>
        <xsl:when test="Type = 'string'">
          <xsl:value-of select="$BaseFieldContainer"/><xsl:text>.ToString() ?? ""</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'string[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(string[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : []</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'integer'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(int)</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'integer[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(int[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : []</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'numeric'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(decimal)</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'numeric[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(decimal[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : []</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'boolean'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(bool)</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : false</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'time'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>TimeSpan.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>?.ToString() ?? DateTime.MinValue.TimeOfDay.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue.TimeOfDay</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'date' or Type = 'datetime'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>DateTime.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>.ToString() ?? DateTime.MinValue.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'pointer'">
          <xsl:text>new </xsl:text><xsl:value-of select="Pointer"/>
          <xsl:text>_Pointer(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>)</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'any_pointer'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(Guid)</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : Guid.Empty</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'composite_pointer'">
		    <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(UuidAndText)</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : new UuidAndText()</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'enum'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(</xsl:text><xsl:value-of select="Pointer"/><xsl:text>)</xsl:text>
          <xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
		    <xsl:when test="Type = 'bytea'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(byte[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : []</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'uuid[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(Guid[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/>
          <xsl:text> : []</xsl:text>
        </xsl:when>
     </xsl:choose>
  </xsl:template>

  <!-- Для перетворення поля в ХМЛ стрічку -->
  <!--<xsl:template name="SerializeFieldValue">
    <xsl:text>"&lt;</xsl:text><xsl:value-of select="Name"/><xsl:text>&gt;" + </xsl:text>
    <xsl:choose>
      <xsl:when test="Type = 'enum'">
        <xsl:text>((int)</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text>(</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string'">
        <xsl:text>"&lt;![CDATA[" + </xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]'">
        <xsl:text>ArrayToXml&lt;string&gt;.Convert(</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer[]'">
        <xsl:text>ArrayToXml&lt;int&gt;.Convert(</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric[]'">
        <xsl:text>ArrayToXml&lt;decimal&gt;.Convert(</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'uuid[]'">
        <xsl:text>ArrayToXml&lt;Guid&gt;.Convert(</xsl:text>
      </xsl:when>
    </xsl:choose>
    <xsl:value-of select="Name"/>
    <xsl:choose>
      <xsl:when test="Type = 'enum'">
        <xsl:text>).ToString()</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text> == true ? "1" : "0")</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer' or Type = 'numeric' or 
                Type = 'date' or Type = 'datetime' or Type = 'time' or
                Type = 'pointer' or Type = 'any_pointer' or Type = 'composite_pointer'">
        <xsl:text>.ToString()</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string'">
        <xsl:text> + "]]&gt;"</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'uuid[]'">
        <xsl:text>).ToString()</xsl:text>
      </xsl:when>
    </xsl:choose> 
    <xsl:text> + "&lt;/</xsl:text><xsl:value-of select="Name"/><xsl:text>&gt;" </xsl:text>
  </xsl:template>-->
  
  <!-- Документування коду -->
  <!--<xsl:template name="CommentSummary">
    <xsl:variable name="normalize_space_Desc" select="normalize-space(Desc)" />
    <xsl:if test="$normalize_space_Desc != ''">
    <xsl:text>///&lt;summary</xsl:text>&gt;
    <xsl:text>///</xsl:text>
    <xsl:value-of select="$normalize_space_Desc"/>.
    <xsl:text>///&lt;/summary&gt;</xsl:text>
    </xsl:if>
  </xsl:template>-->
  
  <!-- Функція очищення регістрів для Документ_Object та Документ_Pointer -->
  <xsl:template name="ClearRegAccumFunction">
        /* Очищення регістрів накопичення */
        async void ClearRegAccum()
        {
          <xsl:choose>
            <xsl:when test="count(AllowRegisterAccumulation/Name) &gt; 0">
            if(!this.UnigueID.IsEmpty())
            {
              <xsl:for-each select="AllowRegisterAccumulation/Name">
                await new РегістриНакопичення.<xsl:value-of select="text()"/>_RecordsSet().Delete(this.UnigueID.UGuid);
              </xsl:for-each>
            }
            </xsl:when>
            <xsl:otherwise>await ValueTask.FromResult(true);</xsl:otherwise>
          </xsl:choose>
        }
  </xsl:template>

  <xsl:template match="/">
/*
 *
 * Конфігурації "<xsl:value-of select="Configuration/Name"/>"
 * Автор <xsl:value-of select="Configuration/Author"/>
 * Дата конфігурації: <xsl:value-of select="Configuration/DateTimeSave"/>
 *
 *
 * Цей код згенерований в Конфігураторі 3. Шаблон GeneratedCode.xslt
 *
 */

using AccountingSoftware;
using System.Xml;

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>
{
    public static class Config
    {
        #region Const

        //Простір імен згенерованого коду
        public const string NameSpageCodeGeneration = "<xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>";

        //Простір імен програми
        public const string NameSpageProgram = "<xsl:value-of select="Configuration/NameSpace"/>";

        #endregion
        
        public static Kernel Kernel { get; set; } = new Kernel();
        public static async void StartBackgroundTask()
        {
            /*
            Схема роботи:

            1. В процесі запису в регістр залишків - додається запис у таблицю тригерів.
              Запис в таблицю тригерів містить дату запису в регістр, назву регістру.

            2. Раз на 5 сек викликається процедура SpetialTableRegAccumTrigerExecute і
              відбувається розрахунок віртуальних таблиць регістрів залишків.

              Розраховуються тільки змінені регістри на дату проведення документу і
              додатково на дату якщо змінена дата документу і документ уже був проведений.

              Додатково розраховуються підсумки в кінці всіх розрахунків.
            */

            if (Kernel.Session == Guid.Empty)
                throw new Exception("Порожня сесія користувача. Спочатку потрібно залогінитись, а тоді вже викликати функцію StartBackgroundTask()");

            while (true)
            {
                //Виконання обчислень
                await Kernel.DataBase.SpetialTableRegAccumTrigerExecute
                (
                    Kernel.Session,
                    РегістриНакопичення.VirtualTablesСalculation.Execute, 
                    РегістриНакопичення.VirtualTablesСalculation.ExecuteFinalCalculation
                );

                //Затримка на 5 сек
                await Task.Delay(5000);
            }
        }
    }

    public class Functions
    {
        /*
          Функція для типу який задається користувачем.
          Повертає презентацію для uuidAndText
        */
        public static async ValueTask&lt;CompositePointerPresentation_Record&gt; CompositePointerPresentation(UuidAndText uuidAndText)
        {
            CompositePointerPresentation_Record record = new();

            (bool result, string pointerGroup, string pointerType) = Configuration.PointerParse(uuidAndText.Text, out Exception? _);
            if (result)
            {
                record.pointer = pointerGroup;
                record.type = pointerType;

                if (!uuidAndText.IsEmpty())
                    if (record.pointer == "Довідники") 
                    {
                        <xsl:text>record.result = record.type switch</xsl:text>
                        {
                        <xsl:for-each select="Configuration/Directories/Directory">
                            <xsl:variable name="DirectoryName" select="Name"/>
                            <xsl:text>"</xsl:text><xsl:value-of select="$DirectoryName"/>" =&gt; await new Довідники.<xsl:value-of select="$DirectoryName"/>_Pointer(uuidAndText.Uuid).GetPresentation(),
                        </xsl:for-each>
                        <xsl:text>_ =&gt; ""</xsl:text>
                        };
                    }
                    else if (record.pointer == "Документи") 
                    {
                        <xsl:text>record.result = record.type switch</xsl:text>
                        {
                        <xsl:for-each select="Configuration/Documents/Document">
                            <xsl:variable name="DocumentName" select="Name"/>
                            <xsl:text>"</xsl:text><xsl:value-of select="$DocumentName"/>" =&gt; await new Документи.<xsl:value-of select="$DocumentName"/>_Pointer(uuidAndText.Uuid).GetPresentation(),
                        </xsl:for-each>
                        <xsl:text>_ =&gt; ""</xsl:text>
                        };
                    }
            }
            return record;
        }
    }
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.Константи
{
    <xsl:for-each select="Configuration/ConstantsBlocks/ConstantsBlock">
	  #region CONSTANTS BLOCK "<xsl:value-of select="Name"/>"
    public static class <xsl:value-of select="Name"/>
    {       
        <xsl:for-each select="Constants/Constant">
        <xsl:text>public static </xsl:text>
        <xsl:call-template name="FieldType" />
        <xsl:text> </xsl:text>
        <xsl:value-of select="Name"/>_Const
        {
            get 
            {
                var recordResult = Task.Run( async () =&gt; { return await Config.Kernel.DataBase.SelectConstants(SpecialTables.Constants, "<xsl:value-of select="NameInTable"/>"); } ).Result;
                <xsl:text>return recordResult.Result ? (</xsl:text>
                <xsl:call-template name="ReadFieldValue2">
                  <xsl:with-param name="BaseFieldContainer">recordResult.Value</xsl:with-param>
                </xsl:call-template>
                <xsl:text>) : </xsl:text>
                <xsl:call-template name="DefaultFieldValue" />;
            }
            set
            {
                Config.Kernel.DataBase.SaveConstants(SpecialTables.Constants, "<xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                <xsl:choose>
                  <xsl:when test="Type = 'enum'">
                    <xsl:text>(int)</xsl:text>
                  </xsl:when>
                </xsl:choose>
                <xsl:text>value</xsl:text>
                <xsl:choose>
                  <xsl:when test="Type = 'pointer'">
                    <xsl:text>.UnigueID.UGuid</xsl:text>
                  </xsl:when>
                </xsl:choose>);
            }
        }
        </xsl:for-each>

        <xsl:for-each select="Constants/Constant">
          <xsl:variable name="ConstantsName" select="Name"/>
          <xsl:for-each select="TabularParts/TablePart">
            <!-- TableParts -->
            <xsl:variable name="TablePartName" select="Name"/>
            <xsl:variable name="TablePartFullName" select="concat($ConstantsName, '_', $TablePartName)"/>
        
        public class <xsl:value-of select="$TablePartFullName"/>_TablePart : ConstantsTablePart
        {
            public <xsl:value-of select="$TablePartFullName"/>_TablePart() : base(Config.Kernel, "<xsl:value-of select="Table"/>",
                 <xsl:text>[</xsl:text>
                 <xsl:for-each select="Fields/Field">
                   <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                 </xsl:for-each>]) { }
            
            public const string TABLE = "<xsl:value-of select="Table"/>";
            <xsl:for-each select="Fields/Field">
            public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
            public List&lt;Record&gt; Records { get; set; } = [];
        
            public async ValueTask Read()
            {
                Records.Clear();
                await base.BaseRead();

                foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
                {
                    Record record = new Record()
                    {
                        UID = (Guid)fieldValue["uid"],
                        <xsl:for-each select="Fields/Field">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:call-template name="ReadFieldValue">
                            <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                          </xsl:call-template>,
                        </xsl:for-each>
                    };
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public async ValueTask Save(bool clear_all_before_save /*= true*/) 
            {
                await base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    await base.BaseDelete();

                <xsl:for-each select="Fields/Field[Type = 'integer' and AutomaticNumbering = '1']">
                int sequenceNumber_<xsl:value-of select="Name"/> = 0;
                </xsl:for-each>

                foreach (Record record in Records)
                {
                    <xsl:for-each select="Fields/Field[Type = 'integer' and AutomaticNumbering = '1']">
                    record.<xsl:value-of select="Name"/> = ++sequenceNumber_<xsl:value-of select="Name"/>;
                    </xsl:for-each>
                    Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                    {
                        <xsl:for-each select="Fields/Field">
                            <xsl:text>{"</xsl:text>
                            <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                            <xsl:if test="Type = 'enum'">
                                <xsl:text>(int)</xsl:text>
                            </xsl:if>
                            <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                            <xsl:choose>
                                <xsl:when test="Type = 'pointer'">
                                    <xsl:text>.UnigueID.UGuid</xsl:text>
                                </xsl:when>
                            </xsl:choose>
                            <xsl:text>}</xsl:text>,
                        </xsl:for-each>
                    };
                    record.UID = await base.BaseSave(record.UID, fieldValue);
                }
                
                await base.BaseCommitTransaction();
            }

            public async ValueTask Remove(Record record)
            {
                await base.BaseRemove(record.UID);
                Records.RemoveAll((Record item) =&gt; record.UID == item.UID);
            }

            public async ValueTask RemoveAll(List&lt;Record&gt; records)
            {
                List&lt;Guid&gt; removeList = [];

                await base.BaseBeginTransaction();
                foreach (Record record in records)
                {
                    removeList.Add(record.UID);
                    await base.BaseRemove(record.UID);
                }
                await base.BaseCommitTransaction();

                Records.RemoveAll((Record item) =&gt; removeList.Exists((Guid uid) =&gt; uid == item.UID));
            }
        
            public async ValueTask Delete()
            {
                await base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:text>public </xsl:text>
                  <xsl:call-template name="FieldType" />
                  <xsl:text> </xsl:text>
                  <xsl:value-of select="Name"/>
                  <xsl:text> { get; set; } = </xsl:text>
                  <xsl:call-template name="DefaultFieldValue" />;
                </xsl:for-each>
            }
        }
          </xsl:for-each>
        </xsl:for-each>     
    }
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.Довідники
{
    <xsl:for-each select="Configuration/Directories/Directory">
      <xsl:variable name="DirectoryName" select="Name"/>
      <xsl:variable name="DirectoryTable" select="Table"/>
    #region DIRECTORY "<xsl:value-of select="$DirectoryName"/>"
    public static class <xsl:value-of select="$DirectoryName"/>_Const
    {
        public const string TABLE = "<xsl:value-of select="Table"/>";
        public const string TYPE = "<xsl:value-of select="$DirectoryName"/>"; /* Назва вказівника */
        public const string POINTER = "Довідники.<xsl:value-of select="$DirectoryName"/>"; /* Повна назва вказівника */
        public const string FULLNAME = "<xsl:value-of select="normalize-space(FullName)"/>"; /* Повна назва об'єкта */
        public const string DELETION_LABEL = "deletion_label"; /* Помітка на видалення true|false */
        public readonly static string[] PRESENTATION_FIELDS = <xsl:text>[</xsl:text>
          <xsl:for-each select="Fields/Field[IsPresentation=1]">
            <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
          </xsl:for-each>];
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }

    public class <xsl:value-of select="$DirectoryName"/>_Objest : DirectoryObject
    {
        public event EventHandler&lt;UnigueID&gt;? UnigueIDChanged;
        public event EventHandler&lt;string&gt;? CaptionChanged;

        public <xsl:value-of select="$DirectoryName"/>_Objest() : base(Config.Kernel, "<xsl:value-of select="Table"/>", <xsl:value-of select="$DirectoryName"/>_Const.TYPE,
             <xsl:text>[</xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>]) 
        {
            <xsl:if test="count(TabularParts/TablePart) &gt; 0">
                //Табличні частини
                <xsl:for-each select="TabularParts/TablePart">
                    <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
                    <xsl:value-of select="$TablePartName"/><xsl:text> = new </xsl:text>
                    <xsl:value-of select="concat($DirectoryName, '_', $TablePartName)"/><xsl:text>(this)</xsl:text>;
                </xsl:for-each>
            </xsl:if>
        }
        
        public async ValueTask New()
        {
            BaseNew();
            UnigueIDChanged?.Invoke(this, base.UnigueID);
            CaptionChanged?.Invoke(this, <xsl:value-of select="$DirectoryName"/>_Const.FULLNAME + " *");
            <xsl:choose>
              <xsl:when test="normalize-space(TriggerFunctions/New) != '' and TriggerFunctions/New[@Action = '1']">
                await <xsl:value-of select="$DirectoryName"/>_Triggers.<xsl:value-of select="TriggerFunctions/New"/>(this);
              </xsl:when>
              <xsl:otherwise>
                await ValueTask.FromResult(true);
              </xsl:otherwise>
            </xsl:choose>
        }

        public async ValueTask&lt;bool&gt; Read(UnigueID uid, bool readAllTablePart = false)
        {
            if (await BaseRead(uid))
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">base.FieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                BaseClear();
                <xsl:if test="count(TabularParts/TablePart) != 0">
                if (readAllTablePart)
                {
                    <xsl:for-each select="TabularParts/TablePart">
                    await <xsl:value-of select="concat(Name, '_TablePart')"/>.Read();</xsl:for-each>
                }
                </xsl:if>
                UnigueIDChanged?.Invoke(this, base.UnigueID);
                CaptionChanged?.Invoke(this, string.Join(", ", [<xsl:for-each select="Fields/Field[IsPresentation=1]"><xsl:value-of select="Name"/>, </xsl:for-each>]));
                return true;
            }
            else
                return false;
        }
        
        public async ValueTask&lt;bool&gt; Save()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeSave) != '' and TriggerFunctions/BeforeSave[@Action = '1']">
                await <xsl:value-of select="$DirectoryName"/>_Triggers.<xsl:value-of select="TriggerFunctions/BeforeSave"/>(this);
            </xsl:if>
            <xsl:for-each select="Fields/Field">
              <xsl:text>base.FieldValue["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] = </xsl:text>
              <xsl:if test="Type = 'enum'">
                  <xsl:text>(int)</xsl:text>      
              </xsl:if>
              <xsl:value-of select="Name"/>
              <xsl:choose>
                <xsl:when test="Type = 'pointer'">
                  <xsl:text>.UnigueID.UGuid</xsl:text>
                </xsl:when>
              </xsl:choose>;
            </xsl:for-each>
            bool result = await BaseSave();
            if (result)
            {
                <xsl:if test="normalize-space(TriggerFunctions/AfterSave) != '' and TriggerFunctions/AfterSave[@Action = '1']">
                await <xsl:value-of select="$DirectoryName"/>_Triggers.<xsl:value-of select="TriggerFunctions/AfterSave"/>(this);     
                </xsl:if>
                <xsl:if test="count(Fields/Field[IsFullTextSearch = '1' and Type = 'string']) &gt; 0">
                await BaseWriteFullTextSearch(GetBasis(), [<xsl:for-each select="Fields/Field[IsFullTextSearch = '1' and Type = 'string']"><xsl:value-of select="Name"/>, </xsl:for-each>]);
                </xsl:if>
            }
            CaptionChanged?.Invoke(this, string.Join(", ", [<xsl:for-each select="Fields/Field[IsPresentation=1]"><xsl:value-of select="Name"/>, </xsl:for-each>]));
            return result;
        }

        public async ValueTask&lt;<xsl:value-of select="$DirectoryName"/>_Objest&gt; Copy(bool copyTableParts = false)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest copy = new <xsl:value-of select="$DirectoryName"/>_Objest()
            {
                <xsl:for-each select="Fields/Field">
                    <xsl:value-of select="Name"/><xsl:text> = </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
            <xsl:if test="count(TabularParts/TablePart) != 0">
            if (copyTableParts)
            {
            <xsl:for-each select="TabularParts/TablePart">
                <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
                //<xsl:value-of select="Name"/> - Таблична частина
                await <xsl:value-of select="$TablePartName"/>.Read();
                copy.<xsl:value-of select="$TablePartName"/>.Records = <xsl:value-of select="$TablePartName"/>.Copy();
            </xsl:for-each>
            }
            </xsl:if>

            await copy.New();
            <xsl:if test="normalize-space(TriggerFunctions/Copying) != '' and TriggerFunctions/Copying[@Action = '1']">
            await <xsl:value-of select="$DirectoryName"/>_Triggers.<xsl:value-of select="TriggerFunctions/Copying"/>(copy, this);      
            </xsl:if>
            return copy;
        }

        public async ValueTask SetDeletionLabel(bool label = true)
        {
            <xsl:if test="normalize-space(TriggerFunctions/SetDeletionLabel) != '' and TriggerFunctions/SetDeletionLabel[@Action = '1']">
                await <xsl:value-of select="$DirectoryName"/>_Triggers.<xsl:value-of select="TriggerFunctions/SetDeletionLabel"/>(this, label);      
            </xsl:if>
            await base.BaseDeletionLabel(label);
        }

        public async ValueTask Delete()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeDelete) != '' and TriggerFunctions/BeforeDelete[@Action = '1']">
                await <xsl:value-of select="$DirectoryName"/>_Triggers.<xsl:value-of select="TriggerFunctions/BeforeDelete"/>(this);      
            </xsl:if>
            await base.BaseDelete([<xsl:for-each select="TabularParts/TablePart">"<xsl:value-of select="Table"/>", </xsl:for-each>]);
        }
        
        public <xsl:value-of select="$DirectoryName"/>_Pointer GetDirectoryPointer()
        {
            return new <xsl:value-of select="$DirectoryName"/>_Pointer(UnigueID.UGuid);
        }

        public async ValueTask&lt;string&gt; GetPresentation()
        {
            return await base.BasePresentation(<xsl:value-of select="$DirectoryName"/>_Const.PRESENTATION_FIELDS);
        }
                
        <xsl:for-each select="Fields/Field">
          <xsl:text>public </xsl:text>
          <xsl:call-template name="FieldType" />
          <xsl:text> </xsl:text>
          <xsl:value-of select="Name"/>
          <xsl:text> { get; set; } = </xsl:text>
          <xsl:call-template name="DefaultFieldValue" />;
        </xsl:for-each>
        <xsl:if test="count(TabularParts/TablePart) &gt; 0">
        //Табличні частини
        </xsl:if>
        <xsl:for-each select="TabularParts/TablePart">
            <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
            <xsl:text>public </xsl:text><xsl:value-of select="concat($DirectoryName, '_', $TablePartName)"/><xsl:text> </xsl:text>
            <xsl:value-of select="$TablePartName"/><xsl:text> { get; private set; </xsl:text>}
        </xsl:for-each>
    }

    public class <xsl:value-of select="$DirectoryName"/>_Pointer : DirectoryPointer
    {
        public <xsl:value-of select="$DirectoryName"/>_Pointer(object? uid = null) : base(Config.Kernel, "<xsl:value-of select="Table"/>", <xsl:value-of select="$DirectoryName"/>_Const.TYPE)
        {
            base.Init(new UnigueID(uid));
        }
        
        public <xsl:value-of select="$DirectoryName"/>_Pointer(UnigueID uid, Dictionary&lt;string, object&gt;? fields = null) : base(Config.Kernel, "<xsl:value-of select="Table"/>", <xsl:value-of select="$DirectoryName"/>_Const.TYPE)
        {
            base.Init(uid, fields);
        }
        
        public async ValueTask&lt;<xsl:value-of select="$DirectoryName"/>_Objest?&gt; GetDirectoryObject(bool readAllTablePart = false)
        {
            if (this.IsEmpty()) return null;
            <xsl:value-of select="$DirectoryName"/>_Objest obj = new <xsl:value-of select="$DirectoryName"/>_Objest();
            return await obj.Read(base.UnigueID, readAllTablePart) ? obj : null;
        }

        public <xsl:value-of select="$DirectoryName"/>_Pointer Copy()
        {
            return new <xsl:value-of select="$DirectoryName"/>_Pointer(base.UnigueID, base.Fields) { Name = Name };
        }

        public string Назва
        {
            get { return Name; } set { Name = value; }
        }

        public async ValueTask&lt;string&gt; GetPresentation()
        {
            return Name = await base.BasePresentation(<xsl:value-of select="$DirectoryName"/>_Const.PRESENTATION_FIELDS);
        }

        public static void GetJoin(Query querySelect, string joinField, string parentTable, string joinTableAlias, string fieldAlias)
        {
            string[] presentationField = new string [<xsl:value-of select="$DirectoryName"/>_Const.PRESENTATION_FIELDS.Length];
            for (int i = 0; i &lt; presentationField.Length; i++) presentationField[i] = $"{joinTableAlias}.{<xsl:value-of select="$DirectoryName"/>_Const.PRESENTATION_FIELDS[i]}";
            querySelect.Joins.Add(new Join(<xsl:value-of select="$DirectoryName"/>_Const.TABLE, joinField, parentTable, joinTableAlias));
            querySelect.FieldAndAlias.Add(new NameValue&lt;string&gt;(presentationField.Length switch { 1 =&gt; presentationField[0], &gt;1 =&gt; $"concat_ws (', ', " + string.Join(", ", presentationField) + ")", _ =&gt; "'#'" }, fieldAlias));
        }

        public async ValueTask&lt;bool?&gt; GetDeletionLabel()
        {
            return await base.BaseGetDeletionLabel();
        }

        public async ValueTask SetDeletionLabel(bool label = true)
        {
            <xsl:if test="normalize-space(TriggerFunctions/SetDeletionLabel) != '' and TriggerFunctions/SetDeletionLabel[@Action = '1']">
              <xsl:value-of select="$DirectoryName"/>_Objest? obj = await GetDirectoryObject();
              if (obj != null) await <xsl:value-of select="$DirectoryName"/>_Triggers.<xsl:value-of select="TriggerFunctions/SetDeletionLabel"/>(obj, label);
            </xsl:if>
            await base.BaseDeletionLabel(label);
        }
		
        public <xsl:value-of select="$DirectoryName"/>_Pointer GetEmptyPointer()
        {
            return new <xsl:value-of select="$DirectoryName"/>_Pointer();
        }
    }
    
    public class <xsl:value-of select="$DirectoryName"/>_Select : DirectorySelect
    {
        public <xsl:value-of select="$DirectoryName"/>_Select() : base(Config.Kernel, "<xsl:value-of select="Table"/>") { }        
        public async ValueTask&lt;bool&gt; Select() { return await base.BaseSelect(); }
        public async ValueTask&lt;bool&gt; SelectSingle() { if (await base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        public bool MoveNext() { if (base.MoveToPosition() &amp;&amp; base.CurrentPointerPosition.HasValue) { Current = new <xsl:value-of select="$DirectoryName"/>_Pointer(base.CurrentPointerPosition.Value.UnigueID, base.CurrentPointerPosition.Value.Fields); return true; } else { Current = null; return false; } }
        public <xsl:value-of select="$DirectoryName"/>_Pointer? Current { get; private set; }
        
        public async ValueTask&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; FindByField(string name, object value)
        {
            UnigueID? pointer = await base.BaseFindByField(name, value);
            return pointer != null ? new <xsl:value-of select="$DirectoryName"/>_Pointer(pointer) : new <xsl:value-of select="$DirectoryName"/>_Pointer();
        }
        
        public async ValueTask&lt;List&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt;&gt; FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; directoryPointerList = [];
            foreach (var directoryPointer in await base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new <xsl:value-of select="$DirectoryName"/>_Pointer(directoryPointer.UnigueID, directoryPointer.Fields));
            return directoryPointerList;
        }
    }

    <xsl:variable name="ParentField" select="ParentField"/>
    <xsl:if test="Type = 'Hierarchical' and count(Fields/Field[Name = $ParentField]) != 0">
    public class <xsl:value-of select="$DirectoryName"/>_SelectHierarchical : DirectorySelectHierarchical
    {
        public <xsl:value-of select="$DirectoryName"/>_SelectHierarchical() : base(Config.Kernel, "<xsl:value-of select="Table"/>", "<xsl:value-of select="Fields/Field[Name = $ParentField]/NameInTable"/>") { }        
        public async ValueTask&lt;bool&gt; Select() { return await base.BaseSelect(); }
        public async ValueTask&lt;bool&gt; SelectSingle() { if (await base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = Parent = null; Level = 0; return false; } }
        public bool MoveNext() { if (base.MoveToPosition() &amp;&amp; base.CurrentPointerPositionHierarchical.HasValue) { 
          Current = new <xsl:value-of select="$DirectoryName"/>_Pointer(base.CurrentPointerPositionHierarchical.Value.UnigueID, base.CurrentPointerPositionHierarchical.Value.Fields); 
          Parent = new <xsl:value-of select="$DirectoryName"/>_Pointer(base.CurrentPointerPositionHierarchical.Value.Parent); 
          Level = base.CurrentPointerPositionHierarchical.Value.Level; return true; } else { Current = Parent = null; Level = 0; return false; } }
        public <xsl:value-of select="$DirectoryName"/>_Pointer? Current { get; private set; }
        public <xsl:value-of select="$DirectoryName"/>_Pointer? Parent { get; private set; }
        public int Level { get; private set; } = 0;
    }
    </xsl:if>

      <xsl:for-each select="TabularParts/TablePart"> <!-- TableParts -->
        <xsl:variable name="TablePartName" select="Name"/>
        <xsl:variable name="TablePartFullName" select="concat($DirectoryName, '_', $TablePartName)"/>
    
    public class <xsl:value-of select="$TablePartFullName"/>_TablePart : DirectoryTablePart
    {
        public <xsl:value-of select="$TablePartFullName"/>_TablePart(<xsl:value-of select="$DirectoryName"/>_Objest owner) : base(Config.Kernel, "<xsl:value-of select="Table"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>])
        {
            if (owner == null) throw new Exception("owner null");
            Owner = owner;
        }

        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>

        public <xsl:value-of select="$DirectoryName"/>_Objest Owner { get; private set; }
        
        public List&lt;Record&gt; Records { get; set; } = [];
        
        public void FillJoin(string[]? orderFields = null)
        {
            QuerySelect.Clear();

            if (orderFields!=null)
              foreach(string field in orderFields)
                QuerySelect.Order.Add(field, SelectOrder.ASC);

            <xsl:for-each select="Fields/Field">
                <xsl:if test="Type = 'pointer'">
                  <xsl:value-of select="substring-before(Pointer, '.')"/>.<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer.GetJoin(QuerySelect, <xsl:value-of select="Name"/>, "<xsl:value-of select="../../Table"/>", "join_tab_<xsl:value-of select="position()"/>", "<xsl:value-of select="Name"/>");
                </xsl:if>
            </xsl:for-each>
        }

        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead(Owner.UnigueID);

            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    <xsl:for-each select="Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
                <xsl:if test="count(Fields/Field[Type = 'pointer']) != 0">
                if (JoinValue.TryGetValue(record.UID.ToString(), out var ItemValue))
                {
                  record.JoinItemValue = ItemValue;
                  <xsl:for-each select="Fields/Field">
                      <xsl:if test="Type = 'pointer'">
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.Name = ItemValue["<xsl:value-of select="Name"/>"];
                      </xsl:if>
                  </xsl:for-each>
                }
                </xsl:if>
            }
            
            base.BaseClear();
        }
        
        public async ValueTask Save(bool clear_all_before_save) 
        {
            if (!await base.IsExistOwner(Owner.UnigueID, "<xsl:value-of select="$DirectoryTable"/>"))
                throw new Exception("Owner not exist");
                
            await base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                await base.BaseDelete(Owner.UnigueID);

            <xsl:for-each select="Fields/Field[Type = 'integer' and AutomaticNumbering = '1']">
            int sequenceNumber_<xsl:value-of select="Name"/> = 0;
            </xsl:for-each>
            
            foreach (Record record in Records)
            {
                <xsl:for-each select="Fields/Field[Type = 'integer' and AutomaticNumbering = '1']">
                record.<xsl:value-of select="Name"/> = ++sequenceNumber_<xsl:value-of select="Name"/>;
                </xsl:for-each>
                Dictionary&lt;string, object&gt; fieldValue = new()
                {
                    <xsl:for-each select="Fields/Field">
                        <xsl:text>{"</xsl:text>
                        <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                        <xsl:if test="Type = 'enum'">
                          <xsl:text>(int)</xsl:text>
                        </xsl:if>
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                        <xsl:choose>
                            <xsl:when test="Type = 'pointer'">
                                <xsl:text>.UnigueID.UGuid</xsl:text>
                            </xsl:when>
                        </xsl:choose>
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            await base.BaseCommitTransaction();
        }

        public async ValueTask Remove(Record record)
        {
            await base.BaseRemove(record.UID, Owner.UnigueID);
            Records.RemoveAll((Record item) =&gt; record.UID == item.UID);
        }

        public async ValueTask RemoveAll(List&lt;Record&gt; records)
        {
            List&lt;Guid&gt; removeList = [];

            await base.BaseBeginTransaction();
            foreach (Record record in records)
            {
                removeList.Add(record.UID);
                await base.BaseRemove(record.UID, Owner.UnigueID);
            }
            await base.BaseCommitTransaction();

            Records.RemoveAll((Record item) =&gt; removeList.Exists((Guid uid) =&gt; uid == item.UID));
        }
        
        public async ValueTask Delete()
        {
            await base.BaseDelete(Owner.UnigueID);
        }

        public List&lt;Record&gt; Copy()
        {
            List&lt;Record&gt; copyRecords = new(Records);
            foreach (Record copyRecordItem in Records)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }
        
        public class Record : DirectoryTablePartRecord
        {
            <xsl:for-each select="Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
              <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
    }
      </xsl:for-each> <!-- TableParts -->
   
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.Перелічення
{
    <xsl:for-each select="Configuration/Enums/Enum">
    #region ENUM "<xsl:value-of select="Name"/>"
    public enum <xsl:value-of select="Name"/>
    {
         <xsl:variable name="CountEnumField" select="count(Fields/Field)" />
         <xsl:for-each select="Fields/Field">
             <xsl:value-of select="Name"/>
             <xsl:text> = </xsl:text>
             <xsl:value-of select="Value"/>
             <xsl:if test="position() &lt; $CountEnumField">
         <xsl:text>,
         </xsl:text>
             </xsl:if>
         </xsl:for-each>
    }
    #endregion
    </xsl:for-each>

    public static class ПсевдонімиПерелічення
    {
    <xsl:for-each select="Configuration/Enums/Enum">
        <xsl:variable name="EnumName" select="Name" />
        <xsl:variable name="CountEnumField" select="count(Fields/Field)" />
        #region ENUM "<xsl:value-of select="$EnumName"/>"
        public static string <xsl:value-of select="$EnumName"/>_Alias(<xsl:value-of select="$EnumName"/> value)
        {
            return value switch
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:variable name="ReturnValue">
                      <xsl:choose>
                          <xsl:when test="normalize-space(Desc) != ''">
                              <xsl:value-of select="normalize-space(Desc)"/>
                          </xsl:when>
                          <xsl:otherwise>
                              <xsl:value-of select="normalize-space(Name)"/>
                          </xsl:otherwise>
                      </xsl:choose>
                  </xsl:variable>
                  <xsl:value-of select="$EnumName"/>.<xsl:value-of select="Name"/> =&gt; "<xsl:value-of select="$ReturnValue"/>",
                </xsl:for-each>
                <xsl:text>_ =&gt; ""</xsl:text>
            };
        }

        public static <xsl:value-of select="$EnumName"/>? <xsl:value-of select="$EnumName"/>_FindByName(string name)
        {
            return name switch
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:variable name="ReturnValue">
                      <xsl:choose>
                          <xsl:when test="normalize-space(Desc) != ''">
                              <xsl:value-of select="normalize-space(Desc)"/>
                          </xsl:when>
                          <xsl:otherwise>
                              <xsl:value-of select="normalize-space(Name)"/>
                          </xsl:otherwise>
                      </xsl:choose>
                  </xsl:variable>
                  <xsl:text>"</xsl:text><xsl:value-of select="$ReturnValue"/>" =&gt; <xsl:value-of select="$EnumName"/>.<xsl:value-of select="Name"/>,
                </xsl:for-each>
                <xsl:text>_ =&gt; null</xsl:text>
            };
        }

        public static List&lt;NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;&gt; <xsl:value-of select="$EnumName"/>_List()
        {
            return new List&lt;NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;&gt;() {
            <xsl:for-each select="Fields/Field">
              <xsl:variable name="ReturnValue">
                  <xsl:choose>
                      <xsl:when test="normalize-space(Desc) != ''">
                          <xsl:value-of select="normalize-space(Desc)"/>
                      </xsl:when>
                      <xsl:otherwise>
                          <xsl:value-of select="normalize-space(Name)"/>
                      </xsl:otherwise>
                  </xsl:choose>
              </xsl:variable>
              <xsl:text>new NameValue&lt;</xsl:text><xsl:value-of select="$EnumName"/>&gt;("<xsl:value-of select="$ReturnValue"/>", <xsl:value-of select="$EnumName"/>.<xsl:value-of select="Name"/>),
            </xsl:for-each>};
        }
        #endregion
    </xsl:for-each>
    }
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.Документи
{
    <xsl:for-each select="Configuration/Documents/Document">
      <xsl:variable name="DocumentName" select="Name"/>
      <xsl:variable name="DocumentTable" select="Table"/>
    #region DOCUMENT "<xsl:value-of select="$DocumentName"/>"
    public static class <xsl:value-of select="$DocumentName"/>_Const
    {
        public const string TABLE = "<xsl:value-of select="Table"/>";
        public const string TYPE = "<xsl:value-of select="$DocumentName"/>"; /* Назва вказівника */
        public const string POINTER = "Документи.<xsl:value-of select="$DocumentName"/>"; /* Повна назва вказівника */
        public const string FULLNAME = "<xsl:value-of select="normalize-space(FullName)"/>"; /* Повна назва об'єкта */
        public const string DELETION_LABEL = "deletion_label"; /* Помітка на видалення true|false */
        public const string SPEND = "spend"; /* Проведений true|false */
        public const string SPEND_DATE = "spend_date"; /* Дата проведення DateTime */
        public readonly static string[] PRESENTATION_FIELDS = <xsl:text>[</xsl:text>
          <xsl:for-each select="Fields/Field[IsPresentation=1]">
            <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
          </xsl:for-each>];
        
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }

    public class <xsl:value-of select="$DocumentName"/>_Objest : DocumentObject
    {
        public event EventHandler&lt;UnigueID&gt;? UnigueIDChanged;
        public event EventHandler&lt;string&gt;? CaptionChanged;

        public <xsl:value-of select="$DocumentName"/>_Objest() : base(Config.Kernel, "<xsl:value-of select="Table"/>", <xsl:value-of select="$DocumentName"/>_Const.TYPE,
             <xsl:text>[</xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>])
        {
            <xsl:if test="count(TabularParts/TablePart) &gt; 0">
                //Табличні частини
                <xsl:for-each select="TabularParts/TablePart">
                    <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
                    <xsl:value-of select="$TablePartName"/><xsl:text> = new </xsl:text>
                    <xsl:value-of select="concat($DocumentName, '_', $TablePartName)"/><xsl:text>(this)</xsl:text>;
                </xsl:for-each>
            </xsl:if>
        }
        
        public async ValueTask New()
        {
            BaseNew();
            UnigueIDChanged?.Invoke(this, base.UnigueID);
            CaptionChanged?.Invoke(this, <xsl:value-of select="$DocumentName"/>_Const.FULLNAME + " *");
            <xsl:choose>
              <xsl:when test="normalize-space(TriggerFunctions/New) != '' and TriggerFunctions/New[@Action = '1']">
                await <xsl:value-of select="$DocumentName"/>_Triggers.<xsl:value-of select="TriggerFunctions/New"/>(this);
              </xsl:when>
              <xsl:otherwise>
                await ValueTask.FromResult(true);
              </xsl:otherwise>
            </xsl:choose>
        }

        public async ValueTask&lt;bool&gt; Read(UnigueID uid, bool readAllTablePart = false)
        {
            if (await BaseRead(uid))
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">base.FieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                BaseClear();
                <xsl:if test="count(TabularParts/TablePart) != 0">
                if (readAllTablePart)
                {
                    <xsl:for-each select="TabularParts/TablePart">
                    await <xsl:value-of select="concat(Name, '_TablePart')"/>.Read();</xsl:for-each>
                }
                </xsl:if>
                UnigueIDChanged?.Invoke(this, base.UnigueID);
                CaptionChanged?.Invoke(this, string.Join(", ", [<xsl:for-each select="Fields/Field[IsPresentation=1]"><xsl:value-of select="Name"/>, </xsl:for-each>]));
                return true;
            }
            else
                return false;
        }
        
        public async ValueTask&lt;bool&gt; Save()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeSave) != '' and TriggerFunctions/BeforeSave[@Action = '1']">
                await <xsl:value-of select="$DocumentName"/>_Triggers.<xsl:value-of select="TriggerFunctions/BeforeSave"/>(this);
            </xsl:if>
            <xsl:for-each select="Fields/Field">
              <xsl:text>base.FieldValue["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] = </xsl:text>
              <xsl:if test="Type = 'enum'">
                  <xsl:text>(int)</xsl:text>      
              </xsl:if>
              <xsl:value-of select="Name"/>
              <xsl:choose>
                <xsl:when test="Type = 'pointer'">
                  <xsl:text>.UnigueID.UGuid</xsl:text>
                </xsl:when>
              </xsl:choose>;
            </xsl:for-each>
            bool result = await BaseSave();
            if (result)
            {
                <xsl:if test="normalize-space(TriggerFunctions/AfterSave) != '' and TriggerFunctions/AfterSave[@Action = '1']">
                await <xsl:value-of select="$DocumentName"/>_Triggers.<xsl:value-of select="TriggerFunctions/AfterSave"/>(this);      
                </xsl:if>
                <xsl:if test="count(Fields/Field[IsFullTextSearch = '1' and Type = 'string']) &gt; 0">
                await BaseWriteFullTextSearch(GetBasis(), [<xsl:for-each select="Fields/Field[IsFullTextSearch = '1' and Type = 'string']"><xsl:value-of select="Name"/>, </xsl:for-each>]);
                </xsl:if>
            }
            CaptionChanged?.Invoke(this, string.Join(", ", [<xsl:for-each select="Fields/Field[IsPresentation=1]"><xsl:value-of select="Name"/>, </xsl:for-each>]));
            return result;
        }

        public async ValueTask&lt;bool&gt; SpendTheDocument(DateTime spendDate)
        {
            <xsl:choose>
                <xsl:when test="normalize-space(SpendFunctions/Spend) != '' and SpendFunctions/Spend[@Action = '1']">
            await BaseAddIgnoreDocumentList();
            bool spend = await <xsl:value-of select="$DocumentName"/>_SpendTheDocument.<xsl:value-of select="SpendFunctions/Spend"/>(this);
            if (!spend) ClearRegAccum();
            await BaseSpend(spend, spend ? spendDate : DateTime.MinValue);
            await BaseRemoveIgnoreDocumentList();
            return spend;
                </xsl:when>
                <xsl:otherwise>
            await BaseSpend(false, DateTime.MinValue);
            return false;
                </xsl:otherwise>
            </xsl:choose>
        }

        <!-- Очищення регістрів накопичення функція -->
        <xsl:call-template name="ClearRegAccumFunction" />

        public async ValueTask ClearSpendTheDocument()
        {
            ClearRegAccum();
            <xsl:if test="normalize-space(SpendFunctions/ClearSpend) != '' and SpendFunctions/ClearSpend[@Action = '1']">
            await <xsl:value-of select="$DocumentName"/>_SpendTheDocument.<xsl:value-of select="SpendFunctions/ClearSpend"/>(this);
            </xsl:if>
            await BaseSpend(false, DateTime.MinValue);
        }

        public async ValueTask&lt;<xsl:value-of select="$DocumentName"/>_Objest&gt; Copy(bool copyTableParts = false)
        {
            <xsl:value-of select="$DocumentName"/>_Objest copy = new <xsl:value-of select="$DocumentName"/>_Objest()
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:value-of select="Name"/><xsl:text> = </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
            <xsl:if test="count(TabularParts/TablePart) != 0">
            if (copyTableParts)
            {
            <xsl:for-each select="TabularParts/TablePart">
                <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
                //<xsl:value-of select="Name"/> - Таблична частина
                await <xsl:value-of select="$TablePartName"/>.Read();
                copy.<xsl:value-of select="$TablePartName"/>.Records = <xsl:value-of select="$TablePartName"/>.Copy();
            </xsl:for-each>
            }
            </xsl:if>

            await copy.New();
            <xsl:if test="normalize-space(TriggerFunctions/Copying) != '' and TriggerFunctions/Copying[@Action = '1']">
                await <xsl:value-of select="$DocumentName"/>_Triggers.<xsl:value-of select="TriggerFunctions/Copying"/>(copy, this);      
            </xsl:if>
            return copy;
        }

        public async ValueTask SetDeletionLabel(bool label = true)
        {
            <xsl:if test="normalize-space(TriggerFunctions/SetDeletionLabel) != '' and TriggerFunctions/SetDeletionLabel[@Action = '1']">
                await <xsl:value-of select="$DocumentName"/>_Triggers.<xsl:value-of select="TriggerFunctions/SetDeletionLabel"/>(this, label);      
            </xsl:if>
            await ClearSpendTheDocument();
            await base.BaseDeletionLabel(label);
        }

        public async ValueTask Delete()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeDelete) != '' and TriggerFunctions/BeforeDelete[@Action = '1']">
                await <xsl:value-of select="$DocumentName"/>_Triggers.<xsl:value-of select="TriggerFunctions/BeforeDelete"/>(this);      
            </xsl:if>
            await ClearSpendTheDocument();
            await base.BaseDelete([<xsl:for-each select="TabularParts/TablePart">"<xsl:value-of select="Table"/>", </xsl:for-each>]);
        }
        
        public <xsl:value-of select="$DocumentName"/>_Pointer GetDocumentPointer()
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer(UnigueID.UGuid);
        }

        public async ValueTask&lt;string&gt; GetPresentation()
        {
            return await base.BasePresentation(<xsl:value-of select="$DocumentName"/>_Const.PRESENTATION_FIELDS);
        }
        
        <xsl:for-each select="Fields/Field">
          <xsl:text>public </xsl:text>
          <xsl:call-template name="FieldType" />
          <xsl:text> </xsl:text>
          <xsl:value-of select="Name"/>
          <xsl:text> { get; set; } = </xsl:text>
          <xsl:call-template name="DefaultFieldValue" />;
        </xsl:for-each>
        <xsl:if test="count(TabularParts/TablePart) &gt; 0">
        //Табличні частини
        </xsl:if>
        <xsl:for-each select="TabularParts/TablePart">
            <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
            <xsl:text>public </xsl:text><xsl:value-of select="concat($DocumentName, '_', $TablePartName)"/><xsl:text> </xsl:text>
            <xsl:value-of select="$TablePartName"/><xsl:text> { get; set; </xsl:text>}
        </xsl:for-each>
    }
    
    public class <xsl:value-of select="$DocumentName"/>_Pointer : DocumentPointer
    {
        public <xsl:value-of select="$DocumentName"/>_Pointer(object? uid = null) : base(Config.Kernel, "<xsl:value-of select="Table"/>", <xsl:value-of select="$DocumentName"/>_Const.TYPE)
        {
            base.Init(new UnigueID(uid));
        }
        
        public <xsl:value-of select="$DocumentName"/>_Pointer(UnigueID uid, Dictionary&lt;string, object&gt;? fields = null) : base(Config.Kernel, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$DocumentName"/>")
        {
            base.Init(uid, fields);
        }

        public string Назва
        {
            get { return Name; } set { Name = value; }
        }

        public async ValueTask&lt;string&gt; GetPresentation()
        {
            return Name = await base.BasePresentation(<xsl:value-of select="$DocumentName"/>_Const.PRESENTATION_FIELDS);
        }

        public static void GetJoin(Query querySelect, string joinField, string parentTable, string joinTableAlias, string fieldAlias)
        {
            string[] presentationField = new string [<xsl:value-of select="$DocumentName"/>_Const.PRESENTATION_FIELDS.Length];
            for (int i = 0; i &lt; presentationField.Length; i++) presentationField[i] = $"{joinTableAlias}.{<xsl:value-of select="$DocumentName"/>_Const.PRESENTATION_FIELDS[i]}";
            querySelect.Joins.Add(new Join(<xsl:value-of select="$DocumentName"/>_Const.TABLE, joinField, parentTable, joinTableAlias));
            querySelect.FieldAndAlias.Add(new NameValue&lt;string&gt;(presentationField.Length switch { 1 =&gt; presentationField[0], &gt;1 =&gt; $"concat_ws (', ', " + string.Join(", ", presentationField) + ")", _ =&gt; "'#'" }, fieldAlias));
        }

        public async ValueTask&lt;bool?&gt; IsSpend()
        {
            return await base.BaseIsSpend();
        }

        public async ValueTask&lt;(bool? Spend, DateTime SpendDate)&gt; GetSpend()
        {
            return await base.BaseGetSpend();
        }

        public async ValueTask&lt;bool&gt; SpendTheDocument(DateTime spendDate)
        {
            <xsl:value-of select="$DocumentName"/>_Objest? obj = await GetDocumentObject();
            return obj != null &amp;&amp; await obj.SpendTheDocument(spendDate);
        }

        public async ValueTask ClearSpendTheDocument()
        {
            <xsl:choose>
                <xsl:when test="normalize-space(SpendFunctions/ClearSpend) != '' and SpendFunctions/ClearSpend[@Action = '1']">
            <xsl:value-of select="$DocumentName"/>_Objest? obj = await GetDocumentObject();
            if (obj != null) await obj.ClearSpendTheDocument();
                </xsl:when>
                <xsl:otherwise>
            ClearRegAccum();
            await BaseSpend(false, DateTime.MinValue);
                </xsl:otherwise>
            </xsl:choose>
        }

        public async ValueTask&lt;bool?&gt; GetDeletionLabel()
        {
            return await base.BaseGetDeletionLabel();
        }

        <!-- Очищення регістрів накопичення функція -->
        <xsl:call-template name="ClearRegAccumFunction" />

        public async ValueTask SetDeletionLabel(bool label = true)
        {
          <xsl:choose>
            <xsl:when test="(normalize-space(TriggerFunctions/SetDeletionLabel) != '' and TriggerFunctions/SetDeletionLabel[@Action = '1']) or 
                            (normalize-space(SpendFunctions/ClearSpend) != '' and SpendFunctions/ClearSpend[@Action = '1'])">
            <xsl:value-of select="$DocumentName"/>_Objest? obj = await GetDocumentObject();
            if (obj == null) return;
            <xsl:if test="normalize-space(TriggerFunctions/SetDeletionLabel) != '' and TriggerFunctions/SetDeletionLabel[@Action = '1']">
                await <xsl:value-of select="$DocumentName"/>_Triggers.<xsl:value-of select="TriggerFunctions/SetDeletionLabel"/>(obj, label);
            </xsl:if>
            if (label) await obj.ClearSpendTheDocument();
            </xsl:when>
            <xsl:otherwise>
            if (label)
            {
                ClearRegAccum();
                await BaseSpend(false, DateTime.MinValue);
            }
            </xsl:otherwise>
          </xsl:choose>
          await base.BaseDeletionLabel(label);
        }

        public <xsl:value-of select="$DocumentName"/>_Pointer Copy()
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer(base.UnigueID, base.Fields) { Name = Name };
        }

        public <xsl:value-of select="$DocumentName"/>_Pointer GetEmptyPointer()
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer();
        }

        public async ValueTask&lt;<xsl:value-of select="$DocumentName"/>_Objest?&gt; GetDocumentObject(bool readAllTablePart = false)
        {
            if (this.IsEmpty()) return null;
            <xsl:value-of select="$DocumentName"/>_Objest obj = new <xsl:value-of select="$DocumentName"/>_Objest();
            return await obj.Read(base.UnigueID, readAllTablePart) ? obj : null;
        }
    }

    public class <xsl:value-of select="$DocumentName"/>_Select : DocumentSelect
    {		
        public <xsl:value-of select="$DocumentName"/>_Select() : base(Config.Kernel, "<xsl:value-of select="Table"/>") { }
        public async ValueTask&lt;bool&gt; Select() { return await base.BaseSelect(); }
        public async ValueTask&lt;bool&gt; SelectSingle() { if (await base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        public bool MoveNext() { if (base.MoveToPosition() &amp;&amp; base.CurrentPointerPosition.HasValue) { Current = new <xsl:value-of select="$DocumentName"/>_Pointer(base.CurrentPointerPosition.Value.UnigueID, base.CurrentPointerPosition.Value.Fields); return true; } else { Current = null; return false; } }
        public <xsl:value-of select="$DocumentName"/>_Pointer? Current { get; private set; }

        public async ValueTask&lt;<xsl:value-of select="$DocumentName"/>_Pointer&gt; FindByField(string name, object value)
        {
            UnigueID? pointer = await base.BaseFindByField(name, value);
            return pointer != null ? new <xsl:value-of select="$DocumentName"/>_Pointer(pointer) : new <xsl:value-of select="$DocumentName"/>_Pointer();
        }
        
        public async ValueTask&lt;List&lt;<xsl:value-of select="$DocumentName"/>_Pointer&gt;&gt; FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List&lt;<xsl:value-of select="$DocumentName"/>_Pointer&gt; documentPointerList = [];
            foreach (var documentPointer in await base.BaseFindListByField(name, value, limit, offset)) 
                documentPointerList.Add(new <xsl:value-of select="$DocumentName"/>_Pointer(documentPointer.UnigueID, documentPointer.Fields));
            return documentPointerList;
        }
    }

      <xsl:for-each select="TabularParts/TablePart"> <!-- TableParts -->
        <xsl:variable name="TablePartName" select="Name"/>
        <xsl:variable name="TablePartFullName" select="concat($DocumentName, '_', $TablePartName)"/>
    
    public class <xsl:value-of select="$TablePartFullName"/>_TablePart : DocumentTablePart
    {
        public <xsl:value-of select="$TablePartFullName"/>_TablePart(<xsl:value-of select="$DocumentName"/>_Objest owner) : base(Config.Kernel, "<xsl:value-of select="Table"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>])
        {
            if (owner == null) throw new Exception("owner null");
            Owner = owner;
        }

        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>

        public <xsl:value-of select="$DocumentName"/>_Objest Owner { get; private set; }
        
        public List&lt;Record&gt; Records { get; set; } = [];
        
        public void FillJoin(string[]? orderFields = null)
        {
            QuerySelect.Clear();

            if (orderFields!=null)
              foreach(string field in orderFields)
                QuerySelect.Order.Add(field, SelectOrder.ASC);

            <xsl:for-each select="Fields/Field">
                <xsl:if test="Type = 'pointer'">
                  <xsl:value-of select="substring-before(Pointer, '.')"/>.<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer.GetJoin(QuerySelect, <xsl:value-of select="Name"/>, "<xsl:value-of select="../../Table"/>", "join_tab_<xsl:value-of select="position()"/>", "<xsl:value-of select="Name"/>");
                </xsl:if>
            </xsl:for-each>
        }

        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead(Owner.UnigueID);

            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    <xsl:for-each select="Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
                <xsl:if test="count(Fields/Field[Type = 'pointer']) != 0">
                if (JoinValue.TryGetValue(record.UID.ToString(), out var ItemValue))
                {
                  record.JoinItemValue = ItemValue;
                  <xsl:for-each select="Fields/Field">
                      <xsl:if test="Type = 'pointer'">
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.Name = ItemValue["<xsl:value-of select="Name"/>"];
                      </xsl:if>
                  </xsl:for-each>
                }
                </xsl:if>
            }
            
            base.BaseClear();
        }
        
        public async ValueTask Save(bool clear_all_before_save) 
        {
            if (!await base.IsExistOwner(Owner.UnigueID, "<xsl:value-of select="$DocumentTable"/>"))
                throw new Exception("Owner not exist");

            await base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                await base.BaseDelete(Owner.UnigueID);

            <xsl:for-each select="Fields/Field[Type = 'integer' and AutomaticNumbering = '1']">
            int sequenceNumber_<xsl:value-of select="Name"/> = 0;
            </xsl:for-each>

            foreach (Record record in Records)
            {
                <xsl:for-each select="Fields/Field[Type = 'integer' and AutomaticNumbering = '1']">
                record.<xsl:value-of select="Name"/> = ++sequenceNumber_<xsl:value-of select="Name"/>;
                </xsl:for-each>
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                {
                    <xsl:for-each select="Fields/Field">
                        <xsl:text>{"</xsl:text>
                        <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                        <xsl:if test="Type = 'enum'">
                          <xsl:text>(int)</xsl:text>
                        </xsl:if>
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                        <xsl:choose>
                            <xsl:when test="Type = 'pointer'">
                                <xsl:text>.UnigueID.UGuid</xsl:text>
                            </xsl:when>
                        </xsl:choose>
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
            
            await base.BaseCommitTransaction();
        }

        public async ValueTask Remove(Record record)
        {
            await base.BaseRemove(record.UID, Owner.UnigueID);
            Records.RemoveAll((Record item) =&gt; record.UID == item.UID);
        }

        public async ValueTask RemoveAll(List&lt;Record&gt; records)
        {
            List&lt;Guid&gt; removeList = [];

            await base.BaseBeginTransaction();
            foreach (Record record in records)
            {
                removeList.Add(record.UID);
                await base.BaseRemove(record.UID, Owner.UnigueID);
            }
            await base.BaseCommitTransaction();

            Records.RemoveAll((Record item) =&gt; removeList.Exists((Guid uid) =&gt; uid == item.UID));
        }

        public async ValueTask Delete()
        {
            await base.BaseDelete(Owner.UnigueID);
        }

        public List&lt;Record&gt; Copy()
        {
            List&lt;Record&gt; copyRecords = new(Records);
            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }

        public class Record : DocumentTablePartRecord
        {
            <xsl:for-each select="Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
              <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
    }
      </xsl:for-each> <!-- TableParts -->
    
    public static class <xsl:value-of select="$DocumentName"/>_Export
    {
        public static async ValueTask ToXmlFile(<xsl:value-of select="$DocumentName"/>_Pointer <xsl:value-of select="$DocumentName"/>, string pathToSave)
        {
        <xsl:choose>
          <xsl:when test="ExportXml = '1'">
            <xsl:value-of select="$DocumentName"/>_Objest? obj = await <xsl:value-of select="$DocumentName"/>.GetDocumentObject(true);
            if (obj == null) return;

            XmlWriter xmlWriter = XmlWriter.Create(pathToSave, new XmlWriterSettings() { Indent = true, Encoding = System.Text.Encoding.UTF8 });
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Document");
            xmlWriter.WriteAttributeString("uid", obj.UnigueID.ToString());
            <xsl:for-each select="Fields/Field[IsExport = '1']">
            xmlWriter.WriteStartElement("<xsl:value-of select="Name"/>");
            xmlWriter.WriteAttributeString("type", "<xsl:value-of select="Type"/>");
            <xsl:choose>
              <xsl:when test="Type = 'pointer'">
                <xsl:variable name="groupPointer" select="substring-before(Pointer, '.')" />
                <xsl:choose>
                  <xsl:when test="$groupPointer = 'Довідники' or $groupPointer = 'Документи'">
                    xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                    xmlWriter.WriteAttributeString("uid", obj.<xsl:value-of select="Name"/>.UnigueID.ToString());
                    xmlWriter.WriteCData(await obj.<xsl:value-of select="Name"/>.GetPresentation());
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="Type = 'string'">
                xmlWriter.WriteCData(obj.<xsl:value-of select="Name"/>);
              </xsl:when>
              <xsl:when test="Type = 'date'">
                xmlWriter.WriteValue(obj.<xsl:value-of select="Name"/>.ToString("dd.MM.yyyy"));
              </xsl:when>
              <xsl:when test="Type = 'datetime'">
                xmlWriter.WriteValue(obj.<xsl:value-of select="Name"/>.ToString("dd.MM.yyyy HH:mm:ss"));
              </xsl:when>
              <xsl:when test="Type = 'time'">
                xmlWriter.WriteValue(obj.<xsl:value-of select="Name"/>.ToString(@"hh\:mm\:ss"));
              </xsl:when>
              <xsl:when test="Type = 'enum'">
                xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                xmlWriter.WriteAttributeString("uid", ((int)obj.<xsl:value-of select="Name"/>).ToString());
                xmlWriter.WriteString(obj.<xsl:value-of select="Name"/>.ToString());
              </xsl:when>
              <xsl:when test="Type = 'composite_pointer'">
                xmlWriter.WriteRaw(((UuidAndText)obj.<xsl:value-of select="Name"/>).ToXml());
              </xsl:when>
              <xsl:otherwise>
                xmlWriter.WriteValue(obj.<xsl:value-of select="Name"/>);
              </xsl:otherwise>
            </xsl:choose>
            xmlWriter.WriteEndElement(); //<xsl:value-of select="Name"/>
            </xsl:for-each>

            <xsl:if test="count(TabularParts/TablePart) &gt; 0">
                /*  Табличні частини */
                xmlWriter.WriteStartElement("TabularParts");
                <xsl:for-each select="TabularParts/TablePart">
                    <xsl:variable name="TablePartName" select="Name"/>
                    <xsl:variable name="TablePartFullName" select="concat($DocumentName, '_', $TablePartName)"/>
                    xmlWriter.WriteStartElement("TablePart");
                    xmlWriter.WriteAttributeString("name", "<xsl:value-of select="Name"/>");

                    foreach(<xsl:value-of select="$TablePartFullName"/>_TablePart.Record record in obj.<xsl:value-of select="$TablePartName"/>_TablePart.Records)
                    {
                        xmlWriter.WriteStartElement("row");
                        xmlWriter.WriteAttributeString("uid", record.UID.ToString());
                        <xsl:for-each select="Fields/Field[IsExport = '1']">
                        xmlWriter.WriteStartElement("<xsl:value-of select="Name"/>");
                        xmlWriter.WriteAttributeString("type", "<xsl:value-of select="Type"/>");
                        <xsl:choose>
                          <xsl:when test="Type = 'pointer'">
                            <xsl:variable name="groupPointer" select="substring-before(Pointer, '.')" />
                            <xsl:choose>
                              <xsl:when test="$groupPointer = 'Довідники' or $groupPointer = 'Документи'">
                                xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                                xmlWriter.WriteAttributeString("uid", record.<xsl:value-of select="Name"/>.UnigueID.ToString());
                                xmlWriter.WriteCData(await record.<xsl:value-of select="Name"/>.GetPresentation());
                              </xsl:when>
                            </xsl:choose>
                          </xsl:when>
                          <xsl:when test="Type = 'string'">
                            xmlWriter.WriteCData(record.<xsl:value-of select="Name"/>);
                          </xsl:when>
                          <xsl:when test="Type = 'date'">
                            xmlWriter.WriteValue(record.<xsl:value-of select="Name"/>.ToString("dd.MM.yyyy"));
                          </xsl:when>
                          <xsl:when test="Type = 'datetime'">
                            xmlWriter.WriteValue(record.<xsl:value-of select="Name"/>.ToString("dd.MM.yyyy HH:mm:ss"));
                          </xsl:when>
                          <xsl:when test="Type = 'time'">
                            xmlWriter.WriteValue(record.<xsl:value-of select="Name"/>.ToString(@"hh\:mm\:ss"));
                          </xsl:when>
                          <xsl:when test="Type = 'enum'">
                            xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                            xmlWriter.WriteAttributeString("uid", ((int)record.<xsl:value-of select="Name"/>).ToString());
                            xmlWriter.WriteString(record.<xsl:value-of select="Name"/>.ToString());
                          </xsl:when>
                          <xsl:when test="Type = 'composite_pointer'">
                            xmlWriter.WriteRaw(((UuidAndText)record.<xsl:value-of select="Name"/>).ToXml());
                          </xsl:when>
                          <xsl:otherwise>
                            xmlWriter.WriteValue(record.<xsl:value-of select="Name"/>);
                          </xsl:otherwise>
                        </xsl:choose>
                        xmlWriter.WriteEndElement(); //<xsl:value-of select="Name"/>
                        </xsl:for-each>
                        xmlWriter.WriteEndElement(); //row
                    }

                    xmlWriter.WriteEndElement(); //TablePart
                </xsl:for-each>
                xmlWriter.WriteEndElement(); //TabularParts
            </xsl:if>

            xmlWriter.WriteEndElement(); //root
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
          </xsl:when>
          <xsl:otherwise>await ValueTask.FromResult(true);</xsl:otherwise>
        </xsl:choose>
        }
    }

    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.Журнали
{
    #region Journal
    public class JournalSelect: AccountingSoftware.JournalSelect
    {
        public JournalSelect() : base(Config.Kernel,
             <xsl:text>[</xsl:text><xsl:for-each select="Configuration/Documents/Document"><xsl:text>"</xsl:text><xsl:value-of select="Table"/><xsl:text>", </xsl:text></xsl:for-each>],
             <xsl:text>[</xsl:text><xsl:for-each select="Configuration/Documents/Document"><xsl:text>"</xsl:text><xsl:value-of select="Name"/><xsl:text>", </xsl:text></xsl:for-each>]) { }

        public async ValueTask&lt;DocumentObject?&gt; GetDocumentObject(bool readAllTablePart = true)
        {
            if (Current == null) return null;
            return Current.TypeDocument switch
            {
                <xsl:for-each select="Configuration/Documents/Document">
                    <xsl:text>"</xsl:text><xsl:value-of select="Name"/>" =&gt; await new Документи.<xsl:value-of select="Name"/>_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart),
                </xsl:for-each>
                <xsl:text>_ =&gt; null</xsl:text>
            };
        }
    }
    #endregion
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.РегістриВідомостей
{
    <xsl:for-each select="Configuration/RegistersInformation/RegisterInformation">
       <xsl:variable name="RegisterName" select="Name"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    public static class <xsl:value-of select="$RegisterName"/>_Const
    {
        public const string FULLNAME = "<xsl:value-of select="normalize-space(FullName)"/>";
        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }

    public class <xsl:value-of select="$RegisterName"/>_RecordsSet : RegisterInformationRecordsSet
    {
        public <xsl:value-of select="$RegisterName"/>_RecordsSet() : base(Config.Kernel, "<xsl:value-of select="Table"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>]) { }
		
        public List&lt;Record&gt; Records { get; set; } = [];
        
        public void FillJoin(string[]? orderFields = null)
        {
            QuerySelect.Clear();

            if (orderFields!=null)
              foreach(string field in orderFields)
                QuerySelect.Order.Add(field, SelectOrder.ASC);

            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                <xsl:if test="Type = 'pointer'">
                  <xsl:value-of select="substring-before(Pointer, '.')"/>.<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer.GetJoin(QuerySelect, 
                  <xsl:value-of select="$RegisterName"/>_Const.<xsl:value-of select="Name"/>, "<xsl:value-of select="../../../Table"/>", "join_tab_<xsl:value-of select="position()"/>", "<xsl:value-of select="Name"/>");
                </xsl:if>
            </xsl:for-each>
        }

        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead();
            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    Period = DateTime.Parse(fieldValue["period"]?.ToString() ?? DateTime.MinValue.ToString()),
                    Owner = (Guid)fieldValue["owner"],
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
                <xsl:if test="count((DimensionFields|ResourcesFields|PropertyFields)/Fields/Field[Type = 'pointer']) != 0">
                  if (JoinValue.TryGetValue(record.UID.ToString(), out var ItemValue))
                  {
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                        <xsl:if test="Type = 'pointer'">
                          <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.Name = ItemValue["<xsl:value-of select="Name"/>"];
                        </xsl:if>
                    </xsl:for-each>
                  }
                </xsl:if>
            }
            base.BaseClear();
        }
        
        public async ValueTask Save(DateTime period, Guid owner)
        {
            await base.BaseBeginTransaction();
            await base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                {
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                        <xsl:text>{"</xsl:text>
                        <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                        <xsl:if test="Type = 'enum'">
                            <xsl:text>(int)</xsl:text>      
                        </xsl:if>
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                        <xsl:if test="Type = 'pointer'">
                        <xsl:text>.UnigueID.UGuid</xsl:text>
                        </xsl:if>
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, period, owner, fieldValue);
            }
            await base.BaseCommitTransaction();
        }

        public async ValueTask Remove(Record record)
        {
            await base.BaseRemove(record.UID);
            Records.RemoveAll((Record item) =&gt; record.UID == item.UID);
        }

        public async ValueTask RemoveAll(List&lt;Record&gt; records)
        {
            List&lt;Guid&gt; removeList = [];

            await base.BaseBeginTransaction();
            foreach (Record record in records)
            {
                removeList.Add(record.UID);
                await base.BaseRemove(record.UID);
            }
            await base.BaseCommitTransaction();

            Records.RemoveAll((Record item) =&gt; removeList.Exists((Guid uid) =&gt; uid == item.UID));
        }
        
        public async ValueTask Delete(Guid owner)
        {
            await base.BaseDelete(owner);
        }

        public class Record : RegisterInformationRecord
        {
            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
              <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
    }

    public class <xsl:value-of select="$RegisterName"/>_Objest : RegisterInformationObject
    {
        public event EventHandler&lt;UnigueID&gt;? UnigueIDChanged;
        public event EventHandler&lt;string&gt;? CaptionChanged;

		    public <xsl:value-of select="$RegisterName"/>_Objest() : base(Config.Kernel, "<xsl:value-of select="Table"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>]) { }

        public void New()
        {
            BaseNew();
            UnigueIDChanged?.Invoke(this, base.UnigueID);
            CaptionChanged?.Invoke(this, <xsl:value-of select="$RegisterName"/>_Const.FULLNAME + " *");
        }

        public async ValueTask&lt;bool&gt; Read(UnigueID uid)
        {
            if (await BaseRead(uid))
            {
                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">base.FieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                BaseClear();
                UnigueIDChanged?.Invoke(this, base.UnigueID);
                CaptionChanged?.Invoke(this, string.Join(", ", [Period.ToString(), <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field[IsPresentation=1]"><xsl:value-of select="Name"/>, </xsl:for-each>]));
                return true;
            }
            else
                return false;
        }
        
        public async ValueTask&lt;bool&gt; Save()
        {
            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
              <xsl:text>base.FieldValue["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] = </xsl:text>
              <xsl:if test="Type = 'enum'">
                  <xsl:text>(int)</xsl:text>      
              </xsl:if>
              <xsl:value-of select="Name"/>
              <xsl:choose>
                <xsl:when test="Type = 'pointer'">
                  <xsl:text>.UnigueID.UGuid</xsl:text>
                </xsl:when>
              </xsl:choose>;
            </xsl:for-each>
            bool result = await BaseSave();
            CaptionChanged?.Invoke(this, string.Join(", ", [Period.ToString(), <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field[IsPresentation=1]"><xsl:value-of select="Name"/>, </xsl:for-each>]));
            return result;
        }

        public <xsl:value-of select="$RegisterName"/>_Objest Copy()
        {
            <xsl:value-of select="$RegisterName"/>_Objest copy = new <xsl:value-of select="$RegisterName"/>_Objest()
            {
                Period = Period, /* Базове поле */
                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                    <xsl:value-of select="Name"/><xsl:text> = </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
            copy.New();
            return copy;
        }

        public async ValueTask Delete()
        {
			      await base.BaseDelete();
        }

        <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
          <xsl:text>public </xsl:text>
          <xsl:call-template name="FieldType" />
          <xsl:text> </xsl:text>
          <xsl:value-of select="Name"/>
          <xsl:text> { get; set; } = </xsl:text>
          <xsl:call-template name="DefaultFieldValue" />;
        </xsl:for-each>
    }
	
    #endregion
  </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.РегістриНакопичення
{
    public static class VirtualTablesСalculation
    {
        /* Функція повного очищення віртуальних таблиць */
        public static void ClearAll()
        {
            /*  */
        }

        /* Функція для обчислення віртуальних таблиць  */
        public static async ValueTask Execute(DateTime period, string regAccumName)
        {
            <xsl:variable name="QueryAllCountCalculation" select="count(Configuration/RegistersAccumulation/RegisterAccumulation/QueryBlockList/QueryBlock[FinalCalculation = '0']/Query)"/>
            <xsl:if test="$QueryAllCountCalculation != 0">
            Dictionary&lt;string, object&gt; paramQuery = new Dictionary&lt;string, object&gt;{ { "ПеріодДеньВідбір", period } };

            switch(regAccumName)
            {
            <xsl:for-each select="Configuration/RegistersAccumulation/RegisterAccumulation">
                <xsl:variable name="QueryCount" select="count(QueryBlockList/QueryBlock[FinalCalculation = '0']/Query)"/>
                <xsl:if test="$QueryCount != 0">
                case "<xsl:value-of select="Name"/>":
                {
                    byte transactionID = await Config.Kernel.DataBase.BeginTransaction();
                    <xsl:for-each select="QueryBlockList/QueryBlock[FinalCalculation = '0']">
                    /* QueryBlock: <xsl:value-of select="Name"/> */
                        <xsl:for-each select="Query">
                            <xsl:sort select="@position" data-type="number" order="ascending" />
                    await Config.Kernel.DataBase.ExecuteSQL($@"<xsl:value-of select="normalize-space(.)"/>", paramQuery, transactionID);
                        </xsl:for-each>
                    </xsl:for-each>
                    await Config.Kernel.DataBase.CommitTransaction(transactionID);
                    break;
                }
                </xsl:if>
            </xsl:for-each>
                    default:
                        break;
            }
            </xsl:if>
            <xsl:if test="$QueryAllCountCalculation = 0">
                <xsl:text>await ValueTask.FromResult(true);</xsl:text>
            </xsl:if>
        }

        /* Функція для обчислення підсумкових віртуальних таблиць */
        public static async ValueTask ExecuteFinalCalculation(List&lt;string&gt; regAccumNameList)
        {
            <xsl:variable name="QueryAllCountFinalCalculation" select="count(Configuration/RegistersAccumulation/RegisterAccumulation/QueryBlockList/QueryBlock[FinalCalculation = '1']/Query)"/>
            <xsl:if test="$QueryAllCountFinalCalculation != 0">
            foreach (string regAccumName in regAccumNameList)
                switch(regAccumName)
                {
                <xsl:for-each select="Configuration/RegistersAccumulation/RegisterAccumulation">
                    <xsl:variable name="QueryCount" select="count(QueryBlockList/QueryBlock[FinalCalculation = '1']/Query)"/>
                    <xsl:if test="$QueryCount != 0">
                    case "<xsl:value-of select="Name"/>":
                    {
                        byte transactionID = await Config.Kernel.DataBase.BeginTransaction();
                        <xsl:for-each select="QueryBlockList/QueryBlock[FinalCalculation = '1']">
                        /* QueryBlock: <xsl:value-of select="Name"/> */
                            <xsl:for-each select="Query">
                                <xsl:sort select="@position" data-type="number" order="ascending" />
                        await Config.Kernel.DataBase.ExecuteSQL($@"<xsl:value-of select="normalize-space(.)"/>", null, transactionID);
                            </xsl:for-each>
                        </xsl:for-each>
                        await Config.Kernel.DataBase.CommitTransaction(transactionID);
                        break;
                    }
                    </xsl:if>
                </xsl:for-each>
                        default:
                            break;
                }
            </xsl:if>
            <xsl:if test="$QueryAllCountCalculation = 0">
                <xsl:text>await ValueTask.FromResult(true);</xsl:text>
            </xsl:if>
        }
    }

    <xsl:for-each select="Configuration/RegistersAccumulation/RegisterAccumulation">
	    <xsl:variable name="Documents" select="../../Documents"/>
      <xsl:variable name="RegisterName" select="Name"/>
      <xsl:variable name="Table" select="Table"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    public static class <xsl:value-of select="$RegisterName"/>_Const
    {
        public const string FULLNAME = "<xsl:value-of select="normalize-space(FullName)"/>";
        public const string TABLE = "<xsl:value-of select="Table"/>";
		    public static readonly string[] AllowDocumentSpendTable = [<xsl:for-each select="AllowDocumentSpend/Name">
			  <xsl:variable name="AllowDocumentSpendName" select="."/>
        <xsl:text>"</xsl:text><xsl:value-of select="$Documents/Document[Name = $AllowDocumentSpendName]/Table"/><xsl:text>", </xsl:text></xsl:for-each>];
		    public static readonly string[] AllowDocumentSpendType = [<xsl:for-each select="AllowDocumentSpend/Name"><xsl:text>"</xsl:text><xsl:value-of select="."/><xsl:text>", </xsl:text></xsl:for-each>];
        <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }
	
    public class <xsl:value-of select="$RegisterName"/>_RecordsSet : RegisterAccumulationRecordsSet
    {
        public <xsl:value-of select="$RegisterName"/>_RecordsSet() : base(Config.Kernel, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$RegisterName"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>]) { }
		
        public List&lt;Record&gt; Records { get; set; } = [];
        
        public void FillJoin(string[]? orderFields = null, bool docname_required = true)
        {
            QuerySelect.Clear();

            if (orderFields!=null)
              foreach(string field in orderFields)
                QuerySelect.Order.Add(field, SelectOrder.ASC);

            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                <xsl:if test="Type = 'pointer'">
                  <xsl:value-of select="substring-before(Pointer, '.')"/>.<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer.GetJoin(QuerySelect, 
                  <xsl:value-of select="$RegisterName"/>_Const.<xsl:value-of select="Name"/>, "<xsl:value-of select="$Table"/>", "join_tab_<xsl:value-of select="position()"/>", "<xsl:value-of select="Name"/>");
                </xsl:if>
            </xsl:for-each>

            //Назва документу
            if (docname_required)
            {
              <xsl:text>string query_case = $"CASE </xsl:text>
              <xsl:for-each select="AllowDocumentSpend/Name">
                <xsl:variable name="AllowDocumentSpendName" select="."/>
                <xsl:variable name="AllowDocumentSpendTable" select="$Documents/Document[Name = $AllowDocumentSpendName]/Table"/>
                <xsl:value-of select="concat('WHEN join_doc_', position(), '.uid IS NOT NULL THEN join_doc_', position(), '.{Документи.', $AllowDocumentSpendName, '_Const.Назва} ')"/>
              </xsl:for-each>
              <xsl:text>END</xsl:text>";
              QuerySelect.FieldAndAlias.Add(new NameValue&lt;string&gt;(query_case, "docname"));

              int i = 0;
              foreach (string table in <xsl:value-of select="$RegisterName"/>_Const.AllowDocumentSpendTable)
                  QuerySelect.Joins.Add(new Join(table, "owner", "<xsl:value-of select="$Table"/>", $"join_doc_{++i}"));
            }
        }

        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead();
            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    Period = DateTime.Parse(fieldValue["period"]?.ToString() ?? DateTime.MinValue.ToString()),
                    Income = (bool)fieldValue["income"],
                    Owner = (Guid)fieldValue["owner"],
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
                <xsl:if test="count((DimensionFields|ResourcesFields|PropertyFields)/Fields/Field[Type = 'pointer']) != 0">
                if (JoinValue.TryGetValue(record.UID.ToString(), out var ItemValue))
                {
                    //record.JoinItemValue = ItemValue;
                    if (ItemValue.TryGetValue("docname", out var ownerName)) record.OwnerName = ownerName;
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                        <xsl:if test="Type = 'pointer'">
                          <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.Name = ItemValue["<xsl:value-of select="Name"/>"];
                        </xsl:if>
                    </xsl:for-each>
                }
                </xsl:if>
            }
            base.BaseClear();
        }
        
        public async ValueTask Save(DateTime period, Guid owner) 
        {
            await base.BaseBeginTransaction();
            await base.BaseSelectPeriodForOwner(owner, period);
            await base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                {
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                        <xsl:text>{"</xsl:text>
                        <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                        <xsl:if test="Type = 'enum'">
                            <xsl:text>(int)</xsl:text>      
                        </xsl:if>
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                        <xsl:if test="Type = 'pointer'">
                        <xsl:text>.UnigueID.UGuid</xsl:text>
                        </xsl:if>
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            await base.BaseTrigerAdd(period, owner);
            await base.BaseCommitTransaction();
        }

        public async ValueTask Delete(Guid owner)
        {
            await base.BaseSelectPeriodForOwner(owner);
            await base.BaseDelete(owner);
        }
        
        public class Record : RegisterAccumulationRecord
        {
            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
               <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
    }
    
    <xsl:for-each select="TabularParts/TablePart">
        <!-- TableParts -->
        <xsl:variable name="TablePartName" select="Name"/>
        <xsl:variable name="TablePartFullName" select="concat($RegisterName, '_', $TablePartName)"/>
    
    public class <xsl:value-of select="$TablePartFullName"/>_TablePart : RegisterAccumulationTablePart
    {
        public <xsl:value-of select="$TablePartFullName"/>_TablePart() : base(Config.Kernel, "<xsl:value-of select="Table"/>",
              <xsl:text>[</xsl:text>
              <xsl:for-each select="Fields/Field">
                <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
              </xsl:for-each>]) { }
        
        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
        public List&lt;Record&gt; Records { get; set; } = [];
    
        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead();
            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    <xsl:for-each select="Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
            }
            base.BaseClear();
        }
    
        public async ValueTask Save(bool clear_all_before_save /*= true*/) 
        {
            await base.BaseBeginTransaction();
            if (clear_all_before_save) await base.BaseDelete();
            foreach (Record record in Records)
            {
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                {
                    <xsl:for-each select="Fields/Field">
                        <xsl:text>{"</xsl:text>
                        <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                        <xsl:if test="Type = 'enum'">
                            <xsl:text>(int)</xsl:text>
                          </xsl:if>
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                        <xsl:choose>
                        <xsl:when test="Type = 'pointer'">
                            <xsl:text>.UnigueID.UGuid</xsl:text>
                        </xsl:when>
                        </xsl:choose>
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, fieldValue);
            }
            await base.BaseCommitTransaction();
        }

        public async ValueTask Remove(Record record)
        {
            await base.BaseRemove(record.UID);
            Records.RemoveAll((Record item) =&gt; record.UID == item.UID);
        }

        public async ValueTask RemoveAll(List&lt;Record&gt; records)
        {
            List&lt;Guid&gt; removeList = [];

            await base.BaseBeginTransaction();
            foreach (Record record in records)
            {
                removeList.Add(record.UID);
                await base.BaseRemove(record.UID);
            }
            await base.BaseCommitTransaction();

            Records.RemoveAll((Record item) =&gt; removeList.Exists((Guid uid) =&gt; uid == item.UID));
        }
    
        public async ValueTask Delete()
        {
            await base.BaseDelete();
        }
        
        public class Record : RegisterAccumulationTablePartRecord
        {
            <xsl:for-each select="Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
              <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }            
    }
    </xsl:for-each> <!-- TableParts -->
    #endregion
  </xsl:for-each>
}
  </xsl:template>
</xsl:stylesheet>