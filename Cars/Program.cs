using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
       static void Main(string[] args)
       {
            var cars = ProcessFile("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");

            //var query =
            //from manufacturer in manufacturers
            //join car in cars on manufacturer.Name equals car.Manufacturer
            //into carGroup
            //select new
            //{
            //    Manufacturer = manufacturer,
            //    Cars = carGroup
            //} into result
            //group result by result.Manufacturer.Headquarters;

            var query2 =
                cars.GroupBy(c => c.Manufacturer)
                .Select(g =>
               {
                   var results = g.Aggregate(new CarStatistics(),
                                       (acc, c) => acc.Accumulate(c),
                                       acc => acc.Compute());
                   return new
                   {
                       Name = g.Key,
                       Avg = results.Average,
                       Min = results.Min,
                       Max = results.Max
                   };
               })
                .OrderByDescending(r => r.Max);




            foreach (var result in query2)
            {
                Console.WriteLine($"{result.Name}");
                Console.WriteLine($"\t{result.Max}");
                Console.WriteLine($"\t{result.Min}");
            }

            //    foreach (var group in query)
            //{
            //    Console.WriteLine($"{group.Key}");
            //    foreach (var car in group.SelectMany(g=>g.Cars).OrderByDescending(c=>c.Combined).Take(3))
            //    {
            //        Console.WriteLine($"\t {car.Name} : {car.Combined}" );
            //    }

                //Console.WriteLine($"{group.Manufacturer.Name} : {group.Manufacturer.Headquarters}");
                //foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                //{
                //    Console.WriteLine($"\t {car.Name} : {car.Combined}");
                //}

                //Console.WriteLine($"{result.Key} has {result.Count()} cars");
            }


            //var query =
            //    from car in cars
            //    group car by car.Manufacturer.ToUpper() into manufacturer
            //    orderby manufacturer.Key
            //    select manufacturer;











            //var query2 = cars.Join(manufactures,
            //    c => c.Manufacturer,
            //    m => m.Name, (c, m) => new
            //    {
            //        m.Headquarters,
            //        c.Name,
            //        c.Combined
            //    })
            //.OrderByDescending(c => c.Combined)
            //.ThenBy(c => c.Name);

            //foreach (var car in query2.Take(10))
            //{
            //    Console.WriteLine($"{car.Headquarters} {car.Name} : {car.Combined}");

            //}



            //var result = cars.All(c => c.Manufacturer == "Ford") ;


            //var top = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
            //    .OrderByDescending(c => c.Combined)
            //    .ThenBy(c => c.Name)
            //    .First();


            //Console.WriteLine(top);

            //foreach (var car in query.Take(10))
            //      {
            //               Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");

            //      }
            // var query = cars.OrderByDescending(c => c.Combined)
            //.ThenBy(c => c.Name);

        

        public class CarStatistics
        {
            public CarStatistics()
            {
                Max = Int32.MinValue;
                Min = Int32.MaxValue;
            }

            public CarStatistics Accumulate(Car car)
            {
                
                Count += 1;
                Total += car.Combined;
                Max = Math.Max(Max, car.Combined);
                Min = Math.Min(Min, car.Combined);

                return this;
            }

            public CarStatistics Compute()
            {
                Average = Total / Count;
                return this;
            }

            public int Max { get; set; }
            public int Min { get; set; }
            public int Total { get; set; }
            public int Count { get; set; }
            public double Average { get; set; }
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path)
                .Where(l => l.Length > 1)
                .Select(l =>
                {
                    var columns = l.Split(',');
                    return new Manufacturer
                    {
                        Name = columns[0],
                        Headquarters = columns[1],
                        Year = int.Parse(columns[2])
                    };
                });
            return query.ToList();
        }

        private static List<Car> ProcessFile(string path)
        {
            var query =
                File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 1)
                .ToCar();

            return query.ToList();
            //return
            //    File.ReadAllLines(path)
            //      .Skip(1)
            //      .Where(line => line.Length > 1)
            //      .Select(Car.ParseFromCsv)
            //      .ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                 yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])

                };
            }
        }
    }
}
