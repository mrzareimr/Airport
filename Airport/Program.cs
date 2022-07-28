using System;
using System.Collections.Generic;
using System.Linq;

namespace Airport
{
    internal class Program
    {
        public 
        static void Main(string[] args)
        {

            string firstparms = Console.ReadLine();
            string[] FP = firstparms.Split(' ');
            int n = int.Parse(FP[0]);
            int k = int.Parse(FP[1]);
            List<Runway> Runways = new List<Runway>();
            for (int r = 1; r <= k; r++)
            {
                Runways.Add(new Runway(r, false));
            }
            int q = 0;

            List<airplane> Airplanes = new List<airplane>();
            bool Checker = true;

            while (Checker)
            {
                string param = Console.ReadLine();
                if (param.Length == 10)
                {
                    Airplanes.Add(new airplane(param, 1));
                }
                else
                {
                    q = int.Parse(param);
                    Checker = false;
                }
            }
            string[] Commands = new string[q];
            for (int i = 0; i < q; i++)
            {
                string command = Console.ReadLine();
                Commands[i] = command;
            }
            //-----------------------------------
            for (int j = 0; j < Commands.Length; j++)
            {
                string[] perform = Commands[j].Split(' ');
                string CommandID = perform[1];
                airplane InCommand = Airplanes.Where(p => p._ID == CommandID).FirstOrDefault();
                if(InCommand == null)
                {
                    InCommand = new airplane(CommandID, 4);
                    Airplanes.Add(InCommand);
                }
                //------------------------
                Dictionary<int,string> Command = new Dictionary<int,string>();
                Command.Add(1, "TAKE-OFF");
                Command.Add(3, "PLANE-STATUS");
                Command.Add(4, "BAND-STATUS");
                Command.Add(2, "LANDING");
                int RunCommand = Command.Where(p => p.Value == perform[0]).FirstOrDefault().Key;
                Dto result=new(InCommand,Runways);
                switch (RunCommand)
                {
                    case 1:
                       result= TAKE_OFF(new Dto(  InCommand, Runways));
                        break;
                    case 2:
                      result=  LANDING(new Dto( InCommand, Runways));
                        break;
                    case 3:
                        Console.WriteLine(InCommand._Status);
                        break;
                    case 4:
                        Runway RunwayInfo=Runways.Where(p=>p._ID.ToString()==CommandID).FirstOrDefault();
                        if (RunwayInfo._Status != false)
                        {
                            Console.WriteLine(RunwayInfo.airplane._ID);
                        }
                        else
                        {
                            Console.WriteLine("FREE");
                        }
                        break;

                }
                Runways = result.Runways;
                Airplanes.Where(item => item._ID == InCommand._ID).FirstOrDefault()._Status = result.Airplane._Status;
                   
            }

        }
     
        public static Dto TAKE_OFF(Dto dto)
        {
            List<Runway> EmptyRunway = dto.Runways.Where(p => p._Status == false).ToList();
            Runway minRunway = EmptyRunway.Where(p=>p._ID== EmptyRunway.Min(p => p._ID)).FirstOrDefault();
            switch (dto.Airplane._Status)
            {


                case 1:
                    if (EmptyRunway.Count == 0)
                    {
                        Console.WriteLine("NO FREE BOUND");
                    }
                    else
                    {
                        dto.Airplane._Status = 3;
                        dto.Runways.Where(item => item._ID == minRunway._ID).FirstOrDefault()._Status = true;
                        dto.Runways.Where(item => item._ID == minRunway._ID).FirstOrDefault().airplane = dto.Airplane;

                    }
                    break;
                case 2:
                    Console.WriteLine("YOU ARE TAKING OFF");
                    break;
                case 3:
                    Console.WriteLine("YOU ARE LANDING NOW");
                    break;
                case 4:
                    Console.WriteLine("YOU ARE NOT HERE");
                    break;
            }
            return dto;

        }
        public static Dto LANDING(Dto dto)
        {
            List<Runway> EmptyRunway = dto.Runways.Where(p => p._Status == false).ToList();
            Runway MaxRunway = EmptyRunway.Where(p => p._ID == EmptyRunway.Max(p => p._ID)).FirstOrDefault();
            switch (dto.Airplane._Status)
            {


                case 4:
                    if (EmptyRunway.Count == 0)
                    {
                        Console.WriteLine("NO FREE BOUND");
                        
                    }
                    else
                    {
                        
                        dto.Airplane._Status = 3;
                        dto.Runways.Where(item => item._ID == MaxRunway._ID).FirstOrDefault()._Status = true;
                        dto.Runways.Where(item => item._ID == MaxRunway._ID).FirstOrDefault().airplane = dto.Airplane;
                        
                    }
                    break;
              
                case 3:
                    Console.WriteLine("YOU ARE LANDING NOW");
                   
                    break;
                case 2:
                    Console.WriteLine("YOU ARE TAKING OFF");
                   
                    break;
                case 1:
                    Console.WriteLine("YOU ARE HERE");
                   
                    break;

            }
            return dto;
        }

    }
    public class airplane
    {
        public string _ID;
        public int _Status;
        public airplane(string ID, int Status)
        {
            _ID = ID;
            _Status = Status;
        }
    }
    public class Runway
    {
        public int _ID;
        public bool _Status;
       public airplane airplane;
        public Runway(int ID, bool Status)
        {
            _ID = ID;
            _Status = Status;
           
        }
    }
    public class Dto 
    {
        public airplane Airplane;
        public List< Runway> Runways;
        public Dto(airplane airplane, List<Runway> runways)
        {
            Airplane = airplane;
            Runways = runways;
        }
    }


}
