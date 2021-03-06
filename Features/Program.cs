﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Collections;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee { Id = 1, Name = "Scott"},
                new Employee { Id = 2, Name = "Chris"}
            };
            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee {Id= 3, Name = "Alex"}
            };


            foreach (var employee in developers.Where( e=>e.Name.Length == 5)
                .OrderBy(e => e.Name))
            {
                Console.WriteLine(employee.Name);
            }

            //Console.WriteLine(developers.Count());
            //IEnumerator<Employee> enumerator = developers.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    Console.WriteLine(enumerator.Current.Name);
            //}


        }

        private static bool NameStartsWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
        }
    }
}
