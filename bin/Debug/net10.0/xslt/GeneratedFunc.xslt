<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes" />

  <xsl:template name="Item">
      <xsl:param name="DirectoryOrDocument" />
      <xsl:for-each select="$DirectoryOrDocument">
          <xsl:variable name="FieldCount" select="count(Fields/Field[IsPresentation=1])"/>
          <xsl:text>WHEN '</xsl:text><xsl:value-of select="Name"/>' THEN (SELECT <xsl:choose>
            <xsl:when test="$FieldCount = 1">"<xsl:value-of select="Fields/Field[IsPresentation=1]/NameInTable"/>"</xsl:when>
            <xsl:when test="$FieldCount &gt; 1">
                <xsl:text>concat_ws (', ', </xsl:text>
                <xsl:for-each select="Fields/Field[IsPresentation=1]">
                    <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
                    <xsl:if test="position() != $FieldCount">, </xsl:if>
                </xsl:for-each>
                <xsl:text>)</xsl:text>
            </xsl:when>
            <xsl:otherwise>
                <xsl:text>"#"</xsl:text>
            </xsl:otherwise>
          </xsl:choose> FROM <xsl:value-of select="Table"/> WHERE uid = row_uid)
      </xsl:for-each>
  </xsl:template>

  <xsl:template match="/">

    <!--
      CREATE OR REPLACE VIEW view_special_presentation AS
      <xsl:for-each select="Configuration/Directories/Directory | Configuration/Documents/Document">
          <xsl:variable name="FieldCount" select="count(Fields/Field[IsPresentation=1])"/>
          <xsl:if test="position() &gt; 1">
      UNION ALL</xsl:if>
      SELECT (uid, '<xsl:choose>
            <xsl:when test="name() = 'Directory'">Довідники.</xsl:when>
            <xsl:when test="name() = 'Document'">Документи.</xsl:when>
        </xsl:choose><xsl:value-of select="Name"/>')::uuidtext AS uid, <xsl:choose>
          <xsl:when test="$FieldCount = 1">"<xsl:value-of select="Fields/Field[IsPresentation=1]/NameInTable"/>"</xsl:when>
          <xsl:when test="$FieldCount &gt; 1">
              <xsl:text>concat_ws (', ', </xsl:text>
              <xsl:for-each select="Fields/Field[IsPresentation=1]">
                  <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
                  <xsl:if test="position() != $FieldCount">, </xsl:if>
              </xsl:for-each>
              <xsl:text>)</xsl:text>
          </xsl:when>
          <xsl:otherwise>
              <xsl:text>"#"</xsl:text>
          </xsl:otherwise>
        </xsl:choose> AS name FROM <xsl:value-of select="Table"/>
      </xsl:for-each>
    -->

    <root>
      <info>Створити функцію func_special_composite_presentation(data uuidtext)</info>
      <sql>
        CREATE OR REPLACE FUNCTION func_special_composite_presentation(data uuidtext) 
        RETURNS TEXT AS $$
        DECLARE
            conf_group TEXT; -- Змінна для групи (Довідники або Документи)
            conf_name  TEXT; -- Змінна для назви об'єкта
            row_uid uuid;
        BEGIN
            conf_group := split_part(data.text, '.', 1); -- Отримаємо частину ДО крапки
            conf_name  := split_part(data.text, '.', 2); -- Отримаємо частину ПІСЛЯ крапки
            row_uid := data.uuid;

            IF conf_group = 'Довідники' THEN
              RETURN CASE conf_name
                <xsl:call-template name="Item">
                    <xsl:with-param name="DirectoryOrDocument" select="Configuration/Directories/Directory" />
                </xsl:call-template>
              END;
            ELSIF conf_group = 'Документи' THEN
              RETURN CASE conf_name
                <xsl:call-template name="Item">
                    <xsl:with-param name="DirectoryOrDocument" select="Configuration/Documents/Document" />
                </xsl:call-template>
              END;
            END IF;

            RETURN NULL;
        END;
        $$ LANGUAGE plpgsql STABLE;
      </sql>

      <info>Створити функцію func_special_composite_presentation(row_uid uuid, conf_type nametext)</info>
      <sql>
        CREATE OR REPLACE FUNCTION func_special_composite_presentation(row_uid uuid, conf_type nametext) 
        RETURNS TEXT AS $$
        BEGIN
            IF conf_type.name = 'Довідники' THEN
              RETURN CASE conf_type.text
                <xsl:call-template name="Item">
                    <xsl:with-param name="DirectoryOrDocument" select="Configuration/Directories/Directory" />
                </xsl:call-template>
              END;
            ELSIF conf_type.name = 'Документи' THEN
              RETURN CASE conf_type.text
                <xsl:call-template name="Item">
                    <xsl:with-param name="DirectoryOrDocument" select="Configuration/Documents/Document" />
                </xsl:call-template>
              END;
            END IF;

            RETURN NULL;
        END;
        $$ LANGUAGE plpgsql STABLE;
      </sql>
    </root>
  </xsl:template>
</xsl:stylesheet>