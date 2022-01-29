using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7DTD_Loot_Parser
{
    public class LocalizationParser
    {
        private Dictionary<string, string> ParseLocalizationFile()
        {
            /*
            To find Display name for containers
            1) Scan blocks.xml to find the LootList entry for that group:
             
            blocks.xml:
            <block name="cntMedicineCabinetClosed">
	            <property name="Extends" value="cntMedicineCabinetOpen"/>
	            <property name="CreativeMode" value="Player"/>
	            <property name="Class" value="Loot"/>
	        --> <property name="LootList" value="medicineCabinet"/>
	            <property name="Model" value="#Entities/LootContainers?medicine_cabinet_closedPrefab.prefab"/>
            </block>

            2) Find the localization value in localization.txt
            localization.txt:
            cntMedicineCabinetClosed,blocks,Container,,,"Medicine Cabinet, Closed"
            */
            var data = new Dictionary<string, string>();
            using (var parser = new TextFieldParser("Localization.txt"))
            {
                //parser.CommentTokens = new string[] { "#" };
                parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = false;

                // Skip over header line.
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] fields = parser.ReadFields();

                        //if (fields[1] == "items" && fields[2] == "Item")
                        if (fields[5] != "")
                        {
                            data.Add(fields[0], fields[5]);
                        }
                    }
                    catch { };


                }
            }
            return data;
        }

    }

    public class LocalizationData
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
