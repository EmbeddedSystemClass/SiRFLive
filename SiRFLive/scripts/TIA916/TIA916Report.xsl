<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="html" indent="yes" encoding="iso-8859-1" doctype-public="-//W3C//DTD HTML 4.01//EN" 
	doctype-system="http: //www.w3.org/TR/html4/strict.dtd"/>
	<xsl:template match="TIA916">
		<html>
			<head>
				<title> TIA916 Test Report Summary </title>
			</head>
			<body>
				<xsl:apply-templates select="swVersion"/>
        <xsl:apply-templates select="testSetup"/>
			</body>
		</html>
	</xsl:template>
  <xsl:template match="testSetup">
    <h1 align="center"><b>QoS Settings</b></h1>
    <table align="center" border="1">
      <tr>
        <th></th>
        <xsl:for-each select="rx[1]/field">
          <th width="50">
            <font size="2">
              <xsl:value-of select="@name" />
            </font>
          </th>
        </xsl:for-each>
      </tr>
      <xsl:for-each select="rx">
        <tr>
          <td align="center" width="40">
            <xsl:value-of select="@name" />
          </td>
          <xsl:apply-templates select="field" />
        </tr>
      </xsl:for-each>           
    </table>
  </xsl:template>
	<xsl:template match="swVersion">
		<h1 align="center"><xsl:value-of select="@name"/> </h1>
		<table align="center" border="1">
			<tr>
				<th></th>
				<th></th>
				<xsl:for-each select="environment[1]/site[1]/field">
					<th width="50"><font size="2"><xsl:value-of select="@name" /></font></th>
				</xsl:for-each>	
			</tr>			
			<xsl:apply-templates select="environment" />
		</table>
		<br /> <br />
	</xsl:template>
	
	<xsl:template match="environment">
		<xsl:variable name="environment" select="@name" />
		<xsl:variable name="numberOfSites" select="count(site)" />
		<xsl:for-each select="site">
			<tr>
				<xsl:if test="position() = 1">
					<td rowspan="{$numberOfSites}"><xsl:value-of select="$environment" /></td>
				</xsl:if>
				<td align="center" width="40"><xsl:value-of select="@number" /></td>
				<xsl:apply-templates select="field" />
			</tr>			
		</xsl:for-each>
		<tr>
				<td align="right" colspan="2"><b>Limits</b></td>
				<xsl:for-each select="site[1]/field">
					<td width="50"><xsl:value-of select="@direction" /><xsl:value-of select="@criteria" /></td>
				</xsl:for-each>					
			</tr>
	</xsl:template>
	
	<xsl:template match="field">
		<xsl:choose>
			<xsl:when test="@criteria = ''">
				<td align="center"><xsl:value-of select="@value" /></td>
			</xsl:when>
			<xsl:when test="@value='Pass'and @criteria = 'Pass/Fail'">
				<td align="center" bgcolor="limegreen"><xsl:value-of select="@value" /></td>
			</xsl:when>
			<xsl:when test="(@value &lt; @criteria and @direction = '&lt;') or (@value &gt; @criteria and @direction = '&gt;')" >
				<td align="center" bgcolor="limegreen"><xsl:value-of select="@value" /></td>
			</xsl:when>
			<xsl:when test="(@value &lt;= @criteria and @direction = '&lt;=') or (@value &gt;= @criteria and @direction = '&gt;=')" >
				<td align="center" bgcolor="limegreen"><xsl:value-of select="@value" /></td>
			</xsl:when>			
			<xsl:otherwise>
				<td align="center" bgcolor="red"><xsl:value-of select="@value" /></td>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>