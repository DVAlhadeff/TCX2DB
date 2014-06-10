namespace TCX2DB
{

    public class TCX2DB
    {
        static void Main(string[] args)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("M:\\TCX");
            System.IO.FileInfo[] fis = di.GetFiles("*.tcx");
            
            int currIndex = 1;
            foreach(System.IO.FileInfo file in fis) {
                System.Console.WriteLine(currIndex.ToString() + " of " + fis.Length + ": " +file.FullName);
                currIndex++;

                Activity a = GarminUtils.ConvertTCS(file.FullName);

                string[] parts = file.Name.Split(new char[] {'-'} );
                int y, m, d;
                int.TryParse(parts[0], out y);
                int.TryParse(parts[1], out m);
                int.TryParse(parts[2], out d);

                int h, min, s;
                int.TryParse(parts[3].Substring(0,2) , out h);
                int.TryParse(parts[3].Substring(2,2) , out min);
                int.TryParse(parts[3].Substring(4,2) , out s);

                System.DateTime dt = new System.DateTime(y, m, d, h, min, s);

                WorkoutsDataSet wds = new WorkoutsDataSet();
                WorkoutsDataSetTableAdapters.DataPointsTableAdapter dpta = new WorkoutsDataSetTableAdapters.DataPointsTableAdapter();

                foreach (Lap lap in a.Laps)
                {
                    foreach (Track t in lap.Tracks)
                    {
                        foreach (TrackPoint tp in t.TrackPoints)
                        {
                            if (tp.Positionx.Count == 0) continue;

                            dpta.Insert(a.Sport,
                                dt,
                                tp.Positionx[0].LatitudeDegrees,
                                tp.Positionx[0].LongitudeDegrees,
                                tp.DistanceMeters,
                                tp.AltitudeMeters,
                                tp.Timex,
                                tp.HeartRateBpm,
                                tp.Cadence,
                                tp.Speed,
                                0);
                        }
                    }
                }
            }
        }
    }
}