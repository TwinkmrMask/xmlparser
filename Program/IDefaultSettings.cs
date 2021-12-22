﻿using System.Collections.Generic;

namespace XmlParser
{
    public interface IDefaultSettings
    {
        const string DefaultPath = "../../../Resources/";
        static string NameExcelFile => DefaultPath + "declarationInfo.xlsx";
        static string IndexFileName => DefaultPath + "links";
        static string DataFileName => DefaultPath + "db";

        static readonly List<string> TransportDocumentCodes = new()
        {
            "02011", "02012", "02013",
            "02014", "02015", "02016",
            "02017", "02018", "02019",
            "02020", "02021", "02022",
            "02024", "02025", "02099"
        };
        
    }
}
