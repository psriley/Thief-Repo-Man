using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TImport = TilemapPipeline.TilemapContent;

namespace TilemapPipeline
{
    /// <summary>
    /// An importer for a basic tilemap file. The purpose of an importer to to load all important data 
    /// from a file into a content object; any processing of that data occurs in the subsequent content
    /// processor step. 
    /// </summary>
    [ContentImporter(".tmap", DisplayName = "TilemapImporter", DefaultProcessor = "TilemapProcessor")]
    public class TilemapImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            // Create a new TilemapContent
            TilemapContent map = new();

            // Read in the map file and split along newlines 
            string data = File.ReadAllText(filename);
            var lines = data.Split('\n');

            // First line in the map file is the image file name,
            // we store it so it can be loaded in the processor
            map.TilesetImageFilename = lines[0].Trim();

            // Second line is the tileset image size
            var secondLine = lines[1].Split(',');
            map.TileWidth = int.Parse(secondLine[0]);
            map.TileHeight = int.Parse(secondLine[1]);

            // Third line is the map size (in tiles)
            var thirdLine = lines[2].Split(',');
            map.MapWidth = int.Parse(thirdLine[0]);
            map.MapHeight = int.Parse(thirdLine[1]);

            // Fourth line is the map data (the indices of tiles in the map)
            // We can use the Linq Select() method to convert the array of strings
            // into an array of ints
            map.TileIndices = lines[3].Split(',').Select(index => int.Parse(index)).ToArray();

            // At this point, we've copied all of the file data into our
            // TilemapContent object, so we pass it on to the processor
            return map;
        }
    }
}
