using System;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public class StringToArray
{
    #region RETORNA ARRAY DE STRINGS

    [SqlFunction(FillRowMethodName = "StringListFillRow",TableDefinition = "[valor] [nvarchar] (max) NULL", Name = "FN_StringToArrayStr")]
    public static IEnumerable StringToArrayStr(SqlString str, SqlString delimiter)
    {
        return SplitString(ref str, ref delimiter);      
    }

    private static IEnumerable SplitString(ref SqlString str, ref SqlString delimiter)
    {
        if (!str.IsNull && (!delimiter.IsNull && delimiter.Value.Length>0))
        {
            return str.Value.Split(delimiter.Value.ToCharArray(0, 1));
        }
        else
        {
            return "";
        }
    }
    public static void StringListFillRow(object row, out string str)
    {
        str = (string)row;
        str = str.Trim();
    }

    #endregion

    #region RETORNA ARRAY DE INTEIROS

    [SqlFunction(FillRowMethodName = "Int32ListFillRow", TableDefinition = "[valor] [int] NULL", Name = "FN_StringToArrayInt32")]
    public static IEnumerable StringToArrayInt32(SqlString str, SqlString delimiter)
    {
        return SplitString(ref str, ref delimiter);
    }
    public static void Int32ListFillRow(object row, out int n)
    {
        n = System.Convert.ToInt32((string)row);
    }

    #endregion

    #region RETORNA ARRAY DE INTEIROS LONGO

    [SqlFunction(FillRowMethodName = "Int64ListFillRow", TableDefinition = "[valor] [bigint] NULL", Name = "FN_StringToArrayInt64")]
    public static IEnumerable StringToArrayInt64(SqlString str, SqlString delimiter)
    {
        return SplitString(ref str, ref delimiter);
    }
    public static void Int64ListFillRow(object row, out long n)
    {
        n = System.Convert.ToInt64((string)row);
    }

    #endregion

    #region RETORNA ARRAY DE UNIQUE IDENTIFIER

    [SqlFunction(FillRowMethodName = "UniqueIdentifierListFillRow", TableDefinition = "[valor] [uniqueidentifier] NULL", Name = "FN_StringToArrayUniqueIdentifier")]
    public static IEnumerable StringToArrayUniqueIdentifier(SqlString str, SqlString delimiter)
    {
        return SplitString(ref str, ref delimiter);
    }
    public static void UniqueIdentifierListFillRow(object row, out Guid n)
    {
        n = new Guid((string) row);
    }

    #endregion
}