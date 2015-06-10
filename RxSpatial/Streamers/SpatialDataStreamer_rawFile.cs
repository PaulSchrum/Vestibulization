using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reactive.Linq;

namespace RxSpatial.Streamers
{
   public class SpatialDataStreamer_rawFile : SpatialDataStreamer_raw
   {
      internal List<AccelerometerFrame_raw> allFrames { get; set; }
      private Double ticksPerSecond_ { get; set; }

      internal SpatialDataStreamer_rawFile(String filename)
      {
         if(null == filename) throw new ArgumentNullException("filename");
         if(!File.Exists(filename)) throw new FileNotFoundException(filename);
         allFrames = new List<AccelerometerFrame_raw>();

         using(var file = new StreamReader(filename))
         {
            long count = 0;
            String line;
            while((line = file.ReadLine()) != null)
            {
               if(count == 0)
               {
                  count++;
                  ticksPerSecond_ = Double.Parse(line.Split(',').FirstOrDefault().Trim());
                  AccelerometerFrame_raw.OverrideTicksPerSecond(ticksPerSecond_);
                  continue;
               }
               var parsedLine = line.Split(',').Select(s => s.Trim()).ToArray();
               //Make a new frame and add it to the list
               var newFrame = new AccelerometerFrame_raw(parsedLine);
               allFrames.Add(newFrame);
            }
         }
         if (allFrames == null) throw new Exception("member allFrames unexpectedly null.");
         if (allFrames.Count < 2) throw new Exception("member allFrames unexpectedly empty.");
         //var observable = Observable.Generate(
         //   null,
         //   n => null == n,

         //   )
          //figure //out how to make this observable
      }
   }
}
