using System.Xml.Linq;
using Robotersteuerung.Models;

namespace Robotersteuerung.Utils
{
    public static class FieldLoader
    {
        /// <summary>
        /// Das Custom Control parst das XML selbst.
        /// Diese Methode prüft nur ob die Datei existiert.
        /// </summary>
        public static void ValidateXmlFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception($"XML-Datei nicht gefunden: {filePath}");

            try
            {
                // Nur validieren dass es gültiges XML ist
                XDocument doc = XDocument.Load(filePath);
                
                XElement root = doc.Root;
                if (root == null)
                    throw new Exception("XML-Datei ist leer");

                // Unterstützte Formate prüfen
                if (root.Name.LocalName != "XML_Field" && root.Name.LocalName != "field")
                    throw new Exception("Unbekanntes XML-Format. Erwartet: <XML_Field> oder <field>");
            }
            catch (Exception ex)
            {
                throw new Exception($"XML-Validierungsfehler: {ex.Message}");
            }
        }

        public static string FieldToString(GameField field, Robot robot)
        {
            var sb = new System.Text.StringBuilder();
            for (int y = 0; y < field.Height; y++)
            {
                for (int x = 0; x < field.Width; x++)
                {
                    if (x == robot.X && y == robot.Y)
                    {
                        sb.Append('R');
                    }
                    else
                    {
                        sb.Append(field.Field[y, x]);
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
