using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserData
{
    class Timeline
    {
        double totalTime;
        public double currTime { get; set; }
        public Timeline(double totalTime)
        {
            this.totalTime = totalTime;
        }

        //void timer_Tick(object sender, EventArgs e)
        //{
        //    // Check if the movie finished calculate it's total time
        //    if (totalTime > 0)
        //    {
        //        // Updating time slider
        //        slPosition.Value = videoFox.Position.TotalSeconds /
        //                           TotalTime.TotalSeconds;
        //    }


        //}
    }
}
