<?xml version="1.0" encoding="utf-8"?>
<!--
/*

Друга ступінь перетворення ХМЛ файлу вигружених даних в ХМЛ файл готових SQL запитів з параметрами

Результат:

  <row>
    <sql>
		INSERT INTO tab_a77 (uid, col_a2, col_a9, col_a1, col_a3, col_a8, col_a5, col_a6, col_a7) 
		VALUES (@uid, @col_a2, @col_a9, @col_a1, @col_a3, @col_a8, @col_a5, @col_a6, @col_a7) 
		ON CONFLICT (uid) DO UPDATE SET uid = @uid, col_a2 = @col_a2, col_a9 = @col_a9, col_a1 = @col_a1, col_a3 = @col_a3, 
		col_a8 = @col_a8, col_a5 = @col_a5, col_a6 = @col_a6, col_a7 = @col_a7
	</sql>
	
    <p name="uid" type="Guid">1a13e463-e587-4184-b8e3-be2c28bd82bd</p>
    <p name="col_a2" type="DateTime">25.07.2022 17:16:53</p>
    <p name="col_a9" type="String">1fcdb976-2ca4-40ff-96ef-8d57eb424f0e</p>
    <p name="col_a1" type="String">АктВиконанихРобіт</p>
    <p name="col_a3" type="DateTime">25.07.2022 0:00:00</p>
    <p name="col_a8" type="String">Add</p>
    <p name="col_a5" type="Boolean">False</p>
    <p name="col_a6" type="Boolean">True</p>
    <p name="col_a7" type="String"></p>
  </row>


Перша ступень:

  <sql tab="tab_a77">
    <row name="uid" type="Guid">1a13e463-e587-4184-b8e3-be2c28bd82bd</row>
    <row name="col_a2" type="DateTime">25.07.2022 17:16:53</row>
    <row name="col_a9" type="String">1fcdb976-2ca4-40ff-96ef-8d57eb424f0e</row>
    <row name="col_a1" type="String">АктВиконанихРобіт</row>
    <row name="col_a3" type="DateTime">25.07.2022 0:00:00</row>
    <row name="col_a8" type="String">Add</row>
    <row name="col_a5" type="Boolean">False</row>
    <row name="col_a6" type="Boolean">True</row>
    <row name="col_a7" type="String"></row>
  </sql>

*/
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/">
		<root>
		
		<xsl:for-each select="root/sql">

			<row>
			
				<sql>INSERT INTO <xsl:value-of select="@tab"/>
				<xsl:text> (</xsl:text>
			
				<xsl:for-each select="row">
					<xsl:if test="position() != 1">
						<xsl:text>, </xsl:text>
					</xsl:if>
					<xsl:value-of select="@name"/>   
				</xsl:for-each>

				<xsl:text>) VALUES (</xsl:text>

				<xsl:for-each select="row">
					<xsl:if test="position() != 1">
						<xsl:text>, </xsl:text>
					</xsl:if>
					<xsl:value-of select="concat('@', @name)"/>
				</xsl:for-each>
			
				<xsl:text>) ON CONFLICT (uid) DO UPDATE SET </xsl:text>
			
				<xsl:for-each select="row">
					<xsl:if test="position() != 1">
						<xsl:text>, </xsl:text>
					</xsl:if>
					<xsl:value-of select="@name"/>
					<xsl:text> = </xsl:text>
					<xsl:value-of select="concat('@', @name)"/>
				</xsl:for-each>
			
				</sql>

				<xsl:for-each select="row">
					<p name="{@name}" type="{@type}">
						<xsl:choose>
							<xsl:when test="@type = 'String[]' or @type = 'Int32[]' or @type = 'Decimal[]' or @type = 'UuidAndText'">
								<xsl:copy-of select="node()"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="text()"/>
							</xsl:otherwise>
						</xsl:choose>
					</p>
				</xsl:for-each>
				
			</row>
				
		</xsl:for-each>
			
		</root>
	
    </xsl:template>
	
</xsl:stylesheet>
