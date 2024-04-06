using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using Autodesk.DataExchange.Core.Models;
using Autodesk.DataExchange.DataModels;
using Autodesk.DataExchange.SchemaObjects.Components;
using Autodesk.DataExchange.SchemaObjects.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataExchangeGrasshopperConnector.Evaluation
{
    public class CreateJson
    {
        public static void WriteJson(List<Element> elementList)
        {
            var jsonArray = new JArray();
            var groupedElements = elementList.GroupBy(e => e.Category);
            foreach (var categoryGroup in groupedElements)
            {
                var categoryData = new JObject
                {
                    ["Category"] = categoryGroup.Key,
                    ["Data"] = new JArray()
                };

                foreach (var element in categoryGroup)
                {
                    var elementData = new JObject
                    {
                        ["ElementName"] = element.Name,
                        ["Transformation"] = SerializeTransformation(element.Asset.Transformation.MatrixRepresentation)
                    };
                    ((JArray)categoryData["Data"]).Add(elementData);
                }

                jsonArray.Add(categoryData);
               
            }

            var jsonObject = new JObject
            {
                ["Element"] = jsonArray
            };
            string filepath = @"E:\PSET\grashopper_unit_testing\DataExchangeGrasshopperConnector.Test\Evaluation\JSONData\ElementList3.json";
            string jsonString = jsonObject.ToString();
            string jsonString2 = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            File.WriteAllText(filepath, jsonString2);

        }

        static JObject SerializeTransformation(Matrix4d transformation)
        {
            var jsonObject = new JObject();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var value = transformation.Elements.GetValue(i);
                    jsonObject[$"transformationObject.M{i}{j}"] = (float)value;
                }
            }

            return jsonObject;
        }
    }

}
    
